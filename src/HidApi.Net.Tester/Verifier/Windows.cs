namespace HidApi.Net.Tester;

public class Windows : HidApiVerifier
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
        string? result;
        const string Expected = "hid_error for global errors is not implemented yet";
        unsafe
        {
            var ptr = NativeMethods.Error(DeviceSafeHandle.Null);
            result = WCharT.GetString(ptr);
            if (result == Expected)
                return true;
        }

        Console.WriteLine($"Error: Could not verify HIDAPI. WCharT string seems to have incorrect format. ({result})");

        return false;
    }
}
