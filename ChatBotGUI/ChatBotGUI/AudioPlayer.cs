using System;
using System.IO;
using System.Media;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace ChatBotGUI
{
    public static class AudioPlayer
    {
        [SupportedOSPlatform("windows")]
        public static void PlayGreeting(string audioPath)
        {
            try
            {
                // Check if the file actually exists where the program is looking
                if (!File.Exists(audioPath))
                {
                    MessageBox.Show($"Audio file not found at path:\n{Path.GetFullPath(audioPath)}", "Audio Error");
                    return;
                }

                SoundPlayer player = new SoundPlayer(audioPath);
                player.Play();
            }
            catch (Exception ex)
            {
                // This will pop up the exact error if format or playback fails
                MessageBox.Show($"Audio playback failed: {ex.Message}", "Audio Exception");
            }
        }
    }
}