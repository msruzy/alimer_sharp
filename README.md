# Alimer

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/amerkoleci/alimer_sharp/blob/master/LICENSE)
[![Build status](https://ci.appveyor.com/api/projects/status/17mwmj5jq6aqfj9d?svg=true)](https://ci.appveyor.com/project/amerkoleci/alimer-sharp)
[![NuGet](https://img.shields.io/nuget/v/Alimer.svg)](https://www.nuget.org/packages?q=Tags%3A%22Alimer%22)

**Alimer** is a cross platform 2D and 3D .NET engine implemented in C#.

## Features

- Cross-platform targets .NET Standard 2.0.
- Modern graphics/rendering API using Direct3D12, Vulkan and Direct3D11.1.
- CommandBuffer, Command Queue based rendering.

## Planned Features

- Add Metal rendering backend.
- Add OpenGL3.2+ and OpenGLES3.0+ backend.
- GLTF 2.0 support.
- Extendible Asset Pipeline.

## Building

- Install [git](https://git-scm.com) and [.NET Core 2.x SDK](https://www.microsoft.com/net/download/core)
  - Ensure they are added to your *PATH* environment variable
- Run the following commands in the terminal/command line:
  - `git clone https://github.com/amerkoleci/alimer_sharp`
  - `cd alimer_sharp`
  - `cd src`
  - `dotnet restore Alimer.sln`
  - `msbuild /p:Configuration=Release Alimer.sln /m` to build entire solution with Release configuration.

## Download

All packages are available as NuGet packages: [![NuGet](https://img.shields.io/nuget/v/Alimer.svg)](https://www.nuget.org/packages?q=Tags%3A%22Alimer%22)

Nightly packages can be download by adding the NuGet feed "https://ci.appveyor.com/nuget/alimer" to your `NuGet.config` file:

```xml
 <configuration>
   <packageSources>
     <!-- ... -->
     <add key="myget alimer" value="https://ci.appveyor.com/nuget/alimer" />
     <!-- ... -->
   </packageSources>
</configuration>     
```

## Credits

Alimer development, contributions and bugfixes by:

- Amer Koleci

Uses the following open-source and third-party libraries:

- [Vortice.Windows](https://github.com/amerkoleci/Vortice.Windows)

Additional inspiration, research or code used:

- Vulkan examples from Sascha Willems (<https://github.com/SaschaWillems/Vulkan>)
- Veldrid (<https://github.com/mellinoe/veldrid>).
- Granite (<https://github.com/Themaister/Granite>)
