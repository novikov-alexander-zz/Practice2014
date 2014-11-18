using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace FractalSetLibrary
{
    public class SequintialSetSolver:IFractalSet
    {
        public SequintialSetSolver(IFractalFormula formula,
            float xmin, float xmax, float ymin, float ymax,
            uint xDefinition, uint yDefinition, uint N, float M) :
            base(formula, xmin, xmax, ymin, ymax,
            xDefinition, yDefinition, N, M){
                getSet = (delegate(uint xDef, uint yDef) {
                    uint[,] tempArr = new uint[xDef, yDef];
                    for (int i = 0; i < xDef; i++)
                        for (int j = 0; j < yDef; j++)
                        {
                            Complex enterPoint = new Complex(xmin + (xmax - xmin) / (xDef - 1) * i, ymin + (ymax - ymin) / (yDef - 1) * j);
                            Complex lastPoint;
                            tempArr[i, j] = N;

                            lastPoint = fractalFormula.F(enterPoint, enterPoint);
                            if (lastPoint.Magnitude > radius)
                            {
                                tempArr[i, j] = 0;
                                continue;
                            }

                            for (uint n = 1; n < count; n++)
                            {
                                lastPoint = fractalFormula.G(enterPoint, lastPoint);
                                if (lastPoint.Magnitude > radius)
                                {
                                    tempArr[i, j] = n;
                                    break;
                                }
                            }
                        }
                    return tempArr;
                });
        }
    }
}
