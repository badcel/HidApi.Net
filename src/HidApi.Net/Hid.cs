using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace HidApi;

/// <summary>
/// Global methods to interact with HID devices.
/// </summary>
public static class Hid
{
    /// <summary>
    /// Initializes HIDAPI.
    /// </summary>
    /// <remarks>The initialization happens automatically and must only be invoked manually if multiple threads try to connect devices at the same time.</remarks>
    /// <exception cref="HidException">Thrown if an error occurs</exception>
    public static void Init()
    {
        var result = NativeMethods.Init();
        if (result == -1)
            HidException.Throw(DeviceSafeHandle.Null);
    }

    /// <summary>
    /// Frees all memory of static data.
    /// </summary>
    /// <remarks>Should be called at the end of a programm to avoid memory leaks.</remarks>
    /// <exception cref="HidException">Thrown if an error occurs</exception>
    public static void Exit()
    {
        var result = NativeMethods.Exit();
        if (result == -1)
            HidException.Throw(DeviceSafeHandle.Null);
    }

    /// <summary>
    /// Enuemrate available HID devices.
    /// </summary>
    /// <param name="vendorId">Vendor id of devices to open or 0 to match any vendor</param>
    /// <param name="productId">Product id of devices to open or 0 to match any product</param>
    /// <returns>Enumerable of <seealso cref="DeviceInfo"/></returns>
    public static IEnumerable<DeviceInfo> Enumerate(ushort vendorId = 0, ushort productId = 0)
    {
        return Enumerator.Enumerate(vendorId, productId);
    }

    /// <summary>
    /// Returns the version of HIDAPI.
    /// </summary>
    /// <returns>A string representing the version number</returns>
    /// <remarks>Available since hidapi 0.10.0</remarks>
    public static string VersionString()
    {
        return Marshal.PtrToStringUTF8(NativeMethods.VersionString()) ?? string.Empty;
    }

    /// <summary>
    /// Returns the HIDAPI version.
    /// </summary>
    /// <returns>A reference to <see cref="ApiVersion"/> containing the information</returns>
    /// <remarks>Available since hidapi 0.10.0</remarks>
    public static ref ApiVersion Version()
    {
        unsafe
        {
            return ref Unsafe.AsRef<ApiVersion>(NativeMethods.Version());
        }
    }
}
