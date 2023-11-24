#if NET6_0
using System.Runtime.InteropServices;

namespace HidApi;

internal static partial class NativeMethods
{
    [DllImport(Library, EntryPoint = "hid_init")]
    public static extern int Init();

    [DllImport(Library, EntryPoint = "hid_exit")]
    public static extern int Exit();

    [DllImport(Library, EntryPoint = "hid_enumerate")]
    public static extern unsafe NativeDeviceInfo* Enumerate(ushort vendorId, ushort productId);

    [DllImport(Library, EntryPoint = "hid_free_enumeration")]
    public static extern unsafe void FreeEnumeration(NativeDeviceInfo* devices);

    [DllImport(Library, EntryPoint = "hid_open")]
    private static extern unsafe DeviceSafeHandle Open(ushort vendorId, ushort productId, byte* serialNumber);

    [DllImport(Library, EntryPoint = "hid_open_path", BestFitMapping = false)]
    public static extern DeviceSafeHandle OpenPath([MarshalAs(UnmanagedType.LPStr)] string path);

    [DllImport(Library, EntryPoint = "hid_write")]
    private static extern int Write(DeviceSafeHandle device, ref byte data, nuint length);

    [DllImport(Library, EntryPoint = "hid_read_timeout")]
    private static extern int ReadTimeOut(DeviceSafeHandle device, ref byte data, nuint length, int milliseconds);

    [DllImport(Library, EntryPoint = "hid_read")]
    private static extern int Read(DeviceSafeHandle device, ref byte data, nuint length);

    [DllImport(Library, EntryPoint = "hid_set_nonblocking")]
    public static extern int SetNonBlocking(DeviceSafeHandle device, int nonBlock);

    [DllImport(Library, EntryPoint = "hid_send_feature_report")]
    private static extern int SendFeatureReport(DeviceSafeHandle device, ref byte data, nuint length);

    [DllImport(Library, EntryPoint = "hid_get_feature_report")]
    private static extern int GetFeatureReport(DeviceSafeHandle device, ref byte data, nuint length);

    [DllImport(Library, EntryPoint = "hid_get_input_report")]
    private static extern int GetInputReport(DeviceSafeHandle device, ref byte data, nuint length);

    [DllImport(Library, EntryPoint = "hid_close")]
    public static extern void Close(IntPtr device);

    [DllImport(Library, EntryPoint = "hid_get_manufacturer_string")]
    private static extern int GetManufacturerString(DeviceSafeHandle device, ref byte buffer, nuint maxLength);

    [DllImport(Library, EntryPoint = "hid_get_product_string")]
    private static extern int GetProductString(DeviceSafeHandle device, ref byte buffer, nuint maxLength);

    [DllImport(Library, EntryPoint = "hid_get_serial_number_string")]
    private static extern int GetSerialNumberString(DeviceSafeHandle device, ref byte buffer, nuint maxLength);

    [DllImport(Library, EntryPoint = "hid_get_device_info")]
    public static extern unsafe NativeDeviceInfo* GetDeviceInfo(DeviceSafeHandle device);

    [DllImport(Library, EntryPoint = "hid_get_indexed_string")]
    private static extern int GetIndexedString(DeviceSafeHandle device, int stringIndex, ref byte buffer, nuint maxLength);

    [DllImport(Library, EntryPoint = "hid_get_report_descriptor")]
    private static extern int GetReportDescriptor(DeviceSafeHandle device, ref byte buf, nuint bufSize);

    [DllImport(Library, EntryPoint = "hid_error")]
    public static extern unsafe byte* Error(DeviceSafeHandle device);

    [DllImport(Library, EntryPoint = "hid_version")]
    public static extern unsafe ApiVersion* Version();

    [DllImport(Library, EntryPoint = "hid_version_str")]
    public static extern IntPtr VersionString();
}
#endif
