using InstagramWeb.Application.Chat.Commands.SendMessage;
using InstagramWeb.Application.Common.Interfaces;
using InstagramWeb.Application.Common.Models;
using Microsoft.Extensions.Logging;
using Serilog.Context;

namespace InstagramWeb.Application.Common.Behaviours;
public class SendMessageLoggingBeihaviour<TRequest, TResponse>(ILogger<SendMessageLoggingBeihaviour<TRequest, TResponse>> logger, IIdentityService identityService, IUser user) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : SendMessageCommand
    where TResponse : Result
{
    private readonly ILogger<SendMessageLoggingBeihaviour<TRequest, TResponse>> _logger = logger;

    private readonly IUser _user = user;
    private readonly IIdentityService _identityService = identityService;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        _logger.LogInformation("Handling {RequestName} with content: {@Request}", requestName, request);
        try
        {

            if (string.IsNullOrEmpty(_user.Id))
                throw new ArgumentNullException("User id is null or empty.");

            _logger.LogInformation("Request {RequestName} : Processing [{UserId}] request", requestName, _user.Id);

            var response = await next();

            if (response is null)
                throw new ArgumentNullException("Response from the request is null.");
            if (response.Succeeded)
            {
                _logger.LogInformation("Completed {RequestName} successfully with {response}.", requestName, response);
            }
            else
            {
                using (LogContext.PushProperty("Error", response.Errors))
                {
                    foreach (var error in response.Errors)
                    {
                        _logger.LogError("Request {RequestName} completed with error, Message : {Message}", requestName, error);
                    }
                }

            }
            return response;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Request {RequestName} failed.", requestName);
        }
        return await next();
    }
}
