using CMS.Core.Model;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using WebApplication.SharedDatabase;

namespace CMS.Data.Identity
{

    [DbConfigurationType(typeof(CustomDbConfig))]
    public class AuthContext : IdentityDbContext<User, IdentityRole, string, IdentityUserLogin
        , IdentityUserRole, IdentityUserClaim>
    {
        public AuthContext() : base("AttendanceLite")
        {
            Database.SetInitializer(new AuthContextInitializer());
        }

        #region tables with security schema
        public DbSet<AccessPathCategory> AccessPathCategories { get; set; }
        public DbSet<AccessPath> AccessPaths { get; set; }
        public DbSet<RoleAccessPath> RolesAccessPaths { get; set; }
        public DbSet<UserLog> UserLogs { get; set; }
        public DbSet<RestrictedIP> RestrictedIPs { get; set; }
        public DbSet<RestrictedAccessTime> RestrictedAccessTimes { get; set; }
        #endregion
        public DbSet<Message> Messages { get; set; }
        public DbSet<RequestMessage> RequestMessage { get; set; }

        public static AuthContext Create()
        {
            return new AuthContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region table naming conventions
            modelBuilder.Entity<User>().ToTable("Users", "security");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles", "security");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UsersInRoles", "security");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaims", "security");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogins", "security");
            modelBuilder.Entity<RoleAccessPath>().ToTable("RolesAccessPaths", "security");
            modelBuilder.Entity<AccessPath>().ToTable("AccessPaths", "security");
            modelBuilder.Entity<AccessPathCategory>().ToTable("AccessPathCategories", "security");
            modelBuilder.Entity<UserLog>().ToTable("UserLogs", "security");
            modelBuilder.Entity<RestrictedIP>().ToTable("RestrictedIPs", "security");
            modelBuilder.Entity<RestrictedAccessTime>().ToTable("RestrictedAccessTimes", "security");
            #endregion

            #region configure relations

            #region Message
            modelBuilder.Entity<Message>()
                .HasRequired(m => m.Sender)
                .WithMany(t => t.Outbox)
                .HasForeignKey(m => m.SenderId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Message>()
                .HasRequired(m => m.Receiver)
                .WithMany(t => t.Inbox)
                .HasForeignKey(m => m.ReceiverId)
                .WillCascadeOnDelete(false);
            #endregion

            #endregion
        }
    }

    public class AuthContextInitializer : DropCreateDatabaseIfModelChanges<AuthContext>
    {
        protected override void Seed(AuthContext context)
        {
            Identity.Seed.Init(context);

            base.Seed(context);
        }
    }
}