using Microsoft.AspNetCore.Connections;

namespace Kroker.Core;

public class TcpConnectionHandler : ConnectionHandler
{
    private readonly ILogger<TcpConnectionHandler> _logger;

    public TcpConnectionHandler(ILogger<TcpConnectionHandler> logger)
    {
        _logger = logger;
    }

    public override async Task OnConnectedAsync(ConnectionContext connection)
    {
        _logger.LogDebug("Connection {ConnectionId} connected", connection.ConnectionId);
        while (true)
        {
            var result = await connection.Transport.Input.ReadAsync();
            var buffer = result.Buffer;

            foreach (var segment in buffer)
            {
                // TODO: process the segment, don't echo back
                await connection.Transport.Output.WriteAsync(segment);
            }

            // TODO: don't break on completed, break on processor result
            if (result.IsCompleted)
            {
                break;
            }

            connection.Transport.Input.AdvanceTo(buffer.End);
        }

        _logger.LogDebug("Connection {ConnectionId} disconnected", connection.ConnectionId);
    }
}