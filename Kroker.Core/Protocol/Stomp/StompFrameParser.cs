using System.Buffers;
using System.Collections;
using System.Text;
using Kroker.Core.Extensions;

namespace Kroker.Core.Protocol.Stomp;

public class StompFrameParser : IStompFrameParser, IStompFrameWriter
{
    public bool TryParseFrame(ref ReadOnlySequence<byte> buffer, out StompFrame? frame)
    {
        var reader = new BufferReader(buffer);
        var command = ReadLineTerminatedString(ref reader);
        var headers = ParseHeaders(ref reader);

        // TODO: handle null octets and content-length headers
        var body = ParseBody(ref reader);

        frame = new StompFrame(command, headers, body);
        return true;
    }

    private static string ReadLineTerminatedString(ref BufferReader reader)
    {
        var crlfOffsetFromCurrent = BufferReader.FindNextCrLf(reader);
        if (crlfOffsetFromCurrent.Item1 < 0) return null;
        var payload = reader.ConsumeAsBuffer(crlfOffsetFromCurrent.Item1);
        var consumeEndCount = crlfOffsetFromCurrent.Item2 switch
        {
            LineEndFound.Lf => 1,
            LineEndFound.CrLf => 2,
            LineEndFound.None => 0,
            _ => throw new ArgumentOutOfRangeException()
        };
        reader.Consume(consumeEndCount);
        return Encoding.UTF8.GetString(payload);
    }

    private static Dictionary<string, string> ParseHeaders(ref BufferReader reader)
    {
        var lines = new List<string>();
        var line = ReadLineTerminatedString(ref reader);
        while (!string.IsNullOrWhiteSpace(line))
        {
            lines.Add(line);
            line = ReadLineTerminatedString(ref reader);
        }

        var headerTable = new Dictionary<string, string>();

        foreach (var headerLine in lines)
        {
            var parts = headerLine.Split(':', 2);
            if (parts.Length < 2) continue;
            var key = parts[0].TrimEnd('\r').Trim();
            var value = parts[1].TrimEnd('\r').Trim();
            headerTable[key] = value;
        }

        return headerTable;
    }

    private static string ParseBody(ref BufferReader reader)
    {
        var lines = new List<string>();
        var line = ReadLineTerminatedString(ref reader);
        while (!string.IsNullOrWhiteSpace(line))
        {
            lines.Add(line);
            line = ReadLineTerminatedString(ref reader);
        }

        return string.Join("", lines);
    }


    public bool TryWriteFrame(StompFrame frame, out ReadOnlySequence<byte> buffer)
    {
        var sb = new StringBuilder();
        sb.Append(frame.Command.ToStompString());
        sb.Append('\n');

        foreach (var (key, value) in frame.Headers)
        {
            sb.Append(key);
            sb.Append(':');
            sb.Append(value);
            sb.Append('\n');
        }

        sb.Append('\n');
        sb.Append(frame.Content);
        sb.Append('\n');
        sb.Append('\0');

        var bytes = Encoding.UTF8.GetBytes(sb.ToString());
        buffer = new ReadOnlySequence<byte>(bytes);
        return true;
    }
}