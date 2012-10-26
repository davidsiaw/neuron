namespace Neuron
{
    partial class Neurone
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
            this.btn_addConfiguration = new System.Windows.Forms.Button();
            this.chk_autoForwardProp = new System.Windows.Forms.CheckBox();
            this.btn_initialize = new System.Windows.Forms.Button();
            this.btn_train = new System.Windows.Forms.Button();
            this.btn_remove = new System.Windows.Forms.Button();
            this.btn_addLogOutputLayer = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btn_addLogHiddenLayer = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.btn_add = new System.Windows.Forms.Button();
            this.tab = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.data_configurations = new System.Windows.Forms.DataGridView();
            this.col_data_moveToTrain = new System.Windows.Forms.DataGridViewButtonColumn();
            this.col_data_moveToCV = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Configuration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.data_training = new System.Windows.Forms.DataGridView();
            this.dataGridViewButtonColumn1 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dataGridViewButtonColumn2 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.data_crossValidationData = new System.Windows.Forms.DataGridView();
            this.dataGridViewButtonColumn3 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dataGridViewButtonColumn4 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_Save = new System.Windows.Forms.Button();
            this.btn_load = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tab.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.data_configurations)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.data_training)).BeginInit();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.data_crossValidationData)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btn_load);
            this.splitContainer1.Panel1.Controls.Add(this.btn_Save);
            this.splitContainer1.Panel1.Controls.Add(this.btn_addConfiguration);
            this.splitContainer1.Panel1.Controls.Add(this.chk_autoForwardProp);
            this.splitContainer1.Panel1.Controls.Add(this.btn_initialize);
            this.splitContainer1.Panel1.Controls.Add(this.btn_train);
            this.splitContainer1.Panel1.Controls.Add(this.btn_remove);
            this.splitContainer1.Panel1.Controls.Add(this.btn_addLogOutputLayer);
            this.splitContainer1.Panel1.Controls.Add(this.button2);
            this.splitContainer1.Panel1.Controls.Add(this.btn_addLogHiddenLayer);
            this.splitContainer1.Panel1.Controls.Add(this.button1);
            this.splitContainer1.Panel1.Controls.Add(this.btn_add);
            this.splitContainer1.Size = new System.Drawing.Size(924, 622);
            this.splitContainer1.SplitterDistance = 312;
            this.splitContainer1.TabIndex = 0;
            // 
            // btn_addConfiguration
            // 
            this.btn_addConfiguration.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_addConfiguration.Location = new System.Drawing.Point(0, 207);
            this.btn_addConfiguration.Name = "btn_addConfiguration";
            this.btn_addConfiguration.Size = new System.Drawing.Size(312, 23);
            this.btn_addConfiguration.TabIndex = 13;
            this.btn_addConfiguration.Text = "Record Current Configuration";
            this.btn_addConfiguration.UseVisualStyleBackColor = true;
            this.btn_addConfiguration.Click += new System.EventHandler(this.btn_addConfiguration_Click);
            // 
            // chk_autoForwardProp
            // 
            this.chk_autoForwardProp.Appearance = System.Windows.Forms.Appearance.Button;
            this.chk_autoForwardProp.AutoSize = true;
            this.chk_autoForwardProp.Dock = System.Windows.Forms.DockStyle.Top;
            this.chk_autoForwardProp.Location = new System.Drawing.Point(0, 184);
            this.chk_autoForwardProp.Name = "chk_autoForwardProp";
            this.chk_autoForwardProp.Size = new System.Drawing.Size(312, 23);
            this.chk_autoForwardProp.TabIndex = 11;
            this.chk_autoForwardProp.Text = "EvaluateFromInputs";
            this.chk_autoForwardProp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chk_autoForwardProp.UseVisualStyleBackColor = true;
            this.chk_autoForwardProp.Click += new System.EventHandler(this.btn_forwardProp_Click);
            // 
            // btn_initialize
            // 
            this.btn_initialize.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_initialize.Location = new System.Drawing.Point(0, 161);
            this.btn_initialize.Name = "btn_initialize";
            this.btn_initialize.Size = new System.Drawing.Size(312, 23);
            this.btn_initialize.TabIndex = 8;
            this.btn_initialize.Text = "Initialize Weights";
            this.btn_initialize.UseVisualStyleBackColor = true;
            this.btn_initialize.Click += new System.EventHandler(this.btn_initialize_Click);
            // 
            // btn_train
            // 
            this.btn_train.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_train.Location = new System.Drawing.Point(0, 138);
            this.btn_train.Name = "btn_train";
            this.btn_train.Size = new System.Drawing.Size(312, 23);
            this.btn_train.TabIndex = 6;
            this.btn_train.Text = "Train With Training Data";
            this.btn_train.UseVisualStyleBackColor = true;
            this.btn_train.Click += new System.EventHandler(this.btn_train_Click);
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
            // btn_addLogOutputLayer
            // 
            this.btn_addLogOutputLayer.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.btn_addLogOutputLayer.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_addLogOutputLayer.Location = new System.Drawing.Point(0, 92);
            this.btn_addLogOutputLayer.Name = "btn_addLogOutputLayer";
            this.btn_addLogOutputLayer.Size = new System.Drawing.Size(312, 23);
            this.btn_addLogOutputLayer.TabIndex = 4;
            this.btn_addLogOutputLayer.Text = "Add Logistic Output Layer";
            this.btn_addLogOutputLayer.UseVisualStyleBackColor = false;
            this.btn_addLogOutputLayer.Click += new System.EventHandler(this.btn_addLogOutputLayer_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.LightBlue;
            this.button2.Dock = System.Windows.Forms.DockStyle.Top;
            this.button2.Location = new System.Drawing.Point(0, 69);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(312, 23);
            this.button2.TabIndex = 10;
            this.button2.Text = "Add Linear Output Layer";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.btn_addLinOutputLayer_Click);
            // 
            // btn_addLogHiddenLayer
            // 
            this.btn_addLogHiddenLayer.BackColor = System.Drawing.Color.HotPink;
            this.btn_addLogHiddenLayer.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_addLogHiddenLayer.Location = new System.Drawing.Point(0, 46);
            this.btn_addLogHiddenLayer.Name = "btn_addLogHiddenLayer";
            this.btn_addLogHiddenLayer.Size = new System.Drawing.Size(312, 23);
            this.btn_addLogHiddenLayer.TabIndex = 2;
            this.btn_addLogHiddenLayer.Text = "Add Logistic Hidden Layer";
            this.btn_addLogHiddenLayer.UseVisualStyleBackColor = false;
            this.btn_addLogHiddenLayer.Click += new System.EventHandler(this.btn_addLogHiddenLayer_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Pink;
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.Location = new System.Drawing.Point(0, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(312, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "Add Linear Hidden Layer";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.btn_addLinHiddenLayer_Click);
            // 
            // btn_add
            // 
            this.btn_add.BackColor = System.Drawing.Color.LightGreen;
            this.btn_add.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_add.Location = new System.Drawing.Point(0, 0);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(312, 23);
            this.btn_add.TabIndex = 0;
            this.btn_add.Text = "Add Feature Vector";
            this.btn_add.UseVisualStyleBackColor = false;
            this.btn_add.Click += new System.EventHandler(this.btn_add_Click);
            // 
            // tab
            // 
            this.tab.Controls.Add(this.tabPage1);
            this.tab.Controls.Add(this.tabPage2);
            this.tab.Controls.Add(this.tabPage3);
            this.tab.Controls.Add(this.tabPage4);
            this.tab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tab.Location = new System.Drawing.Point(0, 0);
            this.tab.Name = "tab";
            this.tab.SelectedIndex = 0;
            this.tab.Size = new System.Drawing.Size(938, 654);
            this.tab.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(930, 628);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Topography";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.data_configurations);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(930, 628);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Data";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // data_configurations
            // 
            this.data_configurations.AllowUserToAddRows = false;
            this.data_configurations.AllowUserToDeleteRows = false;
            this.data_configurations.AllowUserToResizeColumns = false;
            this.data_configurations.AllowUserToResizeRows = false;
            this.data_configurations.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.data_configurations.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.col_data_moveToTrain,
            this.col_data_moveToCV,
            this.Configuration});
            this.data_configurations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.data_configurations.Location = new System.Drawing.Point(3, 3);
            this.data_configurations.Name = "data_configurations";
            this.data_configurations.RowHeadersVisible = false;
            this.data_configurations.Size = new System.Drawing.Size(924, 622);
            this.data_configurations.TabIndex = 0;
            this.data_configurations.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.data_configurations_CellClick);
            // 
            // col_data_moveToTrain
            // 
            this.col_data_moveToTrain.HeaderText = "";
            this.col_data_moveToTrain.Name = "col_data_moveToTrain";
            this.col_data_moveToTrain.Text = "";
            // 
            // col_data_moveToCV
            // 
            this.col_data_moveToCV.HeaderText = "";
            this.col_data_moveToCV.Name = "col_data_moveToCV";
            // 
            // Configuration
            // 
            this.Configuration.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Configuration.HeaderText = "Configuration";
            this.Configuration.Name = "Configuration";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.data_training);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(930, 628);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Training Data";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // data_training
            // 
            this.data_training.AllowUserToAddRows = false;
            this.data_training.AllowUserToDeleteRows = false;
            this.data_training.AllowUserToResizeColumns = false;
            this.data_training.AllowUserToResizeRows = false;
            this.data_training.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.data_training.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewButtonColumn1,
            this.dataGridViewButtonColumn2,
            this.dataGridViewTextBoxColumn1});
            this.data_training.Dock = System.Windows.Forms.DockStyle.Fill;
            this.data_training.Location = new System.Drawing.Point(3, 3);
            this.data_training.Name = "data_training";
            this.data_training.RowHeadersVisible = false;
            this.data_training.Size = new System.Drawing.Size(924, 622);
            this.data_training.TabIndex = 1;
            this.data_training.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.data_training_CellClick);
            // 
            // dataGridViewButtonColumn1
            // 
            this.dataGridViewButtonColumn1.HeaderText = "";
            this.dataGridViewButtonColumn1.Name = "dataGridViewButtonColumn1";
            this.dataGridViewButtonColumn1.Text = "";
            // 
            // dataGridViewButtonColumn2
            // 
            this.dataGridViewButtonColumn2.HeaderText = "";
            this.dataGridViewButtonColumn2.Name = "dataGridViewButtonColumn2";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.HeaderText = "Configuration";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.data_crossValidationData);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(930, 628);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Cross Validation Data";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // data_crossValidationData
            // 
            this.data_crossValidationData.AllowUserToAddRows = false;
            this.data_crossValidationData.AllowUserToDeleteRows = false;
            this.data_crossValidationData.AllowUserToResizeColumns = false;
            this.data_crossValidationData.AllowUserToResizeRows = false;
            this.data_crossValidationData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.data_crossValidationData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewButtonColumn3,
            this.dataGridViewButtonColumn4,
            this.dataGridViewTextBoxColumn2});
            this.data_crossValidationData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.data_crossValidationData.Location = new System.Drawing.Point(3, 3);
            this.data_crossValidationData.Name = "data_crossValidationData";
            this.data_crossValidationData.RowHeadersVisible = false;
            this.data_crossValidationData.Size = new System.Drawing.Size(924, 622);
            this.data_crossValidationData.TabIndex = 1;
            this.data_crossValidationData.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.data_crossValidationData_CellClick);
            // 
            // dataGridViewButtonColumn3
            // 
            this.dataGridViewButtonColumn3.HeaderText = "";
            this.dataGridViewButtonColumn3.Name = "dataGridViewButtonColumn3";
            this.dataGridViewButtonColumn3.Text = "";
            // 
            // dataGridViewButtonColumn4
            // 
            this.dataGridViewButtonColumn4.HeaderText = "";
            this.dataGridViewButtonColumn4.Name = "dataGridViewButtonColumn4";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.HeaderText = "Configuration";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // btn_Save
            // 
            this.btn_Save.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_Save.Location = new System.Drawing.Point(0, 230);
            this.btn_Save.Name = "btn_Save";
            this.btn_Save.Size = new System.Drawing.Size(312, 23);
            this.btn_Save.TabIndex = 14;
            this.btn_Save.Text = "Save State";
            this.btn_Save.UseVisualStyleBackColor = true;
            this.btn_Save.Click += new System.EventHandler(this.btn_Save_Click);
            // 
            // btn_load
            // 
            this.btn_load.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_load.Location = new System.Drawing.Point(0, 253);
            this.btn_load.Name = "btn_load";
            this.btn_load.Size = new System.Drawing.Size(312, 23);
            this.btn_load.TabIndex = 15;
            this.btn_load.Text = "Load State";
            this.btn_load.UseVisualStyleBackColor = true;
            this.btn_load.Click += new System.EventHandler(this.btn_load_Click);
            // 
            // Neurone
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(938, 654);
            this.Controls.Add(this.tab);
            this.Name = "Neurone";
            this.Text = "Neuron";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tab.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.data_configurations)).EndInit();
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.data_training)).EndInit();
            this.tabPage4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.data_crossValidationData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btn_remove;
        private System.Windows.Forms.Button btn_add;
        private System.Windows.Forms.Button btn_addLogOutputLayer;
        private System.Windows.Forms.Button btn_addLogHiddenLayer;
        private System.Windows.Forms.Button btn_train;
        private System.Windows.Forms.Button btn_initialize;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox chk_autoForwardProp;
        private System.Windows.Forms.Button btn_addConfiguration;
        private System.Windows.Forms.TabControl tab;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView data_configurations;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataGridView data_training;
        private System.Windows.Forms.DataGridViewButtonColumn dataGridViewButtonColumn1;
        private System.Windows.Forms.DataGridViewButtonColumn dataGridViewButtonColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.DataGridView data_crossValidationData;
        private System.Windows.Forms.DataGridViewButtonColumn dataGridViewButtonColumn3;
        private System.Windows.Forms.DataGridViewButtonColumn dataGridViewButtonColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewButtonColumn col_data_moveToTrain;
        private System.Windows.Forms.DataGridViewButtonColumn col_data_moveToCV;
        private System.Windows.Forms.DataGridViewTextBoxColumn Configuration;
        private System.Windows.Forms.Button btn_load;
        private System.Windows.Forms.Button btn_Save;
    }
}

