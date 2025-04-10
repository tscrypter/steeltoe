// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information.

using Steeltoe.Management.Endpoint.Actuators.Info;

namespace Steeltoe.Management.Endpoint.Test.Actuators.Info;

public sealed class InfoBuilderTest : BaseTest
{
    [Fact]
    public void ReturnsEmptyDictionary()
    {
        var builder = new InfoBuilder();
        IDictionary<string, object> built = builder.Build();
        Assert.NotNull(built);
        Assert.Empty(built);
    }

    [Fact]
    public void WithInfoSingleValueAddsValue()
    {
        var builder = new InfoBuilder();
        builder.WithInfo("foo", "bar");
        IDictionary<string, object> built = builder.Build();
        Assert.NotNull(built);
        Assert.Single(built);
        Assert.Equal("bar", built["foo"]);
    }

    [Fact]
    public void WithInfoDictionaryAddsValues()
    {
        var builder = new InfoBuilder();

        var items = new Dictionary<string, object>
        {
            ["foo"] = "bar",
            ["bar"] = 100
        };

        builder.WithInfo(items);
        IDictionary<string, object> built = builder.Build();
        Assert.NotNull(built);
        Assert.Equal(2, built.Count);
        Assert.Equal("bar", built["foo"]);
        Assert.Equal(100, built["bar"]);
    }
}
