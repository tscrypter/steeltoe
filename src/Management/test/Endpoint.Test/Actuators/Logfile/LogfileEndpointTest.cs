// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Steeltoe.Management.Configuration;
using Steeltoe.Management.Endpoint.Actuators.Logfile;
using Xunit.Abstractions;

namespace Steeltoe.Management.Endpoint.Test.Actuators.Logfile;

public sealed class LogfileEndpointTest(ITestOutputHelper testOutputHelper) : BaseTest
{
    private readonly ITestOutputHelper _testOutputHelper = testOutputHelper;

    [Fact]
    public void GetLogFilePathWithRelativePath_ReturnsExpected()
    {
        // arrange
        string directoryName = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!;
        string expectedFilePath = Path.Combine(directoryName, "logs/testfile.log");
        var appSettings = new Dictionary<string, string?>
        {
            ["management:endpoints:logfile:filePath"] = "logs/testfile.log",
            ["management:endpoints:logfile:enabled"] = "true"
        };

        using var testContext = new TestContext(_testOutputHelper);

        testContext.AdditionalConfiguration = configuration =>
        {
            configuration.AddInMemoryCollection(appSettings);
        };

        testContext.AdditionalServices = (services, _) =>
        {
            services.AddSingleton(TestHostEnvironmentFactory.Create());
            services.ConfigureEndpointOptions<LogfileEndpointOptions, ConfigureLogfileEndpointOptions>();
            services.AddSingleton<ILogfileEndpointHandler, LogfileEndpointHandler>();
        };

        var handler = (LogfileEndpointHandler)testContext.GetRequiredService<ILogfileEndpointHandler>();

        // act
        string result = handler.GetLogFilePath();

        // assert
        Assert.NotNull(result);
        Assert.Equal(expectedFilePath, result);
    }

    [Fact]
    public void GetLogFilePathWithAbsolutePath_ReturnsExpected()
    {
        // arrange
        const string expectedFilePath = "/logs/testfile.log";
        var appSettings = new Dictionary<string, string?>
        {
            ["management:endpoints:logfile:filePath"] = expectedFilePath,
            ["management:endpoints:logfile:enabled"] = "true"
        };

        using var testContext = new TestContext(_testOutputHelper);

        testContext.AdditionalConfiguration = configuration =>
        {
            configuration.AddInMemoryCollection(appSettings);
        };

        testContext.AdditionalServices = (services, _) =>
        {
            services.AddSingleton(TestHostEnvironmentFactory.Create());
            services.ConfigureEndpointOptions<LogfileEndpointOptions, ConfigureLogfileEndpointOptions>();
            services.AddSingleton<ILogfileEndpointHandler, LogfileEndpointHandler>();
        };

        var handler = (LogfileEndpointHandler)testContext.GetRequiredService<ILogfileEndpointHandler>();

        // act
        string result = handler.GetLogFilePath();

        // assert
        Assert.NotNull(result);
        Assert.Equal(expectedFilePath, result);
    }

    [Fact]
    public void Options_ReturnsExpected()
    {
        // arrange
        var appSettings = new Dictionary<string, string?>
        {
            ["management:endpoints:logfile:filePath"] = "logs/testfile.log",
            ["management:endpoints:logfile:enabled"] = "true"
        };

        using var testContext = new TestContext(_testOutputHelper);

        testContext.AdditionalConfiguration = configuration =>
        {
            configuration.AddInMemoryCollection(appSettings);
        };

        testContext.AdditionalServices = (services, _) =>
        {
            services.AddSingleton(TestHostEnvironmentFactory.Create());
            services.ConfigureEndpointOptions<LogfileEndpointOptions, ConfigureLogfileEndpointOptions>();
            services.AddSingleton<ILogfileEndpointHandler, LogfileEndpointHandler>();
        };

        var handler = (LogfileEndpointHandler)testContext.GetRequiredService<ILogfileEndpointHandler>();

        // act
        LogfileEndpointOptions options = (handler.Options as LogfileEndpointOptions)!;

        // assert
        options.Id.Should().Be("logfile");
        options.RequiredPermissions.Should().Be(EndpointPermissions.Restricted);
        options.Path.Should().Be("logfile");
        options.FilePath.Should().Be("logs/testfile.log");
        options.AllowedVerbs.Should().Contain("Get");
        options.AllowedVerbs.Should().HaveCount(1);
    }
}
