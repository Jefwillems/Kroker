using System.Runtime.CompilerServices;

namespace Kroker.Core.Extensions;

public enum LineEndFound
{
    Lf,
    CrLf,
    None
}

public static class SpanExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int VectorSafeIndexOf(this ReadOnlySpan<byte> span, byte value)
        => span.IndexOf(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int VectorSafeIndexOfCrlf(this ReadOnlySpan<byte> span)
        => span.IndexOf(CRLF);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (int, LineEndFound) VectorSafeIndexOfNewlineOrCrlf(this ReadOnlySpan<byte> span)
    {
        var index = span.IndexOf(CRLF);
        return index >= (0) ? (index, LineEndFound.CrLf) : (span.IndexOf((byte)'\n'), Cr: LineEndFound.Lf);
    }

    // note that this is *not* actually an array; this is compiled into a .data section
    // (confirmed down to net472, which is the lowest TFM that uses this branch)
    private static ReadOnlySpan<byte> CRLF => new byte[] { (byte)'\r', (byte)'\n' };
}