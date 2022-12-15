using Napos.Core.Attributes;
using Napos.Data.Entities;
using Napos.Domain.Services.Base;
using Napos.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Napos.Domain.Services
{
    [Api]
    public class StoreService : BaseDataService
    {
        public StoreService(IServiceProvider services) : base(services)
        {

        }

        [Api(true)]
        public async Task<string> Create(CreateStoreForm form)
        {
            var store = new Store(form.Name)
            {
                Description = form.Description,
            };

            await Db.SaveOrUpdateAsync(store);

            return store.Id;
        }

        [Api(true)]
        public async Task Update(StoreForm form)
        {
            var store = await Db.GetAsync<Store>(form.Id);

            store.SetName(form.Name);
            store.Description = form.Description;

            await Db.SaveOrUpdateAsync(store);
        }

        [Api]
        public async Task<StoreForm> Get(IdForm form)
        {
            var store = await Db.GetAsync<Store>(form.Id);

            return new StoreForm()
            {
                Id = store.Id,
                Name = store.Name,
                Description = store.Description,
            };
        }

        [Api]
        public async Task<IList<StoreItem>> GetList()
        {
            Store store = null;

            var query = Db.Query(() => store)
                .SelectAll(() => store)
                ;

            var result = await Db.ToListNoProxyAsync<StoreItem>(query);

            return result;
        }

        [Api(true)]
        public async Task Delete(IdForm form)
        {
            await Db.RemoveAsync<Store>(form.Id);
        }
    }
}
