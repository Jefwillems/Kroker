namespace Kroker.Core.Protocol.Stomp;

public class StompFrameBuffer : IDisposable, IAsyncDisposable
{
    private MemoryStream _buffer = new MemoryStream();

    private int readPosition = 0;
    private int consumedPosition = 0;


    public void Write(byte[] buffer)
    {
        _buffer.Seek(0, SeekOrigin.End);
        _buffer.Write(buffer, 0, buffer.Length);
        _buffer.Seek(readPosition, SeekOrigin.Begin);
    }

    public byte[] Read(int length)
    {
        var buffer = new byte[length];
        var bytesRead = _buffer.Read(buffer, readPosition, length);
        readPosition += bytesRead;
        return buffer;
    }

    public void Dispose()
    {
        _buffer.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        await _buffer.DisposeAsync();
    }
}