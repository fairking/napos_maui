using Napos.Core.Attributes;
using Napos.Core.Exceptions;
using Napos.Core.Helpers;
using Napos.Data.Entities;
using Napos.Domain.Services.Base;
using Napos.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Napos.Domain.Services
{
    [Api]
    public class ContactService : BaseDataService
    {
        private SettingService _settings => GetService<SettingService>();

        public ContactService(IServiceProvider services) : base(services)
        {
        }

        #region Public

        [Api(true)]
        public async Task<string> Create(CreateContactForm form)
        {
            var contact = new Contact(form.Name)
            {
            };

            await Db.SaveOrUpdateAsync(contact);

            return contact.Id;
        }

        [Api(true)]
        public async Task Update(ContactForm form)
        {
            var contact = await Db.GetAsync<Contact>(form.Id);

            contact.SetName(form.Name);

            await Db.SaveOrUpdateAsync(contact);
        }

        [Api]
        public async Task<ContactForm> Get(IdForm form)
        {
            var contact = await Db.GetAsync<Contact>(form.Id);

            return new ContactForm()
            {
                Id = contact.Id,
                Name = contact.Name,
            };
        }

        [Api]
        public async Task<IList<ContactItem>> GetList()
        {
            Contact contact = null;

            var query = Db.Query(() => contact)
                .SelectAll(() => contact)
                ;

            var result = await Db.ToListNoProxyAsync<ContactItem>(query);

            return result;
        }

        [Api(true)]
        public async Task Delete(IdForm form)
        {
            await Db.RemoveAsync<Contact>(form.Id);
        }

        [Api]
        [Description("Prepare a form signature for the given contact")]
        public async Task<ContactSignatureForm> PrepareSignature(IdForm form)
        {
            var contact = await Db.GetAsync<Contact>(form.Id);

            var security = await _settings.GetSecurity();
            var request = EncryptHelper.CreateHash(security.PrivateKey);

            return new ContactSignatureForm()
            {
                Id = contact.Id,
                Request = request,
            };
        }

        [Api(true)]
        [Description("Apply signature for the given contact")]
        public async Task ApplySignature(ContactSignatureForm form)
        {
            var contact = await Db.GetAsync<Contact>(form.Id);

            if (contact.Signed)
                throw new UserException($"Contact already has a signature.");

            if (form.Response.IsNullOrEmpty())
                throw new UserException("Contact signature response is empty");

            contact.SetSignature(form.Response);

            await Db.SaveOrUpdateAsync(contact);
        }

        [Api]
        [Description("Validate signature for the given contact")]
        public async Task<bool> ValidateSignature(ContactSignatureForm form)
        {
            var contact = await Db.GetAsync<Contact>(form.Id);

            if (!contact.Signed)
                throw new UserException("Contact is not signed. Only signed contacts can be validated.");

            var result = !form.Response.IsNullOrEmpty() && contact.Signature == form.Response;

            return result;
        }

        [Api]
        [Description("Response to contact with own signature")]
        public async Task<ContactSignatureForm> ResponseSignature(ContactSignatureForm form)
        {
            var security = await _settings.GetSecurity();

            form.Response = EncryptHelper.CreateHash(form.Request, security.PrivateKey);

            return form;
        }

        #endregion Public

        #region Internal

        #endregion Internal
    }
}
