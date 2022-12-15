using Napos.Core.Helpers;
using QueryMan.Attributes;
using System;

namespace Napos.Data.Entities
{
    [Table("contacts")]
    public class Contact : BaseAuditedEntity
    {
        protected Contact() { }

        public Contact(string name) : this()
        {
            SetName(name);
        }

        public virtual string Name { get; protected set; }

        public virtual bool Signed { get; protected set; }

        public virtual string Signature { get; protected set; }

        public void SetSignature(string signature)
        {
            Signature = signature;
            Signed = string.IsNullOrEmpty(signature);
        }

        public void SetName(string name)
        {
            if (name.IsNullOrEmpty())
                throw new ArgumentNullException(nameof(name));

            Name = name;
        }
    }
}
