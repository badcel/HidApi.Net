using System.Runtime.InteropServices;

namespace HidApi;

/// <summary>
/// Describes a HID device.
/// </summary>
/// <param name="Path">Path of the device</param>
/// <param name="VendorId">Vendor id</param>
/// <param name="ProductId">Product id</param>
/// <param name="SerialNumber">Serial Number</param>
/// <param name="ReleaseNumber">Release number</param>
/// <param name="ManufacturerString">Manufacturer string</param>
/// <param name="ProductString">Product string</param>
/// <param name="UsagePage">Usage page</param>
/// <param name="Usage">Usage</param>
/// <param name="InterfaceNumber">interface number</param>
#if HIDAPI_0130
/// <param name="BusType"><see cref="BusType"/> (Available since hidapi 0.13.0)</param>
#endif
public record DeviceInfo(
    string Path
    , ushort VendorId
    , ushort ProductId
    , string SerialNumber
    , ushort ReleaseNumber
    , string ManufacturerString
    , string ProductString
    , ushort UsagePage
    , ushort Usage
    , int InterfaceNumber
#if HIDAPI_0130
    , BusType BusType
#endif
)
{
    /// <summary>
    /// Connects to the device defined by the 'Path' property.
    /// </summary>
    /// <returns>A new <see cref="Device"/></returns>
    public Device ConnectToDevice()
    {
        return new Device(Path);
    }

    internal static unsafe DeviceInfo From(NativeDeviceInfo* nativeDeviceInfo)
    {
        return new DeviceInfo(
            Path: Marshal.PtrToStringAnsi((IntPtr) nativeDeviceInfo->Path) ?? string.Empty
            , VendorId: nativeDeviceInfo->VendorId
            , ProductId: nativeDeviceInfo->ProductId
            , SerialNumber: WCharT.GetString(nativeDeviceInfo->SerialNumber)
            , ReleaseNumber: nativeDeviceInfo->ReleaseNumber
            , ManufacturerString: WCharT.GetString(nativeDeviceInfo->ManufacturerString)
            , ProductString: WCharT.GetString(nativeDeviceInfo->ProductString)
            , UsagePage: nativeDeviceInfo->UsagePage
            , Usage: nativeDeviceInfo->Usage
            , InterfaceNumber: nativeDeviceInfo->InterfaceNumber
#if HIDAPI_0130
            , BusType: nativeDeviceInfo->BusType
#endif
        );
    }
}
