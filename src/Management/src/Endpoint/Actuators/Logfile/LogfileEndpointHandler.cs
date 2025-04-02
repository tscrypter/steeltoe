// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Steeltoe.Management.Configuration;

namespace Steeltoe.Management.Endpoint.Actuators.Logfile;

public sealed class LogfileEndpointHandler : ILogfileEndpointHandler
{
    private readonly IOptionsMonitor<LogfileEndpointOptions> _optionsMonitor;
    private readonly ILogger<LogfileEndpointHandler> _logger;

    public LogfileEndpointHandler(IOptionsMonitor<LogfileEndpointOptions> optionsMonitorMonitor, ILoggerFactory loggerFactory)
    {
        ArgumentNullException.ThrowIfNull(optionsMonitorMonitor);
        ArgumentNullException.ThrowIfNull(loggerFactory);

        _optionsMonitor = optionsMonitorMonitor;
        _logger = loggerFactory.CreateLogger<LogfileEndpointHandler>();
    }

    public EndpointOptions Options => _optionsMonitor.CurrentValue;

    public Task<string> InvokeAsync(object? argument, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    internal string GetLogFilePath()
    {
        _logger.LogTrace("Getting log file path");
        return Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!, _optionsMonitor.CurrentValue.FilePath ?? string.Empty);
    }
}
