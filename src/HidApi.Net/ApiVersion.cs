using System.Runtime.InteropServices;

namespace HidApi;

/// <summary>
/// A readonly struct containing the different parts of the HIDAPI version number.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public readonly struct ApiVersion
{
    /// <summary>
    /// Major version number.
    /// </summary>
    public readonly int Major;

    /// <summary>
    /// Minor version number.
    /// </summary>
    public readonly int Minor;

    /// <summary>
    /// Patch version number.
    /// </summary>
    public readonly int Patch;
}
