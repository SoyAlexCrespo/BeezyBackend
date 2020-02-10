using System;
using System.Collections.Generic;
using System.Text;

namespace BeezyBackend.Domain.Services
{
    public interface IUtc
    {
        public DateTime Now();
    }
}
