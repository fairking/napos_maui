using QueryMan.Attributes;
using System;

namespace Napos.Data.Entities
{
    [Table("stores")]
    public class Store : BaseAuditedEntity
    {
        protected Store() : base() { }

        public Store(string name) : this()
        {
            SetName(name);
        }

        #region Properties

        public virtual string Name { get; protected set; }

        public virtual string Description { get; set; }

        #endregion Properties

        #region Methods

        public virtual void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            Name = name;
        }

        #endregion Methods
    }
}
