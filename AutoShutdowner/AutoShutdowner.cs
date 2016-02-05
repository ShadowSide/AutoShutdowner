using System;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using NotifyIcon = System.Windows.Forms.NotifyIcon;
using Icon = System.Drawing.Icon;

namespace AutoShutdowner
{
    public class AutoShutdownerApp: INotifyPropertyChanged
    {
        readonly MainWindow _window;
        readonly Timer _shower = new Timer();//IDispose doesn't matter in this app
        readonly Shutdowner _shutdown = new Shutdowner();
        public Icon Icon { get; } = System.Drawing.Icon.FromHandle(AutoShutdowner.Properties.Resources.MainIcon.GetHicon());//IDispose doesn't matter in this app
        readonly NotifyIcon _notifyIcon = new NotifyIcon()//IDispose doesn't matter in this app
        {
            Text = "AutoShutdowner",
            Visible = true
        };
        private double _showAfterInterval = 2 * 60 * 60 * 1000;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public AutoShutdownerApp(MainWindow window)
        {
            _window = window;
            _shower.Elapsed += showMe;
            _shower.Interval = ShowAfterInterval;
            _shower.Stop();
            _notifyIcon.DoubleClick += (object sender, EventArgs e) => { showMe(null, null); };
            _notifyIcon.Icon = Icon;
            _shutdown.Enable = true;
        }

        void showMe(object sender, ElapsedEventArgs e)
        {
            if (_window.IsShutdowned)
                return;
            _window.Dispatcher.Invoke(()=>{ if(!_window.IsShutdowned) _window.Visibility = Visibility.Visible; });
            _shutdown.Enable = true;
        }

        public double ShowAfterInterval
        {
            get
            {
                return _showAfterInterval;
            }

            set
            {
                _showAfterInterval = value;
                if (value < 1)
                    _showAfterInterval = 1;
                OnPropertyChanged("ShowAfterInterval");
            }
        }

        public void reset()
        {
            _shutdown.Enable = false;
            _shower.Interval = ShowAfterInterval;
            _shower.Start();
        }
    }
}
