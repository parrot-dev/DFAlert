
/*
 * DFAlert 1.1.2
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
        public override Version Version { get { return new Version(1, 1, 3); } }

        public override string Name
        {
            get { return "DFAlert"; }
        }
        public override bool WantButton
        {
            get { return true; }
        }
        public override string ButtonText
        {
            get { return "Settings"; }
        }
        public override void OnButtonPress()
        {
            var sf = new settingsForm();
            sf.ShowDialog();

        }
        public override void OnEnabled()
        {
            if (!Settings.CreateSettingsFile())
            {
                Log.print("Could not create Settings.xml, make sure the plugin is installed in Plugins/DFAlert/ folder");
            }
            Settings.Load();            
            _dutyJoiner = new DutyJoiner(); 
        }
        #endregion

        private bool isUp;
        private DutyJoiner _dutyJoiner;

        public override void OnPulse()
        {
            if (ff14bot.Core.IsInGame)
                run();
        }
        private void run()
        {
            if (ContentsFinderConfirm.IsOpen)
            {
                if (!isUp)
                {
                    isUp = true;
                    SndPlayer.Play();
                    Log.print("Dungeon is ready");
                    if(Settings.Current.pBullet.active)
                        new PushBullet.Note("DfAlert", "Dungeon is ready").Push();
                    _dutyJoiner.Reset();
                }
                if (Settings.Current.autoCommence.active)
                    _dutyJoiner.Commence();
            }
            else            
                isUp = false;            
        }

        private class DutyJoiner
        {
            private DateTime _joinTime;
            private bool _isTimeSet;
            private bool _commenced;

            public DutyJoiner()
            {
                _isTimeSet = false;
                _commenced = false;
            }

            public bool Commence()
            {
                if(!_isTimeSet)
                {
                    _joinTime = DateTime.Now.Add(TimeSpan.FromSeconds(Settings.Current.autoCommence.delay));
                    _isTimeSet = true;
                }

                if(!_commenced && DateTime.Now > _joinTime && ContentsFinderConfirm.IsOpen)
                {
                    ContentsFinderConfirm.Commence();
                    _commenced = true;
                    return true;
                }

                return false;
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
                    SoundPlayer sp = new SoundPlayer();
                    sp.SoundLocation = System.Windows.Forms.Application.StartupPath + @"\Plugins\DFAlert\Sounds\"+FileName;
                    sp.Play();
                }
                catch (Exception e)
                {
                    Log.print("Error: Could not play audio.\n" + e.Message);
                }
            }
        }
    }
}




