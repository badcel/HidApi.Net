using WCharT;

namespace HidApi.Net.Tester;

public class Osx : IHidApiVerifier
{
    public bool VerifyCharPointer()
    {
        var version = Hid.VersionString();
        if (version.Split('.').Length == 3 && version.StartsWith("0.", StringComparison.InvariantCulture))
            return true;

        Console.WriteLine("Error: Could not verify HIDAPI. String seems to have incorrect format.");

        return false;
    }

    public bool VerifyWCharPointer()
    {
        const string Expected = "Success";
        unsafe
        {
            var ptr = NativeMethods.Error(DeviceSafeHandle.Null);
            var result = new WCharTString(ptr).GetString();
            if (result == Expected)
                return true;
        }

        Console.WriteLine("Error: Could not verify HIDAPI. WCharT string seems to have incorrect format.");

        return false;
    }
}
