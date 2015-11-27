using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

/*
 * BUGS:
 * * Committing changes doesn't take into account whether or not you've edited
 *   an alarm or a timer
 */

namespace DamosAlarmsNTimers {
    public partial class frmEditWindow : Form {

        private frmDamoANTs primaryWindow = null;
        private const Boolean editDebugging = false;

        int ndx;
        Boolean alOrTm;

        public frmEditWindow(frmDamoANTs pf, Boolean alarm) {
            InitializeComponent();

            primaryWindow = pf;

            if (editDebugging) {
                Console.WriteLine("Debugging in Edit window");
            }

            if (alarm) {
                alOrTm = true;
                radioAlarmType.Checked = true;
                ndx = pf.chklstAlarms.SelectedIndex;

                if (ndx == -1) {
                    showDispleasureAtIncompetence();
                    return;
                }

                if (editDebugging) {
                    Console.WriteLine("ndx: " + ndx);

                    Console.WriteLine("Press a key to continue . . .");
                    Console.ReadKey();
                }

                txtEditAlarmName.Text = pf.activeAls[ndx].name;
                nudEditHour.Value = pf.activeAls[ndx].ringAt.Hour;
                nudEditMinute.Value = pf.activeAls[ndx].ringAt.Minute;
                nudEditSecond.Value = pf.activeAls[ndx].ringAt.Second;
                txtShowSoundByte.Text = pf.activeAls[ndx].soundBite;
            } else {
                alOrTm = false;
                radioTimerType.Checked = true;
                ndx = pf.chklstTimers.SelectedIndex;

                if (ndx == -1) {
                    showDispleasureAtIncompetence();
                    return;
                }

                txtEditAlarmName.Text = pf.activeTms[ndx].name;

                var ouah = pf.activeTms[ndx].getOrigInterval();
                
                nudEditHour.Value = ouah.Hours;
                nudEditMinute.Value = ouah.Minutes;
                nudEditSecond.Value = ouah.Seconds;
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
            primaryWindow.editWindowMadeChanges(alOrTm, ndx, 
                txtEditAlarmName.Text, (int)nudEditHour.Value, 
                (int)nudEditMinute.Value, (int)nudEditSecond.Value,
                txtShowSoundByte.Text);
            
            this.Close();
        }

        private void showDispleasureAtIncompetence() {
            MessageBox.Show("You must have selected a valid alarm/timer");
        }
    }
}
