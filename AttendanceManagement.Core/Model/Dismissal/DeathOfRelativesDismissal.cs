﻿namespace AttendanceManagement.Core.Model
{
    public class DeathOfRelativesDismissal : Dismissal
    {
        public int DeathOfRelativesAllowanceInTotal { get; set; }
        public bool DeathOfRelativesIsAllowedToSubtractFromDemandedDismissalAfterLimit { get; set; }
    }
}
