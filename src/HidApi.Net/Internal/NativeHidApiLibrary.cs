using System.Runtime.InteropServices;

namespace HidApi;

internal static class NativeHidApiLibrary
{
    public static IEnumerable<string> GetNames()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return GetLinuxNames();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return GetWindowsNames();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return GetOsxNames();

        throw new NotSupportedException("Unsupported platform to get hidapi library name");
    }

    private static IEnumerable<string> GetLinuxNames()
    {
        yield return "libhidapi-hidraw.so.0";
    }

    private static IEnumerable<string> GetWindowsNames()
    {
        yield return "hidapi.dll"; //Official release package
        yield return "libhidapi-0.dll"; //MSYS 2 package
    }

    private static IEnumerable<string> GetOsxNames()
    {
        yield return "libhidapi.dylib";
    }
}
