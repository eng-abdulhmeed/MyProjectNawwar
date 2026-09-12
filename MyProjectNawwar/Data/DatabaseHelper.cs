using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace MyProjectNawwar.Data
{
    public class DatabaseHelper
    {
        // تم إضافة TrustServerCertificate=True و Encrypt=False لحل مشكلة SSL نهائياً
        // تذكر: ضع اسم السيرفر الخاص بك بدلاً من YOUR_SERVER_NAME
        private readonly string _connectionString = "Server=TARIQSWEAR;Database=NawwarSystemDB;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;"; public SqlConnection GetConnection()
        {
            // نكتفي بتجهيز الاتصال وإرساله مغلقاً، وطبقة Services هي من تتولى فتحه وإغلاقه
            return new SqlConnection(_connectionString);
        }
    }
}