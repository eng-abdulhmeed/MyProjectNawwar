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
    public partial class Regestr : Form
    {
        // ألوان الحالات (خطأ - نجاح - الحدود الافتراضية)
        private readonly Color ErrorColor = Color.FromArgb(248, 113, 113); // أحمر أنيق وواضح
        private readonly Color SuccessColor = Color.FromArgb(52, 211, 153); // أخضر أنيق
        private readonly Color DefaultBorderColor = Color.FromArgb(35, 53, 77); // الإطار الافتراضي

        public Regestr()
        {
            InitializeComponent();
            this.Icon = Helpers.AppAssets.AppIcon;
            if (Helpers.AppAssets.Logo != null)
            {
                picLogoLeft.Image = Helpers.AppAssets.Logo;
                picLogoRight.Image = Helpers.AppAssets.Logo;
            }
        }

        // دالة مساعدة لإظهار الخطأ وتلوين إطار الحقل بالأحمر
        private void ShowError(Label lbl, Guna.UI2.WinForms.Guna2TextBox txt, string message)
        {
            lbl.Text = message;
            lbl.Visible = true;
            txt.BorderColor = ErrorColor;
        }

        // دالة مساعدة لإخفاء الخطأ وتلوين الإطار بالأخضر عند الصحة
        private void ClearError(Label lbl, Guna.UI2.WinForms.Guna2TextBox txt, bool isSuccess = true)
        {
            lbl.Text = "";
            lbl.Visible = false;
            txt.BorderColor = isSuccess ? SuccessColor : DefaultBorderColor;
        }

        // --- 1. التحقق الفوري من الاسم الكامل ---
        private bool ValidateFullName(bool isTyping = false)
        {
            string name = txtFullName.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                if (!isTyping)
                {
                    ShowError(lblFullNameError, txtFullName, "يرجى إدخال الاسم الكامل.");
                    return false;
                }
                ClearError(lblFullNameError, txtFullName, false);
                return false;
            }

            // التأكد من أن الاسم يحتوي على أحرف فقط (عربية أو إنجليزية) ومسافات
            if (!Regex.IsMatch(name, @"^[\p{L}\s'-]+$"))
            {
                ShowError(lblFullNameError, txtFullName, "يجب أن يحتوي الاسم على أحرف فقط بدون أرقام.");
                return false;
            }

            if (name.Length < 3)
            {
                ShowError(lblFullNameError, txtFullName, "يجب أن يتكون الاسم من 3 أحرف على الأقل.");
                return false;
            }

            ClearError(lblFullNameError, txtFullName, true);
            return true;
        }

        // منع إدخال الأرقام والرموز في خانة الاسم لحظياً
        private void txtFullName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '\'' && e.KeyChar != '-')
            {
                e.Handled = true; // منع الإدخال
            }
        }

        // --- 2. التحقق الفوري من البريد الإلكتروني ---
        private bool ValidateEmail(bool isTyping = false)
        {
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrWhiteSpace(email))
            {
                if (!isTyping)
                {
                    ShowError(lblEmailError, txtEmail, "يرجى إدخال البريد الإلكتروني.");
                    return false;
                }
                ClearError(lblEmailError, txtEmail, false);
                return false;
            }

            // صيغة قياسية عامة لقبول جميع أنواع الإيميلات الصحيحة (Gmail, Yahoo, Outlook, Nawwar, إلخ)
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (!Regex.IsMatch(email, emailPattern, RegexOptions.IgnoreCase))
            {
                if (!email.Contains("@"))
                {
                    ShowError(lblEmailError, txtEmail, "صيغة غير مكتملة، يجب أن يحتوي على @ ومزود الخدمة.");
                }
                else if (!email.Substring(email.IndexOf('@')).Contains("."))
                {
                    ShowError(lblEmailError, txtEmail, "يرجى إكمال نطاق البريد (مثل: .com أو .net).");
                }
                else
                {
                    ShowError(lblEmailError, txtEmail, "صيغة البريد الإلكتروني غير صالحة (مثال: name@example.com).");
                }
                return false;
            }

            ClearError(lblEmailError, txtEmail, true);
            return true;
        }

        // --- 3. التحقق الفوري من رقم الهاتف ---
        private bool ValidatePhone(bool isTyping = false)
        {
            string phone = txtPhone.Text.Trim();

            if (string.IsNullOrWhiteSpace(phone))
            {
                if (!isTyping)
                {
                    ShowError(lblPhoneError, txtPhone, "يرجى إدخال رقم الهاتف.");
                    return false;
                }
                ClearError(lblPhoneError, txtPhone, false);
                return false;
            }

            // فحص أنه يحتوي على أرقام فقط مع إمكانية البدء بـ + فقط
            if (!Regex.IsMatch(phone, @"^\+?[0-9]+$"))
            {
                ShowError(lblPhoneError, txtPhone, "يجب إدخال أرقام فقط، ويمكن البدء بعلامة + فقط.");
                return false;
            }

            string digitsOnly = phone.Replace("+", "");
            if (digitsOnly.Length < 7 || digitsOnly.Length > 15)
            {
                ShowError(lblPhoneError, txtPhone, "يجب أن يتكون رقم الهاتف من 7 إلى 15 رقماً.");
                return false;
            }

            ClearError(lblPhoneError, txtPhone, true);
            return true;
        }

        // منع إدخال أي أحرف أو رموز في خانة الهاتف، والسماح بعلامة + فقط في البداية
        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar))
            {
                return;
            }

            // السماح بعلامة + فقط في البداية ولمرة واحدة
            if (e.KeyChar == '+')
            {
                if (txtPhone.SelectionStart == 0 && !txtPhone.Text.Contains("+"))
                {
                    return;
                }
                e.Handled = true;
                return;
            }

            // السماح بالأرقام فقط
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        // --- 4. التحقق الفوري من كلمة المرور ---
        private bool ValidatePassword(bool isTyping = false)
        {
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(password))
            {
                if (!isTyping)
                {
                    ShowError(lblPasswordError, txtPassword, "يرجى إدخال كلمة المرور.");
                    return false;
                }
                ClearError(lblPasswordError, txtPassword, false);
                return false;
            }

            if (password.Length < 6)
            {
                ShowError(lblPasswordError, txtPassword, "يجب ألا تقل كلمة المرور عن 6 خانات.");
                return false;
            }

            ClearError(lblPasswordError, txtPassword, true);

            // عند تغيير كلمة المرور، أعد فحص تأكيد كلمة المرور إذا كان يحتوي نصاً
            if (!string.IsNullOrEmpty(txtConfirm.Text))
            {
                ValidateConfirmPassword(true);
            }

            return true;
        }

        // --- 5. التحقق الفوري من تأكيد كلمة المرور ---
        private bool ValidateConfirmPassword(bool isTyping = false)
        {
            string confirm = txtConfirm.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(confirm))
            {
                if (!isTyping)
                {
                    ShowError(lblConfirmError, txtConfirm, "يرجى تأكيد كلمة المرور.");
                    return false;
                }
                ClearError(lblConfirmError, txtConfirm, false);
                return false;
            }

            if (confirm != password)
            {
                ShowError(lblConfirmError, txtConfirm, "كلمتا المرور غير متطابقتين.");
                return false;
            }

            ClearError(lblConfirmError, txtConfirm, true);
            return true;
        }

        // --- أحداث التغيير المباشر أثناء الكتابة (Real-Time TextChanged) ---
        private void txtFullName_TextChanged(object sender, EventArgs e)
        {
            ValidateFullName(true);
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            ValidateEmail(true);
        }

        private void txtPhone_TextChanged(object sender, EventArgs e)
        {
            ValidatePhone(true);
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            ValidatePassword(true);
        }

        private void txtConfirm_TextChanged(object sender, EventArgs e)
        {
            ValidateConfirmPassword(true);
        }

        // --- زر إنشاء الحساب (Sign Up) ---
        private void SignUp_Click(object sender, EventArgs e)
        {
            // فحص شامل لجميع الحقول مع إظهار الأخطاء تحت أي حقل غير مكتمل
            bool isNameOk = ValidateFullName(false);
            bool isEmailOk = ValidateEmail(false);
            bool isPhoneOk = ValidatePhone(false);
            bool isPassOk = ValidatePassword(false);
            bool isConfirmOk = ValidateConfirmPassword(false);

            if (!isNameOk || !isEmailOk || !isPhoneOk || !isPassOk || !isConfirmOk)
            {
                // التركيز على أول حقل فيه خطأ مباشرة بدون رسائل منبثقة مزعجة
                if (!isNameOk) txtFullName.Focus();
                else if (!isEmailOk) txtEmail.Focus();
                else if (!isPhoneOk) txtPhone.Focus();
                else if (!isPassOk) txtPassword.Focus();
                else if (!isConfirmOk) txtConfirm.Focus();

                return;
            }

            string name = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            // استدعاء خدمة تسجيل المستخدم
            Services.UserService userService = new Services.UserService();
            bool isSuccess = userService.Register(name, email, password, out string responseMessage);

            if (isSuccess)
            {
                MessageBox.Show(responseMessage, "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // العودة لشاشة تسجيل الدخول تلقائياً بعد نجاح التسجيل
                this.Close();
            }
            else
            {
                // إذا كان البريد مسجلاً مسبقاً، نظهر التنبيه تحت حقل البريد مباشرة
                if (responseMessage.Contains("مسجل لدينا بالفعل"))
                {
                    ShowError(lblEmailError, txtEmail, responseMessage);
                    txtEmail.Focus();
                }
                else
                {
                    MessageBox.Show(responseMessage, "فشل التسجيل", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // العودة لشاشة تسجيل الدخول عند النقر على Log in
            this.Close();
        }
    }
}
