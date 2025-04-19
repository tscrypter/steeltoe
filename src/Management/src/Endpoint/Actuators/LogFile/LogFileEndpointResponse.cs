// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using System.Text;

namespace Steeltoe.Management.Endpoint.Actuators.LogFile;

public class LogFileEndpointResponse(IAsyncEnumerable<byte[]>? content, long? contentLength, long? rangeStartByte, long? rangeEndByte, long? totalFileSize, DateTime? lastModified)
{
    public IAsyncEnumerable<byte[]>? Content { get; } = content;
    public long? ContentLength { get; } = contentLength;
    public long? RangeStartByte { get; } = rangeStartByte;
    public long? RangeEndByte { get; } = rangeEndByte;
    public long? TotalFileSize { get; } = totalFileSize;
    public DateTime? LastModified { get; } = lastModified;
    public LogFileEndpointResponse()
        : this(null, null, null, null, null, null)
    {
    }
}
