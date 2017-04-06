using System;
using System.Windows.Forms;
using Emotiv;
using System.Runtime.InteropServices;
using System.Threading;

namespace EmotivTetris
{
    public partial class MainForm : Form
    {
        private class CommandState
        {
            public bool Down
                = false;
            public float Pwr
                = 0f;
            public float PwrMax
                = 0f;
            public float DownThresh
                = 0.1f;
            public float UpThresh
                = 0.1f;

            public int PwrInt
            {
                get { return Math.Min(100, (int)(Pwr * 100f)); }
            }

            public void FireEvents(float pwr)
            {
                Pwr = pwr;
                // TODO
            }
        }

        private CommandState commandStateLeft 
            = new CommandState();
        private CommandState commandStateRight 
            = new CommandState();
        private CommandState commandStateTurn 
            = new CommandState();
        private CommandState commandStateDrop
            = new CommandState();

        public static HeadsetStatus headsetStatus
            = new HeadsetStatus();

        public MainForm()
        {
            InitializeComponent();
            WebBrowserHelper.ClearCache();
            webBrowser.Navigate("http://localhost/tetromini/firsttetrisever.html");
        }

        private void btnHeadsetStatus(object sender, EventArgs e)
        {
            headsetStatus.Show();
        }

        public void SetMentalCommand(EdkDll.IEE_MentalCommandAction_t command, float power)
        {
            foreach (var item in new[] { 
                new { cmd = EdkDll.IEE_MentalCommandAction_t.MC_LEFT, state = commandStateLeft },
                new { cmd = EdkDll.IEE_MentalCommandAction_t.MC_RIGHT, state = commandStateRight },
                new { cmd = EdkDll.IEE_MentalCommandAction_t.MC_PULL, state = commandStateTurn },
                new { cmd = EdkDll.IEE_MentalCommandAction_t.MC_PUSH, state = commandStateDrop }
            })
            {
                item.state.FireEvents(command == item.cmd ? power : 0f);
            }
            foreach (var item in new[] { 
                new { bar = barPwrLeft, state = commandStateLeft },
                new { bar = barPwrRight, state = commandStateRight },
                new { bar = barPwrTurn, state = commandStateTurn },
                new { bar = barPwrDrop, state = commandStateDrop }
            })
            {
                item.bar.Value = item.state.PwrInt;
            }
        }

        private void FireKeyEvent(string eventName, int keyCode)
        {
            webBrowser.Document.InvokeScript("eval", new[] { string.Format(@"
                var e = $.Event('{0}');
                e.which = {1};
                $(document).trigger(e);", eventName, keyCode
            )});
        }

        private void btnCommandLeftDown(object sender, MouseEventArgs e)
        {
            FireKeyEvent("keydown", 37); // right = 39
        }

        private void btnCommandLeftUp(object sender, MouseEventArgs e)
        {
            FireKeyEvent("keyup", 37);
        }
    }
}
