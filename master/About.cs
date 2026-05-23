using System;
using System.Windows.Forms;

namespace TTG_Tools
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            buttonClose.Focus();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
