namespace Kroker.Core.Protocol.Stomp;

public interface ICommand;

public abstract record StompCommand : ICommand
{
    public static implicit operator StompCommand(string? cmd)
    {
        return cmd switch
        {
            "SEND" => new Send(),
            "CONNECT" => new Connect(),
            "MESSAGE" => new Message(),
            "ERROR" => new Error(),
            "CONNECTED" => new Connected(),
            "SUBSCRIBE" => new Subscribe(),
            "UNSUBSCRIBE" => new Unsubscribe(),
            "BEGIN" => new Begin(),
            "COMMIT" => new Commit(),
            "ABORT" => new Abort(),
            "ACK" => new Ack(),
            "NACK" => new Nack(),
            "DISCONNECT" => new Disconnect(),
            "STOMP" => new StompCmd(),
            _ => throw new NotSupportedException($"Command {cmd} not supported")
        };
    }
}

public static class CommandExtensions
{
    public static string ToStompString(this StompCommand command)
    {
        return command switch
        {
            Send => "SEND",
            Connect => "CONNECT",
            Message => "MESSAGE",
            Error => "ERROR",
            Connected => "CONNECTED",
            Subscribe => "SUBSCRIBE",
            Unsubscribe => "UNSUBSCRIBE",
            Begin => "BEGIN",
            Commit => "COMMIT",
            Abort => "ABORT",
            Ack => "ACK",
            Nack => "NACK",
            Disconnect => "DISCONNECT",
            StompCmd => "STOMP",
            _ => throw new NotSupportedException($"Command {command} not supported")
        };
    }
}

public record Send : StompCommand;

public record Connect : StompCommand;

public record Message : StompCommand;

public record Error : StompCommand;

public record Connected : StompCommand;

public record Subscribe : StompCommand;

public record Unsubscribe : StompCommand;

public record Begin : StompCommand;

public record Commit : StompCommand;

public record Abort : StompCommand;

public record Ack : StompCommand;

public record Nack : StompCommand;

public record Disconnect : StompCommand;

public record StompCmd : StompCommand;