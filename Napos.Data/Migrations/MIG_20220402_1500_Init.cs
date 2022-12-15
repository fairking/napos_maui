using FluentMigrator;
using System;

namespace Napos.Data.Migrations
{
    [Migration(20220402_1500)]
    public class MIG_20220402_1500_Init : Migration
    {
        public override void Up()
        {
            Create.Table("stores")
                .WithColumn("id").AsAnsiString(16).NotNullable().PrimaryKey("pk_stores_id")
                .WithColumn("created").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("updated").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)

                .WithColumn("name").AsString(25).NotNullable()
                .WithColumn("description").AsString(150).Nullable()
                ;

            Create.Table("settings")
                .WithColumn("id").AsAnsiString(16).NotNullable().PrimaryKey("pk_settings_id")
                .WithColumn("created").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("updated").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)

                .WithColumn("key").AsString(25).NotNullable().Unique("uq_settings_key")
                .WithColumn("value").AsString(1000).Nullable()
                ;

            Create.Table("contacts")
                .WithColumn("id").AsAnsiString(16).NotNullable().PrimaryKey("pk_contacts_id")
                .WithColumn("created").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("updated").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)

                .WithColumn("name").AsString(50).NotNullable()
                .WithColumn("signed").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("signature").AsString(150).Nullable()
                ;
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}
