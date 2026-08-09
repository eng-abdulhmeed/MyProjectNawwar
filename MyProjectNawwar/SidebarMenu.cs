using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace MyProjectNawwar
{
    public partial class SidebarMenu : UserControl
    {
        public SidebarMenu()
        {
            InitializeComponent();

            // ربط كل أزرار القائمة بحدث واحد عند النقر
            btnHome.Click += MenuButton_Click;
            btnSessions.Click += MenuButton_Click;
            btnBookmarks.Click += MenuButton_Click;
            btnDebateRooms.Click += MenuButton_Click;
            btnProfilewd.Click += MenuButton_Click;
        }

        private void MenuButton_Click(object sender, EventArgs e)
        {
            Guna2Button clickedButton = sender as Guna2Button;

            // 1. إعادة جميع الأزرار لحالتها غير النشطة
            ResetButtons();

            // 2. تفعيل الزر الذي تم النقر عليه
            clickedButton.FillColor = Color.FromArgb(33, 78, 125, 165);
            clickedButton.ForeColor = ColorTranslator.FromHtml("#4E7DA5");
            clickedButton.Font = new Font("Arial", 13, FontStyle.Bold);
        }

        private void ResetButtons()
        {
            // قائمة بكل الأزرار لإرجاعها لوضعها الافتراضي
            Guna2Button[] buttons = { btnHome, btnSessions, btnBookmarks, btnDebateRooms, btnProfile };

            foreach (var btn in buttons)
            {
                btn.FillColor = Color.Transparent;
                btn.ForeColor = ColorTranslator.FromHtml("#E2E8F0");
                btn.Font = new Font("Arial", 13, FontStyle.Regular);
            }
        }

        private void SidebarMenu_Load(object sender, EventArgs e)
        {


        }


        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
