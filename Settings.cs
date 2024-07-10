using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Windows.Media;
using ff14bot.Managers;

namespace DFAlert
{
    public static class Settings
    {
        public static Profile Current = new Profile();
        public static string PluginRootDir = PluginManager.PluginDirectory + @"\DFAlert";

        public static bool Save()
        {
            return Save(Current);
        }

        public static bool Save(Profile profile)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Profile));
                TextWriter textWriter = new StreamWriter(PluginRootDir + @"\Settings.xml");
                serializer.Serialize(textWriter, profile);
                textWriter.Close();
            }
            catch (Exception e)
            {
                Log.Print("Error while saving :\n" + e.Message);
                return false;
            }
            Log.Print("Saved settings.", Colors.White);
            return true;
        }

        public static bool Load()
        {
            try
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(Profile));
                TextReader textReader = new StreamReader(PluginRootDir + @"\Settings.xml");
                Current = (Profile)deserializer.Deserialize(textReader);
                textReader.Close();
            }
            catch (Exception e)
            {
                Log.Print("Failed to load settings :\n" + e.Message);
                Current = new Profile();
                return false;
            }

            Log.Print("Settings loaded", Colors.White);
            return true;

        }

        public static bool CreateSettingsFile()
        {
            if (!File.Exists(PluginRootDir + @"\Settings.xml"))
            {
                Log.Print("Settings.xml is missing, creating a new file.", Colors.White);
                return Save(new Profile());
            }
            return true;
        }

        public class Profile
        {
            public AutoCommence autoCommence;
            public PBullet pBullet;
            public Profile()
            {
                autoCommence = new AutoCommence();
                pBullet = new PBullet();
            }
        }
        public class AutoCommence
        {
            public bool active;
            public int delay; ///Seconds
            public AutoCommence()
            {
                active = false;
                delay = 20;
            }
        }

        public class PBullet
        {
            public bool active;
            public string token;

            public PBullet()
            {
                active = false;
                token = "";
            }
        }
    }
}
