using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TARge25Shop.Data.Migrations
{
    /// <inheritdoc />
    public partial class KindergartenFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF OBJECT_ID(N'[FileToDatabases]', N'U') IS NULL
                BEGIN
                    CREATE TABLE [FileToDatabases] (
                        [Id] uniqueidentifier NOT NULL,
                        [ImageTitle] nvarchar(max) NULL,
                        [ImageData] varbinary(max) NULL,
                        [RealEstateId] uniqueidentifier NULL,
                        [KindergartenId] uniqueidentifier NULL,
                        CONSTRAINT [PK_FileToDatabases] PRIMARY KEY ([Id])
                    );
                END
                ELSE
                BEGIN
                    IF COL_LENGTH('FileToDatabases', 'KindergartenId') IS NULL
                    BEGIN
                        ALTER TABLE [FileToDatabases] ADD [KindergartenId] uniqueidentifier NULL;
                    END
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileToDatabases");
        }
    }
}
