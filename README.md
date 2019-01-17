# Vortice

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/amerkoleci/vortice/blob/master/LICENSE)
[![Build status](https://ci.appveyor.com/api/projects/status/2343cyoaemt8o4bm?svg=true)](https://ci.appveyor.com/project/amerkoleci/vortice)

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

## Credits

Vortice development, contributions and bugfixes by:

- Amer Koleci

See [Credits](https://github.com/amerkoleci/vortice/blob/master/CREDITS.md) for list of libraries, research, inspirations and assets being used.
