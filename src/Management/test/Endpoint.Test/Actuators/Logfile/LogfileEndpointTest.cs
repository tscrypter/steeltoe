// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using Microsoft.AspNetCore.Routing.Matching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        string expectedFilePath = Path.Combine(Assembly.GetEntryAssembly()!.Location, "logs/testfile.log");
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
}
