using BeezyBackend.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeezyBackend.API.Services
{
    public class Utc : IUtc
    {
        public DateTime Now() => DateTime.UtcNow;
    }
}
