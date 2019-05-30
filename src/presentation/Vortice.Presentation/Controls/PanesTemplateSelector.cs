// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.
// Implementation is based on Gemini: https://github.com/tgjones/gemini
// Gemini is licensed under Apache 2.0 (https://github.com/tgjones/gemini/blob/master/LICENCE.txt)

using System.Windows;
using System.Windows.Controls;

namespace Vortice.Presentation.Controls
{
    public class PanesTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ToolTemplate { get; set; }

        public DataTemplate DocumentTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ITool)
                return ToolTemplate;

            if (item is IDocument)
                return DocumentTemplate;

            return base.SelectTemplate(item, container);
        }
    }
}
