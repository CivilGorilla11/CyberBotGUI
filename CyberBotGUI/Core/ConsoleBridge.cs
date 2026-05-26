using System;
using System.Collections.Generic;
using System.Text;
using CyberBot;


namespace CyberBotGUI.Core
{
    public static class ConsoleBridge
    {
        public static void DrawDivider()
        {
            try
            {
                SectionDivider.Draw();
            }
            catch { }
        }

        public static void LogoArt()
        { 
            AsciiArt.DrawLock();
        }


        public static void PlayGreeting()
        {
            System.Threading.Thread voiceThread = new System.Threading.Thread(() =>
            {
                try
                {
                    VoiceGreeting.Speak();
                }
                catch { }
            });
            voiceThread.IsBackground = true;
            voiceThread.Start();
        }
    }
}
