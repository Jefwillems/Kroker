using System.Collections;
using System.Collections.Immutable;
using System.Text;
using Kroker.Core.Extensions;

namespace Kroker.Core.Protocol.Stomp;

public class StompFrame
{
    public ImmutableDictionary<string, string> Headers { get; private set; }
    public string Content { get; private set; }
    public StompCommand Command { get; private set; }

    public StompFrame(StompCommand command, Dictionary<string, string> headers, string body)
    {
        Command = command;
        Headers = headers.ToImmutableDictionary();
        Content = body;
    }
}