using System;
using System.Collections.Generic;
using System.Text;

namespace Napos.Core.Abstract
{
    public interface IDateTimeService
    {
        DateTime Today();
        DateTime NowUtc();
    }
}
