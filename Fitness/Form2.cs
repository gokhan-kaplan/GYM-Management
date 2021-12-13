using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

using System.IO;

namespace Fitness
{
    public partial class Form2 : Form
    {
        public TextBox TxtBox = new TextBox();
        string yeniTxt;
        OpenFileDialog ofd = new OpenFileDialog();

        Form1 mainFrm = new Form1();

        public int[] textBoxesAlıcı;
        public DateTime[] dtpickersAlıcı;
        public int bcdurum = 0;
        public int taksitSayisi = 0;

        public Form2()
        {
            InitializeComponent();
        }

        //taksit seçenekleri penceresi açılır
        private void button3_Click(object sender, EventArgs e)
        {
            checkBox1.Checked = false;
            if (textBox4.Text == "")
            {
                MessageBox.Show("Fiyat Boş Bırakılamaz");
            }
            else
            {
                FormTaksit frmTaksit = new FormTaksit();
                int fiyatGonder = Convert.ToInt32(textBox4.Text.ToString());
                frmTaksit.toplamTutar(fiyatGonder);
                frmTaksit.ShowDialog();
            }
        }

        //odenecek tutarı ikiye böl.
        public void taksitlendir(int[] a, DateTime[] b)
        {
            int toplam_fiyat = Convert.ToInt32(textBox4.Text);
            textBoxesAlıcı = (int[])a.Clone();
            dtpickersAlıcı = (DateTime[])b.Clone();
            kalanTutar = toplam_fiyat - a[0];
        }

        int odenenTutar = 0;
        int toplamTutar = 0;
        int kalanTutar = 0;
        //Kaydetmek için gerekli method'a yönlendiriliyor.
        private void button2_Click(object sender, EventArgs e)
        {
            kaydet1();
        }
        ////////////Ödeme şekli belirleniyor. 
        public void kaydet1()
        {
            try
            {
                toplamTutar = Convert.ToInt32(textBox4.Text.ToString());
                if (checkBox1.Checked == true)
                {
                    odenenTutar = toplamTutar;
                    kalanTutar = 0;
                }
                else
                {
                    odenenTutar = textBoxesAlıcı[0];
                    kalanTutar = toplamTutar - textBoxesAlıcı[0];
                }
            }
            catch (Exception) { }
            kaydet2();
        }
        public void kaydet2()
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString =
            "Data Source=PC;Initial Catalog=fitness;Integrated Security=True";
            con.Open();

            try
            {
                //uyelik bitis hesaplama
                DateTimePicker dtp = new DateTimePicker();
                int kalan_tarih = Int32.Parse(numericUpDown1.Value.ToString());
                dtp.Value = dateTimePicker1.Value.AddMonths(kalan_tarih);

                SqlCommand komut = new SqlCommand("INSERT INTO uye (ad,soyad,cinsiyet,kayit_uzunlugu,fiyat_toplam,fiyat_odenen,fiyat_kalan,baslangic_trh,bitis_trh,calisma_prog) VALUES (@ad, @soyad, @cins,@kayit_sure,@fiyat_toplam,@fiyat_odenen,@fiyat_kalan,@baslangic_trh,@bitis,@calisma_prog) SET @ID = SCOPE_IDENTITY()", con);
                komut.Parameters.AddWithValue("@ad", textBox1.Text);
                komut.Parameters.AddWithValue("@soyad", textBox2.Text);
                komut.Parameters.AddWithValue("@cins", comboBox1.Text);
                komut.Parameters.AddWithValue("@kayit_sure", numericUpDown1.Value.ToString());
                komut.Parameters.AddWithValue("@fiyat_toplam", toplamTutar);
                komut.Parameters.AddWithValue("@fiyat_odenen", odenenTutar);
                komut.Parameters.AddWithValue("@fiyat_kalan", kalanTutar);
                komut.Parameters.AddWithValue("@baslangic_trh", dateTimePicker1.Value.Date);
                komut.Parameters.AddWithValue("@bitis", dtp.Value.Date);
                komut.Parameters.AddWithValue("@calisma_prog", textBox14.Text);
                komut.Parameters.Add("@ID", SqlDbType.Int).Direction = ParameterDirection.Output;
                komut.ExecuteNonQuery();

                ////////////////-----scope
                int result = Convert.ToInt32(komut.Parameters["@ID"].Value);
                label1.Text = result.ToString();
                ///////////////////

                if (textBox15.Text != "")
                {
                    //image kaydetme
                    byte[] imageData = ReadImageFile(textBox15.Text);
                    SqlCommand komutfoto = new SqlCommand("UPDATE uye SET fotograf=@ft WHERE uye_id LIKE " + result + "", con);
                    komutfoto.Parameters.Add(new SqlParameter("@ft", imageData));
                    komutfoto.ExecuteNonQuery();
                }

                int sifir = 0;
                SqlCommand komut2 = new SqlCommand("INSERT INTO gelisim (uye_id,kilo,omuz,gogus,bel,sag_kol,sol_kol,sag_bacak,sol_bacak,basen,kilo_son,omuz_son,gogus_son,bel_son,sag_kol_son,sol_kol_son,sag_bacak_son,sol_bacak_son,basen_son) VALUES (@uye,@kiloo,@omuz,@gogus,@bel,@sag_kol,@sol_kol,@sag_bacak,@sol_bacak,@basen,@kilo_son,@omuz_son,@gogus_son,@bel_son,@sag_kol_son,@sol_kol_son,@sag_bacak_son,@sol_bacak_son,@basen_son)", con);
                komut2.Parameters.AddWithValue("@uye", result);
                komut2.Parameters.AddWithValue("@kiloo", textBox6.Text);
                komut2.Parameters.AddWithValue("@omuz", textBox7.Text);
                komut2.Parameters.AddWithValue("@gogus", textBox8.Text);
                komut2.Parameters.AddWithValue("@bel", textBox9.Text);
                komut2.Parameters.AddWithValue("@sag_kol", textBox10.Text);
                komut2.Parameters.AddWithValue("@sol_kol", textBox11.Text);
                komut2.Parameters.AddWithValue("@sag_bacak", textBox13.Text);
                komut2.Parameters.AddWithValue("@sol_bacak", textBox5.Text);
                komut2.Parameters.AddWithValue("@basen", textBox3.Text);
                komut2.Parameters.AddWithValue("@kilo_son", sifir);
                komut2.Parameters.AddWithValue("@omuz_son", sifir);
                komut2.Parameters.AddWithValue("@gogus_son", sifir);
                komut2.Parameters.AddWithValue("@bel_son", sifir);
                komut2.Parameters.AddWithValue("@sag_kol_son", sifir);
                komut2.Parameters.AddWithValue("@sol_kol_son", sifir);
                komut2.Parameters.AddWithValue("@sag_bacak_son", sifir);
                komut2.Parameters.AddWithValue("@sol_bacak_son", sifir);
                komut2.Parameters.AddWithValue("@basen_son", sifir);
                komut2.ExecuteNonQuery();
                if (checkBox1.Checked == true)
                {
                    SqlCommand komut5 = new SqlCommand("INSERT INTO Borc (uye_id,borc,odeme_tarihi,durum,odeme_sekli) VALUES (@uye,@borc,@odm_trh,@drm,@odmsekli)", con);
                    komut5.Parameters.AddWithValue("@uye", result);
                    komut5.Parameters.AddWithValue("@borc", toplamTutar);
                    komut5.Parameters.AddWithValue("@odm_trh", System.DateTime.Now);
                    komut5.Parameters.AddWithValue("@drm", "ÖDENDİ");
                    komut5.Parameters.AddWithValue("@odmsekli", "Nakit");
                    komut5.ExecuteNonQuery();
                }
                else if (textBoxesAlıcı != null || textBoxesAlıcı.Length != 0)
                {
                    SqlCommand komut3 = new SqlCommand("INSERT INTO Borc (uye_id,borc,odeme_tarihi,durum,odeme_sekli) VALUES (@uye,@borc,@odm_trh,@drm,@odmsekli)", con);
                    komut3.Parameters.AddWithValue("@uye", result);
                    komut3.Parameters.AddWithValue("@borc", textBoxesAlıcı[0]);
                    komut3.Parameters.AddWithValue("@odm_trh", dtpickersAlıcı[0]);
                    komut3.Parameters.AddWithValue("@drm", "ÖDENDİ");
                    komut3.Parameters.AddWithValue("@odmsekli", "Taksit");
                    komut3.ExecuteNonQuery();
                    

                    SqlCommand komut4 = new SqlCommand("INSERT INTO Borc (uye_id,borc,odeme_tarihi,durum,odeme_sekli) VALUES (@uye,@borc,@odm_trh,@drm,@odmsekli)", con);
                    komut4.Parameters.AddWithValue("@uye", result);
                    komut4.Parameters.AddWithValue("@borc", textBoxesAlıcı[1]);
                    komut4.Parameters.AddWithValue("@odm_trh", dtpickersAlıcı[1]);
                    komut4.Parameters.AddWithValue("@drm", "ÖDENMEDİ");
                    komut4.Parameters.AddWithValue("@odmsekli", "Taksit");
                    komut4.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { }
            //ana form'da uye tablosunu yenileyip, kayıt formunu kapatma

            con.Close();
            MessageBox.Show(textBox1.Text + " Adlı Yeni Üye Sisteme Eklendi");

            var principalForm = Application.OpenForms.OfType<Form1>().Single();
            principalForm.uyeGoster();
            this.Close();
        }

        //resim yukleme butonu
        private void button1_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == ofd.ShowDialog())
            {
                textBox15.Text = ofd.FileName;
            }
        }

        public byte[] ReadImageFile(string imageLocation)
        {
            byte[] imageData = null;
            FileInfo fileInfo = new FileInfo(imageLocation);
            long imageFileLength = fileInfo.Length;
            FileStream fs = new FileStream(imageLocation, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            imageData = br.ReadBytes((int)imageFileLength);
            return imageData;
        }
        // eğer comboBox nesnesinden cinsiyet kadın olarak seçilirse ise basen textBox'ının bulundugu tableLayout görünür olacak
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox3.Text = "";
            if (comboBox1.SelectedIndex == 1)
            {
                tableLayoutPanel2.Visible = true;
            }
            else if (comboBox1.SelectedIndex == 0)
            {
                tableLayoutPanel2.Visible = false;
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                AutoClosingMessageBox.Show("Toplam tutar, tek seferde ödendi.", "Ödeme Şekli", 1000);
                try
                {
                    Array.Clear(textBoxesAlıcı, 0, textBoxesAlıcı.Length);
                    Array.Clear(dtpickersAlıcı, 0, dtpickersAlıcı.Length);
                }
                catch (Exception ex) { }
            }
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            panel1.BackColor = Color.Yellow;
            label1.BackColor = Color.Yellow;
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            panel1.BackColor = Color.Lavender;
            label1.BackColor = Color.Lavender;
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            panel2.BackColor = Color.Yellow;
            label2.BackColor = Color.Yellow;
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            panel2.BackColor = Color.Lavender;
            label2.BackColor = Color.Lavender;
        }

        private void numericUpDown1_Enter(object sender, EventArgs e)
        {
            panel3.BackColor = Color.Yellow;
            label3.BackColor = Color.Yellow;
        }

        private void numericUpDown1_Leave(object sender, EventArgs e)
        {
            panel3.BackColor = Color.Lavender;
            label3.BackColor = Color.Lavender;
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            panel4.BackColor = Color.Yellow;
            label4.BackColor = Color.Yellow;
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            panel4.BackColor = Color.Lavender;
            label4.BackColor = Color.Lavender;
        }

        private void dateTimePicker1_Enter(object sender, EventArgs e)
        {
            panel5.BackColor = Color.Yellow;
            label5.BackColor = Color.Yellow;
        }

        private void dateTimePicker1_Leave(object sender, EventArgs e)
        {
            panel5.BackColor = Color.Lavender;
            label5.BackColor = Color.Lavender;
        }

        private void textBox6_Enter(object sender, EventArgs e)
        {
            panel6.BackColor = Color.Yellow;
            label6.BackColor = Color.Yellow;
        }

        private void textBox6_Leave(object sender, EventArgs e)
        {
            panel6.BackColor = Color.Lavender;
            label6.BackColor = Color.Lavender;
        }

        private void textBox7_Enter(object sender, EventArgs e)
        {
            panel7.BackColor = Color.Yellow;
            label7.BackColor = Color.Yellow;
        }

        private void textBox7_Leave(object sender, EventArgs e)
        {
            panel7.BackColor = Color.Lavender;
            label7.BackColor = Color.Lavender;
        }

        private void textBox8_Enter(object sender, EventArgs e)
        {
            panel8.BackColor = Color.Yellow;
            label8.BackColor = Color.Yellow;
        }

        private void textBox8_Leave(object sender, EventArgs e)
        {
            panel8.BackColor = Color.Lavender;
            label8.BackColor = Color.Lavender;
        }

        private void textBox9_Enter(object sender, EventArgs e)
        {
            panel9.BackColor = Color.Yellow;
            label9.BackColor = Color.Yellow;
        }

        private void textBox9_Leave(object sender, EventArgs e)
        {
            panel9.BackColor = Color.Lavender;
            label9.BackColor = Color.Lavender;
        }

        private void textBox10_Enter(object sender, EventArgs e)
        {
            panel10.BackColor = Color.Yellow;
            label10.BackColor = Color.Yellow;
        }

        private void textBox10_Leave(object sender, EventArgs e)
        {
            panel10.BackColor = Color.Lavender;
            label10.BackColor = Color.Lavender;
        }

        private void textBox11_Enter(object sender, EventArgs e)
        {
            panel11.BackColor = Color.Yellow;
            label11.BackColor = Color.Yellow;
        }

        private void textBox11_Leave(object sender, EventArgs e)
        {
            panel11.BackColor = Color.Lavender;
            label11.BackColor = Color.Lavender;
        }

        private void comboBox1_Enter(object sender, EventArgs e)
        {
            panel13.BackColor = Color.Yellow;
            label13.BackColor = Color.Yellow;
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            panel13.BackColor = Color.Lavender;
            label13.BackColor = Color.Lavender;
        }

        private void textBox12_Enter(object sender, EventArgs e)
        {
            panel15.BackColor = Color.Yellow;
            label15.BackColor = Color.Yellow;
        }

        private void textBox12_Leave(object sender, EventArgs e)
        {
            panel15.BackColor = Color.Lavender;
            label15.BackColor = Color.Lavender;
        }

        private void textBox13_Enter(object sender, EventArgs e)
        {
            panel16.BackColor = Color.Yellow;
            label16.BackColor = Color.Yellow;
        }

        private void textBox13_Leave(object sender, EventArgs e)
        {
            panel16.BackColor = Color.Lavender;
            label16.BackColor = Color.Lavender;
        }

        private void textBox5_Enter(object sender, EventArgs e)
        {
            panel17.BackColor = Color.Yellow;
            label17.BackColor = Color.Yellow;
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            panel17.BackColor = Color.Lavender;
            label17.BackColor = Color.Lavender;
        }

        private void textBox14_Enter(object sender, EventArgs e)
        {
            panel12.BackColor = Color.Yellow;
            label12.BackColor = Color.Yellow;
        }

        private void textBox14_Leave(object sender, EventArgs e)
        {
            panel12.BackColor = Color.Lavender;
            label12.BackColor = Color.Lavender;
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            panel14.BackColor = Color.Yellow;
            label14.BackColor = Color.Yellow;
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            panel14.BackColor = Color.Lavender;
            label14.BackColor = Color.Lavender;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            TxtBox = textBox1;
            KontrolEt();
            textBox1.Text = yeniTxt;
            textBox1.SelectionStart = textBox1.Text.Length;

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            TxtBox = textBox2;
            KontrolEt();
            textBox2.Text = yeniTxt;
            textBox2.SelectionStart = textBox2.Text.Length;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            TxtBox = textBox4;
            KontrolEt2();
            textBox4.Text = yeniTxt;
            textBox4.SelectionStart = textBox4.Text.Length;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            TxtBox = textBox6;
            KontrolEt2();
            textBox6.Text = yeniTxt;
            textBox6.SelectionStart = textBox6.Text.Length;
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            TxtBox = textBox7;
            KontrolEt2();
            textBox7.Text = yeniTxt;
            textBox7.SelectionStart = textBox7.Text.Length;
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            TxtBox = textBox8;
            KontrolEt2();
            textBox8.Text = yeniTxt;
            textBox8.SelectionStart = textBox8.Text.Length;
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            TxtBox = textBox9;
            KontrolEt2();
            textBox9.Text = yeniTxt;
            textBox9.SelectionStart = textBox9.Text.Length;
        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            TxtBox = textBox10;
            KontrolEt2();
            textBox10.Text = yeniTxt;
            textBox10.SelectionStart = textBox10.Text.Length;
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            TxtBox = textBox11;
            KontrolEt2();
            textBox11.Text = yeniTxt;
            textBox11.SelectionStart = textBox11.Text.Length;
        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            TxtBox = textBox13;
            KontrolEt2();
            textBox13.Text = yeniTxt;
            textBox13.SelectionStart = textBox13.Text.Length;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            TxtBox = textBox5;
            KontrolEt2();
            textBox5.Text = yeniTxt;
            textBox5.SelectionStart = textBox5.Text.Length;
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            TxtBox = textBox14;
            KontrolEt();
            textBox14.Text = yeniTxt;
            textBox14.SelectionStart = textBox14.Text.Length;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            TxtBox = textBox3;
            KontrolEt2();
            textBox3.Text = yeniTxt;
            textBox3.SelectionStart = textBox3.Text.Length;
        }

        private void Form2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        public void KontrolEt()
        {
            ///Bu kontrolü yazmazsak textboxtan son karakter silinmiyor!!!
            if (TxtBox.Text == "")
            {
                yeniTxt = "";
                TxtBox.Text = "";
            }
            try
            {
                foreach (char Karakter in TxtBox.Text)
                {

                    if (char.IsLetter(Karakter) || char.IsWhiteSpace(Karakter))
                    {
                        yeniTxt = TxtBox.Text;
                        RegexOptions options = RegexOptions.None;
                        Regex regex = new Regex(@"[ ]{2,}", options);
                        yeniTxt = regex.Replace(yeniTxt, @" ");
                    }
                    else
                    {
                        yeniTxt = TxtBox.Text;
                        MessageBox.Show("Bu Alana Sayı ve Karakter Girilemez!");
                        yeniTxt = yeniTxt.Remove(yeniTxt.Length - 1);
                    }
                }
            }
            catch (Exception) { }
        }

        public void KontrolEt2()
        {
            ///Bu kontrolü yazmazsak textboxtan son karakter silinmiyor!!!
            if (TxtBox.Text == "")
            {
                yeniTxt = "";
                TxtBox.Text = "";
            }
            try
            {

                foreach (char Karakter in TxtBox.Text)
                {

                    if (char.IsDigit(Karakter))
                    {
                        yeniTxt = TxtBox.Text;
                    }
                    else
                    {
                        yeniTxt = TxtBox.Text;
                        MessageBox.Show("Bu Alana Harf ve Karakter Girilemez!");
                        yeniTxt = yeniTxt.Remove(yeniTxt.Length - 1);

                    }
                }
            }
            catch (Exception) { }
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
