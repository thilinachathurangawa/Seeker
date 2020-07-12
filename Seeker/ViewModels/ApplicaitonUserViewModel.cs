using Seeker.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seeker.ViewModels
{
    public class ApplicaitonUserViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Zip { get; set; }
        public UserType UserType { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
		public bool IsProvider { get; set; }
	}
}
