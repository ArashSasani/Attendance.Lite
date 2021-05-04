using System;
using WebApplication.SharedKernel.Enums;
using WebApplication.SharedKernel.Interfaces;

namespace AttendanceManagement.Core.Model
{
    public class PersonnelDismissal : IEntity
    {
        public int Id { get; set; }
        public int PersonnelId { get; set; }
        public int DismissalId { get; set; }
        public DateTime SubmittedDate { get; set; }
        public string RequestDescription { get; set; }
        public string FileUploadPath { get; set; }
        public RequestDuration DismissalDuration { get; set; }

        #region confirmation or rejection
        public DateTime? ActionDate { get; set; }
        public RequestAction RequestAction { get; set; }
        public string ActionDescription { get; set; }
        #endregion

        public DeleteState DeleteState { get; set; }

        public Personnel Personnel { get; set; }
        public Dismissal Dismissal { get; set; }
    }
}
