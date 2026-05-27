using System.Windows.Forms;

namespace TTG_Tools
{
    public partial class AboutMain : Form
    {
        public AboutMain()
        {
            InitializeComponent();
            buttonClose.Click += (s, e) => Close();
        }
    }
}
