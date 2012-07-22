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
 * * Timers section is not fully implemented yet.
 * * need to save the selected state of a window between redrawings when an
 *   alarm is active - then 'selection' as opposed to 'checked' status needs
 *   to be used for non-activating events as this is annoying and not very
 *   intuitive for the end user
 * * Wipe Alarm button doesn't stop timer from ticking -- modularize the code
 *   to check for timer/alarm tickingness because it needs to be used in
 *   several different places; nor does finishing the editing process for an
 *   alarm or timer
 * * When toggling one alarm from active to inactive and back, the alarm does
 *   not properly start the timer ticking and countdown again; this is not a
 *   problem when activating one then deactivating it, toggling another one, 
 *   and then coming back to the first one, though
 * * Refactor checkDate() as per George Dorn's instructions for more efficient
 *   and legible code (ie compare entire dates, not multiple DateTime.Nows)
 * * checkActiveTimers() & checkActiveAlarms() both need to be more modular
 * * alternating clicking between active timers and active alarms causes 
 *   bugs rendering the alarms, timers, or both, inactive even when properly
 *   checked
 * * timer now sets interval correctly; however, the code for this is kludgy
 *   as hell due to using the same object <AlarmsTimers> for both sorts of
 *   items -- need to separate the class into one class for each
 * * Alarm being set to run doesn't always fix the date object with 
 *   checkAlarmDay() <-- not working properly
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
            public Boolean alarm;
            public DateTime tmpTarget;

            //method correctly sets interval for an alarm
            public void autoSetInterval() {
                if (alarm) {
                    interval = target - DateTime.Now;
                } else {
                    interval = tmpTarget - DateTime.Now;
                }
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

        private DateTime checkAlarmDay(int hr, int min, int sec) {
            //check to see if alarm is for tomorrow
            //goddamn this needs to be refactored to just utilize the
            //hr/min/sec data and ditch the date
            if ((DateTime.Now >=
                 new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                              DateTime.Now.Day, hr, min, sec))) {
                //make it tomorrow
                return (new DateTime(DateTime.Now.AddDays(1).Year,
                        DateTime.Now.AddDays(1).Month,
                        DateTime.Now.AddDays(1).Day, hr, min, sec));
            } else /* if ((DateTime.Now >= 
                        new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                                     DateTime.Now.Day, 0, 0, 0))) */ {
                return (new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                                 DateTime.Now.Day, hr, min, sec));
            }
        }

        private void btnAddAlarm_Click(object sender, EventArgs e) {
            //verify that name box is not still full of default value
            if (txtAlarmName.Text.CompareTo("Alarm Name Here") == 0) {
                MessageBox.Show("You must enter a valid name for the " +
                    "alarm that you are setting!");
                return;
            }
            //verify that numericUpDown selectors are not at 0,0,0
            if (!legitTime((int)numAlarmHr.Value, (int)numAlarmMin.Value,
                (int)numAlarmSec.Value, true)) {
                return;
            }
            
            AlarmsTimers tmpAlarm = new AlarmsTimers();

            tmpAlarm.soundBite = soundByteSelection();

            //check to see if alarm is for tomorrow, set other options
            tmpAlarm.target = checkAlarmDay((int)numAlarmHr.Value,
                                            (int)numAlarmMin.Value,
                                            (int)numAlarmSec.Value);
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
            int cntr;

            if (activeAls.Count() == 0) { return false; }
            
            System.IO.StreamWriter cFile = 
                new System.IO.StreamWriter(cfgFile);

            if (debugging) {
                Console.WriteLine("Opened " + cfgFile + " for writing");
            }

            for (cntr = 0; cntr < activeAls.Count(); cntr++) {
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
                    MessageBox.Show("Error adding activeAls[" +
                        cntr.ToString() + "]!");
                }
            }
            for (cntr = 0; cntr < activeTms.Count(); cntr++) {
                if (debugging) {
                    Console.WriteLine("Adding activeTms[" + cntr.ToString() +
                        "] to save file . . .");
                } try {
                    cFile.WriteLine("T," + activeTms[cntr].name + "," +
                        activeTms[cntr].target.Hour + "," +
                        activeTms[cntr].target.Minute + "," +
                        activeTms[cntr].target.Second + "," +
                        activeTms[cntr].soundBite);
                } catch {
                    MessageBox.Show("Error adding activeTms[" +
                        cntr.ToString() + "]!");
                }
            }
            cFile.Close();
            return true;
        }

        private String[] parseSavedFieldsLine(String rawLine) {
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
            //refactor this shit as per George Dorn's suggestions
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
            int aCntr = 0, tCntr = 0;

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
                AlarmsTimers tmpEntry = new AlarmsTimers();

                rawFields = parseSavedFieldsLine(raw);
                if (rawFields[0] == null) {
                    //DOH!
                    break;
                } else if (rawFields[0].CompareTo("A") == 0) {   //alarm
                    tmpTimeData = convertSavedFields(rawFields);

                    //add to active alarms
                    tmpEntry.name = rawFields[1];
                    tmpEntry.running = false;
                    tmpEntry.target = checkDate(tmpTimeData);
                    tmpEntry.soundBite = rawFields[5];

                    activeAls.Add(tmpEntry);
                    addAlarm(aCntr++);
                } else if (rawFields[0].CompareTo("T") == 0) {
                    //we're working with a timer here
                    //MessageBox.Show("Timer unimplemented as of now");

                    tmpTimeData = convertSavedFields(rawFields);

                    tmpEntry.name = rawFields[1];
                    tmpEntry.running = false;
                    tmpEntry.target = checkDate(tmpTimeData);
                    tmpEntry.soundBite = rawFields[5];

                    activeTms.Add(tmpEntry);
                    addTimer(tCntr++);
                    //return false;
                } else {
                    //serious garbled file issues
                    MessageBox.Show("Issue parsing config file!",
                        "Cannot Parse DANT.cfg", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            return true;
        }

        private void addAlarm(int alarmNo) {
            chklstAlarms.Items.Insert(alarmNo,
                (activeAls.ElementAt(alarmNo).name + " -> " +
                 addZeroesToTime(activeAls.ElementAt(alarmNo).target)));
        }

        private void addTimer(int timerNo) {
            chklstTimers.Items.Insert(timerNo,
                (activeTms.ElementAt(timerNo).name + " -> " +
                 addZeroesToTime(activeTms.ElementAt(timerNo).target)));
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
                         addZeroesToTime(activeAls.ElementAt(cntr).target)));
                }

                if (chklstAlarms.GetItemChecked(cntr)) {
                    activeAls.ElementAt(cntr).running = true;
                    activeAls.ElementAt(cntr).target = 
                        checkAlarmDay(
                            (int)activeAls.ElementAt(cntr).target.Hour,
                            (int)activeAls.ElementAt(cntr).target.Minute,
                            (int)activeAls.ElementAt(cntr).target.Second);
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

                            MessageBox.Show(activeAls.ElementAt(cntr).name +
                                ": -+=* Ring ring, Neo *=+-",
                                activeAls.ElementAt(cntr).name + " Firing",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation);
                            chklstAlarms.Items.RemoveAt(cntr);
                            addAlarm(cntr);
                            wplayer.controls.stop();
                        }
                    }
                }
            }

            //timers -- obviously this needs to be modularized
            for (int cntr = 0; cntr < activeTms.Count; cntr++) {
                if (!chklstTimers.GetItemChecked(cntr)) {
                    if (debugging) {
                        Console.WriteLine("Non-Active Timer #" +
                            cntr.ToString() + " being unset");
                    }
                    activeTms.ElementAt(cntr).running = false;
                    chklstTimers.Items.RemoveAt(cntr);
                    chklstTimers.Items.Insert(cntr,
                        (activeTms.ElementAt(cntr).name + " -> " +
                         addZeroesToTime(activeTms.ElementAt(cntr).target)));
                }

                if (chklstTimers.GetItemChecked(cntr)) {
                    activeTms.ElementAt(cntr).running = true;
                    activeTms.ElementAt(cntr).target =
                        checkAlarmDay(
                            (int)activeTms.ElementAt(cntr).target.Hour,
                            (int)activeTms.ElementAt(cntr).target.Minute,
                            (int)activeTms.ElementAt(cntr).target.Second);
                    activeTms.ElementAt(cntr).autoSetInterval();

                    //update the display
                    chklstTimers.Items.RemoveAt(cntr);
                    chklstTimers.Items.Insert(cntr,
                        activeTms.ElementAt(cntr).name + ": Remaining: " +
                        activeTms.ElementAt(cntr).returnCountdown());
                    chklstTimers.SetItemChecked(cntr, true);    //necessary?

                    if (activeTms.ElementAt(cntr).checkIfFiring()) {
                        if (debugging) {
                            Console.WriteLine("Found activeAls[" +
                                cntr.ToString() + "] to be firing");
                        }
                        chklstTimers.SetItemChecked(cntr, false);
                        chklstTimers.Items.RemoveAt(cntr);
                        chklstTimers.Items.Insert(cntr,
                            activeTms.ElementAt(cntr).name + " -+=* RING " +
                            " RING *=+-");
                        if (chklstTimers.CheckedIndices.Count == 0) {
                            tmrOneSec.Enabled = false;
                            tmrOneSec.Stop();
                        }
                        if (activeTms.ElementAt(cntr).soundBite == null) {
                            SystemSounds.Beep.Play();
                        } else {
                            WMPLib.WindowsMediaPlayer wplayer =
                                new WMPLib.WindowsMediaPlayer();
                            wplayer.URL = activeTms.ElementAt(cntr).soundBite;
                            wplayer.controls.play();

                            MessageBox.Show(activeTms.ElementAt(cntr).name +
                                ": -+=* Ring ring, Neo *=+-",
                                activeTms.ElementAt(cntr).name + " Firing",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation);
                            chklstTimers.Items.RemoveAt(cntr);
                            addTimer(cntr);
                            wplayer.controls.stop();
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

            //refactor to check between alarms & timers, not just one or other
            if ((chklstAlarms.CheckedIndices.Count == 0) &&
                (tmrOneSec.Enabled == true)) {
                //turn the timer off, por dios
                tmrOneSec.Stop();
                tmrOneSec.Enabled = false;
                return;
            }

            foreach (int temp in chklstAlarms.CheckedIndices) {
                //these are specifically checked, so double checking that in the
                //conditional should not be necessary
                if (activeAls.ElementAt(temp).running != true) {
                    if (debugging) {
                        Console.WriteLine("Flagged alarm #" + temp.ToString());
                    }

                    //enable timer if it hasn't been handled already
                    if (tmrOneSec.Enabled == false) {
                        activeAls.ElementAt(temp).running = true;
                        tmrOneSec.Enabled = true;
                        tmrOneSec.Start();
                    }

                    //handle checking the date due to our shitty handling
                    activeAls.ElementAt(temp).target =
                        checkAlarmDay(activeAls.ElementAt(temp).target.Hour,
                            activeAls.ElementAt(temp).target.Minute,
                            activeAls.ElementAt(temp).target.Second);
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
                txtTimerName.ForeColor = System.Drawing.SystemColors.WindowText;
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
                //need to add code in here to stop timer from ticking if
                //necessary (if this was the only active alarm/timer)
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
                editWindow = new frmEditWindow(this, true);
                editWindow.Show();
                //don't forget to wipe and re-load alarms and timers here
                //afterwards
            }
        }

        public void editWindowMadeChanges(Boolean alarm, int ndx, String an,
            int hr, int min, int sec, String fn) {
            if (alarm) {
                activeAls[ndx].name = an;
                activeAls[ndx].target = new DateTime(DateTime.Now.Year,
                    DateTime.Now.Month, DateTime.Now.Day, hr, min, sec);
                activeAls[ndx].soundBite = fn;
                activeAls[ndx].autoSetInterval();

                chklstAlarms.SetItemChecked(ndx, false);
            } else {
                activeTms[ndx].name = an;
                activeTms[ndx].target = new DateTime(DateTime.Now.Year,
                    DateTime.Now.Month, DateTime.Now.Day, hr, min, sec);
                activeTms[ndx].soundBite = fn;
                activeTms[ndx].autoSetInterval();

                chklstTimers.SetItemChecked(ndx, false);
            }

            if (!saveAlarmsTimers()) {
                MessageBox.Show("Had an issue trying to save configuration");
            }
        }

        private String addZeroesToTime(DateTime ouah) {
            int hr, min, sec;
            String targetZeroesAdded;

            hr = ouah.Hour; min = ouah.Minute; sec = ouah.Second;

            if (hr < 10) {
                targetZeroesAdded = "0" + hr.ToString();
            } else {
                targetZeroesAdded = hr.ToString();
            }
            targetZeroesAdded += ":";
            if (min < 10) {
                targetZeroesAdded += "0" + min.ToString();
            } else {
                targetZeroesAdded += min.ToString();
            }
            targetZeroesAdded += ":";
            if (sec < 10) {
                targetZeroesAdded += "0" + sec.ToString();
            } else {
                targetZeroesAdded += sec.ToString();
            }

            return targetZeroesAdded;
        }

        private void btnAddTimer_Click(object sender, EventArgs e) {
            if (txtTimerName.Text.CompareTo("") == 0) {
                MessageBox.Show("You must enter a timer name!",
                    "Timer Name Required");
            }
            if (!legitTime((int)numTimerHr.Value, (int)numTimerMin.Value,
                                (int)numTimerSec.Value, false)) {
                return;
            }

            AlarmsTimers tmpTimer = new AlarmsTimers();

            tmpTimer.name = txtTimerName.Text;
            tmpTimer.target = new DateTime(DateTime.Now.Year,
                DateTime.Now.Month, DateTime.Now.Day, (int)numTimerHr.Value,
                (int)numTimerMin.Value, (int)numTimerSec.Value);
            tmpTimer.running = false;
            tmpTimer.soundBite = soundByteSelection();

            //reset textbox & numericUpDowns
            txtTimerName.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            txtTimerName.Text = "Timer Name Here";
            numTimerHr.Value = 0; numTimerMin.Value = 0; numTimerSec.Value = 0;

            //add it to the list
            activeTms.Add(tmpTimer);

            //add timer to the 'active' timers list in the checkboxlist
            addTimer(activeTms.IndexOf(tmpTimer));
            saveAlarmsTimers();
        }

        private String soundByteSelection() {
            String ouah;

            DialogResult whatev = openSoundFile.ShowDialog();
            while (whatev != DialogResult.OK) {
                whatev = openSoundFile.ShowDialog();
            }

            ouah = openSoundFile.FileName;

            return ouah;
        }

        private Boolean legitTime(int hr, int min, int sec, Boolean alarm) {
            if ((hr == 0) && (min == 0) && (sec == 0) && (alarm)) {
                DialogResult rslt = MessageBox.Show("Use a time of " +
                    "midnight?", "Confirm Midnight",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                switch (rslt) {
                    case DialogResult.Yes:
                        return true;
                        //break;
                    case DialogResult.No:
                        numAlarmHr.Focus();
                        return false;
                        //break;
                }
            } else if ((hr == 0) && (min == 0) && (sec == 0) && (!alarm)) {
                MessageBox.Show("Invalid timer duration!", "Zero Timer",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void chklstTimers_SelectedIndexChanged(object sender, EventArgs e) {
            this.BeginInvoke(new MethodInvoker(checkActiveTimers), null);
        }

        private void checkActiveTimers() {
            if (debugging) {
                Console.WriteLine("Firing chklstTimers_Clicked");
            }

            //refactor and move to its own func to check for alarms _&_ timers
            if ((chklstTimers.CheckedIndices.Count == 0) &&
                (chklstAlarms.CheckedIndices.Count == 0) &&
                (tmrOneSec.Enabled == true)) {
                //turn the timer off, por dios
                tmrOneSec.Stop();
                tmrOneSec.Enabled = false;
                return;
            }

            foreach (int temp in chklstTimers.CheckedIndices) {
                //these are specifically checked, so double checking that in the
                //conditional should not be necessary
                if (activeTms.ElementAt(temp).running != true) {
                    if (debugging) {
                        Console.WriteLine("Flagged timer #" + temp.ToString());
                    }

                    //enable timer if it hasn't been handled already
                    if (tmrOneSec.Enabled == false) {
                        if (activeTms.ElementAt(temp).tmpTarget ==
                            DateTime.MinValue) {
                            /* this is a CRAPPY way to handle this; I really
                             * need to separate AlarmsTimers into 2 classes,
                             * one for each sort of list to avoid this kludge*/
                            activeTms.ElementAt(temp).tmpTarget =
                                DateTime.Now.AddSeconds(
                                    (activeTms.ElementAt(temp).target.Hour * 3600) +
                                    (activeTms.ElementAt(temp).target.Minute * 60) +
                                    (activeTms.ElementAt(temp).target.Second));
                        }
                        activeTms.ElementAt(temp).running = true;
                        tmrOneSec.Enabled = true;
                        tmrOneSec.Start();
                    }
                }
            }
        }

        private void btnEditTimer_Click(object sender, EventArgs e) {
            if (chklstTimers.CheckedIndices.Count == 0) {
                MessageBox.Show("You must check a timer before trying to " +
                    "edit it!");
            } else {
                editWindow = new frmEditWindow(this, false);
                editWindow.Show();
                //don't forget to wipe and re-load alarms and timers here
                //afterwards
            }
        }

    }
}
