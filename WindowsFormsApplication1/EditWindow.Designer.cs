namespace DamosAlarmsNTimers {
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
            this.lblItemName = new System.Windows.Forms.Label();
            this.txtItemName = new System.Windows.Forms.TextBox();
            this.lblHours = new System.Windows.Forms.Label();
            this.nudHrs = new System.Windows.Forms.NumericUpDown();
            this.lblMinutes = new System.Windows.Forms.Label();
            this.nudMinutes = new System.Windows.Forms.NumericUpDown();
            this.lblSeconds = new System.Windows.Forms.Label();
            this.nudSeconds = new System.Windows.Forms.NumericUpDown();
            this.btnEditSoundbite = new System.Windows.Forms.Button();
            this.openChangedSoundFile = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.nudHrs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinutes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSeconds)).BeginInit();
            this.SuspendLayout();
            // 
            // lblItemName
            // 
            this.lblItemName.AutoSize = true;
            this.lblItemName.Location = new System.Drawing.Point(12, 9);
            this.lblItemName.Name = "lblItemName";
            this.lblItemName.Size = new System.Drawing.Size(61, 13);
            this.lblItemName.TabIndex = 0;
            this.lblItemName.Text = "Item Name:";
            // 
            // txtItemName
            // 
            this.txtItemName.Location = new System.Drawing.Point(79, 6);
            this.txtItemName.Name = "txtItemName";
            this.txtItemName.Size = new System.Drawing.Size(193, 20);
            this.txtItemName.TabIndex = 1;
            // 
            // lblHours
            // 
            this.lblHours.AutoSize = true;
            this.lblHours.Location = new System.Drawing.Point(12, 38);
            this.lblHours.Name = "lblHours";
            this.lblHours.Size = new System.Drawing.Size(44, 13);
            this.lblHours.TabIndex = 2;
            this.lblHours.Text = "Hour(s):";
            // 
            // nudHrs
            // 
            this.nudHrs.Location = new System.Drawing.Point(79, 36);
            this.nudHrs.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.nudHrs.Name = "nudHrs";
            this.nudHrs.Size = new System.Drawing.Size(38, 20);
            this.nudHrs.TabIndex = 3;
            // 
            // lblMinutes
            // 
            this.lblMinutes.AutoSize = true;
            this.lblMinutes.Location = new System.Drawing.Point(12, 62);
            this.lblMinutes.Name = "lblMinutes";
            this.lblMinutes.Size = new System.Drawing.Size(53, 13);
            this.lblMinutes.TabIndex = 4;
            this.lblMinutes.Text = "Minute(s):";
            // 
            // nudMinutes
            // 
            this.nudMinutes.Location = new System.Drawing.Point(79, 60);
            this.nudMinutes.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.nudMinutes.Name = "nudMinutes";
            this.nudMinutes.Size = new System.Drawing.Size(38, 20);
            this.nudMinutes.TabIndex = 5;
            // 
            // lblSeconds
            // 
            this.lblSeconds.AutoSize = true;
            this.lblSeconds.Location = new System.Drawing.Point(12, 86);
            this.lblSeconds.Name = "lblSeconds";
            this.lblSeconds.Size = new System.Drawing.Size(58, 13);
            this.lblSeconds.TabIndex = 6;
            this.lblSeconds.Text = "Second(s):";
            // 
            // nudSeconds
            // 
            this.nudSeconds.Location = new System.Drawing.Point(79, 84);
            this.nudSeconds.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.nudSeconds.Name = "nudSeconds";
            this.nudSeconds.Size = new System.Drawing.Size(38, 20);
            this.nudSeconds.TabIndex = 7;
            // 
            // btnEditSoundbite
            // 
            this.btnEditSoundbite.Location = new System.Drawing.Point(164, 38);
            this.btnEditSoundbite.Name = "btnEditSoundbite";
            this.btnEditSoundbite.Size = new System.Drawing.Size(75, 66);
            this.btnEditSoundbite.TabIndex = 8;
            this.btnEditSoundbite.Text = "Edit\r\nAlarm/Timer\r\nSound";
            this.btnEditSoundbite.UseVisualStyleBackColor = true;
            // 
            // openChangedSoundFile
            // 
            this.openChangedSoundFile.FileName = "openFileDialog1";
            // 
            // frmEditWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 122);
            this.Controls.Add(this.btnEditSoundbite);
            this.Controls.Add(this.nudSeconds);
            this.Controls.Add(this.lblSeconds);
            this.Controls.Add(this.nudMinutes);
            this.Controls.Add(this.lblMinutes);
            this.Controls.Add(this.nudHrs);
            this.Controls.Add(this.lblHours);
            this.Controls.Add(this.txtItemName);
            this.Controls.Add(this.lblItemName);
            this.Enabled = false;
            this.Name = "frmEditWindow";
            this.Text = "Edit Alarm/Timer Info";
            ((System.ComponentModel.ISupportInitialize)(this.nudHrs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinutes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSeconds)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblItemName;
        private System.Windows.Forms.TextBox txtItemName;
        private System.Windows.Forms.Label lblHours;
        private System.Windows.Forms.NumericUpDown nudHrs;
        private System.Windows.Forms.Label lblMinutes;
        private System.Windows.Forms.NumericUpDown nudMinutes;
        private System.Windows.Forms.Label lblSeconds;
        private System.Windows.Forms.NumericUpDown nudSeconds;
        private System.Windows.Forms.Button btnEditSoundbite;
        private System.Windows.Forms.OpenFileDialog openChangedSoundFile;
    }
}