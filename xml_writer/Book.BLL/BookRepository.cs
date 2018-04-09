using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xml_writer
{
    public class BookRepository
    {
        public IList<Book> listBooks;
         
        public BookRepository()
        {
            listBooks = new List<Book>();
        }

        public bool LoadFromFile(string fileName)
        {
            if (!File.Exists(fileName))
                return false;

            DataSet ds = new DataSet();
            ds.ReadXml(fileName); 

            foreach (DataRow item in ds.Tables["Employee"].Rows)
            {
                var book = new Book
                {
                    Name = item["Название"].ToString(),
                    Year = Convert.ToInt32(item["Год"].ToString()),
                    Genre = item["Жанр"].ToString(),
                    Author = item["Автор"].ToString()
                };
                listBooks.Add(book);
            }

            return true;
        }

        public void SaveToXml(string fileName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable(); 
            dt.TableName = "Employee"; 
            dt.Columns.Add("Название"); 
            dt.Columns.Add("Год");
            dt.Columns.Add("Жанр");
            dt.Columns.Add("Автор");
            ds.Tables.Add(dt); 

            foreach (Book book in listBooks) 
            {
                DataRow row = ds.Tables["Employee"].NewRow(); 
                row["Название"] = book.Name; 
                row["Год"] = book.Year; 
                row["Жанр"] = book.Genre; 
                row["Автор"] = book.Author; 
                ds.Tables["Employee"].Rows.Add(row); 
            }
            ds.WriteXml(fileName);
        }
    }
}
