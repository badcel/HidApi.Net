using System.Reflection;
using System.Runtime.InteropServices;
using WCharT;

namespace HidApi;

internal static partial class NativeMethods
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

    public static unsafe DeviceSafeHandle Open(ushort vendorId, ushort productId, WCharTString serialNumber)
    {
        fixed (byte* ptr = serialNumber)
        {
            return Open(vendorId, productId, ptr);
        }
    }

    public static int Write(DeviceSafeHandle device, ReadOnlySpan<byte> data)
    {
        return Write(device, ref MemoryMarshal.GetReference(data), (nuint) data.Length);
    }

    public static int ReadTimeOut(DeviceSafeHandle device, Span<byte> data, int milliseconds)
    {
        return ReadTimeOut(device, ref MemoryMarshal.GetReference(data), (nuint) data.Length, milliseconds);
    }

    public static int Read(DeviceSafeHandle device, Span<byte> data)
    {
        return Read(device, ref MemoryMarshal.GetReference(data), (nuint) data.Length);
    }

    public static int GetManufacturerString(DeviceSafeHandle device, ReadOnlySpan<byte> buffer, int maxLength)
    {
        return GetManufacturerString(device, ref MemoryMarshal.GetReference(buffer), (nuint) maxLength);
    }

    public static int GetProductString(DeviceSafeHandle device, ReadOnlySpan<byte> buffer, int maxLength)
    {
        return GetProductString(device, ref MemoryMarshal.GetReference(buffer), (nuint) maxLength);
    }

    public static int GetSerialNumberString(DeviceSafeHandle device, ReadOnlySpan<byte> buffer, int maxLength)
    {
        return GetSerialNumberString(device, ref MemoryMarshal.GetReference(buffer), (nuint) maxLength);
    }

    public static int GetIndexedString(DeviceSafeHandle device, int index, ReadOnlySpan<byte> buffer, int maxLength)
    {
        return GetIndexedString(device, index, ref MemoryMarshal.GetReference(buffer), (nuint) maxLength);
    }

    public static int SendFeatureReport(DeviceSafeHandle device, ReadOnlySpan<byte> data)
    {
        return SendFeatureReport(device, ref MemoryMarshal.GetReference(data), (nuint) data.Length);
    }

    public static int GetFeatureReport(DeviceSafeHandle device, Span<byte> data)
    {
        return GetFeatureReport(device, ref MemoryMarshal.GetReference(data), (nuint) data.Length);
    }

    public static int GetInputReport(DeviceSafeHandle device, Span<byte> data)
    {
        return GetInputReport(device, ref MemoryMarshal.GetReference(data), (nuint) data.Length);
    }

    public static int GetReportDescriptor(DeviceSafeHandle device, Span<byte> buf)
    {
        return GetReportDescriptor(device, ref MemoryMarshal.GetReference(buf), (nuint) buf.Length);
    }

#if NET7_0_OR_GREATER

    [LibraryImport(Library, EntryPoint = "hid_init")]
    public static partial int Init();

    [LibraryImport(Library, EntryPoint = "hid_exit")]
    public static partial int Exit();

    [LibraryImport(Library, EntryPoint = "hid_enumerate")]
    public static unsafe partial NativeDeviceInfo* Enumerate(ushort vendorId, ushort productId);

    [LibraryImport(Library, EntryPoint = "hid_free_enumeration")]
    public static unsafe partial void FreeEnumeration(NativeDeviceInfo* devices);

    [LibraryImport(Library, EntryPoint = "hid_open")]
    private static unsafe partial DeviceSafeHandle Open(ushort vendorId, ushort productId, byte* serialNumber);

    [LibraryImport(Library, EntryPoint = "hid_open_path")]
    public static partial DeviceSafeHandle OpenPath([MarshalAs(UnmanagedType.LPStr)] string path);

    [LibraryImport(Library, EntryPoint = "hid_write")]
    private static partial int Write(DeviceSafeHandle device, ref byte data, nuint length);

    [LibraryImport(Library, EntryPoint = "hid_read_timeout")]
    private static partial int ReadTimeOut(DeviceSafeHandle device, ref byte data, nuint length, int milliseconds);

    [LibraryImport(Library, EntryPoint = "hid_read")]
    private static partial int Read(DeviceSafeHandle device, ref byte data, nuint length);

    [LibraryImport(Library, EntryPoint = "hid_set_nonblocking")]
    public static partial int SetNonBlocking(DeviceSafeHandle device, int nonBlock);

    [LibraryImport(Library, EntryPoint = "hid_send_feature_report")]
    private static partial int SendFeatureReport(DeviceSafeHandle device, ref byte data, nuint length);

    [LibraryImport(Library, EntryPoint = "hid_get_feature_report")]
    private static partial int GetFeatureReport(DeviceSafeHandle device, ref byte data, nuint length);

    [LibraryImport(Library, EntryPoint = "hid_get_input_report")]
    private static partial int GetInputReport(DeviceSafeHandle device, ref byte data, nuint length);

    [LibraryImport(Library, EntryPoint = "hid_close")]
    public static partial void Close(IntPtr device);

    [LibraryImport(Library, EntryPoint = "hid_get_manufacturer_string")]
    private static partial int GetManufacturerString(DeviceSafeHandle device, ref byte buffer, nuint maxLength);

    [LibraryImport(Library, EntryPoint = "hid_get_product_string")]
    private static partial int GetProductString(DeviceSafeHandle device, ref byte buffer, nuint maxLength);

    [LibraryImport(Library, EntryPoint = "hid_get_serial_number_string")]
    private static partial int GetSerialNumberString(DeviceSafeHandle device, ref byte buffer, nuint maxLength);

    [LibraryImport(Library, EntryPoint = "hid_get_device_info")]
    public static unsafe partial NativeDeviceInfo* GetDeviceInfo(DeviceSafeHandle device);

    [LibraryImport(Library, EntryPoint = "hid_get_indexed_string")]
    private static partial int GetIndexedString(DeviceSafeHandle device, int stringIndex, ref byte buffer, nuint maxLength);

    [LibraryImport(Library, EntryPoint = "hid_get_report_descriptor")]
    private static partial int GetReportDescriptor(DeviceSafeHandle device, ref byte buf, nuint bufSize);

    [LibraryImport(Library, EntryPoint = "hid_error")]
    public static unsafe partial byte* Error(DeviceSafeHandle device);

    [LibraryImport(Library, EntryPoint = "hid_version")]
    public static unsafe partial ApiVersion* Version();

    [LibraryImport(Library, EntryPoint = "hid_version_str")]
    public static partial IntPtr VersionString();
#endif
}
