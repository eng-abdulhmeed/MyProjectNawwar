using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace MyProjectNawwar.Models // تأكد من المسار
{
    public static class SessionManager
    {
        public static Guid UserID { get; set; }
        public static string FullName { get; set; }

        // يمكنك إضافة دالة لتسجيل الخروج وتفريغ البيانات لاحقاً
        public static void Logout()
        {
            UserID = Guid.Empty;
            FullName = string.Empty;
        }
    }
}
