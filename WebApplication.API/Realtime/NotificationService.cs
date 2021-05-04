using CMS.Service.Interfaces;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using WebApplication.API.Realtime.Interfaces;

namespace WebApplication.API.Realtime
{
    public class NotificationService : INotificationService
    {
        private readonly IMessageService _messageService;
        private readonly IAuthService _authService;

        public NotificationService(IMessageService messageService, IAuthService authService)
        {
            _messageService = messageService;
            _authService = authService;
        }

        public async Task NotifyUpdates(string userId)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            if (hubContext != null)
            {
                var notifications = await _messageService.GetNotifications(userId);
                if (notifications.Count > 0)
                {
                    var user = await _authService.FindUserAsync(userId);

                    foreach (var connectionId in ConnectionManager.Instance.Connections
                        .GetConnections(user.Username.ToLower()))
                    {
                        hubContext.Clients.Client(connectionId).pushNotifications(notifications);
                    }
                }
            }
        }
    }
}