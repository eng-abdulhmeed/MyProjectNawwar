using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MyProjectNawwar.Helpers;
using MyProjectNawwar.Models;
using MyProjectNawwar.Services;

namespace MyProjectNawwar
{
    public partial class EditProfileForm : Form
    {
        private readonly UserService _userService = new UserService();

        public EditProfileForm()
        {
            InitializeComponent();
        }

        private void EditProfileForm_Load(object sender, EventArgs e)
        {
            // تعيين أيقونة وشعار المنصة
            picLogo.Image = AppAssets.Logo;
            this.Icon = AppAssets.AppIcon;

            // تعبئة الحقول بالبيانات الحالية للمستخدم من الجلسة
            txtFullName.Text = SessionManager.FullName;
            txtEmail.Text = SessionManager.Email;

            // تحديث الأفاتار الحالي
            UpdateAvatar();
        }

        private void UpdateAvatar()
        {
            string name = txtFullName.Text.Trim();
            picAvatar.Image = AvatarHelper.GenerateAvatar(name, picAvatar.Width);
        }

        private void txtFullName_TextChanged(object sender, EventArgs e)
        {
            UpdateAvatar();

            string name = txtFullName.Text.Trim();
            if (string.IsNullOrEmpty(name))
            {
                lblNameError.Text = "يرجى إدخال الاسم الكامل.";
                lblNameError.Visible = true;
                txtFullName.BorderColor = Color.FromArgb(248, 113, 113);
            }
            else if (!Regex.IsMatch(name, @"^[\p{L}\s]+$"))
            {
                lblNameError.Text = "الاسم يجب أن يحتوي على أحرف ومسافات فقط.";
                lblNameError.Visible = true;
                txtFullName.BorderColor = Color.FromArgb(248, 113, 113);
            }
            else
            {
                lblNameError.Visible = false;
                txtFullName.BorderColor = Color.FromArgb(52, 211, 153);
            }
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            if (string.IsNullOrEmpty(email))
            {
                lblEmailError.Text = "يرجى إدخال البريد الإلكتروني.";
                lblEmailError.Visible = true;
                txtEmail.BorderColor = Color.FromArgb(248, 113, 113);
            }
            else if (!Regex.IsMatch(email, emailPattern, RegexOptions.IgnoreCase))
            {
                lblEmailError.Text = "صيغة البريد الإلكتروني غير صحيحة.";
                lblEmailError.Visible = true;
                txtEmail.BorderColor = Color.FromArgb(248, 113, 113);
            }
            else
            {
                lblEmailError.Visible = false;
                txtEmail.BorderColor = Color.FromArgb(52, 211, 153);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string newPassword = txtPassword.Text.Trim();

            // التحقق النهائي من الحقول
            if (string.IsNullOrEmpty(fullName) || !Regex.IsMatch(fullName, @"^[\p{L}\s]+$"))
            {
                txtFullName.Focus();
                return;
            }

            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (string.IsNullOrEmpty(email) || !Regex.IsMatch(email, emailPattern, RegexOptions.IgnoreCase))
            {
                txtEmail.Focus();
                return;
            }

            if (!string.IsNullOrEmpty(newPassword) && newPassword.Length < 6)
            {
                MessageBox.Show("يجب ألا تقل كلمة المرور الجديدة عن 6 خانات.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            // تنفيذ التحديث في قاعدة البيانات
            bool success = _userService.UpdateProfile(SessionManager.UserID, fullName, email, newPassword, out string message);

            if (success)
            {
                MessageBox.Show(message, "تم بنجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show(message, "فشل التحديث", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
