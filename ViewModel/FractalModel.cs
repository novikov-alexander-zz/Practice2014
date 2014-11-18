using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using FractalSetLibrary;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using System.Threading;
using System.ComponentModel;
using System.Windows;
using MVVMApp;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Windows.Media;

namespace ViewModel
{
    
    public class FractalModel:INotifyPropertyChanged
    {
        /// <summary>Размер изображения, ожидающего перерисовку или -1, если в очереди запросов нет</summary>
        private int pendingSize = -1;

        /// <summary>Работает ли в данный момент поток рисования фрактала</summary>
        private bool isRendering = false;

        private BitmapSource _fractalImg;
        public BitmapSource fractalImg
        {
            get { return _fractalImg; } 
            set{_fractalImg = value; RaisePropertyChangedEvent("fractalImg");}
        }

        public DelegateCommand SizeCommand { get; private set; }
        public DelegateCommand SelectCommand { get; private set; }

        private uint _size=768;
        public uint size { get { return _size; } set { RaisePropertyChangedEvent("size"); _size = value; } }
        
        private bool _isMandelbrot = true;
        public bool isMandelbrot { get{return _isMandelbrot;} set { RaisePropertyChangedEvent("isMandelbrot"); _isMandelbrot = value; } }

        private bool _isJulia;
        public bool isJulia { get { return _isJulia; } set { RaisePropertyChangedEvent("isJulia"); _isJulia = value; } }

        private bool _isParallel;
        public bool isParallel { get { return _isParallel; } set { RaisePropertyChangedEvent("isParallel"); _isParallel = value; } }
        private bool _isSequintial = true;
        public bool isSequintial { get { return _isSequintial; } set { RaisePropertyChangedEvent("isSequintial"); _isSequintial = value; } }

        private XDocument database = new XDocument();
        private string dataPath;
        private const int def = 30;
        
        IFractalSet fractal = new SequintialSetSolver(new MandelbrotFormula(), -2, 2, -2, 2, 768, 768, 150, 2);
        public FractalModel()
        {
            var homepath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);
            dataPath = homepath + "\\database.xml";
            try { database = XDocument.Load(dataPath); }
            catch (FileNotFoundException ex)
            {
                database = new XDocument(new XElement("root"));
                database.Save(dataPath);
            }

            SelectCommand = new DelegateCommand(x =>
            {
                IFractalFormula formula = new MandelbrotFormula();
                if (_isJulia)
                    formula = new JuliaFormula();
                if (_isMandelbrot)
                    formula = new MandelbrotFormula();

                if (_isParallel)
                    fractal = new ParallelSetSolver(formula, -2, 2, -2, 2, size, size, 150, 2);
                if (_isSequintial)
                    fractal = new SequintialSetSolver(formula, -2, 2, -2, 2, size, size, 150, 2);

                    EnqueueRenderRequest((int)size);
            });
        }

        public void UpdateSize(int height, int width)
        {
            size = (uint)height;
            EnqueueRenderRequest(height);
        }

        private void BeginRendering(uint size)
        {
            isRendering = true;

            // TPL-based approach
            //var uis = TaskScheduler.FromCurrentSynchronizationContext();
            Task.Factory.StartNew(() =>
            {

                BitmapSource result = null;
                foreach (XElement el in database.Root.Elements())
                {
                    if (el.Attribute("size").Value == (size / def).ToString())
                    {
                        if ((isJulia && (el.Attribute("type").Value == "Julia"))
                            || (isMandelbrot && (el.Attribute("type").Value == "Mandelbrot")))
                        {
                            result = BitmapSource.Create((int)(size / def) * def, (int)(size / def) * def,
                                96, 96,
                                PixelFormats.Rgb24, null,
                                Convert.FromBase64String(el.Value),
                                3 * (int)(size / def) * def);
                        }
                    }
                        
                }

                if (result == null)
                {
                    result = fractal.render(size/30*30);
                    byte[] pixels = new byte[3 * (size / def) * def * (size / def) * def];
                    result.CopyPixels(pixels, 3 * (int)(size / def) * def, 0);
                    if (isJulia)
                        database.Root.Add(new XElement("grid", Convert.ToBase64String(pixels), new XAttribute("size", size / 30), new XAttribute("type", "Julia")));
                    if (isMandelbrot)
                        database.Root.Add(new XElement("grid", Convert.ToBase64String(pixels), new XAttribute("size", size / 30), new XAttribute("type", "Mandelbrot")));
                    database.Save(dataPath);
                }
                
                result.Freeze();
                return result;
            }).ContinueWith(t =>
            {
                fractalImg = t.Result;
                isRendering = false;
                if (pendingSize != -1)
                {
                    BeginRendering((uint)pendingSize);
                    pendingSize = -1;
                }
            });
        }

        private void EnqueueRenderRequest(int size)
        {
            if (isRendering)
                pendingSize = size;
            else
                BeginRendering((uint)size);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChangedEvent(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
