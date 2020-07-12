using Seeker.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seeker.Models
{
	public class ApplicationUserModel
	{
		public string FirstName { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		public string LastName { get; set; }
        public string Address { get; set; }
        public UserType UserType { get; set; }
    }
}
