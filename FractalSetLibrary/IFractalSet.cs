using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;



namespace FractalSetLibrary
{
    public delegate uint[,] GetSetDelegate(uint x, uint y);

    public class IFractalSet
    {
        protected float radius;
        protected uint count;

        private float xmin;
        private float xmax;
        private float ymin;
        private float ymax;
        private uint xDefinition;
        private uint yDefinition;
        private object definitionsMonitor = new object();
        protected IFractalFormula fractalFormula;
        public GetSetDelegate getSet {protected set; get;}
     
        private IFractalSet(){}
        public IFractalSet(IFractalFormula formula,
            float xmin, float xmax, float ymin, float ymax, 
            uint xDefinition, uint yDefinition, uint N, float M)
        {
            if (M <= 0)
                throw new Exception("Wrong radius");
            this.xmin = xmin;
            this.ymin = ymin;
            this.xmax = xmax;
            this.ymax = ymax;
            this.xDefinition = xDefinition;
            this.yDefinition = yDefinition;
            count = N;
            radius = M;
            fractalFormula = formula;
        }

       public BitmapSource render(uint size)
       {
           BitmapSource image;
           lock (definitionsMonitor)
           {
               Debug.WriteLine(xDefinition);
               xDefinition = size;
               yDefinition = size;
               uint[,] arr = getSet(xDefinition, yDefinition);
               Debug.WriteLine(xDefinition);
               uint p = 0;
               byte[] colorArr = new byte[xDefinition * yDefinition * 3];
               Debug.WriteLine(xDefinition);
               for (int x = 0; x < xDefinition; x++)
                   for (int y = 0; y < yDefinition; y++)
                   {
                       if (arr[x, y] < count)
                           colorArr[p++] = (byte)(255 * (Math.Exp(arr[x, y] / (float)count) - 1));
                       else
                           colorArr[p++] = 0;
                       if (arr[x, y] < count)
                           colorArr[p++] = (byte)(255 * (Math.Exp(arr[x, y] / (float)count) - 1));
                       else
                           colorArr[p++] = 0;
                       if (arr[x, y] < count)
                           colorArr[p++] = (byte)((255 * (Math.Exp(arr[x, y] / (float)count)) - 1) + 100);
                       else
                           colorArr[p++] = 0;

                   }

               image = BitmapFrame.Create((int)xDefinition, (int)yDefinition,
                96, 96,
                PixelFormats.Rgb24, null, colorArr, 3 * (int)xDefinition);
           }
                   return image;
        }
    }
}
