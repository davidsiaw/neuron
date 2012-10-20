namespace neuron {
	partial class Form1 {
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
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.btn_addbox = new System.Windows.Forms.Button();
			this.btn_sel = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.btn_sel);
			this.splitContainer1.Panel1.Controls.Add(this.btn_addbox);
			// 
			// btn_addbox
			// 
			this.btn_addbox.Location = new System.Drawing.Point(81, 71);
			this.btn_addbox.Name = "btn_addbox";
			this.btn_addbox.Size = new System.Drawing.Size(75, 23);
			this.btn_addbox.TabIndex = 0;
			this.btn_addbox.Text = "Add Box";
			this.btn_addbox.UseVisualStyleBackColor = true;
			this.btn_addbox.Click += new System.EventHandler(this.btn_addbox_Click);
			// 
			// btn_sel
			// 
			this.btn_sel.Location = new System.Drawing.Point(56, 323);
			this.btn_sel.Name = "btn_sel";
			this.btn_sel.Size = new System.Drawing.Size(75, 23);
			this.btn_sel.TabIndex = 1;
			this.btn_sel.Text = "Select";
			this.btn_sel.UseVisualStyleBackColor = true;
			this.btn_sel.Click += new System.EventHandler(this.btn_sel_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(783, 493);
			this.Controls.Add(this.splitContainer1);
			this.Name = "Form1";
			this.Text = "Neuron";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.Button btn_addbox;
		private System.Windows.Forms.Button btn_sel;
	}
}

