using SampleDomain;
using SampleDomain.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreRESTUniformInterface.Models.Validation
{
    public class UniqueApprenticeEmailAttribute : ValidationAttribute
    {
        IApprenticeCache _apprenticeCache;

        public UniqueApprenticeEmailAttribute(IApprenticeCache apprenticeCache)
        {
            _apprenticeCache = apprenticeCache;
        }

        private string GetErrorMessage(string emailAddress) =>
            $"The email address {emailAddress} is already in use on another account, please use a unique email for this account.";

        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            var apprentice = (Apprentice)validationContext.ObjectInstance;
            if (_apprenticeCache.Apprentices.Any(a => a.Id != apprentice.Id && a.Email.ToLower().Trim() == apprentice.Email.ToLower().Trim()))
            {
                return new ValidationResult(GetErrorMessage(apprentice.Email));
            }

            return ValidationResult.Success;
        }
    }
}
