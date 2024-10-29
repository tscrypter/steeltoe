// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Microsoft.Extensions.Configuration;
using Steeltoe.Management.Endpoint.Configuration;

namespace Steeltoe.Management.Endpoint.Actuators.Health.Availability;

internal sealed class ConfigureLivenessHealthContributorOptions : IConfigureOptionsWithKey<LivenessHealthContributorOptions>
{
    private readonly IConfiguration _configuration;

    public string ConfigurationKey => "management:endpoints:health:liveness";

    public ConfigureLivenessHealthContributorOptions(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        _configuration = configuration;
    }

    public void Configure(LivenessHealthContributorOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        _configuration.GetSection(ConfigurationKey).Bind(options);
    }
}
