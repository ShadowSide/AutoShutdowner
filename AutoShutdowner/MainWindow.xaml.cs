using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace AutoShutdowner
{
    public partial class MainWindow : Window
    {
        public AutoShutdownerApp App { get; set; }
        public bool IsShutdowned { get; private set; }
        const double ChangeInterval = 0.5*60*60*1000;
        public MainWindow()
        {
            App = new AutoShutdownerApp(this);
            InitializeComponent();
            var icon =  AutoShutdowner.Properties.Resources.MainIcon;
            Icon = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(//IDispose doesn't matter in this app
                icon.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
            App.reset();
        }

        private void UpDownShutdownInterval_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (e.NewValue < e.OldValue)
                App.ShowAfterInterval += ChangeInterval;
            else
                App.ShowAfterInterval -= ChangeInterval;
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.Up:
                    App.ShowAfterInterval += ChangeInterval;
                    break;
                case Key.Down:
                    App.ShowAfterInterval -= ChangeInterval;
                    break;
                case Key.Enter:
                    OK_Click(null, null);
                    break;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            IsShutdowned = true;
        }
    }
}
