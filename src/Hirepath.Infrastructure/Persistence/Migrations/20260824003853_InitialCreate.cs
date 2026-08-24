using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hirepath.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Analyses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ResumeId = table.Column<Guid>(type: "TEXT", nullable: false),
                    JobDescriptionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    OverallScore = table.Column<int>(type: "INTEGER", nullable: false),
                    Verdict = table.Column<string>(type: "TEXT", nullable: false),
                    Dimensions = table.Column<string>(type: "TEXT", nullable: false),
                    Strengths = table.Column<string>(type: "TEXT", nullable: false),
                    Gaps = table.Column<string>(type: "TEXT", nullable: false),
                    ResumeSuggestions = table.Column<string>(type: "TEXT", nullable: false),
                    PrepPlan = table.Column<string>(type: "TEXT", nullable: false),
                    Deterministic = table.Column<string>(type: "TEXT", nullable: false),
                    ModelId = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    PromptVersion = table.Column<string>(type: "TEXT", maxLength: 80, nullable: true),
                    CreatedAt = table.Column<long>(type: "INTEGER", nullable: false),
                    UpdatedAt = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Analyses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobDescriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Company = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Location = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    WorkMode = table.Column<string>(type: "TEXT", nullable: false),
                    Seniority = table.Column<string>(type: "TEXT", nullable: false),
                    RawText = table.Column<string>(type: "TEXT", nullable: false),
                    Parsed = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<long>(type: "INTEGER", nullable: false),
                    UpdatedAt = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobDescriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Location = table.Column<string>(type: "TEXT", nullable: true),
                    YearsOfExperience = table.Column<int>(type: "INTEGER", nullable: true),
                    Seniority = table.Column<string>(type: "TEXT", nullable: false),
                    TargetRoles = table.Column<string>(type: "TEXT", nullable: false),
                    Links = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<long>(type: "INTEGER", nullable: false),
                    UpdatedAt = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Resumes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Label = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    FileName = table.Column<string>(type: "TEXT", maxLength: 400, nullable: false),
                    ContentType = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    StoredPath = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    SizeBytes = table.Column<long>(type: "INTEGER", nullable: false),
                    ExtractedText = table.Column<string>(type: "TEXT", nullable: false),
                    Parsed = table.Column<string>(type: "TEXT", nullable: true),
                    IsDefault = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<long>(type: "INTEGER", nullable: false),
                    UpdatedAt = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resumes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Provider = table.Column<string>(type: "TEXT", nullable: false),
                    Model = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    ApiKey = table.Column<string>(type: "TEXT", nullable: true),
                    BaseUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Theme = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    DemoMode = table.Column<bool>(type: "INTEGER", nullable: false),
                    RedactPii = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<long>(type: "INTEGER", nullable: false),
                    UpdatedAt = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Threads",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    PinnedResumeId = table.Column<Guid>(type: "TEXT", nullable: true),
                    PinnedJobDescriptionId = table.Column<Guid>(type: "TEXT", nullable: true),
                    PinnedAnalysisId = table.Column<Guid>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<long>(type: "INTEGER", nullable: false),
                    UpdatedAt = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Threads", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ThreadId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false),
                    Text = table.Column<string>(type: "TEXT", nullable: false),
                    Sequence = table.Column<int>(type: "INTEGER", nullable: false),
                    ToolCalls = table.Column<string>(type: "TEXT", nullable: false),
                    Widgets = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<long>(type: "INTEGER", nullable: false),
                    UpdatedAt = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Threads_ThreadId",
                        column: x => x.ThreadId,
                        principalTable: "Threads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Analyses_JobDescriptionId",
                table: "Analyses",
                column: "JobDescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Analyses_ResumeId",
                table: "Analyses",
                column: "ResumeId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ThreadId_Sequence",
                table: "Messages",
                columns: new[] { "ThreadId", "Sequence" });

            migrationBuilder.CreateIndex(
                name: "IX_Resumes_IsDefault",
                table: "Resumes",
                column: "IsDefault");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Analyses");

            migrationBuilder.DropTable(
                name: "JobDescriptions");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "Resumes");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "Threads");
        }
    }
}
