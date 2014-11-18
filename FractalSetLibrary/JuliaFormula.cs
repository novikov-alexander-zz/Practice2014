using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FractalSetLibrary
{
    public class JuliaFormula:IFractalFormula
    {
        public JuliaFormula()
        {
            F = ((enterPoint, lastPoint) => { return lastPoint; });
            G = ((enterPoint, lastPoint) => { lastPoint = lastPoint * lastPoint + new Complex(0.3, 0.6); return lastPoint; });
        }
    }
}
