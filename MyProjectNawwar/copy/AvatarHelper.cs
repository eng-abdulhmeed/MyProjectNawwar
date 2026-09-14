/*using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace MyProjectNawwar.Helpers
{
    public static class AvatarHelper
    {
        // Pleasing palette matching Nawwar's modern aesthetic
        private static readonly Color[] Palette = new Color[]
        {
            Color.FromArgb(78, 125, 165),   // #4E7DA5 Nawwar Brand Blue
            Color.FromArgb(59, 130, 246),   // Blue
            Color.FromArgb(14, 165, 233),   // Sky
            Color.FromArgb(99, 102, 241),   // Indigo
            Color.FromArgb(139, 92, 246),   // Violet
            Color.FromArgb(16, 185, 129),   // Emerald
            Color.FromArgb(20, 184, 166),   // Teal
            Color.FromArgb(245, 158, 11),   // Amber
            Color.FromArgb(239, 68, 68),    // Red
            Color.FromArgb(236, 72, 153)    // Pink
        };

        public static Bitmap GenerateAvatar(string name, int size = 80)
        {
            if (size <= 0) size = 80;

            Bitmap bmp = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                // Pick color deterministically based on name
                int colorIndex = 0;
                if (!string.IsNullOrWhiteSpace(name))
                {
                    colorIndex = Math.Abs(name.Trim().GetHashCode()) % Palette.Length;
                }
                Color bgColor = Palette[colorIndex];

                // Draw filled background circle
                using (Brush brush = new SolidBrush(bgColor))
                {
                    g.FillEllipse(brush, 0, 0, size - 1, size - 1);
                }

                // Add subtle inner border
                using (Pen pen = new Pen(Color.FromArgb(60, 255, 255, 255), 1.5f))
                {
                    g.DrawEllipse(pen, 1, 1, size - 3, size - 3);
                }

                // Extract first character
                string initial = "ن";
                if (!string.IsNullOrWhiteSpace(name))
                {
                    string clean = name.Trim();
                    initial = clean.Substring(0, 1).ToUpperInvariant();
                }

                // Calculate font size relative to circle size
                float fontSize = size * 0.44f;
                using (Font font = new Font("Arial", fontSize, FontStyle.Bold, GraphicsUnit.Pixel))
                using (Brush textBrush = new SolidBrush(Color.White))
                using (StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                })
                {
                    // Draw centered character
                    g.DrawString(initial, font, textBrush, new RectangleF(0, 0, size, size), sf);
                }
            }

            return bmp;
        }
    }
}
*/