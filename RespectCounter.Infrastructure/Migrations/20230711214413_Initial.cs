using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EFContext.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeathDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PublicScore = table.Column<float>(type: "real", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Persons_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Persons_AspNetUsers_LastUpdatedById",
                        column: x => x.LastUpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsMainTag = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tags_AspNetUsers_LastUpdatedById",
                        column: x => x.LastUpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Happend = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Source = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Verified = table.Column<bool>(type: "bit", nullable: false),
                    IsQuote = table.Column<bool>(type: "bit", nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activities_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Activities_AspNetUsers_LastUpdatedById",
                        column: x => x.LastUpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Activities_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonTag",
                columns: table => new
                {
                    PersonsId = table.Column<int>(type: "int", nullable: false),
                    TagsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonTag", x => new { x.PersonsId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_PersonTag_Persons_PersonsId",
                        column: x => x.PersonsId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonTag_Tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActivityId = table.Column<int>(type: "int", nullable: true),
                    PersonId = table.Column<int>(type: "int", nullable: true),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comment_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comment_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comment_AspNetUsers_LastUpdatedById",
                        column: x => x.LastUpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comment_Comment_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Comment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comment_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Reactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReactionType = table.Column<int>(type: "int", nullable: false),
                    ActivityId = table.Column<int>(type: "int", nullable: true),
                    CommentId = table.Column<int>(type: "int", nullable: true),
                    PersonId = table.Column<int>(type: "int", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedById = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reactions_Activities_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reactions_AspNetUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reactions_AspNetUsers_LastUpdatedById",
                        column: x => x.LastUpdatedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reactions_Comment_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reactions_Persons_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Persons",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "010f8164-7f43-4d1a-aee3-7097b4b53c27", null, "User", null },
                    { "47b57377-8a22-4cff-b4db-b5e0860de74d", null, "Admin", null }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", 0, "44fcb463-9459-4138-bc54-a2b82dbbde43", null, false, false, null, null, null, "AQAAAAIAAYagAAAAEFec0Qo/0M7ej5Fr5DNKsYMk4YF7QBrqxuTDb+WHS9SW1LysNhf2hYX5s4DpGuKGvQ==", null, false, "033ae3be-6a6d-405c-bf01-5f8f84665674", false, "sys" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "47b57377-8a22-4cff-b4db-b5e0860de74d", "d6dfd01e-c1be-4dbf-a899-e00e64598ccb" });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "Id", "Birthday", "Created", "CreatedById", "DeathDate", "Description", "FirstName", "LastName", "LastUpdated", "LastUpdatedById", "Nationality", "PublicScore" },
                values: new object[,]
                {
                    { 1, new DateTime(1988, 8, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3647), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test desc", "Robert", "Lewandowski", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3650), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", "Polish", 5f },
                    { 2, new DateTime(1984, 12, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3659), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test desc", "Robert", "Kubica", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3661), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", "Polish", 5f },
                    { 3, new DateTime(1972, 5, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3666), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test desc", "Andrzej", "Duda", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3668), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", "Polish", 5f },
                    { 4, new DateTime(1957, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3674), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test desc", "Donald", "Tusk", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3675), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", "Polish", 5f }
                });

            migrationBuilder.InsertData(
                table: "Tags",
                columns: new[] { "Id", "Created", "CreatedById", "Description", "IsMainTag", "LastUpdated", "LastUpdatedById", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3715), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", "Test desc", true, new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3717), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", "Sport" },
                    { 2, new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3724), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", "Test desc", false, new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3725), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", "Football" },
                    { 3, new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3730), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", "Test desc", false, new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3732), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", "FC Barcelona" },
                    { 4, new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3737), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", "Test desc", false, new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3738), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", "F1" },
                    { 5, new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3743), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", "Test desc", false, new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3745), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", "WEC" },
                    { 6, new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3759), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", "Test desc", true, new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3761), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", "Politics" },
                    { 7, new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3766), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", "Test desc", false, new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3767), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", "PiS" },
                    { 8, new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3772), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", "Test desc", false, new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3774), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", "PO" }
                });

            migrationBuilder.InsertData(
                table: "Activities",
                columns: new[] { "Id", "Created", "CreatedById", "Description", "Happend", "IsQuote", "LastUpdated", "LastUpdatedById", "PersonId", "Source", "Value", "Verified" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3804), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), true, new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3806), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", 1, "Dude, just trust me", "Milik jest słaby", false },
                    { 2, new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3814), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", "Można utknąć w eeeee korku", new DateTime(2010, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), false, new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3816), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", 2, "https://www.youtube.com/watch?v=qbYMoKxif6I", "Monaco GP 2010: Robeeeeeeeert Kubica P2 in Quali", true }
                });

            migrationBuilder.InsertData(
                table: "Comment",
                columns: new[] { "Id", "ActivityId", "Content", "Created", "CreatedById", "LastUpdated", "LastUpdatedById", "ParentId", "PersonId" },
                values: new object[,]
                {
                    { 1, null, "Najlepszy zawodnik!", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3840), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3842), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", null, 1 },
                    { 8, null, "Bardzo memiczna osoba", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3983), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3985), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", null, 3 },
                    { 11, null, "Ja tam mu nei ufam", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(4005), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(4006), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", null, 3 },
                    { 12, null, "Nie lubiem go, bo Andrzej to dziwne imię", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(4011), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(4013), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", null, 3 }
                });

            migrationBuilder.InsertData(
                table: "Reactions",
                columns: new[] { "Id", "ActivityId", "CommentId", "Created", "CreatedById", "LastUpdated", "LastUpdatedById", "PersonId", "ReactionType" },
                values: new object[,]
                {
                    { 8, null, null, new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(4040), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(4042), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", 1, 0 },
                    { 9, null, null, new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(4047), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(4049), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", 1, 2 },
                    { 10, null, null, new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(4054), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(4055), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", 1, 1 },
                    { 11, null, null, new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(4060), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(4061), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", 1, 3 },
                    { 12, null, null, new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(4066), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(4068), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", 1, 4 },
                    { 13, null, null, new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(4073), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(4075), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", 1, 4 },
                    { 14, null, null, new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(4080), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(4081), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", 1, 4 },
                    { 15, null, null, new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(4086), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(4087), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", 1, 4 }
                });

            migrationBuilder.InsertData(
                table: "Comment",
                columns: new[] { "Id", "ActivityId", "Content", "Created", "CreatedById", "LastUpdated", "LastUpdatedById", "ParentId", "PersonId" },
                values: new object[,]
                {
                    { 2, null, "No nie wiem. Milik lepszy!", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3849), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3851), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", 1, null },
                    { 3, null, "Jest całkiem dobry faktycznie", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3856), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3857), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", 1, null },
                    { 4, 1, "Fajność!", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3863), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3864), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", null, null },
                    { 6, 1, "Niefajność", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3877), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3878), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", null, null },
                    { 7, 2, "Lepsza weeeeeersja: https://www.youtube.com/watch?v=vmLonweq6wA", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3883), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3885), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", null, null },
                    { 9, null, "Hańba!", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3990), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3992), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", 8, null },
                    { 10, null, "Chyba ty", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3998), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(4000), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", 8, null }
                });

            migrationBuilder.InsertData(
                table: "Reactions",
                columns: new[] { "Id", "ActivityId", "CommentId", "Created", "CreatedById", "LastUpdated", "LastUpdatedById", "PersonId", "ReactionType" },
                values: new object[,]
                {
                    { 4, 1, null, new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(4092), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(4094), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", null, 3 },
                    { 5, 1, null, new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(4099), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(4101), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", null, 2 },
                    { 6, 1, null, new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(4106), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(4107), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", null, 3 },
                    { 7, 1, null, new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(4112), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(4113), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", null, 4 }
                });

            migrationBuilder.InsertData(
                table: "Comment",
                columns: new[] { "Id", "ActivityId", "Content", "Created", "CreatedById", "LastUpdated", "LastUpdatedById", "ParentId", "PersonId" },
                values: new object[] { 5, null, "Zgadza się!", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3869), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(3871), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", 4, null });

            migrationBuilder.InsertData(
                table: "Reactions",
                columns: new[] { "Id", "ActivityId", "CommentId", "Created", "CreatedById", "LastUpdated", "LastUpdatedById", "PersonId", "ReactionType" },
                values: new object[,]
                {
                    { 3, null, 4, new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(4118), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(4120), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", null, 4 },
                    { 1, null, 5, new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(4124), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(4126), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", null, 3 },
                    { 2, null, 5, new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(4131), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", new DateTime(2023, 7, 11, 23, 44, 13, 809, DateTimeKind.Local).AddTicks(4132), "d6dfd01e-c1be-4dbf-a899-e00e64598ccb", null, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_CreatedById",
                table: "Activities",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_LastUpdatedById",
                table: "Activities",
                column: "LastUpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_PersonId",
                table: "Activities",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ActivityId",
                table: "Comment",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_CreatedById",
                table: "Comment",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_LastUpdatedById",
                table: "Comment",
                column: "LastUpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ParentId",
                table: "Comment",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_PersonId",
                table: "Comment",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_CreatedById",
                table: "Persons",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Persons_LastUpdatedById",
                table: "Persons",
                column: "LastUpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PersonTag_TagsId",
                table: "PersonTag",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_ActivityId",
                table: "Reactions",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_CommentId",
                table: "Reactions",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_CreatedById",
                table: "Reactions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_LastUpdatedById",
                table: "Reactions",
                column: "LastUpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_PersonId",
                table: "Reactions",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_CreatedById",
                table: "Tags",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_LastUpdatedById",
                table: "Tags",
                column: "LastUpdatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "PersonTag");

            migrationBuilder.DropTable(
                name: "Reactions");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Activities");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
