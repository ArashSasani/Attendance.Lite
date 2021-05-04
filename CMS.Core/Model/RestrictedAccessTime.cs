using System;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace CMS.Core.Model
{
    public class RestrictedAccessTime : IEntity
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan FromTime { get; set; }
        public TimeSpan ToTime { get; set; }
        public DeleteState DeleteState { get; set; }
    }
}
