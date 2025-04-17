// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System.Text;

namespace Steeltoe.Management.Endpoint.Actuators.LogFile;

public class LogFileEndpointResponse(string? content, long? contentLength, long? totalFileSize, DateTime? lastModified)
{
    public string? Content { get; } = content;
    public long? ContentLength { get; } = contentLength;
    public long? TotalFileSize { get; } = totalFileSize;
    public DateTime? LastModified { get; } = lastModified;
    public LogFileEndpointResponse()
        : this(null, null, null, null)
    {
    }
}
