using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seeker.Models
{
	public class AuditableBaseModel: BaseModel
	{
		public Guid? CountryId { get; set; }

		public DateTime CreatedDateTime { get; set; }

		public DateTime LastUpdatedDateTime { get; set; }

		public string CreatedBy { get; set; }

		public string LastUpdatedBy { get; set; }
		
	}
}
