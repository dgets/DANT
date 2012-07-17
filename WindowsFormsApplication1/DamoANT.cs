using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Media;

/*
 * BUGS:
 * * When loading the save file, the target date is calculated automatically
 *   which, if the application is left running beyond the magic amount of time,
 *   will cause the target date to be in the past, and thus bogus.  Time needs 
 *   to be saved just as the hours, minutes, seconds, and calculated only when 
 *   the alarm is activated in order to fix this problem.
 * * Timers section is not implemented yet.
 * * handling for replacing the 'RING RING' checklist text with the original
 *   alarm text after the sound is done playing or the user stops it needs to
 *   be taken care of
 * * need to save the selected state of a window between redrawings when an
 *   alarm is active
 */

namespace WindowsFormsApplication1
{
    public partial class frmDamoANTs : Form
    {
        public List<AlarmsTimers> activeAls = new List<AlarmsTimers>();
        public List<AlarmsTimers> activeTms = new List<AlarmsTimers>();
        private const Boolean debugging = true;
        private String cfgFile;
        public frmEditWindow editWindow = null;
        
        public frmDamoANTs() { 
            String ouah;

            ouah = System.Environment.GetFolderPath(
                Environment.SpecialFolder.Personal) + "\\DANT.cfg";
            cfgFile = ouah;

            InitializeComponent();

            if (!loadAlarmsTimers()) {
                MessageBox.Show("Issues loading saved alarms & timers!");
            }
        }

        /*  --==** ALARM/TIMER CLASS **==-- */
        public partial class AlarmsTimers {
            public String name;
            public DateTime target;
            private TimeSpan interval;
            public Boolean running;
            public String soundBite;

            //method correctly sets interval for an alarm
            public void autoSetInterval() {
                interval = target - DateTime.Now;
            }

            //method determines whether alarm/timer is 'firing' or not
            public Boolean checkIfFiring() {
                if (debugging) {
                    Console.WriteLine("Checking if firing\nRunning value for: " +
                        name + " is: " + running.ToString() + "\nSeconds " +
                        "left: " + interval.TotalSeconds.ToString() + "\n" +
                        "Target time: " + target.ToShortTimeString());
                }

                if ((interval.TotalSeconds < 1) &&
                    (interval.TotalSeconds > -1)) {
                    running = false;
                    return true;
                } else {
                    return false;
                }
            }

            public String returnCountdown() {
                String tmpHr, tmpMin, tmpSec;

                if (interval.Hours < 10) {
                    tmpHr = "0" + interval.Hours.ToString();
                } else {
                    tmpHr = interval.Hours.ToString();
                }
                if (interval.Minutes < 10) {
                    tmpMin = "0" + interval.Minutes.ToString();
                } else {
                    tmpMin = interval.Minutes.ToString();
                }
                if (interval.Seconds < 10) {
                    tmpSec = "0" + interval.Seconds.ToString();
                } else {
                    tmpSec = interval.Seconds.ToString();
                }

                return (tmpHr + ":" + tmpMin + ":" + tmpSec);
            }
        }

        private Boolean verifyLegitTime(Boolean alarmOrTimer) {
            //note this method will be passed a T to check alarm data, F to
            //check timer data (evil, but I'm not sure how to do it better)
            if (alarmOrTimer) {
                //alarm
                if ((numAlarmHr.Value == 0) && (numAlarmMin.Value == 0) &&
                    (numAlarmSec.Value == 0)) {
                    //bogus alarm entry
                    MessageBox.Show("You must enter a valid time for the " +
                        "alarm to go off!");
                    return false;
                }
            } else {
                //timer
                if ((numTimerHr.Value == 0) && (numTimerMin.Value == 0) &&
                    (numTimerSec.Value == 0)) {
                    //bogus timer entry
                    MessageBox.Show("You must enter a valid duration for " +
                        "the timer to go off!");
                    return false;
                }
            }

            return true;
        }

        private DateTime checkAlarmDay() {
            //check to see if alarm is for tomorrow
            //goddamn this needs to be refactored to just utilize the
            //hr/min/sec data and ditch the date
            if ((DateTime.Now >=
                 new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                              DateTime.Now.Day, (int)numAlarmHr.Value,
                              (int)numAlarmMin.Value,
                              (int)numAlarmSec.Value))) {
                //make it tomorrow
                return (new DateTime(DateTime.Now.AddDays(1).Year,
                        DateTime.Now.AddDays(1).Month,
                        DateTime.Now.AddDays(1).Day, (int)numAlarmHr.Value,
                        (int)numAlarmMin.Value, (int)numAlarmSec.Value));
            } else {
                return (new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                                 DateTime.Now.Day, (int)numAlarmHr.Value,
                                 (int)numAlarmMin.Value,
                                 (int)numAlarmSec.Value));
            }
        }

        private void btnAddAlarm_Click(object sender, EventArgs e) {
            AlarmsTimers tmpAlarm = new AlarmsTimers();

            DialogResult whatev = openSoundFile.ShowDialog();
            while (whatev != DialogResult.OK) {
                whatev = openSoundFile.ShowDialog();
            }

            tmpAlarm.soundBite = openSoundFile.FileName;

            //verify that numericUpDown selectors are not at 0,0,0
            if (!verifyLegitTime(true)) {
                return;
            }

            //verify that name box is not still full of default value
            if (txtAlarmName.Text.CompareTo("Alarm Name Here") == 0) {
                MessageBox.Show("You must enter a valid name for the " +
                    "alarm that you are setting!");
                return;
            }

            //check to see if alarm is for tomorrow, set other options
            tmpAlarm.target = checkAlarmDay();
            tmpAlarm.name = txtAlarmName.Text;

            //reset textbox & numericUpDowns
            txtAlarmName.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            txtAlarmName.Text = "Alarm Name Here";
            numAlarmHr.Value = 0; numAlarmMin.Value = 0; numAlarmSec.Value = 0;

            //add it to the list
            activeAls.Add(tmpAlarm);

            //add alarm to the 'active' alarms list in the checkboxlist
            addAlarm(activeAls.IndexOf(tmpAlarm));
            saveAlarmsTimers();
        }
        
        private Boolean saveAlarmsTimers() {
            if (activeAls.Count() == 0) { return false; }
            
            System.IO.StreamWriter cFile = 
                new System.IO.StreamWriter(cfgFile);

            if (debugging) {
                Console.WriteLine("Opened " + cfgFile + " for writing");
            }

            for (int cntr = 0; cntr < activeAls.Count(); cntr++) {
                if (debugging) {
                    Console.WriteLine("Adding activeAls[" + cntr.ToString() +
                        "] to save file . . .");
                } try {
                    cFile.WriteLine("A," + activeAls[cntr].name + "," +
                        activeAls[cntr].target.Hour + "," + 
                        activeAls[cntr].target.Minute + "," +
                        activeAls[cntr].target.Second + "," +
                        activeAls[cntr].soundBite);
                } catch {
                    Console.WriteLine("Error adding activeAls[" +
                        cntr.ToString() + "]!");
                }
            }
            cFile.Close();
            return true;
        }

        private String[] parseSavedFieldsLine(String rawLine) {
            //per line basis
            String[] rawFields;

            rawFields = rawLine.Split(',');
            if (rawFields.Count() != 6) {
                MessageBox.Show("Error parsing " + cfgFile +
                    "; please check the config file and try again!");
                rawFields[0] = null;    //poor way to indicate error condition
            }

            return rawFields;
        }

        private int[] convertSavedFields(String[] rawFields) {
            int[] tmpTimes = {0, 0, 0};
            Boolean tmpFlag = false;    //try to find out a better way to do

            if (!Int32.TryParse(rawFields[2], out tmpTimes[0])) {
                tmpFlag = true;
            }
            if (!Int32.TryParse(rawFields[3], out tmpTimes[1])) {
                tmpFlag = true;
            }
            if (!Int32.TryParse(rawFields[4], out tmpTimes[2])) {
                tmpFlag = true;
            }
            if (tmpFlag) {
                MessageBox.Show("There was an error trying to parse " +
                    "one of the fields in " + cfgFile + "\nPlease " +
                    "try to find out what the hell is going on and " +
                    "try again later.");
                tmpTimes[0] = 0; tmpTimes[1] = 0; tmpTimes[2] = 0;
            }
            return tmpTimes;
        }

        private DateTime checkDate(int[] timeData) {
            Boolean tmpFlag = false;
            DateTime tDate = new DateTime();

            if (timeData[0] < DateTime.Now.Hour) {
                tmpFlag = true;
            } else if ((timeData[0] == DateTime.Now.Hour) &&
              (timeData[1] < DateTime.Now.Minute)) {
                tmpFlag = true;
            } else if ((timeData[0] == DateTime.Now.Hour) &&
                       (timeData[1] == DateTime.Now.Minute) &&
                       (timeData[2] < DateTime.Now.Second)) {
                tmpFlag = true;
            }
            if (tmpFlag) {
                tDate =
                    new DateTime(DateTime.Now.AddDays(1).Year,
                        DateTime.Now.AddDays(1).Month,
                        DateTime.Now.AddDays(1).Day, timeData[0],
                        timeData[1], timeData[2]);
            } else {
                tDate =
                    new DateTime(DateTime.Now.Year,
                        DateTime.Now.Month,
                        DateTime.Now.Day, timeData[0], timeData[1], 
                        timeData[2]);
            }
            //of course the preceding code will have to be changed as
            //the application will occasionally probably run more than
            //one day rendering the dates calculated above bogus
            //(see hint in BUGS at the top of the code)
            return tDate;
        }

        private Boolean loadAlarmsTimers() {
            if (!File.Exists(cfgFile)) {
                if (debugging) {
                    Console.WriteLine("Empty or nonexistant config file " +
                        "detected");
                }
                return true;    //not an error condition
            }

            String[] rawFile;
            int cntr = 0;

            try {
                rawFile = System.IO.File.ReadAllLines(cfgFile); //no need4close
            } catch {
                MessageBox.Show("There was an error reading " + cfgFile +
                    ", aborting.");
                return false;
            }

            foreach (String raw in rawFile) {
                String[] rawFields;
                int[] tmpTimeData;
                AlarmsTimers tmpAlarm = new AlarmsTimers();

                rawFields = parseSavedFieldsLine(raw);
                if (rawFields[0] == null) {
                    //DOH!
                    break;
                } else if (rawFields[0] == "A") {   //alarm
                    tmpTimeData = convertSavedFields(rawFields);

                    if ((tmpTimeData[0] == 0) && (tmpTimeData[1] == 0) &&
                        (tmpTimeData[2] == 0)) {
                        break;
                    }

                    //add to active alarms
                    tmpAlarm.name = rawFields[1];
                    tmpAlarm.running = false;
                    tmpAlarm.target = checkDate(tmpTimeData);
                    tmpAlarm.soundBite = rawFields[5];

                    activeAls.Add(tmpAlarm);
                    addAlarm(cntr++);
                } else {
                    //we're working with a timer here (add code later)
                    MessageBox.Show("Timer unimplemented as of now");
                    return false;
                }
            }
            return true;
        }

        private void addAlarm(int alarmNo) {
            String tmpHr, tmpMin, tmpSec;
            //there has really got to be simpler formatting methods to handle this shit
            if (activeAls.ElementAt(alarmNo).target.Hour < 10) {
                tmpHr = "0" + activeAls.ElementAt(alarmNo).target.Hour.ToString();
            } else {
                tmpHr = activeAls.ElementAt(alarmNo).target.Hour.ToString();
            }

            if (activeAls.ElementAt(alarmNo).target.Minute < 10) {
                tmpMin = "0" + activeAls.ElementAt(alarmNo).target.Minute.ToString();
            } else {
                tmpMin = activeAls.ElementAt(alarmNo).target.Minute.ToString();
            }

            if (activeAls.ElementAt(alarmNo).target.Second < 10) {
                tmpSec = "0" + activeAls.ElementAt(alarmNo).target.Second.ToString();
            } else {
                tmpSec = activeAls.ElementAt(alarmNo).target.Second.ToString();
            }

            //note alarm will not be running at this point; just add it to the chkboxlist
            chklstAlarms.Items.Insert(alarmNo, (activeAls.ElementAt(alarmNo).name + " -> " +
                tmpHr + ":" + tmpMin + ":" + tmpSec));
        }

        private void chklstAlarms_CheckedChanged(object sender, ItemCheckEventArgs e) {
            if (debugging) {
                Console.WriteLine("activeAls.Count: " + activeAls.Count.ToString() +
                    "e.Index: " + e.Index.ToString() + "e.NewValue.ToString(): " +
                    e.NewValue.ToString());
            }
        }

        private void tmrOneSec_Tick(object sender, EventArgs e) {
            //alarms
            for (int cntr = 0; cntr < activeAls.Count; cntr++) {
                if (!chklstAlarms.GetItemChecked(cntr)) {
                    if (debugging) {
                        Console.WriteLine("Non-Active Alarm #" +
                            cntr.ToString() + " being unset");
                    }
                    activeAls.ElementAt(cntr).running = false;
                    chklstAlarms.Items.RemoveAt(cntr);
                    chklstAlarms.Items.Insert(cntr,
                        (activeAls.ElementAt(cntr).name + " -> " +
                        activeAls.ElementAt(cntr).target.Hour + ":" +
                        activeAls.ElementAt(cntr).target.Minute +
                        ":" + activeAls.ElementAt(cntr).target.Second));
                }

                if (chklstAlarms.GetItemChecked(cntr)) {
                    activeAls.ElementAt(cntr).running = true;
                    activeAls.ElementAt(cntr).autoSetInterval();

                    //update the display
                    chklstAlarms.Items.RemoveAt(cntr);
                    chklstAlarms.Items.Insert(cntr, 
                        activeAls.ElementAt(cntr).name + ": Remaining: " +
                        activeAls.ElementAt(cntr).returnCountdown());
                    chklstAlarms.SetItemChecked(cntr, true);    //necessary?

                    if (activeAls.ElementAt(cntr).checkIfFiring()) {
                        if (debugging) {
                            Console.WriteLine("Found activeAls[" +
                                cntr.ToString() + "] to be firing");
                        }
                        chklstAlarms.SetItemChecked(cntr, false);
                        chklstAlarms.Items.RemoveAt(cntr);
                        chklstAlarms.Items.Insert(cntr, 
                            activeAls.ElementAt(cntr).name + " -+=* RING " +
                            " RING *=+-");
                        if (chklstAlarms.CheckedIndices.Count == 0) {
                            tmrOneSec.Enabled = false;
                            tmrOneSec.Stop();
                        }
                        if (activeAls.ElementAt(cntr).soundBite == null) {
                            SystemSounds.Beep.Play();
                        } else {
                            WMPLib.WindowsMediaPlayer wplayer =
                                new WMPLib.WindowsMediaPlayer();
                            wplayer.URL = activeAls.ElementAt(cntr).soundBite;
                            wplayer.controls.play();
                        }
                    }
                }
            }
        }

        private void chklstAlarms_SelectedIndexChanged(object sender, EventArgs e) {
            //this works beautifully
            this.BeginInvoke(new MethodInvoker(checkActiveAlarms), null);
        }

        private void checkActiveAlarms() {
            if (debugging) {
                Console.WriteLine("Firing chklstAlarms_Clicked");
            }

            foreach (int temp in chklstAlarms.CheckedIndices) {
                //these are specifically checked, so double checking that in the
                //conditional should not be necessary
                if (activeAls.ElementAt(temp).running != true) {
                    if (debugging) {
                        Console.WriteLine("Flagged #" + temp.ToString());
                    }

                    //enable timer if it hasn't been handled already
                    if (tmrOneSec.Enabled == false) {
                        activeAls.ElementAt(temp).running = true;
                        tmrOneSec.Enabled = true;
                        tmrOneSec.Start();
                    }
                }
            }
        }

        private void txtAlarmName_Enter(object sender, EventArgs e) {
            if (txtAlarmName.Text.CompareTo("Alarm Name Here") == 0) {
                txtAlarmName.Text = "";
                txtAlarmName.ForeColor = System.Drawing.SystemColors.WindowText;
            }
        }

        private void txtTimerName_Enter(object sender, EventArgs e) {
            if (txtTimerName.Text.CompareTo("Timer Name Here") == 0) {
                txtTimerName.Text = "";
                txtAlarmName.ForeColor = System.Drawing.SystemColors.WindowText;
            }
        }

        private void txtAlarmName_Leave(object sender, EventArgs e) {
            if (txtAlarmName.Text.CompareTo("") == 0) {
                txtAlarmName.ForeColor = System.Drawing.SystemColors.InactiveCaption;
                txtAlarmName.Text = "Alarm Name Here";
            }
        }

        private void txtTimerName_Leave(object sender, EventArgs e) {
            if (txtTimerName.Text.CompareTo("") == 0) {
                txtTimerName.ForeColor = System.Drawing.SystemColors.InactiveCaption;
                txtTimerName.Text = "Timer Name Here";
            }
        }

        private void btnToastAlarm_Click(object sender, EventArgs e) {
            foreach (int ndx in chklstAlarms.CheckedIndices) {
                chklstAlarms.Items.RemoveAt(ndx);
                activeAls.RemoveAt(ndx);
                saveAlarmsTimers();
            }
        }

        private void btnEditAlarm_Click(object sender, EventArgs e) {
            if (chklstAlarms.CheckedIndices.Count == 0) {
                MessageBox.Show("You must check an alarm before trying to " +
                    "edit it!");
            } else {
                editWindow = new frmEditWindow(this);
                editWindow.Show();
                //don't forget to wipe and re-load alarms and timers here
                //afterwards with this very ghey solution
            }
        }

        public void editWindowMadeChanges(int ndx, String an, int hr, int min, int sec, String fn) {
            //create a method for resetting the chklst string if this is not
            //accomplished by unchecking the list item and/or calling the
            //method that is fired when it is unchecked
            activeAls[ndx].name = an;
            activeAls[ndx].target = new DateTime(DateTime.Now.Year,
                DateTime.Now.Month, DateTime.Now.Day, hr, min, sec);
            activeAls[ndx].soundBite = fn;
            activeAls[ndx].autoSetInterval();

            chklstAlarms.SetItemChecked(ndx, false);
            if (!saveAlarmsTimers()) {
                MessageBox.Show("Had an issue trying to save configuration");
            }
        }
    }
}
