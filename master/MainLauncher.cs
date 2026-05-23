using System;
using System.Linq;
using System.Windows.Forms;

namespace TTG_Tools
{
    public partial class MainLauncher : Form
    {
        public MainLauncher()
        {
            InitializeComponent();
            WireCardEvents();
        }

        private void WireCardEvents()
        {
            WireCard(_pnlArchiveUnpacker, () => OpenTool<ArchiveUnpacker>());
            WireCard(_pnlLandbEditor, () => OpenTool<LandbEditor>());
            WireCard(_pnlLandbReviewer, () => OpenTool<LandbReviewer>());
            WireCard(_pnlScanner, () => OpenTool<Ttarch2Scanner>());
            WireCard(_pnlSettings, () => OpenTool<FontEditor>());
            WireCard(_pnlArchivePacker, () => OpenTool<ArchivePacker>());
            WireCard(_pnlAutoPacker, () => OpenTool<AutoPacker>());
            WireCard(_pnlQuickTools, () => OpenSettings());
            WireCard(_pnlAbout, () =>
            {
                var about = new About();
                about.ShowDialog(this);
            });
        }

        private bool _suppressCardClick;

        private void WireCard(Panel panel, Action action)
        {
            panel.MouseDown += (s, e) => { if (!_suppressCardClick) { _suppressCardClick = true; action(); } };
            panel.MouseUp += (s, e) => _suppressCardClick = false;
            foreach (Control c in panel.Controls)
            {
                c.MouseDown += (s, e) => { if (!_suppressCardClick) { _suppressCardClick = true; action(); } };
                c.MouseUp += (s, e) => _suppressCardClick = false;
            }
        }

        private void OpenTool<T>() where T : Form, new()
        {
            if (Application.OpenForms.OfType<T>().Any()) return;
            var form = new T();
            form.Show();
        }

        private void OpenSettings()
        {
            if (Application.OpenForms.OfType<FormSettings>().Any()) return;
            var settings = new FormSettings();
            settings.Show(this);
        }
    }
}
