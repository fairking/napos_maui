using Napos.Core.Validation;
using Napos.Models.Base;

namespace Napos.Models
{
    public class CreateContactForm : BaseForm
    {
        [ValRequired]
        [ValLength(50)]
        public string Name { get; set; }
    }

    public class ContactForm : BaseIdForm
    {
        [ValRequired]
        [ValLength(50)]
        public string Name { get; set; }
    }

    public class ContactItem : BaseAuditedItem
    {
        public string Name { get; set; }
    }

    public class ContactSignatureForm : BaseIdForm
    {
        [ValRequired]
        public string Request { get; set; }

        public string Response { get; set; }
    }
}
