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


}
