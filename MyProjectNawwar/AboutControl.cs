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

            // تعيين صور الأفاتار للمطورين
            if (picDev1 != null)
            {
                picDev1.Image = AvatarHelper.GenerateAvatar("Tariq Swar", picDev1.Width);
            }

            if (picDev2 != null)
            {
                picDev2.Image = AvatarHelper.GenerateAvatar("Abdulhmeed Abo-Hatem", picDev2.Width);
            }
        }

        private void lblDev1Name_Click(object sender, EventArgs e)
        {

        }
    }
}
