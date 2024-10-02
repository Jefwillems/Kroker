using System.Buffers;

namespace Kroker.Core.Protocol.Stomp;

public interface IStompFrameParser
{
    bool TryParseFrame(ref ReadOnlySequence<byte> buffer, out StompFrame? frame);
}