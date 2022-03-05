using ClinicProject.Server.Data.DBModels.AppointmentsTypes;
using ClinicProject.Server.Data.DBModels.AppUsers;
using ClinicProject.Server.Data.DBModels.NotesTypes;
using ClinicProject.Server.Data.DBModels.PatientTypes;
using ClinicProject.Server.Data.DBModels.PaymentTypes;
using ClinicProject.Server.Data.DBModels.TreatmentTypes;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ClinicProject.Server.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Patient

            builder.Entity<Patient>().
               HasMany(p => p.Appointments)
               .WithOne().IsRequired()
               .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Patient>().
               HasMany(p => p.Treatments)
               .WithOne().IsRequired()
               .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Patient>().
                 HasMany(p => p.Notes)
                 .WithOne().IsRequired()
                 .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Patient>().
             HasOne(p => p.ExtraData)
             .WithOne()
             .HasForeignKey<ExtraData>(e => e.PatientId)
             .IsRequired()
             .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region Treatments

            builder.Entity<Treatment>()
                .HasMany(t => t.Payments)
                .WithOne()
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            builder.ApplyConfigurationsFromAssembly(System.Reflection.Assembly.GetExecutingAssembly());
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Treatment> Treatments { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<ExtraData> ExtraDatas { get; set; }
    }
}