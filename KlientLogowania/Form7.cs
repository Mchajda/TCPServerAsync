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
    public partial class Form7 : Form
    {
        Form1 mainForm;

        public Form7()
        {
            InitializeComponent();
        }

        public Form7(Form1 _mainform)
        {
            InitializeComponent();
            mainForm = _mainform;

            InitializeControls();
        }

        private void InitializeControls()
        {
            //Dodawanie kontrolek, czyli wypisywanie listy znajomych obok są dwa przyciski - jeden "Rozpocznij czat", drugi "Usuń znajomego"

            for (int i = 0; i < 9; i++)
            {
                Label nameLabel = new Label();
                nameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
                nameLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
                nameLabel.Location = new System.Drawing.Point(3, 0);
                nameLabel.Name = "label" + i;
                nameLabel.Size = new System.Drawing.Size(240, 40);
                nameLabel.TabIndex = 3*i;
                nameLabel.Text = "label" + i;
                nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
                flowLayoutPanel1.Controls.Add(nameLabel);

                Button chatButton = new Button();
                chatButton.BackgroundImage = global::KlientLogowania.Properties.Resources.pobrane;
                chatButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
                chatButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
                chatButton.ForeColor = System.Drawing.Color.Red;
                chatButton.Location = new System.Drawing.Point(269, 3);
                chatButton.Name = "chatButton" + i;
                chatButton.Size = new System.Drawing.Size(40, 40);
                chatButton.TabIndex = 3*i + 1;
                chatButton.UseVisualStyleBackColor = true;
                chatButton.Show();
                chatButton.Click += new System.EventHandler(this.OpenChat);
                flowLayoutPanel1.Controls.Add(chatButton);

                Button deleteButton = new Button();
                deleteButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
                deleteButton.ForeColor = System.Drawing.Color.Red;
                deleteButton.Location = new System.Drawing.Point(315, 3);
                deleteButton.Name = "deleteButton" + i;
                deleteButton.Size = new System.Drawing.Size(40, 40);
                deleteButton.TabIndex = 3*i + 2;
                deleteButton.Text = "X";
                deleteButton.UseVisualStyleBackColor = true;
                deleteButton.Click += new System.EventHandler(this.button1_Click);
                deleteButton.Show();
                deleteButton.Click += new System.EventHandler(this.DeleteFriend);
                flowLayoutPanel1.Controls.Add(deleteButton);
            }
        }

        private void OpenChat(object sender, EventArgs e)
        {
            //this opens chat

            var trigger = sender as Button;
            MessageBox.Show(trigger.Name);


        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            // opens add a friend form
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
