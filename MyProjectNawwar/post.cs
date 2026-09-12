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
    public partial class post : UserControl
    {
        public post()
        {
            InitializeComponent();
        }

        // --- المتغيرات ---
        private int currentPostID; // رقم المنشور الحالي
        private bool isLiked = false; // متغير يحفظ حالة الإعجاب
        private bool isBookmarked = false; // متغير يحفظ حالة الحفظ
        private bool isReposted = false; // متغير يحفظ حالة إعادة النشر

        // --- دالة تعبئة بيانات المنشور ---
        public void SetPostData(int postID, string name, string username, string timeAgo, string title, string bodyText)
        {
            currentPostID = postID;

            guna2HtmlLabel25.Text = name;             // Dr. Smith
            guna2HtmlLabel24.Text = username;         // @dr.smith
            guna2HtmlLabel23.Text = timeAgo;          // 2h
            guna2HtmlLabel17.Text = title;            // The Future of AI
            guna2HtmlLabel16.Text = bodyText;
        }

        // ==========================================
        // 1. برمجة زر الإعجاب (guna2PictureBox9)
        // ==========================================


        private void guna2PictureBox9_Click1(object sender, EventArgs e)
        {
            string connectionString = @"Server=.;Database=NawwarSystemDB;Trusted_Connection=True;Encrypt=False;";
            string loggedInUserId = "11111111-1111-1111-1111-111111111111";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                if (isLiked)
                {
                    string deleteQuery = "DELETE FROM Post_Likes WHERE Post_ID = @PostID AND User_ID = @UserID";
                    using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@PostID", currentPostID);
                        cmd.Parameters.AddWithValue("@UserID", loggedInUserId);
                        cmd.ExecuteNonQuery();
                    }

                    isLiked = false;
                    guna2PictureBox9.ForeColor = Color.Gray;

                    int currentLikes = int.Parse(lblLikeCount.Text);
                    lblLikeCount.Text = (currentLikes - 1).ToString();
                }
                else
                {
                    string insertQuery = "INSERT INTO Post_Likes (Post_ID, User_ID) VALUES (@PostID, @UserID)";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@PostID", currentPostID);
                        cmd.Parameters.AddWithValue("@UserID", loggedInUserId);
                        cmd.ExecuteNonQuery();
                    }

                    isLiked = true;
                    guna2PictureBox9.ForeColor = Color.Red;

                    int currentLikes = int.Parse(lblLikeCount.Text);
                    lblLikeCount.Text = (currentLikes + 1).ToString();


                }
            }
        }
        private void guna2PictureBox9_Click(object sender, EventArgs e)
        {
            string connectionString = @"Server=.;Database=NawwarSystemDB;Trusted_Connection=True;Encrypt=False;";
            string loggedInUserId = "11111111-1111-1111-1111-111111111111";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                if (isLiked)
                {
                    // --- كود إلغاء الإعجاب ---
                    string deleteQuery = "DELETE FROM Post_Likes WHERE Post_ID = @PostID AND User_ID = @UserID";
                    using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@PostID", currentPostID);
                        cmd.Parameters.AddWithValue("@UserID", loggedInUserId);
                        cmd.ExecuteNonQuery();
                    }

                    isLiked = false;

                    // التأثير البصري: إعادة الرقم للون الرمادي (أو الأبيض حسب تصميمك) وإنقاص العدد
                    lblLikeCount.ForeColor = Color.Gray;
                    int currentLikes = int.Parse(lblLikeCount.Text);
                    lblLikeCount.Text = (currentLikes - 1).ToString();
                }
                else
                {
                    // --- كود إضافة الإعجاب ---
                    string insertQuery = "INSERT INTO Post_Likes (Post_ID, User_ID) VALUES (@PostID, @UserID)";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@PostID", currentPostID);
                        cmd.Parameters.AddWithValue("@UserID", loggedInUserId);
                        cmd.ExecuteNonQuery();
                    }

                    isLiked = true;

                    // التأثير البصري: تحويل لون الرقم للأحمر ليعطي إحساساً بالتفاعل، وزيادة العدد
                    lblLikeCount.ForeColor = Color.Red;
                    int currentLikes = int.Parse(lblLikeCount.Text);
                    lblLikeCount.Text = (currentLikes + 1).ToString();
                }
            }
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
            string connectionString = @"Server=.;Database=NawwarSystemDB;Trusted_Connection=True;Encrypt=False;";
            string loggedInUserId = "11111111-1111-1111-1111-111111111111";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                if (isBookmarked)
                {
                    string deleteQuery = "DELETE FROM Post_Bookmarks WHERE Post_ID = @PostID AND User_ID = @UserID";
                    using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@PostID", currentPostID);
                        cmd.Parameters.AddWithValue("@UserID", loggedInUserId);
                        cmd.ExecuteNonQuery();
                    }

                    isBookmarked = false;
                    lblLikeCount2.ForeColor = Color.Gray;
                    int currentLikes = int.Parse(lblLikeCount2.Text);
                    lblLikeCount2.Text = (currentLikes - 1).ToString();
                    //guna2PictureBox6.ForeColor = Color.Gray;
                }
                else
                {
                    string insertQuery = "INSERT INTO Post_Bookmarks (Post_ID, User_ID) VALUES (@PostID, @UserID)";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@PostID", currentPostID);
                        cmd.Parameters.AddWithValue("@UserID", loggedInUserId);
                        cmd.ExecuteNonQuery();
                    }

                    isBookmarked = true;
                    // التأثير البصري: تحويل لون الرقم للأحمر ليعطي إحساساً بالتفاعل، وزيادة العدد
                    lblLikeCount2.ForeColor = Color.DodgerBlue;
                    int currentLikes = int.Parse(lblLikeCount2.Text);
                    lblLikeCount2.Text = (currentLikes + 1).ToString();
                    //guna2PictureBox6.ForeColor = Color.DodgerBlue;
                }
            }
        }
        private void guna2PictureBox6_Cli_1(object sender, EventArgs e)
        {
            string connectionString = @"Server=.;Database=NawwarSystemDB;Trusted_Connection=True;Encrypt=False;";
            string loggedInUserId = "11111111-1111-1111-1111-111111111111";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                if (isLiked)
                {
                    // --- كود إلغاء الإعجاب ---
                    string deleteQuery = "DELETE FROM Post_Likes WHERE Post_ID = @PostID AND User_ID = @UserID";
                    using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@PostID", currentPostID);
                        cmd.Parameters.AddWithValue("@UserID", loggedInUserId);
                        cmd.ExecuteNonQuery();
                    }

                    isLiked = false;

                    // التأثير البصري: إعادة الرقم للون الرمادي (أو الأبيض حسب تصميمك) وإنقاص العدد
                    lblLikeCount2.ForeColor = Color.Gray;
                    int currentLikes = int.Parse(lblLikeCount2.Text);
                    lblLikeCount2.Text = (currentLikes - 1).ToString();
                }
                else
                {
                    // --- كود إضافة الإعجاب ---
                    string insertQuery = "INSERT INTO Post_Likes (Post_ID, User_ID) VALUES (@PostID, @UserID)";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@PostID", currentPostID);
                        cmd.Parameters.AddWithValue("@UserID", loggedInUserId);
                        cmd.ExecuteNonQuery();
                    }

                    isLiked = true;

                    // التأثير البصري: تحويل لون الرقم للأحمر ليعطي إحساساً بالتفاعل، وزيادة العدد
                    lblLikeCount2.ForeColor = Color.DodgerBlue;
                    int currentLikes = int.Parse(lblLikeCount2.Text);
                    lblLikeCount2.Text = (currentLikes + 1).ToString();
                }
            }
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
            string connectionString = @"Server=.;Database=NawwarSystemDB;Trusted_Connection=True;Encrypt=False;";
            string loggedInUserId = "11111111-1111-1111-1111-111111111111";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                if (isReposted)
                {
                    string deleteQuery = "DELETE FROM Post_Reposts WHERE Post_ID = @PostID AND User_ID = @UserID";
                    using (SqlCommand cmd = new SqlCommand(deleteQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@PostID", currentPostID);
                        cmd.Parameters.AddWithValue("@UserID", loggedInUserId);
                        cmd.ExecuteNonQuery();
                    }

                    isReposted = false;
                    guna2PictureBox7.ForeColor = Color.Gray;
                }
                else
                {
                    string insertQuery = "INSERT INTO Post_Reposts (Post_ID, User_ID) VALUES (@PostID, @UserID)";
                    using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@PostID", currentPostID);
                        cmd.Parameters.AddWithValue("@UserID", loggedInUserId);
                        cmd.ExecuteNonQuery();
                    }

                    isReposted = true;
                    guna2PictureBox7.ForeColor = Color.Green;
                }
            }
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
 

