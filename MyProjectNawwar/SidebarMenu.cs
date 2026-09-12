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
using Guna.UI2.WinForms;

namespace MyProjectNawwar
{
    public partial class SidebarMenu : UserControl
    {
        public SidebarMenu()
        {
            InitializeComponent();

            // ربط كل أزرار القائمة بحدث واحد لتغيير اللون عند النقر
            btnHome.Click += MenuButton_Click;
            btnSessions.Click += MenuButton_Click;
            btnBookmarks.Click += MenuButton_Click;
            btnDebateRooms.Click += MenuButton_Click;
            btnProfile.Click += MenuButton_Click; // تم تصحيح الاسم هنا
        }

        private void SidebarMenu_Load(object sender, EventArgs e)
        {
            string connectionString = @"Server=.;Database=NawwarSystemDB;Trusted_Connection=True;Encrypt=False;";
            string loggedInUserId = "11111111-1111-1111-1111-111111111111"; // المعرف التجريبي

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT Full_Name, Email FROM Users WHERE ID = @ID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", loggedInUserId);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // عرض بيانات المستخدم
                            lblUserName.Text = reader["Full_Name"].ToString();
                            lblUserEmail.Text = reader["Email"].ToString();
                        }
                    }
                }
            }
        }

        // --- تأثيرات الألوان عند النقر ---
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

        // --- برمجة التنقل بين الشاشات ---

        // 1. زر الرئيسية (Home)
      
       /* private void btnHome_Click(object sender, EventArgs e)
        {

            // 1. تنظيف الحاوية الرئيسية (MainPanlMain) من أي شاشات أو صفحات مفتوحة سابقاً
            MainPanlMain.Controls.Clear();

            // 2. أخذ نسخة من صفحة الـ Home
            Home homeScreen = new Home();

            // 3. السطر السحري: إجبار النافذة على التصرف كعنصر داخلي يقبل الدخول في الحاوية
            homeScreen.TopLevel = false;

            // 4. إخفاء إطار النافذة (علامة X والتكبير والتصغير) لكي تبدو كجزء من الصفحة
            homeScreen.FormBorderStyle = FormBorderStyle.None;

            // 5. جعلها تتمدد لتغطي مساحة MainPanlMain بالكامل
            homeScreen.Dock = DockStyle.Fill;

            // 6. إضافتها أخيراً وإظهارها للمستخدم
            MainPanlMain.Controls.Add(homeScreen);
            homeScreen.Show();
        }*/
        private void btnHome_Click(object sender, EventArgs e)
        {
            // 1. أمر البحث: نطلب من القائمة الجانبية البحث عن الحاوية في النافذة الرئيسية (الأب)
            Panel mainPanel = (Panel)this.ParentForm.Controls.Find("MainPanlMain", true).FirstOrDefault();

            // نتحقق من أنه وجد الحاوية بنجاح
            if (mainPanel != null)
            {
                // 2. تنظيف الحاوية من أي شاشات سابقة
                mainPanel.Controls.Clear();

                // 3. أخذ نسخة من صفحة الـ Home وتجهيزها كعنصر داخلي
                Home homeScreen = new Home();
                homeScreen.TopLevel = false;
                homeScreen.FormBorderStyle = FormBorderStyle.None;
                homeScreen.Dock = DockStyle.Fill;

                // 4. إضافتها أخيراً وإظهارها للمستخدم
                mainPanel.Controls.Add(homeScreen);
                homeScreen.Show();
            }
            else
            {
                MessageBox.Show("لم يتم العثور على الحاوية الرئيسية!");
            }
        }

        // 2. زر المحفوظات (Bookmarks)
        private void btnBookmarks_Click_1(object sender, EventArgs e)
        {
            Home parent = this.ParentForm as Home;
            if (parent != null)
            {
                // عندما تصمم شاشة المحفوظات، قم بإزالة التعليق عن هذا السطر:
                // parent.LoadScreen(new BookmarksControl());
            }
        }

        // 3. زر الجلسات (Sessions)
        private void btnSessions_Click_1(object sender, EventArgs e)
        {
            Home parent = this.ParentForm as Home;
            if (parent != null)
            {
                // عندما تصمم شاشة الجلسات، قم بإزالة التعليق عن هذا السطر:
                // parent.LoadScreen(new SessionsControl());
            }
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }


        // يمكنك إضافة دوال لباقي الأزرار (Debate Rooms, Profile) بنفس الطريقة هنا
    }
}



/*using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
        private void SidebarMenu_Load(object sender, EventArgs e)
        {
            string connectionString = @"Server=.;Database=NawwarSystemDB;Trusted_Connection=True;Encrypt=False;";
            string loggedInUserId = "11111111-1111-1111-1111-111111111111"; // المعرف التجريبي

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT Full_Name, Email FROM Users WHERE ID = @ID";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", loggedInUserId);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // تأكد من مطابقة أسماء الـ Labels لتصميمك
                            lblUserName.Text = reader["Full_Name"].ToString();
                            lblUserEmail.Text = reader["Email"].ToString();
                        }
                    }
                }
            }
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

      


        private void label2_Click(object sender, EventArgs e)
        {

        }

       
        // 1. برمجة زر الرئيسية (Home)
        private void btnHome_Click(object sender, EventArgs e)
        {
            // الوصول لشاشة الهوم واستدعاء دالة إظهار المنشورات
            Home parent = this.ParentForm as Home;
            if (parent != null)
            {
                parent.ShowHomeFeed();
            }
        }

        // 2. برمجة زر المحفوظات (Bookmarks)
        private void btnBookmarks_Click_1(object sender, EventArgs e)
        {
            Home parent = this.ParentForm as Home;
            if (parent != null)
            {
                // بافتراض أنك ستقوم بإنشاء User Control جديد لصفحة المحفوظات اسمه BookmarksControl
                // parent.LoadScreen(new BookmarksControl());
            }
        }

        // 3. برمجة زر الجلسات (Sessions)
        private void btnSessions_Click_1(object sender, EventArgs e)
        {
            Home parent = this.ParentForm as Home;
            if (parent != null)
            {
                // parent.LoadScreen(new SessionsControl());
            }
        }

        private void btnBookmarks_Click(object sender, EventArgs e)
        {

        }



        // وتستمر بنفس الطريقة لباقي الأزرار (Debate Rooms, Profile, Start a Session)
    }
}
*/