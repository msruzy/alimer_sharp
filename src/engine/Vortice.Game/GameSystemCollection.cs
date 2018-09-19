// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Collections.Generic;

namespace Vortice
{
    /// <summary>
    /// Defines a collection of <see cref="GameSystem"/>.
    /// </summary>
    public class GameSystemCollection : IGameSystemCollection
    {
        private readonly List<GameSystem> _systems = new List<GameSystem>();

        /// <summary>
        /// Occurs when the <see cref="GameSystem"/> is added.
        /// </summary>
        public event TypedEventHandler<GameSystem> Added;

        /// <summary>
        /// Occurs when the <see cref="GameSystem"/> is removed.
        /// </summary>
        public event TypedEventHandler<GameSystem> Removed;

        public void Add(GameSystem gameSystem)
        {
            Guard.NotNull(gameSystem, nameof(gameSystem));

            if (_systems.Contains(gameSystem))
            {
                throw new ArgumentException($"Cannot add ${nameof(GameSystem)} more than once.");
            }

            _systems.Add(gameSystem);
            OnGameSystemAdded(gameSystem);
        }

        protected virtual void OnGameSystemAdded(GameSystem gameSystem)
        {
            Added?.Invoke(gameSystem);
        }

        protected virtual void OnGameSystemRemoved(GameSystem gameSystem)
        {
            Removed?.Invoke(gameSystem);
        }

        public IEnumerator<GameSystem> GetEnumerator() => _systems.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _systems.GetEnumerator();
    }
}
