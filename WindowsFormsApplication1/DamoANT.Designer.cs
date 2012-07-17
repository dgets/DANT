namespace WindowsFormsApplication1
{
    partial class frmDamoANTs
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnEditAlarm = new System.Windows.Forms.Button();
            this.lblWipeAlarm = new System.Windows.Forms.Label();
            this.btnToastAlarm = new System.Windows.Forms.Button();
            this.lblAlarmName = new System.Windows.Forms.Label();
            this.txtAlarmName = new System.Windows.Forms.TextBox();
            this.btnAddAlarm = new System.Windows.Forms.Button();
            this.lblAlarmAddTime = new System.Windows.Forms.Label();
            this.numAlarmSec = new System.Windows.Forms.NumericUpDown();
            this.numAlarmMin = new System.Windows.Forms.NumericUpDown();
            this.numAlarmHr = new System.Windows.Forms.NumericUpDown();
            this.chklstAlarms = new System.Windows.Forms.CheckedListBox();
            this.lblAlarms = new System.Windows.Forms.Label();
            this.lblWipeTimer = new System.Windows.Forms.Label();
            this.btnToastTimer = new System.Windows.Forms.Button();
            this.lblTimerName = new System.Windows.Forms.Label();
            this.txtTimerName = new System.Windows.Forms.TextBox();
            this.btnAddTimer = new System.Windows.Forms.Button();
            this.lblTimerAddTime = new System.Windows.Forms.Label();
            this.numTimerSec = new System.Windows.Forms.NumericUpDown();
            this.numTimerMin = new System.Windows.Forms.NumericUpDown();
            this.numTimerHr = new System.Windows.Forms.NumericUpDown();
            this.chklstTimers = new System.Windows.Forms.CheckedListBox();
            this.lblTimers = new System.Windows.Forms.Label();
            this.tmrOneSec = new System.Windows.Forms.Timer(this.components);
            this.openSoundFile = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAlarmSec)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAlarmMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAlarmHr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTimerSec)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTimerMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTimerHr)).BeginInit();
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
            this.splitContainer1.Panel1.Controls.Add(this.btnEditAlarm);
            this.splitContainer1.Panel1.Controls.Add(this.lblWipeAlarm);
            this.splitContainer1.Panel1.Controls.Add(this.btnToastAlarm);
            this.splitContainer1.Panel1.Controls.Add(this.lblAlarmName);
            this.splitContainer1.Panel1.Controls.Add(this.txtAlarmName);
            this.splitContainer1.Panel1.Controls.Add(this.btnAddAlarm);
            this.splitContainer1.Panel1.Controls.Add(this.lblAlarmAddTime);
            this.splitContainer1.Panel1.Controls.Add(this.numAlarmSec);
            this.splitContainer1.Panel1.Controls.Add(this.numAlarmMin);
            this.splitContainer1.Panel1.Controls.Add(this.numAlarmHr);
            this.splitContainer1.Panel1.Controls.Add(this.chklstAlarms);
            this.splitContainer1.Panel1.Controls.Add(this.lblAlarms);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lblWipeTimer);
            this.splitContainer1.Panel2.Controls.Add(this.btnToastTimer);
            this.splitContainer1.Panel2.Controls.Add(this.lblTimerName);
            this.splitContainer1.Panel2.Controls.Add(this.txtTimerName);
            this.splitContainer1.Panel2.Controls.Add(this.btnAddTimer);
            this.splitContainer1.Panel2.Controls.Add(this.lblTimerAddTime);
            this.splitContainer1.Panel2.Controls.Add(this.numTimerSec);
            this.splitContainer1.Panel2.Controls.Add(this.numTimerMin);
            this.splitContainer1.Panel2.Controls.Add(this.numTimerHr);
            this.splitContainer1.Panel2.Controls.Add(this.chklstTimers);
            this.splitContainer1.Panel2.Controls.Add(this.lblTimers);
            this.splitContainer1.Size = new System.Drawing.Size(515, 432);
            this.splitContainer1.SplitterDistance = 262;
            this.splitContainer1.TabIndex = 0;
            // 
            // btnEditAlarm
            // 
            this.btnEditAlarm.Location = new System.Drawing.Point(96, 375);
            this.btnEditAlarm.Name = "btnEditAlarm";
            this.btnEditAlarm.Size = new System.Drawing.Size(75, 23);
            this.btnEditAlarm.TabIndex = 1;
            this.btnEditAlarm.Text = "Edit Alarm";
            this.btnEditAlarm.UseVisualStyleBackColor = true;
            this.btnEditAlarm.Click += new System.EventHandler(this.btnEditAlarm_Click);
            // 
            // lblWipeAlarm
            // 
            this.lblWipeAlarm.AutoSize = true;
            this.lblWipeAlarm.Location = new System.Drawing.Point(9, 359);
            this.lblWipeAlarm.Name = "lblWipeAlarm";
            this.lblWipeAlarm.Size = new System.Drawing.Size(78, 13);
            this.lblWipeAlarm.TabIndex = 1;
            this.lblWipeAlarm.Text = "Check first and";
            // 
            // btnToastAlarm
            // 
            this.btnToastAlarm.Location = new System.Drawing.Point(12, 375);
            this.btnToastAlarm.Name = "btnToastAlarm";
            this.btnToastAlarm.Size = new System.Drawing.Size(75, 23);
            this.btnToastAlarm.TabIndex = 1;
            this.btnToastAlarm.Text = "Wipe Alarm";
            this.btnToastAlarm.UseVisualStyleBackColor = true;
            this.btnToastAlarm.Click += new System.EventHandler(this.btnToastAlarm_Click);
            // 
            // lblAlarmName
            // 
            this.lblAlarmName.AutoSize = true;
            this.lblAlarmName.Location = new System.Drawing.Point(118, 305);
            this.lblAlarmName.Name = "lblAlarmName";
            this.lblAlarmName.Size = new System.Drawing.Size(64, 13);
            this.lblAlarmName.TabIndex = 12;
            this.lblAlarmName.Text = "Alarm Name";
            // 
            // txtAlarmName
            // 
            this.txtAlarmName.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.txtAlarmName.Location = new System.Drawing.Point(12, 302);
            this.txtAlarmName.Name = "txtAlarmName";
            this.txtAlarmName.Size = new System.Drawing.Size(100, 20);
            this.txtAlarmName.TabIndex = 11;
            this.txtAlarmName.Text = "Alarm Name Here";
            this.txtAlarmName.Enter += new System.EventHandler(this.txtAlarmName_Enter);
            this.txtAlarmName.Leave += new System.EventHandler(this.txtAlarmName_Leave);
            // 
            // btnAddAlarm
            // 
            this.btnAddAlarm.Location = new System.Drawing.Point(12, 327);
            this.btnAddAlarm.Name = "btnAddAlarm";
            this.btnAddAlarm.Size = new System.Drawing.Size(75, 23);
            this.btnAddAlarm.TabIndex = 5;
            this.btnAddAlarm.Text = "Add Alarm";
            this.btnAddAlarm.UseVisualStyleBackColor = true;
            this.btnAddAlarm.Click += new System.EventHandler(this.btnAddAlarm_Click);
            // 
            // lblAlarmAddTime
            // 
            this.lblAlarmAddTime.AutoSize = true;
            this.lblAlarmAddTime.Location = new System.Drawing.Point(137, 278);
            this.lblAlarmAddTime.Name = "lblAlarmAddTime";
            this.lblAlarmAddTime.Size = new System.Drawing.Size(59, 13);
            this.lblAlarmAddTime.TabIndex = 4;
            this.lblAlarmAddTime.Text = "Alarm Time";
            // 
            // numAlarmSec
            // 
            this.numAlarmSec.CausesValidation = false;
            this.numAlarmSec.Location = new System.Drawing.Point(96, 276);
            this.numAlarmSec.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.numAlarmSec.Name = "numAlarmSec";
            this.numAlarmSec.Size = new System.Drawing.Size(35, 20);
            this.numAlarmSec.TabIndex = 3;
            // 
            // numAlarmMin
            // 
            this.numAlarmMin.CausesValidation = false;
            this.numAlarmMin.Location = new System.Drawing.Point(54, 276);
            this.numAlarmMin.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.numAlarmMin.Name = "numAlarmMin";
            this.numAlarmMin.Size = new System.Drawing.Size(36, 20);
            this.numAlarmMin.TabIndex = 2;
            // 
            // numAlarmHr
            // 
            this.numAlarmHr.CausesValidation = false;
            this.numAlarmHr.Location = new System.Drawing.Point(12, 276);
            this.numAlarmHr.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.numAlarmHr.Name = "numAlarmHr";
            this.numAlarmHr.Size = new System.Drawing.Size(36, 20);
            this.numAlarmHr.TabIndex = 1;
            // 
            // chklstAlarms
            // 
            this.chklstAlarms.CheckOnClick = true;
            this.chklstAlarms.FormattingEnabled = true;
            this.chklstAlarms.Location = new System.Drawing.Point(12, 56);
            this.chklstAlarms.Name = "chklstAlarms";
            this.chklstAlarms.Size = new System.Drawing.Size(238, 214);
            this.chklstAlarms.TabIndex = 1;
            this.chklstAlarms.SelectedIndexChanged += new System.EventHandler(this.chklstAlarms_SelectedIndexChanged);
            // 
            // lblAlarms
            // 
            this.lblAlarms.AutoSize = true;
            this.lblAlarms.Font = new System.Drawing.Font("Viner Hand ITC", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAlarms.Location = new System.Drawing.Point(12, 9);
            this.lblAlarms.Name = "lblAlarms";
            this.lblAlarms.Size = new System.Drawing.Size(113, 44);
            this.lblAlarms.TabIndex = 0;
            this.lblAlarms.Text = "Alarms";
            // 
            // lblWipeTimer
            // 
            this.lblWipeTimer.AutoSize = true;
            this.lblWipeTimer.Location = new System.Drawing.Point(8, 359);
            this.lblWipeTimer.Name = "lblWipeTimer";
            this.lblWipeTimer.Size = new System.Drawing.Size(78, 13);
            this.lblWipeTimer.TabIndex = 13;
            this.lblWipeTimer.Text = "Check first and";
            // 
            // btnToastTimer
            // 
            this.btnToastTimer.Location = new System.Drawing.Point(11, 375);
            this.btnToastTimer.Name = "btnToastTimer";
            this.btnToastTimer.Size = new System.Drawing.Size(75, 23);
            this.btnToastTimer.TabIndex = 13;
            this.btnToastTimer.Text = "Wipe Timer";
            this.btnToastTimer.UseVisualStyleBackColor = true;
            // 
            // lblTimerName
            // 
            this.lblTimerName.AutoSize = true;
            this.lblTimerName.Location = new System.Drawing.Point(117, 305);
            this.lblTimerName.Name = "lblTimerName";
            this.lblTimerName.Size = new System.Drawing.Size(64, 13);
            this.lblTimerName.TabIndex = 14;
            this.lblTimerName.Text = "Timer Name";
            // 
            // txtTimerName
            // 
            this.txtTimerName.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.txtTimerName.Location = new System.Drawing.Point(11, 302);
            this.txtTimerName.Name = "txtTimerName";
            this.txtTimerName.Size = new System.Drawing.Size(100, 20);
            this.txtTimerName.TabIndex = 13;
            this.txtTimerName.Text = "Timer Name Here";
            this.txtTimerName.Enter += new System.EventHandler(this.txtTimerName_Enter);
            this.txtTimerName.Leave += new System.EventHandler(this.txtTimerName_Leave);
            // 
            // btnAddTimer
            // 
            this.btnAddTimer.Location = new System.Drawing.Point(11, 327);
            this.btnAddTimer.Name = "btnAddTimer";
            this.btnAddTimer.Size = new System.Drawing.Size(75, 23);
            this.btnAddTimer.TabIndex = 10;
            this.btnAddTimer.Text = "Add Timer";
            this.btnAddTimer.UseVisualStyleBackColor = true;
            // 
            // lblTimerAddTime
            // 
            this.lblTimerAddTime.AutoSize = true;
            this.lblTimerAddTime.Location = new System.Drawing.Point(136, 278);
            this.lblTimerAddTime.Name = "lblTimerAddTime";
            this.lblTimerAddTime.Size = new System.Drawing.Size(71, 13);
            this.lblTimerAddTime.TabIndex = 9;
            this.lblTimerAddTime.Text = "Timer Interval";
            // 
            // numTimerSec
            // 
            this.numTimerSec.CausesValidation = false;
            this.numTimerSec.Location = new System.Drawing.Point(95, 276);
            this.numTimerSec.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numTimerSec.Name = "numTimerSec";
            this.numTimerSec.Size = new System.Drawing.Size(35, 20);
            this.numTimerSec.TabIndex = 8;
            // 
            // numTimerMin
            // 
            this.numTimerMin.CausesValidation = false;
            this.numTimerMin.Location = new System.Drawing.Point(52, 276);
            this.numTimerMin.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numTimerMin.Name = "numTimerMin";
            this.numTimerMin.Size = new System.Drawing.Size(37, 20);
            this.numTimerMin.TabIndex = 7;
            // 
            // numTimerHr
            // 
            this.numTimerHr.CausesValidation = false;
            this.numTimerHr.Location = new System.Drawing.Point(11, 276);
            this.numTimerHr.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numTimerHr.Name = "numTimerHr";
            this.numTimerHr.Size = new System.Drawing.Size(35, 20);
            this.numTimerHr.TabIndex = 6;
            // 
            // chklstTimers
            // 
            this.chklstTimers.CheckOnClick = true;
            this.chklstTimers.FormattingEnabled = true;
            this.chklstTimers.Location = new System.Drawing.Point(11, 56);
            this.chklstTimers.Name = "chklstTimers";
            this.chklstTimers.Size = new System.Drawing.Size(226, 214);
            this.chklstTimers.TabIndex = 2;
            // 
            // lblTimers
            // 
            this.lblTimers.AutoSize = true;
            this.lblTimers.Font = new System.Drawing.Font("Viner Hand ITC", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimers.Location = new System.Drawing.Point(3, 9);
            this.lblTimers.Name = "lblTimers";
            this.lblTimers.Size = new System.Drawing.Size(102, 44);
            this.lblTimers.TabIndex = 0;
            this.lblTimers.Text = "Timers";
            // 
            // tmrOneSec
            // 
            this.tmrOneSec.Interval = 1000;
            this.tmrOneSec.Tick += new System.EventHandler(this.tmrOneSec_Tick);
            // 
            // openSoundFile
            // 
            this.openSoundFile.DefaultExt = "mp3";
            this.openSoundFile.FileName = "openFileDialog1";
            this.openSoundFile.Title = "Alarm/Timer Notification Sound";
            // 
            // frmDamoANTs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(515, 432);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmDamoANTs";
            this.Text = "Damo\'s Alarms \'N Timers";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numAlarmSec)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAlarmMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAlarmHr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTimerSec)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTimerMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTimerHr)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label lblAlarms;
        private System.Windows.Forms.CheckedListBox chklstTimers;
        private System.Windows.Forms.Label lblTimers;
        private System.Windows.Forms.Button btnAddAlarm;
        private System.Windows.Forms.Label lblAlarmAddTime;
        private System.Windows.Forms.NumericUpDown numAlarmSec;
        private System.Windows.Forms.NumericUpDown numAlarmMin;
        private System.Windows.Forms.NumericUpDown numAlarmHr;
        private System.Windows.Forms.Button btnAddTimer;
        private System.Windows.Forms.Label lblTimerAddTime;
        private System.Windows.Forms.NumericUpDown numTimerSec;
        private System.Windows.Forms.NumericUpDown numTimerMin;
        private System.Windows.Forms.NumericUpDown numTimerHr;
        private System.Windows.Forms.Timer tmrOneSec;
        private System.Windows.Forms.Label lblAlarmName;
        private System.Windows.Forms.Label lblTimerName;
        private System.Windows.Forms.TextBox txtTimerName;
        private System.Windows.Forms.Button btnToastAlarm;
        private System.Windows.Forms.Button btnToastTimer;
        private System.Windows.Forms.OpenFileDialog openSoundFile;
        private System.Windows.Forms.Label lblWipeAlarm;
        private System.Windows.Forms.Label lblWipeTimer;
        private System.Windows.Forms.Button btnEditAlarm;
        public System.Windows.Forms.CheckedListBox chklstAlarms;
        internal System.Windows.Forms.TextBox txtAlarmName;
    }
}

