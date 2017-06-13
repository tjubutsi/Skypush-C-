using Skypush.Classes;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Skypush
{
    static class Helpers
    {
        public static Size GetScreenSize()
        {
            return new Size(SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);
        }

        public static ScreenLocation GetWindowLocation()
        {
            return new ScreenLocation(0, 0);
        }

        public static ScreenLocation GetTopLeftLocation()
        {
            return new ScreenLocation(SystemInformation.VirtualScreen.Left, SystemInformation.VirtualScreen.Top);
        }

        public static Bitmap GetAllMonitorsScreenshot()
        {
            var screenLocation = GetTopLeftLocation();
            var size = GetScreenSize();
            var printscreen = new Bitmap(size.Width, size.Height);
            using (Graphics graphics = Graphics.FromImage(printscreen as Image))
            {
                graphics.CopyFromScreen(screenLocation.Horizontal, screenLocation.Vertical, 0, 0, size);
            }
            return printscreen;
        }

        public static Bitmap GetWindowScreenshot(User32.RECT rect)
        {
            var bounds = new Rectangle(rect.left, rect.top, rect.right - rect.left, rect.bottom - rect.top);
            var printscreen = new Bitmap(bounds.Width, bounds.Height);

            using (var graphics = Graphics.FromImage(printscreen))
            {
                graphics.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
            }
            return printscreen;
        }

        public static User32.RECT GetCurrentWindowRect()
        {
            var window = User32.GetForegroundWindow();
            var rect = new User32.RECT();
            User32.GetWindowRect(window, ref rect);
            if (User32.IsZoomed(window))
            {
                rect.bottom -= 8;
                rect.top += 8;
                rect.left += 8;
                rect.right -= 8;
            }
            return rect;
        }
    }
}
