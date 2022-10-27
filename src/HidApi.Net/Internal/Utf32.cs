using System.Text;

namespace HidApi;

internal static class Utf32
{
    public static NullTerminatedString CreateNullTerminatedString(string str)
    {
        var src = Encoding.UTF32.GetBytes(str);
        var dest = new byte[src.Length + sizeof(uint)];
        Array.Copy(src, dest, src.Length);

        return new NullTerminatedString(ref dest);
    }

    public static ReadOnlySpan<byte> CreateBuffer(int size)
    {
        return new byte[size * sizeof(uint)];
    }

    public static unsafe string Read(byte* ptr)
    {
        var data = (IntPtr) ptr == IntPtr.Zero
            ? new ReadOnlySpan<byte>()
            : new ReadOnlySpan<byte>(ptr, Length(ptr));

        return Encoding.UTF32.GetString(data);
    }

    private static unsafe int Length(byte* ptr)
    {
        //check code to throw exception in case of arithmethic overflow
        checked
        {
            var current = (uint*) ptr;
            while (*current != 0)
            {
                current += 1; //Jump to next UTF32 char
            }

            return (int) ((byte*) current - ptr);
        }
    }
}
