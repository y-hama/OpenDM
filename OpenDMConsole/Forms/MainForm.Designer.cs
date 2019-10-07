namespace OpenDMConsole.Forms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.ErrorChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.Timer_10 = new System.Windows.Forms.Timer(this.components);
            this.GenerationLabel = new System.Windows.Forms.Label();
            this.ErrorLabel = new System.Windows.Forms.Label();
            this.T_Label = new System.Windows.Forms.Label();
            this.o_Label = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.EpochError = new System.Windows.Forms.Label();
            this.EpochGenerationLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorChart)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ErrorChart
            // 
            chartArea2.AxisX.Interval = 1000D;
            chartArea2.Name = "ErrorArea";
            this.ErrorChart.ChartAreas.Add(chartArea2);
            this.ErrorChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ErrorChart.Location = new System.Drawing.Point(0, 69);
            this.ErrorChart.Name = "ErrorChart";
            series4.ChartArea = "ErrorArea";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
            series4.Name = "EpochError";
            series5.ChartArea = "ErrorArea";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series5.Name = "ErrorMean";
            series6.ChartArea = "ErrorArea";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series6.Legend = "Legend1";
            series6.Name = "ErrorDefault";
            this.ErrorChart.Series.Add(series4);
            this.ErrorChart.Series.Add(series5);
            this.ErrorChart.Series.Add(series6);
            this.ErrorChart.Size = new System.Drawing.Size(284, 192);
            this.ErrorChart.TabIndex = 0;
            this.ErrorChart.Text = "Error";
            // 
            // Timer_10
            // 
            this.Timer_10.Enabled = true;
            this.Timer_10.Interval = 250;
            this.Timer_10.Tick += new System.EventHandler(this.Timer_10_Tick);
            // 
            // GenerationLabel
            // 
            this.GenerationLabel.AutoSize = true;
            this.GenerationLabel.Location = new System.Drawing.Point(3, 0);
            this.GenerationLabel.Name = "GenerationLabel";
            this.GenerationLabel.Size = new System.Drawing.Size(60, 12);
            this.GenerationLabel.TabIndex = 1;
            this.GenerationLabel.Text = "Generation";
            // 
            // ErrorLabel
            // 
            this.ErrorLabel.AutoSize = true;
            this.ErrorLabel.Location = new System.Drawing.Point(3, 12);
            this.ErrorLabel.Name = "ErrorLabel";
            this.ErrorLabel.Size = new System.Drawing.Size(30, 12);
            this.ErrorLabel.TabIndex = 2;
            this.ErrorLabel.Text = "Error";
            // 
            // T_Label
            // 
            this.T_Label.AutoSize = true;
            this.T_Label.Font = new System.Drawing.Font("メイリオ", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.T_Label.Location = new System.Drawing.Point(3, 24);
            this.T_Label.Name = "T_Label";
            this.T_Label.Size = new System.Drawing.Size(13, 14);
            this.T_Label.TabIndex = 3;
            this.T_Label.Text = "T";
            // 
            // o_Label
            // 
            this.o_Label.AutoSize = true;
            this.o_Label.Font = new System.Drawing.Font("メイリオ", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.o_Label.Location = new System.Drawing.Point(3, 36);
            this.o_Label.Name = "o_Label";
            this.o_Label.Size = new System.Drawing.Size(14, 14);
            this.o_Label.TabIndex = 4;
            this.o_Label.Text = "O";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.EpochError);
            this.panel1.Controls.Add(this.EpochGenerationLabel);
            this.panel1.Controls.Add(this.GenerationLabel);
            this.panel1.Controls.Add(this.o_Label);
            this.panel1.Controls.Add(this.ErrorLabel);
            this.panel1.Controls.Add(this.T_Label);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(284, 69);
            this.panel1.TabIndex = 5;
            // 
            // EpochError
            // 
            this.EpochError.AutoSize = true;
            this.EpochError.Location = new System.Drawing.Point(131, 12);
            this.EpochError.Name = "EpochError";
            this.EpochError.Size = new System.Drawing.Size(61, 12);
            this.EpochError.TabIndex = 6;
            this.EpochError.Text = "EpochError";
            // 
            // EpochGenerationLabel
            // 
            this.EpochGenerationLabel.AutoSize = true;
            this.EpochGenerationLabel.Location = new System.Drawing.Point(131, 0);
            this.EpochGenerationLabel.Name = "EpochGenerationLabel";
            this.EpochGenerationLabel.Size = new System.Drawing.Size(91, 12);
            this.EpochGenerationLabel.TabIndex = 5;
            this.EpochGenerationLabel.Text = "EpochGeneration";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.ErrorChart);
            this.Controls.Add(this.panel1);
            this.Name = "MainForm";
            this.Text = "MainForm";
            ((System.ComponentModel.ISupportInitialize)(this.ErrorChart)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart ErrorChart;
        private System.Windows.Forms.Timer Timer_10;
        private System.Windows.Forms.Label GenerationLabel;
        private System.Windows.Forms.Label ErrorLabel;
        private System.Windows.Forms.Label T_Label;
        private System.Windows.Forms.Label o_Label;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label EpochGenerationLabel;
        private System.Windows.Forms.Label EpochError;
    }
}