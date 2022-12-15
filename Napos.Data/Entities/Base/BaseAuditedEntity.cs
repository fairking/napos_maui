using System;
using System.Collections.Generic;
using System.Text;

namespace Napos.Data.Entities
{
    public abstract class BaseAuditedEntity : BaseEntity
    {
        protected BaseAuditedEntity() : base() { }

        public virtual DateTime Created { get; protected set; }

        public virtual DateTime Updated { get; protected set; }
    }
}
