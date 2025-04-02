// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Steeltoe.Management.Configuration;

namespace Steeltoe.Management.Endpoint.Actuators.Logfile;

public sealed class LogfileEndpointOptions : EndpointOptions
{
    /// <summary>
    /// Gets the file path to the log file on disk. The path can be absolute or relative to Assembly.GetEntryAssembly().
    /// </summary>
    public string? FilePath { get; set; }
}
