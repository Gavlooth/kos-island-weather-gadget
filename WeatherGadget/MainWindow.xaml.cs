using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Drawing;
using System.Threading;
using System.ComponentModel;
namespace WeatherGadget
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        public static void SetOnDesktop(Window window)
        {
            IntPtr hWnd = new System.Windows.Interop.WindowInteropHelper(window).Handle;
            IntPtr hWndProgMan = FindWindow("Progman", "Program Manager");
            SetParent(hWnd, hWndProgMan);
        }

        private Gadget skata = new Gadget();
        private Dictionary<string, string> WeatherDictionary = new Dictionary<string, string>();
        double _origin = 90;
        static double zeroPoint = 135;
        BackgroundWorker InBackround;

        public void m_oWorker_DoWork(object sender, DoWorkEventArgs e)
        {   while (true)
            {

              //  Thread.Sleep(60000);
            InBackround.ReportProgress(0);
            Thread.Sleep(600000);
        }
        }



  

private void MyWindow_Loaded(object sender, RoutedEventArgs e)
{/*
    try
    {
        SetOnDesktop(this.main_window);
    }
    catch (Exception pipa) { MessageBox.Show(pipa.ToString()); }
   */ }




        public void Update_Weather()
        {
         
              
                

                lock (WeatherDictionary)
                {
                  

                    string pipa;
                    WeatherDictionary.TryGetValue("WindAngle", out pipa);
                    SetCompass(_origin, 90 - Convert.ToDouble(pipa));

                    WeatherDictionary = skata.DataScrap(); //retrive data from the weather station
                    foreach (KeyValuePair<string, string> item in WeatherDictionary)
                    {


                        var temp = ((Label)this.FindName(item.Key.ToString()));
                        if (temp == null)
                            ;//MessageBox.Show("roufikten"); Do nothing
                        else
                        {
                            lock (temp)
                            {
                                if (item.Key.ToString() == "Temperature")
                                    temp.Content = item.Value.ToString() + " °C";
                                else
                                    temp.Content = (item.Key.ToString() == "Wind") ? item.Value.ToString() : item.Key.ToString().Replace("_", " ") + ": " + item.Value.ToString();
                            }
                        }
                                             
                   }
         }
        
    }



        public void SetCompass (double _origin,  double _destination)
        {

            _origin += zeroPoint;
            _destination += zeroPoint;

            var dirs = ( _destination - _origin < 360 + _origin - _destination) ? new[] {_origin, _destination} : new[] {_origin, _destination-360};


             DoubleAnimation movePointer = new DoubleAnimation();
             movePointer.From = dirs[0];
             movePointer.To = dirs[1];

            movePointer.Duration = new Duration(TimeSpan.FromSeconds(3));
       
            RotateTransform rt = new RotateTransform();
            rt.CenterX =35 ;
            rt.CenterY = 35;  
            
                pointer.RenderTransform = rt;
                rt.BeginAnimation(RotateTransform.AngleProperty, movePointer);
            
            this._origin =   _destination - zeroPoint;

        }


        void m_oWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
         
          Update_Weather();

        }


        void m_oWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //If it was cancelled midway
            if (e.Cancelled)
            {
                MessageBox.Show ("Task Cancelled.");
            }
 
        }
        
        private void title_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public MainWindow()
        {
            try
            {

               

                #region GadgetTheme
                this.ShowInTaskbar = false; //gadget doesnt display a taskbar
                InitializeComponent();
                X_Button.Opacity = 0; //the button x and tool beggin invisible
                T_Button.Opacity = 0;
                #endregion

                #region initialize wind animation
                pointer.Opacity = 0;
                DoubleAnimation da = new DoubleAnimation();
                da.From = 0;
                da.To = zeroPoint+_origin;
                da.Duration = new Duration(TimeSpan.FromSeconds(0.1));
                //    da.RepeatBehavior = RepeatBehavior.Forever;
                RotateTransform rt = new RotateTransform();
                rt.CenterX = 35;
                rt.CenterY = 35;
                pointer.RenderTransform = rt;
                rt.BeginAnimation(RotateTransform.AngleProperty, da);
                pointer.Opacity = 1;
                #endregion
                                
                #region initialize Weather data
                WeatherDictionary = skata.DataScrap(); //retrive data from the weather station
               foreach (KeyValuePair<string,string> item in WeatherDictionary)
                {


                    var temp = ((Label)this.FindName(item.Key.ToString()));
                    if (temp == null)
                        ;//MessageBox.Show("roufikten"); Do nothing
                    else
                    {
                        if (item.Key.ToString() == "Temperature")
                            temp.Content = item.Value.ToString() + " °C";
                        else
                        temp.Content = (item.Key.ToString() == "Wind") ? item.Value.ToString() : item.Key.ToString().Replace("_", " ") + ": " + item.Value.ToString();
                    }
                    
                }


    /*          string pipa;
               WeatherDictionary.TryGetValue("WindAngle", out pipa);
               SetCompass(_origin, Convert.ToDouble(pipa));
    */
                #endregion

               InBackround = new BackgroundWorker();
               InBackround.DoWork += new DoWorkEventHandler(m_oWorker_DoWork);
               InBackround.ProgressChanged += new ProgressChangedEventHandler(m_oWorker_ProgressChanged);
               InBackround.RunWorkerCompleted += new RunWorkerCompletedEventHandler(m_oWorker_RunWorkerCompleted);
               InBackround.WorkerReportsProgress = true;
               InBackround.WorkerSupportsCancellation = false;

               InBackround.RunWorkerAsync();
           
               
            }
            catch (Exception e) { MessageBox.Show(e.ToString()); }
        }


        #region Button Style
        public void ReduceOpacity(object sender, EventArgs e)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = X_Button.Opacity;
            doubleAnimation.To = 0;
            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
            this.X_Button.BeginAnimation(OpacityProperty, doubleAnimation);
            this.T_Button.BeginAnimation(OpacityProperty, doubleAnimation);

        }


        public void EnhanceOpacity(object sender, EventArgs e)
        {
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = X_Button.Opacity;
            doubleAnimation.To = 1;
            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
            this.X_Button.BeginAnimation(OpacityProperty, doubleAnimation);
            this.T_Button.BeginAnimation(OpacityProperty, doubleAnimation);

        }

        #endregion

   

    }
     
 }

