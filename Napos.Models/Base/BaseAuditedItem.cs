using System;
using System.Collections.Generic;
using System.Text;

namespace Napos.Models.Base
{
    public class BaseAuditedItem : BaseItem
    {
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
    }
}
