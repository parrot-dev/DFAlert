
/*
 * DFAlert 1.1.3
 */

using ff14bot.Enums;
using ff14bot.RemoteWindows;
using ff14bot.Helpers;
using ff14bot.Interfaces;
using ff14bot.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using ff14bot.AClasses;

namespace DFAlert
{
    public class DFAlert : BotPlugin
    {
        #region init
        public override string Author { get { return "Parrot"; } }
        public override string Description { get { return "Alerts user when DF group is ready"; } }
        public override Version Version { get { return new Version(1, 1, 4); } }

        public override string Name => "DFAlert";

        public override bool WantButton => true;

        public override string ButtonText => "Settings";

        public override void OnButtonPress()
        {
            new SettingsForm().ShowDialog();

        }
        public override void OnEnabled()
        {
            if (!Settings.CreateSettingsFile())
            {
                Log.Print("Could not create Settings.xml, make sure the plugin is installed in Plugins/DFAlert/ folder");
            }
            Settings.Load();            
            _dutyJoiner = new DutyJoiner(); 
        }
        #endregion

        private bool _isUp;
        private DutyJoiner _dutyJoiner;

        public override void OnPulse()
        {
            if (ff14bot.Core.IsInGame)
                Run();
        }
        private void Run()
        {
            if (ContentsFinderConfirm.IsOpen)
            {
                if (!_isUp)
                {
                    _isUp = true;
                    SndPlayer.Play();
                    Log.Print("Dungeon is ready");
                    if (Settings.Current.pBullet.active)
                        new PushBullet.Note("DfAlert", "Dungeon is ready").Push();
                    _dutyJoiner.Reset();
                }
                if (Settings.Current.autoCommence.active)
                    _dutyJoiner.Commence();
            }
            else            
                _isUp = false;            
        }

        private class DutyJoiner
        {
            private DateTime _joinTime;
            private bool _isTimeSet = false;
            private bool _commenced = false;

            public bool Commence()
            {
                if(!_isTimeSet)
                {
                    _joinTime = DateTime.Now.Add(TimeSpan.FromSeconds(Settings.Current.autoCommence.delay));
                    _isTimeSet = true;
                }

                if (_commenced || DateTime.Now <= _joinTime || !ContentsFinderConfirm.IsOpen) return false;
                
                ContentsFinderConfirm.Commence();
                _commenced = true;
                return true;

            }
            public void Reset()
            {
                _commenced = false;
                _isTimeSet = false;
            }            
        }

        private static class SndPlayer
        {
            private const string FileName = "DungeonIsReady.wav";
            public static void Play()
            {
                try
                {
                    var soundPlayer = new SoundPlayer();
                    soundPlayer.SoundLocation = System.Windows.Forms.Application.StartupPath + @"\Plugins\DFAlert\Sounds\"+FileName;
                    soundPlayer.Play();
                }
                catch (Exception e)
                {
                    Log.Print("Error: Could not play audio.\n" + e.Message);
                }
            }
        }
    }
}




