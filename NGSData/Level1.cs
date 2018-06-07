using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGSData
{


    public class Level1:Level
    {
        public String Chrom { get; set; }
        public long Start { get; set; }
        public String End { get; set; }
        public String GenotypeChrom { get; set; }
        public long   GenotypePos { get; set; }
        public String GenotypeId { get; set; }
        public String GenotypeRef { get; set; }
        public String GenotypeAlt { get; set; }
        
    }

}