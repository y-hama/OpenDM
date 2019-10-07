using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpenDMConsole.Forms
{
    public partial class MainForm : Form
    {
        private Model.DebugProcess dprocess { get; set; }

        private object __qlock = new object();
        private Queue<OpenDM.Unit.Process.UpdateInstanceArgs> errorQueue { get; set; }
        private const int MaxErrorLength = 2500;

        private OpenDM.Unit.Process.UpdateInstanceArgs epochData { get; set; }

        public MainForm()
        {
            InitializeComponent();

            errorQueue = new Queue<OpenDM.Unit.Process.UpdateInstanceArgs>();

            dprocess = new Model.DebugProcess(UpdateInstance, EpochIteration);
        }

        private void UpdateInstance(OpenDM.Unit.Process.UpdateInstanceArgs e)
        {
            lock (__qlock)
            {
                errorQueue.Enqueue(e);
                if (errorQueue.Count > MaxErrorLength) { errorQueue.Dequeue(); }
            }
        }

        private void EpochIteration(OpenDM.Unit.Process.UpdateInstanceArgs e)
        {
            lock (__qlock)
            {
                epochData = e;
            }
        }

        private void Timer_10_Tick(object sender, EventArgs e)
        {
            OpenDM.Unit.Process.UpdateInstanceArgs[] error;
            lock (__qlock)
            {
                if (epochData != null)
                {
                    EpochGenerationLabel.Text = epochData.Epoch.ToString();
                    EpochError.Text = epochData.EpochError.ToString();
                    foreach (var item in errorQueue)
                    {
                        if (item.Epoch == 0)
                        {
                            item.Epoch = epochData.Epoch;
                            item.EpochError = epochData.EpochError;
                        }
                    }
                }
                error = errorQueue.ToArray();
            }
            if (error.Length > 0)
            {
                var latestdata = error[error.Length - 1];

                GenerationLabel.Text = latestdata.Generation.ToString();
                ErrorLabel.Text = latestdata.Error.ToString();

                T_Label.Text = "T " + latestdata.Teacher.ToString(0);
                o_Label.Text = "O " + latestdata.Output.ToString(0);

                double errave = 0;
                double ratio = 3.0 / 4.0;
                for (int i = (int)(error.Length * ratio); i < error.Length; i++)
                {
                    errave += error[i].Error;
                }
                errave /= (1 + error.Length * (1 - ratio));

                var see = ErrorChart.Series["EpochError"];
                var ser = ErrorChart.Series["ErrorDefault"];
                var sem = ErrorChart.Series["ErrorMean"];
                var cha = ErrorChart.ChartAreas["ErrorArea"];
                see.Points.Clear();
                ser.Points.Clear();
                sem.Points.Clear();
                var ema = 0.0; var cnt = 0.0; var pema = 0.0;
                int ksize = 10;
                double erho = 0.99;
                double smax = 0;
                for (int i = 0; i < error.Length; i++)
                {
                    if (smax < error[i].Error) { smax = error[i].Error; }
                    see.Points.AddY(error[i].EpochError);
                    ser.Points.AddY(errave);
                    ema = 0; cnt = 0;
                    for (int s = -ksize; s <= ksize; s++)
                    {
                        if (i + s > 0 && i + s < error.Length)
                        {
                            cnt++;
                            ema += error[i + s].Error;
                        }
                    }
                    if (cnt != 0) { ema /= cnt; }
                    if (i == 0)
                    {
                        sem.Points.AddY(ema);
                    }
                    else
                    {
                        ema = erho * ema + (1 - erho) * pema;
                        sem.Points.AddY(ema);
                    }
                    pema = ema;
                }
                for (int i = error.Length; i < MaxErrorLength; i++)
                {
                    ser.Points.AddY(errave);
                    sem.Points.AddY(errave);
                }
                cha.AxisY.Maximum = Math.Ceiling(smax);
            }
        }
    }
}
