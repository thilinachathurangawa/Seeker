using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seeker.Models
{
	public class AuthenticationContext : IdentityDbContext<ApplicationUser>
	{
		public AuthenticationContext(DbContextOptions options) : base(options)
		{			
		}
		public DbSet<Job> Jobs { get; set; }
		public DbSet<Attachment> Attachments { get; set; }
		//public DbSet<ApplicationUser> ApplicationUser { get; set; }
	}
}
