using System.Runtime.InteropServices;
using System.Text;
using BenchmarkDotNet.Attributes;

namespace HidApi.Benchmark;

public class VersionString
{
    [DllImport(Library.Name, EntryPoint = "hid_version_str")]
    private static extern unsafe byte* GetVersionStringUnsafe();

    [DllImport(Library.Name, EntryPoint = "hid_version_str")]
    private static extern IntPtr GetVersionString();

    [Benchmark]
    public string GetVersionUnsafe()
    {
        unsafe
        {
            var ptr = GetVersionStringUnsafe();
            var data = MemoryMarshal.CreateReadOnlySpanFromNullTerminated(ptr);
            return Encoding.UTF8.GetString(data);
        }
    }

    [Benchmark]
    public string GetVersion()
    {
        var ptr = GetVersionString();
        return Marshal.PtrToStringUTF8(ptr) ?? string.Empty;
    }
}
