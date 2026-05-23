using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TTG_Tools.ClassesStructs.Text;
using TTG_Tools.Texts;

namespace TTG_Tools
{
    public partial class LandbNormalizer : Form
    {
        private string _filePath;
        private LandbClass _landb;
        private List<CommonText> _texts;
        private bool _isUnicode;
        private bool _mapCredits;
        private bool _isDirty;

        // Find/Replace state
        private FindReplaceDialog _findReplaceDlg;
        private int _lastSearchRow = -1;
        private string _lastSearchText = "";

        // Normalization preview cache
        private List<NormalizePreviewEntry> _previewEntries;
        private bool _previewDirty = true;

        internal static LandbNormalizer ActiveInstance { get; private set; }

        private const int ROWS_PER_ENTRY = 5;

        public LandbNormalizer()
        {
            InitializeComponent();
            ActiveInstance = this;
            if (DesignMode) return;
            InitFindReplace();
            HookEditingControls();
            LoadSettings();
        }

        // ========== Init ==========

        private void InitFindReplace()
        {
            _findReplaceDlg = new FindReplaceDialog();
            _findReplaceDlg.FindNextClicked += OnFindNextClicked;
            _findReplaceDlg.ReplaceClicked += OnReplaceClicked;
            _findReplaceDlg.ReplaceAllClicked += OnReplaceAllClicked;
            // Hide the Side group since we only have one side
            _findReplaceDlg.SetSingleSideMode();
        }

        private void HookEditingControls()
        {
            _gridView.EditingControlShowing += (sender, e) =>
            {
                var tb = e.Control as TextBox;
                if (tb == null) return;
                tb.Multiline = true;
                tb.AcceptsReturn = true;
                tb.ScrollBars = ScrollBars.Vertical;
                tb.WordWrap = true;
                if (_gridView.CurrentCell != null)
                {
                    string cellValue = _gridView.CurrentCell.Value?.ToString() ?? "";
                    if (cellValue.Contains("\n"))
                        tb.Text = cellValue;
                }
            };
        }

        private void LoadSettings()
        {
            _checkDotToChinese.Checked = AppData.settings.replaceDotToChinesePeriodInImport;
            _checkRemoveCjkBlanks.Checked = AppData.settings.removeBlanksBetweenCjkCharsInImport;
            _checkAutoWrap.Checked = AppData.settings.autoInsertSubtitleNewlineInImport;
            _checkNormalizePunctuation.Checked = AppData.settings.normalizePunctuationBeforeNewlineInImport;
            SyncMenuChecks();
        }

        private void SyncMenuChecks()
        {
            _menuCheckDotToChinese.Checked = _checkDotToChinese.Checked;
            _menuCheckRemoveCjkBlanks.Checked = _checkRemoveCjkBlanks.Checked;
            _menuCheckAutoWrap.Checked = _checkAutoWrap.Checked;
            _menuCheckNormalizePunctuation.Checked = _checkNormalizePunctuation.Checked;
        }

        // ========== Menu / toolbar events ==========

        private void OnOpenDir(object sender, EventArgs e) => BrowseDirectory();
        private void OnCloseMenu(object sender, EventArgs e) => Close();
        private void OnBrowse(object sender, EventArgs e) => BrowseDirectory();
        private void OnSave(object sender, EventArgs e) => Save();
        private void OnSaveAs(object sender, EventArgs e) => SaveAs();
        private void OnExportChars(object sender, EventArgs e) => ExportAllChars();
        private void OnTreeSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node?.Tag != null && File.Exists(e.Node.Tag.ToString()))
                LoadLandb(e.Node.Tag.ToString());
        }

        private void OnCellChangedHandler(object sender, DataGridViewCellEventArgs e)
        {
            _isDirty = true;
            _previewDirty = true;
            UpdateTitle();
        }

        private void OnCellValidatingHandler(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 1 && e.RowIndex % ROWS_PER_ENTRY == 4)
            {
                string v = e.FormattedValue?.ToString() ?? "";
                if (v.Length > 8) { e.Cancel = true; Log("ERROR: flags max 8 chars"); return; }
                foreach (char c in v) { if (c != '0' && c != '1') { e.Cancel = true; Log("ERROR: flags 0/1 only"); return; } }
            }
        }

        private void OnFormKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                e.SuppressKeyPress = true;
                Save();
            }
            else if ((e.Control && e.KeyCode == Keys.F) || (e.Control && e.KeyCode == Keys.H))
            {
                e.SuppressKeyPress = true;
                OpenFindReplace();
            }
            else if (e.KeyCode == Keys.F3)
            {
                e.SuppressKeyPress = true;
                FindNext();
            }
        }

        // ========== Find / Replace ==========

        private void OnFindOpen(object sender, EventArgs e) => OpenFindReplace();
        private void OnFindNextMenu(object sender, EventArgs e) => FindNext();

        private void OpenFindReplace()
        {
            string selectedText = GetSelectedText();
            // For single-side mode, open with side placeholder
            _findReplaceDlg.OpenSingleSide(selectedText ?? "");

            _findReplaceDlg.StartPosition = FormStartPosition.Manual;
            _findReplaceDlg.Location = new System.Drawing.Point(
                this.Location.X + this.Width - _findReplaceDlg.Width - 50,
                this.Location.Y + 80);
        }

        private string GetSelectedText()
        {
            if (_gridView?.CurrentCell != null)
            {
                if (_gridView.EditingControl is TextBox tb && !string.IsNullOrEmpty(tb.SelectedText))
                    return tb.SelectedText;
            }
            return "";
        }

        private void OnFindNextClicked(object sender, EventArgs e)
        {
            int startRow = (_lastSearchRow >= 0 && _lastSearchRow < _gridView.Rows.Count) ? _lastSearchRow + 1 : 0;
            int found = FindInGrid(_gridView, _findReplaceDlg.FindText, startRow, _findReplaceDlg.MatchCase);
            if (found >= 0)
            {
                _lastSearchRow = found;
                _lastSearchText = _findReplaceDlg.FindText;
                SelectCell(_gridView, found);
                _findReplaceDlg.ShowStatus($"Found at row {found}");
            }
            else
            {
                _findReplaceDlg.ShowStatus("Not found", isError: true);
            }
        }

        private void OnReplaceClicked(object sender, EventArgs e)
        {
            string findText = _findReplaceDlg.FindText;
            string replaceText = _findReplaceDlg.ReplaceText;
            bool matchCase = _findReplaceDlg.MatchCase;
            if (string.IsNullOrEmpty(findText)) return;

            if (_lastSearchRow >= 0 && _lastSearchRow < _gridView.Rows.Count)
            {
                string currentVal = _gridView.Rows[_lastSearchRow].Cells[1].Value?.ToString() ?? "";
                StringComparison cmp = matchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
                if (currentVal.IndexOf(findText, cmp) >= 0)
                {
                    string newVal = ReplaceFirst(currentVal, findText, replaceText, cmp);
                    _gridView.Rows[_lastSearchRow].Cells[1].Value = newVal;
                    _isDirty = true;
                    _previewDirty = true;
                    UpdateTitle();
                }
            }

            int startRow = (_lastSearchRow >= 0 && _lastSearchRow < _gridView.Rows.Count) ? _lastSearchRow + 1 : 0;
            int found = FindInGrid(_gridView, findText, startRow, matchCase);
            if (found >= 0)
            {
                _lastSearchRow = found;
                _lastSearchText = findText;
                SelectCell(_gridView, found);
                _findReplaceDlg.ShowStatus($"Replaced, next at row {found}");
            }
            else
            {
                _lastSearchRow = -1;
                _lastSearchText = findText;
                _findReplaceDlg.ShowStatus("Replaced, no more matches", isError: true);
            }
        }

        private void OnReplaceAllClicked(object sender, EventArgs e)
        {
            string findText = _findReplaceDlg.FindText;
            string replaceText = _findReplaceDlg.ReplaceText;
            bool matchCase = _findReplaceDlg.MatchCase;
            if (string.IsNullOrEmpty(findText))
            {
                _findReplaceDlg.ShowStatus("Nothing to find", isError: true);
                return;
            }

            StringComparison cmp = matchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            int count = 0;
            for (int r = 0; r < _gridView.Rows.Count; r++)
            {
                string value = _gridView.Rows[r].Cells[1].Value?.ToString() ?? "";
                if (value.IndexOf(findText, cmp) >= 0)
                {
                    string newVal = matchCase
                        ? value.Replace(findText, replaceText)
                        : ReplaceAllIgnoreCase(value, findText, replaceText);
                    _gridView.Rows[r].Cells[1].Value = newVal;
                    count++;
                }
            }

            if (count > 0)
            {
                _isDirty = true;
                _previewDirty = true;
                UpdateTitle();
                _lastSearchRow = -1;
                _lastSearchText = findText;
                _findReplaceDlg.ShowStatus($"Replaced {count} occurrence(s)");
                Log($"Replaced All: {count} occurrence(s) of \"{findText}\"");
            }
            else
            {
                _findReplaceDlg.ShowStatus("No matches found", isError: true);
            }
        }

        private void FindNext()
        {
            if (_findReplaceDlg == null || string.IsNullOrEmpty(_findReplaceDlg.FindText))
            {
                OpenFindReplace();
                return;
            }

            if (_lastSearchText != _findReplaceDlg.FindText)
                _lastSearchRow = -1;

            int startRow = (_lastSearchRow >= 0 && _lastSearchRow < _gridView.Rows.Count) ? _lastSearchRow + 1 : 0;
            int found = FindInGrid(_gridView, _findReplaceDlg.FindText, startRow, _findReplaceDlg.MatchCase);
            if (found >= 0)
            {
                _lastSearchRow = found;
                _lastSearchText = _findReplaceDlg.FindText;
                SelectCell(_gridView, found);
                _findReplaceDlg.ShowStatus($"Found at row {found}");
            }
            else
            {
                if (startRow > 0)
                {
                    found = FindInGrid(_gridView, _findReplaceDlg.FindText, 0, _findReplaceDlg.MatchCase);
                    if (found >= 0)
                    {
                        _lastSearchRow = found;
                        _lastSearchText = _findReplaceDlg.FindText;
                        SelectCell(_gridView, found);
                        _findReplaceDlg.ShowStatus($"Wrapped - found at row {found}");
                        return;
                    }
                }
                _findReplaceDlg.ShowStatus("Not found", isError: true);
            }
        }

        private static int FindInGrid(DataGridView grid, string searchText, int startRow, bool matchCase)
        {
            if (grid == null || grid.Rows.Count == 0 || string.IsNullOrEmpty(searchText))
                return -1;

            StringComparison cmp = matchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            int totalRows = grid.Rows.Count;
            if (startRow < 0) startRow = 0;
            if (startRow >= totalRows) startRow = 0;

            for (int r = startRow; r < totalRows; r++)
            {
                string value = grid.Rows[r].Cells[1].Value?.ToString() ?? "";
                if (value.IndexOf(searchText, cmp) >= 0) return r;
            }
            for (int r = 0; r < startRow; r++)
            {
                string value = grid.Rows[r].Cells[1].Value?.ToString() ?? "";
                if (value.IndexOf(searchText, cmp) >= 0) return r;
            }
            return -1;
        }

        private static void SelectCell(DataGridView grid, int rowIndex)
        {
            if (grid == null || rowIndex < 0 || rowIndex >= grid.Rows.Count) return;
            grid.ClearSelection();
            grid.CurrentCell = grid.Rows[rowIndex].Cells[1];
            if (rowIndex < grid.FirstDisplayedScrollingRowIndex ||
                rowIndex >= grid.FirstDisplayedScrollingRowIndex + grid.DisplayedRowCount(false))
            {
                grid.FirstDisplayedScrollingRowIndex = Math.Max(0, rowIndex - 5);
            }
        }

        private static string ReplaceFirst(string text, string oldValue, string newValue, StringComparison cmp)
        {
            int idx = text.IndexOf(oldValue, cmp);
            if (idx < 0) return text;
            return text.Substring(0, idx) + newValue + text.Substring(idx + oldValue.Length);
        }

        private static string ReplaceAllIgnoreCase(string text, string oldValue, string newValue)
        {
            if (string.IsNullOrEmpty(oldValue)) return text;
            var sb = new StringBuilder();
            int pos = 0;
            while (pos < text.Length)
            {
                int idx = text.IndexOf(oldValue, pos, StringComparison.OrdinalIgnoreCase);
                if (idx < 0) { sb.Append(text.Substring(pos)); break; }
                sb.Append(text.Substring(pos, idx - pos));
                sb.Append(newValue);
                pos = idx + oldValue.Length;
            }
            return sb.ToString();
        }

        // ========== Directory ==========

        private void BrowseDirectory()
        {
            using (var dlg = new FolderBrowserDialog { Description = "Select directory containing .landb files" })
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    _txtPath.Text = dlg.SelectedPath;
                    RefreshTree(dlg.SelectedPath);
                    AppData.settings.landbEditorLastDirA = dlg.SelectedPath;
                    Settings.SaveConfig(AppData.settings);
                    Log($"Directory: {dlg.SelectedPath}");
                }
            }
        }

        private void RefreshTree(string rootDir)
        {
            _treeView.Nodes.Clear();
            if (!Directory.Exists(rootDir)) return;
            var rootNode = new TreeNode(Path.GetFileName(rootDir)) { Tag = rootDir, Name = rootDir };
            _treeView.Nodes.Add(rootNode);
            PopulateTreeNodes(rootNode, rootDir);
            rootNode.Expand();
        }

        private void PopulateTreeNodes(TreeNode parentNode, string dirPath)
        {
            try
            {
                foreach (var subDir in Directory.GetDirectories(dirPath))
                {
                    var dirNode = new TreeNode(Path.GetFileName(subDir)) { Tag = subDir, Name = subDir };
                    parentNode.Nodes.Add(dirNode);
                    PopulateTreeNodes(dirNode, subDir);
                }
                foreach (var file in Directory.GetFiles(dirPath, "*.landb"))
                {
                    var fileNode = new TreeNode(Path.GetFileName(file)) { Tag = file, Name = file, ForeColor = Color.DarkBlue };
                    parentNode.Nodes.Add(fileNode);
                }
            }
            catch (UnauthorizedAccessException) { }
        }

        // ========== File load / save ==========

        private void LoadLandb(string filePath)
        {
            try
            {
                bool isUnicode, mapCredits; string errorMsg;
                var landb = LandbWorker.LoadLandbFromFile(filePath, out isUnicode, out mapCredits, out errorMsg);
                if (landb == null) { Log($"ERROR: {errorMsg}"); return; }
                var texts = LandbWorker.LandbToCommonTextList(landb, mapCredits);

                _filePath = filePath;
                _landb = landb;
                _texts = texts;
                _isUnicode = isUnicode;
                _mapCredits = mapCredits;
                _isDirty = false;
                _previewDirty = true;

                PopulateGrid(texts);
                _lblFileInfo.Text = $"{Path.GetFileName(filePath)} ({texts.Count} entries)" + (isUnicode ? " [U]" : "");
                _btnSave.Enabled = _btnSaveAs.Enabled = true;
                HidePreview();
                ClearNormalizeStats();
                UpdateTitle();
                Log($"Loaded: {Path.GetFileName(filePath)} - {texts.Count} entries");
            }
            catch (Exception ex) { Log($"ERROR loading: {ex.Message}"); }
        }

        private void PopulateGrid(List<CommonText> texts)
        {
            _gridView.Rows.Clear();
            if (texts == null) return;
            foreach (var t in texts)
            {
                string orig = (t.actorSpeechOriginal ?? "").Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\r\n");
                string trans = (t.actorSpeechTranslation ?? "").Replace("\r\n", "\n").Replace("\r", "\n").Replace("\n", "\r\n");
                _gridView.Rows.Add("langid", t.strNumber.ToString());
                _gridView.Rows.Add("actor", t.actorName ?? "");
                _gridView.Rows.Add("speechOriginal", orig);
                _gridView.Rows.Add("speechTranslation", trans);
                _gridView.Rows.Add("flags", t.flags ?? "00000000");
            }
            for (int i = 0; i < texts.Count; i++)
            {
                var back = i % 2 == 0 ? SystemColors.Window : Color.FromArgb(245, 248, 252);
                for (int r = 0; r < ROWS_PER_ENTRY; r++)
                {
                    var row = _gridView.Rows[i * ROWS_PER_ENTRY + r];
                    row.DefaultCellStyle.BackColor = back;
                    row.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                }
            }
            _gridView.AutoResizeRows();
        }

        private void Save()
        {
            if (!_isDirty) { Log("No changes to save."); return; }
            DoSave(_filePath, _filePath);
        }

        private void SaveAs()
        {
            using (var dlg = new SaveFileDialog { Filter = "Landb files (*.landb)|*.landb", DefaultExt = ".landb" })
            {
                if (dlg.ShowDialog(this) == DialogResult.OK)
                    DoSave(_filePath, dlg.FileName);
            }
        }

        private void DoSave(string origPath, string outPath)
        {
            try
            {
                _texts = ReadTextsFromGrid();
                string result = LandbWorker.SaveLandbToFile(origPath, outPath, _landb, _texts, _mapCredits);
                Log(result);
                if (!result.Contains("error") && !result.Contains("Error"))
                {
                    _isDirty = false;
                    if (outPath != origPath) _filePath = outPath;
                    UpdateTitle();
                }
            }
            catch (Exception ex) { Log($"ERROR saving: {ex.Message}"); }
        }

        private List<CommonText> ReadTextsFromGrid()
        {
            var result = new List<CommonText>();
            int count = Math.Min(_gridView.Rows.Count / ROWS_PER_ENTRY, _texts?.Count ?? 0);
            for (int i = 0; i < count; i++)
            {
                var t = _texts[i];
                int baseRow = i * ROWS_PER_ENTRY;
                string trans = (_gridView.Rows[baseRow + 3].Cells[1].Value?.ToString() ?? "").Replace("\r\n", "\n");
                t.actorSpeechTranslation = trans;
                t.flags = (_gridView.Rows[baseRow + 4].Cells[1].Value?.ToString() ?? "00000000");
                var sb = new StringBuilder();
                foreach (char c in t.flags) if (c == '0' || c == '1') sb.Append(c);
                t.flags = sb.ToString().PadLeft(8, '0');
                result.Add(t);
            }
            return result;
        }

        // ========== Character extraction ==========

        private void ExportAllChars()
        {
            if (_texts == null || _texts.Count == 0) { Log("No data loaded."); return; }

            using (var dlg = new SaveFileDialog
            {
                Filter = "Text files (*.txt)|*.txt",
                DefaultExt = ".txt",
                FileName = "chars_export.txt"
            })
            {
                if (dlg.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    var charSet = new HashSet<string>();
                    foreach (var t in _texts)
                    {
                        if (string.IsNullOrEmpty(t.actorSpeechOriginal)) continue;
                        foreach (char c in t.actorSpeechOriginal)
                            charSet.Add(c.ToString());
                    }
                    var sorted = charSet
                        .OrderBy(s => s.Length > 0 && char.IsLetterOrDigit(s[0]) ? 0 : 1)
                        .ThenBy(s => (int)(s.Length > 0 ? s[0] : 0))
                        .ToList();
                    using (var sw = new StreamWriter(dlg.FileName, false, new UTF8Encoding(true)))
                    {
                        foreach (var ch in sorted) sw.Write(ch);
                    }
                    Log($"Exported {charSet.Count} unique chars → {Path.GetFileName(dlg.FileName)}");
                }
                catch (Exception ex) { Log($"ERROR exporting chars: {ex.Message}"); }
            }
        }

        // ========== Normalization ==========

        private bool AnyNormalizationEnabled =>
            _checkDotToChinese.Checked || _checkRemoveCjkBlanks.Checked ||
            _checkAutoWrap.Checked || _checkNormalizePunctuation.Checked;

        private void InvalidatePreview() { _previewDirty = true; ClearNormalizeStats(); }

        private void ClearNormalizeStats() { _lblNormalizeStats.Text = ""; }

        // ---- Checkbox sync (form ↔ menu) ----

        private void OnCheckDotToChineseChanged(object sender, EventArgs e)
        {
            AppData.settings.replaceDotToChinesePeriodInImport = _checkDotToChinese.Checked;
            _menuCheckDotToChinese.Checked = _checkDotToChinese.Checked;
            Settings.SaveConfig(AppData.settings);
            InvalidatePreview();
        }
        private void OnCheckRemoveCjkBlanksChanged(object sender, EventArgs e)
        {
            AppData.settings.removeBlanksBetweenCjkCharsInImport = _checkRemoveCjkBlanks.Checked;
            _menuCheckRemoveCjkBlanks.Checked = _checkRemoveCjkBlanks.Checked;
            Settings.SaveConfig(AppData.settings);
            InvalidatePreview();
        }
        private void OnCheckAutoWrapChanged(object sender, EventArgs e)
        {
            AppData.settings.autoInsertSubtitleNewlineInImport = _checkAutoWrap.Checked;
            _menuCheckAutoWrap.Checked = _checkAutoWrap.Checked;
            Settings.SaveConfig(AppData.settings);
            InvalidatePreview();
        }
        private void OnCheckNormalizePunctuationChanged(object sender, EventArgs e)
        {
            AppData.settings.normalizePunctuationBeforeNewlineInImport = _checkNormalizePunctuation.Checked;
            _menuCheckNormalizePunctuation.Checked = _checkNormalizePunctuation.Checked;
            Settings.SaveConfig(AppData.settings);
            InvalidatePreview();
        }

        // Menu → form sync
        private void OnMenuCheckDotToChineseChanged(object sender, EventArgs e)
        {
            _checkDotToChinese.Checked = _menuCheckDotToChinese.Checked;
        }
        private void OnMenuCheckRemoveCjkBlanksChanged(object sender, EventArgs e)
        {
            _checkRemoveCjkBlanks.Checked = _menuCheckRemoveCjkBlanks.Checked;
        }
        private void OnMenuCheckAutoWrapChanged(object sender, EventArgs e)
        {
            _checkAutoWrap.Checked = _menuCheckAutoWrap.Checked;
        }
        private void OnMenuCheckNormalizePunctuationChanged(object sender, EventArgs e)
        {
            _checkNormalizePunctuation.Checked = _menuCheckNormalizePunctuation.Checked;
        }

        // ---- Normalize a single text ----

        internal static string NormalizeTranslation(string text,
            bool dotToChinese, bool removeCjkBlanks, bool autoWrap, bool normalizePunctuation)
        {
            if (string.IsNullOrEmpty(text)) return text;

            string result = Methods.ConvertLiteralNewlineMarkers(text);

            if (normalizePunctuation)
                result = Methods.TransformOutsideMarkers(result, Methods.NormalizePunctuationBeforeNewline);

            if (!Methods.ContainsCjkCharacters(result)) return autoWrap
                ? Methods.ApplyAutoSubtitleWrapAfterReplace(result) : result;

            if (removeCjkBlanks)
                result = Methods.TransformOutsideMarkers(result, Methods.RemoveWhitespacesBetweenCjkCharacters);

            if (dotToChinese)
                result = Methods.TransformOutsideMarkers(result, Methods.ReplaceDotsNearCjkWithChinesePeriod);

            if (autoWrap)
                result = Methods.ApplyAutoSubtitleWrapAfterReplace(result);

            return result;
        }

        // ---- Preview ----

        private struct NormalizePreviewEntry
        {
            public int EntryIndex;
            public string Before;
            public string After;
        }

        private void RebuildPreview()
        {
            _previewEntries = new List<NormalizePreviewEntry>();
            if (_texts == null || _texts.Count == 0) return;

            bool dotToChinese = _checkDotToChinese.Checked;
            bool removeCjkBlanks = _checkRemoveCjkBlanks.Checked;
            bool autoWrap = _checkAutoWrap.Checked;
            bool normalizePunctuation = _checkNormalizePunctuation.Checked;

            if (!dotToChinese && !removeCjkBlanks && !autoWrap && !normalizePunctuation) return;

            for (int i = 0; i < _texts.Count; i++)
            {
                string translation = _texts[i].actorSpeechTranslation ?? "";
                if (string.IsNullOrEmpty(translation)) continue;

                string normalized = NormalizeTranslation(translation,
                    dotToChinese, removeCjkBlanks, autoWrap, normalizePunctuation);

                if (!string.Equals(translation, normalized, StringComparison.Ordinal))
                {
                    _previewEntries.Add(new NormalizePreviewEntry
                    {
                        EntryIndex = i,
                        Before = translation,
                        After = normalized
                    });
                }
            }

            _previewDirty = false;
        }

        private void OnPreviewChanges(object sender, EventArgs e)
        {
            if (_texts == null || _texts.Count == 0) { Log("No file loaded."); return; }
            if (!AnyNormalizationEnabled) { Log("No normalization options selected."); return; }

            if (_previewDirty) RebuildPreview();

            _listPreview.Items.Clear();

            if (_previewEntries.Count == 0)
            {
                _lblPreviewHeader.Text = "Preview: 0 texts would be modified — all text is already normalized.";
                ShowPreview();
                _lblNormalizeStats.Text = "No changes needed.";
                return;
            }

            // Stats
            int dotsChanged = 0, blanksRemoved = 0, newlinesInserted = 0, punctMoved = 0;
            foreach (var entry in _previewEntries)
            {
                int beforeNewlines = entry.Before.Split('\n').Length;
                int afterNewlines = entry.After.Split('\n').Length;
                if (afterNewlines > beforeNewlines) newlinesInserted += (afterNewlines - beforeNewlines);
                if (entry.Before.Contains(".") && entry.After.Contains("。")) dotsChanged++;
                // Count blanks removed: count spaces in "before" that don't appear in "after"
                int blankDiff = entry.Before.Count(c => c == ' ') - entry.After.Count(c => c == ' ');
                if (blankDiff > 0) blanksRemoved += blankDiff;
                // Count punctuation moved: entries modified by NormalizePunctuationBeforeNewline
                if (_checkNormalizePunctuation.Checked && entry.Before.Contains("\n"))
                    punctMoved++;
            }

            var statParts = new List<string>();
            statParts.Add($"{_previewEntries.Count} texts would be modified");
            if (dotsChanged > 0) statParts.Add($"{dotsChanged} dots → 。");
            if (blanksRemoved > 0) statParts.Add($"{blanksRemoved} CJK blanks removed");
            if (newlinesInserted > 0) statParts.Add($"{newlinesInserted} newlines inserted");
            if (punctMoved > 0) statParts.Add($"{punctMoved} punctuation normalized");
            _lblNormalizeStats.Text = string.Join(" · ", statParts);

            // Populate list (max 200 to avoid UI freeze)
            int maxShow = Math.Min(_previewEntries.Count, 200);
            for (int i = 0; i < maxShow; i++)
            {
                var entry = _previewEntries[i];
                string beforeDisplay = TruncateForDisplay(entry.Before, 55);
                string afterDisplay = TruncateForDisplay(entry.After, 55);
                var item = new ListViewItem((entry.EntryIndex + 1).ToString());
                item.SubItems.Add(beforeDisplay);
                item.SubItems.Add(afterDisplay);
                item.Tag = entry.EntryIndex;
                _listPreview.Items.Add(item);
            }

            _lblPreviewHeader.Text = $"Preview: {_previewEntries.Count} texts would be modified" +
                (maxShow < _previewEntries.Count ? $" (showing first {maxShow})" : "");
            ShowPreview();
        }

        private static string TruncateForDisplay(string text, int maxLen)
        {
            if (string.IsNullOrEmpty(text)) return "";
            string display = text.Replace("\r\n", "\\n").Replace("\n", "\\n").Replace("\r", "\\n");
            if (display.Length <= maxLen) return display;
            return display.Substring(0, maxLen) + "…";
        }

        private void ShowPreview()
        {
            _grpNormalize.Visible = false;
            _panelPreview.Visible = true;
            _panelPreview.BringToFront();
        }

        private void HidePreview()
        {
            _panelPreview.Visible = false;
            _grpNormalize.Visible = true;
            _grpNormalize.BringToFront();
        }

        private void OnCollapsePreview(object sender, EventArgs e) => HidePreview();

        private void OnPreviewDoubleClick(object sender, EventArgs e)
        {
            if (_listPreview.SelectedItems.Count == 0) return;
            int entryIndex = (int)_listPreview.SelectedItems[0].Tag;
            int targetRow = entryIndex * ROWS_PER_ENTRY + 3; // speechTranslation row
            SelectCell(_gridView, targetRow);
            Log($"Navigated to entry {entryIndex + 1}");
        }

        // ---- Apply ----

        private void OnApplyNormalization(object sender, EventArgs e)
        {
            if (_texts == null || _texts.Count == 0) { Log("No file loaded."); return; }
            if (!AnyNormalizationEnabled) { Log("No normalization options selected."); return; }

            if (_previewDirty) RebuildPreview();

            if (_previewEntries.Count == 0)
            {
                Log("Normalization: all text is already normalized. No changes applied.");
                HidePreview();
                return;
            }

            bool dotToChinese = _checkDotToChinese.Checked;
            bool removeCjkBlanks = _checkRemoveCjkBlanks.Checked;
            bool autoWrap = _checkAutoWrap.Checked;
            bool normalizePunctuation = _checkNormalizePunctuation.Checked;

            int modified = 0;
            foreach (var entry in _previewEntries)
            {
                int baseRow = entry.EntryIndex * ROWS_PER_ENTRY;
                string currentTrans = (_gridView.Rows[baseRow + 3].Cells[1].Value?.ToString() ?? "")
                    .Replace("\r\n", "\n");
                string normalized = NormalizeTranslation(currentTrans,
                    dotToChinese, removeCjkBlanks, autoWrap, normalizePunctuation);

                if (!string.Equals(currentTrans, normalized, StringComparison.Ordinal))
                {
                    _gridView.Rows[baseRow + 3].Cells[1].Value = normalized.Replace("\n", "\r\n");
                    // Highlight briefly
                    _gridView.Rows[baseRow + 3].DefaultCellStyle.BackColor = Color.FromArgb(255, 253, 231);
                    modified++;
                }
            }

            _isDirty = true;
            _previewDirty = true;
            UpdateTitle();
            HidePreview();

            // Build summary
            var parts = new List<string> { $"{modified} texts modified" };
            if (dotToChinese) parts.Add("dots→。");
            if (removeCjkBlanks) parts.Add("CJK blanks removed");
            if (autoWrap) parts.Add("auto-wrapped");
            if (normalizePunctuation) parts.Add("punctuation normalized");
            Log($"Normalization applied: {string.Join(", ", parts)}. Use Save to commit or reload to discard.");

            // Reset highlights after 2 seconds
            var timer = new Timer { Interval = 2000 };
            timer.Tick += (s, args) =>
            {
                timer.Stop();
                timer.Dispose();
                foreach (var entry in _previewEntries)
                {
                    int baseRow = entry.EntryIndex * ROWS_PER_ENTRY;
                    if (baseRow + 3 < _gridView.Rows.Count)
                    {
                        var back = entry.EntryIndex % 2 == 0 ? SystemColors.Window : Color.FromArgb(245, 248, 252);
                        _gridView.Rows[baseRow + 3].DefaultCellStyle.BackColor = back;
                    }
                }
            };
            timer.Start();
        }

        // ========== Form close ==========

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isDirty)
            {
                var r = MessageBox.Show("Unsaved changes.\n\nSave before closing?", "Unsaved Changes",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (r == DialogResult.Yes) Save();
                else if (r == DialogResult.Cancel) { e.Cancel = true; return; }
            }
            if (_findReplaceDlg != null && !_findReplaceDlg.IsDisposed)
            {
                _findReplaceDlg.Close();
                _findReplaceDlg.Dispose();
                _findReplaceDlg = null;
            }
        }

        private void UpdateTitle()
        {
            Text = "Landb Normalizer" + (_isDirty ? " [*]" : "");
        }

        private void Log(string message)
        {
            if (_txtLog.InvokeRequired)
                _txtLog.Invoke(new Action<string>(Log), message);
            else
            {
                _txtLog.AppendText(message + Environment.NewLine);
                _txtLog.SelectionStart = _txtLog.TextLength;
                _txtLog.ScrollToCaret();
            }
        }

        private void _grpNormalize_Enter(object sender, EventArgs e)
        {

        }
    }
}
