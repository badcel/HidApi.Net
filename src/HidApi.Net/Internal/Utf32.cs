using System.Text;

namespace HidApi;

internal static class Utf32
{
    private const int Utf32Size = 4;

    public static ReadOnlySpan<byte> CreateBuffer(int size)
    {
        return new byte[size * Utf32Size];
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
            var current = ptr;
            while (*current != 0)
            {
                current += Utf32Size; //Jump to next UTF32 char
            }

            return (int) (current - ptr);
        }
    }
}
