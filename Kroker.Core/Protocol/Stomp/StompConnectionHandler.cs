using Microsoft.AspNetCore.Connections;

namespace Kroker.Core.Protocol.Stomp;

public class StompConnectionHandler(
    ILogger<StompConnectionHandler> logger,
    IStompFrameParser frameParser,
    StompProcessor processor)
    : ConnectionHandler
{
    public override async Task OnConnectedAsync(ConnectionContext connection)
    {
        logger.LogDebug("Connection {ConnectionId} connected", connection.ConnectionId);
        var input = connection.Transport.Input;
        while (true)
        {
            var result = await input.ReadAsync();
            var buffer = result.Buffer;

            if (frameParser.TryParseFrame(ref buffer, out var frame))
            {
                // TODO: process frame
                var processingResult = await ProcessFrame(frame);
                if (processingResult.ShouldDisconnect)
                {
                    break;
                }
            }

            input.AdvanceTo(buffer.Start, buffer.End);
        }

        logger.LogDebug("Connection {ConnectionId} disconnected", connection.ConnectionId);
    }

    private async Task<FrameResult> ProcessFrame(StompFrame frame)
    {
        await processor.Process(frame);
        return frame.Command switch
        {
            Abort abort => throw new NotImplementedException(),
            Ack ack => throw new NotImplementedException(),
            Begin begin => throw new NotImplementedException(),
            Commit commit => throw new NotImplementedException(),
            Connect connect => throw new NotImplementedException(),
            Connected connected => throw new NotImplementedException(),
            Disconnect disconnect => new FrameResult(true),
            Error error => throw new NotImplementedException(),
            Message message => throw new NotImplementedException(),
            Nack nack => throw new NotImplementedException(),
            Send send => throw new NotImplementedException(),
            StompCmd stompCmd => throw new NotImplementedException(),
            Subscribe subscribe => throw new NotImplementedException(),
            Unsubscribe unsubscribe => throw new NotImplementedException(),
            _ => new FrameResult(true)
        };
    }
}