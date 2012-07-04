using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

/*
 * BUGS:
 * * When loading the save file, the target date is calculated automatically
 *   which, if the application is left running beyond the magic amount of time,
 *   will cause the target date to be in the past, and thus bogus.  Time needs 
 *   to be saved just as the hours, minutes, seconds, and calculated only when 
 *   the alarm is activated in order to fix this problem.
 * * More than one active alarm fucks things up; only one alarm can run at a
 *   time, although more than one can now be successfully checked.
 */

namespace WindowsFormsApplication1
{
    public partial class frmDamoANTs : Form
    {
        public List<AlarmsTimers> activeAls = new List<AlarmsTimers>();
        public List<AlarmsTimers> activeTms = new List<AlarmsTimers>();
        private const Boolean debugging = true;
        private const String cfgFile = 
            @"\Users\Rohypnol Larry\Desktop\DANT.cfg";

        public frmDamoANTs()
        {
            InitializeComponent();
            if (!loadAlarmsTimers())
            {
                MessageBox.Show("Issues loading saved alarms & timers!");
            }
        }

        private void btnAddAlarm_Click(object sender, EventArgs e)
        {
            AlarmsTimers tmpAlarm = new AlarmsTimers();
            
            //verify that numericUpDown selectors are not at 0,0,0
            if ((numAlarmHr.Value == 0) && 
                (numAlarmMin.Value == 0) &&
                (numAlarmSec.Value == 0))
            {
                //bogus entry
                Console.WriteLine("You must enter a valid time for the " +
                    "alarm to go off!");
                return;
            }

            //verify that name box is not still full of default value
            if (txtAlarmName.Text.CompareTo("Alarm Name Here") == 0)
            {
                Console.WriteLine("You must enter a valid name for the " +
                    "alarm that you are setting!");
                return;
            }

            //check to see if alarm is for tomorrow
            if ((DateTime.Now >=  
                 new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                              DateTime.Now.Day, (int) numAlarmHr.Value,
                              (int) numAlarmMin.Value, 
                              (int) numAlarmSec.Value))) {
                //make it tomorrow
                tmpAlarm.target = 
                    new DateTime(DateTime.Now.AddDays(1).Year,
                        DateTime.Now.AddDays(1).Month,
                        DateTime.Now.AddDays(1).Day, (int) numAlarmHr.Value,
                        (int) numAlarmMin.Value, (int) numAlarmSec.Value);
            } else {
                tmpAlarm.target =
                    new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                                 DateTime.Now.Day, (int) numAlarmHr.Value,
                                 (int) numAlarmMin.Value, 
                                 (int) numAlarmSec.Value);
            }

            //set interval & name (actually we don't need to set interval here)
            //tmpAlarm.autoSetInterval();
            tmpAlarm.name = txtAlarmName.Text;
            tmpAlarm.running = true;

            //reset textbox & numericUpDowns
            txtAlarmName.Text = "Alarm Name Here";
            numAlarmHr.Value = 0;
            numAlarmMin.Value = 0;
            numAlarmSec.Value = 0;

            //add it to the list (do not set it running for now, we'll
            //handle that with the checkboxlist code)
            activeAls.Add(tmpAlarm);

            //add alarm to the 'active' alarms list in the checkboxlist
            addAlarm(activeAls.IndexOf(tmpAlarm));
            //and, of course, add it to our configuration file
            saveAlarmsTimers();
        }
        
        private Boolean saveAlarmsTimers() {
            if (activeAls.Count() == 0) { return false; }
            
            System.IO.StreamWriter cFile = 
                new System.IO.StreamWriter(cfgFile);

            if (debugging)
            {
                Console.WriteLine("Opened " + cfgFile + " for writing");
            }

            for (int cntr = 0; cntr < activeAls.Count(); cntr++) {
                if (debugging)
                {
                    Console.WriteLine("Adding activeAls[" + cntr.ToString() +
                        "] to save file . . .");
                }
                try
                {
                    cFile.WriteLine("A," + activeAls[cntr].name + "," +
                        activeAls[cntr].target.Hour + "," + 
                        activeAls[cntr].target.Minute + "," +
                        activeAls[cntr].target.Second);
                }
                catch
                {
                    Console.WriteLine("Error adding activeAls[" +
                        cntr.ToString() + "]!");
                }
            }
            cFile.Close();
            return true;
        }

        private Boolean loadAlarmsTimers()
        {
            if (!File.Exists(cfgFile)) {
                if (debugging) {
                    Console.WriteLine("Empty or nonexistant config file " +
                        "detected");
                }
                return true;    //not an error condition
            }

            String[] rawFile;
            int cntr = 0;

            try
            {
                //no need to close after ReadAllLines()
                rawFile = System.IO.File.ReadAllLines(cfgFile);
            }
            catch
            {
                MessageBox.Show("There was an error reading " + cfgFile +
                    ", aborting.");
                return false;
            }

            foreach (String raw in rawFile)
            {
                String[] rawFields;
                //DateTime tmpTime;
                int tmpHr, tmpMin, tmpSec;
                Boolean tmpFlag = false;
                AlarmsTimers tmpAlarm = new AlarmsTimers();

                rawFields = raw.Split(',');
                if (rawFields.Count() != 5)
                {
                    //error in the textfile
                    MessageBox.Show("Error parsing " + cfgFile +
                        "; please check what the hell is going on and try " +
                        "again later :)");
                    break;
                }
                if (rawFields[0] == "A")
                {
                    //We're dealing with an alarm
                    if (!Int32.TryParse(rawFields[2], out tmpHr))
                    {
                        tmpFlag = true;
                    }
                    if (!Int32.TryParse(rawFields[3], out tmpMin))
                    {
                        tmpFlag = true;
                    }
                    if (!Int32.TryParse(rawFields[4], out tmpSec))
                    {
                        tmpFlag = true;
                    }
                    if (tmpFlag)
                    {
                        MessageBox.Show("There was an error trying to parse " +
                            "one of the fields in " + cfgFile + "\nPlease " +
                            "try to find out what the hell is going on and " +
                            "try again later.");
                        return false;
                    }

                    //add to active alarms
                    tmpAlarm.name = rawFields[1];
                    tmpAlarm.running = false;
                    tmpFlag = false;

                    if (tmpHr < DateTime.Now.Hour)
                    {
                        tmpFlag = true;
                    }
                    else if ((tmpHr == DateTime.Now.Hour) &&
                      (tmpMin < DateTime.Now.Minute))
                    {
                        tmpFlag = true;
                    }
                    else if ((tmpHr == DateTime.Now.Hour) &&
                      (tmpMin == DateTime.Now.Minute) &&
                      (tmpSec < DateTime.Now.Second))
                    {
                        tmpFlag = true;
                    }
                    if (tmpFlag)
                    {
                        tmpAlarm.target =
                            new DateTime(DateTime.Now.AddDays(1).Year,
                                DateTime.Now.AddDays(1).Month,
                                DateTime.Now.AddDays(1).Day, tmpHr,
                                tmpMin, tmpSec);
                    }
                    else
                    {
                        tmpAlarm.target =
                            new DateTime(DateTime.Now.Year,
                                DateTime.Now.Month,
                                DateTime.Now.Day, tmpHr, tmpMin, tmpSec);
                    }
                    //of course the preceding code will have to be changed as
                    //the application will occasionally probably run more than
                    //one day rendering the dates calculated above bogus
                    //(see hint in BUGS at the top of the code)
                    activeAls.Add(tmpAlarm);
                    addAlarm(cntr++);
                }
                else
                {
                    //we're working with a timer here (add code later)
                }
            }

            return true;
        }

        private void addAlarm(int alarmNo)
        {
            //need to add code to verify that we aren't adding the same name more than
            //once as that is how we are going to be identifying specific instances
            //ACTUALLY this should be moot as they'll be indexed by # in the list

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

        /*  --==** ALARM/TIMER CLASS **==-- */
        public partial class AlarmsTimers
        {
            public String name;
            public DateTime target;
            private TimeSpan interval;
            public Boolean running;

            //method correctly sets interval for an alarm
            public void autoSetInterval()
            {
                interval = target - DateTime.Now;
            }

            //method determines whether alarm/timer is 'firing' or not
            public Boolean checkIfFiring()
            {
                if (debugging)
                {
                    Console.WriteLine("Checking if firing\nRunning value for: " +
                        name + " is: " + running.ToString() + "\nSeconds " +
                        "left: " + interval.TotalSeconds.ToString() + "\n" +
                        "Target time: " + target.ToShortTimeString());
                }

                if ((interval.TotalSeconds < 1) &&
                    (interval.TotalSeconds > -1))
                {
                    //firing
                    //MessageBox.Show("Ring ring, Neo.");
                    //add other firing code here
                    running = false;
                    return true;
                }
                else
                {
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

        private void chklstAlarms_CheckedChanged(object sender, ItemCheckEventArgs e)
        {
            if (debugging)
            {
                Console.WriteLine("activeAls.Count: " + activeAls.Count.ToString() +
                    "e.Index: " + e.Index.ToString() + "e.NewValue.ToString(): " +
                    e.NewValue.ToString());
            }
            for (int cntr = 0; cntr < activeAls.Count; cntr++)
            {
                if ((e.Index == cntr) && (e.NewValue == CheckState.Checked))
                {
                    if (debugging)
                    {
                        Console.WriteLine(e.NewValue.ToString());
                    }
                    if (activeAls.ElementAt(cntr).running == false) {
                        activeAls.ElementAt(cntr).running = true;
                        if (tmrOneSec.Enabled == false) {
                            tmrOneSec.Enabled = true;
                            tmrOneSec.Start();
                            //noneRunning = false;
                        }
                    }
                }
                else if ((e.Index == cntr) && (e.NewValue == CheckState.Unchecked))
                {
                    if (activeAls.ElementAt(cntr).running == true)
                    {
                        activeAls.ElementAt(cntr).running = false;
                    }
                }
            }

        }

        private void tmrOneSec_Tick(object sender, EventArgs e) {
            //alarms
            for (int cntr = 0; cntr < activeAls.Count; cntr++) {
                //presume this is running
                if (activeAls.ElementAt(cntr).running == true) {
                    activeAls.ElementAt(cntr).autoSetInterval();

                    //update the display
                    chklstAlarms.Items.RemoveAt(cntr);
                    chklstAlarms.Items.Insert(cntr, 
                        activeAls.ElementAt(cntr).name + ": Remaining: " +
                        activeAls.ElementAt(cntr).returnCountdown());
                    chklstAlarms.SetItemChecked(cntr, true);

                    if (activeAls.ElementAt(cntr).checkIfFiring()) {
                        if (debugging) {
                            Console.WriteLine("Found activeAls[" +
                                cntr.ToString() + "] to be firing");
                        }
                        chklstAlarms.Items.RemoveAt(cntr);
                        tmrOneSec.Enabled = false;
                        tmrOneSec.Stop();
                        MessageBox.Show("Ring ring, Neo.");
                        addAlarm(cntr);
                    }
                }
            }
        }

        private void chklstAlarms_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this works beautifully
            this.BeginInvoke(new MethodInvoker(checkActiveAlarms), null);
        }

        private void checkActiveAlarms()
        {
            //going to need something here to check the unchecked ones, too, to see if
            //anything has stopped running :|
            if (debugging)
            {
                Console.WriteLine("Firing chklstAlarms_Clicked");
            }

            foreach (int temp in chklstAlarms.CheckedIndices)
            {
                //these are specifically checked, so double checking that in the
                //conditional should not be necessary
                if (activeAls.ElementAt(temp).running != true) {
                    if (debugging)
                    {
                        Console.WriteLine("Flagged #" + temp.ToString());
                    }

                    if (tmrOneSec.Enabled == false) {
                        activeAls.ElementAt(temp).running = true;
                        tmrOneSec.Enabled = true;
                        tmrOneSec.Start();
                    }
                }
            }

        }

        private void txtAlarmName_Enter(object sender, EventArgs e)
        {
            if (txtAlarmName.Text.CompareTo("Alarm Name Here") == 0)
            {
                txtAlarmName.Text = "";
                txtAlarmName.ForeColor = System.Drawing.SystemColors.WindowText;
            }
        }

        private void txtTimerName_Enter(object sender, EventArgs e)
        {
            if (txtTimerName.Text.CompareTo("Timer Name Here") == 0)
            {
                txtTimerName.Text = "";
                txtAlarmName.ForeColor = System.Drawing.SystemColors.WindowText;
            }
        }

        private void txtAlarmName_Leave(object sender, EventArgs e)
        {
            if (txtAlarmName.Text.CompareTo("") == 0)
            {
                txtAlarmName.ForeColor = System.Drawing.SystemColors.InactiveCaption;
                txtAlarmName.Text = "Alarm Name Here";
            }
        }

        private void txtTimerName_Leave(object sender, EventArgs e)
        {
            if (txtTimerName.Text.CompareTo("") == 0)
            {
                txtTimerName.ForeColor = System.Drawing.SystemColors.InactiveCaption;
                txtTimerName.Text = "Timer Name Here";
            }
        }

        private void btnAddTimer_Click(object sender, EventArgs e) {

        }

    }
}
