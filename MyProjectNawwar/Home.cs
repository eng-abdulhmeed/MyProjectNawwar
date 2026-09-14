using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using MyProjectNawwar.Data;
using MyProjectNawwar.Helpers;
using MyProjectNawwar.Models;

namespace MyProjectNawwar
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        // =========================================================
        // تحميل صفحة داخل MainPanelMain
        // =========================================================
        public void LoadScreen(UserControl uc)
        {
            if (uc == null)
                return;

            MainPanelMain.Visible = true;
            ClearMainPanel();

            uc.Dock = DockStyle.Fill;
            MainPanelMain.Controls.Add(uc);
            uc.BringToFront();
        }

        // =========================================================
        // العودة إلى صفحة Home / المنشورات
        // =========================================================
        public void ShowHomeFeed()
        {
            MainPanelMain.Visible = true;
            ClearMainPanel();

            MainPanel.Dock = DockStyle.Fill;
            MainPanel.Visible = true;

            MainPanelMain.Controls.Add(MainPanel);
            MainPanel.BringToFront();

            // تحديث المنشورات عند العودة للرئيسية
            LoadPosts();
        }

        // =========================================================
        // تنظيف MainPanelMain
        // =========================================================
        private void ClearMainPanel()
        {
            Control[] controls = new Control[MainPanelMain.Controls.Count];
            MainPanelMain.Controls.CopyTo(controls, 0);

            foreach (Control control in controls)
            {
                MainPanelMain.Controls.Remove(control);

                if (control != MainPanel)
                {
                    control.Dispose();
                }
            }
        }

        // =========================================================
        // تحميل الصفحة
        // =========================================================
        private void Home_Load(object sender, EventArgs e)
        {
            this.Icon = AppAssets.AppIcon;

            // التأكد من تحميل بيانات الجلسة إذا كانت فارغة
            if (!SessionManager.IsLoggedIn)
            {
                try
                {
                    using (SqlConnection conn = DatabaseHelper.CreateConnection())
                    {
                        conn.Open();
                        using (SqlCommand cmd = new SqlCommand("SELECT TOP 1 ID FROM Users WHERE Status = 'Active' ORDER BY Trust_Score DESC", conn))
                        {
                            object val = cmd.ExecuteScalar();
                            if (val != null)
                            {
                                SessionManager.LoadUser(Guid.Parse(val.ToString()));
                            }
                        }
                    }
                }
                catch { }
            }

            ShowHomeFeed();
        }

        private void LoadPosts()
        {
            SearchPosts(null);
        }

        // =========================================================
        // البحث في المنشورات وتصفيتها
        // =========================================================
        public void SearchPosts(string keyword)
        {
            flowLayoutPanel1.Controls.Clear();

            bool isFiltered = !string.IsNullOrWhiteSpace(keyword);
            string query;

            if (!isFiltered)
            {
                query = @"
                    SELECT 
                        Posts.ID,
                        Users.Full_Name,
                        Users.Email,
                        Posts.Title,
                        Posts.Content_Body 
                    FROM Posts
                    INNER JOIN Users 
                        ON Posts.Publisher_ID = Users.ID 
                    ORDER BY Posts.ID DESC";
            }
            else
            {
                query = @"
                    SELECT 
                        Posts.ID,
                        Users.Full_Name,
                        Users.Email,
                        Posts.Title,
                        Posts.Content_Body 
                    FROM Posts
                    INNER JOIN Users 
                        ON Posts.Publisher_ID = Users.ID 
                    WHERE Posts.Title LIKE @Search 
                       OR Posts.Content_Body LIKE @Search 
                       OR Posts.Category LIKE @Search 
                       OR Users.Full_Name LIKE @Search 
                       OR Users.Email LIKE @Search
                    ORDER BY Posts.ID DESC";
            }

            try
            {
                using (SqlConnection conn = DatabaseHelper.CreateConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (isFiltered)
                        {
                            string cleanKw = keyword.Trim().TrimStart('#');
                            cmd.Parameters.AddWithValue("@Search", "%" + cleanKw + "%");
                        }

                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            int matchCount = 0;
                            while (reader.Read())
                            {
                                matchCount++;
                                post postControl = new post();

                                string email = reader["Email"].ToString();
                                string extractedUsername = email.Contains("@") ? email.Split('@')[0] : email;

                                postControl.SetPostData(
                                    Convert.ToInt32(reader["ID"]),
                                    reader["Full_Name"].ToString(),
                                    "@" + extractedUsername,
                                    "الآن",
                                    reader["Title"].ToString(),
                                    reader["Content_Body"].ToString()
                                );

                                flowLayoutPanel1.Controls.Add(postControl);
                            }

                            if (matchCount == 0 && isFiltered)
                            {
                                Panel pnlEmpty = new Panel
                                {
                                    Size = new Size(flowLayoutPanel1.Width - 30, 160),
                                    BackColor = Color.FromArgb(21, 34, 56),
                                    Margin = new Padding(15, 20, 15, 20)
                                };

                                Label lblEmptyTitle = new Label
                                {
                                    Text = "🔍 لا توجد منشورات تطابق: \"" + keyword + "\"",
                                    ForeColor = Color.FromArgb(226, 232, 240),
                                    Font = new Font("Arial", 12, FontStyle.Bold),
                                    Dock = DockStyle.Top,
                                    Height = 50,
                                    TextAlign = ContentAlignment.MiddleCenter
                                };

                                Label lblEmptySub = new Label
                                {
                                    Text = "جرب البحث بكلمات أخرى أو اختر أحد الموضوعات المتداولة في القائمة الجانبية.",
                                    ForeColor = Color.FromArgb(175, 193, 208),
                                    Font = new Font("Arial", 10, FontStyle.Regular),
                                    Dock = DockStyle.Fill,
                                    TextAlign = ContentAlignment.TopCenter
                                };

                                pnlEmpty.Controls.Add(lblEmptySub);
                                pnlEmpty.Controls.Add(lblEmptyTitle);
                                flowLayoutPanel1.Controls.Add(pnlEmpty);
                            }
                        }
                    }
                }
            }
            catch
            {
                // في حال حدوث خطأ أثناء جلب المنشورات
            }
        }

        // =========================================================
        // شاشة الترحيب
        // =========================================================
        private void Home_Shown(object sender, EventArgs e)
        {
            WelcomeScreen welcome = new WelcomeScreen();
            welcome.Size = this.Size;
            welcome.Location = this.Location;
            welcome.Show(this);
        }

        private void sidebarMenu1_Load(object sender, EventArgs e)
        {
        }

        private void guna2Panel3_Paint(object sender, PaintEventArgs e)
        {
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void sidebarMenu1_Load_1(object sender, EventArgs e)
        {

        }
    }
}