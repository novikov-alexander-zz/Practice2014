using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FractalSetLibrary
{
    public class MandelbrotFormula : IFractalFormula
    {
        public MandelbrotFormula()
        {
            F = ((enterPoint, lastPoint) => { return lastPoint; });
            G = ((enterPoint, lastPoint) => { lastPoint = lastPoint * lastPoint + enterPoint; return lastPoint; });
        }
    }
}