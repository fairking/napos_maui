using FluentMigrator;
using System;

namespace Napos.Data.Migrations
{
    [Migration(20220611_1500)]
    public class MIG_20220611_1500_Seed : Migration
    {
        public override void Up()
        {
            var privateKey = QueryMan.Helpers.RandomStringGenerator.RandomString(16, includeCapitals: true);

            // Private Key
            Insert.IntoTable("settings").Row(new
            {
                id = QueryMan.Helpers.RandomStringGenerator.WebHash(),
                key = "PrivateKey",
                value = privateKey,
            });
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}
