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

        /* debugging flags */
        public const Boolean generalDebugging = true; //misc
        public const Boolean alarmDebugging = false;
        public const Boolean timerDebugging = true;
        public const Boolean tickDebugging = true;
        public const Boolean fileIODebugging = true;

        private String cfgFile;

        public frmEditWindow editWindow = null;
        public frmHelp helpWindow = null;

        /*
         * Constructor for the frmDamoANTs form
         */
        public frmDamoANTs() { 
            String ouah;

            ouah = System.Environment.GetFolderPath(
                Environment.SpecialFolder.Personal) + "\\DANT.cfg";
            cfgFile = ouah;

            InitializeComponent();

            try {
                loadAlarmsTimers();
            } catch (Exception e) {
                Console.WriteLine("Error in loadAlarmsTimers():");
                Console.WriteLine(e.Message);
                if (fileIODebugging) {
                    MessageBox.Show("Error loading alarms and timers\n" +
                        e.Message);
                }
            }
        }

        /*
         * Alarms first; class defines each alarm entry
         */
        public partial class Alarms {
            public String name;
            public DateTime ringAt;
            public String soundBite;

            private TimeSpan interval;  //may be phasing this out in here soon
            private Boolean hasRung = false;
            private Boolean running;

            //getters and setters
            public void setInterval() {
                interval = ringAt - DateTime.Now;
            }
            public TimeSpan getInterval() {
                return interval;
            }
            public void setHasRung() {
                hasRung = true;
            }
            public Boolean getHasRung() {
                return hasRung;
            }
            public void setRunning(Boolean isRunning) {
                running = isRunning;
            }
            public Boolean getRunning() {
                return running;
            }

            /*
             * toggles the state of the running flag and returns its new
             * value
             */
            public Boolean toggleRunning() {
                if (running == true) {
                    running = false;
                } else {
                    running = true;
                }
                return running;
            }

            //this should, perhaps, have some rewriting taking place now that
            //'hasRung' has been added to properties
            public Boolean checkIfFiring() {
                if (alarmDebugging) {
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
        }

        /*
         * Timers class follows; each Timer entry is defined as such
         * 
         * NOTE: There is still a large amount of confusion/error in
         * the switch-over to implementation to 'origInterval' utilizing
         * the 'get/setOrigInterval()' routines.  The previous ones in
         * place had utilized DateTime construction, so not all modification
         * went easy, thus leaving things more broken at this point for
         * timers than they have been for awhile
         */
        public partial class Timers {
            public String name;
            
            //public Boolean running;
            public String soundBite;

            private TimeSpan origInterval;
            private TimeSpan interval;
            private Boolean hasRung = false;
            private Boolean running;

            //getters and setters
            /*
             * this should atone for the crap of the old method below
             * pretty sure I was on drugs back then.  ohOH
             */
            public void setInterval(int hrs, int min, int sec) {
                interval = new TimeSpan(0, hrs, min, sec, 0);
            }
            //overload for setInterval to directly take a new timespan
            public void setInterval(TimeSpan newSpan) {
                interval = newSpan;
            }
            public TimeSpan getInterval() {
                return interval;
            }
            public void setHasRung() {
                hasRung = true;
            }
            public Boolean getHasRung() {
                return hasRung;
            }
            public void setRunning(Boolean isRunning) {
                running = isRunning;
            }
            public Boolean getRunning() {
                return running;
            }
            public TimeSpan getOrigInterval() {
                return origInterval;
            }
            public void setOrigInterval(TimeSpan newtime) {
                origInterval = newtime;
            }

            /*
             * toggles the state of the running flag and returns its new
             * value
             */
            public Boolean toggleRunning() {
                if (running == true) {
                    //there needs to be modification near here in order to support
                    //changing the time display to show time left if it is not the same
                    //as the total time remaining
                    running = false;
                } else {
                    running = true;
                }
                return running;
            }

            //again, this may need rewriting w/hasRung implementation now
            public Boolean checkIfFiring() {
                if (timerDebugging) {
                    Console.WriteLine("Checking if firing\nRunning value for: " +
                        name + " is: " + running.ToString() + "\nSeconds " +
                        "left: " + interval.TotalSeconds.ToString() + "\n" +
                        "Target time: " + (DateTime.Now - interval).ToShortTimeString());
                }

                //why is this duplicated from not far above?  Modularize.
                if ((interval.TotalSeconds < 1) &&
                    (interval.TotalSeconds > -1)) {
                    running = false;
                    return true;
                } else {
                    return false;
                }
            }
        }

        /*
         * Method returns the countdown for either alarm or timer
         * (part of modularizing)
         */
        public String returnCountdown(TimeSpan interval) {
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
         * 
         * It's been way too long since I've been familiar with this code;
         * why the hell did I do this?
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
                throw new DANTException("Enter valid name for the alarm/" +
                    "timer being set");
            }
            //verify that numericUpDown selectors are not at 0,0,0
            if (!legitTime((int)numAlarmHr.Value, (int)numAlarmMin.Value,
                (int)numAlarmSec.Value, true)) {
                throw new DANTException("At least one element must be " +
                    "nonzero");
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
            try {
                saveAlarmsTimers();
            } catch {
                if (fileIODebugging) {
                    Console.WriteLine("Error saving alarms/timers\n");
                }
                throw new DANTException("Error saving alarms/timers\n");
            }
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

            if (activeAls.Count() == 0) { return false; }   //wut? no timers?
            
            System.IO.StreamWriter cFile = 
                new System.IO.StreamWriter(cfgFile);

            if (fileIODebugging) {
                Console.WriteLine("Opened " + cfgFile + " for writing");
            }

            for (cntr = 0; cntr < activeAls.Count(); cntr++) {
                if (fileIODebugging) {
                    Console.WriteLine("Adding activeAls[" + cntr.ToString() +
                        "] to save file . . .");
                } 
                
                try {
                    cFile.WriteLine(createCfgCSVString(true, cntr));
                } catch {
                    MessageBox.Show("Error adding activeAls[" +
                        cntr.ToString() + "]!");
                    throw new DANTException("Error adding activeAls[" +
                                cntr.ToString() + "]!");
                }
            }
            for (cntr = 0; cntr < activeTms.Count(); cntr++) {
                if (fileIODebugging) {
                    Console.WriteLine("Adding activeTms[" + cntr.ToString() +
                        "] to save file . . .");
                } 
                
                try {
                    cFile.WriteLine(createCfgCSVString(false, cntr));
                } catch {
                    MessageBox.Show("Error adding activeTms[" +
                        cntr.ToString() + "]!");
                    throw new DANTException("Error adding activeTms[" +
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
                var ouah = "A," + activeAls[cntr].name + "," +
                    activeAls[cntr].ringAt.Hour + "," +
                    activeAls[cntr].ringAt.Minute + "," +
                    activeAls[cntr].ringAt.Second + "," +
                    activeAls[cntr].soundBite;

                if (fileIODebugging) {
                    Console.WriteLine("CSV: " + ouah);
                }

                return ouah;
            } else {
                var oiv = activeTms[cntr].getOrigInterval();
                //is the interval what we wanted to do here?  I need to review
                //the structure of the timers' data storage

                var ouah = "T," + activeTms[cntr].name + "," +
                    oiv.Hours + "," +
                    oiv.Minutes + "," +
                    oiv.Seconds + "," +
                    activeTms[cntr].soundBite;

                if (fileIODebugging) {
                    Console.WriteLine("CSV: " + ouah);
                }

                return ouah;
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
                throw new DANTException("Error parsing config file");
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
                throw new DANTException("Error parsing cfgFile fields");
            }
            return tmpTimes;
        }

        /*
         * Method attempts to load the alarms & timers config file, checking
         * for file corruption, incorrect permissions, and for file existance;
         * Method adds any found alarms or timers to the appropriate global
         * List objects and returns 'true' if no errors are found or throws
         * DANTException upon garbled data or no file found
         */
        private void loadAlarmsTimers() {
            String[] rawFile;
            int aCntr = 0, tCntr = 0; 
            
            if (chkCfg()) { return; }

            rawFile = readCfg();
            if (rawFile == null) {
                throw new DANTException("Error reading cfg file");
            }

            foreach (String raw in rawFile) {
                String[] rawFields;
                //not sure how to fix this just yet; it might require
                //a change in the config file at this point, also
                Alarms tmpAlarm = new Alarms();
                Timers tmpTimer = new Timers();

                if (fileIODebugging) {
                    Console.WriteLine("raw: " + raw);
                }

                try {
                    rawFields = parseSavedFieldsLine(raw);
                } catch {
                    if (fileIODebugging) {
                        Console.WriteLine("Error parsing saved fields " +
                            "line\n");
                    }
                    rawFields = null;   //don't remember :|
                    throw new DANTException("Error parsing saved " +
                        "fields line\n");
                }

                if (rawFields == null) {
                    if (fileIODebugging) {
                        Console.WriteLine("null string");
                    }
                    continue; 
                } else if (rawFields[0].CompareTo("A") == 0) {
                    tmpAlarm = createTmpAlarm(rawFields);
                    activeAls.Add(tmpAlarm);
                    addAlarm(aCntr++);
                } else if (rawFields[0].CompareTo("T") == 0) {
                    tmpTimer = createTmpTimer(rawFields);
                    activeTms.Add(tmpTimer);
                    addTimer(tCntr++);
                } else {
                    MessageBox.Show("Issue parsing config file!",
                        "Cannot Parse DANT.cfg", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    throw new DANTException("Yet another issue trying to " +
                        "parse the config file!");
                }
            }
            return;
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
                throw new DANTException("Error reading " + cfgFile);
            }
        }

        /*
         * Method checks for the existance of the config file; returns
         * true if nonexistent (probably not the most intuitive)
         */
        private Boolean chkCfg() {
            if (!File.Exists(cfgFile)) {
                if (generalDebugging) {
                    Console.WriteLine("Empty or nonexistant config file " +
                        "detected");
                }
                return true;    //not an error condition
            } else { return false; }
        }

        /*
         * Method creates a temporary Alarm entry to be utilized
         * prior to pushing onto the list
         */
        private Alarms createTmpAlarm(string[] fields) {
            if (fields[0] == null) { return null; }
            if (fields[0].CompareTo("A") != 0) { return null; }
            //the behavior above may not be needed after reimplemented
            //from the calling code (where it would better live)

            Alarms tmp = new Alarms();

            tmp.name = fields[1];
            tmp.setRunning(false);
            tmp.ringAt = checkAlarmDay(convertSavedFields(fields));
            tmp.soundBite = fields[5];

            return tmp;
        }

        /*
         * Method creates a temporary timer to be utilized before
         * pushing it onto the active stack of timers
         */
        private Timers createTmpTimer(string[] fields) {
            if (fields[0] == null) { return null; }
            if (fields[0].CompareTo("T") != 0) { return null; }

            Timers tmp = new Timers();
            int[] ouah = null;

            tmp.name = fields[1];
            tmp.setRunning(false);
            ouah = convertSavedFields(fields);
            tmp.setInterval(ouah[0], ouah[1], ouah[2]);

            //this should be changed to include tryParse, perhaps, and throw
            //an exception if something bogus is added
            /*try {
                for (int guh = 0; guh < 3; guh++) {
                    if (fileIODebugging) {
                        Console.WriteLine("Field " + (guh + 2) + ": " + 
                            fields[guh + 2]);
                    }
                    ouah[guh] = int.Parse(fields[guh + 2]);
                }
            } catch {
                throw new DANTException(
                    "Unable to parse fields for tmpTimer");
            }*/
            //we now need this in 'interval', also, as that one will be used
            //for counting down
            //in hindsight, these should both be set by TimeSpan, not one by
            //such and the other by 3 integers :P
            tmp.setOrigInterval(new TimeSpan(ouah[0], ouah[1], ouah[2]));
            //tmp.setInterval(ouah[0], ouah[1], ouah[2]);

            return tmp;
        }

        /*
         * Method adds alarm to appropriate checklist
         */
        private void addAlarm(int alarmNo) {
            chklstAlarms.Items.Insert(alarmNo,
                (activeAls.ElementAt(alarmNo).name + " -> " +
                 addZeroesToAlarm(activeAls.ElementAt(alarmNo).ringAt)));
        }

        /*
         * Method adds timer to appropriate checklist
         */
        private void addTimer(int timerNo) {
            chklstTimers.Items.Insert(timerNo,
                (activeTms.ElementAt(timerNo).name + " -> " +
                 activeTms.ElementAt(timerNo).getOrigInterval().ToString()));
        }

        /*
         * Method unsets a specified index number from the appropriate List
         * object, and proceeds to handle unsetting the item and fixing the
         * display in the checklist properly as well
         */
        private void unsetItem(int ndx, Boolean alarm) {
            if (alarm) {
                activeAls.ElementAt(ndx).setRunning(false);
                //reset the checklist, too
                chklstAlarms.Items.RemoveAt(ndx);
                chklstAlarms.Items.Insert(ndx,
                    (activeAls.ElementAt(ndx).name + " -> " +
                     addZeroesToAlarm(activeAls.ElementAt(ndx).ringAt)));
            } else {
                activeTms.ElementAt(ndx).setRunning(false);
                //checklist, etc etc etc
                chklstTimers.Items.RemoveAt(ndx);
                chklstTimers.Items.Insert(ndx,
                    (activeTms.ElementAt(ndx).name + " -> " +
                     addZeroesToTimer(
                        activeTms.ElementAt(ndx).getOrigInterval())) +
                    ": Rem -> " + (addZeroesToTimer(
                        activeTms.ElementAt(ndx).getInterval())));
                    
            }
        }

        /*
         * Method updates the display for specified index for the appropriate
         * checklist while counting down
         */
        private void updateDisplay(int ndx, Boolean alarm) {
            Boolean isSelected = false;

            if (alarm) {
                if (chklstAlarms.SelectedIndex == ndx) {
                    isSelected = true;
                }
                chklstAlarms.Items.RemoveAt(ndx);
                chklstAlarms.Items.Insert(ndx,
                    activeAls.ElementAt(ndx).name + ": Remaining: " +
                    returnCountdown(activeAls.ElementAt(ndx).getInterval()));
                chklstAlarms.SetItemChecked(ndx, true);
                if (isSelected) {
                    chklstAlarms.SelectedIndex = ndx;
                }
            } else {
                if (chklstTimers.SelectedIndex == ndx) {
                    isSelected = true;
                }
                chklstTimers.Items.RemoveAt(ndx);
                chklstTimers.Items.Insert(ndx,
                    activeTms.ElementAt(ndx).name + ": Remaining: " +
                    returnCountdown(activeTms.ElementAt(ndx).getInterval()));
                chklstTimers.SetItemChecked(ndx, true);
                if (isSelected) {
                    chklstTimers.SelectedIndex = ndx;
                }
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
            int selected = -1;

            for (int cntr = 0; cntr < activeAls.Count; cntr++) {
                if (tickDebugging && (cntr == 0)) {
                    Console.WriteLine("Alarm Tick");
                }

                selected = chklstAlarms.SelectedIndex;

                if (chklstAlarms.GetItemChecked(cntr)) {
                    if (tickDebugging) {
                        Console.WriteLine("Checked: " + cntr);
                    }
                    checkAlTmSetInterval(cntr, true);
                    updateDisplay(cntr, true);

                    if (activeAls.ElementAt(cntr).checkIfFiring()) {
                        if (tickDebugging) {
                            Console.WriteLine("Found activeAls[" +
                                cntr.ToString() + "] to be firing");
                        }

                        if (!activeAls.ElementAt(cntr).getHasRung()) {
                            activeAls.ElementAt(cntr).setHasRung();

                            playAudibleAlarm(true,
                                activeAls.ElementAt(cntr).soundBite, cntr);
                        }
                    }
                } else {
                    unsetItem(cntr, true);
                }

                if (selected != -1) { 
                    chklstAlarms.SetSelected(selected, true);
                }
            }
        }

        /*
         * Method is the once-per-second invocation that runs through the
         * countdown procedure and testing for firing for all timers
         */
        private void tickDoTimers() {
            int selected = -1;

            for (int cntr = 0; cntr < activeTms.Count; cntr++) {
                if (tickDebugging && (cntr == 0)) {
                    Console.WriteLine("Timer Tick");
                }

                selected = chklstTimers.SelectedIndex;

                if (chklstTimers.GetItemChecked(cntr)) {
                    Console.WriteLine("Checked: " + cntr);

                    //like how the parameters are swapped?  FIX THAT SHIT
                    checkAlTmSetInterval(cntr, false);
                    updateDisplay(cntr, false);

                    if (activeTms.ElementAt(cntr).checkIfFiring()) {
                        if (tickDebugging) {
                            Console.WriteLine("Found activeAls[" +
                                cntr.ToString() + "] to be firing");
                        }
                        if (!activeTms.ElementAt(cntr).getHasRung()) {
                            activeTms.ElementAt(cntr).setHasRung();

                            playAudibleAlarm(false,
                                activeTms.ElementAt(cntr).soundBite, cntr);
                        }
                    } else {
                        var ouah = activeTms.ElementAt(cntr).getInterval();
                        ouah -= new TimeSpan(0, 0, 1);
                        activeTms.ElementAt(cntr).setInterval(ouah.Hours,
                            ouah.Minutes, ouah.Seconds);
                    }
                } else {
                    unsetItem(cntr, false);
                }

                if (selected != -1) { 
                    chklstTimers.SetSelected(selected, true);
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
        private void checkAlTmSetInterval(int ndx, Boolean alarm) {
            if (alarm) {
                if (activeAls.ElementAt(ndx).getRunning() == false) {
                    activeAls.ElementAt(ndx).setRunning(true);
                    activeAls.ElementAt(ndx).ringAt =
                        checkAlarmDay(
                            (int)activeAls.ElementAt(ndx).ringAt.Hour,
                            (int)activeAls.ElementAt(ndx).ringAt.Minute,
                            (int)activeAls.ElementAt(ndx).ringAt.Second);
                }
                activeAls.ElementAt(ndx).setInterval();
            } else {
                if (activeTms.ElementAt(ndx).getRunning() == false) {
                    activeTms.ElementAt(ndx).setRunning(true);
                    //activeTms.ElementAt(ndx).tmpTarget =
                        //DateTime.Now + activeTms.ElementAt(ndx).getInterval();
                    //why does this only have debugging output for timer?
                    if (tickDebugging || timerDebugging) {
                        Console.WriteLine("tmpTarget for Timer #" +
                            ndx.ToString() + " set to " +
                            activeTms.ElementAt(ndx).getInterval());
                    }
                }
                //activeTms.ElementAt(ndx).setInterval(); //BUG
            }
        }

        /*
         * Method simply plays the audio or the beep: trying to modularize code
         */
        private WMPLib.WindowsMediaPlayer playAudio(string fn) {
            WMPLib.WindowsMediaPlayer wp =
                new WMPLib.WindowsMediaPlayer();
            wp.URL = fn;
            
            //NOTE: A way to loop this audio until the button is pressed needs
            //to be created
            if (fn == null) {
                SystemSounds.Beep.Play();
            } else {
                try {
                    wp.controls.play();
                } catch {
                    throw new DANTException("Unable to play audio: " + fn);
                }
            }

            return wp;
        }

        /*
         * Method plays the audible alarm specified in the respective List
         * object and waits for user interaction to remove the item from
         * active status, etc
         */
        private void playAudibleAlarm(Boolean alarm, string fn, int ndx) {
            WMPLib.WindowsMediaPlayer wp = playAudio(fn);
            //insert code here to loop this again until there is a button hit

            ringRingNeo(alarm, ndx);

            wp.controls.stop(); //this needs to only happen after a keypress
        }

        /*
         * Method sets respective checklist text to 'ring ring, neo'
         */
        private void ringRingNeo(Boolean alarm, int ndx) {
            if (alarm) {

                chklstAlarms.Items.RemoveAt(ndx);
                addAlarm(ndx);

                chklstAlarms.SetItemChecked(ndx, false);
                chklstAlarms.Items.RemoveAt(ndx);
                chklstAlarms.Items.Insert(ndx,
                    activeAls.ElementAt(ndx).name + " -+=* RING " +
                    " RING *=+-");

                MessageBox.Show(activeAls.ElementAt(ndx).name +
                    ": -+=* Ring ring, Neo *=+-",
                    activeAls.ElementAt(ndx).name + " Firing",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            } else {
                chklstTimers.Items.RemoveAt(ndx);
                addTimer(ndx);

                chklstTimers.SetItemChecked(ndx, false);
                chklstTimers.Items.RemoveAt(ndx);
                chklstTimers.Items.Insert(ndx,
                    activeTms.ElementAt(ndx).name + " -+=* RING " +
                    " RING *=+-");

                MessageBox.Show(activeTms.ElementAt(ndx).name +
                    ": -+=* Ring ring, Neo *=+-",
                    activeTms.ElementAt(ndx).name + " Firing",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }
        }

        /*
         * Method is to be employed any time the selected index in the 
         * Alarms list ends up changing
         */
        private void chklstAlarms_SelectedIndexChanged(object sender, 
                                                       EventArgs e) {
            //this works beautifully
            this.BeginInvoke(new MethodInvoker(checkActiveAlarms), null);
        }

        /*
         * Method checks whether or not any alarms, timers, or both are
         * running
         */
        private Boolean anyRunning(Boolean onlyAlarms, Boolean checkAll) {
            if (checkAll) {
                if (generalDebugging) {
                    Console.Write("Checking all alarms/timers: ");
                }

                if ((chklstAlarms.CheckedIndices.Count == 0) &&
                    (chklstTimers.CheckedIndices.Count == 0)) {
                    if (generalDebugging) {
                        Console.WriteLine("None checked");
                    }
                    
                    return false;
                } else {
                    if (generalDebugging) {
                        Console.WriteLine("Found checked");
                    }

                    return true;
                }
            } else if (onlyAlarms) {
                if (generalDebugging) { 
                    Console.Write("Checking alarms: ");
                }

                if (chklstAlarms.CheckedIndices.Count == 0) {
                    if (generalDebugging) {
                        Console.WriteLine("None checked");
                    }

                    return false;
                } else {
                    if (generalDebugging) {
                        Console.WriteLine("Found checked");
                    }

                    return true;
                }
            } else {
                if (generalDebugging) { 
                    Console.Write("Checking timers: ");
                }

                if (chklstTimers.CheckedIndices.Count == 0) {
                    if (generalDebugging) {
                        Console.WriteLine("None checked");
                    }

                    return false;
                } else {
                    if (generalDebugging) { 
                        Console.WriteLine("Found checked");
                    }

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
            if (alarmDebugging) {
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
                if (activeAls.ElementAt(temp).getRunning() != true) {
                    if (alarmDebugging) {
                        Console.WriteLine("Flagged alarm #" + temp.ToString());
                    }

                    //handle checking the date due to our shitty handling
                    activeAls.ElementAt(temp).ringAt =
                        checkAlarmDay(activeAls.ElementAt(temp).ringAt.Hour,
                            activeAls.ElementAt(temp).ringAt.Minute,
                            activeAls.ElementAt(temp).ringAt.Second);

                    //enable timer if it hasn't been handled already
                    if (tmrOneSec.Enabled == false) {
                        activeAls.ElementAt(temp).setRunning(true);
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
            int ndx = chklstAlarms.SelectedIndex;

            if (activeAls[ndx].getRunning()) {
                activeAls[ndx].toggleRunning();
            }
            chklstAlarms.Items.RemoveAt(ndx);
            activeAls.RemoveAt(ndx);
            try {
                saveAlarmsTimers();
            } catch {
                if (generalDebugging) {
                    Console.WriteLine("Error toasting alarm.");
                    throw new DANTException("Error toasting alarm");
                }
            }   //there really should be more error handling

        }

        /*
         * Method is invoked when the Edit Alarm button is clicked,
         * verifies that an alarm is checked for editing, and if so, opens
         * the appropriate editing form/winder
         */
        private void btnEditAlarm_Click(object sender, EventArgs e) {
            //trying out selection-based editing to avoid some confusion
            if (chklstAlarms.SelectedIndex == -1) {
                MessageBox.Show("You must select an alarm before trying to " +
                    "edit it!");
            } else {
                if (anyRunning(false, true)) {
                    tmrOneSec.Enabled = false;
                    tmrOneSec.Stop();
                }

                editWindow = new frmEditWindow(this, true);
                editWindow.Show();
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
                activeAls[ndx].ringAt = new DateTime(DateTime.Now.Year,
                    DateTime.Now.Month, DateTime.Now.Day, hr, min, sec);
                activeAls[ndx].soundBite = fn;
                activeAls[ndx].setRunning(false);
                activeAls[ndx].setInterval();

                chklstAlarms.SetItemChecked(ndx, false);

                updateDisplay(ndx, true);
            } else {
                activeTms[ndx].name = an;
                activeTms[ndx].setOrigInterval(new TimeSpan(hr, min, sec));
                activeTms[ndx].soundBite = fn;
                activeTms[ndx].setRunning(false);
                activeTms[ndx].setInterval(hr, min, sec);
                chklstTimers.SetItemChecked(ndx, false);

                updateDisplay(ndx, false);
            }

            try {
                saveAlarmsTimers();
            } catch {
                MessageBox.Show("Had an issue trying to save configuration");
                if (fileIODebugging) {
                    Console.WriteLine("Issue saving alarms/timers config\n");
                }
            }
        }

        /*
         * Method pads single digit time entities with zeroes for more
         * aesthetically proper display
         */
        private String addZeroesToTimer(TimeSpan ouah) {
            int hr, min, sec;
            String targetZeroesAdded;

            hr = ouah.Hours; min = ouah.Minutes; sec = ouah.Seconds;

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
         * Method padds the single zero strings in an alarm entry in order to
         * make them more visually appealing as above (so below ahrhrhr)
         */
        private String addZeroesToAlarm(DateTime ouah) {
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
                throw new DANTException("Not legit time from legitTime()\n");
            }

            Timers tmpTimer = new Timers();

            tmpTimer.name = txtTimerName.Text;
            tmpTimer.setInterval((int) numTimerHr.Value, 
                                 (int) numTimerMin.Value,
                                 (int) numTimerSec.Value);
            tmpTimer.setOrigInterval(new TimeSpan((int) numTimerHr.Value,
                                      (int) numTimerMin.Value,
                                      (int) numTimerSec.Value));

            tmpTimer.setRunning(false);
            tmpTimer.soundBite = soundByteSelection();

            grayItemNameBoxNResetNumerics(false);

            //add it to the list
            activeTms.Add(tmpTimer);

            //add timer to the 'active' timers list in the checkboxlist
            addTimer(activeTms.IndexOf(tmpTimer));
            try {
                saveAlarmsTimers();
            } catch {
                if (fileIODebugging) {
                    Console.WriteLine("Error saving alarms/timers\n");
                }
                throw new DANTException("Error saving alarms/timers");
            }
        }

        /*
         * Method is invoked for selection of the user's choice of a sound
         * file to be played when the alarm countdown is reached
         */
        private String soundByteSelection() {
            String ouah;
            DialogResult whatev;

            whatev = MessageBox.Show("Use the console beep?", "Beep or Media File",
                                     MessageBoxButtons.YesNo);

            if (whatev == DialogResult.Yes) {
                return null;
            } else {
                whatev = openSoundFile.ShowDialog();
                while (whatev != DialogResult.OK) {
                    whatev = openSoundFile.ShowDialog();
                }

                ouah = openSoundFile.FileName;

                return ouah;
            }
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
                throw new DANTException("Invalid timer duration");
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
            if (timerDebugging) {
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
                if (activeTms.ElementAt(temp).getRunning() != true) {
                    if (timerDebugging) {
                        Console.WriteLine("Flagged timer #" + temp.ToString());
                    }

                    //enable timer if it hasn't been handled already
                    if (tmrOneSec.Enabled == false) {
                        //not sure if this is correct in the 'if' conditional
                        /*if (activeTms.ElementAt(temp).tmpTarget ==
                            (DateTime.Now + 
                             activeTms.ElementAt(temp).getInterval())) {
                            /*
                             * this is a CRAPPY way to handle this; I really
                             * need to separate AlarmsTimers into 2 classes,
                             * one for each sort of list to avoid this kludge
                             */
                            /* activeTms.ElementAt(temp).tmpTarget =
                                DateTime.Now + activeTms.ElementAt(temp).getInterval();
                        }*/
                        activeTms.ElementAt(temp).setRunning(true);
                        tmrOneSec.Enabled = true;
                        tmrOneSec.Start();
                    }
                }
            }
        }

        /*
         * Method is used to display general help for the application, nothing more
         * at this point, though this will probably be expounded upon in the future
         * 
         * BUG: Shit won't show no matter what right now :(
         */
        private void btnGetHelp_Click(object sender, EventArgs e) {
            Console.WriteLine("generalDebugging: " + generalDebugging);
            if (generalDebugging) {
                Console.WriteLine("Click to enter help window picked up");
            }

            helpWindow = new frmHelp(this);
            helpWindow.Show();
        }

        /*
         * Method is used to verify that a timer has been selected for 
         * editing when the Edit Timer button is clicked, and for opening
         * the appropriate form/winder to edit the data if necessary
         */
        private void btnEditTimer_Click(object sender, EventArgs e) {
            if (chklstTimers.SelectedIndex == -1) {
                MessageBox.Show("You must check a timer before trying to " +
                    "edit it!");
            } else {
                if (anyRunning(false, true)) {
                    tmrOneSec.Enabled = false;
                    tmrOneSec.Stop();
                }
                
                editWindow = new frmEditWindow(this, false);
                editWindow.Show();

            }
        }

        /*
         * Method is used to wipe specific timer List data when one is
         * properly selected
         */
        private void btnToastTimer_Click(object sender, EventArgs e) {
            int ndx = chklstTimers.SelectedIndex;

            chklstTimers.Items.RemoveAt(ndx);
            activeTms.RemoveAt(ndx);
            try {
                saveAlarmsTimers();
            } catch {
                if (fileIODebugging) {
                    Console.WriteLine("Error saving alarms/timers");
                }
                throw new DANTException("Error saving alarms/timers");
            }

        }

        /*
         * method is used to reset timer to its original interval
         */
        private void btnResetTimer_Click(object sender, EventArgs e) {
            //Boolean foundAny = false;
            
            if (timerDebugging) {
                Console.Write("Entered btnResetTimer_Click\nResetting: ");
            }

            int ndx = chklstTimers.SelectedIndex;

            if (timerDebugging && (ndx != -1)) {
                Console.WriteLine(ndx + " ");

                activeTms.ElementAt(ndx).setInterval(
                    activeTms.ElementAt(ndx).getOrigInterval());
                activeTms.ElementAt(ndx).setRunning(false);
                chklstTimers.SetItemCheckState(ndx, CheckState.Unchecked);
            } else if (timerDebugging) {
                Console.WriteLine("Nothing, apparently");
            }

        }

    }
    
    public class DANTException : ApplicationException {
        /*
         * method exists solely for throwing as an exception
         */
        public DANTException(String s) : base (s) {
        }
    }
}
