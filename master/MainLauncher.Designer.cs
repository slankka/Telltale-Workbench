using System.Drawing;
using System.Windows.Forms;

namespace TTG_Tools
{
    partial class MainLauncher
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this._mainGrid = new System.Windows.Forms.TableLayoutPanel();
            this._pnlArchiveUnpacker = new System.Windows.Forms.Panel();
            this._lblTitle1 = new System.Windows.Forms.Label();
            this._lblDesc1 = new System.Windows.Forms.Label();
            this._pnlLandbEditor = new System.Windows.Forms.Panel();
            this._lblTitle2 = new System.Windows.Forms.Label();
            this._lblDesc2 = new System.Windows.Forms.Label();
            this._pnlLandbReviewer = new System.Windows.Forms.Panel();
            this._lblTitle3 = new System.Windows.Forms.Label();
            this._lblDesc3 = new System.Windows.Forms.Label();
            this._pnlScanner = new System.Windows.Forms.Panel();
            this._lblTitle4 = new System.Windows.Forms.Label();
            this._lblDesc4 = new System.Windows.Forms.Label();
            this._pnlFontCreator = new System.Windows.Forms.Panel();
            this._lblTitle5 = new System.Windows.Forms.Label();
            this._lblDesc5 = new System.Windows.Forms.Label();
            this._pnlArchivePacker = new System.Windows.Forms.Panel();
            this._lblTitle6 = new System.Windows.Forms.Label();
            this._lblDesc6 = new System.Windows.Forms.Label();
            this._pnlAutoPacker = new System.Windows.Forms.Panel();
            this._lblTitle7 = new System.Windows.Forms.Label();
            this._lblDesc7 = new System.Windows.Forms.Label();
            this._pnlSettings = new System.Windows.Forms.Panel();
            this._lblTitle8 = new System.Windows.Forms.Label();
            this._lblDesc8 = new System.Windows.Forms.Label();
            this._pnlAbout = new System.Windows.Forms.Panel();
            this._lblTitle9 = new System.Windows.Forms.Label();
            this._lblDesc9 = new System.Windows.Forms.Label();

            this._mainGrid.SuspendLayout();
            this.SuspendLayout();

            // _mainGrid
            this._mainGrid.ColumnCount = 3;
            this._mainGrid.RowCount = 3;
            this._mainGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._mainGrid.Padding = new System.Windows.Forms.Padding(20);
            for (int i = 0; i < 3; i++)
            {
                this._mainGrid.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
                this._mainGrid.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            }

            // --- Panel 1: Archive Unpacker ---
            SetupCard(this._pnlArchiveUnpacker, this._lblTitle1, this._lblDesc1,
                "Archive Unpacker", "Extract .ttarch / .ttarch2 archives", 0, 0);
            this._pnlArchiveUnpacker.Click += (s, e) => OpenTool<ArchiveUnpacker>();
            // --- Panel 2: Landb Editor ---
            SetupCard(this._pnlLandbEditor, this._lblTitle2, this._lblDesc2,
                "Landb Editor", "Edit & normalize .landb translation files", 0, 1);
            this._pnlLandbEditor.Click += (s, e) => OpenTool<LandbEditor>();
            // --- Panel 3: Landb Reviewer ---
            SetupCard(this._pnlLandbReviewer, this._lblTitle3, this._lblDesc3,
                "Landb Reviewer", "Side-by-side translation comparison", 0, 2);
            this._pnlLandbReviewer.Click += (s, e) => OpenTool<LandbReviewer>();
            // --- Panel 4: Ttarch2 Scanner ---
            SetupCard(this._pnlScanner, this._lblTitle4, this._lblDesc4,
                "Ttarch2 Scanner (beta)", "Scan and analyze archive structures", 1, 0);
            this._pnlScanner.Click += (s, e) => OpenTool<Ttarch2Scanner>();
            // --- Panel 5: Font Creator ---
            SetupCard(this._pnlFontCreator, this._lblTitle5, this._lblDesc5,
                "Font Creator", "Create & edit .font texture files", 1, 1);
            this._pnlFontCreator.Click += (s, e) => OpenTool<FontCreator>();
            // --- Panel 6: Archive Packer ---
            SetupCard(this._pnlArchivePacker, this._lblTitle6, this._lblDesc6,
                "Archive Packer", "Create .ttarch / .ttarch2 archives", 1, 2);
            this._pnlArchivePacker.Click += (s, e) => OpenTool<ArchivePacker>();
            // --- Panel 7: Auto (De)Packer ---
            SetupCard(this._pnlAutoPacker, this._lblTitle7, this._lblDesc7,
                "Auto (De)Packer", "Decrypt, unpack & repack game assets (LANGDB, D3DTX, LANDb, Vector Fonts, Lua)", 2, 0);
            this._pnlAutoPacker.Click += (s, e) => OpenTool<AutoPacker>();
            // --- Panel 8: Settings ---
            SetupCard(this._pnlSettings, this._lblTitle8, this._lblDesc8,
                "Settings", "Configure tool settings & preferences", 2, 1);
            this._pnlSettings.Click += (s, e) => OpenSettings();
            // --- Panel 9: About ---
            SetupCard(this._pnlAbout, this._lblTitle9, this._lblDesc9,
                "About", "Version info & credits", 2, 2);
            this._pnlAbout.Click += (s, e) =>
            {
                var about = new AboutMain();
                about.ShowDialog(this);
            };

            // MainLauncher
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 540);
            this.Controls.Add(this._mainGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainLauncher";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Telltale Workbench";

            this._mainGrid.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private void SetupCard(Panel panel, Label title, Label desc, string titleText, string descText, int row, int col)
        {
            panel.BackColor = System.Drawing.Color.FromArgb(245, 248, 252);
            panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panel.Cursor = System.Windows.Forms.Cursors.Hand;
            panel.Margin = new System.Windows.Forms.Padding(6);
            panel.Dock = System.Windows.Forms.DockStyle.Fill;

            title.Text = titleText;
            title.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            title.ForeColor = System.Drawing.Color.FromArgb(30, 60, 120);
            title.Location = new System.Drawing.Point(12, 10);
            title.AutoSize = true;

            desc.Text = descText;
            desc.Font = new System.Drawing.Font("Tahoma", 8.25F);
            desc.ForeColor = System.Drawing.Color.FromArgb(100, 100, 100);
            desc.Location = new System.Drawing.Point(12, 36);
            desc.AutoSize = true;
            desc.MaximumSize = new System.Drawing.Size(210, 0);

            panel.Controls.Add(title);
            panel.Controls.Add(desc);

            this._mainGrid.Controls.Add(panel, col, row);
        }

        private System.Windows.Forms.TableLayoutPanel _mainGrid;
        private System.Windows.Forms.Panel _pnlArchiveUnpacker, _pnlLandbEditor, _pnlLandbReviewer,
            _pnlScanner, _pnlFontCreator, _pnlArchivePacker, _pnlAutoPacker, _pnlSettings, _pnlAbout;
        private System.Windows.Forms.Label _lblTitle1, _lblTitle2, _lblTitle3, _lblTitle4, _lblTitle5,
            _lblTitle6, _lblTitle7, _lblTitle8, _lblTitle9;
        private System.Windows.Forms.Label _lblDesc1, _lblDesc2, _lblDesc3, _lblDesc4, _lblDesc5,
            _lblDesc6, _lblDesc7, _lblDesc8, _lblDesc9;
    }
}
