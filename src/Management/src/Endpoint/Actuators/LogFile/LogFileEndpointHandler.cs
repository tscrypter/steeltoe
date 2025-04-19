// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System.Reflection;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Steeltoe.Management.Configuration;

namespace Steeltoe.Management.Endpoint.Actuators.LogFile;

internal sealed class LogFileEndpointHandler : ILogFileEndpointHandler
{
    private readonly IOptionsMonitor<LogFileEndpointOptions> _optionsMonitor;
    private readonly ILogger<LogFileEndpointHandler> _logger;

    public EndpointOptions Options => _optionsMonitor.CurrentValue;

    public LogFileEndpointHandler(IOptionsMonitor<LogFileEndpointOptions> optionsMonitor, ILoggerFactory loggerFactory)
    {
        ArgumentNullException.ThrowIfNull(optionsMonitor);
        ArgumentNullException.ThrowIfNull(loggerFactory);

        _optionsMonitor = optionsMonitor;
        _logger = loggerFactory.CreateLogger<LogFileEndpointHandler>();
    }

    public async Task<LogFileEndpointResponse> InvokeAsync(LogFileEndpointRequest argument, CancellationToken cancellationToken)
    {
        _logger.LogTrace("Invoking {Handler} with argument: {Argument}", nameof(LogFileEndpointHandler), argument);
        cancellationToken.ThrowIfCancellationRequested();

        string logFilePath = GetLogFilePath();

        if (!string.IsNullOrEmpty(logFilePath))
        {
            FileInfo logFileInfo = new FileInfo(logFilePath);

            var logFileResult = argument.RangeStartByte.HasValue
                ? GetPartialLogFile(logFilePath, logFileInfo, argument.RangeStartByte.Value, argument.RangeLastByte, cancellationToken)
                : GetEntireLogFile(logFilePath, logFileInfo, cancellationToken);

            return logFileResult;
        }

        return new LogFileEndpointResponse();
    }

    private static LogFileEndpointResponse GetPartialLogFile(string logFilePath, FileInfo logFileInfo, long rangeStartByte, long? rangeEndByte, CancellationToken cancellationToken)
    {
        return new LogFileEndpointResponse(LogFileStreamer.ReadLogFileAsync(logFilePath, rangeStartByte, rangeEndByte ?? logFileInfo.Length -1, cancellationToken),
            rangeEndByte.HasValue ? (rangeEndByte - rangeStartByte + 1) : (logFileInfo.Length - rangeStartByte),
            rangeStartByte,
            rangeEndByte ?? logFileInfo.Length - 1,
            logFileInfo.Length,
            logFileInfo.LastWriteTimeUtc);
    }

    private static LogFileEndpointResponse GetEntireLogFile(string logFilePath, FileInfo logFileInfo, CancellationToken cancellationToken)
    {
        return new LogFileEndpointResponse(LogFileStreamer.ReadLogFileAsync(logFilePath, cancellationToken),
            logFileInfo.Length,
            null,
            null,
            logFileInfo.Length,
            logFileInfo.LastWriteTimeUtc);
    }

    internal string GetLogFilePath()
    {
        _logger.LogTrace("Getting log file path");

        if (!string.IsNullOrEmpty(_optionsMonitor.CurrentValue.FilePath))
        {
            string entryAssemblyDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)!;
            return Path.Combine(entryAssemblyDirectory, _optionsMonitor.CurrentValue.FilePath ?? string.Empty);
        }

        _logger.LogWarning("File path is not set");
        return string.Empty;
    }
}
