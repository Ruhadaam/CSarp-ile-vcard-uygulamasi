using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace vcardProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public class CharacterConverter
        {
            public static string ConvertToEnglish(string input)
            {
                // Türkçe karakterleri normalize ederek İngilizce karakterlere dönüştür
                string normalizedString = input.Normalize(NormalizationForm.FormD);
                StringBuilder result = new StringBuilder();

                foreach (char c in normalizedString)
                {
                    // Türkçe karakterlerin İngilizce karakterlere dönüşümünü gerçekleştir
                    if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    {
                        result.Append(c);
                    }
                }

                // İngilizce karakterlere dönüştürülmüş metni geri döndür
                return result.ToString();
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        public void clear()
        {

            txtName.Text = "";
            txtSurname.Text = "";
            txtPhone.Text = "";
            txtBussines.Text = "";
            txtMail.Text = "";
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
        DataTable dt = new DataTable();
        private void Form1_Load(object sender, EventArgs e)
        {

            dt.Columns.Add("Ad");
            dt.Columns.Add("Soyad");
            dt.Columns.Add("Cep Telefonu");
            dt.Columns.Add("İş Telefonu");
            dt.Columns.Add("Mail Adresi");
            dataGridView1.RowHeadersVisible = false; 
            dataGridView1.DataSource = dt;
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            string name = txtName.Text;
            string surname = txtSurname.Text;
            string phone = txtPhone.Text;
            string bussines = txtBussines.Text;
            string mail = txtMail.Text;

            name = CharacterConverter.ConvertToEnglish(name);
            surname = CharacterConverter.ConvertToEnglish(surname);
            phone = CharacterConverter.ConvertToEnglish(phone);
            bussines = CharacterConverter.ConvertToEnglish(bussines);
            mail = CharacterConverter.ConvertToEnglish(mail);

            dt.Rows.Add(name, surname, phone, bussines, mail);

            clear();
            MessageBox.Show("Kişi başarıyla listeye eklendi.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "vCard Files|*.vcf";
            saveFileDialog.Title = "Save vCard Files";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName, false, Encoding.UTF8))
                    {
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            // Hücre değerlerini kontrol ederek null olup olmadıklarını kontrol edin.
                            if (row.Cells["Ad"].Value != null &&
                                row.Cells["Soyad"].Value != null &&
                                row.Cells["Cep Telefonu"].Value != null &&
                                row.Cells["İş Telefonu"].Value != null &&
                                row.Cells["Mail Adresi"].Value != null)
                            {
                                string name = row.Cells["Ad"].Value.ToString();
                                string surname = row.Cells["Soyad"].Value.ToString();
                                string phone = row.Cells["Cep Telefonu"].Value.ToString();
                                string bussines = row.Cells["İş Telefonu"].Value.ToString();
                                string mail = row.Cells["Mail Adresi"].Value.ToString();

                                sw.WriteLine("BEGIN:VCARD");
                                sw.WriteLine("VERSION:3.0");
                                sw.WriteLine($"N:{surname};{name}");
                                sw.WriteLine($"FN:{name} {surname}");
                                sw.WriteLine($"TEL;TYPE=CELL:{phone}");
                                sw.WriteLine($"TEL;TYPE=WORK:{bussines}");
                                sw.WriteLine($"EMAIL;TYPE=INTERNET:{mail}");
                                sw.WriteLine("END:VCARD");
                                sw.WriteLine();
                            }
                        }
                    }

                    MessageBox.Show("Listedeki kişiler Başarıyla Vcard'a dönüştürüldü.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;

            txtName.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
            txtSurname.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
            txtPhone.Text = dataGridView1.Rows[secilen].Cells[2].Value.ToString();
            txtBussines.Text = dataGridView1.Rows[secilen].Cells[3].Value.ToString();
            txtMail.Text = dataGridView1.Rows[secilen].Cells[4].Value.ToString();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            string nameToDelete = txtName.Text;
            string surnameToDelete = txtSurname.Text;
            string phoneToDelete = txtPhone.Text;
            string bussinesToDelete = txtBussines.Text;
            string mailToDelete = txtMail.Text;
            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                DataRow row = dt.Rows[i];

                // Verileri karşılaştır ve eşleşen satırı sil
                if (row["Ad"].ToString() == nameToDelete &&
                    row["Soyad"].ToString() == surnameToDelete &&
                    row["Cep Telefonu"].ToString() == phoneToDelete &&
                    row["İş Telefonu"].ToString() == bussinesToDelete &&
                    row["Mail Adresi"].ToString() == mailToDelete)
                {
                    dt.Rows.Remove(row);
                }
            }

            clear();
            MessageBox.Show("Kişi Başarıyla Silindi.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void updateButton_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click_2(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized; 
        }
        int mouseX, mouseY;

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Left = MousePosition.X - mouseX;
            this.Top  = MousePosition.Y - mouseY;
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            timer1.Enabled = false; 
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseX = MousePosition.X - this.Left;
            mouseY = MousePosition.Y - this.Top;
            timer1.Enabled = true;
        }
        
    }
}
