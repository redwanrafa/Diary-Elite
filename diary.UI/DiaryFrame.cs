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
using System.Media;

namespace diary.UI
{
    public partial class DiaryFrame : MetroForm
    {
        AppointmentNotification appointmentNotification = new AppointmentNotification();
        LoginFrame lf = new LoginFrame();
       // UserInfo s = new UserInfo();
        UserAccess sp = new UserAccess();
        Appointment app = new Appointment();
        public string AppointmentTitle;
        public string AppointmentTime;

        byte[] Image; 

        OpenFileDialog op = new OpenFileDialog();

        string Username;
        public DiaryFrame()
        {
            InitializeComponent();
            Image = new byte[10];
            AppointmentTitle = app.AppointmentTitle;
            AppointmentTime = app.AppointmentTime;
        }

        public DiaryFrame(LoginFrame lf,string Username)
        {
            InitializeComponent();
            this.lf = lf;
            this.Username = Username;
            labelTitle.Text = "Hello! " + Username;
            Image = new byte[10];
            AppointmentTitle = app.AppointmentTitle;
            AppointmentTime = app.AppointmentTime;

            

        }
        private void timer_Tick(object sender, EventArgs e)
        {
            int hr;
            hr = Convert.ToInt32(DateTime.Now.ToString("HH"));
            if (hr > 12)
            {
                hr -= 12;
                time.Text = hr + ":" + DateTime.Now.ToString("mm:ss tt");
            }
            else
                time.Text = DateTime.Now.ToString("HH:mm:ss tt");

            date.Text = DateTime.Now.ToString("dd/MM/yyyy ");
            try
            {
                if (sp.checkReminder(Username, metroDateTime.Text, Convert.ToInt32(DateTime.Now.ToString("HH")), Convert.ToInt32(DateTime.Now.ToString("mm")), Convert.ToInt32(DateTime.Now.ToString("ss")), Convert.ToInt32(DateTime.Now.ToString("fffff"))))
                {
                    //MessageBox.Show("TEST");
                    //MessageBox.Show(Convert.ToDouble(DateTime.Now.ToString("fff"))+" ");
                    appointmentNotification.Username = Username;
                    appointmentNotification.Date = metroDateTime.Text;
                    appointmentNotification.set();
                    appointmentNotification.Show();
                    
                    System.Threading.Thread.Sleep(1000);

                    SoundPlayer simpleSound = new SoundPlayer(Properties.Resources.alert);
                    simpleSound.Play();
                    
                   
                }
            }
            catch (Exception)
            {
            }


        }
        private void buttonOpen_Click(object sender, EventArgs e)
        {
            groupBox1.SendToBack();
            groupPage.BringToFront();
            buttonNext.Show();
            buttonPrevious.Show();
            buttonOpen.Hide();
            buttonClose.Show();
            buttonSave.Enabled = true;
            buttonAttachImage.Enabled = true;
            buttonAddAppointment.Enabled = true;

            if (sp.checkDate(Username, metroDateTime.Text))
            {
                richTextBox.Text = sp.loadData(Username, metroDateTime.Text, ref Image);
                try
                {
                    MemoryStream ms1 = new MemoryStream(Image);
                    pictureImage.Image = Bitmap.FromStream(ms1);
                }
                catch (Exception) { Image = null; }
            }
            else
            {
                richTextBox.Text = "What's in your mind today?";
            }

        }
        private void buttonClose_Click(object sender, EventArgs e)
        {
            groupPage.SendToBack();

            buttonNext.Hide();
            buttonPrevious.Hide();
            buttonOpen.Show();
            buttonClose.Hide();
            buttonSave.Enabled = false;
            buttonAttachImage.Enabled = false;
            buttonAddAppointment.Enabled = false;
            this.pictureImage.Image = global::diary.UI.Properties.Resources.picframe;
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (sp.checkDate(Username, metroDateTime.Text))
            {
                sp.Update(Username, metroDateTime.Text, richTextBox.Text, Image, app.AppointmentTitle, app.AppointmentTime, app.AppointmentMode);
                richTextBox.Text = sp.loadData(Username, metroDateTime.Text, ref Image);
                AppointmentTime = sp.getAppointmentTime();
                AppointmentTitle = sp.getAppointmentTitle();
                try
                {
                    MemoryStream ms1 = new MemoryStream(Image);
                    pictureImage.Image = Bitmap.FromStream(ms1);
                }
                catch (Exception) { Image = null; }
                MessageBox.Show("Updated");
            }
            else
            {
                sp.Insert(Username, metroDateTime.Text, richTextBox.Text, ref Image, app.AppointmentTitle, app.AppointmentTime, app.AppointmentMode);
                MessageBox.Show("Saved");
            }
        }


        private void metroDateTime_ValueChanged(object sender, EventArgs e)
        {
            Image = null;
            this.pictureImage.Image = global::diary.UI.Properties.Resources.picframe;
            if (sp.checkDate(Username, metroDateTime.Text))
            {
                richTextBox.Text = sp.loadData(Username, metroDateTime.Text,ref Image);
                AppointmentTime = sp.getAppointmentTime();
                AppointmentTitle = sp.getAppointmentTitle();
                try
                {
                    MemoryStream ms1 = new MemoryStream(Image);
                    pictureImage.Image = Bitmap.FromStream(ms1);
                }
                catch (Exception)
                {
                    Image = null;
                    this.pictureImage.Image = global::diary.UI.Properties.Resources.picframe;
                }
            }
            else
            {
                richTextBox.Text = "What's in your mind today?";
                this.pictureImage.Image = global::diary.UI.Properties.Resources.picframe;
            }
        }
        private void richText_Changed(object sender, EventArgs e)
        {
        }


        private void DiaryFrame_Load(object sender, EventArgs e)
        {
            buttonSave.Enabled = false;
            buttonAttachImage.Enabled = false;
           
        }

        private void buttonAttachImage_Click(object sender, EventArgs e)
        {
            try
            {
                op.InitialDirectory = @"C:\";
                op.Filter = "Image files (*.jpg, *.png, *.bmp)|*.jpg; *.png; *.bmp|All Files (*.*)|*.*";
                op.ShowDialog();

                FileStream fs = new FileStream(op.FileName, FileMode.Open, FileAccess.Read);
                Image = new byte[fs.Length];
                fs.Read(Image, 0, Convert.ToInt32(fs.Length));
                fs.Close();
                if (File.Exists(op.FileName))
                {
                    MemoryStream ms1 = new MemoryStream(Image);
                    pictureImage.Image = Bitmap.FromStream(ms1);
                }
            }
            catch (Exception) { }


        }


        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            metroDateTime.Value=metroDateTime.Value.AddDays(-1);
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            metroDateTime.Value=metroDateTime.Value.AddDays(1);

        }

        private void buttonLogOut_Click(object sender, EventArgs e)
        {
            lf.Show();
            lf.BringToFront();
            this.Hide();
            lf.setTextBox("","");
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            string h = MessageBox.Show("Are you Sure?", "Exit Application", MessageBoxButtons.YesNo).ToString();
            if(h=="Yes")
            {
               this.Close();
               Application.Exit();
            }          
        }

        private void DiaryFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            lf.Show();
            lf.BringToFront();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            app.AppointmentTime = AppointmentTime;
            app.AppointmentTitle = AppointmentTitle;
            app.Show();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show(app.AppointmentTitle+" "+app.AppointmentTime);
        }

       

        private void btnuti_Click(object sender, EventArgs e)
        {
            Utilities u = new Utilities();
            this.Hide();
            u.Show();
        }
    }
}
