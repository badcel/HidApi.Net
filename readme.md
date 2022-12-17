# HidApi.Net
[![Build Status](https://img.shields.io/github/actions/workflow/status/badcel/HidApi.Net/ci.yml?branch=main)](https://github.com/badcel/HidApi.Net/actions/workflows/ci.yml)[![NuGet](https://img.shields.io/nuget/v/HidApi.Net)](https://www.nuget.org/packages/HidApi.Net/)[![License (MIT)](https://img.shields.io/github/license/badcel/HidApi.Net)](https://github.com/badcel/HidApi.Net/blob/main/license.txt)

Welcome to HidApi.Net a modern .NET 6 cross platform C# binding for the C [HIDAPI] library. Supported platforms are Linux, OSX and Windows.

## Use
To use the library please reference the [nuget package](https://www.nuget.org/packages/HidApi.Net/) in your project. Additionally it is required to either ensure that [HIDAPI] is available on the host system or is distributed as part of your application.

### Code sample
You can use the [Hid class](https://github.com/badcel/HidApi.Net/blob/main/src/HidApi.Net/Hid.cs) to enumerate over devices or directly use the [device class](https://github.com/badcel/HidApi.Net/blob/main/src/HidApi.Net/Device.cs) to connect to a known device:

```csharp
foreach(var deviceInfo in Hid.Enumerate())
{
    using var device = deviceInfo.ConnectToDevice();
    Console.WriteLine(device.GetManufacturer());
}
Hid.Exit(); //Call at the end of your program
```

```csharp
var device = new Device(0x00, 0x00); //Fill vendor id and product id
Console.WriteLine(device.GetManufacturer());
Hid.Exit(); //Call at the end of your program
```

### Linux
In order to access HID devices as an unprivileged user an udev rule must be installed on the host system. Please refer to the sample [udev file][udev] of the [HIDAPI] project.

## Build
To build the solution locally execute the following commands:

```sh
$ git clone https://github.com/badcel/HidApi.Net.git
$ cd HidApi.Net/src
$ dotnet build
```

## Licensing terms
HidApi.Net is licensed under the terms of the MIT-License. Please see the [license file][license] for further information.

[HIDAPI]:https://github.com/libusb/hidapi
[udev]:https://github.com/libusb/hidapi/blob/master/udev/69-hid.rules
[license]:https://raw.githubusercontent.com/badcel/HidApi.Net/main/license.txt
