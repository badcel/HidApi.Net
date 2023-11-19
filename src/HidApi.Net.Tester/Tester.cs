using System.Runtime.InteropServices;

namespace HidApi.Net.Tester;

public static class Tester
{
    public static int Test(OSPlatform? requiredOsPlatform, bool? require64Bit, bool? require32Bit)
    {
        LogSystemData();

        if (!CheckOsPlatform(requiredOsPlatform))
            return 1;

        if (!Check32BitOs(require32Bit))
            return 1;

        if (!Check64BitOs(require64Bit))
            return 1;

        var verifier = GetVerifier();
        if (!verifier.VerifyCharPointer())
            return 1;

        if (!verifier.VerifyWCharPointer())
            return 1;

        Console.WriteLine("Success, all tests valid.");
        return 0;
    }

    private static void LogSystemData()
    {
        Console.WriteLine($"OS Platform: {GetOsPlatform()}");
        Console.WriteLine($"64 bit OS: {Environment.Is64BitOperatingSystem}");
    }

    private static bool CheckOsPlatform(OSPlatform? requiredOsPlatform)
    {
        if (requiredOsPlatform is null)
            return true;

        if (GetOsPlatform() == requiredOsPlatform)
            return true;

        Console.WriteLine($"Error: OS is {GetOsPlatform()} but option requires it to be {requiredOsPlatform}");

        return false;
    }

    private static bool Check32BitOs(bool? require32Bit)
    {
        if (require32Bit is null or false)
            return true;

        if (!Environment.Is64BitOperatingSystem)
            return true;

        Console.WriteLine("Error: OS is required to be 32 bit.");

        return false;
    }

    private static bool Check64BitOs(bool? require64Bit)
    {
        if (require64Bit is null or false)
            return true;

        if (Environment.Is64BitOperatingSystem)
            return true;

        Console.WriteLine("Error: OS is required to be 64 bit.");

        return false;
    }

    private static IHidApiVerifier GetVerifier()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return new Linux();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return new Windows();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return new Osx();

        throw new NotSupportedException("No verifier available for os platform.");
    }

    private static OSPlatform GetOsPlatform()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return OSPlatform.Linux;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return OSPlatform.Windows;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return OSPlatform.OSX;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
            return OSPlatform.FreeBSD;

        throw new NotSupportedException("Unknown OsPlatform");
    }
}
