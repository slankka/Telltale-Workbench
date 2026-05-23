namespace TTG_Tools
{
    partial class LandbEditor
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this._menuStrip = new System.Windows.Forms.MenuStrip();
            this._fileMenu = new System.Windows.Forms.ToolStripMenuItem();
            this._openDirMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._exportCharsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this._closeMenu = new System.Windows.Forms.ToolStripMenuItem();
            this._editMenu = new System.Windows.Forms.ToolStripMenuItem();
            this._findMenu = new System.Windows.Forms.ToolStripMenuItem();
            this._findNextMenu = new System.Windows.Forms.ToolStripMenuItem();
            this._normalizeMenu = new System.Windows.Forms.ToolStripMenuItem();
            this._menuCheckDotToChinese = new System.Windows.Forms.ToolStripMenuItem();
            this._menuCheckRemoveCjkBlanks = new System.Windows.Forms.ToolStripMenuItem();
            this._menuCheckAutoWrap = new System.Windows.Forms.ToolStripMenuItem();
            this._menuCheckNormalizePunctuation = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this._menuPreviewChanges = new System.Windows.Forms.ToolStripMenuItem();
            this._menuApplyNormalization = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this._menuNormalizeInFiles = new System.Windows.Forms.ToolStripMenuItem();
            this._splitMain = new System.Windows.Forms.SplitContainer();
            this._panelTree = new System.Windows.Forms.Panel();
            this._treeView = new System.Windows.Forms.TreeView();
            this._panelRight = new System.Windows.Forms.Panel();
            this._splitRight = new System.Windows.Forms.SplitContainer();
            this._panelGrid = new System.Windows.Forms.Panel();
            this._lblFileInfo = new System.Windows.Forms.Label();
            this._gridView = new System.Windows.Forms.DataGridView();
            this._colField = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._colValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._flowButtons = new System.Windows.Forms.FlowLayoutPanel();
            this._btnSave = new System.Windows.Forms.Button();
            this._btnSaveAs = new System.Windows.Forms.Button();
            this._splitBottom = new System.Windows.Forms.SplitContainer();
            this._panelNormalizeHost = new System.Windows.Forms.Panel();
            this._grpNormalize = new System.Windows.Forms.GroupBox();
            this._checkDotToChinese = new System.Windows.Forms.CheckBox();
            this._checkRemoveCjkBlanks = new System.Windows.Forms.CheckBox();
            this._checkAutoWrap = new System.Windows.Forms.CheckBox();
            this._checkNormalizePunctuation = new System.Windows.Forms.CheckBox();
            this._btnPreviewChanges = new System.Windows.Forms.Button();
            this._btnApplyNormalization = new System.Windows.Forms.Button();
            this._lblNormalizeStats = new System.Windows.Forms.Label();
            this._panelPreview = new System.Windows.Forms.Panel();
            this._previewHeaderLayout = new System.Windows.Forms.TableLayoutPanel();
            this._lblPreviewHeader = new System.Windows.Forms.Label();
            this._btnCollapsePreview = new System.Windows.Forms.Button();
            this._btnApplySelected = new System.Windows.Forms.Button();
            this._btnApplyFromPreview = new System.Windows.Forms.Button();
            this._listPreview = new System.Windows.Forms.ListView();
            this._colPreviewNum = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._colPreviewBefore = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._colPreviewAfter = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._txtLog = new System.Windows.Forms.TextBox();
            this._topBar = new System.Windows.Forms.Panel();
            this._topBarLayout = new System.Windows.Forms.TableLayoutPanel();
            this._txtPath = new System.Windows.Forms.TextBox();
            this._btnBrowse = new System.Windows.Forms.Button();
            this._txtEntryId = new System.Windows.Forms.TextBox();
            this._btnGoToEntry = new System.Windows.Forms.Button();
            this._menuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._splitMain)).BeginInit();
            this._splitMain.Panel1.SuspendLayout();
            this._splitMain.Panel2.SuspendLayout();
            this._splitMain.SuspendLayout();
            this._panelTree.SuspendLayout();
            this._panelRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._splitRight)).BeginInit();
            this._splitRight.Panel1.SuspendLayout();
            this._splitRight.Panel2.SuspendLayout();
            this._splitRight.SuspendLayout();
            this._panelGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridView)).BeginInit();
            this._flowButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._splitBottom)).BeginInit();
            this._splitBottom.Panel1.SuspendLayout();
            this._splitBottom.Panel2.SuspendLayout();
            this._splitBottom.SuspendLayout();
            this._panelNormalizeHost.SuspendLayout();
            this._grpNormalize.SuspendLayout();
            this._panelPreview.SuspendLayout();
            this._previewHeaderLayout.SuspendLayout();
            this._topBar.SuspendLayout();
            this._topBarLayout.SuspendLayout();
            this.SuspendLayout();
            // 
            // _menuStrip
            // 
            this._menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._fileMenu,
            this._editMenu,
            this._normalizeMenu});
            this._menuStrip.Location = new System.Drawing.Point(0, 0);
            this._menuStrip.Name = "_menuStrip";
            this._menuStrip.Size = new System.Drawing.Size(1100, 25);
            this._menuStrip.TabIndex = 0;
            // 
            // _fileMenu
            // 
            this._fileMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._openDirMenu,
            this.toolStripSeparator1,
            this._exportCharsMenu,
            this.toolStripSeparator2,
            this._closeMenu});
            this._fileMenu.Name = "_fileMenu";
            this._fileMenu.Size = new System.Drawing.Size(39, 21);
            this._fileMenu.Text = "File";
            // 
            // _openDirMenu
            // 
            this._openDirMenu.Name = "_openDirMenu";
            this._openDirMenu.Size = new System.Drawing.Size(207, 22);
            this._openDirMenu.Text = "Open Directory...";
            this._openDirMenu.Click += new System.EventHandler(this.OnOpenDir);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(204, 6);
            // 
            // _exportCharsMenu
            // 
            this._exportCharsMenu.Name = "_exportCharsMenu";
            this._exportCharsMenu.Size = new System.Drawing.Size(207, 22);
            this._exportCharsMenu.Text = "Export All Characters...";
            this._exportCharsMenu.Click += new System.EventHandler(this.OnExportChars);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(204, 6);
            // 
            // _closeMenu
            // 
            this._closeMenu.Name = "_closeMenu";
            this._closeMenu.Size = new System.Drawing.Size(207, 22);
            this._closeMenu.Text = "Close";
            this._closeMenu.Click += new System.EventHandler(this.OnCloseMenu);
            // 
            // _editMenu
            // 
            this._editMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._findMenu,
            this._findNextMenu});
            this._editMenu.Name = "_editMenu";
            this._editMenu.Size = new System.Drawing.Size(42, 21);
            this._editMenu.Text = "Edit";
            // 
            // _findMenu
            // 
            this._findMenu.Name = "_findMenu";
            this._findMenu.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this._findMenu.Size = new System.Drawing.Size(216, 22);
            this._findMenu.Text = "Find && Replace...";
            this._findMenu.Click += new System.EventHandler(this.OnFindOpen);
            // 
            // _findNextMenu
            // 
            this._findNextMenu.Name = "_findNextMenu";
            this._findNextMenu.ShortcutKeys = System.Windows.Forms.Keys.F3;
            this._findNextMenu.Size = new System.Drawing.Size(216, 22);
            this._findNextMenu.Text = "Find Next";
            this._findNextMenu.Click += new System.EventHandler(this.OnFindNextMenu);
            // 
            // _normalizeMenu
            // 
            this._normalizeMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._menuCheckDotToChinese,
            this._menuCheckRemoveCjkBlanks,
            this._menuCheckAutoWrap,
            this._menuCheckNormalizePunctuation,
            this.toolStripSeparator3,
            this._menuPreviewChanges,
            this._menuApplyNormalization,
            this.toolStripSeparator4,
            this._menuNormalizeInFiles});
            this._normalizeMenu.Name = "_normalizeMenu";
            this._normalizeMenu.Size = new System.Drawing.Size(80, 21);
            this._normalizeMenu.Text = "Normalize";
            // 
            // _menuCheckDotToChinese
            // 
            this._menuCheckDotToChinese.CheckOnClick = true;
            this._menuCheckDotToChinese.Name = "_menuCheckDotToChinese";
            this._menuCheckDotToChinese.Size = new System.Drawing.Size(279, 22);
            this._menuCheckDotToChinese.Text = "Replace dots to Chinese period";
            this._menuCheckDotToChinese.CheckedChanged += new System.EventHandler(this.OnMenuCheckDotToChineseChanged);
            // 
            // _menuCheckRemoveCjkBlanks
            // 
            this._menuCheckRemoveCjkBlanks.CheckOnClick = true;
            this._menuCheckRemoveCjkBlanks.Name = "_menuCheckRemoveCjkBlanks";
            this._menuCheckRemoveCjkBlanks.Size = new System.Drawing.Size(279, 22);
            this._menuCheckRemoveCjkBlanks.Text = "Remove blanks between CJK chars";
            this._menuCheckRemoveCjkBlanks.CheckedChanged += new System.EventHandler(this.OnMenuCheckRemoveCjkBlanksChanged);
            // 
            // _menuCheckAutoWrap
            // 
            this._menuCheckAutoWrap.CheckOnClick = true;
            this._menuCheckAutoWrap.Name = "_menuCheckAutoWrap";
            this._menuCheckAutoWrap.Size = new System.Drawing.Size(279, 22);
            this._menuCheckAutoWrap.Text = "Auto-wrap long subtitles (insert \\n)";
            this._menuCheckAutoWrap.CheckedChanged += new System.EventHandler(this.OnMenuCheckAutoWrapChanged);
            // 
            // _menuCheckNormalizePunctuation
            // 
            this._menuCheckNormalizePunctuation.CheckOnClick = true;
            this._menuCheckNormalizePunctuation.Name = "_menuCheckNormalizePunctuation";
            this._menuCheckNormalizePunctuation.Size = new System.Drawing.Size(279, 22);
            this._menuCheckNormalizePunctuation.Text = "Normalize punctuation before \\n";
            this._menuCheckNormalizePunctuation.CheckedChanged += new System.EventHandler(this.OnMenuCheckNormalizePunctuationChanged);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(276, 6);
            // 
            // _menuPreviewChanges
            // 
            this._menuPreviewChanges.Name = "_menuPreviewChanges";
            this._menuPreviewChanges.Size = new System.Drawing.Size(279, 22);
            this._menuPreviewChanges.Text = "Preview Changes";
            this._menuPreviewChanges.Click += new System.EventHandler(this.OnPreviewChanges);
            // 
            // _menuApplyNormalization
            // 
            this._menuApplyNormalization.Name = "_menuApplyNormalization";
            this._menuApplyNormalization.Size = new System.Drawing.Size(279, 22);
            this._menuApplyNormalization.Text = "Apply Normalization";
            this._menuApplyNormalization.Click += new System.EventHandler(this.OnApplyNormalization);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(276, 6);
            // 
            // _menuNormalizeInFiles
            // 
            this._menuNormalizeInFiles.Name = "_menuNormalizeInFiles";
            this._menuNormalizeInFiles.Size = new System.Drawing.Size(279, 22);
            this._menuNormalizeInFiles.Text = "Normalize in Files...";
            this._menuNormalizeInFiles.Click += new System.EventHandler(this.OnNormalizeInFiles);
            // 
            // _splitMain
            // 
            this._splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this._splitMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this._splitMain.Location = new System.Drawing.Point(0, 57);
            this._splitMain.Name = "_splitMain";
            // 
            // _splitMain.Panel1
            // 
            this._splitMain.Panel1.Controls.Add(this._panelTree);
            // 
            // _splitMain.Panel2
            // 
            this._splitMain.Panel2.Controls.Add(this._panelRight);
            this._splitMain.Size = new System.Drawing.Size(1100, 647);
            this._splitMain.SplitterDistance = 274;
            this._splitMain.TabIndex = 2;
            // 
            // _panelTree
            // 
            this._panelTree.Controls.Add(this._treeView);
            this._panelTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this._panelTree.Location = new System.Drawing.Point(0, 0);
            this._panelTree.Name = "_panelTree";
            this._panelTree.Padding = new System.Windows.Forms.Padding(2);
            this._panelTree.Size = new System.Drawing.Size(274, 647);
            this._panelTree.TabIndex = 0;
            // 
            // _treeView
            // 
            this._treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this._treeView.HideSelection = false;
            this._treeView.Location = new System.Drawing.Point(2, 2);
            this._treeView.Name = "_treeView";
            this._treeView.Size = new System.Drawing.Size(270, 643);
            this._treeView.TabIndex = 1;
            this._treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.OnTreeSelect);
            this._treeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.OnTreeDoubleClick);
            this._treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.OnTreeNodeMouseClick);
            // 
            // _panelRight
            // 
            this._panelRight.Controls.Add(this._splitRight);
            this._panelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this._panelRight.Location = new System.Drawing.Point(0, 0);
            this._panelRight.Name = "_panelRight";
            this._panelRight.Size = new System.Drawing.Size(822, 647);
            this._panelRight.TabIndex = 0;
            // 
            // _splitRight
            // 
            this._splitRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this._splitRight.Location = new System.Drawing.Point(0, 0);
            this._splitRight.Name = "_splitRight";
            this._splitRight.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // _splitRight.Panel1
            // 
            this._splitRight.Panel1.Controls.Add(this._panelGrid);
            // 
            // _splitRight.Panel2
            // 
            this._splitRight.Panel2.Controls.Add(this._splitBottom);
            this._splitRight.Size = new System.Drawing.Size(822, 647);
            this._splitRight.SplitterDistance = 362;
            this._splitRight.TabIndex = 0;
            // 
            // _panelGrid
            // 
            this._panelGrid.Controls.Add(this._lblFileInfo);
            this._panelGrid.Controls.Add(this._gridView);
            this._panelGrid.Controls.Add(this._flowButtons);
            this._panelGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this._panelGrid.Location = new System.Drawing.Point(0, 0);
            this._panelGrid.Name = "_panelGrid";
            this._panelGrid.Size = new System.Drawing.Size(822, 362);
            this._panelGrid.TabIndex = 0;
            // 
            // _lblFileInfo
            // 
            this._lblFileInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._lblFileInfo.Location = new System.Drawing.Point(4, 4);
            this._lblFileInfo.Name = "_lblFileInfo";
            this._lblFileInfo.Size = new System.Drawing.Size(814, 20);
            this._lblFileInfo.TabIndex = 0;
            this._lblFileInfo.Text = "No file loaded";
            this._lblFileInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _gridView
            // 
            this._gridView.AllowUserToAddRows = false;
            this._gridView.AllowUserToDeleteRows = false;
            this._gridView.AllowUserToResizeRows = false;
            this._gridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._gridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this._gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._gridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this._colField,
            this._colValue});
            this._gridView.Location = new System.Drawing.Point(4, 26);
            this._gridView.Name = "_gridView";
            this._gridView.RowHeadersWidth = 50;
            this._gridView.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this._gridView.Size = new System.Drawing.Size(818, 300);
            this._gridView.TabIndex = 1;
            this._gridView.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.OnCellValidatingHandler);
            this._gridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.OnCellChangedHandler);
            // 
            // _colField
            // 
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this._colField.DefaultCellStyle = dataGridViewCellStyle1;
            this._colField.HeaderText = "Field";
            this._colField.Name = "_colField";
            this._colField.ReadOnly = true;
            this._colField.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this._colField.Width = 120;
            // 
            // _colValue
            // 
            this._colValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this._colValue.DefaultCellStyle = dataGridViewCellStyle2;
            this._colValue.HeaderText = "Value";
            this._colValue.Name = "_colValue";
            this._colValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // _flowButtons
            // 
            this._flowButtons.Controls.Add(this._btnSave);
            this._flowButtons.Controls.Add(this._btnSaveAs);
            this._flowButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._flowButtons.Location = new System.Drawing.Point(0, 326);
            this._flowButtons.Name = "_flowButtons";
            this._flowButtons.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this._flowButtons.Size = new System.Drawing.Size(822, 36);
            this._flowButtons.TabIndex = 2;
            // 
            // _btnSave
            // 
            this._btnSave.Enabled = false;
            this._btnSave.Location = new System.Drawing.Point(3, 6);
            this._btnSave.Name = "_btnSave";
            this._btnSave.Size = new System.Drawing.Size(72, 25);
            this._btnSave.TabIndex = 0;
            this._btnSave.Text = "Save";
            this._btnSave.UseVisualStyleBackColor = true;
            this._btnSave.Click += new System.EventHandler(this.OnSave);
            // 
            // _btnSaveAs
            // 
            this._btnSaveAs.Enabled = false;
            this._btnSaveAs.Location = new System.Drawing.Point(81, 6);
            this._btnSaveAs.Name = "_btnSaveAs";
            this._btnSaveAs.Size = new System.Drawing.Size(86, 25);
            this._btnSaveAs.TabIndex = 1;
            this._btnSaveAs.Text = "Save As...";
            this._btnSaveAs.UseVisualStyleBackColor = true;
            this._btnSaveAs.Click += new System.EventHandler(this.OnSaveAs);
            // 
            // _splitBottom
            // 
            this._splitBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this._splitBottom.Location = new System.Drawing.Point(0, 0);
            this._splitBottom.Name = "_splitBottom";
            this._splitBottom.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // _splitBottom.Panel1
            // 
            this._splitBottom.Panel1.Controls.Add(this._panelNormalizeHost);
            // 
            // _splitBottom.Panel2
            // 
            this._splitBottom.Panel2.Controls.Add(this._txtLog);
            this._splitBottom.Size = new System.Drawing.Size(822, 281);
            this._splitBottom.SplitterDistance = 114;
            this._splitBottom.TabIndex = 0;
            // 
            // _panelNormalizeHost
            // 
            this._panelNormalizeHost.Controls.Add(this._grpNormalize);
            this._panelNormalizeHost.Controls.Add(this._panelPreview);
            this._panelNormalizeHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this._panelNormalizeHost.Location = new System.Drawing.Point(0, 0);
            this._panelNormalizeHost.Name = "_panelNormalizeHost";
            this._panelNormalizeHost.Size = new System.Drawing.Size(822, 114);
            this._panelNormalizeHost.TabIndex = 0;
            // 
            // _grpNormalize
            // 
            this._grpNormalize.Controls.Add(this._checkDotToChinese);
            this._grpNormalize.Controls.Add(this._checkRemoveCjkBlanks);
            this._grpNormalize.Controls.Add(this._checkAutoWrap);
            this._grpNormalize.Controls.Add(this._checkNormalizePunctuation);
            this._grpNormalize.Controls.Add(this._btnPreviewChanges);
            this._grpNormalize.Controls.Add(this._btnApplyNormalization);
            this._grpNormalize.Controls.Add(this._lblNormalizeStats);
            this._grpNormalize.Dock = System.Windows.Forms.DockStyle.Fill;
            this._grpNormalize.Location = new System.Drawing.Point(0, 0);
            this._grpNormalize.Name = "_grpNormalize";
            this._grpNormalize.Size = new System.Drawing.Size(822, 114);
            this._grpNormalize.TabIndex = 0;
            this._grpNormalize.TabStop = false;
            this._grpNormalize.Text = "Normalization";
            this._grpNormalize.Enter += new System.EventHandler(this._grpNormalize_Enter);
            // 
            // _checkDotToChinese
            // 
            this._checkDotToChinese.AutoSize = true;
            this._checkDotToChinese.Location = new System.Drawing.Point(10, 20);
            this._checkDotToChinese.Name = "_checkDotToChinese";
            this._checkDotToChinese.Size = new System.Drawing.Size(175, 17);
            this._checkDotToChinese.TabIndex = 0;
            this._checkDotToChinese.Text = "Replace dots to Chinese period";
            this._checkDotToChinese.UseVisualStyleBackColor = true;
            this._checkDotToChinese.CheckedChanged += new System.EventHandler(this.OnCheckDotToChineseChanged);
            // 
            // _checkRemoveCjkBlanks
            // 
            this._checkRemoveCjkBlanks.AutoSize = true;
            this._checkRemoveCjkBlanks.Location = new System.Drawing.Point(256, 20);
            this._checkRemoveCjkBlanks.Name = "_checkRemoveCjkBlanks";
            this._checkRemoveCjkBlanks.Size = new System.Drawing.Size(193, 17);
            this._checkRemoveCjkBlanks.TabIndex = 1;
            this._checkRemoveCjkBlanks.Text = "Remove blanks between CJK chars";
            this._checkRemoveCjkBlanks.UseVisualStyleBackColor = true;
            this._checkRemoveCjkBlanks.CheckedChanged += new System.EventHandler(this.OnCheckRemoveCjkBlanksChanged);
            // 
            // _checkAutoWrap
            // 
            this._checkAutoWrap.AutoSize = true;
            this._checkAutoWrap.Location = new System.Drawing.Point(10, 42);
            this._checkAutoWrap.Name = "_checkAutoWrap";
            this._checkAutoWrap.Size = new System.Drawing.Size(194, 17);
            this._checkAutoWrap.TabIndex = 2;
            this._checkAutoWrap.Text = "Auto-wrap long subtitles (insert \\n)";
            this._checkAutoWrap.UseVisualStyleBackColor = true;
            this._checkAutoWrap.CheckedChanged += new System.EventHandler(this.OnCheckAutoWrapChanged);
            // 
            // _checkNormalizePunctuation
            // 
            this._checkNormalizePunctuation.AutoSize = true;
            this._checkNormalizePunctuation.Location = new System.Drawing.Point(256, 42);
            this._checkNormalizePunctuation.Name = "_checkNormalizePunctuation";
            this._checkNormalizePunctuation.Size = new System.Drawing.Size(180, 17);
            this._checkNormalizePunctuation.TabIndex = 3;
            this._checkNormalizePunctuation.Text = "Normalize punctuation before \\n";
            this._checkNormalizePunctuation.UseVisualStyleBackColor = true;
            this._checkNormalizePunctuation.CheckedChanged += new System.EventHandler(this.OnCheckNormalizePunctuationChanged);
            // 
            // _btnPreviewChanges
            // 
            this._btnPreviewChanges.Location = new System.Drawing.Point(10, 68);
            this._btnPreviewChanges.Name = "_btnPreviewChanges";
            this._btnPreviewChanges.Size = new System.Drawing.Size(130, 24);
            this._btnPreviewChanges.TabIndex = 4;
            this._btnPreviewChanges.Text = "🔍 Preview Changes...";
            this._btnPreviewChanges.UseVisualStyleBackColor = true;
            this._btnPreviewChanges.Click += new System.EventHandler(this.OnPreviewChanges);
            // 
            // _btnApplyNormalization
            // 
            this._btnApplyNormalization.Location = new System.Drawing.Point(146, 68);
            this._btnApplyNormalization.Name = "_btnApplyNormalization";
            this._btnApplyNormalization.Size = new System.Drawing.Size(130, 24);
            this._btnApplyNormalization.TabIndex = 5;
            this._btnApplyNormalization.Text = "✅ Apply Normalization";
            this._btnApplyNormalization.UseVisualStyleBackColor = true;
            this._btnApplyNormalization.Click += new System.EventHandler(this.OnApplyNormalization);
            // 
            // _lblNormalizeStats
            // 
            this._lblNormalizeStats.AutoSize = true;
            this._lblNormalizeStats.Location = new System.Drawing.Point(290, 73);
            this._lblNormalizeStats.Name = "_lblNormalizeStats";
            this._lblNormalizeStats.Size = new System.Drawing.Size(0, 13);
            this._lblNormalizeStats.TabIndex = 6;
            // 
            // _panelPreview
            // 
            this._panelPreview.Controls.Add(this._previewHeaderLayout);
            this._panelPreview.Controls.Add(this._listPreview);
            this._panelPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this._panelPreview.Location = new System.Drawing.Point(0, 0);
            this._panelPreview.Name = "_panelPreview";
            this._panelPreview.Size = new System.Drawing.Size(822, 114);
            this._panelPreview.TabIndex = 1;
            this._panelPreview.Visible = false;
            // 
            // _previewHeaderLayout
            // 
            this._previewHeaderLayout.ColumnCount = 4;
            this._previewHeaderLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._previewHeaderLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._previewHeaderLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._previewHeaderLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._previewHeaderLayout.Controls.Add(this._lblPreviewHeader, 0, 0);
            this._previewHeaderLayout.Controls.Add(this._btnCollapsePreview, 1, 0);
            this._previewHeaderLayout.Controls.Add(this._btnApplySelected, 2, 0);
            this._previewHeaderLayout.Controls.Add(this._btnApplyFromPreview, 3, 0);
            this._previewHeaderLayout.Dock = System.Windows.Forms.DockStyle.Top;
            this._previewHeaderLayout.Location = new System.Drawing.Point(0, 0);
            this._previewHeaderLayout.Name = "_previewHeaderLayout";
            this._previewHeaderLayout.RowCount = 1;
            this._previewHeaderLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this._previewHeaderLayout.Size = new System.Drawing.Size(822, 24);
            this._previewHeaderLayout.TabIndex = 4;
            // 
            // _lblPreviewHeader
            // 
            this._lblPreviewHeader.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this._lblPreviewHeader.AutoSize = true;
            this._lblPreviewHeader.Location = new System.Drawing.Point(3, 6);
            this._lblPreviewHeader.Name = "_lblPreviewHeader";
            this._lblPreviewHeader.Size = new System.Drawing.Size(175, 13);
            this._lblPreviewHeader.TabIndex = 0;
            this._lblPreviewHeader.Text = "Preview: 0 texts would be modified";
            // 
            // _btnCollapsePreview
            // 
            this._btnCollapsePreview.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this._btnCollapsePreview.Location = new System.Drawing.Point(563, 3);
            this._btnCollapsePreview.Name = "_btnCollapsePreview";
            this._btnCollapsePreview.Size = new System.Drawing.Size(80, 20);
            this._btnCollapsePreview.TabIndex = 1;
            this._btnCollapsePreview.Text = "✕ Hide/Back";
            this._btnCollapsePreview.UseVisualStyleBackColor = true;
            this._btnCollapsePreview.Click += new System.EventHandler(this.OnCollapsePreview);
            // 
            // _btnApplySelected
            // 
            this._btnApplySelected.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this._btnApplySelected.Enabled = false;
            this._btnApplySelected.Location = new System.Drawing.Point(629, 3);
            this._btnApplySelected.Name = "_btnApplySelected";
            this._btnApplySelected.Size = new System.Drawing.Size(92, 20);
            this._btnApplySelected.TabIndex = 5;
            this._btnApplySelected.Text = "✅ Apply Selected";
            this._btnApplySelected.UseVisualStyleBackColor = true;
            this._btnApplySelected.Click += new System.EventHandler(this.OnApplySelected);
            // 
            // _btnApplyFromPreview
            // 
            this._btnApplyFromPreview.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this._btnApplyFromPreview.Location = new System.Drawing.Point(727, 3);
            this._btnApplyFromPreview.Name = "_btnApplyFromPreview";
            this._btnApplyFromPreview.Size = new System.Drawing.Size(92, 20);
            this._btnApplyFromPreview.TabIndex = 2;
            this._btnApplyFromPreview.Text = "✅ Apply All";
            this._btnApplyFromPreview.UseVisualStyleBackColor = true;
            this._btnApplyFromPreview.Click += new System.EventHandler(this.OnApplyNormalization);
            // 
            // _listPreview
            // 
            this._listPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._listPreview.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._colPreviewNum,
            this._colPreviewBefore,
            this._colPreviewAfter});
            this._listPreview.FullRowSelect = true;
            this._listPreview.GridLines = true;
            this._listPreview.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this._listPreview.HideSelection = false;
            this._listPreview.Location = new System.Drawing.Point(0, 26);
            this._listPreview.MultiSelect = false;
            this._listPreview.Name = "_listPreview";
            this._listPreview.Size = new System.Drawing.Size(822, 114);
            this._listPreview.TabIndex = 3;
            this._listPreview.UseCompatibleStateImageBehavior = false;
            this._listPreview.View = System.Windows.Forms.View.Details;
            this._listPreview.SelectedIndexChanged += new System.EventHandler(this.OnPreviewSelectionChanged);
            this._listPreview.DoubleClick += new System.EventHandler(this.OnPreviewDoubleClick);
            // 
            // _colPreviewNum
            // 
            this._colPreviewNum.Text = "#";
            this._colPreviewNum.Width = 50;
            // 
            // _colPreviewBefore
            // 
            this._colPreviewBefore.Text = "Before";
            this._colPreviewBefore.Width = 400;
            // 
            // _colPreviewAfter
            // 
            this._colPreviewAfter.Text = "After";
            this._colPreviewAfter.Width = 400;
            // 
            // _txtLog
            // 
            this._txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this._txtLog.Location = new System.Drawing.Point(0, 0);
            this._txtLog.Multiline = true;
            this._txtLog.Name = "_txtLog";
            this._txtLog.ReadOnly = true;
            this._txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._txtLog.Size = new System.Drawing.Size(822, 163);
            this._txtLog.TabIndex = 1;
            // 
            // _topBar
            // 
            this._topBar.Controls.Add(this._topBarLayout);
            this._topBar.Dock = System.Windows.Forms.DockStyle.Top;
            this._topBar.Location = new System.Drawing.Point(0, 25);
            this._topBar.Name = "_topBar";
            this._topBar.Size = new System.Drawing.Size(1100, 32);
            this._topBar.TabIndex = 0;
            // 
            // _topBarLayout
            // 
            this._topBarLayout.ColumnCount = 4;
            this._topBarLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._topBarLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._topBarLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._topBarLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._topBarLayout.Controls.Add(this._txtPath, 0, 0);
            this._topBarLayout.Controls.Add(this._btnBrowse, 1, 0);
            this._topBarLayout.Controls.Add(this._txtEntryId, 2, 0);
            this._topBarLayout.Controls.Add(this._btnGoToEntry, 3, 0);
            this._topBarLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this._topBarLayout.Location = new System.Drawing.Point(0, 0);
            this._topBarLayout.Name = "_topBarLayout";
            this._topBarLayout.RowCount = 1;
            this._topBarLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._topBarLayout.Size = new System.Drawing.Size(1100, 32);
            this._topBarLayout.TabIndex = 0;
            // 
            // _txtPath
            // 
            this._txtPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._txtPath.Location = new System.Drawing.Point(3, 5);
            this._txtPath.Name = "_txtPath";
            this._txtPath.Size = new System.Drawing.Size(881, 21);
            this._txtPath.TabIndex = 0;
            // 
            // _btnBrowse
            // 
            this._btnBrowse.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this._btnBrowse.Location = new System.Drawing.Point(890, 4);
            this._btnBrowse.Name = "_btnBrowse";
            this._btnBrowse.Size = new System.Drawing.Size(72, 23);
            this._btnBrowse.TabIndex = 1;
            this._btnBrowse.Text = "Browse...";
            this._btnBrowse.UseVisualStyleBackColor = true;
            this._btnBrowse.Click += new System.EventHandler(this.OnBrowse);
            // 
            // _txtEntryId
            // 
            this._txtEntryId.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this._txtEntryId.Location = new System.Drawing.Point(968, 5);
            this._txtEntryId.Name = "_txtEntryId";
            this._txtEntryId.Size = new System.Drawing.Size(60, 21);
            this._txtEntryId.TabIndex = 2;
            this._txtEntryId.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnEntryIdKeyDown);
            // 
            // _btnGoToEntry
            // 
            this._btnGoToEntry.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this._btnGoToEntry.Location = new System.Drawing.Point(1034, 4);
            this._btnGoToEntry.Name = "_btnGoToEntry";
            this._btnGoToEntry.Size = new System.Drawing.Size(63, 23);
            this._btnGoToEntry.TabIndex = 3;
            this._btnGoToEntry.Text = "Go";
            this._btnGoToEntry.UseVisualStyleBackColor = true;
            this._btnGoToEntry.Click += new System.EventHandler(this.OnGoToEntry);
            // 
            // LandbNormalizer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 704);
            this.Controls.Add(this._splitMain);
            this.Controls.Add(this._topBar);
            this.Controls.Add(this._menuStrip);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this._menuStrip;
            this.MinimumSize = new System.Drawing.Size(800, 538);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Name = "LandbEditor";
            this.Text = "Landb Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OnFormKeyDown);
            this._menuStrip.ResumeLayout(false);
            this._menuStrip.PerformLayout();
            this._splitMain.Panel1.ResumeLayout(false);
            this._splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._splitMain)).EndInit();
            this._splitMain.ResumeLayout(false);
            this._panelTree.ResumeLayout(false);
            this._panelRight.ResumeLayout(false);
            this._splitRight.Panel1.ResumeLayout(false);
            this._splitRight.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._splitRight)).EndInit();
            this._splitRight.ResumeLayout(false);
            this._panelGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._gridView)).EndInit();
            this._flowButtons.ResumeLayout(false);
            this._splitBottom.Panel1.ResumeLayout(false);
            this._splitBottom.Panel2.ResumeLayout(false);
            this._splitBottom.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this._splitBottom)).EndInit();
            this._splitBottom.ResumeLayout(false);
            this._panelNormalizeHost.ResumeLayout(false);
            this._grpNormalize.ResumeLayout(false);
            this._grpNormalize.PerformLayout();
            this._panelPreview.ResumeLayout(false);
            this._previewHeaderLayout.ResumeLayout(false);
            this._previewHeaderLayout.PerformLayout();
            this._topBar.ResumeLayout(false);
            this._topBarLayout.ResumeLayout(false);
            this._topBarLayout.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        // Menu
        private System.Windows.Forms.MenuStrip _menuStrip;
        private System.Windows.Forms.ToolStripMenuItem _fileMenu;
        private System.Windows.Forms.ToolStripMenuItem _openDirMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem _exportCharsMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem _closeMenu;
        private System.Windows.Forms.ToolStripMenuItem _editMenu;
        private System.Windows.Forms.ToolStripMenuItem _findMenu;
        private System.Windows.Forms.ToolStripMenuItem _findNextMenu;
        private System.Windows.Forms.ToolStripMenuItem _normalizeMenu;
        private System.Windows.Forms.ToolStripMenuItem _menuCheckDotToChinese;
        private System.Windows.Forms.ToolStripMenuItem _menuCheckRemoveCjkBlanks;
        private System.Windows.Forms.ToolStripMenuItem _menuCheckAutoWrap;
        private System.Windows.Forms.ToolStripMenuItem _menuCheckNormalizePunctuation;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem _menuPreviewChanges;
        private System.Windows.Forms.ToolStripMenuItem _menuApplyNormalization;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem _menuNormalizeInFiles;

        // Layout
        private System.Windows.Forms.SplitContainer _splitMain;
        private System.Windows.Forms.Panel _panelTree;
        private System.Windows.Forms.Panel _topBar;
        private System.Windows.Forms.TableLayoutPanel _topBarLayout;
        private System.Windows.Forms.TextBox _txtPath;
        private System.Windows.Forms.Button _btnBrowse;
        private System.Windows.Forms.TextBox _txtEntryId;
        private System.Windows.Forms.Button _btnGoToEntry;
        private System.Windows.Forms.TreeView _treeView;
        private System.Windows.Forms.Panel _panelRight;
        private System.Windows.Forms.SplitContainer _splitRight;
        private System.Windows.Forms.Panel _panelGrid;
        private System.Windows.Forms.Label _lblFileInfo;
        private System.Windows.Forms.DataGridView _gridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn _colField;
        private System.Windows.Forms.DataGridViewTextBoxColumn _colValue;
        private System.Windows.Forms.FlowLayoutPanel _flowButtons;
        private System.Windows.Forms.Button _btnSave;
        private System.Windows.Forms.Button _btnSaveAs;
        private System.Windows.Forms.SplitContainer _splitBottom;

        // Normalization
        private System.Windows.Forms.Panel _panelNormalizeHost;
        private System.Windows.Forms.GroupBox _grpNormalize;
        private System.Windows.Forms.CheckBox _checkDotToChinese;
        private System.Windows.Forms.CheckBox _checkRemoveCjkBlanks;
        private System.Windows.Forms.CheckBox _checkAutoWrap;
        private System.Windows.Forms.CheckBox _checkNormalizePunctuation;
        private System.Windows.Forms.Button _btnPreviewChanges;
        private System.Windows.Forms.Button _btnApplyNormalization;
        private System.Windows.Forms.Label _lblNormalizeStats;

        // Preview
        private System.Windows.Forms.Panel _panelPreview;
        private System.Windows.Forms.TableLayoutPanel _previewHeaderLayout;
        private System.Windows.Forms.Label _lblPreviewHeader;
        private System.Windows.Forms.Button _btnCollapsePreview;
        private System.Windows.Forms.Button _btnApplyFromPreview;
        private System.Windows.Forms.Button _btnApplySelected;
        private System.Windows.Forms.ListView _listPreview;
        private System.Windows.Forms.ColumnHeader _colPreviewNum;
        private System.Windows.Forms.ColumnHeader _colPreviewBefore;
        private System.Windows.Forms.ColumnHeader _colPreviewAfter;

        // Log
        private System.Windows.Forms.TextBox _txtLog;
    }
}
