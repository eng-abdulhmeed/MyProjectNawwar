using System;
using System.Drawing;
using System.Windows.Forms;
using MyProjectNawwar.Helpers;

namespace MyProjectNawwar
{
    public partial class AboutControl : UserControl
    {
        public AboutControl()
        {
            InitializeComponent();
        }

        private void AboutControl_Load(object sender, EventArgs e)
        {
            picLogo.Image = AppAssets.Logo;
        }
    }
}
