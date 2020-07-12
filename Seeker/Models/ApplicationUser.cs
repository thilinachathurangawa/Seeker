using Microsoft.AspNetCore.Identity;
using Seeker.Constants;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
////using System.ComponentModel.DataAnnotations.Schema;

namespace Seeker.Models
{
	public class ApplicationUser : IdentityUser
	{
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public int Zip { get; set; }

        public UserType UserType { get; set; }

    }
}
