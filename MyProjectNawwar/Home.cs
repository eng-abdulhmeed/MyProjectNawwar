using System;
using System.Data.SqlClient;
using System.Windows.Forms;

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

            // MainPanelMain هو الـ Container الرئيسي
            MainPanelMain.Visible = true;

            // إزالة الصفحة الحالية
            ClearMainPanel();

            // جعل الصفحة الجديدة تملأ المساحة
            uc.Dock = DockStyle.Fill;

            // إضافة الصفحة الجديدة
            MainPanelMain.Controls.Add(uc);

            // إظهارها في المقدمة
            uc.BringToFront();
        }

        // =========================================================
        // العودة إلى صفحة Home / المنشورات
        // =========================================================
        public void ShowHomeFeed()
        {
            // التأكد أن الحاوية الرئيسية ظاهرة
            MainPanelMain.Visible = true;

            // إزالة الصفحة الحالية
            ClearMainPanel();

            // إعادة MainPanel الخاصة بالـ Home
            MainPanel.Dock = DockStyle.Fill;
            MainPanel.Visible = true;

            MainPanelMain.Controls.Add(MainPanel);
            MainPanel.BringToFront();
        }

        // =========================================================
        // تنظيف MainPanelMain
        // =========================================================
        private void ClearMainPanel()
        {
            // ننسخ القائمة لأننا سنعدل Controls أثناء المرور عليها
            Control[] controls = new Control[MainPanelMain.Controls.Count];

            MainPanelMain.Controls.CopyTo(controls, 0);

            foreach (Control control in controls)
            {
                MainPanelMain.Controls.Remove(control);

                // لا نحذف MainPanel نفسه لأنه جزء من Home
                // وسيتم استخدامه مرة أخرى عند الضغط على Home
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
            // عرض Home في البداية
            ShowHomeFeed();

            // تنظيف المنشورات القديمة
            flowLayoutPanel1.Controls.Clear();

            string connectionString =
                @"Server=.;Database=NawwarSystemDB;Trusted_Connection=True;Encrypt=False;";

            string query = @"
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

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            post postControl = new post();

                            string email = reader["Email"].ToString();

                            string extractedUsername =
                                email.Contains("@")
                                ? email.Split('@')[0]
                                : email;

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
                    }
                }
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

        // =========================================================
        // أحداث Designer
        // =========================================================

        private void sidebarMenu1_Load(object sender, EventArgs e)
        {
        }

        private void guna2Panel3_Paint(object sender, PaintEventArgs e)
        {
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}