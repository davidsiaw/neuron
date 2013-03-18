namespace Neuron
{
    partial class TrainingForm
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
            this.plotterDisplayEx1 = new GraphLib.PlotterDisplayEx();
            this.btn_batchtrain = new System.Windows.Forms.Button();
            this.num_iters = new System.Windows.Forms.NumericUpDown();
            this.btn_onlinetrain = new System.Windows.Forms.Button();
            this.num_learningRate = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_reset = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.num_iters)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_learningRate)).BeginInit();
            this.SuspendLayout();
            // 
            // plotterDisplayEx1
            // 
            this.plotterDisplayEx1.BackColor = System.Drawing.Color.Transparent;
            this.plotterDisplayEx1.BackgroundColorBot = System.Drawing.Color.Black;
            this.plotterDisplayEx1.BackgroundColorTop = System.Drawing.Color.Black;
            this.plotterDisplayEx1.DashedGridColor = System.Drawing.Color.DarkGray;
            this.plotterDisplayEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plotterDisplayEx1.DoubleBuffering = false;
            this.plotterDisplayEx1.Location = new System.Drawing.Point(0, 0);
            this.plotterDisplayEx1.Name = "plotterDisplayEx1";
            this.plotterDisplayEx1.PlaySpeed = 0.5F;
            this.plotterDisplayEx1.Size = new System.Drawing.Size(1157, 627);
            this.plotterDisplayEx1.SolidGridColor = System.Drawing.Color.DarkGray;
            this.plotterDisplayEx1.TabIndex = 0;
            // 
            // btn_batchtrain
            // 
            this.btn_batchtrain.Location = new System.Drawing.Point(54, 4);
            this.btn_batchtrain.Name = "btn_batchtrain";
            this.btn_batchtrain.Size = new System.Drawing.Size(159, 26);
            this.btn_batchtrain.TabIndex = 1;
            this.btn_batchtrain.Text = "Batch for N iterations";
            this.btn_batchtrain.UseVisualStyleBackColor = true;
            this.btn_batchtrain.Click += new System.EventHandler(this.btn_train_Click);
            // 
            // num_iters
            // 
            this.num_iters.Location = new System.Drawing.Point(376, 9);
            this.num_iters.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.num_iters.Name = "num_iters";
            this.num_iters.Size = new System.Drawing.Size(120, 20);
            this.num_iters.TabIndex = 2;
            this.num_iters.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.num_iters.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // btn_onlinetrain
            // 
            this.btn_onlinetrain.Location = new System.Drawing.Point(219, 4);
            this.btn_onlinetrain.Name = "btn_onlinetrain";
            this.btn_onlinetrain.Size = new System.Drawing.Size(151, 26);
            this.btn_onlinetrain.TabIndex = 3;
            this.btn_onlinetrain.Text = "Online for N iterations";
            this.btn_onlinetrain.UseVisualStyleBackColor = true;
            this.btn_onlinetrain.Click += new System.EventHandler(this.btn_onlinetrain_Click);
            // 
            // num_learningRate
            // 
            this.num_learningRate.DecimalPlaces = 4;
            this.num_learningRate.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.num_learningRate.Location = new System.Drawing.Point(601, 9);
            this.num_learningRate.Name = "num_learningRate";
            this.num_learningRate.Size = new System.Drawing.Size(102, 20);
            this.num_learningRate.TabIndex = 4;
            this.num_learningRate.Value = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(521, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Learning Rate";
            // 
            // btn_reset
            // 
            this.btn_reset.Location = new System.Drawing.Point(709, 7);
            this.btn_reset.Name = "btn_reset";
            this.btn_reset.Size = new System.Drawing.Size(75, 23);
            this.btn_reset.TabIndex = 6;
            this.btn_reset.Text = "Reset";
            this.btn_reset.UseVisualStyleBackColor = true;
            this.btn_reset.Click += new System.EventHandler(this.btn_reset_Click);
            // 
            // TrainingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1157, 627);
            this.Controls.Add(this.btn_reset);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.num_learningRate);
            this.Controls.Add(this.btn_onlinetrain);
            this.Controls.Add(this.num_iters);
            this.Controls.Add(this.btn_batchtrain);
            this.Controls.Add(this.plotterDisplayEx1);
            this.Name = "TrainingForm";
            this.Text = "Training!";
            this.Load += new System.EventHandler(this.TrainingForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.num_iters)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_learningRate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GraphLib.PlotterDisplayEx plotterDisplayEx1;
        private System.Windows.Forms.Button btn_batchtrain;
        private System.Windows.Forms.NumericUpDown num_iters;
        private System.Windows.Forms.Button btn_onlinetrain;
        private System.Windows.Forms.NumericUpDown num_learningRate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_reset;
    }
}