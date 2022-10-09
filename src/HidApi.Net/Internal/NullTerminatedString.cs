namespace HidApi;

internal readonly ref struct NullTerminatedString
{
    public static NullTerminatedString Empty => new();

    private readonly ReadOnlySpan<byte> data;

    public NullTerminatedString()
    {
        data = ReadOnlySpan<byte>.Empty;
    }

    internal NullTerminatedString(ref byte[] str)
    {
        data = str;
    }

    public ref readonly byte GetPinnableReference() => ref data.GetPinnableReference();
}
