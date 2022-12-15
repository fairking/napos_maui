using Napos.Data.Entities;
using Napos.Domain.Services;
using Napos.Domain.Tests.Base;
using Napos.Domain.Tests.Fixtures;
using Napos.Models;
using System.Threading.Tasks;
using Xunit;

namespace Napos.Domain.Tests.AllTests
{
    [TestCaseOrderer("Napos.Domain.Tests.Helpers.AlphabeticalTestOrderer", "Napos.Domain.Tests")]
    public class StoreTests : BaseTest
    {
        const string Store1 = "Store1";

        private StoreService _storeService;

        public StoreTests(DataFixture dataFixture, BagFixture bagFixture) : base(dataFixture, bagFixture)
        {
            _storeService = GetService<StoreService>();
        }

        [Fact]
        public async Task T01_Create()
        {
            var id = await Db.Execute(async () =>
            {
                return await _storeService.Create(new CreateStoreForm()
                {
                    Name = "Store 1",
                    Description = "Some desc 1"
                });
            });

            Bag.SetValue(Store1, id);

            var store = await _storeService.Get(new IdForm(id));

            Assert.NotNull(store);
            Assert.Equal("Store 1", store.Name);
            Assert.Equal("Some desc 1", store.Description);
        }

        [Fact]
        public async Task T02_Update()
        {
            var id = Bag.GetValue<string>(Store1);

            var store = await _storeService.Get(new IdForm(id));

            store.Name = "Store 1a";
            store.Description = "Some desc 1a";

            await Db.Execute(async () =>
            {
                await _storeService.Update(store);
            });

            var stores = await _storeService.GetList();

            Assert.Equal(1, stores.Count);
            Assert.Equal("Store 1a", stores[0].Name);
            Assert.Equal("Some desc 1a", store.Description);
        }

        [Fact]
        public async Task T03_Delete()
        {
            var id = Bag.GetValue<string>(Store1);

            await Db.Execute(async () => 
            {
                await Db.RemoveAsync<Store>(id);
            });

            var stores = await _storeService.GetList();

            Assert.Equal(0, stores.Count);
        }
    }
}
