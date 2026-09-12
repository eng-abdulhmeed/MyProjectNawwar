
using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

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
        }

        // =========================================================
        // تحميل بيانات المستخدم
        // =========================================================
        private void SidebarMenu_Load(object sender, EventArgs e)
        {
            string connectionString =
                @"Server=.;Database=NawwarSystemDB;Trusted_Connection=True;Encrypt=False;";

            string loggedInUserId =
                "11111111-1111-1111-1111-111111111111";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT Full_Name, Email
                    FROM Users
                    WHERE ID = @ID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", loggedInUserId);

                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            lblUserName.Text =
                                reader["Full_Name"].ToString();

                            lblUserEmail.Text =
                                reader["Email"].ToString();
                        }
                    }
                }
            }
        }

        // =========================================================
        // عند الضغط على أي زر
        // =========================================================
        private void MenuButton_Click(object sender, EventArgs e)
        {
            Guna2Button clickedButton =
                sender as Guna2Button;

            if (clickedButton == null)
                return;

            // إعادة كل الأزرار للوضع الطبيعي
            ResetButtons();

            // تفعيل الزر الحالي
            clickedButton.FillColor =
                Color.FromArgb(33, 78, 125, 165);

            clickedButton.ForeColor =
                ColorTranslator.FromHtml("#4E7DA5");

            clickedButton.Font =
                new Font("Arial", 13, FontStyle.Bold);
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
                btnProfile
            };

            foreach (Guna2Button btn in buttons)
            {
                btn.FillColor = Color.Transparent;

                btn.ForeColor =
                    ColorTranslator.FromHtml("#E2E8F0");

                btn.Font =
                    new Font("Arial", 13, FontStyle.Regular);
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
                // عندما تنشئ SessionsControl:
                // parent.LoadScreen(new SessionsControl());
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
                // عندما تنشئ BookmarksControl:
                // parent.LoadScreen(new BookmarksControl());
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
                // عندما تنشئ DebateRoomsControl:
                // parent.LoadScreen(new DebateRoomsControl());
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
                // عندما تنشئ ProfileControl:
                 parent.LoadScreen(new Profile());
            }
        }

        // =========================================================
        // أحداث Designer
        // =========================================================
        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {

        }
    }
}