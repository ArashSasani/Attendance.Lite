using WebApplication.SharedKernel.Enums;

namespace WebApplication.SharedKernel.Interfaces
{
    public interface IEntity
    {
        int Id { get; set; }
        DeleteState DeleteState { get; set; }
    }
}
