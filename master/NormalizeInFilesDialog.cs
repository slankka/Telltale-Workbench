using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TTG_Tools.ClassesStructs.Text;
using TTG_Tools.Texts;

namespace TTG_Tools
{
    public partial class NormalizeInFilesDialog : Form
    {
        private BackgroundWorker _scanWorker;
        private BackgroundWorker _applyWorker;
        private List<NormalizeFileResult> _results;
        private Action<string> _logCallback;

        private struct NormalizeFileResult
        {
            public string FilePath;
            public int TotalEntries;
            public int ModifiedEntries;
            public string Status;
        }

        public NormalizeInFilesDialog(Action<string> logCallback,
            bool dotToChinese, bool removeCjkBlanks, bool autoWrap, bool normalizePunctuation)
        {
            InitializeComponent();
            _logCallback = logCallback;
            _chkDotToChinese.Checked = dotToChinese;
            _chkRemoveCjkBlanks.Checked = removeCjkBlanks;
            _chkAutoWrap.Checked = autoWrap;
            _chkNormalizePunctuation.Checked = normalizePunctuation;
            InitWorkers();
        }

        private void InitWorkers()
        {
            _scanWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            _scanWorker.DoWork += ScanWorker_DoWork;
            _scanWorker.ProgressChanged += ScanWorker_ProgressChanged;
            _scanWorker.RunWorkerCompleted += ScanWorker_RunWorkerCompleted;

            _applyWorker = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = false
            };
            _applyWorker.DoWork += ApplyWorker_DoWork;
            _applyWorker.ProgressChanged += ApplyWorker_ProgressChanged;
            _applyWorker.RunWorkerCompleted += ApplyWorker_RunWorkerCompleted;
        }

        private bool AnyRuleChecked =>
            _chkDotToChinese.Checked || _chkRemoveCjkBlanks.Checked ||
            _chkAutoWrap.Checked || _chkNormalizePunctuation.Checked;

        // ===== Button handlers =====

        private void OnBrowseDir(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog { Description = "Select root directory containing .landb files" })
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                    _txtDirectory.Text = dlg.SelectedPath;
            }
        }

        private void OnScan(object sender, EventArgs e)
        {
            string dir = _txtDirectory.Text.Trim();
            if (!Directory.Exists(dir))
            {
                SetStatus("Directory not found.", isError: true);
                return;
            }
            if (!AnyRuleChecked)
            {
                SetStatus("No normalization rules selected.", isError: true);
                return;
            }

            _listResults.Items.Clear();
            _btnScan.Enabled = false;
            _btnApply.Enabled = false;
            _progressBar.Value = 0;
            SetStatus("Scanning...");

            _scanWorker.RunWorkerAsync(dir);
        }

        private void OnApply(object sender, EventArgs e)
        {
            if (_results == null || _results.Count == 0) return;

            int willModify = _results.Count(r => r.ModifiedEntries > 0);
            if (willModify == 0)
            {
                SetStatus("No files need modification.");
                return;
            }

            var confirm = MessageBox.Show(
                $"Apply normalization to {willModify} file(s)?\n\nThis will modify files on disk.",
                "Confirm Apply", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

            if (confirm != DialogResult.OK) return;

            _btnScan.Enabled = false;
            _btnApply.Enabled = false;
            _progressBar.Value = 0;
            SetStatus("Applying...");

            _applyWorker.RunWorkerAsync();
        }

        private void OnClose(object sender, EventArgs e) => Close();

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (_scanWorker.IsBusy) _scanWorker.CancelAsync();
            if (_applyWorker.IsBusy) _applyWorker.CancelAsync();
        }

        // ===== Scan worker =====

        private void ScanWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string rootDir = (string)e.Argument;
            bool subdirs = _chkSubdirs.Checked;
            var worker = (BackgroundWorker)sender;

            var files = Directory.GetFiles(rootDir, "*.landb",
                subdirs ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

            var results = new List<NormalizeFileResult>();

            for (int i = 0; i < files.Length; i++)
            {
                if (worker.CancellationPending) { e.Cancel = true; return; }

                string file = files[i];
                var result = new NormalizeFileResult { FilePath = file, Status = "-" };

                try
                {
                    bool isUnicode, mapCredits; string errorMsg;
                    var landb = LandbWorker.LoadLandbFromFile(file, out isUnicode, out mapCredits, out errorMsg);
                    if (landb == null) { result.Status = "Load error"; results.Add(result); continue; }

                    var texts = LandbWorker.LandbToCommonTextList(landb, mapCredits);
                    result.TotalEntries = texts.Count;

                    int modified = 0;
                    foreach (var t in texts)
                    {
                        string trans = t.actorSpeechTranslation ?? "";
                        if (string.IsNullOrEmpty(trans)) continue;

                        string normalized = LandbEditor.NormalizeTranslation(trans,
                            _chkDotToChinese.Checked, _chkRemoveCjkBlanks.Checked,
                            _chkAutoWrap.Checked, _chkNormalizePunctuation.Checked);

                        if (!string.Equals(trans, normalized, StringComparison.Ordinal))
                            modified++;
                    }

                    result.ModifiedEntries = modified;
                    result.Status = modified > 0 ? "pending" : "skipped";
                }
                catch (Exception ex)
                {
                    result.Status = "Error: " + ex.Message;
                }

                results.Add(result);

                int pct = (int)((i + 1) * 100.0 / files.Length);
                worker.ReportProgress(pct, new Tuple<int, NormalizeFileResult>(i, result));
            }

            e.Result = results;
        }

        private void ScanWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _progressBar.Value = e.ProgressPercentage;

            var data = e.UserState as Tuple<int, NormalizeFileResult>;
            if (data == null) return;

            var result = data.Item2;
            string displayPath = MakeRelativePath(result.FilePath, _txtDirectory.Text);

            var item = new ListViewItem(displayPath);
            item.SubItems.Add(result.TotalEntries.ToString());
            item.SubItems.Add(result.ModifiedEntries > 0 ? result.ModifiedEntries.ToString() : "-");
            item.SubItems.Add(result.Status);
            item.Tag = result.FilePath;
            _listResults.Items.Add(item);
        }

        private void ScanWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _btnScan.Enabled = true;

            if (e.Cancelled)
            {
                SetStatus("Scan cancelled.");
                return;
            }
            if (e.Error != null)
            {
                SetStatus("Error: " + e.Error.Message, isError: true);
                return;
            }

            _results = e.Result as List<NormalizeFileResult>;
            if (_results == null) { SetStatus("No results."); return; }

            int totalFiles = _results.Count;
            int totalEntries = _results.Sum(r => r.TotalEntries);
            int willModify = _results.Count(r => r.ModifiedEntries > 0);
            int totalModified = _results.Sum(r => r.ModifiedEntries);

            _btnApply.Enabled = willModify > 0;
            SetStatus($"Done. {totalFiles} files, {totalEntries} entries. " +
                $"{willModify} file(s) with {totalModified} entries to modify.");
        }

        // ===== Apply worker =====

        private void ApplyWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;
            var toProcess = _results.Where(r => r.ModifiedEntries > 0).ToList();

            for (int i = 0; i < toProcess.Count; i++)
            {
                var fileResult = toProcess[i];
                string file = fileResult.FilePath;

                try
                {
                    bool isUnicode, mapCredits; string errorMsg;
                    var landb = LandbWorker.LoadLandbFromFile(file, out isUnicode, out mapCredits, out errorMsg);
                    if (landb == null) continue;

                    var texts = LandbWorker.LandbToCommonTextList(landb, mapCredits);
                    int modified = 0;

                    for (int j = 0; j < texts.Count; j++)
                    {
                        var t = texts[j];
                        string trans = t.actorSpeechTranslation ?? "";
                        if (string.IsNullOrEmpty(trans)) continue;

                        string normalized = LandbEditor.NormalizeTranslation(trans,
                            _chkDotToChinese.Checked, _chkRemoveCjkBlanks.Checked,
                            _chkAutoWrap.Checked, _chkNormalizePunctuation.Checked);

                        if (!string.Equals(trans, normalized, StringComparison.Ordinal))
                        {
                            t.actorSpeechTranslation = normalized;
                            modified++;
                        }
                    }

                    if (modified > 0)
                    {
                        string saveResult = LandbWorker.SaveLandbToFile(file, file, landb, texts, mapCredits);
                        fileResult.Status = saveResult.Contains("error") || saveResult.Contains("Error")
                            ? "Save error" : "done ✓";
                    }
                    else
                    {
                        fileResult.Status = "skipped";
                    }
                }
                catch (Exception ex)
                {
                    fileResult.Status = "Error: " + ex.Message;
                }

                int pct = (int)((i + 1) * 100.0 / toProcess.Count);
                worker.ReportProgress(pct, new Tuple<int, string>(i, fileResult.Status));
            }
        }

        private void ApplyWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _progressBar.Value = e.ProgressPercentage;

            var data = e.UserState as Tuple<int, string>;
            if (data == null) return;

            int index = data.Item1;
            string status = data.Item2;

            // Find the corresponding item in the list
            int listIndex = -1;
            for (int i = 0; i < _listResults.Items.Count; i++)
            {
                if (_listResults.Items[i].SubItems[3].Text == "pending")
                {
                    listIndex = i;
                    break;
                }
            }

            // Update by matching modified entries > 0
            var toProcess = _results.Where(r => r.ModifiedEntries > 0).ToList();
            if (index < toProcess.Count)
            {
                string targetFile = toProcess[index].FilePath;
                for (int i = 0; i < _listResults.Items.Count; i++)
                {
                    if ((string)_listResults.Items[i].Tag == targetFile)
                    {
                        _listResults.Items[i].SubItems[3].Text = status;
                        break;
                    }
                }
            }
        }

        private void ApplyWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _btnScan.Enabled = true;
            _btnApply.Enabled = false;
            _logCallback?.Invoke("[Normalize in Files] Batch normalization complete.");
            SetStatus("Apply complete.");
        }

        // ===== Helpers =====

        private void SetStatus(string message, bool isError = false)
        {
            _lblStatus.Text = message;
            _lblStatus.ForeColor = isError ? System.Drawing.Color.Red : System.Drawing.SystemColors.ControlText;
        }

        private static string MakeRelativePath(string fullPath, string baseDir)
        {
            if (string.IsNullOrEmpty(baseDir)) return Path.GetFileName(fullPath);
            baseDir = baseDir.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
            if (fullPath.StartsWith(baseDir, StringComparison.OrdinalIgnoreCase))
                return fullPath.Substring(baseDir.Length);
            return fullPath;
        }
    }
}
