using NGSData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGSService
{
    public class CalculationLevel5
    {
        public CalculationLevel5(Level5 mutation)
        {
            IfNeedColorListPerson(mutation);
        }

        public void IfNeedColorListPerson(Level5 mutation)
        {
            CalculationLevel1.SetColorInListPerson(mutation.ListPerson, 1);
        }

    }
}
