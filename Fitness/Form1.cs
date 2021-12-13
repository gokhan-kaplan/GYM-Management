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
    public partial class Form1 : Form
    {
        DataTable tablo = new DataTable();
        DataTable tablo2 = new DataTable();
        DataTable tablo3 = new DataTable();
        DataTable tablo44 = new DataTable();
        SqlConnection con = new SqlConnection();
        SqlConnection con2 = new SqlConnection();
        SqlConnection con3 = new SqlConnection();
        SqlConnection con44 = new SqlConnection();
        DateTime rightNow = System.DateTime.Now;


        public Form1()
        {
            InitializeComponent();
            con.ConnectionString = "Data Source=PC;Initial Catalog=fitness;Integrated Security=True";
            con2.ConnectionString = "Data Source=PC;Initial Catalog=fitness;Integrated Security=True";
            con3.ConnectionString = "Data Source=PC;Initial Catalog=fitness;Integrated Security=True";
            con44.ConnectionString = "Data Source=PC;Initial Catalog=fitness;Integrated Security=True";
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            pnlUye.Visible = true;
            button1.BackColor = Color.MediumAquamarine;
            button7.BackColor = Color.Gainsboro;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView3.AllowUserToAddRows = false;
            uyeGoster();


        }
        public void uyeGoster()
        {
            con.Open();
            tablo.Clear();

            SqlDataAdapter adtr = new SqlDataAdapter("SELECT uye_id AS ' ',ad AS AD,soyad AS SOYAD,cinsiyet AS CİNSİYET,baslangic_trh AS 'BAŞLANGIÇ TARİHİ',kayit_uzunlugu AS 'KAYIT SÜRESİ',bitis_trh AS 'BİTİŞ TARİHİ' FROM uye", con);
            adtr.Fill(tablo);
            dataGridView1.DataSource = tablo;
            adtr.Dispose();
            con.Close();

            ////////bir hafta kalanlar
            //DateTime bslng = DateTime.Now;
            //DateTime bitis = bslng.AddDays(7);

            //con.Open();
            //SqlCommand cmdd = new SqlCommand("SELECT COUNT(*) FROM uye WHERE bitis_trh BETWEEN @asd AND @qwe", con);
            //cmdd.Parameters.AddWithValue("@asd", bslng);
            //cmdd.Parameters.AddWithValue("@qwe", bitis);
            //Int32 count = (Int32)cmdd.ExecuteScalar();
            //linkLabel1.Text = count.ToString();
            //con.Close();

            /////bitenler
            //con.Open();
            //SqlCommand cmdd2 = new SqlCommand("SELECT COUNT(*) FROM uye WHERE bitis_trh <= @asd", con);
            //cmdd2.Parameters.AddWithValue("@asd", bslng);
            //Int32 count2 = (Int32)cmdd2.ExecuteScalar();
            //linkLabel2.Text = count2.ToString();
            //con.Close();

            durumBul();
            this.dataGridView1.Columns[0].Width = 60;
            this.dataGridView1.Columns[3].Width = 80;
            this.dataGridView1.Columns[5].Width = 90;

        }
        public void durumBul()
        {
            try
            {
                dataGridView1.Columns.Remove("durum");
            }
            catch (Exception e) { }
            dataGridView1.Columns.Add("durum", "KAYIT DURUMU");
            ////-----uyelik durumu<aktif-pasif bulma>
            DateTime c;
            
            MessageBox.Show(dataGridView1.Columns[6].HeaderText);
            int bitis = Convert.ToInt32(dataGridView1.ColumnCount);
            bitis = bitis - 2;
            //try
            //{
            dataGridView1.Columns[6].ValueType = typeof(DateTime);
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    //c = DateTime.Parse(dataGridView1.Rows[i].Cells[bitis].Value.ToString());
                    
                    c = DateTime.Parse(dataGridView1.Rows[i].Cells[6].Value.ToString());
                    if (c > rightNow)
                    {
                        dataGridView1.Rows[i].Cells["durum"].Value = "AKTİF";
                    }
                    else
                    {
                        dataGridView1.Rows[i].Cells["durum"].Value = "PASİF";
                    }

                }
            //}
            //catch (Exception ex) { }
        }
        //-----------arama------------// 
        public void arama(string asd)
        {
            con44.Open();
            tablo44.Clear();
            SqlDataAdapter adtr44 = new SqlDataAdapter("SELECT uye_id AS ' ',ad AS AD,soyad AS SOYAD,cinsiyet AS CİNSİYET,baslangic_trh AS 'BAŞLANGIÇ TARİHİ',kayit_uzunlugu AS 'KAYIT SÜRESİ',bitis_trh AS 'BİTİŞ TARİHİ' FROM uye WHERE ad LIKE '%" + asd + "%'", con44);
            adtr44.Fill(tablo44);
            dataGridView1.DataSource = tablo44;
            adtr44.Dispose();
            con44.Close();

            //-----uyelik durumu<aktif-pasif bulma>
            durumBul();
            this.dataGridView1.Columns[0].Width = 60;
            this.dataGridView1.Columns[3].Width = 80;
            this.dataGridView1.Columns[5].Width = 90;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pnlUye.Visible = true;
            pnlBorc.Visible = false;
            pnlMuhasebe.Visible = false;
            label6.Visible = true;
            label12.Visible = true;
            linkLabel1.Visible = true;
            linkLabel2.Visible = true;

            button1.BackColor = Color.MediumAquamarine;
            button7.BackColor = Color.Gainsboro;
            button12.BackColor = Color.Gainsboro;
            uyeGoster();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            pnlUye.Visible = false;
            pnlBorc.Visible = false;
            pnlMuhasebe.Visible = false;

            button1.BackColor = Color.Gainsboro;
            button7.BackColor = Color.Gainsboro;
            button12.BackColor = Color.Gainsboro;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form2 uyeKayit = new Form2();
            uyeKayit.ShowDialog();
        }
        private void button11_Click(object sender, EventArgs e)
        {
            Form3 arama = new Form3();
            arama.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            pnlBorc.Visible = true;
            pnlUye.Visible = false;
            pnlMuhasebe.Visible = false;
            label6.Visible = false;
            label12.Visible = false;
            linkLabel1.Visible = false;
            linkLabel2.Visible = false;

            button7.BackColor = Color.MediumAquamarine;
            button1.BackColor = Color.Gainsboro;
            button12.BackColor = Color.Gainsboro;

            DataTable table_borc = new DataTable();
            table_borc.Clear();
            con3.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("SELECT uye_id AS ' ',ad AS AD,soyad AS SOYAD,fiyat_toplam AS 'TOPLAM TUTAR',fiyat_odenen AS 'ÖDENEN TUTAR',fiyat_kalan AS 'KALAN TUTAR' FROM uye WHERE fiyat_kalan > 0", con3);
            adtr.Fill(table_borc);
            dataGridView4.DataSource = table_borc;
            adtr.Dispose();
            con3.Close();

        }
        private void button12_Click(object sender, EventArgs e)
        {
            pnlMuhasebe.Visible = true;
            pnlUye.Visible = false;
            pnlBorc.Visible = false;

            button12.BackColor = Color.MediumAquamarine;
            button1.BackColor = Color.Gainsboro;
            button7.BackColor = Color.Gainsboro;

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //dtgridview nesnesinin başlık kısmı seçimini engelleme.
                if (dataGridView1.Rows[e.RowIndex].Index != -1)
                {
                    Form5 frm5 = new Form5();
                    if (frm5.Visible == false)
                    {
                        Form5 frm5_2 = new Form5();
                        int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;
                        DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];
                        string a = Convert.ToString(selectedRow.Cells[" "].Value);
                        int b = Int32.Parse(a);
                        frm5_2.uyeDetay(b);
                        frm5_2.ShowDialog();
                    }
                }
            }
            catch (Exception) { }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            con.Open();

            DialogResult result = MessageBox.Show("Üye Kaydını Silmek İstediğinden Eminmisin?", "Kayıt Silme", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                try
                {
                    int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;
                    DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];
                    int c = Int32.Parse(selectedRow.Cells[" "].Value.ToString());

                    SqlCommand komutdelete = new SqlCommand("DELETE FROM gelisim WHERE uye_id=@uye_id", con);
                    komutdelete.Parameters.AddWithValue("@uye_id", c);
                    komutdelete.ExecuteNonQuery();
                    SqlCommand komutdelete2 = new SqlCommand("DELETE FROM uye WHERE uye_id=@uye_id", con);
                    komutdelete2.Parameters.AddWithValue("@uye_id", c);
                    komutdelete2.ExecuteNonQuery();
                    MessageBox.Show("Üye Silindi!");
                }
                catch (Exception) { }
            }
            con.Close();
            uyeGoster();
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            con.ConnectionString = "Data Source=PC;Initial Catalog=fitness;Integrated Security=True";
            con.Open();

            DialogResult result = MessageBox.Show("Muhasebe Bilgisini Silmek İstediğinden Eminmisin?", "Girdi Silme", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                try
                {
                    int selectedrowindex = dataGridView3.SelectedCells[0].RowIndex;
                    DataGridViewRow selectedRow = dataGridView3.Rows[selectedrowindex];
                    int c = Int32.Parse(selectedRow.Cells[" "].Value.ToString());

                    SqlCommand komutdelete2 = new SqlCommand("DELETE FROM muhasebe WHERE kayit_id=@kayit_id", con);
                    komutdelete2.Parameters.AddWithValue("@kayit_id", c);
                    komutdelete2.ExecuteNonQuery();
                    MessageBox.Show("Kayıt Silindi!");
                }
                catch (Exception) { }
                con.Close();
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            //new form
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            con.Open();
            pnlUye.Visible = true;
            pnlBorc.Visible = false;
            pnlMuhasebe.Visible = false;

            button1.BackColor = Color.MediumAquamarine;
            button7.BackColor = Color.Gainsboro;
            button12.BackColor = Color.Gainsboro;
            tablo.Clear();
            //string today = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime bslng = DateTime.Now;
            DateTime bitis = bslng.AddDays(7);

            SqlDataAdapter adtr = new SqlDataAdapter("SELECT uye_id AS ' ',ad AS AD,soyad AS SOYAD,cinsiyet AS CİNSİYET,baslangic_trh AS 'BAŞLANGIÇ TARİHİ',kayit_uzunlugu AS 'KAYIT SÜRESİ',bitis_trh AS 'BİTİŞ TARİHİ' FROM uye WHERE bitis_trh BETWEEN @asd AND @qwe", con);
            adtr.SelectCommand.Parameters.AddWithValue("@asd", bslng);
            adtr.SelectCommand.Parameters.AddWithValue("@qwe", bitis);

            adtr.Fill(tablo);
            dataGridView1.DataSource = tablo;
            adtr.Dispose();
            con.Close();

            ////-----uyelik durumu<aktif-pasif bulma>
            durumBul();

            this.dataGridView1.Columns[0].Width = 60;
            this.dataGridView1.Columns[3].Width = 80;
            this.dataGridView1.Columns[5].Width = 90;
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            con.Open();
            pnlUye.Visible = true;
            pnlBorc.Visible = false;
            pnlMuhasebe.Visible = false;

            button1.BackColor = Color.MediumAquamarine;
            button7.BackColor = Color.Gainsboro;
            button12.BackColor = Color.Gainsboro;
            tablo.Clear();
            //string today = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime bslng = DateTime.Now;
            DateTime bitis = bslng.AddDays(7);

            SqlDataAdapter adtr = new SqlDataAdapter("SELECT uye_id AS ' ',ad AS AD,soyad AS SOYAD,cinsiyet AS CİNSİYET,baslangic_trh AS 'BAŞLANGIÇ TARİHİ',kayit_uzunlugu AS 'KAYIT SÜRESİ',bitis_trh AS 'BİTİŞ TARİHİ' FROM uye WHERE bitis_trh <= @asd", con);
            adtr.SelectCommand.Parameters.AddWithValue("@asd", bslng);

            adtr.Fill(tablo);
            dataGridView1.DataSource = tablo;
            adtr.Dispose();
            con.Close();

            ////-----uyelik durumu<aktif-pasif bulma>
            durumBul();
            this.dataGridView1.Columns[0].Width = 60;
            this.dataGridView1.Columns[3].Width = 80;
            this.dataGridView1.Columns[5].Width = 90;
        }

        private void pnlUye_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            durumBul();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            con.Open();

            DialogResult result = MessageBox.Show("Üye Kaydını Silmek İstediğinden Eminmisin?", "Kayıt Silme", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                try
                {
                    int selectedrowindex = dataGridView3.SelectedCells[0].RowIndex;
                    DataGridViewRow selectedRow = dataGridView3.Rows[selectedrowindex];
                    int c = Int32.Parse(selectedRow.Cells[" "].Value.ToString());

                    SqlCommand komutdelete = new SqlCommand("DELETE FROM muhasebe WHERE kayit_id=@id", con);
                    komutdelete.Parameters.AddWithValue("@id", c);
                    komutdelete.ExecuteNonQuery();
                    MessageBox.Show("Kayıt Silindi!");
                }
                catch (Exception) { }
            }
            con.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            uyeGoster();
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            con.Open();
            pnlUye.Visible = true;
            pnlBorc.Visible = false;
            pnlMuhasebe.Visible = false;

            button1.BackColor = Color.MediumAquamarine;
            button7.BackColor = Color.Gainsboro;
            button12.BackColor = Color.Gainsboro;
            tablo.Clear();

            DateTime bslng = DateTime.Now;

            SqlDataAdapter adtr = new SqlDataAdapter("SELECT uye_id AS ' ',ad AS AD,soyad AS SOYAD,cinsiyet AS CİNSİYET,baslangic_trh AS 'BAŞLANGIÇ TARİHİ',kayit_uzunlugu AS 'KAYIT SÜRESİ',bitis_trh AS 'BİTİŞ TARİHİ' FROM uye WHERE bitis_trh >= @asd", con);
            adtr.SelectCommand.Parameters.AddWithValue("@asd", bslng);

            adtr.Fill(tablo);
            dataGridView1.DataSource = tablo;
            adtr.Dispose();
            con.Close();

            ////-----uyelik durumu<aktif-pasif bulma>
            durumBul();
            this.dataGridView1.Columns[0].Width = 60;
            this.dataGridView1.Columns[3].Width = 80;
            this.dataGridView1.Columns[5].Width = 90;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            con.Open();
            pnlUye.Visible = true;
            pnlBorc.Visible = false;
            pnlMuhasebe.Visible = false;

            button1.BackColor = Color.MediumAquamarine;
            button7.BackColor = Color.Gainsboro;
            button12.BackColor = Color.Gainsboro;
            tablo.Clear();

            DateTime bslng = DateTime.Now;

            SqlDataAdapter adtr = new SqlDataAdapter("SELECT uye_id AS ' ',ad AS AD,soyad AS SOYAD,cinsiyet AS CİNSİYET,baslangic_trh AS 'BAŞLANGIÇ TARİHİ',kayit_uzunlugu AS 'KAYIT SÜRESİ',bitis_trh AS 'BİTİŞ TARİHİ' FROM uye WHERE bitis_trh <= @asd", con);
            adtr.SelectCommand.Parameters.AddWithValue("@asd", bslng);

            adtr.Fill(tablo);
            dataGridView1.DataSource = tablo;
            adtr.Dispose();
            con.Close();

            ////-----uyelik durumu<aktif-pasif bulma>
            durumBul();
            this.dataGridView1.Columns[0].Width = 60;
            this.dataGridView1.Columns[3].Width = 80;
            this.dataGridView1.Columns[5].Width = 90;
        }

    }
}
