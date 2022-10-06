using System.Runtime.InteropServices;
using System.Text;

namespace HidApi;

internal static class WCharT
{
    public static string GetString(ReadOnlySpan<byte> buffer)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return Encoding.UTF32.GetString(buffer);

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return Encoding.Unicode.GetString(buffer);

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return Encoding.UTF32.GetString(buffer);

        throw new Exception("Unsupported platform to read from buffer");
    }

    public static unsafe string GetString(byte* ptr)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return Utf32.Read(ptr);

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return Unicode.Read(ptr);

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return Utf32.Read(ptr);

        throw new Exception("Unsupported platform to read from pointer");
    }

    public static ReadOnlySpan<byte> CreateBuffer(int size)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return Utf32.CreateBuffer(size);

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return Unicode.CreateBuffer(size);

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return Utf32.CreateBuffer(size);

        throw new Exception("Unsupported platform to create a buffer");
    }
}
