using diary.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace diary.UI
{
    public partial class AppointmentNotification : Form
    {
        public string Username;
        public string Date;
        UserAccess userAccess = new UserAccess();
        public AppointmentNotification()
        {
            InitializeComponent();
        }

        public void set()
        {
            labelTime.Text = "You have an appointment at "+userAccess.getAppointmentTime(Username, Date);
            labelDescription.Text = "Description: "+userAccess.getAppointmentDescription(Username, Date);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
