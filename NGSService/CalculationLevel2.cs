using NGSData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGSService
{
    public class CalculationLevel2
    {  
        public CalculationLevel2(Level2 mutation)
        {
            IfNeedColorListPerson(mutation);
        }
        public void IfNeedColorListPerson(Level2 mutation)
        {
            CalculationLevel1.SetColorInListPerson(mutation.ListPerson, 1);
        }
    }
}
