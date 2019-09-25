// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.
// Implementation is based on Gemini: https://github.com/tgjones/gemini
// Gemini is licensed under Apache 2.0 (https://github.com/tgjones/gemini/blob/master/LICENCE.txt)

using System.Windows;
using System.Windows.Media;
using Caliburn.Micro;

namespace Vortice.Presentation
{
    public interface ITool : ILayoutItem
    {
        PaneLocation PreferredLocation { get; }
        double PreferredWidth { get; }
        double PreferredHeight { get; }

        bool IsVisible { get; set; }
    }

    public enum PaneLocation
    {
        Left,
        Right,
        Bottom
    }
}
