using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using MyProjectNawwar.Data;
using MyProjectNawwar.Helpers;
using MyProjectNawwar.Models;

namespace MyProjectNawwar
{
    public partial class Profile : UserControl
    {
        public Profile()
        {
            InitializeComponent();
            guna2Button1.Click += btnEditProfile_Click;
            SessionManager.OnSessionUpdated += SessionManager_OnSessionUpdated;
        }

        private void SessionManager_OnSessionUpdated()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(LoadUserProfile));
            }
            else
            {
                LoadUserProfile();
            }
        }

        private void Profile_Load(object sender, EventArgs e)
        {
            LoadUserProfile();
        }

        public void LoadUserProfile()
        {
            // إذا لم تكن هناك جلسة نشطة (مثلاً فتح الفورم مباشرة أثناء التجربة)
            if (!SessionManager.IsLoggedIn)
            {
                // محاولة تحميل أول مستخدم نشط كـ Fallback
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

            string name = !string.IsNullOrEmpty(SessionManager.FullName) ? SessionManager.FullName : "مستخدم نَــــوّار";
            string userHandle = !string.IsNullOrEmpty(SessionManager.Username) ? "@" + SessionManager.Username : "@user";

            label3.Text = name;
            label4.Text = userHandle;
            label1.Text = name;
            label2.Text = userHandle;
            label6.Text = "Role: " + SessionManager.RoleName;
            label10.Text = "Trust Score: " + SessionManager.TrustScore;
            label8.Text = "Points: " + SessionManager.TotalPoints;

            // توليد أيقونة الحساب الدائرية بالحرف الأول (Twitter / Google style)
            guna2CirclePictureBox1.Image = AvatarHelper.GenerateAvatar(name, guna2CirclePictureBox1.Width);

            // جلب الإحصائيات الحقيقية للمستخدم من قاعدة البيانات
            try
            {
                using (SqlConnection conn = DatabaseHelper.CreateConnection())
                {
                    conn.Open();
                    string statsQuery = @"
                        SELECT 
                            (SELECT COUNT(*) FROM Posts WHERE Publisher_ID = @UID) AS PostsCount,
                            (SELECT COUNT(*) FROM Post_Bookmarks WHERE User_ID = @UID) AS BookmarksCount,
                            (SELECT COUNT(*) FROM Post_Likes WHERE User_ID = @UID) AS LikesCount";

                    using (SqlCommand cmd = new SqlCommand(statsQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@UID", SessionManager.UserID);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int postsCount = reader.GetInt32(0);
                                int bookmarksCount = reader.GetInt32(1);
                                int likesCount = reader.GetInt32(2);

                                label17.Text = postsCount.ToString();
                                label12.Text = bookmarksCount.ToString();
                                label15.Text = likesCount.ToString();
                            }
                        }
                    }
                }
            }
            catch
            {
                // الحفاظ على الأرقام الحالية في حال تعذر الاتصال
            }
        }

        private void btnEditProfile_Click(object sender, EventArgs e)
        {
            using (EditProfileForm editForm = new EditProfileForm())
            {
                if (editForm.ShowDialog(this.FindForm()) == DialogResult.OK)
                {
                    LoadUserProfile();
                }
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }
    }
}
