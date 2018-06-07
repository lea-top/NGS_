using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGSData
{

    public abstract class Level
    //interface Level
    {
        public String Id { get; set; }
        public String Ref { get; set; }
        public String Alt { get; set; }
        public String DyDis { get; set; }
        public String DyMut { get; set; }
        public String MutID { get; set; }
        public String ColorDyName { get; set; }
        public List<PersonLevel1> ListPerson { get; set; }
    }
}
