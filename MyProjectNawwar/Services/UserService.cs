using System;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;
using MyProjectNawwar.Data;
using MyProjectNawwar.Models;

using System.Data;




namespace MyProjectNawwar.Services
{
    public class UserService
    {
        private readonly DatabaseHelper _dbHelper = new DatabaseHelper();

        // دالة تسجيل الدخول
        public bool Login(string email, string password, out string message)
        {
            string query = @"
                SELECT u.ID, u.Full_Name, u.Email, u.Status
                FROM Users u
                WHERE u.Email = @Email AND u.Password_Hash = @Password";

            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email.Trim());
                    cmd.Parameters.AddWithValue("@Password", password);

                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string status = reader["Status"] != DBNull.Value ? reader["Status"].ToString() : "Active";

                                if (status == "Blocked")
                                {
                                    message = "عذراً، هذا الحساب محظور.";
                                    return false;
                                }

                                Guid userId = Guid.Parse(reader["ID"].ToString());
                                reader.Close();

                                // تحميل كافة بيانات الجلسة
                                SessionManager.LoadUser(userId);

                                message = $"أهلاً بك مجدداً يا {SessionManager.FullName}!";
                                return true;
                            }
                            else
                            {
                                message = "البريد الإلكتروني أو كلمة المرور غير صحيحة.";
                                return false;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        message = "حدث خطأ أثناء الاتصال بالخادم: " + ex.Message;
                        return false;
                    }
                }
            }
        }

        // دالة إنشاء حساب جديد
        public bool Register(string fullName, string email, string password, out string message)
        {
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email.Trim(), emailPattern, RegexOptions.IgnoreCase))
            {
                message = "صيغة البريد الإلكتروني غير صحيحة.";
                return false;
            }

            try
            {
                using (SqlConnection conn = _dbHelper.GetConnection())
                {
                    conn.Open();

                    // 1. فحص ما إذا كان البريد موجوداً مسبقاً
                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@Email", email.Trim());
                        int emailExists = (int)checkCmd.ExecuteScalar();

                        if (emailExists > 0)
                        {
                            message = "هذا البريد الإلكتروني مسجل لدينا بالفعل، جرب تسجيل الدخول.";
                            return false;
                        }
                    }

                    // 2. إدخال بيانات المستخدم الجديد

                    // 2. إدخال بيانات المستخدم الجديد
                    string insertQuery = @"
    INSERT INTO Users (ID, Full_Name, Email, Password_Hash, Role_ID, Trust_Score, Total_Points, Status)
    VALUES (NEWID(), @FullName, @Email, @Password, @RoleID, 100, 0, 'Active')";

                    using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@FullName", fullName.Trim());
                        insertCmd.Parameters.AddWithValue("@Email", email.Trim());
                        insertCmd.Parameters.AddWithValue("@Password", password);

                        // تمرير رقم الصلاحية الصحيح للمستخدم العادي (تأكد أن رقمه 2 في قاعدة البيانات)
                        insertCmd.Parameters.AddWithValue("@RoleID", 2);

                        int rowsAffected = insertCmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            message = "تم إنشاء الحساب بنجاح! يمكنك الآن تسجيل الدخول.";
                            return true;
                        }
                        else
                        {
                            message = "حدث خطأ غير متوقع ولم يتم حفظ البيانات.";
                            return false;
                        }
                    }
                    /* string insertQuery = @"
                         INSERT INTO Users (ID, Full_Name, Email, Password_Hash, Role_ID, Trust_Score, Total_Points, Status)
                         VALUES (NEWID(), @FullName, @Email, @Password, 4, 100, 0, 'Active')";

                     using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                     {
                         insertCmd.Parameters.AddWithValue("@FullName", fullName.Trim());
                         insertCmd.Parameters.AddWithValue("@Email", email.Trim());
                         insertCmd.Parameters.AddWithValue("@Password", password);

                         int rowsAffected = insertCmd.ExecuteNonQuery();

                         if (rowsAffected > 0)
                         {
                             message = "تم إنشاء الحساب بنجاح! يمكنك الآن تسجيل الدخول.";
                             return true;
                         }
                         else
                         {
                             message = "حدث خطأ غير متوقع ولم يتم حفظ البيانات.";
                             return false;
                         }
                     }*/
                }
            }
            catch (Exception ex)
            {
                message = "حدث خطأ أثناء الاتصال بالخادم: " + ex.Message;
                return false;
            }
        }

        // دالة تحديث بيانات الملف الشخصي (Edit Profile)




        public bool UpdateProfile(
            Guid userId,
            string fullName,
            string email,
            string newPassword,
            out string message)
        {
            try
            {
                using (SqlConnection conn = _dbHelper.GetConnection())
                using (SqlCommand cmd = new SqlCommand(
                    "UpdateUserProfile", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@ID", SqlDbType.UniqueIdentifier)
                        .Value = userId;

                    cmd.Parameters.Add("@Full_Name", SqlDbType.NVarChar, 100)
                        .Value = fullName.Trim();

                    cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100)
                        .Value = email.Trim();

                    conn.Open();

                    int rows = cmd.ExecuteNonQuery();

                    if (rows > 0)
                    {
                        SessionManager.LoadUser(userId);

                        message = "تم تحديث بيانات الملف الشخصي بنجاح!";
                        return true;
                    }

                    message = "لم يتم العثور على الحساب.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "حدث خطأ أثناء التحديث: " + ex.Message;
                return false;
            }
        }







        /*   public bool UpdateProfile(Guid userId, string fullName, string email, string newPassword, out string message)
           {
               if (string.IsNullOrWhiteSpace(fullName))
               {
                   message = "يرجى إدخال الاسم الكامل.";
                   return false;
               }

               string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
               if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email.Trim(), emailPattern, RegexOptions.IgnoreCase))
               {
                   message = "صيغة البريد الإلكتروني غير صحيحة.";
                   return false;
               }

               try
               {
                   using (SqlConnection conn = _dbHelper.GetConnection())
                   {
                       conn.Open();

                       // 1. التأكد من أن البريد الجديد غير مستخدم من حساب آخر
                       string checkEmailQuery = "SELECT COUNT(*) FROM Users WHERE Email = @Email AND ID <> @ID";
                       using (SqlCommand checkCmd = new SqlCommand(checkEmailQuery, conn))
                       {
                           checkCmd.Parameters.AddWithValue("@Email", email.Trim());
                           checkCmd.Parameters.AddWithValue("@ID", userId);

                           int count = (int)checkCmd.ExecuteScalar();
                           if (count > 0)
                           {
                               message = "عذراً، هذا البريد الإلكتروني مستخدم لحساب آخر.";
                               return false;
                           }
                       }

                       // 2. تحديث البيانات
                       string updateQuery;
                       if (!string.IsNullOrWhiteSpace(newPassword))
                       {
                           updateQuery = @"
                               UPDATE Users
                               SET Full_Name = @FullName,
                                   Email = @Email,
                                   Password_Hash = @Password
                               WHERE ID = @ID";
                       }
                       else
                       {
                           updateQuery = @"
                               UPDATE Users
                               SET Full_Name = @FullName,
                                   Email = @Email
                               WHERE ID = @ID";
                       }

                       using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                       {
                           updateCmd.Parameters.AddWithValue("@FullName", fullName.Trim());
                           updateCmd.Parameters.AddWithValue("@Email", email.Trim());
                           updateCmd.Parameters.AddWithValue("@ID", userId);

                           if (!string.IsNullOrWhiteSpace(newPassword))
                           {
                               updateCmd.Parameters.AddWithValue("@Password", newPassword);
                           }

                           int rows = updateCmd.ExecuteNonQuery();
                           if (rows > 0)
                           {
                               // تحديث الجلسة بالبيانات الجديدة مباشرة
                               SessionManager.LoadUser(userId);
                               message = "تم تحديث بيانات الملف الشخصي بنجاح!";
                               return true;
                           }
                           else
                           {
                               message = "لم يتم العثور على الحساب المطلوب تحديثه.";
                               return false;
                           }
                       }
                   }
               }
               catch (Exception ex)
               {
                   message = "حدث خطأ أثناء تحديث البيانات: " + ex.Message;
                   return false;
               }
           }*/
    }
}