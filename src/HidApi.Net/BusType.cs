namespace HidApi;

/// <summary>
/// An enum describing the different bus types.
/// </summary>
/// <remarks>Available since hidapi 0.13.0</remarks>
public enum BusType
{
    Unknown = 0x00,
    Usb = 0x01,
    Bluetooth = 0x02,
    I2C = 0x03,
    Spi = 0x04
}
