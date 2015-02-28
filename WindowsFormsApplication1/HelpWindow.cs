using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DamosAlarmsNTimers {
    public partial class frmHelp : Form {
        private frmDamoANTs primaryWindow;

        public frmHelp(frmDamoANTs pf) {
            InitializeComponent();

            primaryWindow = pf;
        }

        private void btnCloseHelp_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}
