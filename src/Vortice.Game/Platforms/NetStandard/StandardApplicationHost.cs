// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Runtime.InteropServices;

namespace Vortice
{
    public abstract partial class GameHost
    {
        public static GameHost Create(Game game)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return new Windows.WindowsApplicationHost(game);
            }

            throw new PlatformNotSupportedException();
        }
    }
}
