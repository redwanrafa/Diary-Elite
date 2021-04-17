using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using diary.Entities;
using diary.DAL;
using diary.BLL;
using System.IO;

namespace diary.UI
{
    public partial class Appointment : Form
    {
        public string AppointmentTitle;
        public string Hour;
        public string Minute;
        public string AppointmentTime;
        public string AppointmentMode;
        public Appointment()
        {
            InitializeComponent();
            hour.SelectedIndex = 0;
            minute.SelectedIndex = 0;
            remindMode.SelectedIndex = 0;
            remindMode.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            this.Hide();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            AppointmentTitle = appointmentTitle.Text;
            Hour = hour.Text;
            Minute = minute.Text;
            AppointmentTime = hour.Text + ":" + minute.Text;
            AppointmentMode = remindMode.Text;
            this.Hide();
        }
    }
}
