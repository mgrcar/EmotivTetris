using System;
using System.Windows.Forms;
using System.Threading;

namespace EmotivTetris
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            bool abort = false;
            var emotivThread = new Thread(() => {
                while (true) {
                    Console.Write("*");
                    Thread.Sleep(100);
                    if (abort) { break; }
                }
            });
            
            emotivThread.Start();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());

            abort = true; // stop the thread gracefully
        }
    }
}
