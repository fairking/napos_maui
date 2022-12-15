using Napos.Core.Validation;
using Napos.Models.Base;

namespace Napos.Models
{
    public class CreateStoreForm : BaseForm
    {
        [ValRequired]
        [ValLength(25)]
        public string Name { get; set; }

        [ValLength(150)]
        public string Description { get; set; }
    }

    public class StoreForm : BaseIdForm
    {
        [ValRequired]
        [ValLength(25)]
        public string Name { get; set; }

        [ValLength(150)]
        public string Description { get; set; }
    }

    public class StoreItem : BaseAuditedItem
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
