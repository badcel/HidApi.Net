using System.Text;

namespace HidApi;

internal static class Unicode
{
    private const int UnicodeSize = 2;

    public static ReadOnlySpan<byte> CreateBuffer(int size)
    {
        return new byte[size * UnicodeSize];
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
            var current = ptr;
            while (*current != 0)
            {
                current += UnicodeSize; //Jump to next unicode char
            }

            return (int) (current - ptr);
        }
    }
}
