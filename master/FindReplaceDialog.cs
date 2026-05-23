using System;
using System.Drawing;
using System.Windows.Forms;

namespace TTG_Tools
{
    /// <summary>
    /// Modeless Find & Replace dialog for LandbEditor.
    /// Communicates actions back to the parent via events.
    /// </summary>
    public partial class FindReplaceDialog : Form
    {
        public event EventHandler FindNextClicked;
        public event EventHandler ReplaceClicked;
        public event EventHandler ReplaceAllClicked;

        public string FindText => _txtFind.Text;
        public string ReplaceText => _txtReplace.Text;
        public bool MatchCase => _chkMatchCase.Checked;
        public bool UseRegex => _chkRegex.Checked;
        public char Side => _radioSideA.Checked ? 'A' : 'B';

        public FindReplaceDialog()
        {
            InitializeComponent();
        }

        /// <summary>Hides the Side A/B radio group for single-side tools like LandbNormalizer.</summary>
        public void SetSingleSideMode()
        {
            _grpSide.Visible = false;
            // Move Close button and status label up into the freed space
            int offset = _grpSide.Height;
            _btnClose.Top -= offset;
            _lblStatus.Top -= offset;
        }

        /// <summary>
        /// Opens the dialog for single-side mode, pre-filling the find text.
        /// </summary>
        public void OpenSingleSide(string initialFindText)
        {
            if (!string.IsNullOrEmpty(initialFindText))
            {
                _txtFind.Text = initialFindText;
                _txtFind.SelectAll();
            }

            if (!Visible)
                Show(LandbReviewer.ActiveInstance);
            else
                Activate();

            _txtFind.Focus();
        }

        /// <summary>
        /// Opens the dialog, optionally pre-filling the find text and choosing the active side.
        /// If the dialog is already open, just focuses and selects the find text.
        /// </summary>
        public void Open(string initialFindText, char activeSide)
        {
            if (!string.IsNullOrEmpty(initialFindText))
            {
                _txtFind.Text = initialFindText;
                _txtFind.SelectAll();
            }

            if (activeSide == 'A')
                _radioSideA.Checked = true;
            else
                _radioSideB.Checked = true;

            if (!Visible)
                Show(LandbReviewer.ActiveInstance);
            else
                Activate();

            _txtFind.Focus();
        }

        public void ShowStatus(string message, bool isError = false)
        {
            _lblStatus.Text = message;
            _lblStatus.ForeColor = isError ? Color.Red : SystemColors.ControlText;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                Hide();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void OnFindNext(object sender, EventArgs e) => FindNextClicked?.Invoke(this, e);
        private void OnReplace(object sender, EventArgs e) => ReplaceClicked?.Invoke(this, e);
        private void OnReplaceAll(object sender, EventArgs e) => ReplaceAllClicked?.Invoke(this, e);
        private void OnClose(object sender, EventArgs e) => Hide();

        private void OnFindKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                FindNextClicked?.Invoke(this, e);
            }
        }

        private void OnReplaceKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                ReplaceClicked?.Invoke(this, e);
            }
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }
    }
}
