using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.SqlClient;
using MyProjectNawwar.Data; // تأكد من استبدال YourProjectName باسم مشروعك

namespace MyProjectNawwar.Services
{
    public class UserService
    {
        // استدعاء كلاس الاتصال بقاعدة البيانات الذي أنشأناه سابقاً
        private readonly DatabaseHelper _dbHelper = new DatabaseHelper();

        // دالة تسجيل الدخول
        public bool Login(string email, string password, out string message)
        {
            // ملاحظة هندسية: في بيئة العمل الحقيقية، يجب تشفير كلمة المرور (Hashing) 
            // ومقارنة النص المشفر، لكن سنستخدم النص العادي هنا للتوضيح الأولي

            // استخدام البارامترات (@Email, @Password) لمنع ثغرات الحقن (SQL Injection)
            string query = "SELECT ID, Full_Name, Status FROM Users WHERE Email = @Email AND Password_Hash = @Password";
            using (SqlConnection conn = _dbHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);

                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows) // إذا وجد بيانات مطابقة
                            {
                                reader.Read();
                                string status = reader["Status"].ToString();

                                // التحقق من حالة الحساب (مثلاً إذا كان محظوراً أو قيد المراجعة)
                                if (status == "Blocked")
                                {
                                    message = "عذراً، هذا الحساب محظور.";
                                    return false;
                                }

                                // 🌟 التعديل الجديد: حفظ بيانات المستخدم في مدير الجلسة 🌟
                                Models.SessionManager.UserID = Guid.Parse(reader["ID"].ToString());
                                Models.SessionManager.FullName = reader["Full_Name"].ToString();

                                message = $"أهلاً بك مجدداً يا {Models.SessionManager.FullName}!";
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
        public bool Register(string fullName, string email, string password, out string message)
        {
            try
            {
                // استدعاء الاتصال من DatabaseHelper كما فعلنا في تسجيل الدخول
                using (Microsoft.Data.SqlClient.SqlConnection conn = _dbHelper.GetConnection())
                {
                    // 1. فحص ما إذا كان الإيميل موجوداً مسبقاً في النظام
                    string checkQuery = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
                    Microsoft.Data.SqlClient.SqlCommand checkCmd = new Microsoft.Data.SqlClient.SqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@Email", email);

                    conn.Open();
                    int emailExists = (int)checkCmd.ExecuteScalar(); // ترجع رقم (عدد الإيميلات المطابقة)

                    if (emailExists > 0)
                    {
                        message = "هذا البريد الإلكتروني مسجل لدينا بالفعل، جرب تسجيل الدخول.";
                        return false;
                    }

                    // 2. إدخال بيانات المستخدم الجديد إلى قاعدة البيانات
                    // ملاحظة: نستخدم NEWID() لتوليد مُعرّف فريد (GUID) تلقائياً
                    string insertQuery = "INSERT INTO Users (ID, Full_Name, Email, Password_Hash, Status) VALUES (NEWID(), @FullName, @Email, @Password, 'Active')";
                    Microsoft.Data.SqlClient.SqlCommand insertCmd = new Microsoft.Data.SqlClient.SqlCommand(insertQuery, conn);

                    insertCmd.Parameters.AddWithValue("@FullName", fullName);
                    insertCmd.Parameters.AddWithValue("@Email", email);
                    insertCmd.Parameters.AddWithValue("@Password", password); // في التطبيقات الحقيقية يُفضل تشفيرها هنا

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
            }
            catch (Exception ex)
            {
                message = "حدث خطأ أثناء الاتصال بالخادم: " + ex.Message;
                return false;
            }
        }
    }
}