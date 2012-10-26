namespace Neuron
{
    partial class Neuron
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btn_add = new System.Windows.Forms.Button();
            this.btn_remove = new System.Windows.Forms.Button();
            this.btn_addLogHiddenLayer = new System.Windows.Forms.Button();
            this.btn_addLinHiddenLayer = new System.Windows.Forms.Button();
            this.btn_addLogOutputLayer = new System.Windows.Forms.Button();
            this.btn_addLinOutputLayer = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.btn_remove);
            this.splitContainer1.Panel1.Controls.Add(this.btn_addLinOutputLayer);
            this.splitContainer1.Panel1.Controls.Add(this.btn_addLogOutputLayer);
            this.splitContainer1.Panel1.Controls.Add(this.btn_addLinHiddenLayer);
            this.splitContainer1.Panel1.Controls.Add(this.btn_addLogHiddenLayer);
            this.splitContainer1.Panel1.Controls.Add(this.btn_add);
            this.splitContainer1.Size = new System.Drawing.Size(938, 654);
            this.splitContainer1.SplitterDistance = 312;
            this.splitContainer1.TabIndex = 0;
            // 
            // btn_add
            // 
            this.btn_add.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_add.Location = new System.Drawing.Point(0, 0);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(312, 23);
            this.btn_add.TabIndex = 0;
            this.btn_add.Text = "Add Feature Vector";
            this.btn_add.UseVisualStyleBackColor = true;
            this.btn_add.Click += new System.EventHandler(this.btn_add_Click);
            // 
            // btn_remove
            // 
            this.btn_remove.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_remove.Location = new System.Drawing.Point(0, 115);
            this.btn_remove.Name = "btn_remove";
            this.btn_remove.Size = new System.Drawing.Size(312, 23);
            this.btn_remove.TabIndex = 1;
            this.btn_remove.Text = "Remove Layer";
            this.btn_remove.UseVisualStyleBackColor = true;
            this.btn_remove.Click += new System.EventHandler(this.btn_remove_Click);
            // 
            // btn_addLogHiddenLayer
            // 
            this.btn_addLogHiddenLayer.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_addLogHiddenLayer.Location = new System.Drawing.Point(0, 23);
            this.btn_addLogHiddenLayer.Name = "btn_addLogHiddenLayer";
            this.btn_addLogHiddenLayer.Size = new System.Drawing.Size(312, 23);
            this.btn_addLogHiddenLayer.TabIndex = 2;
            this.btn_addLogHiddenLayer.Text = "Add Logistic Hidden Layer";
            this.btn_addLogHiddenLayer.UseVisualStyleBackColor = true;
            this.btn_addLogHiddenLayer.Click += new System.EventHandler(this.btn_addLogHiddenLayer_Click);
            // 
            // btn_addLinHiddenLayer
            // 
            this.btn_addLinHiddenLayer.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_addLinHiddenLayer.Location = new System.Drawing.Point(0, 46);
            this.btn_addLinHiddenLayer.Name = "btn_addLinHiddenLayer";
            this.btn_addLinHiddenLayer.Size = new System.Drawing.Size(312, 23);
            this.btn_addLinHiddenLayer.TabIndex = 3;
            this.btn_addLinHiddenLayer.Text = "Add Linear Hidden Layer";
            this.btn_addLinHiddenLayer.UseVisualStyleBackColor = true;
            this.btn_addLinHiddenLayer.Click += new System.EventHandler(this.btn_addLinHiddenLayer_Click);
            // 
            // btn_addLogOutputLayer
            // 
            this.btn_addLogOutputLayer.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_addLogOutputLayer.Location = new System.Drawing.Point(0, 69);
            this.btn_addLogOutputLayer.Name = "btn_addLogOutputLayer";
            this.btn_addLogOutputLayer.Size = new System.Drawing.Size(312, 23);
            this.btn_addLogOutputLayer.TabIndex = 4;
            this.btn_addLogOutputLayer.Text = "Add Logistic Output Layer";
            this.btn_addLogOutputLayer.UseVisualStyleBackColor = true;
            this.btn_addLogOutputLayer.Click += new System.EventHandler(this.btn_addLogOutputLayer_Click);
            // 
            // btn_addLinOutputLayer
            // 
            this.btn_addLinOutputLayer.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_addLinOutputLayer.Location = new System.Drawing.Point(0, 92);
            this.btn_addLinOutputLayer.Name = "btn_addLinOutputLayer";
            this.btn_addLinOutputLayer.Size = new System.Drawing.Size(312, 23);
            this.btn_addLinOutputLayer.TabIndex = 5;
            this.btn_addLinOutputLayer.Text = "Add Linear Output Layer";
            this.btn_addLinOutputLayer.UseVisualStyleBackColor = true;
            this.btn_addLinOutputLayer.Click += new System.EventHandler(this.btn_addLinOutputLayer_Click);
            // 
            // Neuron
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(938, 654);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Neuron";
            this.Text = "Neuron";
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btn_remove;
        private System.Windows.Forms.Button btn_add;
        private System.Windows.Forms.Button btn_addLinOutputLayer;
        private System.Windows.Forms.Button btn_addLogOutputLayer;
        private System.Windows.Forms.Button btn_addLinHiddenLayer;
        private System.Windows.Forms.Button btn_addLogHiddenLayer;
    }
}

