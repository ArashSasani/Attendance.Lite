using WebApplication.SharedKernel.Enums;

namespace WebApplication.SharedKernel.Interfaces
{
    public interface IActiveEntity : IEntity
    {
        ActiveState ActiveState { get; set; }
    }
}
