namespace DamosAlarmsNTimers {
    partial class frmHelp {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHelp));
            this.btnCloseHelp = new System.Windows.Forms.Button();
            this.lblHelpFormHeading = new System.Windows.Forms.Label();
            this.rtbHelpSynopsis = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnCloseHelp
            // 
            this.btnCloseHelp.Location = new System.Drawing.Point(172, 352);
            this.btnCloseHelp.Name = "btnCloseHelp";
            this.btnCloseHelp.Size = new System.Drawing.Size(75, 23);
            this.btnCloseHelp.TabIndex = 0;
            this.btnCloseHelp.Text = "Close Help";
            this.btnCloseHelp.UseVisualStyleBackColor = true;
            this.btnCloseHelp.Click += new System.EventHandler(this.btnCloseHelp_Click);
            // 
            // lblHelpFormHeading
            // 
            this.lblHelpFormHeading.AutoSize = true;
            this.lblHelpFormHeading.Location = new System.Drawing.Point(100, 9);
            this.lblHelpFormHeading.Name = "lblHelpFormHeading";
            this.lblHelpFormHeading.Size = new System.Drawing.Size(208, 13);
            this.lblHelpFormHeading.TabIndex = 2;
            this.lblHelpFormHeading.Text = "Help Synopsis for Damo\'s Alarms \'N Timers";
            // 
            // rtbHelpSynopsis
            // 
            this.rtbHelpSynopsis.Location = new System.Drawing.Point(12, 25);
            this.rtbHelpSynopsis.Name = "rtbHelpSynopsis";
            this.rtbHelpSynopsis.Size = new System.Drawing.Size(393, 321);
            this.rtbHelpSynopsis.TabIndex = 3;
            this.rtbHelpSynopsis.Text = resources.GetString("rtbHelpSynopsis.Text");
            // 
            // frmHelp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(417, 387);
            this.Controls.Add(this.rtbHelpSynopsis);
            this.Controls.Add(this.lblHelpFormHeading);
            this.Controls.Add(this.btnCloseHelp);
            this.Name = "frmHelp";
            this.Text = "DANT Help";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCloseHelp;
        private System.Windows.Forms.Label lblHelpFormHeading;
        private System.Windows.Forms.RichTextBox rtbHelpSynopsis;
    }
}