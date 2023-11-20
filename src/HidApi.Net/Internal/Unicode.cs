using System.Text;

namespace HidApi;

internal static class Unicode
{
    public static ReadOnlySpan<byte> CreateBuffer(int size)
    {
        return new byte[size * sizeof(ushort)];
    }

    public static unsafe string Read(byte* ptr)
    {
        var data = (IntPtr) ptr == IntPtr.Zero
            ? new ReadOnlySpan<byte>()
            : new ReadOnlySpan<byte>(ptr, Length(ptr));

        return Encoding.Unicode.GetString(data);
    }

    private static unsafe int Length(byte* ptr)
    {
        //check code to throw exception in case of arithmethic overflow
        checked
        {
            var current = (ushort*) ptr;
            while (*current != 0)
            {
                current += 1; //Jump to next unicode char
            }

            return (int) ((byte*) current - ptr);
        }
    }
}
