using System.Data.Entity;
using WebApplication.SharedDatabase.Model;

namespace WebApplication.SharedDatabase.DataModel
{
    public class UberContext : DbContext
    {
        public UberContext() : base("AttendanceLite")
        {
        }

        public DbSet<ApprovalProc> ApprovalProcs { get; set; }
        public DbSet<BreastFeedingDismissal> BreastFeedingDismissals { get; set; }
        public DbSet<CalendarDate> Holidays { get; set; }
        public DbSet<ChildBirthDismissal> ChildBirthDismissals { get; set; }
        public DbSet<DeathOfRelativesDismissal> DeathOfRelativesDismissals { get; set; }
        public DbSet<DemandedDismissal> DemandedDismissals { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Dismissal> Dismissals { get; set; }
        public DbSet<DismissalApproval> DismissalsApprovals { get; set; }
        public DbSet<Duty> Duties { get; set; }
        public DbSet<DutyApproval> DutiesApprovals { get; set; }
        public DbSet<EmployeementType> EmployeementTypes { get; set; }
        public DbSet<EncouragementDismissal> EncouragementDismissals { get; set; }
        public DbSet<GroupCategory> GroupCategories { get; set; }
        public DbSet<HourlyShift> HourlyShifts { get; set; }
        public DbSet<MarriageDismissal> MarriageDismissals { get; set; }
        public DbSet<Personnel> Personnel { get; set; }
        public DbSet<PersonnelApprovalProc> PersonnelApprovalProcs { get; set; }
        public DbSet<PersonnelDailyDismissal> PersonnelDailyDismissals { get; set; }
        public DbSet<PersonnelHourlyDismissal> PersonnelHourlyDismissals { get; set; }
        public DbSet<PersonnelDismissal> PersonnelDismissals { get; set; }
        public DbSet<PersonnelDailyDuty> PersonnelDailyDuties { get; set; }
        public DbSet<PersonnelHourlyDuty> PersonnelHourlyDuties { get; set; }
        public DbSet<PersonnelDuty> PersonnelDuties { get; set; }
        public DbSet<PersonnelEntrance> PersonnelEntrances { get; set; }
        public DbSet<PersonnelDismissalEntrance> PersonnelDismissalEntrances { get; set; }
        public DbSet<PersonnelDutyEntrance> PersonnelDutyEntrances { get; set; }
        public DbSet<PersonnelHourlyShift> PersonnelHourlyShifts { get; set; }
        public DbSet<PersonnelShiftAssignment> PersonnelShiftAssignments { get; set; }
        public DbSet<PersonnelShiftReplacement> PersonnelShiftReplacements { get; set; }
        public DbSet<PersonnelShift> PersonnelShifts { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<SicknessDismissal> SicknessDismissals { get; set; }
        public DbSet<WithoutSalaryDismissal> WithoutSalaryDismissals { get; set; }
        public DbSet<WorkingHour> WorkingHours { get; set; }
        public DbSet<WorkUnit> WorkUnits { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            #region table naming conventions
            modelBuilder.Entity<Personnel>().ToTable("Personnel");
            modelBuilder.Entity<DismissalApproval>().ToTable("DismissalsApprovals");
            modelBuilder.Entity<DutyApproval>().ToTable("DutiesApprovals");
            modelBuilder.Entity<PersonnelDismissal>().ToTable("PersonnelDismissals");
            //inheritance tpc
            modelBuilder.Entity<BreastFeedingDismissal>().ToTable("BreastFeedingDismissals");
            modelBuilder.Entity<ChildBirthDismissal>().ToTable("ChildBirthDismissals");
            modelBuilder.Entity<DeathOfRelativesDismissal>().ToTable("DeathOfRelativesDismissals");
            modelBuilder.Entity<DemandedDismissal>().ToTable("DemandedDismissals");
            modelBuilder.Entity<EncouragementDismissal>().ToTable("EncouragementDismissals");
            modelBuilder.Entity<MarriageDismissal>().ToTable("MarriageDismissals");
            modelBuilder.Entity<SicknessDismissal>().ToTable("SicknessDismissals");
            modelBuilder.Entity<WithoutSalaryDismissal>().ToTable("WithoutSalaryDismissals");
            #endregion

            #region configure relations

            #region PersonnelShiftReplacement
            modelBuilder.Entity<PersonnelShiftReplacement>()
                .HasRequired(m => m.Personnel)
                .WithMany(t => t.PersonnelShiftReplacements)
                .HasForeignKey(m => m.PersonnelId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PersonnelShiftReplacement>()
                .HasRequired(m => m.ReplacedPersonnel)
                .WithMany(t => t.ReplacedPersonnelShiftReplacements)
                .HasForeignKey(m => m.ReplacedPersonnelId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PersonnelShiftReplacement>()
                .HasRequired(m => m.WorkingHour)
                .WithMany(t => t.PersonnelShiftReplacements)
                .HasForeignKey(m => m.WorkingHourId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PersonnelShiftReplacement>()
                .HasRequired(m => m.ReplacedWorkingHour)
                .WithMany(t => t.ReplacedPersonnelShiftReplacements)
                .HasForeignKey(m => m.ReplacedWorkingHourId)
                .WillCascadeOnDelete(false);
            #endregion

            #region ApprovalProc
            modelBuilder.Entity<ApprovalProc>()
                .HasRequired(m => m.FirstPriority)
                .WithMany(t => t.FirstPriorities)
                .HasForeignKey(m => m.FirstPriorityId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApprovalProc>()
                .HasOptional(m => m.SecondPriority)
                .WithMany(t => t.SecondPriorities)
                .HasForeignKey(m => m.SecondPriorityId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApprovalProc>()
                .HasOptional(m => m.ThirdPriority)
                .WithMany(t => t.ThirdPriorities)
                .HasForeignKey(m => m.ThirdPriorityId)
                .WillCascadeOnDelete(false);
            #endregion

            #region PersonnelApprovalProc
            modelBuilder.Entity<PersonnelApprovalProc>()
                .HasRequired(m => m.Personnel)
                .WithRequiredDependent(t => t.PersonnelApprovalProc)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<PersonnelApprovalProc>()
                .HasOptional(m => m.DismissalApprovalProc)
                .WithMany(t => t.DismissalProcs)
                .HasForeignKey(m => m.DismissalApprovalProcId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PersonnelApprovalProc>()
                .HasOptional(m => m.DutyApprovalProc)
                .WithMany(t => t.DutyProcs)
                .HasForeignKey(m => m.DutyApprovalProcId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PersonnelApprovalProc>()
                .HasOptional(m => m.ShiftReplacementProc)
                .WithMany(t => t.ShiftReplacementProcs)
                .HasForeignKey(m => m.ShiftReplacementProcId)
                .WillCascadeOnDelete(false);
            #endregion

            #endregion

            base.OnModelCreating(modelBuilder);
        }
    }
}
