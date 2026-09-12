using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyProjectNawwar
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        // --- 1. دالة عرض الشاشات الجديدة (مثل Bookmarks و Profile) ---
       /* public void LoadScreen(UserControl uc)
        {
            // إخفاء حاوية المنشورات لأننا سنعرض شاشة أخرى
            flowLayoutPanel1.Visible = false;
            MainPanel.Visible = true;

            // مسح أي محتوى سابق داخل اللوحة
            MainPanel.Controls.Clear();

            // جعل الجزء الجديد يملأ مساحة اللوحة بالكامل
            uc.Dock = DockStyle.Fill;

            // إضافة الجزء الجديد إلى اللوحة
            MainPanel.Controls.Add(uc);

            // جلبه للمقدمة للتأكد من ظهوره
            uc.BringToFront();
        }
*/
        // --- 2. دالة العودة للصفحة الرئيسية (المنشورات) ---
        public void ShowHomeFeed()
        {
            // إخفاء حاوية الشاشات وإظهار حاوية المنشورات من جديد
            MainPanelMain.Visible = false; // إخفاء الحاوية الجديدة
            MainPanel.Visible = true;
        }

        private void Home_Load(object sender, EventArgs e)
        {
            // في البداية نعرض المنشورات ونخفي الشاشات الأخرى
            ShowHomeFeed();
            flowLayoutPanel1.Controls.Clear();

            string connectionString = @"Server=.;Database=NawwarSystemDB;Trusted_Connection=True;Encrypt=False;";
            string query = @"
            SELECT Posts.ID, Users.Full_Name, Users.Email, Posts.Title, Posts.Content_Body 
            FROM Posts 
            INNER JOIN Users ON Posts.Publisher_ID = Users.ID 
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
                            post post = new post();

                            string email = reader["Email"].ToString();
                            string extractedUsername = email.Contains("@") ? email.Split('@')[0] : email;

                            post.SetPostData(
                                Convert.ToInt32(reader["ID"]),
                                reader["Full_Name"].ToString(),
                                "@" + extractedUsername,
                                "الآن",
                                reader["Title"].ToString(),
                                reader["Content_Body"].ToString()
                            );

                            flowLayoutPanel1.Controls.Add(post);
                        }
                    }
                }
            }
        }

        private void Home_Shown(object sender, EventArgs e)
        {
            // استدعاء شاشة الترحيب
            WelcomeScreen welcome = new WelcomeScreen();
            welcome.Size = this.Size;
            welcome.Location = this.Location;
            welcome.Show(this);
        }
        public void LoadScreen(UserControl uc)
        {
            MainPanel.Visible = false;
            MainPanelMain.Visible = true; // استخدام الحاوية الجديدة
            MainPanelMain.Controls.Clear();

            uc.Dock = DockStyle.Fill;
            MainPanelMain.Controls.Add(uc);
            uc.BringToFront();
        }

       

        // دوال فارغة يمكنك استخدامها لاحقاً أو تركها كما هي
        private void sidebarMenu1_Load(object sender, EventArgs e) { }
        private void guna2Panel3_Paint(object sender, PaintEventArgs e) { }
        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e) { }
    }
}