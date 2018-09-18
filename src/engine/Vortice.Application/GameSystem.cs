// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;

namespace Vortice
{
    /// <summary>
    /// Defines base game system class.
    /// </summary>
    public abstract class GameSystem
    {
        public Application Game { get; set; }

        protected GameSystem(Application game)
        {
            Game = game;
        }
    }
}
