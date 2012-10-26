namespace Neuron {
	partial class BinaryMap {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
            this.button1 = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.matrixView1 = new DotNumerics.LinearAlgebra.MatrixView();
            this.matrixView2 = new DotNumerics.LinearAlgebra.MatrixView();
            this.binaryMapControl1 = new Neuron.BinaryMapControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.binaryMapControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(0, 372);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(796, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "train";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.binaryMapControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(796, 372);
            this.splitContainer1.SplitterDistance = 607;
            this.splitContainer1.TabIndex = 2;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.matrixView1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.matrixView2);
            this.splitContainer2.Size = new System.Drawing.Size(185, 372);
            this.splitContainer2.SplitterDistance = 165;
            this.splitContainer2.TabIndex = 1;
            // 
            // matrixView1
            // 
            this.matrixView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.matrixView1.Format = "0.000";
            this.matrixView1.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.matrixView1.Location = new System.Drawing.Point(0, 0);
            this.matrixView1.Name = "matrixView1";
            this.matrixView1.ReadOnly = false;
            this.matrixView1.Size = new System.Drawing.Size(185, 165);
            this.matrixView1.TabIndex = 0;
            // 
            // matrixView2
            // 
            this.matrixView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.matrixView2.Format = "0.000";
            this.matrixView2.FormatProvider = new System.Globalization.CultureInfo("en-US");
            this.matrixView2.Location = new System.Drawing.Point(0, 0);
            this.matrixView2.Name = "matrixView2";
            this.matrixView2.ReadOnly = false;
            this.matrixView2.Size = new System.Drawing.Size(185, 203);
            this.matrixView2.TabIndex = 1;
            // 
            // binaryMapControl1
            // 
            this.binaryMapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.binaryMapControl1.Location = new System.Drawing.Point(0, 0);
            this.binaryMapControl1.Name = "binaryMapControl1";
            this.binaryMapControl1.Size = new System.Drawing.Size(607, 372);
            this.binaryMapControl1.TabIndex = 0;
            this.binaryMapControl1.TabStop = false;
            // 
            // BinaryMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 395);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.button1);
            this.Name = "BinaryMap";
            this.Text = "BinaryMap";
            this.Load += new System.EventHandler(this.BinaryMap_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.binaryMapControl1)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private BinaryMapControl binaryMapControl1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private DotNumerics.LinearAlgebra.MatrixView matrixView1;
		private System.Windows.Forms.SplitContainer splitContainer2;
        private DotNumerics.LinearAlgebra.MatrixView matrixView2;
	}
}