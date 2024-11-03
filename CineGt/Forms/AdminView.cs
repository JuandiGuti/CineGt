using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CineGt.Forms
{
    public partial class AdminView : Form
    {
        public String Username;
        public AdminView(String username)
        {
            InitializeComponent();
            label2.Text = label2.Text + username;
            Username = username;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Home homeForm = new Home();
            homeForm.Show();

            this.Hide();

            homeForm.FormClosed += (s, args) => Application.Exit();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            NewMovie newMovie = new NewMovie(Username);
            newMovie.ShowDialog();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            NewSession newSession = new NewSession(Username);
            newSession.ShowDialog();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CancelSession cancelSession = new CancelSession(Username);
            cancelSession.ShowDialog();
        }

        private void AdminView_Load(object sender, EventArgs e)
        {

        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ChangeSeat changeSeat = new ChangeSeat();
            changeSeat.ShowDialog();
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            buyTicketsTransaction buyTickets = new buyTicketsTransaction(Username);
            buyTickets.ShowDialog();
        }
    }
}
