using System;
using System.Collections.Generic;
using System.Text;

namespace FTCollectorApp.Utils
{
    public interface ITextToSpeech
    {
        void Speak(string text);
    }
}
