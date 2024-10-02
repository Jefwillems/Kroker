namespace Kroker.Core.Protocol.Stomp;

public abstract class StompProcessor(ILogger<StompProcessor> logger)
{
    private readonly ILogger<StompProcessor> _logger = logger;

    public abstract Task Process(StompFrame frame);
    protected abstract Task Stomp(StompFrame frame);
    protected abstract Task Send(StompFrame frame);
    protected abstract Task Connect(StompFrame frame);
    protected abstract Task Message(StompFrame frame);
    protected abstract Task Error(StompFrame frame);
    protected abstract Task Connected(StompFrame frame);
    protected abstract Task Subscribe(StompFrame frame);
    protected abstract Task Unsubscribe(StompFrame frame);
    protected abstract Task Begin(StompFrame frame);
    protected abstract Task Commit(StompFrame frame);
    protected abstract Task Abort(StompFrame frame);
    protected abstract Task Ack(StompFrame frame);
    protected abstract Task Nack(StompFrame frame);
    protected abstract Task Disconnect(StompFrame frame);
}

public class Stomp10(ILogger<Stomp10> logger) : StompProcessor(logger)
{
    public static readonly string[] SupportedVersions = ["1.0"];
    private readonly ILogger<Stomp10> _logger = logger;

    public override async Task Process(StompFrame frame)
    {
        var process = frame.Command switch
        {
            Abort abort => Abort(frame),
            Ack ack => Ack(frame),
            Begin begin => Begin(frame),
            Commit commit => Commit(frame),
            Connect connect => Connect(frame),
            Connected connected => Connected(frame),
            Disconnect disconnect => Disconnect(frame),
            Error error => Error(frame),
            Message message => Message(frame),
            Nack nack => Nack(frame),
            Send send => Send(frame),
            StompCmd stompCmd => Stomp(frame),
            Subscribe subscribe => Subscribe(frame),
            Unsubscribe unsubscribe => Unsubscribe(frame),
            _ => throw new ArgumentOutOfRangeException()
        };
        await process;
    }

    protected override Task Stomp(StompFrame frame)
    {
        throw new NotImplementedException();
    }

    protected override Task Send(StompFrame frame)
    {
        throw new NotImplementedException();
    }

    protected override Task Connect(StompFrame frame)
    {
        throw new NotImplementedException();
    }

    protected override Task Message(StompFrame frame)
    {
        throw new NotImplementedException();
    }

    protected override Task Error(StompFrame frame)
    {
        throw new NotImplementedException();
    }

    protected override Task Connected(StompFrame frame)
    {
        throw new NotImplementedException();
    }

    protected override Task Subscribe(StompFrame frame)
    {
        throw new NotImplementedException();
    }

    protected override Task Unsubscribe(StompFrame frame)
    {
        throw new NotImplementedException();
    }

    protected override Task Begin(StompFrame frame)
    {
        throw new NotImplementedException();
    }

    protected override Task Commit(StompFrame frame)
    {
        throw new NotImplementedException();
    }

    protected override Task Abort(StompFrame frame)
    {
        throw new NotImplementedException();
    }

    protected override Task Ack(StompFrame frame)
    {
        throw new NotImplementedException();
    }

    protected override Task Nack(StompFrame frame)
    {
        throw new NotImplementedException();
    }

    protected override Task Disconnect(StompFrame frame)
    {
        throw new NotImplementedException();
    }
}

public class Stomp11(ILogger<Stomp11> logger) : Stomp10(logger)
{
    public new static readonly string[] SupportedVersions = Stomp10.SupportedVersions.Concat(["1.1"]).ToArray();
    private readonly ILogger<Stomp11> _logger = logger;
}

public class Stomp12(ILogger<Stomp11> logger) : Stomp11(logger)
{
    public new static readonly string[] SupportedVersions = Stomp11.SupportedVersions.Concat(["1.2"]).ToArray();
}