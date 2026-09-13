using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MyProjectNawwar.Helpers
{
    public static class AppAssets
    {
        private static Image _logoImage;
        private static Icon _appIcon;

        public static Image Logo
        {
            get
            {
                if (_logoImage == null)
                {
                    try
                    {
                        string appDir = AppDomain.CurrentDomain.BaseDirectory;
                        string[] possiblePaths = new string[]
                        {
                            Path.Combine(appDir, "Resources", "nawwar_logo.png"),
                            Path.Combine(appDir, "..", "..", "Resources", "nawwar_logo.png"),
                            Path.Combine(appDir, "nawwar_logo.png"),
                            @"c:\Users\tariq\source\repos\MyProjectNawwar\MyProjectNawwar\Resources\nawwar_logo.png"
                        };

                        foreach (string path in possiblePaths)
                        {
                            if (File.Exists(path))
                            {
                                using (var temp = Image.FromFile(path))
                                {
                                    _logoImage = new Bitmap(temp);
                                }
                                break;
                            }
                        }
                    }
                    catch
                    {
                        _logoImage = CreatePlaceholderLogo();
                    }

                    if (_logoImage == null)
                    {
                        _logoImage = CreatePlaceholderLogo();
                    }
                }
                return _logoImage;
            }
        }

        public static Icon AppIcon
        {
            get
            {
                if (_appIcon == null)
                {
                    try
                    {
                        using (Bitmap bmp = new Bitmap(Logo, new Size(32, 32)))
                        {
                            IntPtr hIcon = bmp.GetHicon();
                            _appIcon = Icon.FromHandle(hIcon);
                        }
                    }
                    catch
                    {
                        _appIcon = SystemIcons.Application;
                    }
                }
                return _appIcon;
            }
        }

        private static Image CreatePlaceholderLogo()
        {
            Bitmap bmp = new Bitmap(64, 64);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (Brush brush = new SolidBrush(Color.FromArgb(78, 125, 165)))
                {
                    g.FillEllipse(brush, 4, 4, 56, 56);
                }
                using (Font font = new Font("Arial", 24, FontStyle.Bold, GraphicsUnit.Pixel))
                using (Brush textBrush = new SolidBrush(Color.White))
                using (StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
                {
                    g.DrawString("ن", font, textBrush, new RectangleF(0, 0, 64, 64), sf);
                }
            }
            return bmp;
        }
    }
}
