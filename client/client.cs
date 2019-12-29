using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project1
{
    public partial class client : Form
    {
        static void Main()
        {
            client newClient = new client();
            newClient.ShowDialog();
        }

        clientProcess clientpro = new clientProcess();

        public client()
        {
            InitializeComponent();
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            clientpro.IpServer = IPServer.Text;
            if(!clientpro.ConnectToServer())
            {
                connectButton.BackColor = Color.DarkRed;
                connectButton.Text = "Connect"; //

                MessageBox.Show("can't connect to the server");

            }
            else
            {
                connectButton.BackColor = Color.Blue;
                connectButton.Text = "Connected";
            }
        }

        private void send_Click(object sender, EventArgs e)
        {
            if (clientpro.Is_connect())
            {
                Set_font("Me: " + textMessage.Text, Color.DarkGreen);
                Set_font("Server: " + clientpro.SendAnswer(textMessage.Text), Color.DarkBlue);

                this.textMessage.Clear();
            }
            else
            {
                MessageBox.Show("you are not connected");
            }
        }

        private void Set_font(string sentence, Color type)
        {
            textChat.SelectionColor = Color.Black;
            textChat.SelectionFont = new Font("Microsoft Sans Serif", 9, FontStyle.Regular);
            textChat.SelectedText = DateTime.Now.ToString() + Environment.NewLine;

            textChat.SelectionColor = type;
            textChat.SelectionFont = new Font("Microsoft Sans Serif", 11, FontStyle.Regular);
            textChat.SelectedText = sentence + Environment.NewLine + Environment.NewLine;
        }
    }
}
