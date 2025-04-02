// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using Steeltoe.Management.Configuration;

namespace Steeltoe.Management.Endpoint.Actuators.Logfile;

public class LogfileEndpointHandler : ILogfileEndpointHandler
{
    public EndpointOptions Options => throw new NotImplementedException();

    public async Task<string> InvokeAsync(object? argument, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    internal string GetLogFilePath()
    {
        return Path.Combine(Assembly.GetEntryAssembly().Location, "logs/testfile.log");
    }
}
