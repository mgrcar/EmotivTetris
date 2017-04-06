using System;
using System.Windows.Forms;
using Emotiv;

namespace EmotivTetris
{
    public partial class MainForm : Form
    {
        private class CommandState
        {
            public bool ButtonDown
                = false;
            public float Pwr
                = 0f;
            public float Max
                = 0f;
            public float Min
                = 0f;
            public float DownThresh
                = 0.1f;
            public float UpThresh
                = 0.1f;

            public Action OnButtonDown
                = null;
            public Action OnButtonUp
                = null;

            public int PwrInt
            {
                get { return Math.Min(100, (int)(Pwr * 100f)); }
            }

            public void FireEvents(float pwr)
            {
                Pwr = pwr;
                if (!ButtonDown)
                {
                    if (pwr < Min) { Min = pwr; }
                    if (pwr > Min + DownThresh)
                    {
                        if (OnButtonDown != null) 
                        { 
                            OnButtonDown(); 
                        }
                        ButtonDown = true;
                        Max = pwr;
                    }
                }
                else // ButtonDown
                {
                    if (pwr > Max) { Max = pwr; }
                    if (pwr < Max - UpThresh)
                    {
                        if (OnButtonUp != null)
                        {
                            OnButtonUp();
                        }
                        ButtonDown = false;
                        Min = pwr;
                    }
                }
            }
        }

        private CommandState commandStateLeft;
        private CommandState commandStateRight;
        private CommandState commandStateTurn;
        private CommandState commandStateDrop;

        public static HeadsetStatus headsetStatus
            = new HeadsetStatus();

        public MainForm()
        {
            InitializeComponent();
            WebBrowserHelper.ClearCache();
            webBrowser.Navigate(Config.GameUrl);
            commandStateLeft = new CommandState() {
                OnButtonDown = () => FireKeyEvent("keydown", 37),
                OnButtonUp = () => FireKeyEvent("keyup", 37)
            };
            commandStateRight = new CommandState() {
                OnButtonDown = () => FireKeyEvent("keydown", 39),
                OnButtonUp = () => FireKeyEvent("keyup", 39)
            };
            commandStateTurn = new CommandState() {
                OnButtonDown = () => FireKeyEvent("keydown", 38), 
                OnButtonUp = () => FireKeyEvent("keyup", 38) 
            };
            commandStateDrop = new CommandState() {
                OnButtonDown = () => FireKeyEvent("keydown", 40), 
                OnButtonUp = () => FireKeyEvent("keyup", 40) 
            };
        }

        private void btnHeadsetStatus(object sender, EventArgs e)
        {
            headsetStatus.Show();
        }

        public void FireMentalCommand(EdkDll.IEE_MentalCommandAction_t command, float power)
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
    }
}
