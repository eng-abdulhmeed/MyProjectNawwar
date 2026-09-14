using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace MyProjectNawwar
{
    public partial class Log_in : Form
    {
        public Log_in()
        {
            InitializeComponent();
            this.Icon = Helpers.AppAssets.AppIcon;
            if (Helpers.AppAssets.Logo != null)
            {
                picLogoLeft.Image = Helpers.AppAssets.Logo;
                picLogoRight.Image = Helpers.AppAssets.Logo;
            }
        }

        
        private void LogIn_Click(object sender, EventArgs e)
        {
            // أخذ القيم من الحقول النصية في الواجهة
            // استبدل txtEmail و txtPassword بأسماء الحقول الفعلية في تصميمك
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            // التحقق من أن الحقول ليست فارغة قبل إرسالها لقاعدة البيانات
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("يرجى إدخال البريد الإلكتروني وكلمة المرور.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // استدعاء دالة تسجيل الدخول من طبقة الخدمات
            Services.UserService userService = new Services.UserService();

            // استدعاء الدالة وتمرير المتغيرات
            bool isSuccess = userService.Login(email, password, out string responseMessage);

            if (isSuccess)
            {
                MessageBox.Show(responseMessage, "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 1. أخذ نسخة من شاشة الرئيسية
                Home homeForm = new Home();

                // 2. برمجة حدث إغلاق التطبيق بالكامل عند إغلاق شاشة الهوم
                homeForm.FormClosed += (s, args) => this.Close();

                // 3. إظهار شاشة الهوم
                homeForm.Show();

                // 4. إخفاء شاشة تسجيل الدخول الحالية من الواجهة
                this.Hide();
            }
            else
            {
                MessageBox.Show(responseMessage, "فشل تسجيل الدخول", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
                // 1. فتح شاشة إنشاء الحساب
                Regestr signUpForm = new Regestr();

                // 2. جعل شاشة تسجيل الدخول تظهر مجدداً عندما يغلق المستخدم شاشة التسجيل
                signUpForm.FormClosed += (s, args) => this.Show();

                // 3. إظهار شاشة التسجيل وإخفاء شاشة تسجيل الدخول الحالية
                signUpForm.Show();
                this.Hide();
            
        }

        private void pnlLeft_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void pnlRight_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
