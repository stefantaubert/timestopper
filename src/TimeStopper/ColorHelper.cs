using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TimeStopper
{
    public static class ColorHelper
    {
        public static Icon GetIcon(this Color color)
        {
            const int expansion = 1;

            using (var bitmap = new Bitmap(expansion, expansion))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    var colorBrush = new SolidBrush(color);
                    var regionRectangle = new Rectangle(0, 0, expansion, expansion);
                    var colorRegion = new Region(regionRectangle);

                    graphics.FillRegion(colorBrush, colorRegion);
                }

                return Icon.FromHandle(bitmap.GetHicon());
            }
        }
    }
}
