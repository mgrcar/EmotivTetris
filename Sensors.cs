using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using EmotivTetris.Properties;
using Emotiv;

namespace EmotivTetris
{
    public partial class Sensors : Form
    {
        public Sensors()
        {
            InitializeComponent();
        }

        private void Sensors_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }

        public void SetSignalStrength(EdkDll.IEE_SignalStrength_t val)
        {
            switch (val)
            {
                case EdkDll.IEE_SignalStrength_t.NO_SIG:
                    signalStrength.Image = Resources.sensorBlack30;
                    break;
                case EdkDll.IEE_SignalStrength_t.BAD_SIG:
                    signalStrength.Image = Resources.sensorRed30;
                    break;
                default:
                    signalStrength.Image = Resources.sensorGreen30;
                    break;
            }
        }

        public void SetBatteryLevel(int val)
        {
            switch (val)
            {
                case 0:
                    batteryLevel.Image = Resources.sensorBlack30;
                    break;
                case 1:
                    batteryLevel.Image = Resources.sensorRed30;
                    break;
                case 2:
                case 3:
                    batteryLevel.Image = Resources.sensorYellow30;
                    break;
                case 4:
                case 5:
                    batteryLevel.Image = Resources.sensorGreen30;
                    break;
                default:
                    batteryLevel.Image = Resources.sensorEarBlack30;
                    break;
            }
        }

        public void SetSensor(string sensorName, int val)
        {
            Image statusLight;
            bool earSensor = "CMS,P3,DRL,P4".Split(',').Contains(sensorName);
            switch (val)
            { 
                case 0:
                    statusLight = earSensor ? Resources.sensorEarBlack30 : Resources.sensorBlack30;
                    break;
                case 1:
                    statusLight = earSensor ? Resources.sensorEarRed30 : Resources.sensorRed30;
                    break;
                case 2:
                    statusLight = earSensor ? Resources.sensorEarYellow30 : Resources.sensorYellow30;
                    break;
                default:
                    statusLight = earSensor ? Resources.sensorEarGreen30 : Resources.sensorGreen30;
                    break;
            }
            switch (sensorName)
            { 
                case "AF3": 
                case "FP1":
                    sensorAf3Fp1.Image = statusLight;
                    break;
                case "AF4":
                case "FP2":
                    sensorAf4Fp2.Image = statusLight;
                    break;
                case "CMS":
                case "P3":
                    sensorCmsP3.Image = statusLight;
                    break;
                case "DRL":
                case "P4":
                    sensorDrlP4.Image = statusLight;
                    break;
                case "F3":
                    sensorF3.Image = statusLight;
                    break;
                case "F4":
                    sensorF4.Image = statusLight;
                    break;
                case "F7":
                    sensorF7.Image = statusLight;
                    break;
                case "F8":
                    sensorF8.Image = statusLight;
                    break;
                case "FC5":
                    sensorFc5.Image = statusLight;
                    break;
                case "FC6":
                    sensorFc6.Image = statusLight;
                    break;
                case "O1":
                    sensorO1.Image = statusLight;
                    break;
                case "O2":
                    sensorO2.Image = statusLight;
                    break;
                case "P7":
                    sensorP7.Image = statusLight;
                    break;
                case "P8":
                    sensorP8.Image = statusLight;
                    break;
                case "T7":
                    sensorT7.Image = statusLight;
                    break;
                case "T8":
                    sensorT8.Image = statusLight;
                    break;
            }
        }
    }
}