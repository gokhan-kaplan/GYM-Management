using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Fitness
{
    public partial class FormTaksit : Form
    {
        Form2 mainFrm = new Form2();
        public FormTaksit()
        {
            InitializeComponent();
        }

        int gelenTutar = 0;

        public void toplamTutar(int a)
        {
            gelenTutar = a;
        }

        private void FormTaksit_Load(object sender, EventArgs e)
        {

        }

        int[] textBoxes = new int[2];
        DateTime[] dtpickers = new DateTime[2];
        int ilk = 0;
        int iki = 0;
        int dogrula = 0;

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void button2_Click(object sender, EventArgs e)
        {
            ilk = Convert.ToInt32(textBox1.Text.ToString());
            iki = Convert.ToInt32(textBox2.Text.ToString());
            dogrula = ilk + iki;
            if (dogrula == gelenTutar)
            {
                textBoxes[0] = ilk;
                textBoxes[1] = iki;
                dtpickers[0] = dateTimePicker1.Value.Date;
                dtpickers[1] = dateTimePicker2.Value.Date;

                var principalForm = Application.OpenForms.OfType<Form2>().Single();
                //MessageBox.Show("Ödeme " + ilk + " & " + iki + " Olmak Üzere İkiye Bölündü.");
                AutoClosingMessageBox.Show("Ödeme, " + ilk + " & " + iki + " olmak üzere ikiye bölündü.", "Ödeme Şekli", 4000);
                principalForm.taksitlendir(textBoxes, dtpickers);
                this.Close();
            }
            else
            {
                MessageBox.Show("Toplam tutar, Ödenen tutar ile eşleşmiyor.");
            }
        }
        public class AutoClosingMessageBox
        {
            System.Threading.Timer _timeoutTimer;
            string _caption;
            AutoClosingMessageBox(string text, string caption, int timeout)
            {
                _caption = caption;
                _timeoutTimer = new System.Threading.Timer(OnTimerElapsed,
                    null, timeout, System.Threading.Timeout.Infinite);
                MessageBox.Show(text, caption);
            }
            public static void Show(string text, string caption, int timeout)
            {
                new AutoClosingMessageBox(text, caption, timeout);
            }
            void OnTimerElapsed(object state)
            {
                IntPtr mbWnd = FindWindow(null, _caption);
                if (mbWnd != IntPtr.Zero)
                    SendMessage(mbWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                _timeoutTimer.Dispose();
            }
            const int WM_CLOSE = 0x0010;
            [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
            static extern IntPtr FindWindow(string lPClassName, string lpWindowName);
            [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        }

    }
}
