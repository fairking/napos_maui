using System;
using System.Linq;
using System.Threading.Tasks;
using Napos.Core.Attributes;
using Napos.Core.Helpers;
using Napos.Data.Entities;
using Napos.Domain.Services.Base;
using Napos.Models;

namespace Napos.Domain.Services
{
    [Api]
    public class SettingService : BaseDataService
    {
        public SettingService(IServiceProvider services) : base(services)
        {

        }

        #region Public Methods

        [Api]
        public async Task<SettingsForm> Get()
        {
            Setting setting = null;

            var query = Db.SelectAll(() => setting);

            var settings = await Db.ToListNoProxyAsync<Setting>(query);

            var result = new SettingsForm()
            {
                Theme = settings.SingleOrDefault(x => x.Key == nameof(SettingsForm.Theme))?.Value.ToBoolean(),
            };

            return result;
        }

        [Api(true)]
        public async Task Save(SettingsForm model)
        {
            Setting setting = null;

            var query = Db.SelectAll(() => setting);

            var settings = await Db.ToListAsync<Setting>(query);

            // Theme
            var theme = settings.SingleOrDefault(x => x.Key == nameof(SettingsForm.Theme));
            if (theme != null)
            {
                theme.SetValue(model.Theme?.ToString());
            }
            else
            {
                settings.Add(new Setting(nameof(SettingsForm.Theme), model.Theme?.ToString()));
            }

            await Db.SaveOrUpdateBatchAsync(settings);
        }

        #endregion Public Methods

        #region Internal Methods

        public async Task<SecuritySettingsForm> GetSecurity()
        {
            Setting setting = null;

            var query = Db.SelectAll(() => setting);

            var settings = await Db.ToListNoProxyAsync<Setting>(query);

            var result = new SecuritySettingsForm()
            {
                PrivateKey = settings.SingleOrDefault(x => x.Key == nameof(SecuritySettingsForm.PrivateKey))?.Value
                    ?? throw new ArgumentException("Private key not found."),
            };

            return result;
        }

        #endregion Internal Methods

    }
}
