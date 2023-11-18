using System.Reflection;
using System.Runtime.InteropServices;

namespace HidApi;

internal static class NativeMethods
{
    private const string Library = "HidApi";
    private static IntPtr libraryHandle = IntPtr.Zero;

    static NativeMethods()
    {
        NativeLibrary.SetDllImportResolver(typeof(NativeMethods).Assembly, Resolve);
    }

    private static IntPtr Resolve(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
    {
        if (libraryName != Library)
            return IntPtr.Zero;

        if (libraryHandle != IntPtr.Zero)
            return libraryHandle;

        foreach (var library in NativeHidApiLibrary.GetNames())
            if (NativeLibrary.TryLoad(library, assembly, searchPath, out libraryHandle))
                return libraryHandle;

        throw new DllNotFoundException($"Could not find hidapi library tried: {string.Join(", ", NativeHidApiLibrary.GetNames())}");
    }

    public static DeviceSafeHandle Open(ushort vendorId, ushort productId, NullTerminatedString serialNumber)
    {
        unsafe
        {
            fixed (byte* ptr = serialNumber)
            {
                return Open(vendorId, productId, ptr);
            }
        }
    }

    public static int Write(DeviceSafeHandle device, ReadOnlySpan<byte> data)
    {
        return Write(device, ref MemoryMarshal.GetReference(data), (nuint) data.Length);
    }

    public static int ReadTimeOut(DeviceSafeHandle device, ReadOnlySpan<byte> data, int milliseconds)
    {
        return ReadTimeOut(device, ref MemoryMarshal.GetReference(data), (nuint) data.Length, milliseconds);
    }

    public static int Read(DeviceSafeHandle device, ReadOnlySpan<byte> data)
    {
        return Read(device, ref MemoryMarshal.GetReference(data), (nuint) data.Length);
    }

    public static int GetManufacturerString(DeviceSafeHandle device, ReadOnlySpan<byte> buffer)
    {
        return GetManufacturerString(device, ref MemoryMarshal.GetReference(buffer), (nuint) buffer.Length);
    }

    public static int GetProductString(DeviceSafeHandle device, ReadOnlySpan<byte> buffer)
    {
        return GetProductString(device, ref MemoryMarshal.GetReference(buffer), (nuint) buffer.Length);
    }

    public static int GetSerialNumberString(DeviceSafeHandle device, ReadOnlySpan<byte> buffer)
    {
        return GetSerialNumberString(device, ref MemoryMarshal.GetReference(buffer), (nuint) buffer.Length);
    }

    public static int GetIndexedString(DeviceSafeHandle device, int index, ReadOnlySpan<byte> buffer)
    {
        return GetIndexedString(device, index, ref MemoryMarshal.GetReference(buffer), (nuint) buffer.Length);
    }

    public static int SendFeatureReport(DeviceSafeHandle device, ReadOnlySpan<byte> data)
    {
        return SendFeatureReport(device, ref MemoryMarshal.GetReference(data), (nuint) data.Length);
    }

    public static int GetFeatureReport(DeviceSafeHandle device, ReadOnlySpan<byte> data)
    {
        return GetFeatureReport(device, ref MemoryMarshal.GetReference(data), (nuint) data.Length);
    }

    public static int GetInputReport(DeviceSafeHandle device, ReadOnlySpan<byte> data)
    {
        return GetInputReport(device, ref MemoryMarshal.GetReference(data), (nuint) data.Length);
    }

    public static int GetReportDescriptor(DeviceSafeHandle device, ReadOnlySpan<byte> buf)
    {
        return GetReportDescriptor(device, ref MemoryMarshal.GetReference(buf), (nuint) buf.Length);
    }

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

    [DllImport(Library, EntryPoint = "hid_open_path")]
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
    public static extern ref ApiVersion Version();

    [DllImport(Library, EntryPoint = "hid_version_str")]
    public static extern IntPtr VersionString();
}
