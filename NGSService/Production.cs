using NGSData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGSService
{
   public class Production 
    {
        public List<Level1> ListLevel1 { get; set; }
        public List<Level2> ListLevel2 { get; set; }
        public List<Level3> ListLevel3 { get; set; }
        public List<Level3> ListLevel4 { get; set; }
        public List<Level5> ListLevel5 { get; set; }
        public List<Level1> ListLevel6 { get; set; }
        public List<Level0> ListLevel8 { get; set; }
        public List<Level9> ListLevel9 { get; set; }
        public List<Level3> ListMpileupInsertions { get; set; }
        public Run RunPlates { get; set; }
    }
}
