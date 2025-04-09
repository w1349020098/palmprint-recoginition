using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PalRecSystem
{
    public partial class start: UIForm
    {
        public start()
        {
            InitializeComponent();
        }

        public start(userstart userstart)
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            Rec rec = new Rec(this);
            rec.Show();
        }

        private void uiButton3_Click(object sender, EventArgs e)
        {
            userstart userst = new userstart(this);
            userst.Show();
            Hide();

        }

        private void start_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            managerstart managerstart = new managerstart(this);
            managerstart.Show();
            Hide();
        }
    }
}
