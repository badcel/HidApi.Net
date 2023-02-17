using System.Runtime.InteropServices;

namespace HidApi;

[StructLayout(LayoutKind.Sequential)]
internal readonly unsafe struct NativeDeviceInfo
{
    public readonly byte* Path;
    public readonly ushort VendorId;
    public readonly ushort ProductId;
    public readonly byte* SerialNumber;
    public readonly ushort ReleaseNumber;
    public readonly byte* ManufacturerString;
    public readonly byte* ProductString;
    public readonly ushort UsagePage;
    public readonly ushort Usage;
    public readonly int InterfaceNumber;
    public readonly NativeDeviceInfo* Next;
    /// <remarks>Available since hidapi 0.13.0</remarks>
    public readonly BusType BusType;
};
