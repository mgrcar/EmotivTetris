using System;
using System.Windows.Forms;
using System.Threading;
using Emotiv;

namespace EmotivTetris
{
    static class Program
    {
        static int userID = -1;

        static void engine_UserAdded_Event(object sender, EmoEngineEventArgs e)
        {
            Console.WriteLine("User Added Event has occured.");
            userID = (int)e.userId;
        }

        static void engine_EmoStateUpdated(object sender, EmoStateUpdatedEventArgs e)
        {
            EmoState es = e.emoState;

            if (e.userId != 0) { return; }      

            float timeFromStart = es.GetTimeFromStart();
            Console.Write(timeFromStart + ",");

            EdkDll.IEE_SignalStrength_t signalStrength = es.GetWirelessSignalStatus();
            Console.Write(signalStrength + ",");

            Int32 chargeLevel = 0;
            Int32 maxChargeLevel = 0;
            es.GetBatteryChargeLevel(out chargeLevel, out maxChargeLevel);
            Console.Write(chargeLevel + ",");

            Console.Write((int)es.GetContactQuality((int)EdkDll.IEE_InputChannels_t.IEE_CHAN_AF3) + ",");
            Console.Write((int)es.GetContactQuality((int)EdkDll.IEE_InputChannels_t.IEE_CHAN_T7) + ",");
            Console.Write((int)es.GetContactQuality((int)EdkDll.IEE_InputChannels_t.IEE_CHAN_O1) + ",");
            Console.Write((int)es.GetContactQuality((int)EdkDll.IEE_InputChannels_t.IEE_CHAN_T8) + ",");
            Console.Write((int)es.GetContactQuality((int)EdkDll.IEE_InputChannels_t.IEE_CHAN_AF4));

            Console.WriteLine();
        }

        [STAThread]
        static void Main()
        {
            bool abort = false;

            EmoEngine engine = EmoEngine.Instance;

            engine.UserAdded += new EmoEngine.UserAddedEventHandler(engine_UserAdded_Event);
            engine.EmoStateUpdated += new EmoEngine.EmoStateUpdatedEventHandler(engine_EmoStateUpdated);

            engine.Connect();

            Console.WriteLine("Connected.");

            var emotivThread = new Thread(() => {
                while (true) {
                    engine.ProcessEvents();
                    Thread.Sleep(10);
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
