using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using MyProjectNawwar.Helpers;
using MyProjectNawwar.Models;

namespace MyProjectNawwar
{
    public partial class SidebarMenu : UserControl
    {
        public SidebarMenu()
        {
            InitializeComponent();

            // ربط أزرار القائمة بحدث تغيير اللون
            btnHome.Click += MenuButton_Click;
            btnSessions.Click += MenuButton_Click;
            btnBookmarks.Click += MenuButton_Click;
            btnDebateRooms.Click += MenuButton_Click;
            btnProfile.Click += MenuButton_Click;
            guna2Button2.Click += MenuButton_Click;

            // الاشتراك في حدث تحديث الجلسة لتحديث بيانات القائمة تلقائياً
            SessionManager.OnSessionUpdated += SessionManager_OnSessionUpdated;
        }

        private void SessionManager_OnSessionUpdated()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(LoadUserData));
            }
            else
            {
                LoadUserData();
            }
        }

        // =========================================================
        // تحميل بيانات المستخدم من الجلسة والشعار
        // =========================================================
        private void SidebarMenu_Load(object sender, EventArgs e)
        {
            // وضع شعار المنصة في اللوحة العلوية
            if (AppAssets.Logo != null)
            {
                guna2Panel1.BackgroundImage = AppAssets.Logo;
                guna2Panel1.BackgroundImageLayout = ImageLayout.Zoom;
            }

            LoadUserData();
        }

        public void LoadUserData()
        {
            string displayName = !string.IsNullOrEmpty(SessionManager.FullName) ? SessionManager.FullName : "مستخدم نَــــوّار";
            string displayEmail = !string.IsNullOrEmpty(SessionManager.Email) ? SessionManager.Email : "@nawwar.com";

            lblUserName.Text = displayName;
            lblUserEmail.Text = displayEmail;

            // توليد وتعيين أيقونة الحساب الدائرية بالحرف الأول (Twitter / Google style)
            btnProfilewd.Image = AvatarHelper.GenerateAvatar(displayName, btnProfilewd.Width);
            btnProfilewd.Text = string.Empty; // مسح النص واستبداله بصورة الحرف الدائرية
        }

        // =========================================================
        // عند الضغط على أي زر
        // =========================================================
        private void MenuButton_Click(object sender, EventArgs e)
        {
            Guna2Button clickedButton = sender as Guna2Button;
            if (clickedButton == null)
                return;

            // إعادة كل الأزرار للوضع الطبيعي
            ResetButtons();

            // تفعيل الزر الحالي
            clickedButton.FillColor = Color.FromArgb(33, 78, 125, 165);
            clickedButton.ForeColor = ColorTranslator.FromHtml("#4E7DA5");
            clickedButton.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold);
        }

        // =========================================================
        // إعادة الأزرار للوضع الطبيعي
        // =========================================================
        private void ResetButtons()
        {
            Guna2Button[] buttons =
            {
                btnHome,
                btnSessions,
                btnBookmarks,
                btnDebateRooms,
                btnProfile,
                guna2Button2 // About button
            };

            foreach (Guna2Button btn in buttons)
            {
                btn.FillColor = Color.Transparent;
                btn.ForeColor = ColorTranslator.FromHtml("#E2E8F0");
                btn.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold);
            }
        }

        // =========================================================
        // الحصول على Home
        // =========================================================
        private Home GetHome()
        {
            return this.ParentForm as Home;
        }

        // =========================================================
        // HOME
        // =========================================================
        private void btnHome_Click(object sender, EventArgs e)
        {
            Home parent = GetHome();
            if (parent != null)
            {
                parent.ShowHomeFeed();
            }
        }

        // =========================================================
        // SESSIONS
        // =========================================================
        private void btnSessions_Click_1(object sender, EventArgs e)
        {
            Home parent = GetHome();
            if (parent != null)
            {
                // يمكن تحميل شاشة الجلسات هنا عند توفرها
            }
        }

        // =========================================================
        // BOOKMARKS
        // =========================================================
        private void btnBookmarks_Click_1(object sender, EventArgs e)
        {
            Home parent = GetHome();
            if (parent != null)
            {
                // يمكن تحميل شاشة المحفوظات هنا
            }
        }

        // =========================================================
        // DEBATE ROOMS
        // =========================================================
        private void btnDebateRooms_Click(object sender, EventArgs e)
        {
            Home parent = GetHome();
            if (parent != null)
            {
                // يمكن تحميل شاشة المناظرات هنا
            }
        }

        // =========================================================
        // PROFILE
        // =========================================================
        private void btnProfile_Click_1(object sender, EventArgs e)
        {
            Home parent = GetHome();
            if (parent != null)
            {
                parent.LoadScreen(new Profile());
            }
        }

        // =========================================================
        // ABOUT US (عن المنصة)
        // =========================================================
        private void btnAbout_Click(object sender, EventArgs e)
        {
            Home parent = GetHome();
            if (parent != null)
            {
                parent.LoadScreen(new AboutControl());
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            // النقر على اسم المستخدم في الأسفل يفتح صفحة البروفايل
            btnProfile_Click_1(sender, e);
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            btnProfile_Click_1(sender, e);
        }
    }
}