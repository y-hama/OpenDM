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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.ErrorChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.Timer_10 = new System.Windows.Forms.Timer(this.components);
            this.GenerationLabel = new System.Windows.Forms.Label();
            this.ErrorLabel = new System.Windows.Forms.Label();
            this.T_Label = new System.Windows.Forms.Label();
            this.o_Label = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.UpdateTimeLabel = new System.Windows.Forms.Label();
            this.ProcessTimeLabel = new System.Windows.Forms.Label();
            this.OptionButton = new System.Windows.Forms.Button();
            this.EpochError = new System.Windows.Forms.Label();
            this.EpochGenerationLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ErrorChart)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ErrorChart
            // 
            chartArea1.AxisX.Interval = 1000D;
            chartArea1.Name = "ErrorArea";
            this.ErrorChart.ChartAreas.Add(chartArea1);
            this.ErrorChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ErrorChart.Location = new System.Drawing.Point(0, 88);
            this.ErrorChart.Name = "ErrorChart";
            series1.ChartArea = "ErrorArea";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
            series1.Name = "EpochError";
            series2.ChartArea = "ErrorArea";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Name = "ErrorMean";
            series3.ChartArea = "ErrorArea";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series3.Legend = "Legend1";
            series3.Name = "ErrorDefault";
            this.ErrorChart.Series.Add(series1);
            this.ErrorChart.Series.Add(series2);
            this.ErrorChart.Series.Add(series3);
            this.ErrorChart.Size = new System.Drawing.Size(284, 173);
            this.ErrorChart.TabIndex = 0;
            this.ErrorChart.Text = "Error";
            // 
            // Timer_10
            // 
            this.Timer_10.Enabled = true;
            this.Timer_10.Interval = 10;
            this.Timer_10.Tick += new System.EventHandler(this.Timer_10_Tick);
            // 
            // GenerationLabel
            // 
            this.GenerationLabel.AutoSize = true;
            this.GenerationLabel.Location = new System.Drawing.Point(3, 0);
            this.GenerationLabel.Name = "GenerationLabel";
            this.GenerationLabel.Size = new System.Drawing.Size(65, 12);
            this.GenerationLabel.TabIndex = 1;
            this.GenerationLabel.Text = "Generation";
            // 
            // ErrorLabel
            // 
            this.ErrorLabel.AutoSize = true;
            this.ErrorLabel.Location = new System.Drawing.Point(3, 12);
            this.ErrorLabel.Name = "ErrorLabel";
            this.ErrorLabel.Size = new System.Drawing.Size(35, 12);
            this.ErrorLabel.TabIndex = 2;
            this.ErrorLabel.Text = "Error";
            // 
            // T_Label
            // 
            this.T_Label.AutoSize = true;
            this.T_Label.Font = new System.Drawing.Font("ＭＳ ゴシック", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.T_Label.Location = new System.Drawing.Point(3, 24);
            this.T_Label.Name = "T_Label";
            this.T_Label.Size = new System.Drawing.Size(10, 9);
            this.T_Label.TabIndex = 3;
            this.T_Label.Text = "T";
            // 
            // o_Label
            // 
            this.o_Label.AutoSize = true;
            this.o_Label.Font = new System.Drawing.Font("ＭＳ ゴシック", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.o_Label.Location = new System.Drawing.Point(3, 36);
            this.o_Label.Name = "o_Label";
            this.o_Label.Size = new System.Drawing.Size(10, 9);
            this.o_Label.TabIndex = 4;
            this.o_Label.Text = "O";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.UpdateTimeLabel);
            this.panel1.Controls.Add(this.ProcessTimeLabel);
            this.panel1.Controls.Add(this.OptionButton);
            this.panel1.Controls.Add(this.EpochError);
            this.panel1.Controls.Add(this.EpochGenerationLabel);
            this.panel1.Controls.Add(this.GenerationLabel);
            this.panel1.Controls.Add(this.o_Label);
            this.panel1.Controls.Add(this.ErrorLabel);
            this.panel1.Controls.Add(this.T_Label);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(284, 88);
            this.panel1.TabIndex = 5;
            // 
            // UpdateTimeLabel
            // 
            this.UpdateTimeLabel.AutoSize = true;
            this.UpdateTimeLabel.Location = new System.Drawing.Point(93, 73);
            this.UpdateTimeLabel.Name = "UpdateTimeLabel";
            this.UpdateTimeLabel.Size = new System.Drawing.Size(17, 12);
            this.UpdateTimeLabel.TabIndex = 9;
            this.UpdateTimeLabel.Text = "ut";
            // 
            // ProcessTimeLabel
            // 
            this.ProcessTimeLabel.AutoSize = true;
            this.ProcessTimeLabel.Location = new System.Drawing.Point(3, 73);
            this.ProcessTimeLabel.Name = "ProcessTimeLabel";
            this.ProcessTimeLabel.Size = new System.Drawing.Size(17, 12);
            this.ProcessTimeLabel.TabIndex = 8;
            this.ProcessTimeLabel.Text = "pt";
            // 
            // OptionButton
            // 
            this.OptionButton.Location = new System.Drawing.Point(206, 59);
            this.OptionButton.Name = "OptionButton";
            this.OptionButton.Size = new System.Drawing.Size(75, 23);
            this.OptionButton.TabIndex = 7;
            this.OptionButton.Text = "Option";
            this.OptionButton.UseVisualStyleBackColor = true;
            this.OptionButton.Click += new System.EventHandler(this.OptionButton_Click);
            // 
            // EpochError
            // 
            this.EpochError.AutoSize = true;
            this.EpochError.Location = new System.Drawing.Point(131, 12);
            this.EpochError.Name = "EpochError";
            this.EpochError.Size = new System.Drawing.Size(65, 12);
            this.EpochError.TabIndex = 6;
            this.EpochError.Text = "EpochError";
            // 
            // EpochGenerationLabel
            // 
            this.EpochGenerationLabel.AutoSize = true;
            this.EpochGenerationLabel.Location = new System.Drawing.Point(131, 0);
            this.EpochGenerationLabel.Name = "EpochGenerationLabel";
            this.EpochGenerationLabel.Size = new System.Drawing.Size(95, 12);
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
            this.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
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
        private System.Windows.Forms.Button OptionButton;
        private System.Windows.Forms.Label UpdateTimeLabel;
        private System.Windows.Forms.Label ProcessTimeLabel;
    }
}