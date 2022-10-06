using System.Runtime.InteropServices;

namespace HidApi;

internal class DeviceSafeHandle : SafeHandle
{
    public static readonly DeviceSafeHandle Null = new();
    public override bool IsInvalid => handle == IntPtr.Zero;

    private DeviceSafeHandle() : base(IntPtr.Zero, true) { }

    protected override bool ReleaseHandle()
    {
        NativeMethods.Close(handle);
        return true;
    }
}
