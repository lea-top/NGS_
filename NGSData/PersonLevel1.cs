using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGSData
{
   public class PersonLevel1
    {
        public PersonLevel1() { }
        public String Name { get; set; }
        public String Genotype { get; set; }
        public String AlleleCoverage { get; set; }/*int*/
        public String TotalCoverage { get; set; }/* int*/
        public ColorLevel1 Color { get; set; }
    }
}
