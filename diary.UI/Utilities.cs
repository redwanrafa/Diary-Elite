using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace diary.UI
{
    public partial class Utilities : Form
    {
        public Utilities()
        {
            InitializeComponent();
        }

        private void btncalc_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("calc");


        }

        private void btnback_Click(object sender, EventArgs e)
        {
            DiaryFrame d = new DiaryFrame();
            d.Show();
            this.Hide();
            
            

        }

        private void btnbrowser_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("chrome");
        }

        private void btncontacts_Click(object sender, EventArgs e)
        {

            System.Diagnostics.Process.Start("wab");

        }

        private void btnmedia_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("wmplayer");
        }

        private void btnpad_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("wordpad");
        }
    }
}
