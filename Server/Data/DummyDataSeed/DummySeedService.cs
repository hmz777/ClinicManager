using ClinicProject.Server.Data.DBModels.AppointmentsTypes;
using ClinicProject.Server.Data.DBModels.NotesTypes;
using ClinicProject.Server.Data.DBModels.PatientTypes;
using ClinicProject.Server.Data.DBModels.PaymentTypes;
using ClinicProject.Server.Data.DBModels.TreatmentTypes;
using ClinicProject.Shared.Models.Appointment;
using ClinicProject.Shared.Models.Patient;
using ClinicProject.Shared.Models.Payment;
using Microsoft.EntityFrameworkCore;

namespace ClinicProject.Server.Data.DummyDataSeed
{
    public class DummySeedService
    {
        private readonly ApplicationDbContext dbContext;

        public DummySeedService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Seed()
        {
            if (await dbContext.Patients.AnyAsync())
            {
                return;
            }

            for (int i = 0; i < 50; i++)
            {
                dbContext.Patients.Add(new Patient()
                {
                    Age = i,
                    CreationDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    FirstName = "Patient " + i,
                    MiddleName = "Patient Middle " + i,
                    LastName = "Patient Last " + i,
                    Gender = Gender.Male,
                    PhoneNumber = "00000000" + i,
                    Appointments = new List<Appointment>
                    {
                        new Appointment
                        {
                            AppointmentType = AppointmentType.Type1,
                            Date = DateTime.UtcNow,
                            CreationDate = DateTime.UtcNow,
                            UpdateDate= DateTime.UtcNow,
                        },
                         new Appointment
                        {
                            AppointmentType = AppointmentType.Type1,
                            Date = DateTime.UtcNow,
                            CreationDate = DateTime.UtcNow,
                            UpdateDate= DateTime.UtcNow,
                        },
                          new Appointment
                        {
                            AppointmentType = AppointmentType.Type1,
                            Date = DateTime.UtcNow,
                            CreationDate = DateTime.UtcNow,
                            UpdateDate= DateTime.UtcNow,
                        },
                    },
                    Notes = new List<Note>
                    {
                        new Note
                        {
                            Value = "asdasdasd" + i,
                            UpdateDate = DateTime.UtcNow,
                            CreationDate= DateTime.UtcNow,
                        },
                         new Note
                        {
                            Value = "asdasdasd" + i,
                            UpdateDate = DateTime.UtcNow,
                            CreationDate= DateTime.UtcNow,
                        },
                          new Note
                        {
                            Value = "asdasdasd" + i,
                            UpdateDate = DateTime.UtcNow,
                            CreationDate= DateTime.UtcNow,
                        },
                    },
                    Treatments = new List<Treatment>
                    {
                        new Treatment
                        {
                            CreationDate = DateTime.UtcNow,
                            UpdateDate = DateTime.UtcNow,
                            TreatmentType = "asda" + i,
                            PaymentStatus = PaymentStatus.Incomplete,
                            PaymentType = PaymentType.Single,
                            TotalCost = i,
                            Payments = new List<Payment>
                            {
                                new Payment
                                {
                                    CreationDate = DateTime.UtcNow,
                                    UpdateDate = DateTime.UtcNow,
                                    Value = 5000
                                }
                            }

                        }
                    },
                    ExtraData = new ExtraData
                    {
                        CreationDate = DateTime.UtcNow,
                        UpdateDate = DateTime.UtcNow,
                        Data = "asdasdasda" + i
                    }
                });
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
