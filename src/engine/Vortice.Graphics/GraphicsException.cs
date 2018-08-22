// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using System.Runtime.Serialization;

namespace Vortice.Graphics
{
	/// <summary>
	/// Exception for Graphics logic.
	/// </summary>
	public class GraphicsException : Exception
	{
		public GraphicsException()
		{
		}

		protected GraphicsException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public GraphicsException(string message)
			: base(message)
		{
		}

		public GraphicsException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
