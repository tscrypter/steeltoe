// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;
using Steeltoe.Common.TestResources;
using Steeltoe.Management.Tasks;

namespace Steeltoe.Connectors.EntityFrameworkCore.Test;

public sealed class MigrateDbContextTaskTest
{
    [Fact]
    public async Task WebApplication_ExecutesMigrateTask()
    {
        const string taskName = MigrateDbContextTask<TestMigrateDbContext>.Name;
        string[] args = [$"RunTask={taskName}"];

        WebApplicationBuilder builder = TestWebApplicationBuilderFactory.Create(args);

        builder.Services.AddDbContext<TestMigrateDbContext>(options => options.UseSqlite("Data Source=Test.db"));
        builder.Services.AddTask<MigrateDbContextTask<TestMigrateDbContext>>(taskName);

        await using WebApplication app = builder.Build();

        await app.RunWithTasksAsync(TestContext.Current.CancellationToken);

        TestMigrator.HasMigrated.Should().BeTrue();
    }

    private sealed class TestMigrateDbContext(DbContextOptions options)
        : DbContext(options)
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ReplaceService<IMigrator, TestMigrator>();
        }
    }

    private sealed class TestMigrator : IMigrator
    {
        public static bool HasMigrated { get; private set; }

        public void Migrate(string? targetMigration = null)
        {
            throw new NotImplementedException();
        }

        public Task MigrateAsync(string? targetMigration = null, CancellationToken cancellationToken = default)
        {
            HasMigrated = true;
            return Task.CompletedTask;
        }

        public string GenerateScript(string? fromMigration = null, string? toMigration = null,
            MigrationsSqlGenerationOptions options = MigrationsSqlGenerationOptions.Default)
        {
            throw new NotImplementedException();
        }

#if NET9_0_OR_GREATER
        public bool HasPendingModelChanges()
        {
            throw new NotImplementedException();
        }
#endif
    }
}
