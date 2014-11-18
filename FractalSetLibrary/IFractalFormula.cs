using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace FractalSetLibrary
{
    public delegate Complex NextPointDelegate(Complex enterPoint, Complex lastPoint);

    public class IFractalFormula
    {
        public NextPointDelegate F{ protected set; get; }
        public NextPointDelegate G { protected set; get; }
    }
}
