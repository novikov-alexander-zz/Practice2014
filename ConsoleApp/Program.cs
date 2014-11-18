using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FractalSetLibrary;
using System.Diagnostics;

namespace ConsoleApp
{
    class Program
    {
        static IFractalSet fractalSet;
        static Stopwatch timer = new Stopwatch();
/*
        static void serial()
        {
           
            fractalSet = new MandelbrotSet(-2, 2, -2, 2, 100, 100, 150, 2);
            timer.Restart();
            int[,] arr = fractalSet.getPASList_Serially();
            timer.Stop();

            for (int i = 0; i<100; i++)
                for (int j = 0; j<100; j++)
                    Console.WriteLine("x:{0} , y:{1}, state:{2}", -2 + 4.0/100*i, -2 + 4.0/100*j, arr[i,j]);

            Console.WriteLine("Затрачено времени: {0}мс", timer.ElapsedMilliseconds);
            Console.ReadLine();
            
        }

        static void parallel()
        {

            fractalSet = new MandelbrotSet(-2, 2, -2, 2, 100, 100, 150, 2);
            timer.Restart();
            int[,] arr = fractalSet.getPASList_Parallelly();
            timer.Stop();

            for (int i = 0; i<50; i++)
                for (int j = 0; j<50; j++)
                    Console.WriteLine("x:{0} , y:{1}, state:{2}", -2 + 4.0/100*i, -2 + 4.0/100*j, arr[i,j]);

            Console.WriteLine("Затрачено времени: {0}мс", timer.ElapsedMilliseconds);
            Console.ReadLine();
        }
  */      
        static void Main(string[] args)
        {
            string[] menu = { "Последовательно", "Параллельно"};
            ConsoleKeyInfo q;
            int v = 0;
            Console.ForegroundColor = ConsoleColor.Green;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Как Вы желаете вычислить множество Мандельброта?");
 
                for (int i = 0; i < menu.Length; i++)
                {
                    if (i == v) Console.Write("> ");
                    else Console.Write("  ");
                    Console.WriteLine(menu[i]);
                }

                q = Console.ReadKey();

                if (q.Key == ConsoleKey.UpArrow && v != 0) v--;
                if (q.Key == ConsoleKey.DownArrow && v != menu.Length - 1) v++;

                if (q.Key == ConsoleKey.Enter)
                    switch (v)
                    {
                        case 0:
    //                        serial();
                            break;
                        case 1:
      //                      parallel();
                                break;
                        default:
                            break;
                    }

            }
        }
    }
}
