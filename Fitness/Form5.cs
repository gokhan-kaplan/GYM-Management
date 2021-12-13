using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace Fitness
{
    public partial class Form5 : Form
    {
        OpenFileDialog ofd = new OpenFileDialog();
        public Form5()
        {
            InitializeComponent();
        }
        DataTable tablo = new DataTable();
        DataTable tablo2 = new DataTable();
        SqlConnection con = new SqlConnection();
        SqlConnection con2 = new SqlConnection();
        SqlConnection con3 = new SqlConnection();
        DateTime now = DateTime.Now;
        DataSet dataSet = new DataSet();
        int pb_uye_id;

        private void Form5_Load(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
            dataGridView2.ClearSelection();
        }
        public void uyeDetay(int uye_id)
        {
            pb_uye_id = uye_id;
            con.ConnectionString = "Data Source=PC;Initial Catalog=fitness;Integrated Security=True";
            con2.ConnectionString = "Data Source=PC;Initial Catalog=fitness;Integrated Security=True";
            con3.ConnectionString = "Data Source=PC;Initial Catalog=fitness;Integrated Security=True";
            con.Open();

            SqlCommand komut = new SqlCommand("SELECT * FROM uye Where uye_id=@uye_id", con);
            komut.Parameters.AddWithValue("@uye_id", uye_id);
            SqlDataReader sdr = komut.ExecuteReader();

            while (sdr.Read())
            {
                ///uye bilgilerini kontrol nesnelerine çekme
                label1.Text = sdr["uye_id"].ToString();
                textBox1.Text = sdr["ad"].ToString();
                textBox2.Text = sdr["soyad"].ToString();
                comboBox1.Text = sdr["cinsiyet"].ToString();
                numericUpDown1.Text = sdr["kayit_uzunlugu"].ToString();
                label18.Text = sdr["fiyat_toplam"].ToString();
                label15.Text = sdr["fiyat_odenen"].ToString();
                label12.Text = sdr["fiyat_kalan"].ToString();
                dateTimePicker1.Text = sdr["baslangic_trh"].ToString();

                string btis_trh = String.Format("{0:d}", sdr["bitis_trh"]);
                dateTimePicker2.Text = btis_trh;
                textBox14.Text = sdr["calisma_prog"].ToString();

                //image cekme
                try
                {
                    Byte[] data = new Byte[0];
                    data = (Byte[])(sdr["fotograf"]);
                    MemoryStream mem = new MemoryStream(data);
                    pictureBox1.Image = Image.FromStream(mem);
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                catch (Exception ex) { }
                //uyelik bitis tarihini bulup, üyenin aktif ve ya pasif oldugu bulunuyor
                DateTime dt = Convert.ToDateTime(btis_trh);
                if (dt < now)
                {
                    label3.Text = "Pasif";
                    label3.ForeColor = Color.Red;
                    linkLabel1.Visible = true;
                }
                else if (dt > now)
                {
                    label3.Text = "Aktif";
                    label3.ForeColor = Color.Green;
                    linkLabel1.Visible = false;
                }
                //---------------vucut olculeri kontrol nesnelerinde gösterilecek<ilk deger>------------
                con2.Open();

                SqlCommand komut2 = new SqlCommand("SELECT kilo AS Kilo,omuz AS Omuz,gogus As Gögüs,bel AS Bel,sag_kol As 'Sag Kol',sol_kol As 'Sol Kol',sag_bacak As 'Sag Bacak',sol_bacak As 'Sol Bacak',basen As Basen FROM gelisim Where uye_id=@uye_id", con2);
                komut2.Parameters.AddWithValue("@uye_id", uye_id);
                SqlDataAdapter adtr = new SqlDataAdapter(komut2);
                tablo.Clear();
                adtr.Fill(tablo);
                dataGridView1.DataSource = tablo;
                adtr.Dispose();

                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.AllowUserToAddRows = false;

                //---------------------vucut olculeri kontrol nesnelerinde gösterilecek<son deger>------------

                con3.Open();
                SqlCommand komut3 = new SqlCommand("SELECT kilo_son AS Kilo,omuz_son AS Omuz,gogus_son As Gögüs,bel_son AS Bel,sag_kol_son As 'Sag Kol',sol_kol_son As 'Sol Kol',sag_bacak_son As 'Sag Bacak',sol_bacak_son As 'Sol Bacak',basen_son As Basen FROM gelisim Where uye_id=@uye_id", con3);
                komut3.Parameters.AddWithValue("@uye_id", uye_id);
                SqlDataAdapter adtr2 = new SqlDataAdapter(komut3);
                tablo2.Clear();
                adtr2.Fill(tablo2);
                dataGridView2.DataSource = tablo2;
                adtr2.Dispose();
                dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView2.AllowUserToAddRows = false;
            }

            /////////////////////////////////////////////
            con.Close();
            con2.Close();
            con3.Close();
        }
        int update_sayac = 0;
        // ---------------GÜNCELLEME----------------------------
        private void button2_Click(object sender, EventArgs e)
        {
            update_sayac++;
            if (update_sayac == 1)
            {
                button3.Enabled = true;
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                comboBox1.Enabled = true;
                numericUpDown1.Enabled = true;
                dateTimePicker1.Enabled = true;                
                textBox14.Enabled = true;
                dataGridView1.Enabled = true;
                dataGridView2.Enabled = true;
                button2.Text = "Değişiklikleri Kaydet";
            }
            //Güncelleme
            else if (update_sayac == 2)
            {
                try
                {
                    con.ConnectionString = "Data Source=PC;Initial Catalog=fitness;Integrated Security=True";
                    con.Open();

                    int kilo, omuz, gogus, bel, sag_kol, sol_kol, sag_bacak, sol_bacak, basen, kilo_son, omuz_son, gogus_son, bel_son, sag_kol_son, sol_kol_son, sag_bacak_son, sol_bacak_son, basen_son;

                    //fiyat kalan => toplam fiyattan, ödenen tutar çıkartılıp hesaplanacak
                    int kalan_fiyat = Int32.Parse(label18.Text) - Int32.Parse(label15.Text);

                    //uyelik bitis hesaplama
                    DateTimePicker dtp = new DateTimePicker();
                    int kalan_tarih = Int32.Parse(numericUpDown1.Value.ToString());
                    dtp.Value = dateTimePicker1.Value.AddMonths(kalan_tarih);

                    //------------genel bilgiler-----------------------
                    SqlCommand komutupdate3 = new SqlCommand("UPDATE uye SET ad=@ad, soyad=@soyad, cinsiyet=@cinsiyet, kayit_uzunlugu=@kayit_uzunlugu, baslangic_trh=@baslangic_trh, bitis_trh=@bitis_trh, calisma_prog=@calisma_prog WHERE uye_id=@uye_id", con);
                    komutupdate3.Parameters.AddWithValue("@uye_id", pb_uye_id);
                    komutupdate3.Parameters.AddWithValue("@ad", textBox1.Text);
                    komutupdate3.Parameters.AddWithValue("@soyad", textBox2.Text);
                    komutupdate3.Parameters.AddWithValue("@cinsiyet", comboBox1.Text);
                    komutupdate3.Parameters.AddWithValue("@kayit_uzunlugu", numericUpDown1.Value.ToString());
                    komutupdate3.Parameters.AddWithValue("@baslangic_trh", dateTimePicker1.Value.Date);
                    //uyelik bitis
                    komutupdate3.Parameters.AddWithValue("@bitis_trh", dtp.Value.Date);
                    //program
                    komutupdate3.Parameters.AddWithValue("@calisma_prog", textBox14.Text);
                    komutupdate3.ExecuteNonQuery();
                    //---------------VÜCUT OLCULERİNİ GUNCELLE------
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        //---------ilk deger---------
                        kilo = Convert.ToInt32(dataGridView1.Rows[i].Cells["Kilo"].Value);
                        omuz = Convert.ToInt32(dataGridView1.Rows[i].Cells["Omuz"].Value);
                        gogus = Convert.ToInt32(dataGridView1.Rows[i].Cells["Gögüs"].Value);
                        bel = Convert.ToInt32(dataGridView1.Rows[i].Cells["Bel"].Value);
                        sag_kol = Convert.ToInt32(dataGridView1.Rows[i].Cells["Sag Kol"].Value);
                        sol_kol = Convert.ToInt32(dataGridView1.Rows[i].Cells["Sol Kol"].Value);
                        sag_bacak = Convert.ToInt32(dataGridView1.Rows[i].Cells["Sag Bacak"].Value);
                        sol_bacak = Convert.ToInt32(dataGridView1.Rows[i].Cells["Sol Bacak"].Value);
                        basen = Convert.ToInt32(dataGridView1.Rows[i].Cells["Basen"].Value);

                        SqlCommand komutupdate2 = new SqlCommand("UPDATE gelisim SET kilo=@kilo, omuz=@omuz, gogus=@gogus, bel=@bel, sag_kol=@sag_kol, sol_kol=@sol_kol, sag_bacak=@sag_bacak, sol_bacak=@sol_bacak, basen=@basen WHERE uye_id=@uye_id", con);

                        komutupdate2.Parameters.AddWithValue("@uye_id", pb_uye_id);
                        komutupdate2.Parameters.AddWithValue("@kilo", kilo);
                        komutupdate2.Parameters.AddWithValue("@omuz", omuz);
                        komutupdate2.Parameters.AddWithValue("@gogus", gogus);
                        komutupdate2.Parameters.AddWithValue("@bel", bel);
                        komutupdate2.Parameters.AddWithValue("@sag_kol", sag_kol);
                        komutupdate2.Parameters.AddWithValue("@sol_kol", sol_kol);
                        komutupdate2.Parameters.AddWithValue("@sag_bacak", sag_bacak);
                        komutupdate2.Parameters.AddWithValue("@sol_bacak", sol_bacak);
                        komutupdate2.Parameters.AddWithValue("@basen", basen);
                        komutupdate2.ExecuteNonQuery();

                        //----------------son deger-------------------
                        kilo_son = Convert.ToInt32(dataGridView2.Rows[i].Cells["Kilo"].Value);
                        omuz_son = Convert.ToInt32(dataGridView2.Rows[i].Cells["Omuz"].Value);
                        gogus_son = Convert.ToInt32(dataGridView2.Rows[i].Cells["Gögüs"].Value);
                        bel_son = Convert.ToInt32(dataGridView2.Rows[i].Cells["Bel"].Value);
                        sag_kol_son = Convert.ToInt32(dataGridView2.Rows[i].Cells["Sag Kol"].Value);
                        sol_kol_son = Convert.ToInt32(dataGridView2.Rows[i].Cells["Sol Kol"].Value);
                        sag_bacak_son = Convert.ToInt32(dataGridView2.Rows[i].Cells["Sag Bacak"].Value);
                        sol_bacak_son = Convert.ToInt32(dataGridView2.Rows[i].Cells["Sol Bacak"].Value);
                        basen_son = Convert.ToInt32(dataGridView2.Rows[i].Cells["Basen"].Value);

                        SqlCommand komutupdate = new SqlCommand("UPDATE gelisim SET kilo_son=@kilo_son, omuz_son=@omuz_son, gogus_son=@gogus_son, bel_son=@bel_son, sag_kol_son=@sag_kol_son, sol_kol_son=@sol_kol_son, sag_bacak_son=@sag_bacak_son, sol_bacak_son=@sol_bacak_son, basen_son=@basen_son WHERE uye_id=@uye_id", con);

                        komutupdate.Parameters.AddWithValue("@uye_id", pb_uye_id);
                        komutupdate.Parameters.AddWithValue("@kilo_son", kilo_son);
                        komutupdate.Parameters.AddWithValue("@omuz_son", omuz_son);
                        komutupdate.Parameters.AddWithValue("@gogus_son", gogus_son);
                        komutupdate.Parameters.AddWithValue("@bel_son", bel_son);
                        komutupdate.Parameters.AddWithValue("@sag_kol_son", sag_kol_son);
                        komutupdate.Parameters.AddWithValue("@sol_kol_son", sol_kol_son);
                        komutupdate.Parameters.AddWithValue("@sag_bacak_son", sag_bacak_son);
                        komutupdate.Parameters.AddWithValue("@sol_bacak_son", sol_bacak_son);
                        komutupdate.Parameters.AddWithValue("@basen_son", basen_son);
                        komutupdate.ExecuteNonQuery();
                    }
                }
                catch (Exception) { }


                if (textBox15.Text != "")
                {
                    string asd = label1.Text;
                    byte[] imageData = ReadImageFile(textBox15.Text);
                    SqlCommand komutfoto = new SqlCommand("UPDATE uye SET fotograf=@ft WHERE uye_id LIKE " + asd + "", con);
                    komutfoto.Parameters.Add(new SqlParameter("@ft", imageData));
                    komutfoto.ExecuteNonQuery();
                }
                con.Close();
                update_sayac = 0;
                button3.Enabled = false;
                textBox1.Enabled = false;
                textBox2.Enabled = false;
                comboBox1.Enabled = false;
                numericUpDown1.Enabled = false;
                dateTimePicker1.Enabled = false;
                dateTimePicker2.Enabled = false;
                textBox14.Enabled = false;
                dataGridView1.Enabled = false;
                dataGridView2.Enabled = false;
                button2.Text = "Bilgileri Güncelle";
                uyeDetay(pb_uye_id);
            }
        }
        //detaylı görünümde üye silme
        private void button1_Click(object sender, EventArgs e)
        {
            con.ConnectionString = "Data Source=PC;Initial Catalog=fitness;Integrated Security=True";
            con.Open();

            DialogResult result = MessageBox.Show("Üye Kaydını Silmek İstediğinden Eminmisin?", "Kayıt Silme", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                try
                {
                    SqlCommand komutdelete = new SqlCommand("DELETE FROM gelisim WHERE uye_id=@uye_id", con);
                    komutdelete.Parameters.AddWithValue("@uye_id", pb_uye_id);
                    komutdelete.ExecuteNonQuery();
                    SqlCommand komutdelete2 = new SqlCommand("DELETE FROM uye WHERE uye_id=@uye_id", con);
                    komutdelete2.Parameters.AddWithValue("@uye_id", pb_uye_id);
                    komutdelete2.ExecuteNonQuery();
                    SqlCommand komutdelete3 = new SqlCommand("DELETE FROM Borc WHERE uye_id=@uye_id", con);
                    komutdelete3.Parameters.AddWithValue("@uye_id", pb_uye_id);
                    komutdelete3.ExecuteNonQuery();
                    MessageBox.Show("Üye Silindi!");

                    (System.Windows.Forms.Application.OpenForms["Form1"] as Form1).uyeGoster();
                    this.Close();
                }
                catch (Exception) { }
            }
            con.Close();
        }
        private void textBox12_TextChanged(object sender, EventArgs e)
        {
           
        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            int trh = Int32.Parse(numericUpDown1.Value.ToString());
            dateTimePicker2.Value = dateTimePicker1.Value.Date.AddMonths(trh);
        }

        private void button3_Click(object sender, EventArgs e)
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
    }
}
