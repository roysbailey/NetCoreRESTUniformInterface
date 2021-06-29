using SampleDomain;
using SampleDomain.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SampleDomain.Validation
{
    public class UniqueEmailAttribute : ValidationAttribute
    {
        public UniqueEmailAttribute()
        {
        }

        private string GetErrorMessage(string emailAddress) =>
            $"The email address {emailAddress} is not unique, please use a unique email.";

        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            var apprenticeCache = (IApprenticeCache)validationContext
                         .GetService(typeof(IApprenticeCache));
            var apprentice = (Apprentice)validationContext.ObjectInstance;
            if (apprenticeCache.Apprentices.Any(a => a.Id != apprentice.Id && a.Email.ToLower().Trim() == apprentice.Email.ToLower().Trim()))
            {
                return new ValidationResult(GetErrorMessage(apprentice.Email));
            }

            return ValidationResult.Success;
        }
    }
}
