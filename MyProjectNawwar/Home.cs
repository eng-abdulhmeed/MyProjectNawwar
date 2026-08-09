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
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private void Home_Load(object sender, EventArgs e)
        {

        }


        private void sidebarMenu1_Load(object sender, EventArgs e)
        {

        }

     /*   private void sidebarMenu1_Load_1(object sender, EventArgs e, UserControl uc)
        {
            // ضع هذه الدالة داخل كود النافذة الرئيسية (Form1.cs)
           
            
                // 1. مسح أي محتوى سابق داخل اللوحة
                MainPanel.Controls.Clear();

                // 2. جعل الجزء الجديد يملأ مساحة اللوحة بالكامل
                uc.Dock = DockStyle.Fill;

                // 3. إضافة الجزء الجديد إلى اللوحة
                MainPanel.Controls.Add(uc);

                // 4. جلبه للمقدمة للتأكد من ظهوره
                uc.BringToFront();
            }*/
        
    }
}
