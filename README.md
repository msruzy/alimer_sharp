# Vortice

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/amerkoleci/vortice/blob/master/LICENSE)
[![Build status](https://ci.appveyor.com/api/projects/status/8bhaolxnq5dl9w4s?svg=true)](https://ci.appveyor.com/project/amerkoleci/vortice)
[![NuGet](https://img.shields.io/nuget/v/Vortice.Runtime.COM.svg)](https://www.nuget.org/packages?q=Tags%3A%22Vortice%22)

**Vortice** is a cross platform 2D and 3D .NET framework implemented in C#.

## Features

- Cross-platform targets .NET Standard/Core 2.0+, .NET Framework and UWP.
- Modern graphics/rendering API using Vulkan, DirectX12 and DirectX11.1.
- CommandBuffer, Command Queue based rendering.

## Planned Features

- Add OpenGL3.2+ and OpenGLES3.0+ backend.
- GLTF 2.0 support.
- Extendible Asset Pipeline.

## Building

- Install [git](https://git-scm.com) and [.NET Core 2.x SDK](https://www.microsoft.com/net/download/core)
  - Ensure they are added to your *PATH* environment variable
- Run the following commands in the terminal/command line:
  - `git clone https://github.com/amerkoleci/vortice.git`
  - `cd vortice`
  - `cd src`
  - `dotnet restore Vortice.sln`
  - `msbuild /p:Configuration=Release Vortice.sln /m` to build entire solution with Release configuration.

## Download

All packages are available as NuGet packages: [![NuGet](https://img.shields.io/nuget/v/Vortice.Runtime.COM.svg)](https://www.nuget.org/packages?q=Tags%3A%22Vortice%22)

Nightly packages can be download by adding the NuGet feed "https://ci.appveyor.com/nuget/vortice" to your `NuGet.config` file:

```xml
 <configuration>
   <packageSources>
     <!-- ... -->
     <add key="myget vortice" value="https://ci.appveyor.com/nuget/vortice" />
     <!-- ... -->
   </packageSources>
</configuration>     
```

## Credits

Vortice development, contributions and bugfixes by:

- Amer Koleci

Uses the following open-source and third-party libraries:

- [Vortice.Windows](https://github.com/amerkoleci/Vortice.Windows)
- [Vk](https://github.com/mellinoe/vk)

Additional inspiration, research or code used:

- Vulkan examples from Sascha Willems (<https://github.com/SaschaWillems/Vulkan>)
- Veldrid (<https://github.com/mellinoe/veldrid>).
- Granite (<https://github.com/Themaister/Granite>)
