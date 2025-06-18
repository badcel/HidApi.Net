using System.Runtime.InteropServices;

namespace HidApi;

internal class DeviceSafeHandle() : SafeHandle(IntPtr.Zero, true)
{
    public static readonly DeviceSafeHandle Null = new();
    public override bool IsInvalid => handle == IntPtr.Zero;

    protected override bool ReleaseHandle()
    {
        NativeMethods.Close(handle);
        return true;
    }
}
