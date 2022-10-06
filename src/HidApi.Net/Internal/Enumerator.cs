namespace HidApi;

internal static class Enumerator
{
    public static IEnumerable<DeviceInfo> Enumerate(ushort vendorId, ushort productId)
    {
        var deviceInfos = new List<DeviceInfo>();
        unsafe
        {
            var deviceInfoPointer = NativeMethods.Enumerate(vendorId, productId);
            try
            {
                var currentDeviceInfoPointer = deviceInfoPointer;

                while ((IntPtr) currentDeviceInfoPointer != IntPtr.Zero)
                {
                    deviceInfos.Add(DeviceInfo.From(currentDeviceInfoPointer));
                    currentDeviceInfoPointer = currentDeviceInfoPointer->Next;
                }
            }
            finally
            {
                NativeMethods.FreeEnumeration(deviceInfoPointer);
            }
        }

        return deviceInfos;
    }
}
