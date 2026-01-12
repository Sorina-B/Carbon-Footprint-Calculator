using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Proiect2
{
    [Table("CarbonHistory")]
   public class CarbonRecord
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime Date { get; set; }
        public double TotalTons { get; set; }
        public string Rating { get; set; }



    }
}
