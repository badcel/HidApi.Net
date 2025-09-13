using System.Collections;

namespace HidApi;

public static partial class Hid
{
    /// <summary>
    /// An enumerable collection of <see cref="DeviceInfo"/>.
    /// </summary>
    /// <param name="vendorId">Vendor id of devices to open or 0 to match any vendor</param>
    /// <param name="productId">Product id of devices to open or 0 to match any product</param>
    public readonly struct DeviceInfos(ushort vendorId = 0, ushort productId = 0) : IEnumerable<DeviceInfo>
    {
        public Enumerator GetEnumerator()
        {
            return new Enumerator(vendorId, productId);
        }

        IEnumerator<DeviceInfo> IEnumerable<DeviceInfo>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public unsafe struct Enumerator : IEnumerator<DeviceInfo>
        {
            private NativeDeviceInfo* DeviceInfoPointer { get; }
            private NativeDeviceInfo* CurrentDeviceInfoPointer { get; set; }

            object? IEnumerator.Current => Current;
            public DeviceInfo Current { get; private set; } = null!;

            public Enumerator(ushort vendorId, ushort productId)
            {
                DeviceInfoPointer = NativeMethods.Enumerate(vendorId, productId);
                CurrentDeviceInfoPointer = DeviceInfoPointer;
            }

            public bool MoveNext()
            {
                if ((IntPtr) CurrentDeviceInfoPointer == IntPtr.Zero)
                    return false;

                Current = DeviceInfo.From(CurrentDeviceInfoPointer);
                CurrentDeviceInfoPointer = CurrentDeviceInfoPointer->Next;

                return true;
            }

            void IEnumerator.Reset()
            {
                Current = null!;
                CurrentDeviceInfoPointer = DeviceInfoPointer;
            }

            void IDisposable.Dispose()
            {
                NativeMethods.FreeEnumeration(DeviceInfoPointer);
            }
        }
    }
}
