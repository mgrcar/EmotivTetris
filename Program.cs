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

        static void emoEngine_UserAdded_Event(object sender, EmoEngineEventArgs e)
        {
            Console.WriteLine("User Added Event has occured (userID = {0}).", e.userId);
            userID = (int)e.userId;

            uint EPOCmode, eegRate, eegRes, memsRate, memsRes;

            emoEngine.GetHeadsetSettings(
                (uint)userID,
                out EPOCmode,
                out eegRate,
                out eegRes,
                out memsRate,
                out memsRes
            );

            Console.WriteLine("Current config: {0} {1} {2} {3} {4}", EPOCmode, eegRate, eegRes, memsRate, memsRes);

            Console.WriteLine(emoEngine.MentalCommandGetOverallSkillRating((uint)userID));

            //\param EPOCmode
            //--- If 0, then EPOC mode is EPOC.
            //--- If 1, then EPOC mode is EPOC+.
            //\param eegRate
            //--- If 0, then EEG sample rate is 128Hz.
            //--- If 1, then EEG sample rate is 256Hz.
            //\param eegRes
            //--- If 0, then EEG resolution is 14bit.
            //--- If 1, then EEG resolution is 16bit.
            //\param memsRate
            //--- If 0, then motion sample rate is OFF.
            //--- If 1, then motion sample rate is 32Hz.
            //--- If 2, then motion sample rate is 64Hz.
            //--- If 3, then motion sample rate is 128Hz.
            //\param memsRes
            //--- If 0, then motion resolution is 12bit.
            //--- If 1, then motion resolution is 14bit.
            //--- If 2, then motion resolution is 16bit.

            //if (EPOCmode != 1)
            //{
            //    emoEngine.SetHeadsetSettings(
            //        (uint)userID,
            //        EPOCmode: 0,
            //        eegRate: 0,
            //        eegRes: 0,
            //        memsRate: 0,
            //        memsRes: 0
            //    );
            //    emoEngine.Disconnect();
            //    Application.Exit();
            //}

            Console.WriteLine(EdkDll.IEE_LoadUserProfile((uint)userID, @"C:\Documents and Settings\All Users\Application Data\Emotiv\mIHA.emu"));
        }

        static void emoEngine_MentalCommandEmoStateUpdated(object sender, EmoStateUpdatedEventArgs e)
        {
            Console.WriteLine(e.emoState.MentalCommandGetCurrentAction());
            Console.WriteLine(e.emoState.MentalCommandGetCurrentActionPower());
        }

        static void emoEngine_EmoStateUpdated(object sender, EmoStateUpdatedEventArgs e)
        {
            EmoState es = e.emoState;

            if (e.userId != 0) { return; }

            float timeFromStart = es.GetTimeFromStart();
            Console.Write(timeFromStart + ",");

            EdkDll.IEE_SignalStrength_t signalStrength = es.GetWirelessSignalStatus();
            Console.Write(signalStrength + ",");
            MainForm.sensors.SetSignalStrength(signalStrength);

            Int32 chargeLevel = 0;
            Int32 maxChargeLevel = 0;
            es.GetBatteryChargeLevel(out chargeLevel, out maxChargeLevel);
            Console.Write(chargeLevel + "," + maxChargeLevel + ",");
            if (signalStrength == EdkDll.IEE_SignalStrength_t.NO_SIG) { chargeLevel = 0; }
            MainForm.sensors.SetBatteryLevel(chargeLevel);

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
                MainForm.sensors.SetSensor(item.chStr, val);
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

            var emotivThread = new Thread(() => {
                while (true) {
                    emoEngine.ProcessEvents();
                    Thread.Sleep(100);
                    if (abort) { break; }
                }
            });
            
            emotivThread.Start();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(defaultValue: false);
            Application.Run(new MainForm());

            abort = true; // stop the thread gracefully
        }
    }
}
