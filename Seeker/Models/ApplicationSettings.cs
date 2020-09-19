using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seeker.Models
{
    public class ApplicationSettings
    {
        public string JWT_Secret { get; set; }
        public string Client_URL { get; set; }
		public string AzureBlobStorageConnectionString { get; set; }
		public string ImageContainer { get; set; }
		public string VideoContainer { get; set; }
		public string GeneralContainer { get; set; }
	}
}
