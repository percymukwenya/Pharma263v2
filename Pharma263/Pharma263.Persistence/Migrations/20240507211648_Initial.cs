using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Pharma263.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Pharma263");

            migrationBuilder.CreateTable(
                name: "AccountsPayableStatus",
                schema: "Pharma263",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, defaultValue: "'System'"),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TimeStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountsPayableStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AccountsReceivableStatus",
                schema: "Pharma263",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, defaultValue: "'System'"),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TimeStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountsReceivableStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                schema: "Pharma263",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                schema: "Pharma263",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    FirstName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    LastName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    UserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    SecurityStamp = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    PhoneNumber = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditEntry",
                schema: "Pharma263",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    ActionType = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    Username = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    TimeStamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    EntityId = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    Changes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditEntry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerType",
                schema: "Pharma263",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, defaultValue: "'System'"),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TimeStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Medicine",
                schema: "Pharma263",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    GenericName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    Brand = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    Manufacturer = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    DosageForm = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    PackSize = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    QuantityPerUnit = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, defaultValue: "'System'"),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TimeStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicine", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethod",
                schema: "Pharma263",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, defaultValue: "'System'"),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TimeStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethod", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseStatus",
                schema: "Pharma263",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, defaultValue: "'System'"),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TimeStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QuoteStatus",
                schema: "Pharma263",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    TimeStamp = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuoteStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReturnDestination",
                schema: "Pharma263",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, defaultValue: "'System'"),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TimeStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnDestination", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReturnReason",
                schema: "Pharma263",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, defaultValue: "'System'"),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TimeStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnReason", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReturnStatus",
                schema: "Pharma263",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, defaultValue: "'System'"),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TimeStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SaleStatus",
                schema: "Pharma263",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, defaultValue: "'System'"),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TimeStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StoreSetting",
                schema: "Pharma263",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Logo = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    StoreName = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false),
                    Currency = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    MCAZLicence = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    VATNumber = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    BankingDetails = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, defaultValue: "'System'"),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TimeStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreSetting", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Supplier",
                schema: "Pharma263",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Notes = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    MCAZLicence = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    BusinessPartnerNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    VATNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, defaultValue: "'System'"),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TimeStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supplier", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                schema: "Pharma263",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    ClaimType = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    ClaimValue = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Pharma263",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                schema: "Pharma263",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    ClaimType = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    ClaimValue = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "Pharma263",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                schema: "Pharma263",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    ProviderKey = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    UserId = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "Pharma263",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                schema: "Pharma263",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    RoleId = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Pharma263",
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "Pharma263",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                schema: "Pharma263",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    LoginProvider = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Value = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalSchema: "Pharma263",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customer",
                schema: "Pharma263",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    Phone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    PhysicalAddress = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    DeliveryAddress = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    MCAZLicence = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    HPALicense = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    VATNumber = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    CustomerTypeId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, defaultValue: "'System'"),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TimeStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customer_CustomerType_CustomerTypeId",
                        column: x => x.CustomerTypeId,
                        principalSchema: "Pharma263",
                        principalTable: "CustomerType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Quarantine",
                schema: "Pharma263",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalQuantity = table.Column<int>(type: "int", nullable: false),
                    BatchNo = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    ExpiryDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    MedicineId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, defaultValue: "'System'"),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TimeStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quarantine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quarantine_Medicine_MedicineId",
                        column: x => x.MedicineId,
                        principalSchema: "Pharma263",
                        principalTable: "Medicine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Stock",
                schema: "Pharma263",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalQuantity = table.Column<int>(type: "int", nullable: false),
                    NotifyForQuantityBelow = table.Column<int>(type: "int", nullable: false),
                    BatchNo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    ExpiryDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    BuyingPrice = table.Column<decimal>(type: "DECIMAL(14,2)", nullable: false),
                    SellingPrice = table.Column<decimal>(type: "DECIMAL(14,2)", nullable: false),
                    MedicineId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, defaultValue: "'System'"),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TimeStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stock_Medicine_MedicineId",
                        column: x => x.MedicineId,
                        principalSchema: "Pharma263",
                        principalTable: "Medicine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Purchase",
                schema: "Pharma263",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    Total = table.Column<decimal>(type: "DECIMAL(14,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "DECIMAL(14,2)", nullable: false),
                    GrandTotal = table.Column<decimal>(type: "DECIMAL(14,2)", nullable: false),
                    PaymentDueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentMethodId = table.Column<int>(type: "int", nullable: false),
                    PurchaseStatusId = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, defaultValue: "'System'"),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TimeStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchase", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Purchase_PaymentMethod_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalSchema: "Pharma263",
                        principalTable: "PaymentMethod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Purchase_PurchaseStatus_PurchaseStatusId",
                        column: x => x.PurchaseStatusId,
                        principalSchema: "Pharma263",
                        principalTable: "PurchaseStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Purchase_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "Pharma263",
                        principalTable: "Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Quotation",
                schema: "Pharma263",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuotationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    Total = table.Column<decimal>(type: "DECIMAL(14,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "DECIMAL(14,2)", nullable: false),
                    GrandTotal = table.Column<decimal>(type: "DECIMAL(14,2)", nullable: false),
                    QuoteExpiryDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    QuoteStatusId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, defaultValue: "'System'"),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TimeStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quotation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quotation_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "Pharma263",
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Quotation_QuoteStatus_QuoteStatusId",
                        column: x => x.QuoteStatusId,
                        principalSchema: "Pharma263",
                        principalTable: "QuoteStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                schema: "Pharma263",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SalesDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true),
                    Total = table.Column<decimal>(type: "DECIMAL(14,2)", nullable: false),
                    Discount = table.Column<decimal>(type: "DECIMAL(14,2)", nullable: false),
                    GrandTotal = table.Column<decimal>(type: "DECIMAL(14,2)", nullable: false),
                    PaymentDueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentMethodId = table.Column<int>(type: "int", nullable: false),
                    SaleStatusId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, defaultValue: "'System'"),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TimeStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sales_Customers",
                        column: x => x.CustomerId,
                        principalSchema: "Pharma263",
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sales_PaymentMethods",
                        column: x => x.PaymentMethodId,
                        principalSchema: "Pharma263",
                        principalTable: "PaymentMethod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sales_SaleStatuses",
                        column: x => x.SaleStatusId,
                        principalSchema: "Pharma263",
                        principalTable: "SaleStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AccountsPayable",
                schema: "Pharma263",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AmountOwed = table.Column<decimal>(type: "DECIMAL(14,4)", nullable: false, defaultValue: 0m),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "DECIMAL(14,4)", nullable: false, defaultValue: 0m),
                    BalanceOwed = table.Column<decimal>(type: "DECIMAL(14,4)", nullable: false, defaultValue: 0m),
                    AccountsPayableStatusId = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    PurchaseId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, defaultValue: "'System'"),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TimeStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountsPayable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountsPayable_AccountsPayableStatus_AccountsPayableStatusId",
                        column: x => x.AccountsPayableStatusId,
                        principalSchema: "Pharma263",
                        principalTable: "AccountsPayableStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccountsPayable_Purchase_PurchaseId",
                        column: x => x.PurchaseId,
                        principalSchema: "Pharma263",
                        principalTable: "Purchase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccountsPayable_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "Pharma263",
                        principalTable: "Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseItems",
                schema: "Pharma263",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<decimal>(type: "DECIMAL(14,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "DECIMAL(14,2)", nullable: false),
                    BatchNo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    MedicineId = table.Column<int>(type: "int", nullable: false),
                    PurchaseId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, defaultValue: "'System'"),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TimeStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseItems_Medicine_MedicineId",
                        column: x => x.MedicineId,
                        principalSchema: "Pharma263",
                        principalTable: "Medicine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseItems_Purchase_PurchaseId",
                        column: x => x.PurchaseId,
                        principalSchema: "Pharma263",
                        principalTable: "Purchase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuotationItems",
                schema: "Pharma263",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<decimal>(type: "DECIMAL(14,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "DECIMAL(14,2)", nullable: false),
                    StockId = table.Column<int>(type: "int", nullable: false),
                    QuotationId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, defaultValue: "'System'"),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TimeStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotationItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuotationItems_Quotation_QuotationId",
                        column: x => x.QuotationId,
                        principalSchema: "Pharma263",
                        principalTable: "Quotation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuotationItems_Stock_StockId",
                        column: x => x.StockId,
                        principalSchema: "Pharma263",
                        principalTable: "Stock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccountsReceivable",
                schema: "Pharma263",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AmountDue = table.Column<decimal>(type: "DECIMAL(14,4)", nullable: false, defaultValue: 0m),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "DECIMAL(14,4)", nullable: false, defaultValue: 0m),
                    BalanceDue = table.Column<decimal>(type: "DECIMAL(14,4)", nullable: false, defaultValue: 0m),
                    AccountsReceivableStatusId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    SaleId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, defaultValue: "'System'"),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TimeStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountsReceivable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountsPayable_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "Pharma263",
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccountsPayable_Sale_SaleId",
                        column: x => x.SaleId,
                        principalSchema: "Pharma263",
                        principalTable: "Sales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AccountsReceivable_AccountsReceivableStatus_AccountsReceivableStatusId",
                        column: x => x.AccountsReceivableStatusId,
                        principalSchema: "Pharma263",
                        principalTable: "AccountsReceivableStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Returns",
                schema: "Pharma263",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    DateReturned = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    ReturnDestinationId = table.Column<int>(type: "int", nullable: false),
                    ReturnReasonId = table.Column<int>(type: "int", nullable: false),
                    ReturnStatusId = table.Column<int>(type: "int", nullable: false),
                    StockId = table.Column<int>(type: "int", nullable: false),
                    SaleId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, defaultValue: "'System'"),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TimeStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Returns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Returns_ReturnDestination_ReturnDestinationId",
                        column: x => x.ReturnDestinationId,
                        principalSchema: "Pharma263",
                        principalTable: "ReturnDestination",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Returns_ReturnReason_ReturnReasonId",
                        column: x => x.ReturnReasonId,
                        principalSchema: "Pharma263",
                        principalTable: "ReturnReason",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Returns_ReturnStatus_ReturnStatusId",
                        column: x => x.ReturnStatusId,
                        principalSchema: "Pharma263",
                        principalTable: "ReturnStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Returns_Sales_SaleId",
                        column: x => x.SaleId,
                        principalSchema: "Pharma263",
                        principalTable: "Sales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Returns_Stock_StockId",
                        column: x => x.StockId,
                        principalSchema: "Pharma263",
                        principalTable: "Stock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SalesItems",
                schema: "Pharma263",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<decimal>(type: "DECIMAL(14,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "DECIMAL(14,2)", nullable: false),
                    StockId = table.Column<int>(type: "int", nullable: false),
                    SaleId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, defaultValue: "'System'"),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TimeStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SalesItems_Sales_SaleId",
                        column: x => x.SaleId,
                        principalSchema: "Pharma263",
                        principalTable: "Sales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SalesItems_Stock_StockId",
                        column: x => x.StockId,
                        principalSchema: "Pharma263",
                        principalTable: "Stock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMade",
                schema: "Pharma263",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AmountPaid = table.Column<decimal>(type: "DECIMAL(14,4)", nullable: false, defaultValue: 0m),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentMethodId = table.Column<int>(type: "int", nullable: false),
                    AccountPayableId = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, defaultValue: "'System'"),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TimeStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentMade_AccountPayable_AccountPayableId",
                        column: x => x.AccountPayableId,
                        principalSchema: "Pharma263",
                        principalTable: "AccountsPayable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentMade_PaymentMethod_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalSchema: "Pharma263",
                        principalTable: "PaymentMethod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentMade_Supplier_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "Pharma263",
                        principalTable: "Supplier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PaymentReceived",
                schema: "Pharma263",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AmountReceived = table.Column<decimal>(type: "DECIMAL(14,4)", nullable: false, defaultValue: 0m),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentMethodId = table.Column<int>(type: "int", nullable: false),
                    AccountsReceivableId = table.Column<int>(type: "int", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    CreatedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, defaultValue: "'System'"),
                    ModifiedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ModifiedBy = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    TimeStamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentReceived", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentReceived_AccountsReceivable_AccountsReceivableId",
                        column: x => x.AccountsReceivableId,
                        principalSchema: "Pharma263",
                        principalTable: "AccountsReceivable",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentReceived_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "Pharma263",
                        principalTable: "Customer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentReceived_PaymentMethod_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalSchema: "Pharma263",
                        principalTable: "PaymentMethod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "Pharma263",
                table: "AccountsPayableStatus",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "ModifiedBy", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { 1, "System", new DateTimeOffset(new DateTime(2024, 5, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "The account has not been paid yet", null, null, "Unpaid" },
                    { 2, "System", new DateTimeOffset(new DateTime(2024, 5, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "The account has been partially paid", null, null, "Partially Paid" },
                    { 3, "System", new DateTimeOffset(new DateTime(2024, 5, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "The account has been fully paid up", null, null, "Paid" }
                });

            migrationBuilder.InsertData(
                schema: "Pharma263",
                table: "AccountsReceivableStatus",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "ModifiedBy", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { 1, "System", new DateTimeOffset(new DateTime(2024, 5, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "The account has not been paid yet", null, null, "Unpaid" },
                    { 2, "System", new DateTimeOffset(new DateTime(2024, 5, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "The account has been partially paid", null, null, "Partially Paid" },
                    { 3, "System", new DateTimeOffset(new DateTime(2024, 5, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "The account has been fully paid up", null, null, "Paid" }
                });

            migrationBuilder.InsertData(
                schema: "Pharma263",
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "cac43a6e-f7bb-4448-baaf-1add431aabbf", null, "Supervisor", "SUPERVISOR" },
                    { "cac43a6e-f7bb-4448-baaf-1add431ccbbf", null, "Sales", "SALES" },
                    { "cbc43a8e-f7bb-4445-baaf-1add431ffbbf", null, "Administrator", "ADMINISTRATOR" }
                });

            migrationBuilder.InsertData(
                schema: "Pharma263",
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "7e334968-23e4-4652-b7b7-8574d048cdc6", 0, "6fe47ec6-08f2-41c5-91d7-d13f2c802278", "sales@pharma263.com", true, "Sales", "Sales", false, null, "SALES@PHARMA263.COM", "SALES", "AQAAAAIAAYagAAAAELKwtVl832F5Z46hDWPCHE2jZ4Gbp7C10GXdgzHvEA5qr1Y3DkotuF8yxbRwiVYagg==", null, false, "86a27c1b-f230-46a8-bd4c-1a5bd850a36a", false, "Sales" },
                    { "8e445865-a24d-4543-a6c6-9443d0482205", 0, "e0aea8fe-2b62-4fef-a3d8-01921ca1881d", "pfmukwenya@gmail.com", true, "Percy", "Mukwenya", false, null, "PFMUKWENYA@GMAIL.COM", "PERCY", "AQAAAAIAAYagAAAAEL2DRithFcowrOSbL1VRWvtdjVuc26wPV6/SD3Y6J9S5dw1UqniKaAlyE6+KuSVqgw==", null, false, "9bb7deb7-8101-44ad-b866-336677689d16", false, "Percy" },
                    { "8e445865-a24d-4543-a6c6-9443d048cdb9", 0, "3a1b706b-d793-484a-b831-ced69e1ad960", "admin@pharma263.com", true, "Admin", "Admin", false, null, "ADMIN@PHARMA263.COM", "ADMIN", "AQAAAAIAAYagAAAAEP4On9Ni2XIGUKRyEQopgCNZuWN9IYKH7zz0vX8wer52hH7K/u0d1pLGl7nBmWmkjQ==", null, false, "78a2b634-b4bd-4bf5-a0f6-9594ddbeb0a6", false, "admin" },
                    { "9e224968-33e4-4652-b7b7-8574d048cdb9", 0, "83d2a263-36b4-4930-b529-215393807f12", "supervisor@pharma263.com", true, "Supervisor", "Supervisor", false, null, "SUPERVISOR@PHARMA263.COM", "SUPERVISOR", "AQAAAAIAAYagAAAAECrrSnxpW/C8ED9RLd1dy8DXTy/3G460NAeDrsd7QSNZk68Acc1MdA8IciVf4MzlBA==", null, false, "f9018862-266d-41cd-afe8-582ce951e6cb", false, "Supervisor" }
                });

            migrationBuilder.InsertData(
                schema: "Pharma263",
                table: "CustomerType",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "ModifiedBy", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { 1, "System", new DateTimeOffset(new DateTime(2024, 5, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Pharmacy customers", null, null, "Pharmacy" },
                    { 2, "System", new DateTimeOffset(new DateTime(2024, 5, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Doctors", null, null, "Doctor" },
                    { 3, "System", new DateTimeOffset(new DateTime(2024, 5, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Clinic", null, null, "Clinic" },
                    { 4, "System", new DateTimeOffset(new DateTime(2024, 5, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Shop", null, null, "Shop" }
                });

            migrationBuilder.InsertData(
                schema: "Pharma263",
                table: "PaymentMethod",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "ModifiedBy", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { 1, "System", new DateTimeOffset(new DateTime(2024, 5, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Cash payment in USD", null, null, "Cash - USD" },
                    { 2, "System", new DateTimeOffset(new DateTime(2024, 5, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Cash payment in ZiG", null, null, "Cash - ZiG" },
                    { 3, "System", new DateTimeOffset(new DateTime(2024, 5, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Mobile money", null, null, "Mobile money" }
                });

            migrationBuilder.InsertData(
                schema: "Pharma263",
                table: "PurchaseStatus",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "ModifiedBy", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { 1, "System", new DateTimeOffset(new DateTime(2024, 5, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Due date manually added", null, null, "Due" },
                    { 2, "System", new DateTimeOffset(new DateTime(2024, 5, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Purchase partially paid", null, null, "Partially Paid" },
                    { 3, "System", new DateTimeOffset(new DateTime(2024, 5, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Fully Paid", null, null, "Fully Paid" },
                    { 4, "System", new DateTimeOffset(new DateTime(2024, 5, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Payment due in 7 days", null, null, "Due - 7 days" },
                    { 5, "System", new DateTimeOffset(new DateTime(2024, 5, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Payment due in 14 days", null, null, "Due - 14 days" },
                    { 6, "System", new DateTimeOffset(new DateTime(2024, 5, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Payment due in 30 days", null, null, "Due - 30 days" }
                });

            migrationBuilder.InsertData(
                schema: "Pharma263",
                table: "ReturnDestination",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "ModifiedBy", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { 1, "System", new DateTimeOffset(new DateTime(2024, 5, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Stock Update", null, null, "Stock" },
                    { 2, "System", new DateTimeOffset(new DateTime(2024, 5, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Add to Quarantine", null, null, "Quarantine" },
                    { 3, "System", new DateTimeOffset(new DateTime(2024, 5, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Items to be Dispossed of", null, null, "Disposal" },
                    { 4, "System", new DateTimeOffset(new DateTime(2024, 5, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Product to be returned to supplier", null, null, "Supplier" }
                });

            migrationBuilder.InsertData(
                schema: "Pharma263",
                table: "ReturnReason",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "ModifiedBy", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { 1, "System", new DateTimeOffset(new DateTime(2024, 5, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Customer ordered too much", null, null, "Over Ordering" },
                    { 2, "System", new DateTimeOffset(new DateTime(2024, 5, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Expired Product", null, null, "Expired Product" },
                    { 3, "System", new DateTimeOffset(new DateTime(2024, 5, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Damaged product", null, null, "Damaged" },
                    { 4, "System", new DateTimeOffset(new DateTime(2024, 5, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Factory Recall", null, null, "Factory Recall" },
                    { 5, "System", new DateTimeOffset(new DateTime(2024, 5, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Other Reasons", null, null, "Other" }
                });

            migrationBuilder.InsertData(
                schema: "Pharma263",
                table: "ReturnStatus",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "ModifiedBy", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { 1, "System", new DateTimeOffset(new DateTime(2024, 5, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Processed", null, null, "Processed" },
                    { 2, "System", new DateTimeOffset(new DateTime(2024, 5, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Decision Pending", null, null, "Decision Pending" }
                });

            migrationBuilder.InsertData(
                schema: "Pharma263",
                table: "SaleStatus",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Description", "ModifiedBy", "ModifiedDate", "Name" },
                values: new object[,]
                {
                    { 1, "System", new DateTimeOffset(new DateTime(2024, 5, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Due date manually added", null, null, "Due" },
                    { 2, "System", new DateTimeOffset(new DateTime(2024, 5, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Purchase partially paid", null, null, "Partially Paid" },
                    { 3, "System", new DateTimeOffset(new DateTime(2024, 5, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Fully Paid", null, null, "Fully Paid" },
                    { 4, "System", new DateTimeOffset(new DateTime(2024, 5, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Payment due in 7 days", null, null, "Due - 7 days" },
                    { 5, "System", new DateTimeOffset(new DateTime(2024, 5, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Payment due in 14 days", null, null, "Due - 14 days" },
                    { 6, "System", new DateTimeOffset(new DateTime(2024, 5, 3, 8, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Payment due in 30 days", null, null, "Due - 30 days" }
                });

            migrationBuilder.InsertData(
                schema: "Pharma263",
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "cac43a6e-f7bb-4448-baaf-1add431ccbbf", "7e334968-23e4-4652-b7b7-8574d048cdc6" },
                    { "cbc43a8e-f7bb-4445-baaf-1add431ffbbf", "8e445865-a24d-4543-a6c6-9443d0482205" },
                    { "cbc43a8e-f7bb-4445-baaf-1add431ffbbf", "8e445865-a24d-4543-a6c6-9443d048cdb9" },
                    { "cac43a6e-f7bb-4448-baaf-1add431aabbf", "9e224968-33e4-4652-b7b7-8574d048cdb9" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountsPayable_AccountsPayableStatusId",
                schema: "Pharma263",
                table: "AccountsPayable",
                column: "AccountsPayableStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountsPayable_PurchaseId",
                schema: "Pharma263",
                table: "AccountsPayable",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountsPayable_SupplierId",
                schema: "Pharma263",
                table: "AccountsPayable",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "UX_AccountsPayableStatus_Name",
                schema: "Pharma263",
                table: "AccountsPayableStatus",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountsReceivable_AccountsReceivableStatusId",
                schema: "Pharma263",
                table: "AccountsReceivable",
                column: "AccountsReceivableStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountsReceivable_CustomerId",
                schema: "Pharma263",
                table: "AccountsReceivable",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_AccountsReceivable_SaleId",
                schema: "Pharma263",
                table: "AccountsReceivable",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "UX_AccountsReceivableStatus_Name",
                schema: "Pharma263",
                table: "AccountsReceivableStatus",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                schema: "Pharma263",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                schema: "Pharma263",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                schema: "Pharma263",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                schema: "Pharma263",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                schema: "Pharma263",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "Pharma263",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "Pharma263",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AuditEntry_EntityName_EntityId_ActionType",
                schema: "Pharma263",
                table: "AuditEntry",
                columns: new[] { "EntityName", "EntityId", "ActionType" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditEntry_Username",
                schema: "Pharma263",
                table: "AuditEntry",
                column: "Username");

            migrationBuilder.CreateIndex(
                name: "IX_Customer_CustomerTypeId",
                schema: "Pharma263",
                table: "Customer",
                column: "CustomerTypeId");

            migrationBuilder.CreateIndex(
                name: "UX_Customer_Name",
                schema: "Pharma263",
                table: "Customer",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_CustomerType_Name",
                schema: "Pharma263",
                table: "CustomerType",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_Medicine_Name",
                schema: "Pharma263",
                table: "Medicine",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMade_AccountPayableId",
                schema: "Pharma263",
                table: "PaymentMade",
                column: "AccountPayableId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMade_PaymentMethodId",
                schema: "Pharma263",
                table: "PaymentMade",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMade_SupplierId",
                schema: "Pharma263",
                table: "PaymentMade",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "UX_PaymentMethod_Name",
                schema: "Pharma263",
                table: "PaymentMethod",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentReceived_AccountsReceivableId",
                schema: "Pharma263",
                table: "PaymentReceived",
                column: "AccountsReceivableId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentReceived_CustomerId",
                schema: "Pharma263",
                table: "PaymentReceived",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentReceived_PaymentMethodId",
                schema: "Pharma263",
                table: "PaymentReceived",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_Id_PurchaseDate",
                schema: "Pharma263",
                table: "Purchase",
                columns: new[] { "Id", "PurchaseDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_PaymentMethodId",
                schema: "Pharma263",
                table: "Purchase",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_PurchaseStatusId",
                schema: "Pharma263",
                table: "Purchase",
                column: "PurchaseStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_SupplierId",
                schema: "Pharma263",
                table: "Purchase",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseItems_MedicineId",
                schema: "Pharma263",
                table: "PurchaseItems",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseItems_PurchaseId",
                schema: "Pharma263",
                table: "PurchaseItems",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "UX_PurchaseStatus_Name",
                schema: "Pharma263",
                table: "PurchaseStatus",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quarantine_MedicineId",
                schema: "Pharma263",
                table: "Quarantine",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_Quotation_CustomerId",
                schema: "Pharma263",
                table: "Quotation",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Quotation_QuoteStatusId",
                schema: "Pharma263",
                table: "Quotation",
                column: "QuoteStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationItems_QuotationId",
                schema: "Pharma263",
                table: "QuotationItems",
                column: "QuotationId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationItems_StockId",
                schema: "Pharma263",
                table: "QuotationItems",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "UX_ReturnDestination_Name",
                schema: "Pharma263",
                table: "ReturnDestination",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_ReturnReason_Name",
                schema: "Pharma263",
                table: "ReturnReason",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Returns_ReturnDestinationId",
                schema: "Pharma263",
                table: "Returns",
                column: "ReturnDestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_Returns_ReturnReasonId",
                schema: "Pharma263",
                table: "Returns",
                column: "ReturnReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Returns_ReturnStatusId",
                schema: "Pharma263",
                table: "Returns",
                column: "ReturnStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Returns_SaleId",
                schema: "Pharma263",
                table: "Returns",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_Returns_StockId",
                schema: "Pharma263",
                table: "Returns",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "UX_ReturnStatus_Name",
                schema: "Pharma263",
                table: "ReturnStatus",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sale_Id_SaleDate",
                schema: "Pharma263",
                table: "Sales",
                columns: new[] { "Id", "SalesDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sales_CustomerId",
                schema: "Pharma263",
                table: "Sales",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_PaymentMethodId",
                schema: "Pharma263",
                table: "Sales",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_SaleStatusId",
                schema: "Pharma263",
                table: "Sales",
                column: "SaleStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesItems_SaleId",
                schema: "Pharma263",
                table: "SalesItems",
                column: "SaleId");

            migrationBuilder.CreateIndex(
                name: "IX_SalesItems_StockId",
                schema: "Pharma263",
                table: "SalesItems",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "UX_SaleStatus_Name",
                schema: "Pharma263",
                table: "SaleStatus",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Medicine_Name_BatchNo",
                schema: "Pharma263",
                table: "Stock",
                columns: new[] { "MedicineId", "BatchNo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_Supplier_Name",
                schema: "Pharma263",
                table: "Supplier",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims",
                schema: "Pharma263");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims",
                schema: "Pharma263");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins",
                schema: "Pharma263");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles",
                schema: "Pharma263");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens",
                schema: "Pharma263");

            migrationBuilder.DropTable(
                name: "AuditEntry",
                schema: "Pharma263");

            migrationBuilder.DropTable(
                name: "PaymentMade",
                schema: "Pharma263");

            migrationBuilder.DropTable(
                name: "PaymentReceived",
                schema: "Pharma263");

            migrationBuilder.DropTable(
                name: "PurchaseItems",
                schema: "Pharma263");

            migrationBuilder.DropTable(
                name: "Quarantine",
                schema: "Pharma263");

            migrationBuilder.DropTable(
                name: "QuotationItems",
                schema: "Pharma263");

            migrationBuilder.DropTable(
                name: "Returns",
                schema: "Pharma263");

            migrationBuilder.DropTable(
                name: "SalesItems",
                schema: "Pharma263");

            migrationBuilder.DropTable(
                name: "StoreSetting",
                schema: "Pharma263");

            migrationBuilder.DropTable(
                name: "AspNetRoles",
                schema: "Pharma263");

            migrationBuilder.DropTable(
                name: "AspNetUsers",
                schema: "Pharma263");

            migrationBuilder.DropTable(
                name: "AccountsPayable",
                schema: "Pharma263");

            migrationBuilder.DropTable(
                name: "AccountsReceivable",
                schema: "Pharma263");

            migrationBuilder.DropTable(
                name: "Quotation",
                schema: "Pharma263");

            migrationBuilder.DropTable(
                name: "ReturnDestination",
                schema: "Pharma263");

            migrationBuilder.DropTable(
                name: "ReturnReason",
                schema: "Pharma263");

            migrationBuilder.DropTable(
                name: "ReturnStatus",
                schema: "Pharma263");

            migrationBuilder.DropTable(
                name: "Stock",
                schema: "Pharma263");

            migrationBuilder.DropTable(
                name: "AccountsPayableStatus",
                schema: "Pharma263");

            migrationBuilder.DropTable(
                name: "Purchase",
                schema: "Pharma263");

            migrationBuilder.DropTable(
                name: "Sales",
                schema: "Pharma263");

            migrationBuilder.DropTable(
                name: "AccountsReceivableStatus",
                schema: "Pharma263");

            migrationBuilder.DropTable(
                name: "QuoteStatus",
                schema: "Pharma263");

            migrationBuilder.DropTable(
                name: "Medicine",
                schema: "Pharma263");

            migrationBuilder.DropTable(
                name: "PurchaseStatus",
                schema: "Pharma263");

            migrationBuilder.DropTable(
                name: "Supplier",
                schema: "Pharma263");

            migrationBuilder.DropTable(
                name: "Customer",
                schema: "Pharma263");

            migrationBuilder.DropTable(
                name: "PaymentMethod",
                schema: "Pharma263");

            migrationBuilder.DropTable(
                name: "SaleStatus",
                schema: "Pharma263");

            migrationBuilder.DropTable(
                name: "CustomerType",
                schema: "Pharma263");
        }
    }
}
