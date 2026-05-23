namespace TTG_Tools
{
    partial class NormalizeInFilesDialog
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this._lblDir = new System.Windows.Forms.Label();
            this._txtDirectory = new System.Windows.Forms.TextBox();
            this._btnBrowse = new System.Windows.Forms.Button();
            this._chkSubdirs = new System.Windows.Forms.CheckBox();
            this._grpOptions = new System.Windows.Forms.GroupBox();
            this._chkNormalizePunctuation = new System.Windows.Forms.CheckBox();
            this._chkAutoWrap = new System.Windows.Forms.CheckBox();
            this._chkRemoveCjkBlanks = new System.Windows.Forms.CheckBox();
            this._chkDotToChinese = new System.Windows.Forms.CheckBox();
            this._btnScan = new System.Windows.Forms.Button();
            this._btnApply = new System.Windows.Forms.Button();
            this._btnClose = new System.Windows.Forms.Button();
            this._listResults = new System.Windows.Forms.ListView();
            this._colFile = new System.Windows.Forms.ColumnHeader();
            this._colEntries = new System.Windows.Forms.ColumnHeader();
            this._colModified = new System.Windows.Forms.ColumnHeader();
            this._colStatus = new System.Windows.Forms.ColumnHeader();
            this._lblStatus = new System.Windows.Forms.Label();
            this._progressBar = new System.Windows.Forms.ProgressBar();
            this._grpOptions.SuspendLayout();
            this.SuspendLayout();

            // _lblDir
            this._lblDir.AutoSize = true;
            this._lblDir.Location = new System.Drawing.Point(12, 15);
            this._lblDir.Name = "_lblDir";
            this._lblDir.Size = new System.Drawing.Size(52, 13);
            this._lblDir.TabIndex = 0;
            this._lblDir.Text = "Directory:";

            // _txtDirectory
            this._txtDirectory.Location = new System.Drawing.Point(70, 12);
            this._txtDirectory.Name = "_txtDirectory";
            this._txtDirectory.Size = new System.Drawing.Size(440, 21);
            this._txtDirectory.TabIndex = 1;

            // _btnBrowse
            this._btnBrowse.Location = new System.Drawing.Point(516, 10);
            this._btnBrowse.Name = "_btnBrowse";
            this._btnBrowse.Size = new System.Drawing.Size(72, 23);
            this._btnBrowse.TabIndex = 2;
            this._btnBrowse.Text = "Browse...";
            this._btnBrowse.UseVisualStyleBackColor = true;
            this._btnBrowse.Click += new System.EventHandler(this.OnBrowseDir);

            // _chkSubdirs
            this._chkSubdirs.AutoSize = true;
            this._chkSubdirs.Checked = true;
            this._chkSubdirs.CheckState = System.Windows.Forms.CheckState.Checked;
            this._chkSubdirs.Location = new System.Drawing.Point(70, 39);
            this._chkSubdirs.Name = "_chkSubdirs";
            this._chkSubdirs.Size = new System.Drawing.Size(130, 17);
            this._chkSubdirs.TabIndex = 3;
            this._chkSubdirs.Text = "Include subdirectories";
            this._chkSubdirs.UseVisualStyleBackColor = true;

            // _grpOptions
            this._grpOptions.Controls.Add(this._chkNormalizePunctuation);
            this._grpOptions.Controls.Add(this._chkAutoWrap);
            this._grpOptions.Controls.Add(this._chkRemoveCjkBlanks);
            this._grpOptions.Controls.Add(this._chkDotToChinese);
            this._grpOptions.Location = new System.Drawing.Point(12, 65);
            this._grpOptions.Name = "_grpOptions";
            this._grpOptions.Size = new System.Drawing.Size(576, 62);
            this._grpOptions.TabIndex = 4;
            this._grpOptions.TabStop = false;
            this._grpOptions.Text = "Normalization rules";

            // _chkDotToChinese
            this._chkDotToChinese.AutoSize = true;
            this._chkDotToChinese.Location = new System.Drawing.Point(10, 18);
            this._chkDotToChinese.Name = "_chkDotToChinese";
            this._chkDotToChinese.Size = new System.Drawing.Size(195, 17);
            this._chkDotToChinese.TabIndex = 0;
            this._chkDotToChinese.Text = "Replace dots to Chinese period";
            this._chkDotToChinese.UseVisualStyleBackColor = true;

            // _chkRemoveCjkBlanks
            this._chkRemoveCjkBlanks.AutoSize = true;
            this._chkRemoveCjkBlanks.Location = new System.Drawing.Point(220, 18);
            this._chkRemoveCjkBlanks.Name = "_chkRemoveCjkBlanks";
            this._chkRemoveCjkBlanks.Size = new System.Drawing.Size(190, 17);
            this._chkRemoveCjkBlanks.TabIndex = 1;
            this._chkRemoveCjkBlanks.Text = "Remove blanks between CJK chars";
            this._chkRemoveCjkBlanks.UseVisualStyleBackColor = true;

            // _chkAutoWrap
            this._chkAutoWrap.AutoSize = true;
            this._chkAutoWrap.Location = new System.Drawing.Point(10, 39);
            this._chkAutoWrap.Name = "_chkAutoWrap";
            this._chkAutoWrap.Size = new System.Drawing.Size(200, 17);
            this._chkAutoWrap.TabIndex = 2;
            this._chkAutoWrap.Text = "Auto-wrap long subtitles (insert \\n)";
            this._chkAutoWrap.UseVisualStyleBackColor = true;

            // _chkNormalizePunctuation
            this._chkNormalizePunctuation.AutoSize = true;
            this._chkNormalizePunctuation.Location = new System.Drawing.Point(220, 39);
            this._chkNormalizePunctuation.Name = "_chkNormalizePunctuation";
            this._chkNormalizePunctuation.Size = new System.Drawing.Size(195, 17);
            this._chkNormalizePunctuation.TabIndex = 3;
            this._chkNormalizePunctuation.Text = "Normalize punctuation before \\n";
            this._chkNormalizePunctuation.UseVisualStyleBackColor = true;

            // _btnScan
            this._btnScan.Location = new System.Drawing.Point(12, 135);
            this._btnScan.Name = "_btnScan";
            this._btnScan.Size = new System.Drawing.Size(140, 26);
            this._btnScan.TabIndex = 5;
            this._btnScan.Text = "🔍 Scan && Preview";
            this._btnScan.UseVisualStyleBackColor = true;
            this._btnScan.Click += new System.EventHandler(this.OnScan);

            // _btnApply
            this._btnApply.Enabled = false;
            this._btnApply.Location = new System.Drawing.Point(158, 135);
            this._btnApply.Name = "_btnApply";
            this._btnApply.Size = new System.Drawing.Size(140, 26);
            this._btnApply.TabIndex = 6;
            this._btnApply.Text = "✅ Apply to All";
            this._btnApply.UseVisualStyleBackColor = true;
            this._btnApply.Click += new System.EventHandler(this.OnApply);

            // _btnClose
            this._btnClose.Location = new System.Drawing.Point(516, 135);
            this._btnClose.Name = "_btnClose";
            this._btnClose.Size = new System.Drawing.Size(72, 26);
            this._btnClose.TabIndex = 7;
            this._btnClose.Text = "Close";
            this._btnClose.UseVisualStyleBackColor = true;
            this._btnClose.Click += new System.EventHandler(this.OnClose);

            // _listResults
            this._listResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                this._colFile, this._colEntries, this._colModified, this._colStatus});
            this._listResults.FullRowSelect = true;
            this._listResults.GridLines = true;
            this._listResults.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this._listResults.Location = new System.Drawing.Point(12, 170);
            this._listResults.Name = "_listResults";
            this._listResults.Size = new System.Drawing.Size(576, 200);
            this._listResults.TabIndex = 8;
            this._listResults.View = System.Windows.Forms.View.Details;
            this._colFile.Text = "File";
            this._colFile.Width = 260;
            this._colEntries.Text = "Entries";
            this._colEntries.Width = 60;
            this._colModified.Text = "Modified";
            this._colModified.Width = 65;
            this._colStatus.Text = "Status";
            this._colStatus.Width = 170;

            // _lblStatus
            this._lblStatus.AutoSize = true;
            this._lblStatus.Location = new System.Drawing.Point(12, 378);
            this._lblStatus.Name = "_lblStatus";
            this._lblStatus.Size = new System.Drawing.Size(41, 13);
            this._lblStatus.TabIndex = 9;
            this._lblStatus.Text = "Ready.";

            // _progressBar
            this._progressBar.Location = new System.Drawing.Point(12, 396);
            this._progressBar.Name = "_progressBar";
            this._progressBar.Size = new System.Drawing.Size(576, 16);
            this._progressBar.TabIndex = 10;

            // NormalizeInFilesDialog
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 422);
            this.Controls.Add(this._progressBar);
            this.Controls.Add(this._lblStatus);
            this.Controls.Add(this._listResults);
            this.Controls.Add(this._btnClose);
            this.Controls.Add(this._btnApply);
            this.Controls.Add(this._btnScan);
            this.Controls.Add(this._grpOptions);
            this.Controls.Add(this._chkSubdirs);
            this.Controls.Add(this._btnBrowse);
            this.Controls.Add(this._txtDirectory);
            this.Controls.Add(this._lblDir);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NormalizeInFilesDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Normalize in Files";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this._grpOptions.ResumeLayout(false);
            this._grpOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label _lblDir;
        private System.Windows.Forms.TextBox _txtDirectory;
        private System.Windows.Forms.Button _btnBrowse;
        private System.Windows.Forms.CheckBox _chkSubdirs;
        private System.Windows.Forms.GroupBox _grpOptions;
        private System.Windows.Forms.CheckBox _chkNormalizePunctuation;
        private System.Windows.Forms.CheckBox _chkAutoWrap;
        private System.Windows.Forms.CheckBox _chkRemoveCjkBlanks;
        private System.Windows.Forms.CheckBox _chkDotToChinese;
        private System.Windows.Forms.Button _btnScan;
        private System.Windows.Forms.Button _btnApply;
        private System.Windows.Forms.Button _btnClose;
        private System.Windows.Forms.ListView _listResults;
        private System.Windows.Forms.ColumnHeader _colFile;
        private System.Windows.Forms.ColumnHeader _colEntries;
        private System.Windows.Forms.ColumnHeader _colModified;
        private System.Windows.Forms.ColumnHeader _colStatus;
        private System.Windows.Forms.Label _lblStatus;
        private System.Windows.Forms.ProgressBar _progressBar;
    }
}
