using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace StreamingMusic.Models
{
    public class Table1
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime latestOpenApp { get; set; }
    }
}
