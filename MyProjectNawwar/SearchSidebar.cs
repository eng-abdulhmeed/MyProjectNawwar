using System;
using System.Drawing;
using System.Windows.Forms;
using MyProjectNawwar.Helpers;

namespace MyProjectNawwar
{
    public partial class SearchSidebar : UserControl
    {
        private Timer _debounceTimer;
        private bool _isFollowing1 = false;
        private bool _isFollowing2 = false;

        public SearchSidebar()
        {
            InitializeComponent();

            _debounceTimer = new Timer();
            _debounceTimer.Interval = 300; // 300ms debounce
            _debounceTimer.Tick += DebounceTimer_Tick;
        }

        private void SearchSidebar_Load(object sender, EventArgs e)
        {
            // توليد الصور الرمزية للخبراء المقترحين
            if (guna2CirclePictureBox1 != null)
            {
                guna2CirclePictureBox1.Image = AvatarHelper.GenerateAvatar("د. سارة الأحمد", guna2CirclePictureBox1.Width);
            }

            if (guna2CirclePictureBox2 != null)
            {
                guna2CirclePictureBox2.Image = AvatarHelper.GenerateAvatar("أ. فيصل الشمري", guna2CirclePictureBox2.Width);
            }
        }

        // =========================================================
        // البحث أثناء الكتابة (Debounce)
        // =========================================================
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            bool hasText = !string.IsNullOrWhiteSpace(txtSearch.Text);
            if (lblClearSearch != null)
            {
                lblClearSearch.Visible = hasText;
            }

            _debounceTimer.Stop();
            _debounceTimer.Start();
        }

        private void DebounceTimer_Tick(object sender, EventArgs e)
        {
            _debounceTimer.Stop();
            ExecuteSearch(txtSearch.Text);
        }

        // =========================================================
        // البحث الفوري عند الضغط على Enter
        // =========================================================
        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                _debounceTimer.Stop();
                ExecuteSearch(txtSearch.Text);
            }
        }

        // =========================================================
        // تنفيذ عملية التصفية في الصفحة الرئيسية
        // =========================================================
        private void ExecuteSearch(string query)
        {
            Home home = this.FindForm() as Home;
            if (home != null)
            {
                // إذا لم نكن في شاشة الخلاصة الرئيسية، نعيد تفعيلها
                home.ShowHomeFeed();
                home.SearchPosts(query);
            }
        }

        // =========================================================
        // زر إلغاء التصفية
        // =========================================================
        private void lblClearSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            if (lblClearSearch != null)
            {
                lblClearSearch.Visible = false;
            }
            _debounceTimer.Stop();
            ExecuteSearch(null);
        }

        // =========================================================
        // النقر على الموضوعات المتداولة (Hashtags)
        // =========================================================
        private void Topic_Click(object sender, EventArgs e)
        {
            Label lbl = sender as Label;
            if (lbl != null)
            {
                string tag = lbl.Text.Trim();
                if (tag.Contains("#"))
                {
                    // استخراج أول وسم إذا كان هناك أكثر من وسم
                    string[] parts = tag.Split(new char[] { ' ', '·', '|' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string part in parts)
                    {
                        if (part.StartsWith("#"))
                        {
                            tag = part;
                            break;
                        }
                    }
                }

                txtSearch.Text = tag;
                _debounceTimer.Stop();
                ExecuteSearch(tag);
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "الذكاء الاصطناعي";
            _debounceTimer.Stop();
            ExecuteSearch(txtSearch.Text);
        }

        private void label9_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "الأمان السيبراني";
            _debounceTimer.Stop();
            ExecuteSearch(txtSearch.Text);
        }

        // =========================================================
        // النقر على أسماء الخبراء لعرض منشوراتهم
        // =========================================================
        private void Author1_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "سارة الأحمد";
            _debounceTimer.Stop();
            ExecuteSearch(txtSearch.Text);
        }

        private void Author2_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "فيصل الشمري";
            _debounceTimer.Stop();
            ExecuteSearch(txtSearch.Text);
        }

        // =========================================================
        // أزرار المتابعة للخبراء المقترحين
        // =========================================================
        private void btnFollow1_Click(object sender, EventArgs e)
        {
            _isFollowing1 = !_isFollowing1;
            if (_isFollowing1)
            {
                guna2Button1.Text = "✓ تتابعه";
                guna2Button1.ForeColor = Color.FromArgb(52, 211, 153);
                guna2Button1.BorderColor = Color.FromArgb(52, 211, 153);
            }
            else
            {
                guna2Button1.Text = "متابعة";
                guna2Button1.ForeColor = Color.FromArgb(78, 125, 165);
                guna2Button1.BorderColor = Color.FromArgb(78, 125, 165);
            }
        }

        private void btnFollow2_Click(object sender, EventArgs e)
        {
            _isFollowing2 = !_isFollowing2;
            if (_isFollowing2)
            {
                guna2Button2.Text = "✓ تتابعه";
                guna2Button2.ForeColor = Color.FromArgb(52, 211, 153);
                guna2Button2.BorderColor = Color.FromArgb(52, 211, 153);
            }
            else
            {
                guna2Button2.Text = "متابعة";
                guna2Button2.ForeColor = Color.FromArgb(78, 125, 165);
                guna2Button2.BorderColor = Color.FromArgb(78, 125, 165);
            }
        }

        private void lblShowMoreExperts_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "د.";
            _debounceTimer.Stop();
            ExecuteSearch(txtSearch.Text);
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void pnlTrending_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}
