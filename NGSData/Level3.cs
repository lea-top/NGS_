using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGSData
{
   public class Level3:Level
    {
        public String Chrom { get; set; }
        public long Start { get; set; }
        public String End { get; set; }
        public String InsertionsChrom { get; set; }
        public long   InsertionsPos { get; set; }
        public String InsertionsId { get; set; }
        public String InsertionsRef { get; set; }
        public String InsertionsAlt { get; set; }

    }
}
