using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;

namespace HidApi.Benchmark;

public class VersionStruct
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct ApiVersion
    {
        public readonly int Major;
        public readonly int Minor;
        public readonly int Path;
    }

    [DllImport(Library.Name, EntryPoint = "hid_version")]
    private static extern ref ApiVersion VersionRefStruct();

    [DllImport(Library.Name, EntryPoint = "hid_version")]
    private static extern IntPtr Version();

    [Benchmark]
    public ref ApiVersion GetVersionRefStruct()
    {
        return ref VersionRefStruct();
    }

    [Benchmark]
    public ApiVersion GetVersionStruct()
    {
        var ptr = Version();
        return Marshal.PtrToStructure<ApiVersion>(ptr);
    }
}
