using NGSData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGSService
{
    public class CalculationLevel3
    {
        public CalculationLevel3(Level3 mutation)
        {
            IfNeedColorListPerson(mutation);
        }
        public static Level3 Search(List<Level3> mutations, string DyDis, string DyMut,string MutID )
        {
            Level3 result = mutations.First(s => s.DyDis.Equals(DyDis) && s.DyMut.Equals(DyMut) && s.MutID.Equals(MutID));
            return result;
        }
        public void IfNeedColorListPerson(Level3 mutation)
        {
            CalculationLevel1.SetColorInListPerson(mutation.ListPerson, 1);
        }
    }
}
