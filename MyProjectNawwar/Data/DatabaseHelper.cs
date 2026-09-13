using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace MyProjectNawwar.Data
{
    public class DatabaseHelper
    {
        // سلسلة الاتصال المعتمدة لكافة طبقات المشروع
        public const string ConnectionString = "Server=DESKTOP-S4DSEJ1;Database=NawwarSystemDB;Trusted_Connection=True;TrustServerCertificate=True;Encrypt=False;";

        public SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        public static SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}