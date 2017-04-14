using System;
using System.Windows.Forms;
using Emotiv;
using System.Threading;

namespace EmotivTetris
{
    public partial class MainForm : Form
    {
        private class ThreadControl
        {
            public bool stop;
            public bool wait;

            public void Stop()
            {
                stop = true;
                while (wait) { Thread.Sleep(1); }
            }
        }

        private class CommandState
        {
            public bool buttonDown
                = false;
            public ThreadControl threadCtrl
                = null;
            public float pwr
                = 0f;
            public float max
                = 0f;
            public float min
                = 0f;
            public float downThresh
                = 0.2f;
            public float upThresh
                = 0.2f;

            public Action onButtonDown
                = null;
            public Action onButtonUp
                = null;

            public int PwrInt
            {
                get { return Math.Min(100, (int)(pwr * 100f)); }
            }

            private ThreadControl StartButtonDown()
            {
                var threadCtrl = new ThreadControl { 
                    stop = false, 
                    wait = false 
                };
                new Thread(() => {
                    while (!threadCtrl.stop)
                    {
                        onButtonDown();
                        threadCtrl.wait = false;
                        Thread.Sleep(600); // TODO: make this configurable
                        threadCtrl.wait = true;
                    }
                    threadCtrl.wait = false;
                }).Start();
                return threadCtrl;
            }

            public void FireEvents(float pwr)
            {
                this.pwr = pwr;
                if (!buttonDown)
                {
                    if (pwr < min) { min = pwr; }
                    if (pwr > min + downThresh)
                    {
                        if (onButtonDown != null) 
                        {
                            threadCtrl = StartButtonDown();
                        }
                        buttonDown = true;
                        max = pwr;
                    }
                }
                else // ButtonDown
                {
                    if (pwr > max) { max = pwr; }
                    if (pwr < max - upThresh)
                    {
                        if (threadCtrl != null)
                        {
                            threadCtrl.Stop();
                        }
                        if (onButtonUp != null)
                        {
                            onButtonUp();
                        }
                        buttonDown = false;
                        min = pwr;
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
                onButtonDown = () => FireKeyEvent("keydown", 37),
                onButtonUp = () => FireKeyEvent("keyup", 37)
            };
            commandStateRight = new CommandState() {
                onButtonDown = () => FireKeyEvent("keydown", 39),
                onButtonUp = () => FireKeyEvent("keyup", 39)
            };
            commandStateTurn = new CommandState() {
                onButtonDown = () => FireKeyEvent("keydown", 38), 
                onButtonUp = () => FireKeyEvent("keyup", 38) 
            };
            commandStateDrop = new CommandState() {
                //onButtonDown = () => FireKeyEvent("keydown", 40), 
                //onButtonUp = () => FireKeyEvent("keyup", 40) 
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
                new { cmd = EdkDll.IEE_MentalCommandAction_t.MC_PUSH, state = commandStateTurn },
                new { cmd = EdkDll.IEE_MentalCommandAction_t.MC_PULL, state = commandStateDrop }
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
            webBrowser.Invoke(new MethodInvoker(() => {
                webBrowser.Document.InvokeScript("eval", new[] { string.Format(@"
                    var e = $.Event('{0}');
                    e.which = {1};
                    $(document).trigger(e);", eventName, keyCode
                )});
            }));
        }
    }
}
