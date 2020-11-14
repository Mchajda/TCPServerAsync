using Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KlientLogowania
{
    public partial class Form1 : Form
    {
        Form2 register;
        ServerEchoAPM server;

        public Form1()
        {
            InitializeComponent();
            register = new Form2(this);
            label3.Text = "";
            server = new ServerEchoAPM(new System.Net.IPAddress(new byte[] { 127, 0, 0, 1 }), 2311);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            register.Show();
        }
    }
}
