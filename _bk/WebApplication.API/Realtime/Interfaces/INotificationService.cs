using System.Threading.Tasks;

namespace WebApplication.API.Realtime.Interfaces
{
    public interface INotificationService
    {
        Task NotifyUpdates(string userId);
    }
}
