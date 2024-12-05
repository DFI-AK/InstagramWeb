using InstagramWeb.Application.Chat.Commands.SendMessage;
using InstagramWeb.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace InstagramWeb.Application.Common.Behaviours;
public class ChatLoggingBehaviour<TRequest, TResponse>(ILogger<ChatLoggingBehaviour<TRequest, TResponse>> logger, IUser user) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : SendMessageCommand
    where TResponse : notnull
{
    private readonly ILogger<ChatLoggingBehaviour<TRequest, TResponse>> _logger = logger;
    private readonly IUser _user = user;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        var response = await next();

        try
        {
            if (string.IsNullOrEmpty(_user.Id))
                throw new ArgumentNullException("User id is null or empty.");

            _logger.LogInformation("Request {RequestName} : Processing [{UserId}] request", requestName, _user.Id);

            if (response is null)
                throw new ArgumentNullException("Response from the request is null.");

            _logger.LogInformation("Completed {RequestName} successfully with {response}.", requestName, response);
        }
        catch (Exception ex)
        {
            _logger.LogError("Request {RequstName} : Completed with an error, Error : {error}", requestName, ex.Message);
        }
        return response;
    }
}
