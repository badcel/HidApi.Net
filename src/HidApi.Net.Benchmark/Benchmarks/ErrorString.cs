using System.Runtime.InteropServices;
using System.Text;
using BenchmarkDotNet.Attributes;

namespace HidApi.Benchmark;

public class ErrorString
{
    [DllImport(Library.Name, EntryPoint = "hid_error")]
    private static extern unsafe byte* ErrorUnsafe(IntPtr device);

    [DllImport(Library.Name, EntryPoint = "hid_error")]
    private static extern IntPtr Error(IntPtr device);

    [Benchmark]
    public string GetErrorUnsafe()
    {
        unsafe
        {
            var ptr = ErrorUnsafe(IntPtr.Zero);

            var data = (IntPtr) ptr == IntPtr.Zero
                ? new ReadOnlySpan<byte>()
                : new ReadOnlySpan<byte>(ptr, Length(ptr));

            return Encoding.UTF32.GetString(data);
        }
    }

    private static unsafe int Length(byte* ptr)
    {
        var p = ptr;
        while (*p != 0)
        {
            p += 4;
        }
        return (int) (p - ptr);
    }

    [Benchmark]
    public string GetError()
    {
        var ptr = Error(IntPtr.Zero);

        if (ptr == IntPtr.Zero)
            return string.Empty;

        var counter = 0;
        var chars = new List<int>();
        while (true)
        {
            var value = Marshal.ReadInt32(ptr, counter);
            if (value == 0)
                break;

            counter += 4; //Point to the next four bytes
            chars.Add(value);
        }

        return Encoding.UTF32.GetString(chars
            .SelectMany(BitConverter.GetBytes)
            .ToArray()
        );
    }
}
