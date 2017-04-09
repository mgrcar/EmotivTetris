using System;
using System.Windows.Forms;
using System.Threading;
using Emotiv;

namespace EmotivTetris
{
    static class Program
    {
        static int userID 
            = -1;
        static EmoEngine emoEngine 
            = EmoEngine.Instance;
        static MainForm mainForm;

        static void emoEngine_UserAdded_Event(object sender, EmoEngineEventArgs e)
        {
            Console.WriteLine("User Added Event has occured (userID = {0}).", e.userId);
            userID = (int)e.userId;

            //uint EPOCmode, eegRate, eegRes, memsRate, memsRes;

            //emoEngine.GetHeadsetSettings(
            //    (uint)userID,
            //    out EPOCmode,
            //    out eegRate,
            //    out eegRes,
            //    out memsRate,
            //    out memsRes
            //);

            //Console.WriteLine("Current config: {0} {1} {2} {3} {4}", EPOCmode, eegRate, eegRes, memsRate, memsRes);

            Console.WriteLine(EdkDll.IEE_LoadUserProfile((uint)userID, Config.ProfileFileName));
        }

        static void emoEngine_MentalCommandEmoStateUpdated(object sender, EmoStateUpdatedEventArgs e)
        {
            var mentalCommand = e.emoState.MentalCommandGetCurrentAction();
            float power = e.emoState.MentalCommandGetCurrentActionPower();
            Console.WriteLine("Mental command detected: {0} ({1})", mentalCommand, power);
            mainForm.FireMentalCommand(mentalCommand, power);
        }

        static void emoEngine_EmoStateUpdated(object sender, EmoStateUpdatedEventArgs e)
        {
            EmoState es = e.emoState;

            if (e.userId != 0) { return; }

            float timeFromStart = es.GetTimeFromStart();
            Console.Write(timeFromStart + ",");

            EdkDll.IEE_SignalStrength_t signalStrength = es.GetWirelessSignalStatus();
            Console.Write(signalStrength + ",");
            MainForm.headsetStatus.SetSignalStrength(signalStrength);

            Int32 chargeLevel = 0;
            Int32 maxChargeLevel = 0;
            es.GetBatteryChargeLevel(out chargeLevel, out maxChargeLevel);
            Console.Write(chargeLevel + "," + maxChargeLevel + ",");
            if (signalStrength == EdkDll.IEE_SignalStrength_t.NO_SIG) { chargeLevel = 0; }
            MainForm.headsetStatus.SetBatteryLevel(chargeLevel);

            foreach (var item in new[] {
                new { ch = EdkDll.IEE_InputChannels_t.IEE_CHAN_CMS, chStr = "CMS" },
                new { ch = EdkDll.IEE_InputChannels_t.IEE_CHAN_DRL, chStr = "DRL" },
                new { ch = EdkDll.IEE_InputChannels_t.IEE_CHAN_FP1, chStr = "FP1" },
                new { ch = EdkDll.IEE_InputChannels_t.IEE_CHAN_FP2, chStr = "FP2" },
                new { ch = EdkDll.IEE_InputChannels_t.IEE_CHAN_F7, chStr = "F7" },
                new { ch = EdkDll.IEE_InputChannels_t.IEE_CHAN_F3, chStr = "F3" },
                new { ch = EdkDll.IEE_InputChannels_t.IEE_CHAN_F4, chStr = "F4" },
                new { ch = EdkDll.IEE_InputChannels_t.IEE_CHAN_F8, chStr = "F8" },
                new { ch = EdkDll.IEE_InputChannels_t.IEE_CHAN_FC5, chStr = "FC5" },
                new { ch = EdkDll.IEE_InputChannels_t.IEE_CHAN_FC6, chStr = "FC6" },
                new { ch = EdkDll.IEE_InputChannels_t.IEE_CHAN_T7, chStr = "T7" },
                new { ch = EdkDll.IEE_InputChannels_t.IEE_CHAN_T8, chStr = "T8" },
                new { ch = EdkDll.IEE_InputChannels_t.IEE_CHAN_P7, chStr = "P7" },
                new { ch = EdkDll.IEE_InputChannels_t.IEE_CHAN_P8, chStr = "P8" },
                new { ch = EdkDll.IEE_InputChannels_t.IEE_CHAN_O1, chStr = "O1" },
                new { ch = EdkDll.IEE_InputChannels_t.IEE_CHAN_O2, chStr = "O2" }
            })
            {
                int val = (int)es.GetContactQuality((int)item.ch);
                Console.Write(val + ",");
                if (signalStrength == EdkDll.IEE_SignalStrength_t.NO_SIG) { val = 0; }
                MainForm.headsetStatus.SetSensor(item.chStr, val);
            }

            Console.WriteLine();
        }

        [STAThread]
        static void Main()
        {
            bool abort = false;
            
            emoEngine.UserAdded += new EmoEngine.UserAddedEventHandler(emoEngine_UserAdded_Event);
            emoEngine.EmoStateUpdated += new EmoEngine.EmoStateUpdatedEventHandler(emoEngine_EmoStateUpdated);
            emoEngine.MentalCommandEmoStateUpdated += new EmoEngine.MentalCommandEmoStateUpdatedEventHandler(emoEngine_MentalCommandEmoStateUpdated);

            emoEngine.Connect();
            Console.WriteLine("Connected.");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(defaultValue: false);

            mainForm = new MainForm();

            var emotivThread = new Thread(() => {
                while (true)
                {
                    emoEngine.ProcessEvents();
                    Thread.Sleep(100);
                    if (abort) { break; }
                }
            });

            emotivThread.Start();
            
            Application.Run(mainForm);

            abort = true; // stop the thread gracefully
        }
    }
}
