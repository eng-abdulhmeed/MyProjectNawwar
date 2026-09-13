using System;
using System.Drawing;
using System.Windows.Forms;
using MyProjectNawwar.Helpers;
using MyProjectNawwar.Models;

namespace MyProjectNawwar
{
    public partial class WelcomeScreen : Form
    {
        public WelcomeScreen()
        {
            InitializeComponent();
        }

        private void label15_Click(object sender, EventArgs e)
        {
        }

        private void pnlRight_Paint(object sender, PaintEventArgs e)
        {
        }

        private Timer fadeTimer;
        private int waitCounter = 0;

        private void WelcomeScreen_Load(object sender, EventArgs e)
        {
            // تعيين الشعار المضيء
            picWelcomeLogo.Image = AppAssets.Logo;

            // جلب اسم المستخدم من مدير الجلسة
            string name = !string.IsNullOrEmpty(SessionManager.FullName) ? SessionManager.FullName : "بك";
            lblWelcome.Text = $"أهلاً بك يا {name} \nفي منصة نَـــــوّار";

            // إعداد المؤقت برمجياً
            fadeTimer = new Timer();
            fadeTimer.Interval = 50;
            fadeTimer.Tick += FadeTimer_Tick;
            fadeTimer.Start();
        }

        private void FadeTimer_Tick(object sender, EventArgs e)
        {
            waitCounter++;

            if (waitCounter > 30)
            {
                this.Opacity -= 0.05;

                if (this.Opacity <= 0)
                {
                    fadeTimer.Stop();
                    this.Close();
                }
            }
        }
    }
}
