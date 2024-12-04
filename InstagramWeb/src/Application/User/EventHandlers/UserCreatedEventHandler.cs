using InstagramWeb.Application.Common.Interfaces;
using InstagramWeb.Domain.Constants;
using InstagramWeb.Domain.Events;
using Microsoft.Extensions.Logging;

namespace InstagramWeb.Application.User.EventHandlers;
public class UserCreatedEventHandler(IApplicationDbContext context, IIdentityService identityService, ILogger<UserCreatedEventHandler> logger) : INotificationHandler<UserCreatedEvent>
{
    private readonly ILogger<UserCreatedEventHandler> _logger = logger;
    private readonly IApplicationDbContext _context = context;
    private readonly IIdentityService _identityService = identityService;
    public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        using (_logger.BeginScope(nameof(UserCreatedEventHandler)))
        {
            await _context.UserProfiles.AddAsync(new()
            {
                Id = notification.UserId,
                FirstName = notification.FirstName,
                Email = notification.UserName
            }, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            if (!await _identityService.IsInRoleAsync(notification.UserId, Roles.User))
            {
                await _identityService.AddToRoleAsync(notification.UserId, Roles.User);
            }
        }
    }
}
