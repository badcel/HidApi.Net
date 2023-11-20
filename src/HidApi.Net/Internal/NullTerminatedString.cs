using System.Text;

namespace HidApi;

internal readonly ref struct NullTerminatedString
{
    public static NullTerminatedString Empty => new();

    private readonly ReadOnlySpan<byte> data;

    public NullTerminatedString()
    {
        data = ReadOnlySpan<byte>.Empty;
    }

    private NullTerminatedString(ReadOnlySpan<byte> data)
    {
        this.data = data;
    }

    internal static NullTerminatedString WithUnicode(string str)
    {
        var src = Encoding.Unicode.GetBytes(str);
        var dest = new byte[src.Length + sizeof(ushort)];
        Array.Copy(src, dest, src.Length);

        return new NullTerminatedString(dest);
    }

    internal static NullTerminatedString WithUtf32(string str)
    {
        var src = Encoding.UTF32.GetBytes(str);
        var dest = new byte[src.Length + sizeof(uint)];
        Array.Copy(src, dest, src.Length);

        return new NullTerminatedString(dest);
    }

    public ref readonly byte GetPinnableReference() => ref data.GetPinnableReference();
}
