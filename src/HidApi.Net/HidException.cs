namespace HidApi;

/// <summary>
/// Raised if a HID specific exception occurs.
/// </summary>
public class HidException : Exception
{
    private HidException(string message) : base(message) { }

    internal static void Throw(DeviceSafeHandle handle)
    {
        unsafe
        {
            var ptr = NativeMethods.Error(handle);
            throw new HidException(WCharT.GetString(ptr));
        }
    }
}
