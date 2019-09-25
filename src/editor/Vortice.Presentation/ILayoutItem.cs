// Copyright (c) Amer Koleci and contributors.
// Distributed under the MIT license. See the LICENSE file in the project root for more information.
// Implementation is based on Gemini: https://github.com/tgjones/gemini
// Gemini is licensed under Apache 2.0 (https://github.com/tgjones/gemini/blob/master/LICENCE.txt)

using System;
using System.Windows.Input;
using Caliburn.Micro;

namespace Vortice.Presentation
{
    public interface ILayoutItem : IScreen
    {
        Guid Id { get; }
        string ContentId { get; }
        ICommand CloseCommand { get; }
        Uri IconSource { get; }
        bool IsSelected { get; set; }
        bool ShouldReopenOnStart { get; }
        //void LoadState(BinaryReader reader);
        //void SaveState(BinaryWriter writer);
    }
}
