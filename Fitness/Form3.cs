using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Fitness
{
    public partial class Form3 : Form
    {
        Form1 mainFrm = new Form1();
        public string aranacak;

        public Form3()
        {
            InitializeComponent();

        }

        private void Form3_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }

        public void button11_Click(object sender, EventArgs e)
        {

            aranacak = textBox1.Text;
            var principalForm = Application.OpenForms.OfType<Form1>().Single();
            principalForm.arama(aranacak);
            this.Close();

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button11.PerformClick();
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

    }
}
