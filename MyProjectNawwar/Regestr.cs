using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyProjectNawwar
{
    public partial class Regestr : Form
    {
        public Regestr()
        {
            InitializeComponent();
        }

        private void SignUp_Click(object sender, EventArgs e)
        {
            // 1. جلب البيانات من الحقول (استبدل أسماء الحقول بما يطابق تصميمك)
            string name = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();

            // 2. تحقق بسيط من عدم ترك الحقول فارغة
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("يرجى تعبئة جميع الحقول المطلوبة.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 3. استدعاء الخدمة
            Services.UserService userService = new Services.UserService();
            bool isSuccess = userService.Register(name, email, password, out string responseMessage);

            if (isSuccess)
            {
                MessageBox.Show(responseMessage, "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // العودة لشاشة تسجيل الدخول تلقائياً بعد نجاح التسجيل
                this.Close(); // لأننا برمجنا حدث الإغلاق سابقاً ليظهر شاشة الـ Login
            }
            else
            {
                MessageBox.Show(responseMessage, "فشل التسجيل", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }
    }
}
