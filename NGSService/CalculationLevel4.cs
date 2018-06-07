using NGSData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NGSService
{
    public class CalculationLevel4
    {
        public CalculationLevel4(Level3 mutation)
        {
            IfNeedColorListPerson4(mutation);
        }
        public static Level3 searchU2S(List<Level3> mutations)
        {
            foreach (var item in mutations)
            {
                if (item.DyDis.Equals("U2S") && item.DyMut.Equals("C239INS4") && item.MutID.Equals("0156I"))
                    return item;
            }
            return null;

        }
        public static Level3 searchCLT(List<Level3> mutations)
        {

            foreach (var item in mutations)
            {
                if (item.DyDis.Equals("CLT") && item.DyMut.Equals("L1553VFS") && item.MutID.Equals("0208I"))
                    return item;
            }
            return null;
        }
        public void IfNeedColorListPerson4(Level3 mutation)
        {
            CalculationLevel1.SetColorInListPerson(mutation.ListPerson, 1);
        }
    }
}
