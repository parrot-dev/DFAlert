using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DFAlert
{
    public partial class SettingsForm : Form
    {

        public SettingsForm()
        {
            InitializeComponent();

            checkBox1.Checked = Settings.Current.autoCommence.active;
            numericUpDown1.Value = Settings.Current.autoCommence.delay;
            chkboxPbullet.Checked = Settings.Current.pBullet.active;
            txtBoxPBToken.Text = Settings.Current.pBullet.token;

        }


        private void button1_Click(object sender, EventArgs e)
        {
            Settings.Current.autoCommence.active = checkBox1.Checked;
            Settings.Current.autoCommence.delay = (int)numericUpDown1.Value;
            Settings.Current.pBullet.token = string.IsNullOrEmpty(txtBoxPBToken.Text) ? "" : txtBoxPBToken.Text;
            Settings.Current.pBullet.active = chkboxPbullet.Checked;
            Settings.Save();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new PushBullet.Note("DfAlert", "Test").Push(txtBoxPBToken.Text);                       
        }


    }
}
