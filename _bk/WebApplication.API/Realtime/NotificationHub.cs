using Microsoft.AspNet.SignalR;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.API.Realtime
{
    [Authorize]
    public class NotificationHub : Hub
    {
        public override Task OnConnected()
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                string name = Context.User.Identity.Name.ToLower();

                ConnectionManager.Instance.Connections.Add(name, Context.ConnectionId);
            }

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string name = Context.User.Identity.Name.ToLower();

            ConnectionManager.Instance.Connections.Remove(name, Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            string name = Context.User.Identity.Name.ToLower();

            if (!ConnectionManager.Instance.Connections.GetConnections(name)
                .Contains(Context.ConnectionId))
            {
                ConnectionManager.Instance.Connections.Add(name, Context.ConnectionId);
            }

            return base.OnReconnected();
        }
    }
}