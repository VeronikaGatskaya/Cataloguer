using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace xml_writer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public Form1(BookRepository bookRepository)
        {
            InitializeComponent();

            var result = bookRepository.LoadFromFile(@"Logs/Data.xml");

            if (result) 
            {
                foreach (Book book in bookRepository.listBooks)
                {
                    int n = dataGridView1.Rows.Add(); 
                    dataGridView1.Rows[n].Cells[0].Value = book.Name; 
                    dataGridView1.Rows[n].Cells[1].Value = book.Year; 
                    dataGridView1.Rows[n].Cells[2].Value = book.Genre;
                    dataGridView1.Rows[n].Cells[3].Value = book.Author;
                }
            }
            else
            {
                MessageBox.Show("XML-файл не найден.", "Ошибка.");
            }
        }


        private void button1_Click(object sender, EventArgs e) //add
        {

            if (textBox1.Text == "" | textBox2.Text == "") 
            {
                MessageBox.Show("Заполните все поля.", "Ошибка.");
            }
            else
            {
                int n = dataGridView1.Rows.Add();
                dataGridView1.Rows[n].Cells[0].Value = textBox1.Text;
                dataGridView1.Rows[n].Cells[1].Value = numericUpDown1.Value;
                dataGridView1.Rows[n].Cells[2].Value = comboBox1.Text;
                dataGridView1.Rows[n].Cells[3].Value = textBox2.Text;
            }
        }

        private void button4_Click(object sender, EventArgs e) //save data
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable(); 
                dt.TableName = "Book";
                dt.Columns.Add("Название"); 
                dt.Columns.Add("Год");
                dt.Columns.Add("Жанр");
                dt.Columns.Add("Автор");
                ds.Tables.Add(dt); 

                foreach (DataGridViewRow r in dataGridView1.Rows) 
                {
                    DataRow row = ds.Tables["Book"].NewRow(); 
                    row["Название"] = r.Cells[0].Value;  
                    row["Год"] = r.Cells[1].Value;
                    row["Жанр"] = r.Cells[2].Value; 
                    row["Автор"] = r.Cells[3].Value; 
                    ds.Tables["Book"].Rows.Add(row); 
                }
                ds.WriteXml(@"Logs/Data.xml");
                MessageBox.Show("XML-файл успешно сохранён.");
            }
            catch
            {
                MessageBox.Show("Невозможно сохранить.", "Ошибка.");
            }
        }

        private void button5_Click(object sender, EventArgs e) //загрузка файла XML в форму
        {
            if (dataGridView1.Rows.Count > 0) 
            {
                MessageBox.Show("Очистите поле перед загрузкой нового файла.", "Ошибка.");
            }
            else
            {
                if (File.Exists(@"Logs/Data.xml")) 
                {
                    DataSet ds = new DataSet(); // создаем новый пустой кэш данных
                    ds.ReadXml(@"Logs/Data.xml"); 

                    foreach (DataRow item in ds.Tables["Book"].Rows)
                    {
                        int n = dataGridView1.Rows.Add(); // добавляем новую сроку в dataGridView1
                        dataGridView1.Rows[n].Cells[0].Value = item["Название"]; // заносим в первый столбец созданной строки данные из первого столбца таблицы ds.
                        dataGridView1.Rows[n].Cells[1].Value = item["Год"]; 
                        dataGridView1.Rows[n].Cells[2].Value = item["Жанр"]; 
                        dataGridView1.Rows[n].Cells[3].Value = item["Автор"]; 
                    }
                }
                else
                {
                    MessageBox.Show("XML-файл не найден.", "Ошибка.");
                }
            }
        }


        private void dataGridView1_MouseClick(object sender, MouseEventArgs e) // выбор нужной строки для редактирования
        {
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
            int n = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[1].Value);
            numericUpDown1.Value = n;
            comboBox1.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
        }

        private void button2_Click(object sender, EventArgs e) //редактирование
        {

            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (textBox1.Text == "" | textBox2.Text == "")
                {
                    MessageBox.Show("Заполните все поля.", "Ошибка.");
                }
                else
                {
                    int n = dataGridView1.SelectedRows[0].Index;
                    dataGridView1.Rows[n].Cells[0].Value = textBox1.Text;
                    dataGridView1.Rows[n].Cells[1].Value = numericUpDown1.Value;
                    dataGridView1.Rows[n].Cells[2].Value = comboBox1.Text;
                    dataGridView1.Rows[n].Cells[3].Value = textBox2.Text;
                }
            }
            else
            {
                MessageBox.Show("Выберите строку для редактирования.", "Ошибка.");
            }
        }

        private void button3_Click(object sender, EventArgs e) //удалить выбранную строку
        {
            if(dataGridView1.SelectedRows.Count > 0)
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);
            }
            else
            {
                MessageBox.Show("Выберите строку для удаления.", "Ошибка.");
            }
        }

        private void button6_Click(object sender, EventArgs e) //очистить таблицу
        {
            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows.Clear();
            }
            else
            {
                MessageBox.Show("Таблица пустая.", "Ошибка.");
            }
        }

        private void button7_Click(object sender, EventArgs e) //поиск
        {
            for (int i = 0; i <= dataGridView1.Rows.Count - 1; i++)
                for (int j = 0; j <= dataGridView1.ColumnCount - 1; j++)
                    if (dataGridView1.Rows[i].Cells[j].Value != null && dataGridView1.Rows[i].Cells[j].Value.ToString() == textBox3.Text)
                        dataGridView1.Rows[i].Cells[j].Selected = true;
        }



        private void textBox3_KeyDown(object sender, KeyEventArgs e)   //поиск по нажатию Enter
        {
            if (e.KeyCode == Keys.Enter)
                e.Handled = true;
            {
                for (int i = 0; i <= dataGridView1.Rows.Count - 1; i++)
                    for (int j = 0; j <= dataGridView1.ColumnCount - 1; j++)
                        if (dataGridView1.Rows[i].Cells[j].Value != null && dataGridView1.Rows[i].Cells[j].Value.ToString() == textBox3.Text)
                            dataGridView1.Rows[i].Cells[j].Selected = true;
            }
        }

       

       
        
      
      
       

      
    }
}
