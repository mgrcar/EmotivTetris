using System;
using System.Windows.Forms;
using Emotiv;

namespace EmotivTetris
{
    public partial class MainForm : Form
    {
        public static Sensors sensors
            = new Sensors();

        public MainForm()
        {
            InitializeComponent();
            WebBrowserHelper.ClearCache();
            webBrowser.Navigate("http://localhost/tetromini/firsttetrisever.html");
        }

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    // left
        //    webBrowser.Focus();
        //    SendKeys.Send("{LEFT}");
        //}

        //private void button3_Click(object sender, EventArgs e)
        //{
        //    // right
        //    webBrowser.Focus();
        //    SendKeys.Send("{RIGHT}");
        //}

        private void button4_Click(object sender, EventArgs e)
        {
            sensors.Show();
        }

        public void SetMentalCommand(EdkDll.IEE_MentalCommandAction_t command, float power)
        {
            int pwr = Math.Min(100, (int)(power * 100f));
            int pwrLeft = 0, pwrRight = 0, pwrTurn = 0, pwrDrop = 0;
            switch (command)
            {
                case EdkDll.IEE_MentalCommandAction_t.MC_LEFT:
                    pwrLeft = pwr;
                    break;
                case EdkDll.IEE_MentalCommandAction_t.MC_RIGHT:
                    pwrRight = pwr;
                    break;
                case EdkDll.IEE_MentalCommandAction_t.MC_PULL:
                    pwrTurn = pwr;
                    break;
                case EdkDll.IEE_MentalCommandAction_t.MC_PUSH:
                    pwrDrop = pwr;
                    break;
            }
            foreach (var item in new [] { 
                new { bar = barPwrLeft, val = pwrLeft },
                new { bar = barPwrRight, val = pwrRight },
                new { bar = barPwrTurn, val = pwrTurn },
                new { bar = barPwrDrop, val = pwrDrop }
            })
            {
                item.bar.Value = item.val;
            }
        }
    }
}
