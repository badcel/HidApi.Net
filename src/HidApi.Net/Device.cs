using WCharT;

namespace HidApi;

/// <summary>
/// Represents a HID device.
/// </summary>
/// <remarks>Call Dispose to free all unmanaged resources.</remarks>
public sealed class Device : IDisposable
{
    private readonly DeviceSafeHandle handle;

    /// <summary>
    /// Connects to a given device.
    /// </summary>
    /// <param name="vendorId">Vendor id of target device</param>
    /// <param name="productId">product id of target device</param>
    /// <exception cref="HidException">Raised on failure</exception>
    public Device(ushort vendorId, ushort productId)
    {
        handle = NativeMethods.Open(vendorId, productId, new WCharTString(string.Empty));

        if (handle.IsInvalid)
            HidException.Throw(handle);
    }

    /// <summary>
    /// Connects to a given device.
    /// </summary>
    /// <param name="vendorId">Vendor id of target device</param>
    /// <param name="productId">Product id of target device</param>
    /// <param name="serialNumber">Serial number of target device</param>
    /// <exception cref="HidException">Raised on failure</exception>
    public Device(ushort vendorId, ushort productId, string serialNumber)
    {
        handle = NativeMethods.Open(vendorId, productId, new WCharTString(serialNumber));

        if (handle.IsInvalid)
            HidException.Throw(handle);
    }

    /// <summary>
    /// Connects to a given device.
    /// </summary>
    /// <param name="path">Path to the device</param>
    /// <exception cref="HidException">Raised on failure</exception>
    public Device(string path)
    {
        handle = NativeMethods.OpenPath(path);

        if (handle.IsInvalid)
            HidException.Throw(handle);
    }

    /// <summary>
    /// Write an output report to the device.
    /// </summary>
    /// <param name="data">Data to send. The first byte must contain the report id or 0x00 if the device only supports one report</param>
    /// <exception cref="HidException">Raised on failure</exception>
    public void Write(ReadOnlySpan<byte> data)
    {
        var result = NativeMethods.Write(handle, data);

        if (result == -1)
            HidException.Throw(handle);
    }

    /// <summary>
    /// Returns an input report.
    /// </summary>
    /// <param name="maxLength">Max length of the expected data. The value can be greater than the actual report.</param>
    /// <param name="milliseconds">timeout in milliseconds. -1 for blocking mode.</param>
    /// <returns>The received data of the HID device. If the timeout is exceeded an empty result is returned.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Raised if maxlength is smaller than 0</exception>
    /// <exception cref="HidException">Raised on failure</exception>
    public ReadOnlySpan<byte> ReadTimeout(int maxLength, int milliseconds)
    {
        if (maxLength < 0)
            throw new ArgumentOutOfRangeException(nameof(maxLength), maxLength, "Please provide a positive value");

        var buffer = new Span<byte>(new byte[maxLength]);
        var bytesRead = ReadTimeout(buffer, milliseconds);

        return buffer[..bytesRead];
    }

    /// <summary>
    /// Returns an input report.
    /// </summary>
    /// <param name="buffer">Buffer to write the data into</param>
    /// <param name="milliseconds">timeout in milliseconds. -1 for blocking mode.</param>
    /// <returns>The length of the received data in bytes. If the timeout is exceeded 0 is returned.</returns>
    /// <exception cref="HidException">Raised on failure</exception>
    public int ReadTimeout(Span<byte> buffer, int milliseconds)
    {
        var result = NativeMethods.ReadTimeOut(handle, buffer, milliseconds);

        if (result == -1)
            HidException.Throw(handle);

        return result;
    }

    /// <summary>
    /// Returns an input report.
    /// </summary>
    /// <param name="maxLength">Max length of the expected data. The value can be greater than the actual report.</param>
    /// <returns>The received data of the HID device. If non-blocking mode is enabled and no data is available an empty result will be returned.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Raised if maxlength is smaller than 0</exception>
    /// <exception cref="HidException">Raised on failure</exception>
    public ReadOnlySpan<byte> Read(int maxLength)
    {
        if (maxLength < 0)
            throw new ArgumentOutOfRangeException(nameof(maxLength), maxLength, "Please provide a positive value");

        var buffer = new Span<byte>(new byte[maxLength]);
        var bytesRead = Read(buffer);

        return buffer[..bytesRead];
    }

    /// <summary>
    /// Returns an input report.
    /// </summary>
    /// <param name="buffer">Buffer to write the data into</param>
    /// <returns>The length of the received data in bytes. If non-blocking mode is enabled and no data is available 0 will be returned.</returns>
    /// <exception cref="HidException">Raised on failure</exception>
    public int Read(Span<byte> buffer)
    {
        var result = NativeMethods.Read(handle, buffer);

        if (result == -1)
            HidException.Throw(handle);

        return result;
    }

    /// <summary>
    /// In blocking mode a call to <see cref="Read" /> will block until some data is available. In Non blocking mode <see cref="Read"/> will return immediately without data.
    /// </summary>
    /// <param name="setNonBlocking">true: Enable non blocking mode.
    /// false: Disable non blocking mode.</param>
    /// <exception cref="HidException">Raised on failure</exception>
    public void SetNonBlocking(bool setNonBlocking)
    {
        var result = NativeMethods.SetNonBlocking(handle, setNonBlocking ? 1 : 0);

        if (result == -1)
            HidException.Throw(handle);
    }

    /// <summary>
    /// Sends a feature report to the device.
    /// </summary>
    /// <param name="data">The data which should be sent to the device. The first byte must contain the report id or 0x0 if the device does not use numbered reports.</param>
    /// <exception cref="HidException">Raised on failure</exception>
    public void SendFeatureReport(ReadOnlySpan<byte> data)
    {
        var result = NativeMethods.SendFeatureReport(handle, data);

        if (result == -1)
            HidException.Throw(handle);
    }

    /// <summary>
    /// Returns the feature report for a given report id.
    /// </summary>
    /// <param name="reportId">Report id</param>
    /// <param name="maxLength">Max length of the expected data. The value can be greater than the actual report.</param>
    /// <returns>The received data of the HID device.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Raised if maxLength is smaller than 1</exception>
    /// <exception cref="HidException">Raised on failure</exception>
    public ReadOnlySpan<byte> GetFeatureReport(byte reportId, int maxLength)
    {
        if (maxLength < 1)
            throw new ArgumentOutOfRangeException(nameof(maxLength), maxLength, "Please provide a value greater than 1");

        var data = new Span<byte>(new byte[maxLength]);
        data[0] = reportId;
        var result = NativeMethods.GetFeatureReport(handle, data);

        if (result == -1)
            HidException.Throw(handle);

        return data[..result];
    }

    /// <summary>
    /// Returns the input report for a given report id.
    /// </summary>
    /// <param name="reportId">Report id</param>
    /// <param name="maxLength">Max length of the expected data. The value can be greater than the actual report.</param>
    /// <returns>The received data of the HID device.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Raised if maxLength is smaller than 1</exception>
    /// <exception cref="HidException">Raised on failure</exception>
    /// <remarks>Available since hidapi 0.10.0</remarks>
    public ReadOnlySpan<byte> GetInputReport(byte reportId, int maxLength)
    {
        if (maxLength < 1)
            throw new ArgumentOutOfRangeException(nameof(maxLength), maxLength, "Please provide a value greater than 1");

        var data = new Span<byte>(new byte[maxLength]);
        data[0] = reportId;
        var result = NativeMethods.GetInputReport(handle, data);

        if (result == -1)
            HidException.Throw(handle);

        return data[..result];
    }

    /// <summary>
    /// Returns the manufacturer.
    /// </summary>
    /// <param name="maxLength">Max length of the returned manufacturer string</param>
    /// <returns>A string containing the name of the manufacturer. The length is limited to <paramref name="maxLength" /> characters.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Raised if maxlength is smaller than 0</exception>
    /// <exception cref="HidException">Raised on failure</exception>
    /// <exception cref="OverflowException">If the requested string length is too large to be created by the dotnet runtime.</exception>
    public string GetManufacturer(int maxLength = 128)
    {
        if (maxLength < 0)
            throw new ArgumentOutOfRangeException(nameof(maxLength), maxLength, "Please provide a positive value");

        checked { maxLength += 1; } //Increase maxLength by one to care for null termination character

        var buffer = new WCharTString(maxLength);
        var result = NativeMethods.GetManufacturerString(handle, buffer, maxLength);

        if (result == -1)
            HidException.Throw(handle);

        return buffer.GetString().TrimEnd((char) 0);
    }

    /// <summary>
    /// Returns the product.
    /// </summary>
    /// <param name="maxLength">Max length of the returned product string</param>
    /// <returns>A string containing the name of the product. The length is limited to <paramref name="maxLength" /> characters.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Raised if maxlength is smaller than 0</exception>
    /// <exception cref="HidException">Raised on failure</exception>
    /// <exception cref="OverflowException">If the requested string length is too large to be created by the dotnet runtime.</exception>
    public string GetProduct(int maxLength = 128)
    {
        if (maxLength < 0)
            throw new ArgumentOutOfRangeException(nameof(maxLength), maxLength, "Please provide a positive value");

        checked { maxLength += 1; } //Increase maxLength by one to care for null termination character

        var buffer = new WCharTString(maxLength);
        var result = NativeMethods.GetProductString(handle, buffer, maxLength);

        if (result == -1)
            HidException.Throw(handle);

        return buffer.GetString().TrimEnd((char) 0);
    }

    /// <summary>
    /// Returns the serial number.
    /// </summary>
    /// <param name="maxLength">Max length of the returned serial number string</param>
    /// <returns>A string containing the serial number. The length is limited to <paramref name="maxLength" /> characters.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Raised if maxlength is smaller than 0</exception>
    /// <exception cref="HidException">Raised on failure</exception>
    /// <exception cref="OverflowException">If the requested string length is too large to be created by the dotnet runtime.</exception>
    public string GetSerialNumber(int maxLength = 128)
    {
        if (maxLength < 0)
            throw new ArgumentOutOfRangeException(nameof(maxLength), maxLength, "Please provide a positive value");

        checked { maxLength += 1; } //Increase maxLength by one to care for null termination character

        var buffer = new WCharTString(maxLength);
        var result = NativeMethods.GetSerialNumberString(handle, buffer, maxLength);

        if (result == -1)
            HidException.Throw(handle);

        return buffer.GetString().TrimEnd((char) 0);
    }

    /// <summary>
    /// Returns the device info for a device.
    /// </summary>
    /// <returns><see cref="DeviceInfo"/></returns>
    /// <exception cref="HidException">Raised on failure</exception>
    /// <remarks>Available since hidapi 0.13.0</remarks>
    public DeviceInfo GetDeviceInfo()
    {
        unsafe
        {
            var ptr = NativeMethods.GetDeviceInfo(handle);

            if ((IntPtr) ptr == IntPtr.Zero)
                HidException.Throw(handle);

            return DeviceInfo.From(ptr);
        }
    }

    /// <summary>
    /// Returns a string from its index.
    /// </summary>
    /// <param name="stringIndex">The index of the string</param>
    /// <param name="maxLength">Max length of the string</param>
    /// <returns>The string at the given index. The length is limited to <paramref name="maxLength" /> characters.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Raised if maxlength is smaller than 0</exception>
    /// <exception cref="HidException">Raised on failure</exception>
    /// <exception cref="OverflowException">If the requested string length is too large to be created by the dotnet runtime.</exception>
    public string GetIndexedString(int stringIndex, int maxLength = 128)
    {
        if (maxLength < 0)
            throw new ArgumentOutOfRangeException(nameof(maxLength), maxLength, "Please provide a positive value");

        checked { maxLength += 1; } //Increase maxLength by one to care for null termination character

        var buffer = new WCharTString(maxLength);
        var result = NativeMethods.GetIndexedString(handle, stringIndex, buffer, maxLength);

        if (result == -1)
            HidException.Throw(handle);

        return buffer.GetString().TrimEnd((char) 0);
    }

    /// <summary>
    /// Gets the report descriptor of the device.
    /// </summary>
    /// <param name="bufSize">Max length of the expected data.</param>
    /// <returns>The report descriptor</returns>
    /// <remarks>Available since hidapi 0.14.0</remarks>
    /// <exception cref="ArgumentOutOfRangeException">Raised if bufLength is less than 0</exception>
    /// <exception cref="HidException">Raised on failure</exception>
    public ReadOnlySpan<byte> GetReportDescriptor(int bufSize = 4096)
    {
        if (bufSize < 0)
            throw new ArgumentOutOfRangeException(nameof(bufSize), bufSize, "Please provide a positive value");

        var data = new Span<byte>(new byte[bufSize]);
        var result = NativeMethods.GetReportDescriptor(handle, data);

        if (result == -1)
            HidException.Throw(handle);

        return data[..result];
    }

    /// <summary>
    /// Frees all unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        handle.Dispose();
    }
}
