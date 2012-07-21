using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1 {
    public partial class frmEditWindow : Form {

        private frmDamoANTs primaryWindow = null;
        int ndx;

        public frmEditWindow(frmDamoANTs pf, Boolean alarm) {
            InitializeComponent();

            primaryWindow = pf;

            if (alarm) {
                ndx = pf.chklstAlarms.CheckedIndices[0];

                txtEditAlarmName.Text = pf.activeAls[ndx].name;
                nudEditHour.Value = pf.activeAls[ndx].target.Hour;
                nudEditMinute.Value = pf.activeAls[ndx].target.Minute;
                nudEditSecond.Value = pf.activeAls[ndx].target.Second;
                txtShowSoundByte.Text = pf.activeAls[ndx].soundBite;
            } else {
                ndx = pf.chklstTimers.CheckedIndices[0];

                txtEditAlarmName.Text = pf.activeTms[ndx].name;
                nudEditHour.Value = pf.activeTms[ndx].target.Hour;
                nudEditMinute.Value = pf.activeTms[ndx].target.Minute;
                nudEditSecond.Value = pf.activeTms[ndx].target.Second;
                txtShowSoundByte.Text = pf.activeTms[ndx].soundBite;
            }
        }

        private void btnEditSoundbyte_Click(object sender, EventArgs e) {
            DialogResult whatev = openNewSoundFile.ShowDialog();
            while (whatev != DialogResult.OK) {
                openNewSoundFile.ShowDialog();  //how does this get set to an
                                                //initial directory?
            }
            txtShowSoundByte.Text = openNewSoundFile.FileName;
        }

        private void btnEditCommit_Click(object sender, EventArgs e) {
            primaryWindow.editWindowMadeChanges(ndx, txtEditAlarmName.Text,
                (int)nudEditHour.Value, (int)nudEditMinute.Value,
                (int)nudEditSecond.Value, txtShowSoundByte.Text);
            
            this.Close();
        }
    }
}
