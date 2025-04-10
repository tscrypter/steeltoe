// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Serialization;
using Steeltoe.Common.CasingConventions;
using Steeltoe.Common.HealthChecks;
using Steeltoe.Common.Json;

namespace Steeltoe.Management.Endpoint.Actuators.Health;

public sealed class HealthEndpointResponse
{
    /// <summary>
    /// Gets the status of the health check.
    /// </summary>
    [JsonConverter(typeof(SnakeCaseAllCapsEnumMemberJsonConverter))]
    public HealthStatus Status { get; init; }

    /// <summary>
    /// Gets a description of the health check result.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// Gets the individual health check components, including their details.
    /// </summary>
    [JsonIgnoreEmptyCollection]
    public IDictionary<string, HealthCheckResult> Components { get; } = new Dictionary<string, HealthCheckResult>();

    /// <summary>
    /// Gets the list of available health groups.
    /// </summary>
    [JsonIgnoreEmptyCollection]
    public IList<string> Groups { get; } = new List<string>();

    /// <summary>
    /// Gets a value indicating whether a health response exists.
    /// </summary>
    [JsonIgnore]
    public bool Exists { get; init; } = true;

    public HealthEndpointResponse()
    {
    }

    public HealthEndpointResponse(HealthCheckResult result)
    {
        ArgumentNullException.ThrowIfNull(result);

        Status = result.Status;
        Description = result.Description;

        foreach ((string key, object value) in result.Details)
        {
            if (value is HealthCheckResult healthCheck)
            {
                Components[key] = healthCheck;
            }
        }
    }
}
