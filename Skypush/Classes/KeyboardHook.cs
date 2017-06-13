using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skypush.Classes
{
    [Flags]
    public enum CustomModifierKeys : uint
    {
        None = 0,
        Alt = 1,
        Control = 2,
        Shift = 4,
        Win = 8
    }

    public sealed class KeyboardHook : IDisposable
    {
        private class Window : NativeWindow, IDisposable
        {
            private static int WM_HOTKEY = 0x0312;

            public Window()
            {
                CreateHandle(new CreateParams());
            }

            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);

                if (m.Msg == WM_HOTKEY)
                {
                    Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                    CustomModifierKeys modifier = (CustomModifierKeys)((int)m.LParam & 0xFFFF);
                    KeyPressed?.Invoke(this, new KeyPressedEventArgs(modifier, key));
                }
            }

            public event EventHandler<KeyPressedEventArgs> KeyPressed;

            public void Dispose()
            {
                DestroyHandle();
            }
        }

        private Window _window = new Window();
        public int _currentId;

        public KeyboardHook()
        {
            _window.KeyPressed += delegate (object sender, KeyPressedEventArgs args)
            {
                KeyPressed?.Invoke(this, args);
            };
        }

        public bool RegisterHotKey(CustomModifierKeys modifier, Keys key)
        {
            _currentId = _currentId + 1;

            if (!User32.RegisterHotKey(_window.Handle, _currentId, (uint)modifier, (uint)key))
            {
                return false;
            }
            return true;
        }

        public event EventHandler<KeyPressedEventArgs> KeyPressed;

        public void UnregisterHotKey(int HotKeyId)
        {
            User32.UnregisterHotKey(_window.Handle, HotKeyId);
        }

        public void Dispose()
        {
            for (int i = _currentId; i > 0; i--)
            {
                User32.UnregisterHotKey(_window.Handle, i);
            }

            _window.Dispose();
        }
    }
}
