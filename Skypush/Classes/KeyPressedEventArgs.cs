using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skypush.Classes
{
    public class KeyPressedEventArgs : EventArgs
    {
        private CustomModifierKeys _modifier;
        private Keys _key;

        internal KeyPressedEventArgs(CustomModifierKeys modifier, Keys key)
        {
            _modifier = modifier;
            _key = key;
        }

        public CustomModifierKeys Modifier
        {
            get { return _modifier; }
        }

        public Keys Key
        {
            get { return _key; }
        }
    }
}
