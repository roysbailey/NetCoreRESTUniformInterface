using SampleDomain.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SampleDomain
{
    public class Apprentice
    {
        public Apprentice()
        {

        }
        public Apprentice(Apprentice app)
        {
            Id = app.Id;
            FirstName = app.FirstName;
            LastName = app.LastName;
            DateOfBirth = app.DateOfBirth;
            Email = app.Email;
            PersonalDetailsConfirmedOn = app.PersonalDetailsConfirmedOn;
        }

        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [EmailAddress]
        [UniqueEmail]
        [Required]
        public string Email { get; set; }
        public DateTime? PersonalDetailsConfirmedOn { get; set; }
    }
}