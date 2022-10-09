using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HidApi.Tests;

[TestClass]
public class StringTests
{
    [TestMethod]
    public void TestUnicode()
    {
        var data = new byte[6];
        data[0] = 0;
        data[1] = 1;
        data[2] = 0;
        data[3] = 1;
        data[4] = 0;
        data[5] = 0;

        unsafe
        {
            fixed (byte* p = data)
            {
                var str = Unicode.Read(p);
                str.Should().Be(new string(new[] { '\u0100', '\u0100' }));
            }
        }
    }

    [TestMethod]
    public void TestUtf32()
    {
        var data = new byte[12];
        data[0] = 0;
        data[1] = 1;
        data[2] = 0;
        data[3] = 0;
        data[4] = 0;
        data[5] = 1;
        data[6] = 0;
        data[7] = 0;
        data[8] = 0;
        data[9] = 0;
        data[10] = 0;
        data[11] = 0;

        unsafe
        {
            fixed (byte* p = data)
            {
                var str = Utf32.Read(p);
                str.Should().Be(new string(new[] { '\u0100', '\u0100' }));
            }
        }
    }


    [TestMethod]
    public unsafe void CanCreateNullTerminatedString()
    {
        var result = "MyString";
        var nullTerminatedString = WCharT.CreateNullTerminatedString(result);
        fixed (byte* ptr = nullTerminatedString)
        {
            var r = WCharT.GetString(ptr);
            r.Should().Be(result);
        }
    }
}
