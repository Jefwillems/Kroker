using System.Buffers;

namespace Kroker.Core.Protocol.Stomp;

public interface IStompFrameWriter
{
    bool TryWriteFrame(StompFrame frame, out ReadOnlySequence<byte> buffer);
}