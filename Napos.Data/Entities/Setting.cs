using QueryMan.Attributes;
using System;

namespace Napos.Data.Entities
{
    [Table("settings")]
    public class Setting : BaseAuditedEntity
    {
        protected Setting() : base()
        {

        }

        public Setting(string key, string value) : this()
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            Key = key;

            SetValue(value);
        }

        #region Properties

        public virtual string Key { get; protected set; }

        public virtual string Value { get; protected set; }

        #endregion Properties

        #region Methods

        public void SetValue(string value)
        {
            Value = value;
        }

        #endregion Methods
    }
}
