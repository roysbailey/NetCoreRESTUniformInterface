using SampleDomain.Infrastructure.Services;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SampleDomain
{
    public class Apprenticeship : IValidatableObject
    {
        public Apprenticeship()
        {

        }

        public Apprenticeship(Apprenticeship app)
        {
            Id = app.Id;
            ApprenticeId = app.ApprenticeId;
            StandardName = app.StandardName;
            Level = app.Level;
            StartDate = app.StartDate;
            EndDate = app.EndDate;
            Employer = app.Employer;
            Provider = app.Provider;
            ConfirmedSection1 = app.ConfirmedSection1;
            ConfirmedSection2 = app.ConfirmedSection2;
            ConfirmedSection3 = app.ConfirmedSection3;
            ApprenticeshipConfirmedOn = app.ApprenticeshipConfirmedOn;
        }

        public int Id { get; set; }
        public int ApprenticeId { get; set; }
        public string StandardName { get; set; }
        public int Level { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Employer { get; set; }
        public string Provider { get; set; }
        public bool ConfirmedSection1 { get; set; }
        public bool ConfirmedSection2 { get; set; }
        public bool ConfirmedSection3 { get; set; }
        public DateTime? ApprenticeshipConfirmedOn { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var apprenticeshipCache = (IApprenticeshipsCache)validationContext
                         .GetService(typeof(IApprenticeshipsCache));

            if (apprenticeshipCache.Apprenticeships
                .Any(a => 
                    a.Id != Id 
                    && StartDate > a.StartDate && StartDate < a.EndDate
                ))
            {
                yield return new ValidationResult(
                    $"The start date overlaps another apprenticeship for this apprentice.",
                    new[] { nameof(StartDate) });
            }

            if (apprenticeshipCache.Apprenticeships
                .Any(a =>
                    a.Id != Id
                    && EndDate > a.StartDate && EndDate < a.EndDate
                ))
            {
                yield return new ValidationResult(
                    $"The end date overlaps another apprenticeship for this apprentice.",
                    new[] { nameof(EndDate) });
            }
        }
    }
}
