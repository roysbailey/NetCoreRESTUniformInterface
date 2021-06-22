using System;
using System.Collections.Generic;
using System.Text;

namespace SampleDomain
{
    public class Apprentice
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public DateTime? PersonalDetailsConfirmedOn { get; set; }
    }
}