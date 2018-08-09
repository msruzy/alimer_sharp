// Copyright (c) Amer Koleci and contributors.
// Licensed under the MIT License.

namespace Vortice
{
    /// <summary>
    /// Defines a method that handles general events.
    /// </summary>
    /// <typeparam name="TSender">The typed sender type.</typeparam>
    /// <param name="sender">The event source.</param>
    public delegate void TypedEventHandler<TSender>(TSender sender);

    /// <summary>
    /// Defines a method that handles general events.
    /// </summary>
    /// <typeparam name="TSender">The typed sender type.</typeparam>
    /// <typeparam name="TResult">The typed event data.</typeparam>
    /// <param name="sender">The event source.</param>
    /// <param name="args">The event data. If there is no event data, this parameter will be null.</param>
    public delegate void TypedEventHandler<TSender, TResult>(TSender sender, TResult args);
}
