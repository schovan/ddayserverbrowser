using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interactivity;

namespace ServerBrowser.Behaviors
{
    public class WindowBehavior : Behavior<Window>
    {
        #region Dll Imports

        [DllImport("user32.dll")]
        static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
        static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int GetWindowThreadProcessId(IntPtr hWnd, out Int32 lpdwProcessId);

        #endregion

        #region Delegates

        private delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        #endregion

        #region Constants

        private const uint WineventOutofcontext = 0;
        private const uint EventSystemForeground = 3;

        #endregion

        #region Members

        private static IntPtr _hookPtr;
        private static WinEventDelegate _winEventDelegate;
        private static int _pid;
        private static DependencyObject _dependencyObject;

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty EnabledProperty =
            DependencyProperty.RegisterAttached("Enabled", typeof(bool), typeof(WindowBehavior), new PropertyMetadata(OnEnabledChanged));

        public static bool GetEnabled(DependencyObject d)
        {
            return (bool)d.GetValue(EnabledProperty);
        }

        public static void SetEnabled(DependencyObject d, bool value)
        {
            d.SetValue(EnabledProperty, value);
        }

        private static void OnEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var window = d as Window;
            if (window != null && args.NewValue is bool)
            {
                if ((bool)args.NewValue)
                {

                    _dependencyObject = d;
                    SetHook();
                    window.StateChanged += WindowOnStateChanged;
                    window.Closing += WindowOnClosing;
                }
                else
                {
                    UnsetHook();
                    window.StateChanged -= WindowOnStateChanged;
                    window.Closing -= WindowOnClosing;
                    _dependencyObject = null;
                }
            }
        }

        public static readonly DependencyProperty IsWindowHiddenProperty = DependencyProperty.RegisterAttached("IsWindowHidden", typeof(bool), typeof(WindowBehavior), new FrameworkPropertyMetadata(OnIsWindowHiddenChanged)
        {
            BindsTwoWayByDefault = true,
            DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        });

        public static bool GetIsWindowHidden(DependencyObject d)
        {
            return (bool)d.GetValue(IsWindowHiddenProperty);
        }

        public static void SetIsWindowHidden(DependencyObject d, bool value)
        {
            d.SetValue(IsWindowHiddenProperty, value);
        }

        private static void OnIsWindowHiddenChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var window = (Window)d;
            if (args.NewValue is bool && !(bool)args.NewValue)
            {
                window.Show();
                window.WindowState = WindowState.Normal;
                SetIsWindowActive(window, true);
            }
        }

        public static readonly DependencyProperty IsWindowActiveProperty = DependencyProperty.RegisterAttached("IsWindowActive", typeof(bool), typeof(WindowBehavior), new FrameworkPropertyMetadata()
        {
            BindsTwoWayByDefault = true,
            DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        });

        public static bool GetIsWindowActive(DependencyObject d)
        {
            return (bool)d.GetValue(IsWindowActiveProperty);
        }

        public static void SetIsWindowActive(DependencyObject d, bool value)
        {
            d.SetValue(IsWindowActiveProperty, value);
        }

        #endregion

        #region Window Event Handlers

        private static void WindowOnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            var window = (Window)sender;
            cancelEventArgs.Cancel = true;
            window.Hide();
            SetIsWindowHidden(window, true);
            SetIsWindowActive(window, false);
        }

        private static void WindowOnStateChanged(object sender, EventArgs eventArgs)
        {
            var window = (Window)sender;
            if (window.WindowState == WindowState.Minimized)
            {
                window.Hide();
                SetIsWindowHidden(window, true);
                SetIsWindowActive(window, false);
            }
        }
        
        #endregion

        #region Private Static Methods

        private static void SetHook()
        {
            _winEventDelegate = WinEventProc;
            _pid = Process.GetCurrentProcess().Id;
            _hookPtr = SetWinEventHook(EventSystemForeground, EventSystemForeground, IntPtr.Zero, _winEventDelegate, 0, 0, WineventOutofcontext);
        }

        private static void UnsetHook()
        {
            bool result = UnhookWindowsHookEx(_hookPtr);
        }

        private static int GetWindowProcessId()
        {
            Int32 pid;
            IntPtr hwnd = GetForegroundWindow();
            GetWindowThreadProcessId(hwnd, out pid);
            return pid;
        }

        public static void WinEventProc(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            int currentPid = GetWindowProcessId();
            bool isWindowActive = currentPid == _pid;
            SetIsWindowActive(_dependencyObject, isWindowActive);
        }

        #endregion
    }
}