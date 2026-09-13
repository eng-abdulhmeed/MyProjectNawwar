using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
using MyProjectNawwar.Data;
using MyProjectNawwar.Helpers;
using MyProjectNawwar.Models;

namespace MyProjectNawwar
{
    public partial class post : UserControl
    {
        public post()
        {
            InitializeComponent();
        }

        // --- المتغيرات ---
        private int currentPostID;
        private bool isLiked = false;
        private bool isBookmarked = false;
        private bool isReposted = false;

        // --- دالة تعبئة بيانات المنشور ---
        public void SetPostData(int postID, string name, string username, string timeAgo, string title, string bodyText)
        {
            currentPostID = postID;

            guna2HtmlLabel25.Text = name;
            guna2HtmlLabel24.Text = username;
            guna2HtmlLabel23.Text = timeAgo;
            guna2HtmlLabel17.Text = title;
            guna2HtmlLabel16.Text = bodyText;

            // توليد وتعيين صورة الحساب كأول حرف للناشر
            if (guna2CirclePictureBox4 != null)
            {
                guna2CirclePictureBox4.Image = AvatarHelper.GenerateAvatar(name, guna2CirclePictureBox4.Width);
            }

            // تحميل التفاعلات الفعلية للمنشور من قاعدة البيانات
            LoadInteractions();
        }

        private void LoadInteractions()
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.CreateConnection())
                {
                    conn.Open();

                    // عدد الإعجابات وما إذا كان المستخدم الحالي معجباً به
                    string query = @"
                        SELECT 
                            (SELECT COUNT(*) FROM Post_Likes WHERE Post_ID = @PID) AS LikesCount,
                            (SELECT COUNT(*) FROM Post_Bookmarks WHERE Post_ID = @PID) AS BookmarksCount,
                            (SELECT COUNT(*) FROM Post_Likes WHERE Post_ID = @PID AND User_ID = @UID) AS UserLiked,
                            (SELECT COUNT(*) FROM Post_Bookmarks WHERE Post_ID = @PID AND User_ID = @UID) AS UserBookmarked,
                            (SELECT COUNT(*) FROM Post_Reposts WHERE Post_ID = @PID AND User_ID = @UID) AS UserReposted";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@PID", currentPostID);
                        cmd.Parameters.AddWithValue("@UID", SessionManager.UserID);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int likesCount = reader.GetInt32(0);
                                int bookmarksCount = reader.GetInt32(1);
                                isLiked = reader.GetInt32(2) > 0;
                                isBookmarked = reader.GetInt32(3) > 0;
                                isReposted = reader.GetInt32(4) > 0;

                                lblLikeCount.Text = likesCount.ToString();
                                lblLikeCount.ForeColor = isLiked ? Color.Red : Color.Gray;

                                lblLikeCount2.Text = bookmarksCount.ToString();
                                lblLikeCount2.ForeColor = isBookmarked ? Color.DodgerBlue : Color.Gray;

                                if (guna2PictureBox9 != null)
                                    guna2PictureBox9.ForeColor = isLiked ? Color.Red : Color.Gray;

                                if (guna2PictureBox6 != null)
                                    guna2PictureBox6.ForeColor = isBookmarked ? Color.DodgerBlue : Color.Gray;

                                if (guna2PictureBox7 != null)
                                    guna2PictureBox7.ForeColor = isReposted ? Color.Green : Color.Gray;
                            }
                        }
                    }
                }
            }
            catch
            {
                // الحفاظ على الحالة الافتراضية عند تعذر الاستعلام
            }
        }

        // ==========================================
        // 1. برمجة زر الإعجاب (guna2PictureBox9)
        // ==========================================
        private void guna2PictureBox9_Click(object sender, EventArgs e)
        {
            if (SessionManager.UserID == Guid.Empty) return;

            try
            {
                using (SqlConnection conn = DatabaseHelper.CreateConnection())
                {
                    conn.Open();

                    if (isLiked)
                    {
                        string deleteQuery = "DELETE FROM Post_Likes WHERE Post_ID = @PostID AND User_ID = @UserID";
                        using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@PostID", currentPostID);
                            cmd.Parameters.AddWithValue("@UserID", SessionManager.UserID);
                            cmd.ExecuteNonQuery();
                        }

                        isLiked = false;
                        lblLikeCount.ForeColor = Color.Gray;
                        guna2PictureBox9.ForeColor = Color.Gray;

                        if (int.TryParse(lblLikeCount.Text, out int currentLikes) && currentLikes > 0)
                        {
                            lblLikeCount.Text = (currentLikes - 1).ToString();
                        }
                    }
                    else
                    {
                        string insertQuery = "INSERT INTO Post_Likes (Post_ID, User_ID, CreatedAt) VALUES (@PostID, @UserID, GETDATE())";
                        using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@PostID", currentPostID);
                            cmd.Parameters.AddWithValue("@UserID", SessionManager.UserID);
                            cmd.ExecuteNonQuery();
                        }

                        isLiked = true;
                        lblLikeCount.ForeColor = Color.Red;
                        guna2PictureBox9.ForeColor = Color.Red;

                        if (int.TryParse(lblLikeCount.Text, out int currentLikes))
                        {
                            lblLikeCount.Text = (currentLikes + 1).ToString();
                        }
                    }
                }
            }
            catch { }
        }

        private void guna2PictureBox9_Click1(object sender, EventArgs e)
        {
            guna2PictureBox9_Click(sender, e);
        }

        private void guna2PictureBox9_MouseEnter(object sender, EventArgs e)
        {
            if (!isLiked) guna2PictureBox9.ForeColor = Color.LightCoral;
        }

        private void guna2PictureBox9_MouseLeave(object sender, EventArgs e)
        {
            guna2PictureBox9.ForeColor = !isLiked ? Color.Gray : Color.Red;
        }

        // ==========================================
        // 2. برمجة زر الحفظ (guna2PictureBox6)
        // ==========================================
        private void guna2PictureBox6_Click_1(object sender, EventArgs e)
        {
            if (SessionManager.UserID == Guid.Empty) return;

            try
            {
                using (SqlConnection conn = DatabaseHelper.CreateConnection())
                {
                    conn.Open();

                    if (isBookmarked)
                    {
                        string deleteQuery = "DELETE FROM Post_Bookmarks WHERE Post_ID = @PostID AND User_ID = @UserID";
                        using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@PostID", currentPostID);
                            cmd.Parameters.AddWithValue("@UserID", SessionManager.UserID);
                            cmd.ExecuteNonQuery();
                        }

                        isBookmarked = false;
                        lblLikeCount2.ForeColor = Color.Gray;
                        guna2PictureBox6.ForeColor = Color.Gray;

                        if (int.TryParse(lblLikeCount2.Text, out int currentBookmarks) && currentBookmarks > 0)
                        {
                            lblLikeCount2.Text = (currentBookmarks - 1).ToString();
                        }
                    }
                    else
                    {
                        string insertQuery = "INSERT INTO Post_Bookmarks (Post_ID, User_ID, CreatedAt) VALUES (@PostID, @UserID, GETDATE())";
                        using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@PostID", currentPostID);
                            cmd.Parameters.AddWithValue("@UserID", SessionManager.UserID);
                            cmd.ExecuteNonQuery();
                        }

                        isBookmarked = true;
                        lblLikeCount2.ForeColor = Color.DodgerBlue;
                        guna2PictureBox6.ForeColor = Color.DodgerBlue;

                        if (int.TryParse(lblLikeCount2.Text, out int currentBookmarks))
                        {
                            lblLikeCount2.Text = (currentBookmarks + 1).ToString();
                        }
                    }
                }
            }
            catch { }
        }

        private void guna2PictureBox6_Cli_1(object sender, EventArgs e)
        {
            guna2PictureBox6_Click_1(sender, e);
        }

        private void guna2PictureBox6_MouseEnter(object sender, EventArgs e)
        {
            if (!isBookmarked) guna2PictureBox6.ForeColor = Color.LightBlue;
        }

        private void guna2PictureBox6_MouseLeave(object sender, EventArgs e)
        {
            guna2PictureBox6.ForeColor = !isBookmarked ? Color.Gray : Color.DodgerBlue;
        }

        // ==========================================
        // 3. برمجة زر إعادة النشر (guna2PictureBox7)
        // ==========================================
        private void guna2PictureBox7_Click(object sender, EventArgs e)
        {
            if (SessionManager.UserID == Guid.Empty) return;

            try
            {
                using (SqlConnection conn = DatabaseHelper.CreateConnection())
                {
                    conn.Open();

                    if (isReposted)
                    {
                        string deleteQuery = "DELETE FROM Post_Reposts WHERE Post_ID = @PostID AND User_ID = @UserID";
                        using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@PostID", currentPostID);
                            cmd.Parameters.AddWithValue("@UserID", SessionManager.UserID);
                            cmd.ExecuteNonQuery();
                        }

                        isReposted = false;
                        guna2PictureBox7.ForeColor = Color.Gray;
                    }
                    else
                    {
                        string insertQuery = "INSERT INTO Post_Reposts (Post_ID, User_ID, CreatedAt) VALUES (@PostID, @UserID, GETDATE())";
                        using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@PostID", currentPostID);
                            cmd.Parameters.AddWithValue("@UserID", SessionManager.UserID);
                            cmd.ExecuteNonQuery();
                        }

                        isReposted = true;
                        guna2PictureBox7.ForeColor = Color.Green;
                    }
                }
            }
            catch { }
        }

        private void guna2PictureBox7_MouseEnter(object sender, EventArgs e)
        {
            if (!isReposted) guna2PictureBox7.ForeColor = Color.LightGreen;
        }

        private void guna2PictureBox7_MouseLeave(object sender, EventArgs e)
        {
            guna2PictureBox7.ForeColor = !isReposted ? Color.Gray : Color.Green;
        }

        private void guna2PictureBox8_Click(object sender, EventArgs e)
        {
        }

        private void guna2PictureBox8_MouseEnter(object sender, EventArgs e)
        {
        }

        private void guna2PictureBox8_MouseLeave(object sender, EventArgs e)
        {
        }
    }
}
