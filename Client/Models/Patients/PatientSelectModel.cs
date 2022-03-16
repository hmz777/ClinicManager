﻿namespace ClinicProject.Client.Models.Patients
{
    public class PatientSelectModel
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get { return FirstName + " " + LastName; } }
    }
}