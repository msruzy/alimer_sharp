// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.

using System;
using Vortice.Mathematics;
using WindowsPoint = Windows.Foundation.Point;
using WindowsSize = Windows.Foundation.Size;
using WindowsRect = Windows.Foundation.Rect;

namespace Vortice
{
    public static class UAPExtensions
    {
        public static Point ToPoint(this WindowsPoint point)
        {
            if (point.X > int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(point.X));

            if (point.Y > int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(point.Y));

            return new Point((int)point.X, (int)point.Y);
        }

        public static PointF ToPointF(this WindowsPoint point) =>
            new PointF((float)point.X, (float)point.Y);

        public static WindowsPoint FromPoint(this Point point) =>
            new WindowsPoint(point.X, point.Y);

        public static WindowsPoint FromPointF(this PointF point) =>
            new WindowsPoint(point.X, point.Y);

        public static Size ToSize(this WindowsSize size)
        {
            if (size.Width > int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(size.Width));

            if (size.Height > int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(size.Height));

            return new Size((int)size.Width, (int)size.Height);
        }

        public static SizeF ToSizeF(this WindowsSize size) =>
            new SizeF((float)size.Width, (float)size.Height);

        public static WindowsSize FromSize(this Size size) =>
            new WindowsSize(size.Width, size.Height);

        public static WindowsSize FromSizeF(this SizeF size) =>
            new WindowsSize(size.Width, size.Height);

        public static Rectangle ToRectangle(this WindowsRect rect)
        {
            if (rect.X > int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(rect.X));

            if (rect.Y > int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(rect.Y));

            if (rect.Width > int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(rect.Width));

            if (rect.Height > int.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(rect.Height));

            return new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
        }

        public static RectangleF ToRectangleF(this WindowsRect rect) =>
            new RectangleF((float)rect.X, (float)rect.Y, (float)rect.Width, (float)rect.Height);

        public static WindowsRect FromRectangle(this Rectangle rect) =>
            new WindowsRect(rect.X, rect.Y, rect.Width, rect.Height);

        public static WindowsRect FromRectangleF(this RectangleF rect) =>
            new WindowsRect(rect.X, rect.Y, rect.Width, rect.Height);
    }
}
