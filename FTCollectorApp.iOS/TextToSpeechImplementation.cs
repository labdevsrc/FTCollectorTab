using Foundation;
using FTCollectorApp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using AVFoundation;
using FTCollectorApp.iOS;//enables registration outside of namespace

[assembly: Xamarin.Forms.Dependency(typeof(TextToSpeechImplementation))]
namespace FTCollectorApp.iOS
{
    public class TextToSpeechImplementation : ITextToSpeech
    {
        public TextToSpeechImplementation()
        {

        }
        public void Speak(string text)
        {
            var speechSynthesizer = new AVSpeechSynthesizer();
            var speechUtterance = new AVSpeechUtterance(text)
            {
                Rate = AVSpeechUtterance.MaximumSpeechRate / 4,
                Voice = AVSpeechSynthesisVoice.FromLanguage("en-US"),
                Volume = 0.5f,
                PitchMultiplier = 1.0f
            };
            speechSynthesizer.SpeakUtterance(speechUtterance);
        }
    }
}