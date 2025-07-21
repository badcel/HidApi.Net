namespace HidApi;

public unsafe struct HidEnumerator : IDisposable
{
    private NativeDeviceInfo* _currentNativeInfo;
    public DeviceInfo Current { get; private set; }

    public HidEnumerator GetEnumerator() => this;

    public HidEnumerator(ushort vendorId, ushort productId)
    {
        _currentNativeInfo = NativeMethods.Enumerate(vendorId, productId);
        Current = DeviceInfo.From(_currentNativeInfo);
    }

    public bool MoveNext()
    {
        _currentNativeInfo = _currentNativeInfo->Next;

        if ((nint) _currentNativeInfo != nint.Zero)
        {
            Current = DeviceInfo.From(_currentNativeInfo);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Dispose()
    {
        NativeMethods.FreeEnumeration(_currentNativeInfo);
    }
}
