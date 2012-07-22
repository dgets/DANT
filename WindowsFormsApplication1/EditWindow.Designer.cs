namespace WindowsFormsApplication1 {
    partial class frmEditWindow {
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
            this.lblEditing = new System.Windows.Forms.Label();
            this.txtEditAlarmName = new System.Windows.Forms.TextBox();
            this.lblEditTime = new System.Windows.Forms.Label();
            this.nudEditHour = new System.Windows.Forms.NumericUpDown();
            this.nudEditMinute = new System.Windows.Forms.NumericUpDown();
            this.nudEditSecond = new System.Windows.Forms.NumericUpDown();
            this.lblEditHour = new System.Windows.Forms.Label();
            this.lblEditMinute = new System.Windows.Forms.Label();
            this.lblEditSecond = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtShowSoundByte = new System.Windows.Forms.TextBox();
            this.btnEditSoundbyte = new System.Windows.Forms.Button();
            this.btnEditCommit = new System.Windows.Forms.Button();
            this.openNewSoundFile = new System.Windows.Forms.OpenFileDialog();
            this.grpboxEntryType = new System.Windows.Forms.GroupBox();
            this.radioTimerType = new System.Windows.Forms.RadioButton();
            this.radioAlarmType = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.nudEditHour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEditMinute)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEditSecond)).BeginInit();
            this.grpboxEntryType.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblEditing
            // 
            this.lblEditing.AutoSize = true;
            this.lblEditing.Location = new System.Drawing.Point(12, 9);
            this.lblEditing.Name = "lblEditing";
            this.lblEditing.Size = new System.Drawing.Size(42, 13);
            this.lblEditing.TabIndex = 0;
            this.lblEditing.Text = "Editing:";
            // 
            // txtEditAlarmName
            // 
            this.txtEditAlarmName.Location = new System.Drawing.Point(60, 6);
            this.txtEditAlarmName.Name = "txtEditAlarmName";
            this.txtEditAlarmName.Size = new System.Drawing.Size(212, 20);
            this.txtEditAlarmName.TabIndex = 1;
            // 
            // lblEditTime
            // 
            this.lblEditTime.AutoSize = true;
            this.lblEditTime.Location = new System.Drawing.Point(12, 43);
            this.lblEditTime.Name = "lblEditTime";
            this.lblEditTime.Size = new System.Drawing.Size(85, 26);
            this.lblEditTime.TabIndex = 2;
            this.lblEditTime.Text = "Edit Target Time\r\nor Time Interval:";
            // 
            // nudEditHour
            // 
            this.nudEditHour.Location = new System.Drawing.Point(71, 76);
            this.nudEditHour.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.nudEditHour.Name = "nudEditHour";
            this.nudEditHour.Size = new System.Drawing.Size(39, 20);
            this.nudEditHour.TabIndex = 3;
            // 
            // nudEditMinute
            // 
            this.nudEditMinute.Location = new System.Drawing.Point(71, 102);
            this.nudEditMinute.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.nudEditMinute.Name = "nudEditMinute";
            this.nudEditMinute.Size = new System.Drawing.Size(39, 20);
            this.nudEditMinute.TabIndex = 4;
            // 
            // nudEditSecond
            // 
            this.nudEditSecond.Location = new System.Drawing.Point(71, 128);
            this.nudEditSecond.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.nudEditSecond.Name = "nudEditSecond";
            this.nudEditSecond.Size = new System.Drawing.Size(39, 20);
            this.nudEditSecond.TabIndex = 5;
            // 
            // lblEditHour
            // 
            this.lblEditHour.AutoSize = true;
            this.lblEditHour.Location = new System.Drawing.Point(12, 78);
            this.lblEditHour.Name = "lblEditHour";
            this.lblEditHour.Size = new System.Drawing.Size(44, 13);
            this.lblEditHour.TabIndex = 6;
            this.lblEditHour.Text = "Hour(s):";
            // 
            // lblEditMinute
            // 
            this.lblEditMinute.AutoSize = true;
            this.lblEditMinute.Location = new System.Drawing.Point(12, 104);
            this.lblEditMinute.Name = "lblEditMinute";
            this.lblEditMinute.Size = new System.Drawing.Size(53, 13);
            this.lblEditMinute.TabIndex = 7;
            this.lblEditMinute.Text = "Minute(s):";
            // 
            // lblEditSecond
            // 
            this.lblEditSecond.AutoSize = true;
            this.lblEditSecond.Location = new System.Drawing.Point(12, 130);
            this.lblEditSecond.Name = "lblEditSecond";
            this.lblEditSecond.Size = new System.Drawing.Size(58, 13);
            this.lblEditSecond.TabIndex = 8;
            this.lblEditSecond.Text = "Second(s):";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(142, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 26);
            this.label1.TabIndex = 9;
            this.label1.Text = "Edit Soundbyte\r\nCurrently Set To:";
            // 
            // txtShowSoundByte
            // 
            this.txtShowSoundByte.Location = new System.Drawing.Point(145, 58);
            this.txtShowSoundByte.Name = "txtShowSoundByte";
            this.txtShowSoundByte.ReadOnly = true;
            this.txtShowSoundByte.Size = new System.Drawing.Size(100, 20);
            this.txtShowSoundByte.TabIndex = 10;
            // 
            // btnEditSoundbyte
            // 
            this.btnEditSoundbyte.Location = new System.Drawing.Point(158, 84);
            this.btnEditSoundbyte.Name = "btnEditSoundbyte";
            this.btnEditSoundbyte.Size = new System.Drawing.Size(70, 44);
            this.btnEditSoundbyte.TabIndex = 11;
            this.btnEditSoundbyte.Text = "Change\r\nSoundbyte";
            this.btnEditSoundbyte.UseVisualStyleBackColor = true;
            this.btnEditSoundbyte.Click += new System.EventHandler(this.btnEditSoundbyte_Click);
            // 
            // btnEditCommit
            // 
            this.btnEditCommit.Location = new System.Drawing.Point(15, 162);
            this.btnEditCommit.Name = "btnEditCommit";
            this.btnEditCommit.Size = new System.Drawing.Size(75, 41);
            this.btnEditCommit.TabIndex = 12;
            this.btnEditCommit.Text = "Commit\r\nChanges";
            this.btnEditCommit.UseVisualStyleBackColor = true;
            this.btnEditCommit.Click += new System.EventHandler(this.btnEditCommit_Click);
            // 
            // openNewSoundFile
            // 
            this.openNewSoundFile.FileName = "Not Set";
            // 
            // grpboxEntryType
            // 
            this.grpboxEntryType.Controls.Add(this.radioTimerType);
            this.grpboxEntryType.Controls.Add(this.radioAlarmType);
            this.grpboxEntryType.Location = new System.Drawing.Point(158, 134);
            this.grpboxEntryType.Name = "grpboxEntryType";
            this.grpboxEntryType.Size = new System.Drawing.Size(70, 69);
            this.grpboxEntryType.TabIndex = 13;
            this.grpboxEntryType.TabStop = false;
            this.grpboxEntryType.Text = "Type";
            // 
            // radioTimerType
            // 
            this.radioTimerType.AutoSize = true;
            this.radioTimerType.Enabled = false;
            this.radioTimerType.Location = new System.Drawing.Point(6, 42);
            this.radioTimerType.Name = "radioTimerType";
            this.radioTimerType.Size = new System.Drawing.Size(51, 17);
            this.radioTimerType.TabIndex = 1;
            this.radioTimerType.TabStop = true;
            this.radioTimerType.Text = "Timer";
            this.radioTimerType.UseVisualStyleBackColor = true;
            // 
            // radioAlarmType
            // 
            this.radioAlarmType.AutoSize = true;
            this.radioAlarmType.Enabled = false;
            this.radioAlarmType.Location = new System.Drawing.Point(6, 19);
            this.radioAlarmType.Name = "radioAlarmType";
            this.radioAlarmType.Size = new System.Drawing.Size(51, 17);
            this.radioAlarmType.TabIndex = 0;
            this.radioAlarmType.TabStop = true;
            this.radioAlarmType.Text = "Alarm";
            this.radioAlarmType.UseVisualStyleBackColor = true;
            // 
            // frmEditWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 215);
            this.Controls.Add(this.grpboxEntryType);
            this.Controls.Add(this.btnEditCommit);
            this.Controls.Add(this.btnEditSoundbyte);
            this.Controls.Add(this.txtShowSoundByte);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblEditSecond);
            this.Controls.Add(this.lblEditMinute);
            this.Controls.Add(this.lblEditHour);
            this.Controls.Add(this.nudEditSecond);
            this.Controls.Add(this.nudEditMinute);
            this.Controls.Add(this.nudEditHour);
            this.Controls.Add(this.lblEditTime);
            this.Controls.Add(this.txtEditAlarmName);
            this.Controls.Add(this.lblEditing);
            this.Name = "frmEditWindow";
            this.Text = "Edit Alarm/Timer";
            ((System.ComponentModel.ISupportInitialize)(this.nudEditHour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEditMinute)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEditSecond)).EndInit();
            this.grpboxEntryType.ResumeLayout(false);
            this.grpboxEntryType.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblEditing;
        private System.Windows.Forms.TextBox txtEditAlarmName;
        private System.Windows.Forms.Label lblEditTime;
        private System.Windows.Forms.NumericUpDown nudEditHour;
        private System.Windows.Forms.NumericUpDown nudEditMinute;
        private System.Windows.Forms.NumericUpDown nudEditSecond;
        private System.Windows.Forms.Label lblEditHour;
        private System.Windows.Forms.Label lblEditMinute;
        private System.Windows.Forms.Label lblEditSecond;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtShowSoundByte;
        private System.Windows.Forms.Button btnEditSoundbyte;
        private System.Windows.Forms.Button btnEditCommit;
        private System.Windows.Forms.OpenFileDialog openNewSoundFile;
        private System.Windows.Forms.GroupBox grpboxEntryType;
        private System.Windows.Forms.RadioButton radioTimerType;
        private System.Windows.Forms.RadioButton radioAlarmType;
    }
}