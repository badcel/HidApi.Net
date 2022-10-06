namespace HidApi.Net.Tester;

public class Osx : HidApiVerifier
{
    public bool VerifyCharPointer()
    {
        var version = Hid.VersionString();
        if (version.Split('.').Length == 3 && version.StartsWith("0."))
            return true;

        Console.WriteLine("Error: Could not verify HIDAPI. String seems to have incorrect format.");

        return false;
    }

    public bool VerifyWCharPointer()
    {
        const string Expected = "hid_error is not implemented yet";
        unsafe
        {
            var ptr = NativeMethods.Error(DeviceSafeHandle.Null);
            var result = WCharT.GetString(ptr);
            if (result == Expected)
                return true;
        }

        Console.WriteLine("Error: Could not verify HIDAPI. WCharT string seems to have incorrect format.");

        return false;
    }
}
