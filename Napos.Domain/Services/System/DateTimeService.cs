using Napos.Core.Abstract;
using System;

namespace Napos.Domain.Services.System
{
    public class DateTimeService : IDateTimeService, IService<IDateTimeService>
    {
        public DateTime NowUtc()
        {
            return DateTime.UtcNow;
        }

        public DateTime Today()
        {
            return DateTime.Today;
        }
    }
}
