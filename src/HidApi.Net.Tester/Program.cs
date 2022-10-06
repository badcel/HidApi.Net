using System.CommandLine;
using System.Runtime.InteropServices;
using HidApi.Net.Tester;

var requireOsPlatformOption = new Option<Platform?>(
    name: "--requireOsPlatform",
    description: "Verifies the os platform against the given one",
    getDefaultValue: () => null
);

var require64BitOption = new Option<bool?>(
    name: "--require64bit",
    description: "Verifies that the os has 64 bits",
    getDefaultValue: () => null
);

var require32BitOption = new Option<bool?>(
    name: "--require32bit",
    description: "Verifies that the os has 32 bits",
    getDefaultValue: () => null
);

var rootCommand = new RootCommand("Test HidApi.Net bindings");
rootCommand.AddOption(requireOsPlatformOption);
rootCommand.AddOption(require32BitOption);
rootCommand.AddOption(require64BitOption);
rootCommand.SetHandler(context =>
{
    var requiredPlatform = context.ParseResult.GetValueForOption(requireOsPlatformOption);
    OSPlatform? osPlatform = !requiredPlatform.HasValue ? null : OSPlatform.Create(requiredPlatform.Value.ToString());

    context.ExitCode = Tester.Test(
        requiredOsPlatform: osPlatform,
        require32Bit: context.ParseResult.GetValueForOption(require32BitOption),
        require64Bit: context.ParseResult.GetValueForOption(require64BitOption)
    );
});

return rootCommand.Invoke(args);
