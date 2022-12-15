using QueryMan.Attributes;
using QueryMan.IdentityGenerator;

namespace Napos.Data.Entities
{
    public abstract class BaseEntity
    {
        protected BaseEntity() { }

        [Key(typeof(KeyVarChar16Generator))]
        public virtual string Id { get; protected set; }
    }
}
