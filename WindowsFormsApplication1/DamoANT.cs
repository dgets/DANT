﻿using System;
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
 * * Now have been all relocated to GitHub/dgets repository issues
 */

/*
 * CHANGES:
 * * Changes block added 7/27/2012
 * * Modularized code for checking if a timer, an alarm, or both are running
 * * Modularized code for unsetting an item in the timers or alarms list from
 *   _Tick
 * * Finished modularizing _Tick routine 8/6/12
 * * Starting on writing proper method documentation 8/9/12
 * * Modularized the alarm/timer name graying and numeric up/down resetting
 *   code
 * * Modularized multiple methods throughout the rest of the code (left one
 *   or two that need more breaking up for later), finished documentation for
 *   the rest of the existing code 8/16/12
 * * Timers and Alarms now work concurrently; fixed active alarm/timer code to
 *   only execute certain bits when needed to reduce overhead 8/16/12
 * * Finished initial creation of Timers code; just need to debug it now 8/20
 * 
 * NOTE: Depreciating this in the comments now that revision information will
 * be stored by default in GitHub repository
 */

namespace DamosAlarmsNTimers
{
    public partial class frmDamoANTs : Form
    {
        public List<Alarms> activeAls = new List<Alarms>();
        public List<Timers> activeTms = new List<Timers>();
        private const Boolean debugging = true;
        private String cfgFile;
        public frmEditWindow editWindow = null;
        
        /*
         * Constructor for the frmDamoANTs form
         */
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

        /*
         * splitting up alarms & timers here, fo' sho' now
         * Alarms first follow
         */
        public partial class Alarms {
            public String name;
            public DateTime ringAt;
            public Boolean running;
            public String soundBite;
            public TimeSpan interval;  //may be phasing this out in here soon

            public void setInterval() {
                interval = ringAt - DateTime.Now;
            }

            public Boolean checkIfFiring() {
                if (debugging) {
                    Console.WriteLine("Checking if firing\nRunning value for: " +
                        name + " is: " + running.ToString() + "\nSeconds " +
                        "left: " + interval.TotalSeconds.ToString() + "\n" +
                        "Target time: " + ringAt.ToShortTimeString());
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

        /*
         * Timers class follows
         */
        public partial class Timers {
            public String name;
            public DateTime tmpTarget;  /*this needs to be recalculated
                                          after any pause; this is prolly
                                          where the bugs before were
                                          happening after pause */
            private TimeSpan interval;
            public Boolean running;
            public String soundBite;

            public void setInterval() {
                interval = tmpTarget - DateTime.Now;
            }

            public Boolean checkIfFiring() {
                if (debugging) {
                    Console.WriteLine("Checking if firing\nRunning value for: " +
                        name + " is: " + running.ToString() + "\nSeconds " +
                        "left: " + interval.TotalSeconds.ToString() + "\n" +
                        "Target time: " + tmpTarget.ToShortTimeString());
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

        /*
         * AlarmsTimers class definition; still debating whether or not to
         * split this up and make different Alarms/Timers classes; if not
         * handling it for this beta, then certainly for the first real
         * release or next release.
         */
        /* public partial class AlarmsTimers {
            public String name;
            public DateTime target;
            private TimeSpan interval;
            public Boolean running;
            public String soundBite;
            public Boolean alarm;
            public DateTime tmpTarget; */

            /* 
             * Method sets interval for an alarm; allegedly for a timer as
             * well, although this could be where the fuggup is occuring
             * when attempts are made to do timers & alarms simultaneously
             */
            /* 
             * taking this out because it's been moved to the new separate
             * classes
            public void autoSetInterval() {
                if (alarm) {
                    interval = target - DateTime.Now;
                } else {
                    interval = tmpTarget - DateTime.Now;
                }
            } */

            /* 
             * method determines whether alarm/timer is 'firing' or not
             */
            /* got this one moved to separate classes now, too
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
            } */

            /*
             * Method returns unicode zero-padded xx:xx:xx format time
             * interval remaining in the countdown
             */
            /* now fully separated
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
            } */
        //}

        /*
         * Method checks the validity of the time/date object that holds the
         * target time [alarm] or interval [timer--kludge] for the particular
         * item; obviously the total need for this needs to be factored out.
         * The potentially updated item is the return value.
         */
        private DateTime checkAlarmDay(int hr, int min, int sec) {
            //check to see if alarm is for tomorrow
            //goddamn this needs to be refactored to just utilize the
            //hr/min/sec data and ditch the date
            DateTime ouah = new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                DateTime.Now.Day, hr, min, sec);

            if (DateTime.Now >= ouah) {
                return ouah.AddDays(1);
            } else {
                return ouah;
            }
        }

        /*
         * Overloaded method that does the same as the above, but taking 
         * parameters passed as an array.
         */
        private DateTime checkAlarmDay(int[] hrMinSec) {
            DateTime ouah = new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                DateTime.Now.Day, hrMinSec[0], hrMinSec[1], hrMinSec[2]);

            if (DateTime.Now >= ouah) {
                return ouah.AddDays(1);
            } else {
                return ouah;
            }
        }

        /*
         * Method for adding a new alarm to the List and form objects through
         * button click.
         */
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
            
            Alarms tmpAlarm = new Alarms();

            tmpAlarm.soundBite = soundByteSelection();

            //check to see if alarm is for tomorrow, set other options
            tmpAlarm.ringAt = checkAlarmDay((int)numAlarmHr.Value,
                                            (int)numAlarmMin.Value,
                                            (int)numAlarmSec.Value);
            tmpAlarm.name = txtAlarmName.Text;

            grayItemNameBoxNResetNumerics(true);

            //add it to the list
            activeAls.Add(tmpAlarm);

            //add alarm to the 'active' alarms list in the checkboxlist
            addAlarm(activeAls.IndexOf(tmpAlarm));
            saveAlarmsTimers();
        }

        /*
         * Method grays out and resets text for the timer/alarm name, also
         * resets the respective numeric up/down controls back to 0.
         */
        private void grayItemNameBoxNResetNumerics(Boolean alarm) {
            if (alarm) {
                txtAlarmName.ForeColor = 
                    System.Drawing.SystemColors.InactiveCaption;
                txtAlarmName.Text = "Alarm Name Here";
                numAlarmHr.Value = 0; numAlarmMin.Value = 0; 
                numAlarmSec.Value = 0;
            } else {
                txtTimerName.ForeColor = 
                    System.Drawing.SystemColors.InactiveCaption;
                txtTimerName.Text = "Timer Name Here";
                numTimerHr.Value = 0; numTimerMin.Value = 0; 
                numTimerSec.Value = 0;
            }
        }

        /*
         * Method writes all of the current alarms and timers to the config
         * file.
         */
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
                    cFile.WriteLine(createCfgCSVString(true, cntr));
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
                    cFile.WriteLine(createCfgCSVString(false, cntr));
                } catch {
                    MessageBox.Show("Error adding activeTms[" +
                        cntr.ToString() + "]!");
                }
            }
            cFile.Close();
            return true;
        }

        /*
         * Method creates a CSV formatted string suitable for saving in the
         * cfg file for each specified alarm or timer entry.
         */
        private string createCfgCSVString(Boolean alarm, int cntr) {
            if (alarm) {
                return ("A," + activeAls[cntr].name + "," +
                    activeAls[cntr].ringAt.Hour + "," +
                    activeAls[cntr].ringAt.Minute + "," +
                    activeAls[cntr].ringAt.Second + "," +
                    activeAls[cntr].soundBite);
            } else {
                return ("T," + activeTms[cntr].name + "," +
                    activeTms[cntr].tmpTarget.Hour + "," +
                    activeTms[cntr].tmpTarget.Minute + "," +
                    activeTms[cntr].tmpTarget.Second + "," +
                    activeTms[cntr].soundBite);
            }
        }

        /*
         * Method splits a line of what should be the CSV cfg file and
         * returns the individual strings in an array
         */
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

        /*
         * Method converts the appropriate array items from the
         * parseSavedFields() method into their integer counterparts and
         * returns them in an array for inclusion into whatever DateTime or
         * interval object is appropriate
         */
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

        /*
         * Method attempts to load the alarms & timers config file, checking
         * for file corruption, incorrect permissions, and for file existance;
         * Method adds any found alarms or timers to the appropriate global
         * List objects and returns 'true' if no errors are found or 'false'
         * on finding any garbled data
         */
        private Boolean loadAlarmsTimers() {
            String[] rawFile;
            int aCntr = 0, tCntr = 0; 
            
            if (chkCfg()) { return true; }
            rawFile = readCfg();
            if (rawFile == null) {
                return false;
            }

            foreach (String raw in rawFile) {
                String[] rawFields;
                //not sure how to fix this just yet; it might require
                //a change in the config file at this point, also
                Alarms tmpAlarm = new Alarms();
                Timers tmpTimer = new Timers();

                rawFields = parseSavedFieldsLine(raw);
                if ((createTmpEntry(rawFields) == null)) { 
                    break; 
                } else if (rawFields[0].CompareTo("A") == 0) {
                    activeAls.Add(tmpAlarm);
                    addAlarm(aCntr++);
                } else if (rawFields[0].CompareTo("T") == 0) {
                    activeTms.Add(tmpTimer);
                    addTimer(tCntr++);
                } else {
                    MessageBox.Show("Issue parsing config file!",
                        "Cannot Parse DANT.cfg", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            return true;
        }

        /*
         * Method reads the config file and returns an array of strings
         * representing the data found in the file or returns null if
         * file was not readable for some reason
         */
        private string[] readCfg() {
            try {
                return System.IO.File.ReadAllLines(cfgFile);
            } catch {
                MessageBox.Show("There was an error reading " + cfgFile +
                    ", aborting.");
                return null;
            }
        }

        /*
         * Method checks for the existance of the config file; returns
         * true if nonexistent (probably not the most intuitive)
         */
        private Boolean chkCfg() {
            if (!File.Exists(cfgFile)) {
                if (debugging) {
                    Console.WriteLine("Empty or nonexistant config file " +
                        "detected");
                }
                return true;    //not an error condition
            } else { return false; }
        }

        /*
         * Method parses split field data from the config file into a
         * temporary AlarmsTimers object and returns that value
         * 
         * Due to splitting up the classes, this is going to have to
         * be reworked significantly; probably resulting in a new
         * temporary class or passing back things in an array.  
         * Leaving off here on the 'doze machine for today
        
         */
        private AlarmsTimers createTmpEntry(string[] fields) {
            if (fields[0] == null) { return null; }

            AlarmsTimers tmp = new AlarmsTimers();

            tmp.name = fields[1];
            tmp.running = false;
            tmp.target = checkAlarmDay(convertSavedFields(fields));
            tmp.soundBite = fields[5];
            if (fields[0].CompareTo("A") == 0) {
                tmp.alarm = true;
            } else {
                tmp.alarm = false;
            }

            return tmp;
        }

        private Alarms createTmpAlarm(string[] fields) {
            if (fields[0] == null) { return null; }
            if (fields[0].CompareTo("A") != 0) { return null; }
            //the behavior above may not be needed after reimplemented
            //from the calling code (where it would better live)

            Alarms tmp = new Alarms();

            tmp.name = fields[1];
            tmp.running = false;
            tmp.ringAt = checkAlarmDay(convertSavedFields(fields));
            tmp.soundBite = fields[5];

            return tmp;
        }

        private Timers createTmpTimer(string[] fields) {
            if (fields[0] == null) { return null; }
            if (fields[0].CompareTo("T") != 0) { return null; }
            //again, this will be better implemented from the outside

            Timers tmp = new Timers();
            TimeSpan ouah, ouah2;

            tmp.name = fields[1];
            tmp.running = false;
            
            ouah = new TimeSpan(0, fields[2], fields[3], fields[4]);
            tmp.tmpTarget = TimeSpan(0, TimeSpan.FromHours(fields[2]),
                                                    TimeSpan.FromHours(fields[3]),
                                                    TimeSpan.FromMinutes(fields[4]));

            //can't concentrate any more on this due to helplessly babbling
            //little man

        }

        /*
         * Method adds alarm to appropriate checklist
         */
        private void addAlarm(int alarmNo) {
            chklstAlarms.Items.Insert(alarmNo,
                (activeAls.ElementAt(alarmNo).name + " -> " +
                 addZeroesToTime(activeAls.ElementAt(alarmNo).target)));
        }

        /*
         * Method adds timer to appropriate checklist
         */
        private void addTimer(int timerNo) {
            chklstTimers.Items.Insert(timerNo,
                (activeTms.ElementAt(timerNo).name + " -> " +
                 addZeroesToTime(activeTms.ElementAt(timerNo).target)));
        }

        //private void chklstAlarms_CheckedChanged(object sender, ItemCheckEventArgs e) {
        //    if (debugging) {
        //        Console.WriteLine("activeAls.Count: " + activeAls.Count.ToString() +
        //            "e.Index: " + e.Index.ToString() + "e.NewValue.ToString(): " +
        //            e.NewValue.ToString());
        //    }
        //}

        /*
         * Method unsets a specified index number from the appropriate List
         * object, and proceeds to handle unsetting the item and fixing the
         * display in the checklist properly as well
         */
        private void unsetItem(int ndx, Boolean alarm) {
            if (alarm) {
                activeAls.ElementAt(ndx).running = false;
                //reset the checklist, too
                chklstAlarms.Items.RemoveAt(ndx);
                chklstAlarms.Items.Insert(ndx,
                    (activeAls.ElementAt(ndx).name + " -> " +
                     addZeroesToTime(activeAls.ElementAt(ndx).target)));
            } else {
                activeTms.ElementAt(ndx).running = false;
                //checklist, etc etc etc
                chklstTimers.Items.RemoveAt(ndx);
                chklstTimers.Items.Insert(ndx,
                    (activeTms.ElementAt(ndx).name + " -> " +
                     addZeroesToTime(activeTms.ElementAt(ndx).target)));
            }
        }

        /*
         * Method updates the display for specified index for the appropriate
         * checklist while counting down
         */
        private void updateDisplay(int ndx, Boolean alarm) {
            if (alarm) {
                chklstAlarms.Items.RemoveAt(ndx);
                chklstAlarms.Items.Insert(ndx,
                    activeAls.ElementAt(ndx).name + ": Remaining: " +
                    activeAls.ElementAt(ndx).returnCountdown());
                chklstAlarms.SetItemChecked(ndx, true);
            } else {
                chklstTimers.Items.RemoveAt(ndx);
                chklstTimers.Items.Insert(ndx,
                    activeTms.ElementAt(ndx).name + ": Remaining: " +
                    activeTms.ElementAt(ndx).returnCountdown());
                chklstTimers.SetItemChecked(ndx, true);
            }
        }

        /*
         * Method activates once every second as per our Timer object's
         * settings and handles countdowns and firing of alarms, etc
         */
        private void tmrOneSec_Tick(object sender, EventArgs e) {
            tickDoAlarms();
            tickDoTimers();
        }

        /*
         * Method is the once-per-second invocation that runs through the
         * countdown procedure and testing for firing for all alarms
         */
        private void tickDoAlarms() {
            for (int cntr = 0; cntr < activeAls.Count; cntr++) {
                if (!chklstAlarms.GetItemChecked(cntr)) {
                    if (debugging) {
                        Console.WriteLine("Non-Active Alarm #" +
                            cntr.ToString() + " being unset");
                    }
                    unsetItem(cntr, true);
                }

                if (chklstAlarms.GetItemChecked(cntr)) {
                    //like how the parameters are swapped?  fix that shit
                    checkAlTmSetInterval(true, cntr);
                    updateDisplay(cntr, true);

                    if (activeAls.ElementAt(cntr).checkIfFiring()) {
                        if (debugging) {
                            Console.WriteLine("Found activeAls[" +
                                cntr.ToString() + "] to be firing");
                        }
                        ringRingNeo(true, cntr);
                        if (anyRunning(false, true)) {
                            tmrOneSec.Enabled = false;
                            tmrOneSec.Stop();
                        }
                        playAudibleAlarm(false,
                            activeAls.ElementAt(cntr).soundBite, cntr);
                    }
                }
            }
        }

        /*
         * Method is the once-per-second invocation that runs through the
         * countdown procedure and testing for firing for all timers
         */
        private void tickDoTimers() {
            for (int cntr = 0; cntr < activeTms.Count; cntr++) {
                if (!chklstTimers.GetItemChecked(cntr)) {
                    if (debugging) {
                        Console.WriteLine("Non-Active Timer #" +
                            cntr.ToString() + " being unset");
                    }
                    unsetItem(cntr, false);
                }

                if (chklstTimers.GetItemChecked(cntr)) {
                    //like how the parameters are swapped?  FIX THAT SHIT
                    checkAlTmSetInterval(false, cntr);
                    updateDisplay(cntr, false);

                    if (activeTms.ElementAt(cntr).checkIfFiring()) {
                        if (debugging) {
                            Console.WriteLine("Found activeAls[" +
                                cntr.ToString() + "] to be firing");
                        }
                        ringRingNeo(false, cntr);
                        if (anyRunning(false, true)) {
                            tmrOneSec.Enabled = false;
                            tmrOneSec.Stop();
                        }
                        playAudibleAlarm(false,
                            activeTms.ElementAt(cntr).soundBite, cntr);
                    }
                }
            }
        }

        /*
         * Method sets alarm/timer to running, verifies that the TimeDate
         * property of the appropriate List item is valid, and calls 
         * autoSetInterval() to do counting down
         * NOTE: checking the TimeDate property every time the interval
         * is set is redundant, and this should be made less stupid, as well
         * as the setting of the running property to 'true' repeatedly
         */
        private void checkAlTmSetInterval(Boolean alarm, int ndx) {
            if (alarm) {
                if (activeAls.ElementAt(ndx).running == false) {
                    activeAls.ElementAt(ndx).running = true;
                    activeAls.ElementAt(ndx).target =
                        checkAlarmDay(
                            (int)activeAls.ElementAt(ndx).target.Hour,
                            (int)activeAls.ElementAt(ndx).target.Minute,
                            (int)activeAls.ElementAt(ndx).target.Second);
                }
                activeAls.ElementAt(ndx).autoSetInterval();
            } else {
                if (activeTms.ElementAt(ndx).running == false) {
                    activeTms.ElementAt(ndx).running = true;
                    activeTms.ElementAt(ndx).tmpTarget =
                        DateTime.Now.AddSeconds(
                            (activeTms.ElementAt(ndx).target.Hour * 3600) +
                            (activeTms.ElementAt(ndx).target.Minute * 60) +
                            (activeTms.ElementAt(ndx).target.Second));
                    if (debugging) {
                        Console.WriteLine("tmpTarget for Timer #" +
                            ndx.ToString() + " set to " +
                            activeTms.ElementAt(ndx).tmpTarget.ToString());
                    }
                }
                activeTms.ElementAt(ndx).autoSetInterval();
            }
        }

        /*
         * Method plays the audible alarm specified in the respective List
         * object and waits for user interaction to remove the item from
         * active status, etc
         */
        private void playAudibleAlarm(Boolean alarm, string fn, int ndx) {
            WMPLib.WindowsMediaPlayer wp =
                new WMPLib.WindowsMediaPlayer();
            wp.URL = fn;

            //NOTE: A way to loop this audio until the button is pressed needs
            //to be created
            if (fn == null) {
                SystemSounds.Beep.Play();
            } else {
                wp.controls.play();
            }

            if (alarm) {
                MessageBox.Show(activeAls.ElementAt(ndx).name +
                    ": -+=* Ring ring, Neo *=+-",
                    activeAls.ElementAt(ndx).name + " Firing",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                chklstAlarms.Items.RemoveAt(ndx);
                addAlarm(ndx);
            } else {
                MessageBox.Show(activeTms.ElementAt(ndx).name +
                    ": -+=* Ring ring, Neo *=+-",
                    activeTms.ElementAt(ndx).name + " Firing",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                chklstTimers.Items.RemoveAt(ndx);
                addTimer(ndx);
            }
            wp.controls.stop();
        }

        /*
         * Method sets respective checklist text to 'ring ring, neo'
         */
        private void ringRingNeo(Boolean alarm, int ndx) {
            if (alarm) {
                chklstAlarms.SetItemChecked(ndx, false);
                chklstAlarms.Items.RemoveAt(ndx);
                chklstAlarms.Items.Insert(ndx,
                    activeAls.ElementAt(ndx).name + " -+=* RING " +
                    " RING *=+-");
            } else {
                chklstTimers.SetItemChecked(ndx, false);
                chklstTimers.Items.RemoveAt(ndx);
                chklstTimers.Items.Insert(ndx,
                    activeTms.ElementAt(ndx).name + " -+=* RING " +
                    " RING *=+-");
            }
        }

        private void chklstAlarms_SelectedIndexChanged(object sender, EventArgs e) {
            //this works beautifully
            this.BeginInvoke(new MethodInvoker(checkActiveAlarms), null);
        }

        /*
         * Method checks whether or not any alarms, timers, or both are
         * running
         */
        private Boolean anyRunning(Boolean onlyAlarms, Boolean checkAll) {
            if (checkAll) {
                if ((chklstAlarms.CheckedIndices.Count == 0) &&
                    (chklstTimers.CheckedIndices.Count == 0)) {
                    return false;
                } else {
                    return true;
                }
            } else if (onlyAlarms) {
                if (chklstAlarms.CheckedIndices.Count == 0) {
                    return false;
                } else {
                    return true;
                }
            } else {
                if (chklstTimers.CheckedIndices.Count == 0) {
                    return false;
                } else {
                    return true;
                }
            }
        }

        /*
         * Method checks all active alarms to see if the primary timer needs
         * to be shut off, checks the DateTime property for validity, and
         * enables the primary timer if it has not already been activated and
         * is needed
         */
        private void checkActiveAlarms() {
            if (debugging) {
                Console.WriteLine("Firing chklstAlarms_Clicked");
            }

            if ((!anyRunning(false, true)) && (tmrOneSec.Enabled == true)) {
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

                    //handle checking the date due to our shitty handling
                    activeAls.ElementAt(temp).target =
                        checkAlarmDay(activeAls.ElementAt(temp).target.Hour,
                            activeAls.ElementAt(temp).target.Minute,
                            activeAls.ElementAt(temp).target.Second);

                    //enable timer if it hasn't been handled already
                    if (tmrOneSec.Enabled == false) {
                        activeAls.ElementAt(temp).running = true;
                        tmrOneSec.Enabled = true;
                        tmrOneSec.Start();
                    }
                }
            }
        }

        /*
         * Method wipes text from the alarm name textbox (ready for input)
         */
        private void txtAlarmName_Enter(object sender, EventArgs e) {
            if (txtAlarmName.Text.CompareTo("Alarm Name Here") == 0) {
                txtAlarmName.Text = "";
                txtAlarmName.ForeColor = System.Drawing.SystemColors.WindowText;
            }
        }

        /*
         * Method wipes text from the timer name textbox (ready for input)
         */
        private void txtTimerName_Enter(object sender, EventArgs e) {
            if (txtTimerName.Text.CompareTo("Timer Name Here") == 0) {
                txtTimerName.Text = "";
                txtTimerName.ForeColor = System.Drawing.SystemColors.WindowText;
            }
        }

        /*
         * Method grays out and fills in instruction text in the alarm name
         * textbox
         */
        private void txtAlarmName_Leave(object sender, EventArgs e) {
            if (txtAlarmName.Text.CompareTo("") == 0) {
                txtAlarmName.ForeColor = System.Drawing.SystemColors.InactiveCaption;
                txtAlarmName.Text = "Alarm Name Here";
            }
        }

        /*
         * Method grays out and fills in instruction text in the timer name
         * textbox
         */
        private void txtTimerName_Leave(object sender, EventArgs e) {
            if (txtTimerName.Text.CompareTo("") == 0) {
                txtTimerName.ForeColor = System.Drawing.SystemColors.InactiveCaption;
                txtTimerName.Text = "Timer Name Here";
            }
        }

        /*
         * Method is invoked when the Wipe Alarm button is clicked and 
         * removes all necessary List/checklist items
         */
        private void btnToastAlarm_Click(object sender, EventArgs e) {
            foreach (int ndx in chklstAlarms.CheckedIndices) {
                //need to add code in here to stop timer from ticking if
                //necessary (if this was the only active alarm/timer)
                chklstAlarms.Items.RemoveAt(ndx);
                activeAls.RemoveAt(ndx);
                saveAlarmsTimers();
            }
        }

        /*
         * Method is invoked when the Edit Alarm button is clicked,
         * verifies that an alarm is checked for editing, and if so, opens
         * the appropriate editing form/winder
         */
        private void btnEditAlarm_Click(object sender, EventArgs e) {
            if (chklstAlarms.CheckedIndices.Count == 0) {
                MessageBox.Show("You must check an alarm before trying to " +
                    "edit it!");
            } else {
                editWindow = new frmEditWindow(this, true);
                editWindow.Show();

                //no workee; timer keeps ticking
                if (!anyRunning(false, true)) {
                    tmrOneSec.Enabled = false;
                    tmrOneSec.Stop();
                }
            }
        }

        /*
         * Method is invoked when the edit form/winder is closed and changes
         * were made necessitating updates to the proper objects
         */
        public void editWindowMadeChanges(Boolean alarm, int ndx, String an,
            int hr, int min, int sec, String fn) {
            if (alarm) {
                activeAls[ndx].name = an;
                activeAls[ndx].target = new DateTime(DateTime.Now.Year,
                    DateTime.Now.Month, DateTime.Now.Day, hr, min, sec);
                activeAls[ndx].soundBite = fn;
                activeAls[ndx].running = false;
                activeAls[ndx].autoSetInterval();

                chklstAlarms.SetItemChecked(ndx, false);
            } else {
                activeTms[ndx].name = an;
                activeTms[ndx].target = new DateTime(DateTime.Now.Year,
                    DateTime.Now.Month, DateTime.Now.Day, hr, min, sec);
                activeTms[ndx].soundBite = fn;
                activeTms[ndx].running = false;
                activeTms[ndx].autoSetInterval();

                chklstTimers.SetItemChecked(ndx, false);
            }

            if (!saveAlarmsTimers()) {
                MessageBox.Show("Had an issue trying to save configuration");
            }
        }

        /*
         * Method pads single digit time entities with zeroes for more
         * aesthetically proper display
         */
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

        /*
         * Method handles adding new timer data to the appropriate List
         * objects and checklist
         */
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

            grayItemNameBoxNResetNumerics(false);

            //add it to the list
            activeTms.Add(tmpTimer);

            //add timer to the 'active' timers list in the checkboxlist
            addTimer(activeTms.IndexOf(tmpTimer));
            saveAlarmsTimers();
        }

        /*
         * Method is invoked for selection of the user's choice of a sound
         * file to be played when the alarm countdown is reached
         */
        private String soundByteSelection() {
            String ouah;

            DialogResult whatev = openSoundFile.ShowDialog();
            while (whatev != DialogResult.OK) {
                whatev = openSoundFile.ShowDialog();
            }

            ouah = openSoundFile.FileName;

            return ouah;
        }

        /*
         * Method verifies that an alarm time of 00:00:00 is not an error and
         * that the user wants it to go off at midnight, also if it is a
         * timer, lets the user know that a duration of 00:00:00 is invalid
         */
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

        /*
         * Method is used to invoke appropriate method for issues that need
         * to be handled when the user has futzed with the timers checklist
         */
        private void chklstTimers_SelectedIndexChanged(object sender, EventArgs e) {
            this.BeginInvoke(new MethodInvoker(checkActiveTimers), null);
        }

        /*
         * Method turns off the primary timer if necessary (refactor that 
         * out), enables the primary timer if necessary, and handles enabling
         * the specific timer List object if it has been enabled by the user
         */
        private void checkActiveTimers() {
            if (debugging) {
                Console.WriteLine("Firing chklstTimers_Clicked");
            }

            if (!anyRunning(false, true) && (tmrOneSec.Enabled == true)) {
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

        /*
         * Method is used to verify that a timer has been selected for 
         * editing when the Edit Timer button is clicked, and for opening
         * the appropriate form/winder to edit the data if necessary
         */
        private void btnEditTimer_Click(object sender, EventArgs e) {
            if (chklstTimers.CheckedIndices.Count == 0) {
                MessageBox.Show("You must check a timer before trying to " +
                    "edit it!");
            } else {
                editWindow = new frmEditWindow(this, false);
                editWindow.Show();

                //no workee; timer keeps ticking
                if (!anyRunning(false, true)) {
                    tmrOneSec.Enabled = false;
                    tmrOneSec.Stop();
                }
            }
        }

        /*
         * Method is used to wipe specific timer List data when one is
         * properly selected
         */
        private void btnToastTimer_Click(object sender, EventArgs e) {
            foreach (int ndx in chklstTimers.CheckedIndices) {
                //need to add code in here to stop timer from ticking if nec.
                chklstTimers.Items.RemoveAt(ndx);
                activeTms.RemoveAt(ndx);
                saveAlarmsTimers();
            }
        }
    }
}
