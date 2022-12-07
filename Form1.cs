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
using excel = Microsoft.Office.Interop.Excel;
using ClosedXML.Excel;
using Microsoft.Office.Interop.Excel;

namespace MACHINES
{
    public partial class Form1 : Form
    {

        SqlConnection baglanti;
        SqlCommand komut;
        SqlDataAdapter da;

        public object MakinetablosuTableAdapter { get; private set; }

        public Form1()
        {
            InitializeComponent();
        }

        void verilerigetir()
        {
            baglanti= new SqlConnection("Data Source=DESKTOP-7G5E3G9;Initial Catalog=machines;Integrated Security=True");
            baglanti.Open();
            da = new SqlDataAdapter("SELECT * FROM Makinetablosu", baglanti);
            System.Data.DataTable tablo =   new System.Data.DataTable();
            da.Fill(tablo); 
            dataGridView1.DataSource = tablo; 
            baglanti.Close();
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: Bu kod satırı 'machinesDataSet1.Makinetablosu' tablosuna veri yükler. Bunu gerektiği şekilde taşıyabilir, veya kaldırabilirsiniz.
            verilerigetir();
           
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            textBox2.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            textBox3.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            textBox4.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString(); 
            textBox5.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            dateTimePicker1.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            string sorgu = "INSERT INTO Makinetablosu(SiraNo,Firma,MakineAdi,MakineKodu,Adres,Tarih) VALUES(@SiraNo,@Firma,@MakineAdi,@MakineKodu,@Adres,@Tarih) ";
            komut = new SqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@SiraNo", textBox1.Text);
            komut.Parameters.AddWithValue("@Firma", textBox2.Text);
            komut.Parameters.AddWithValue("@MakineAdi", textBox3.Text);
            komut.Parameters.AddWithValue("@MakineKodu", textBox4.Text);
            komut.Parameters.AddWithValue("@Adres", textBox5.Text);

            komut.Parameters.AddWithValue("@dtarih", dateTimePicker1.Value);
   
            baglanti.Open();
            komut.ExecuteNonQuery();   
            baglanti.Close();
            verilerigetir();
           
          

        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            string sorgu = "DELETE  FROM Makinetablosu  WHERE no=@no";
            komut = new SqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@no", Convert.ToInt32(textBox1.Text));
            baglanti.Open() ;  
            komut.ExecuteNonQuery();  
            baglanti.Close();
            verilerigetir();
           

        }

        private void btnGüncelle_Click(object sender, EventArgs e)
        {
            string sorgu = "UPDATE Makinetablosu SET makineadi=@makineadi,firma=@firma,dtarih=@dtarih,adres=@adres,makinekod=@makinekod WHERE no=@no ";

            komut = new SqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@no", Convert.ToInt32(textBox1.Text));
            komut.Parameters.AddWithValue("@makineadi", textBox2.Text);
            komut.Parameters.AddWithValue("@firma", textBox3.Text);
            komut.Parameters.AddWithValue("@dtarih", dateTimePicker1.Value);
            komut.Parameters.AddWithValue("@adres", textBox4.Text);
            komut.Parameters.AddWithValue("@makinekod", textBox5.Text);
            baglanti.Open();
            komut.ExecuteNonQuery();
            baglanti.Close();
            verilerigetir();
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();

        }

       
  
        private void button2_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd= new SaveFileDialog () { Filter = "Excel Workbook|*.xlsx"})
            {

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                     
                    try
                    {
                        using (XLWorkbook wb = new XLWorkbook ())
                        {
                            
                            wb.Worksheets.Add(this.machinesDataSet1.Makinetablosu.CopyToDataTable(), "Makinetablosu");
                            wb.SaveAs(sfd.FileName);
                            
                        }

                        MessageBox.Show("Excel Dosyasına Kaydedildi.","Message",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message, "message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

      
    }
}
