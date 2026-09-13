using System;
using Microsoft.Data.SqlClient;
using MyProjectNawwar.Data;

namespace MyProjectNawwar.Models
{
    public static class SessionManager
    {
        public static Guid UserID { get; set; } = Guid.Empty;
        public static string FullName { get; set; } = string.Empty;
        public static string Email { get; set; } = string.Empty;
        public static string Username { get; set; } = string.Empty;
        public static string RoleName { get; set; } = "عضو | Contributor";
        public static int TrustScore { get; set; } = 100;
        public static int TotalPoints { get; set; } = 0;
        public static string Status { get; set; } = "Active";

        public static bool IsLoggedIn => UserID != Guid.Empty;

        // حدث يتم إطلاقه عند تغيير بيانات الجلسة لتحديث الشاشات المفتوحة تلقائياً
        public static event Action OnSessionUpdated;

        public static void NotifySessionUpdated()
        {
            OnSessionUpdated?.Invoke();
        }

        // دالة لتحميل كافة بيانات المستخدم من قاعدة البيانات
        public static bool LoadUser(Guid userId)
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.CreateConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT u.ID, u.Full_Name, u.Email, u.Trust_Score, u.Total_Points, u.Status,
                               ISNULL(r.Role_Name, 'مساهم | Contributor') AS Role_Name
                        FROM Users u
                        LEFT JOIN Roles r ON u.Role_ID = r.ID
                        WHERE u.ID = @ID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", userId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                UserID = Guid.Parse(reader["ID"].ToString());
                                FullName = reader["Full_Name"] != DBNull.Value ? reader["Full_Name"].ToString() : "";
                                Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : "";
                                
                                if (Email.Contains("@"))
                                {
                                    Username = Email.Split('@')[0];
                                }
                                else
                                {
                                    Username = FullName.Replace(" ", "_").ToLower();
                                }

                                TrustScore = reader["Trust_Score"] != DBNull.Value ? Convert.ToInt32(reader["Trust_Score"]) : 100;
                                TotalPoints = reader["Total_Points"] != DBNull.Value ? Convert.ToInt32(reader["Total_Points"]) : 0;
                                Status = reader["Status"] != DBNull.Value ? reader["Status"].ToString() : "Active";
                                RoleName = reader["Role_Name"] != DBNull.Value ? reader["Role_Name"].ToString() : "مساهم | Contributor";

                                NotifySessionUpdated();
                                return true;
                            }
                        }
                    }
                }
            }
            catch
            {
                // إذا حدث خطأ ما في الاتصال، لا نكسر البرنامج
            }
            return false;
        }

        public static void Refresh()
        {
            if (UserID != Guid.Empty)
            {
                LoadUser(UserID);
            }
        }

        public static void Logout()
        {
            UserID = Guid.Empty;
            FullName = string.Empty;
            Email = string.Empty;
            Username = string.Empty;
            RoleName = "عضو | Contributor";
            TrustScore = 0;
            TotalPoints = 0;
            Status = string.Empty;

            NotifySessionUpdated();
        }
    }
}
