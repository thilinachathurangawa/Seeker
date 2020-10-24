using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Seeker.Constants
{
    public class Enums
    {
    }

    public enum UserType
    {
        [Description("Service Provider")]
        ServiceProvider = 1,
        [Description("Service Provider")]
        Client = 2
    }

	public enum AttachmentFileTypes
	{
		Images = 1,
		Videos = 2,
		Other = 3
	}

	public enum ResponseCodes
	{
		LoginSuccess = 1,
		LoginFailed = 2,
		RegistrationSuccess = 3,
		RegistrationFailed = 4,
		FileUploadFailed = 5,
		FileUploadSuccess = 6,
		AllSuccess = 7,
		AllFail = 8,
		InvalidUser = 9,
	}
	public enum JobworkflowStatus
	{
		PostedJob = 1,
		BidRecivedOrApproveWating = 2,
		BidAccepted = 3,
		InProgress = 4,
		Completed = 5,
		Rejected = 6
	}
	public enum JobAvailabiltyStatus
	{
		Hourly = 0,
		PartTime = 1,
		FullTime = 2
	}
}
