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

            foreach (DataRow item in ds.Tables["Book"].Rows)
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

    }
}
