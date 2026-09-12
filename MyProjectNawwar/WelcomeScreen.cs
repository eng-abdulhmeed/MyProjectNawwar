using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            // 1. جلب اسم المستخدم من مدير الجلسة
            lblWelcome.Text = $"أهلاً بك يا {Models.SessionManager.FullName} \nفي منصة نوار";

            // 2. إعداد المؤقت (Timer) برمجياً
            fadeTimer = new Timer();
            fadeTimer.Interval = 50; // سرعة التحديث (50 جزء من الثانية)
            fadeTimer.Tick += FadeTimer_Tick;
            fadeTimer.Start();
        }

        private void FadeTimer_Tick(object sender, EventArgs e)
        {
            waitCounter++;

            // الانتظار لمدة ثانية ونصف تقريباً قبل بدء التلاشي (30 * 50 = 1500ms)
            if (waitCounter > 30)
            {
                // تقليل الشفافية تدريجياً بنسبة 5% في كل لفة
                this.Opacity -= 0.05;

                // عندما تختفي الشاشة تماماً، نغلقها
                if (this.Opacity <= 0)
                {
                    fadeTimer.Stop();
                    this.Close();
                }
            }
        }
    }
}
