using System.Buffers;
using System.Text;
using Kroker.Core.Protocol.Stomp;

namespace Kroker.Stomp.Tests;

public class FrameParserTests
{
    [Fact]
    public void ParsingCorrectFrame_ShouldReturnOk()
    {
        var parser = new StompFrameParser();

        var frame = new StompFrame("MESSAGE", new Dictionary<string, string>()
        {
            { "content-type", "application/text" },
            { "content-length", "11" }
        }, "Hello World");

        _ = parser.TryWriteFrame(frame, out var buffer);
        var x = Encoding.UTF8.GetString(buffer.ToArray());
        var f = parser.TryParseFrame(ref buffer, out var result);
        Assert.True(f);
        Assert.NotNull(result);
        Assert.Equal("MESSAGE", result.Command);
        Assert.Equal("application/text", result.Headers["content-type"]);
        Assert.Equal("11", result.Headers["content-length"]);
        Assert.Equal("Hello World", result.Content);
    }
    
    [Fact]
    public void ParsingJsonFrame_ShouldReturnOk()
    {
        var parser = new StompFrameParser();

        var frame = new StompFrame("MESSAGE", new Dictionary<string, string>()
        {
            { "content-type", "application/json" },
            { "content-length", "13" }
        }, "{\"key\":\"value\"}");

        _ = parser.TryWriteFrame(frame, out var buffer);
        var x = Encoding.UTF8.GetString(buffer.ToArray());
        var f = parser.TryParseFrame(ref buffer, out var result);
        Assert.True(f);
        Assert.NotNull(result);
        Assert.Equal("MESSAGE", result.Command);
        Assert.Equal("application/json", result.Headers["content-type"]);
        Assert.Equal("13", result.Headers["content-length"]);
        Assert.Equal("{\"key\":\"value\"}", result.Content);
    }
    
    
    [Fact]
    public void ParsingFrameWithInvalidContentLength_ShouldReturnFalse()
    {
        var parser = new StompFrameParser();

        var frame = new StompFrame("MESSAGE", new Dictionary<string, string>()
        {
            { "content-type", "application/text" },
            { "content-length", "5" }
        }, "Too Long Content");

        _ = parser.TryWriteFrame(frame, out var buffer);
        var x = Encoding.UTF8.GetString(buffer.ToArray());
        var f = parser.TryParseFrame(ref buffer, out var result);
        Assert.False(f);
        Assert.Null(result);
    }
    
    [Fact]
    public void ParsingFrameWithEmptyContent_ShouldReturnOk()
    {
        var parser = new StompFrameParser();

        var frame = new StompFrame("MESSAGE", new Dictionary<string, string>()
        {
            { "content-type", "application/text" },
            { "content-length", "0" }
        }, string.Empty);

        _ = parser.TryWriteFrame(frame, out var buffer);
        var x = Encoding.UTF8.GetString(buffer.ToArray());
        var f = parser.TryParseFrame(ref buffer, out var result);
        Assert.True(f);
        Assert.NotNull(result);
        Assert.Equal("MESSAGE", result.Command);
        Assert.Equal("application/text", result.Headers["content-type"]);
        Assert.Equal("0", result.Headers["content-length"]);
        Assert.Equal(string.Empty, result.Content);
    }
    
    [Fact]
    public void ParsingFrameWithNoHeaders_ShouldReturnOk()
    {
        var parser = new StompFrameParser();

        var frame = new StompFrame("MESSAGE", new Dictionary<string, string>(), "No Headers");

        _ = parser.TryWriteFrame(frame, out var buffer);
        var x = Encoding.UTF8.GetString(buffer.ToArray());
        var f = parser.TryParseFrame(ref buffer, out var result);
        Assert.True(f);
        Assert.NotNull(result);
        Assert.Equal("MESSAGE", result.Command);
        Assert.Empty(result.Headers);
        Assert.Equal("No Headers", result.Content);
    }
}