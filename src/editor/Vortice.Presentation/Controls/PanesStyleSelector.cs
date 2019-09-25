// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.
// Implementation is based on Gemini: https://github.com/tgjones/gemini
// Gemini is licensed under Apache 2.0 (https://github.com/tgjones/gemini/blob/master/LICENCE.txt)

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Vortice.Presentation.Controls
{
    public class PanesStyleSelector : StyleSelector
    {
        public Style ToolStyle { get; set; }

        public Style DocumentStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is ITool)
                return ToolStyle;

            if (item is IDocument)
                return DocumentStyle;

            return base.SelectStyle(item, container);
        }
    }
}
