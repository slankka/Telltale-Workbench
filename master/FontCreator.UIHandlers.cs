using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TTG_Tools
{
    public partial class FontCreator
    {
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FontCreator_Load(object sender, EventArgs e)
        {
            edited = false; //Tell a program about first launch window form so font is not modified.

            if (AppData.settings.swizzlePS4 || AppData.settings.swizzleNintendoSwitch || AppData.settings.swizzleXbox360 || AppData.settings.swizzlePSVita || AppData.settings.swizzleNintendoWii)
            {
                if (AppData.settings.swizzlePS4) rbPS4Swizzle.Checked = true;
                else if (AppData.settings.swizzlePSVita) rbPSVitaSwizzle.Checked = true;
                else if (AppData.settings.swizzleXbox360) rbXbox360Swizzle.Checked = true;
                else if (AppData.settings.swizzleNintendoWii) rbWiiSwizzle.Checked = true;
                else rbSwitchSwizzle.Checked = true;
            }
            else
            {
                rbNoSwizzle.Checked = true;
            }

            // Load font profiles
            FontProfileList profiles = FontProfileList.Load();
            RefreshProfileComboBox(profiles, null);

            // 固定尺寸窗体：清除 Anchor（避免尺寸变化时控件被拉伸）
            FontCreator_Load_InitLayout();
        }

        // ======== 高 DPI ========
        // 设计尺寸已按 175% 屏（逻辑 1463x775）可用空间定制：
        //   ClientSize = 1373 x 730，缩放后 = 2403 x 1278，加边框 1339，
        //   可放进 1356 的 175% 屏工作区；125%/100% 下更宽裕。
        //
        // 因此**运行时不再做任何布局调整**，控件几何完全由 WinForms 原生 DPI 缩放决定。
        // 之前所有"改 ClientSize / 改控件坐标 / 开关 AutoScroll"的运行时方案都会与
        // 原生缩放或锚定规则冲突，导致 Swizzle 错位、内容被裁、无滚动条等问题。

        private void FontCreator_Load_InitLayout()
        {
            // 固定尺寸窗体：Anchor 无意义且会在尺寸变化时拉伸控件，统一清除
            ClearAnchorsRecursive(this);

            int devDpi = SafeDpi();          // 会话缓存的系统 DPI（登录时确定，可能过期）
            int monDpi = MonitorDpi();       // 显示器实时 DPI（来自 PerMonitorSettings）
            if (devDpi <= 0) devDpi = 96;
            if (monDpi <= 0) monDpi = 96;

            // 控件此刻被 WinForms 按**会话 DPI(devDpi)** 缩放，因此除以 devDpi
            // 就得到真正的 96 DPI 设计基线。
            CaptureDesign96(devDpi);

            // 会话 DPI 与显示器实际 DPI 分叉（改了缩放但未重新登录）时，
            // 按显示器真实 DPI 重排一次，让控件与窗口都回到正确比例。
            if (devDpi != monDpi)
            {
                DpiDiag("[MISMATCH] devDpi=" + devDpi + " monitorDpi=" + monDpi +
                        " ClientSize=" + ClientSize + " design96=" + _design96);
                ApplyDpiToLayout(monDpi);
                DpiDiag("[MISMATCH] corrected ClientSize=" + ClientSize);
            }

            DpiDiag("Load: ClientSize=" + ClientSize + " devDpi=" + devDpi +
                    " monitorDpi=" + monDpi + " design96=" + _design96 +
                    " screen=" + SafeScreenInfo());
        }

        /// <summary>
        /// 记录"96 DPI 下的设计与控件基线"。
        /// <para>
        /// 这是整个 DPI 处理的**唯一真相来源**：
        /// WinForms 的增量缩放会把自己上一次的结果再乘一次（实测拖到 175% 屏时
        /// 控件被放大到 1.25×1.75=2.19 倍，而窗口只有 1.75 倍 → 右侧被裁）。
        /// 因此改为：每次 DPI 变化都从这份基线**重新计算**绝对坐标，
        /// 彻底摆脱增量累积。
        /// </para>
        /// </summary>
        private void CaptureDesign96(int dpi)
        {
            if (dpi <= 0) dpi = 96;
            float s = dpi / 96f;

            _design96 = new Size(
                (int)Math.Round(ClientSize.Width / s),
                (int)Math.Round(ClientSize.Height / s));

            _designBounds96 = new System.Collections.Generic.Dictionary<Control, Rectangle>();
            CaptureBounds96Recursive(this, s);
        }

        private void CaptureBounds96Recursive(Control parent, float s)
        {
            foreach (Control c in parent.Controls)
            {
                _designBounds96[c] = new Rectangle(
                    (int)Math.Round(c.Left / s),
                    (int)Math.Round(c.Top / s),
                    (int)Math.Round(c.Width / s),
                    (int)Math.Round(c.Height / s));

                if (c.HasChildren) CaptureBounds96Recursive(c, s);
            }
        }

        /// <summary>
        /// 按给定 DPI 把窗口与**全部控件**重新排布到绝对坐标。
        /// 每次都从 _design96 / _designBounds96 现算，不依赖任何增量缩放。
        /// </summary>
        private void ApplyDpiToLayout(int dpi)
        {
            if (dpi <= 0) dpi = 96;
            if (_design96.IsEmpty || _designBounds96 == null) return;

            float s = dpi / 96f;

            _restoringSize = true;
            SuspendLayout();
            try
            {
                ClientSize = new Size(
                    (int)Math.Round(_design96.Width * s),
                    (int)Math.Round(_design96.Height * s));

                ApplyBounds96Recursive(this, s);
            }
            finally
            {
                ResumeLayout(true);
                _restoringSize = false;
            }
        }

        private void ApplyBounds96Recursive(Control parent, float s)
        {
            foreach (Control c in parent.Controls)
            {
                Rectangle b;
                if (_designBounds96.TryGetValue(c, out b))
                {
                    c.Bounds = new Rectangle(
                        (int)Math.Round(b.X * s),
                        (int)Math.Round(b.Y * s),
                        (int)Math.Round(b.Width * s),
                        (int)Math.Round(b.Height * s));
                }

                if (c.HasChildren) ApplyBounds96Recursive(c, s);
            }
        }

        /// <summary>96 DPI 下的等效设计客户区尺寸（所有 DPI 的统一换算基准）。</summary>
        private Size _design96;

        /// <summary>96 DPI 下的控件基线坐标（每次 DPI 变化都从这里重算，避免增量累积）。</summary>
        private System.Collections.Generic.Dictionary<Control, Rectangle> _designBounds96;

        private bool _restoringSize;

        // ---- Win32: 查询"窗口所在显示器"的真实 DPI ----
        // 参考 winsiderss/systeminformer#1386（同类问题的行业级案例）：
        //   · 该 issue 确认「跨屏后内容 DPI 变了，但窗口尺寸不会自动跟着变」是
        //     Windows 的既定行为，应用必须自己按 newDpi/oldDpi 调整窗口尺寸。
        //   · 也确认 GetDpiForSystem / GetDpiForMonitor / GetShellWindow 在某些
        //     情况下会「缓存旧值直到重启」——这正是本项目实测到的 DeviceDpi=120 脏值。
        // 因此这里用两个独立来源交叉校验，取二者中"看起来更可信"的值。
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr MonitorFromPoint(POINT pt, uint dwFlags);

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        private struct POINT { public int X; public int Y; }

        [System.Runtime.InteropServices.DllImport("shcore.dll")]
        private static extern int GetDpiForMonitor(IntPtr hmonitor, int dpiType, out uint dpiX, out uint dpiY);

        // GetScaleFactorForMonitor 返回"人类可读百分比"(100/125/150/175)，
        // 与 GetDpiForMonitor 的 DPI 值(96/120/144/168) 是两套单位，可互为校验。
        [System.Runtime.InteropServices.DllImport("shcore.dll")]
        private static extern int GetScaleFactorForMonitor(IntPtr hmonitor, out int scale);

        private const uint MONITOR_DEFAULTTONEAREST = 2;
        private const int MDT_EFFECTIVE_DPI = 0;

        /// <summary>取"本窗体所在显示器"的监视器句柄。Handle 未就绪时用屏幕位置查。</summary>
        private IntPtr CurrentMonitor()
        {
            try
            {
                if (Handle != IntPtr.Zero)
                {
                    IntPtr h = MonitorFromWindow(Handle, MONITOR_DEFAULTTONEAREST);
                    if (h != IntPtr.Zero) return h;
                }
            }
            catch { }

            try
            {
                // Load 阶段 Handle 可能还没建好，改用"窗体中心点"所属显示器。
                // 这对启动时定位（CenterScreen / 上次位置）已经足够准确。
                Rectangle b = Bounds;
                var pt = new POINT
                {
                    X = b.Left + Math.Max(1, b.Width / 2),
                    Y = b.Top + Math.Max(1, b.Height / 2)
                };
                return MonitorFromPoint(pt, MONITOR_DEFAULTTONEAREST);
            }
            catch { }

            return IntPtr.Zero;
        }

        /// <summary>
        /// 窗口所在显示器的真实 DPI。
        /// 优先 GetDpiForMonitor，并用 GetScaleFactorForMonitor 交叉校验：
        /// 若两者换算后不一致（说明其中一个返回了缓存旧值），取"与显示器实时状态更接近"的。
        /// </summary>
        private int MonitorDpi()
        {
            try
            {
                IntPtr hmon = CurrentMonitor();
                if (hmon == IntPtr.Zero) return 96;

                uint dx = 0, dy = 0;
                int hrDpi = GetDpiForMonitor(hmon, MDT_EFFECTIVE_DPI, out dx, out dy);
                int dpiFromDpi = (hrDpi == 0 && dx > 0) ? (int)dx : 0;

                int scale = 0;
                int hrScale = GetScaleFactorForMonitor(hmon, out scale);
                int dpiFromScale = (hrScale == 0 && scale > 0) ? (int)Math.Round(scale * 96.0 / 100.0) : 0;

                if (dpiFromDpi > 0 && dpiFromScale > 0 && dpiFromDpi != dpiFromScale)
                {
                    // 两者不一致：优先采用与「窗口当前实际尺寸」吻合的那个，
                    // 因为 WinForms 就是按真实 DPI 缩放窗体的。
                    int dpiFromSize = DpiImpliedByWindowSize();
                    int pick = (dpiFromSize > 0 &&
                                Math.Abs(dpiFromSize - dpiFromScale) < Math.Abs(dpiFromSize - dpiFromDpi))
                               ? dpiFromScale : dpiFromDpi;
                    DpiDiag("[MonitorDpi] 不一致: GetDpiForMonitor=" + dpiFromDpi +
                            " GetScaleFactorForMonitor=" + dpiFromScale +
                            " 由窗口尺寸推断=" + dpiFromSize + " → 采用 " + pick);
                    return pick;
                }

                if (dpiFromDpi > 0) return dpiFromDpi;
                if (dpiFromScale > 0) return dpiFromScale;
            }
            catch { }

            return 96;
        }

        /// <summary>
        /// 用「当前 ClientSize 相对 design96 的倍数」反推 DPI。
        /// WinForms 的窗体尺寸就是按真实 DPI 缩放的，因此这个推断很准。
        /// </summary>
        private int DpiImpliedByWindowSize()
        {
            if (_design96.IsEmpty || _design96.Width <= 0) return 0;

            double ratio = (double)ClientSize.Width / _design96.Width;
            int dpi = (int)Math.Round(96 * ratio);

            // 只在常见 DPI 档位附近才采信
            int[] known = { 96, 120, 144, 168, 192, 216, 240, 288, 336, 384 };
            int best = 0, bestDiff = int.MaxValue;
            foreach (int k in known)
            {
                int diff = Math.Abs(k - dpi);
                if (diff < bestDiff) { bestDiff = diff; best = k; }
            }
            return bestDiff <= 12 ? best : 0;
        }

        /// <summary>
        /// DPI 变化后：按"窗口所在显示器的真实 DPI"重设客户区尺寸，并把窗口夹进工作区。
        /// 只调整 ClientSize / Location，**绝不修改任何控件坐标**。
        /// </summary>
        private void FontCreator_DpiChanged(object sender, DpiChangedEventArgs e)
        {
            if (IsDisposed) return;

            DpiDiag("DpiChanged ENTER: event old=" + e.DeviceDpiOld + " new=" + e.DeviceDpiNew +
                    " ClientSize=" + ClientSize + " design96=" + _design96 +
                    " screen=" + SafeScreenInfo());

            // 延后到 WinForms 完成自身缩放、且窗口已真正落到新显示器之后再换算
            BeginInvoke(new Action(() =>
            {
                if (IsDisposed || _restoringSize) return;
                ApplyForCurrentScreen();
            }));
        }

        /// <summary>
        /// 按"窗口所在显示器的真实 DPI"重排窗口与全部控件，并保证窗口完整落在工作区内。
        /// </summary>
        private void ApplyForCurrentScreen()
        {
            if (IsDisposed || _restoringSize) return;

            Screen scr;
            try { scr = Screen.FromControl(this); } catch { return; }
            if (scr == null) return;

            int dpi = MonitorDpi();

            DpiDiag("  [apply] monitorDpi=" + dpi + " ClientSize=" + ClientSize +
                    " screen=" + scr.DeviceName);

            // 从 96 DPI 基线直接重算绝对坐标，彻底摆脱 WinForms 的增量缩放累积
            // （实测增量方式会把控件放大到 1.25×1.75=2.19 倍，窗口只有 1.75 倍）。
            ApplyDpiToLayout(dpi);

            FitToWorkAreaAndScroll(scr);
        }

        /// <summary>
        /// 把窗口收进指定屏幕工作区，并在内容超出时启用滚动。
        /// <para>
        /// 高缩放（如 200%）下内容可能比屏幕还大（实测 200% 需要 2738x1390，
        /// 而 2560x1440 屏可用约 2550x1323）。此时无法"完整显示"，
        /// 唯一正确的做法是：窗口压到工作区内 + 滚动条，保证所有内容仍可访问。
        /// </para>
        /// </summary>
        private void FitToWorkAreaAndScroll(Screen scr)
        {
            if (scr == null) return;

            Rectangle wa = scr.WorkingArea;
            int ncH = base.Height - ClientSize.Height;
            int ncW = base.Width - ClientSize.Width;

            int maxClientH = wa.Height - ncH;
            int maxClientW = wa.Width - ncW;

            _restoringSize = true;
            try
            {
                Size target = ClientSize;
                if (maxClientH > 0 && target.Height > maxClientH) target.Height = maxClientH;
                if (maxClientW > 0 && target.Width > maxClientW) target.Width = maxClientW;

                if (target != ClientSize)
                    ClientSize = target;

                // 按控件实际占用决定是否需要滚动
                int needW = 0, needH = 0;
                foreach (Control c in Controls)
                {
                    if (!c.Visible || c is MenuStrip) continue;
                    if (c.Right > needW) needW = c.Right;
                    if (c.Bottom > needH) needH = c.Bottom;
                }

                if (needW > ClientSize.Width || needH > ClientSize.Height)
                {
                    AutoScroll = true;
                    AutoScrollMinSize = new Size(needW, needH);

                    // 启用滚动后 WinForms 会为滚动条预留空间，窗体可能因此变大。
                    // 这里再夹一次，确保最终窗口确实落在工作区内。
                    int ncH2 = base.Height - ClientSize.Height;
                    int ncW2 = base.Width - ClientSize.Width;
                    Size t2 = ClientSize;
                    if (wa.Height - ncH2 > 0 && t2.Height > wa.Height - ncH2) t2.Height = wa.Height - ncH2;
                    if (wa.Width - ncW2 > 0 && t2.Width > wa.Width - ncW2) t2.Width = wa.Width - ncW2;
                    if (t2 != ClientSize)
                        ClientSize = t2;
                }
                else
                {
                    AutoScroll = false;
                    AutoScrollMinSize = System.Drawing.Size.Empty;
                }

                // 位置：完整夹进工作区（副屏在主屏上方时 y 为负，统一处理）
                int x = Left, y = Top;
                if (x + base.Width > wa.Right) x = wa.Right - base.Width;
                if (y + base.Height > wa.Bottom) y = wa.Bottom - base.Height;
                if (x < wa.Left) x = wa.Left;
                if (y < wa.Top) y = wa.Top;
                if (x != Left || y != Top)
                    Location = new Point(x, y);

                DpiDiag("  [fit] ClientSize=" + ClientSize + " Form=" + Size +
                        " need=" + needW + "x" + needH +
                        " scroll=" + AutoScroll + " WA=" + wa);
            }
            finally
            {
                _restoringSize = false;
            }
        }

        /// <summary>DPI 相关诊断输出（仅在 Debug 构建下可见，不产生文件）。</summary>
        [System.Diagnostics.Conditional("DEBUG")]
        internal static void DpiDiag(string msg)
        {
            System.Diagnostics.Debug.WriteLine("[DPI] " + msg);
        }

        private int SafeDpi()
        {
            try { return DeviceDpi; } catch { return -1; }
        }

        private string SafeScreenInfo()
        {
            try
            {
                var s = Screen.FromControl(this);
                return s.DeviceName + " Bounds=" + s.Bounds + " WA=" + s.WorkingArea;
            }
            catch { return "?"; }
        }
        private void ClearAnchorsRecursive(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                c.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                if (c.HasChildren) ClearAnchorsRecursive(c);
            }
        }

        private void FontCreator_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (edited == true)
            {
                DialogResult status = MessageBox.Show("Save font before closing Font Editor?", "Exit", MessageBoxButtons.YesNoCancel);
                if (status == DialogResult.Cancel)
                // if (state == DialogResult.Cancel)
                {
                    e.Cancel = true; // Cancel = true
                }
                else if (status == DialogResult.Yes) // If (state == DialogResult.Yes)
                {
                    // If no font file is currently open, use Save As instead
                    if (string.IsNullOrEmpty(ofd.FileName))
                    {
                        MessageBox.Show("No font file is currently open. Please use 'Save As...' to save the font.", "Save Required",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        saveAsToolStripMenuItem_Click(sender, e);
                        e.Cancel = true; // Don't close until user saves
                        return;
                    }

                    FileStream fs = new FileStream(ofd.SafeFileName, FileMode.Create); // Save to the open file.
                    SaveFont(fs, font);
                    // After saving, clear the lists
                }
                else // Otherwise just close the program and clear the lists
                {
                }
            }

            if (basePreviewBitmap != null)
            {
                basePreviewBitmap.Dispose();
                basePreviewBitmap = null;
            }
        }

        private void RefreshProfileComboBox(FontProfileList profiles, string selectName)
        {
            comboBoxProfiles.Items.Clear();
            foreach (var profile in profiles.Profiles)
                comboBoxProfiles.Items.Add(profile);

            if (selectName != null)
            {
                for (int i = 0; i < comboBoxProfiles.Items.Count; i++)
                {
                    if (((FontProfile)comboBoxProfiles.Items[i]).Name == selectName)
                    {
                        comboBoxProfiles.SelectedIndex = i;
                        return;
                    }
                }
            }
        }

        private void comboBoxProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxProfiles.SelectedItem == null) return;
            FontProfile profile = (FontProfile)comboBoxProfiles.SelectedItem;
            textBoxYoffset.Text = profile.YOffset.ToString();
            textBoxFontSizeAdjust.Text = profile.FontSizeAdjust.ToString();
            if (!string.IsNullOrEmpty(profile.FontFamilyName))
            {
                selectedFontFamilyName = profile.FontFamilyName;
                selectedFontFilePath = profile.FontFilePath ?? "";
                selectedFontStyle = (System.Drawing.FontStyle)(profile.FontStyleIndex);
                string[] styleNames = { "", " Bold", " Italic", " Bold Italic" };
                string styleSuffix = profile.FontStyleIndex > 0 ? styleNames[profile.FontStyleIndex] : "";
                textBoxGenFont.Text = string.IsNullOrEmpty(selectedFontFilePath)
                    ? selectedFontFamilyName + styleSuffix
                    : Path.GetFileName(selectedFontFilePath) + " (" + selectedFontFamilyName + styleSuffix + ")";
            }
        }

        private void buttonSaveProfile_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBoxYoffset.Text, out int yOffset)) return;
            if (!int.TryParse(textBoxFontSizeAdjust.Text, out int fontSizeAdjust)) return;

            using (Form inputForm = new Form())
            {
                inputForm.Text = "Save Profile";
                inputForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                inputForm.StartPosition = FormStartPosition.CenterParent;
                inputForm.ClientSize = new Size(250, 80);
                inputForm.MaximizeBox = false;
                inputForm.MinimizeBox = false;

                Label lbl = new Label() { Text = "Profile name:", Left = 10, Top = 10, Width = 80 };
                TextBox txt = new TextBox() { Left = 90, Top = 8, Width = 145 };
                Button okBtn = new Button() { Text = "OK", DialogResult = DialogResult.OK, Left = 80, Top = 42, Width = 75 };
                Button cancelBtn = new Button() { Text = "Cancel", DialogResult = DialogResult.Cancel, Left = 160, Top = 42, Width = 75 };

                inputForm.Controls.AddRange(new Control[] { lbl, txt, okBtn, cancelBtn });
                inputForm.AcceptButton = okBtn;
                inputForm.CancelButton = cancelBtn;

                if (inputForm.ShowDialog() != DialogResult.OK) return;
                string name = txt.Text.Trim();
                if (string.IsNullOrEmpty(name)) return;

                FontProfileList profileList = FontProfileList.Load();
                var existing = profileList.Profiles.FirstOrDefault(p => p.Name == name);
                if (existing != null)
                {
                    if (MessageBox.Show($"Profile \"{name}\" already exists. Overwrite?", "Overwrite Profile",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        return;
                    existing.YOffset = yOffset;
                    existing.FontSizeAdjust = fontSizeAdjust;
                    existing.FontFamilyName = selectedFontFamilyName;
                    existing.FontFilePath = selectedFontFilePath;
                    existing.FontStyleIndex = (int)selectedFontStyle;
                }
                else
                {
                    profileList.Profiles.Add(new FontProfile(name, yOffset, fontSizeAdjust, selectedFontFamilyName, selectedFontFilePath, (int)selectedFontStyle));
                }

                profileList.Save();
                RefreshProfileComboBox(profileList, name);
            }
        }

        private void buttonDeleteProfile_Click(object sender, EventArgs e)
        {
            if (comboBoxProfiles.SelectedIndex < 0) return;
            string name = ((FontProfile)comboBoxProfiles.SelectedItem).Name;

            if (MessageBox.Show($"Delete profile \"{name}\"?", "Delete Profile",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            FontProfileList profileList = FontProfileList.Load();
            profileList.Profiles.RemoveAll(p => p.Name == name);
            profileList.Save();
            RefreshProfileComboBox(profileList, null);
        }

        private void buttonApplyYoffsetAdjust_Click(object sender, EventArgs e)
        {
            if (font == null || font.glyph.charsNew == null)
            {
                MessageBox.Show("No font loaded.", "Error");
                return;
            }

            string input = textBoxYoffsetAdjust.Text.Trim();
            if (input.StartsWith("+")) input = input.Substring(1);

            if (!int.TryParse(input, out int adjustment))
            {
                MessageBox.Show("Invalid adjustment value. Enter a number (e.g., -5 or +3).", "Error");
                return;
            }

            for (int i = 0; i < font.glyph.CharCount; i++)
            {
                font.glyph.charsNew[i].YOffset += adjustment;
            }

            fillTableofCoordinates(font, false);
            edited = true;
            textBoxLogOutput.AppendText($"[YOffset] Applied adjustment: {adjustment} to all {font.glyph.CharCount} characters.\r\n");
        }

        private void buttonApplyYAdjust_Click(object sender, EventArgs e)
        {
            if (font == null)
            {
                MessageBox.Show("No font loaded.", "Error");
                return;
            }

            string input = textBoxYAdjust.Text.Trim();
            if (input.StartsWith("+")) input = input.Substring(1);

            if (!int.TryParse(input, out int adjustment))
            {
                MessageBox.Show("Invalid Y Adj value. Enter a number (e.g., 20, -10, +5).", "Error");
                return;
            }

            if (font.NewFormat)
            {
                if (font.glyph.charsNew == null)
                {
                    MessageBox.Show("No character data loaded.", "Error");
                    return;
                }

                for (int i = 0; i < font.glyph.CharCount; i++)
                {
                    if (font.glyph.charsNew[i] == null) continue;
                    font.glyph.charsNew[i].YStart += adjustment;
                    font.glyph.charsNew[i].YEnd += adjustment;
                }
            }
            else
            {
                if (font.glyph.chars == null)
                {
                    MessageBox.Show("No character data loaded.", "Error");
                    return;
                }

                for (int i = 0; i < font.glyph.CharCount; i++)
                {
                    font.glyph.chars[i].YStart += adjustment;
                    font.glyph.chars[i].YEnd += adjustment;
                }
            }

            fillTableofCoordinates(font, false);
            edited = true;
            textBoxLogOutput.AppendText($"[Y Adj] Applied y= adjustment: {adjustment} to all {font.glyph.CharCount} characters.\r\n");
        }

        private void buttonPickFont_Click(object sender, EventArgs e)
        {
            using (FontPickerDialog pickForm = new FontPickerDialog())
            {
                if (pickForm.ShowDialog() != DialogResult.OK)
                    return;

                selectedFontFamilyName = pickForm.SelectedFontFamilyName;
                selectedFontFilePath = pickForm.SelectedFontFilePath;
                selectedFontStyle = pickForm.SelectedFontStyle;

                string[] styleNames = { "", " Bold", " Italic", " Bold Italic" };
                string info = selectedFontFamilyName;
                if (selectedFontStyle != FontStyle.Regular)
                    info += styleNames[(int)selectedFontStyle];
                if (!string.IsNullOrEmpty(selectedFontFilePath))
                    info = Path.GetFileName(selectedFontFilePath) + " (" + info + ")";
                textBoxGenFont.Text = info;
            }
        }











        private void contextMenuStripExport_Import_Opening(object sender, CancelEventArgs e)
        {
            if (dataGridViewWithTextures.Rows.Count > 0)
            {
                if (dataGridViewWithTextures.SelectedCells[0].RowIndex >= 0)
                {
                    exportToolStripMenuItem.Enabled = true;
                    importDDSToolStripMenuItem.Enabled = true;
                }
                else
                {
                    exportToolStripMenuItem.Enabled = false;
                    importDDSToolStripMenuItem.Enabled = false;
                    exportCoordinatesToolStripMenuItem1.Enabled = false;
                    toolStripImportFNT.Enabled = false;
                }
            }
        }

        private void dataGridViewWithTextures_RowContextMenuStripNeeded(object sender, DataGridViewRowContextMenuStripNeededEventArgs e)
        {
            dataGridViewWithTextures.Rows[e.RowIndex].Selected = true;
            MessageBox.Show(dataGridViewWithTextures.Rows[e.RowIndex].Selected.ToString());
        }

        private void dataGridViewWithTextures_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dataGridViewWithTextures.Rows[e.RowIndex].Selected = true;
            }
            if (e.Button == MouseButtons.Left && e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // get coordinates
                Point pntCell = dataGridViewWithTextures.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Location;
                pntCell.X += e.Location.X;
                pntCell.Y += e.Location.Y;

                // show context menu
                contextMenuStripExport_Import.Show(dataGridViewWithTextures, pntCell);
            }

            UpdateTexturePreview();
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int file_n = dataGridViewWithTextures.SelectedCells[0].RowIndex;
            SaveFileDialog saveFD = new SaveFileDialog();
            if ((font.tex != null && font.tex[file_n].isIOS) || (font.NewTex != null && font.NewTex[file_n].isPVR))
            {
                saveFD.Filter = "PVR files (*.pvr)|*.pvr";
                saveFD.FileName = font.FontName + "_" + file_n.ToString() + ".pvr";
            }
            else
            {
                saveFD.Filter = "dds files (*.dds)|*.dds";
                saveFD.FileName = font.FontName + "_" + file_n.ToString() + ".dds";
            }

            if (saveFD.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(saveFD.FileName, FileMode.Create);
                Methods.DeleteCurrentFile(saveFD.FileName);

                switch (font.NewFormat)
                {
                    case true:
                        fs.Write(font.NewTex[file_n].Tex.Content, 0, font.NewTex[file_n].Tex.Content.Length);
                        break;

                    default:
                        fs.Write(font.tex[file_n].Content, 0, font.tex[file_n].Content.Length);
                        break;
                }

                fs.Close();
            }
        }

        private void importDDSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int file_n = dataGridViewWithTextures.SelectedCells[0].RowIndex;
            OpenFileDialog openFD = new OpenFileDialog();

            openFD.Filter = "dds files (*.dds)|*.dds";

            if (openFD.ShowDialog() == DialogResult.OK)
            {
                if (font.NewFormat) ReplaceTexture(openFD.FileName, font.NewTex[file_n]);
                else ReplaceTexture(openFD.FileName, font.tex[file_n]);

                fillTableofTextures(font);
                edited = true; // Mark that the font has been modified
                UpdateTexturePreview();
            }
        }

        private void dataGridViewWithCoord_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int end_edit_column = e.ColumnIndex;
            int end_edit_row = e.RowIndex;
            bool success = false;
            if (old_data != "")
            {
                if ((end_edit_column >= 2 && end_edit_column <= dataGridViewWithCoord.ColumnCount) && Methods.IsNumeric(dataGridViewWithCoord[end_edit_column, end_edit_row].Value.ToString()))
                {
                    if (dataGridViewWithCoord[end_edit_column, end_edit_row].Value.ToString() != old_data)
                    {
                        if (end_edit_column == 2 || end_edit_column == 3) //X
                        {
                            dataGridViewWithCoord[7, end_edit_row].Value = (Convert.ToInt32(dataGridViewWithCoord[3, end_edit_row].Value) - Convert.ToInt32(dataGridViewWithCoord[2, end_edit_row].Value));
                            success = true;
                        }
                        else if (end_edit_column == 4 || end_edit_column == 5) //Y
                        {
                            dataGridViewWithCoord[8, end_edit_row].Value = (Convert.ToInt32(dataGridViewWithCoord[5, end_edit_row].Value) - Convert.ToInt32(dataGridViewWithCoord[4, end_edit_row].Value));
                            success = true;
                        }
                        else if (end_edit_column == 6) //dds
                        {
                            success = true;
                            if (Convert.ToInt32(dataGridViewWithCoord[end_edit_column, end_edit_row].Value) >= dataGridViewWithTextures.RowCount)
                            {
                                dataGridViewWithCoord[end_edit_column, end_edit_row].Value = old_data;
                                success = false;
                            }
                        }
                        else if (end_edit_column > 6 && end_edit_column < 8)
                        {
                            dataGridViewWithCoord[end_edit_column, end_edit_row].Value = old_data;
                        }
                    }
                }
                else
                {
                    dataGridViewWithCoord[end_edit_column, end_edit_row].Value = old_data;
                }
            }
            if (success)
            {
                dataGridViewWithCoord[end_edit_column, end_edit_row].Style.BackColor = Color.DarkCyan;
                if (!font.NewFormat)
                {
                    float.TryParse(dataGridViewWithCoord[2, end_edit_row].Value.ToString(), out font.glyph.chars[end_edit_row].XStart);
                    float.TryParse(dataGridViewWithCoord[3, end_edit_row].Value.ToString(), out font.glyph.chars[end_edit_row].XEnd);
                    float.TryParse(dataGridViewWithCoord[4, end_edit_row].Value.ToString(), out font.glyph.chars[end_edit_row].YStart);
                    float.TryParse(dataGridViewWithCoord[5, end_edit_row].Value.ToString(), out font.glyph.chars[end_edit_row].YEnd);
                    int.TryParse(dataGridViewWithCoord[6, end_edit_row].Value.ToString(), out font.glyph.chars[end_edit_row].TexNum);

                    if (font.hasScaleValue)
                    {
                        float.TryParse(dataGridViewWithCoord[7, end_edit_row].Value.ToString(), out font.glyph.chars[end_edit_row].CharWidth);
                        float.TryParse(dataGridViewWithCoord[8, end_edit_row].Value.ToString(), out font.glyph.chars[end_edit_row].CharHeight);
                    }
                }
                else
                {
                    float.TryParse(dataGridViewWithCoord[4, end_edit_row].Value.ToString(), out font.glyph.charsNew[end_edit_row].YStart);
                    float.TryParse(dataGridViewWithCoord[5, end_edit_row].Value.ToString(), out font.glyph.charsNew[end_edit_row].YEnd);
                    int.TryParse(dataGridViewWithCoord[6, end_edit_row].Value.ToString(), out font.glyph.charsNew[end_edit_row].TexNum);
                    float.TryParse(dataGridViewWithCoord[7, end_edit_row].Value.ToString(), out font.glyph.charsNew[end_edit_row].CharWidth);
                    float.TryParse(dataGridViewWithCoord[8, end_edit_row].Value.ToString(), out font.glyph.charsNew[end_edit_row].CharHeight);
                    float.TryParse(dataGridViewWithCoord[9, end_edit_row].Value.ToString(), out font.glyph.charsNew[end_edit_row].XOffset);
                    float.TryParse(dataGridViewWithCoord[10, end_edit_row].Value.ToString(), out font.glyph.charsNew[end_edit_row].YOffset);
                    float.TryParse(dataGridViewWithCoord[11, end_edit_row].Value.ToString(), out font.glyph.charsNew[end_edit_row].XAdvance);
                    int.TryParse(dataGridViewWithCoord[12, end_edit_row].Value.ToString(), out font.glyph.charsNew[end_edit_row].Channel);
                }
            }
            if (!edited && success)
            {
                edited = success;
            }

            UpdateTexturePreview();
        }

        public static string old_data;

        private void dataGridViewWithCoord_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            int now_edit_column = e.ColumnIndex;
            int now_edit_row = e.RowIndex;
            old_data = dataGridViewWithCoord[now_edit_column, now_edit_row].Value.ToString();
        }

        private void dataGridViewWithCoord_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dataGridViewWithCoord.Rows[e.RowIndex].Selected = true;
            }
            if (e.Button == MouseButtons.Right && e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // get coordinates
                Point pntCell = dataGridViewWithCoord.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true).Location;
                pntCell.X += e.Location.X;
                pntCell.Y += e.Location.Y;

                // show context menu
                contextMenuStripExp_imp_Coord.Show(dataGridViewWithCoord, pntCell);
            }

            UpdateTexturePreview();
        }

        private void dataGridViewWithTextures_SelectionChanged(object sender, EventArgs e)
        {
            UpdateTexturePreview();
        }

        private void dataGridViewWithCoord_SelectionChanged(object sender, EventArgs e)
        {
            // Auto-select the corresponding texture page when a character is selected
            if (dataGridViewWithCoord.SelectedCells.Count > 0)
            {
                int rowIndex = dataGridViewWithCoord.SelectedCells[0].RowIndex;
                if (rowIndex >= 0 && rowIndex < dataGridViewWithCoord.RowCount)
                {
                    float xStart, xEnd, yStart, yEnd;
                    int texNum;
                    if (TryGetGlyphRectFromRow(rowIndex, out xStart, out xEnd, out yStart, out yEnd, out texNum))
                    {
                        if (texNum >= 0 && texNum < dataGridViewWithTextures.RowCount)
                        {
                            dataGridViewWithTextures.ClearSelection();
                            dataGridViewWithTextures.Rows[texNum].Selected = true;
                        }
                    }
                }
            }

            UpdateTexturePreview();
        }

        private void exportCoordinatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exportCoordinatesToolStripMenuItem1_Click(sender, e);
        }

        private void buttonPreviewChar_Click(object sender, EventArgs e)
        {
            if (font == null)
            {
                MessageBox.Show("Please open a font file first.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string searchChar = textBoxSearchChar.Text.Trim();
            if (string.IsNullOrEmpty(searchChar))
            {
                MessageBox.Show("Please enter a character to search.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Get the encoding of the search character
            byte[] charBytes = Encoding.GetEncoding(AppData.settings.ASCII_N).GetBytes(searchChar);
            uint charCode = 0;

            if (font.NewFormat)
            {
                // New format: Use Unicode or ASCII
                if (AppData.settings.unicodeSettings == 0)
                {
                    charBytes = Encoding.Unicode.GetBytes(searchChar);
                    if (charBytes.Length >= 2)
                    {
                        charCode = BitConverter.ToUInt16(charBytes, 0);
                    }
                }
                else
                {
                    if (charBytes.Length > 0)
                    {
                        charCode = charBytes[0];
                    }
                }
            }
            else
            {
                // Old format: Use ASCII encoding
                if (charBytes.Length > 0)
                {
                    charCode = charBytes[0];
                }
            }

            textBoxLogOutput.AppendText($"\r\n=== Preview: '{searchChar}' ===\r\n");
            textBoxLogOutput.AppendText($"  CharCode: {charCode} (0x{charCode:X4})\r\n");
            textBoxLogOutput.AppendText($"  Grid rows: {dataGridViewWithCoord.RowCount}\r\n");

            // Search for this character in font data
            int foundRow = -1;
            int foundTexNum = -1;
            int matchCount = 0;

            for (int i = 0; i < dataGridViewWithCoord.RowCount; i++)
            {
                uint rowCharId;
                if (uint.TryParse(Convert.ToString(dataGridViewWithCoord[0, i].Value), out rowCharId))
                {
                    if (rowCharId == charCode)
                    {
                        matchCount++;
                        if (foundRow < 0)
                        {
                            foundRow = i;
                            int.TryParse(Convert.ToString(dataGridViewWithCoord[6, i].Value), out foundTexNum);
                        }
                    }
                }
            }

            if (matchCount > 1)
            {
                textBoxLogOutput.AppendText($"  WARNING: Character found {matchCount} times (duplicates)\r\n");
            }

            if (foundRow >= 0 && foundTexNum >= 0)
            {
                // Select the found row
                dataGridViewWithCoord.ClearSelection();
                dataGridViewWithCoord.Rows[foundRow].Selected = true;
                dataGridViewWithCoord.FirstDisplayedScrollingRowIndex = foundRow;

                // Select the corresponding texture
                if (foundTexNum < dataGridViewWithTextures.RowCount)
                {
                    dataGridViewWithTextures.ClearSelection();
                    dataGridViewWithTextures.Rows[foundTexNum].Selected = true;
                }

                // Update preview (main panel, not popup)
                UpdateTexturePreview();

                textBoxLogOutput.AppendText($"  Found at row {foundRow + 1}, texture page {foundTexNum}\r\n");

                // Log character details
                if (font.NewFormat && foundRow < font.glyph.charsNew.Length)
                {
                    var ch = font.glyph.charsNew[foundRow];
                    textBoxLogOutput.AppendText($"  XStart={ch.XStart} YStart={ch.YStart} Width={ch.CharWidth} Height={ch.CharHeight}\r\n");
                    textBoxLogOutput.AppendText($"  XOffset={ch.XOffset} YOffset={ch.YOffset} XAdvance={ch.XAdvance}\r\n");
                }

            }
            else
            {
                textBoxLogOutput.AppendText($"  NOT FOUND in {dataGridViewWithCoord.RowCount} rows\r\n");
                MessageBox.Show($"Character '{searchChar}' (code: {charCode}) not found in this font.",
                    "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void textBoxSearchChar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonPreviewChar_Click(sender, e);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void exportCoordinatesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "FNT file (*.fnt) | *.fnt";
            sfd.FileName = font.FontName + ".fnt";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                float exportSize = font.FntInfoSize > 0 ? font.FntInfoSize : font.FntLineHeight;
                string info = "info face=\"" + font.FontName + "\" size=" + exportSize + " bold=0 italic=0 charset=\"\" unicode=";
                switch (font.NewFormat)
                {
                    case true:
                        info += "1\r\n";
                        break;

                    default:
                        info += "0\r\n";
                        break;
                }

                info += "common lineHeight=" + font.FntLineHeight;

                if ((font.One == 0x31 && (Encoding.ASCII.GetString(check_header) == "5VSM"))
                        || (Encoding.ASCII.GetString(check_header) == "6VSM"))
                {
                    info += " base=" + font.FntBaseLine;
                }
                else info += " base=" + font.FntLineHeight;

                info += " pages=" + font.TexCount + "\r\n";

                // Log exported FNT header values
                float loggedBase = (font.One == 0x31 && (Encoding.ASCII.GetString(check_header) == "5VSM"))
                        || (Encoding.ASCII.GetString(check_header) == "6VSM") ? font.FntBaseLine : font.FntLineHeight;
                textBoxLogOutput.AppendText($"[FNT Export] info size={exportSize}, common lineHeight={font.FntLineHeight}, common base={loggedBase}, FntInfoSize={font.FntInfoSize}, FntBaseLine={font.FntBaseLine}, One=0x{font.One:x2}, header={Encoding.ASCII.GetString(check_header ?? new byte[0])}\r\n");

                if (File.Exists(sfd.FileName)) File.Delete(sfd.FileName);
                FileStream fs = new FileStream(sfd.FileName, FileMode.CreateNew);
                StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                sw.Write(info);
                info = "";

                for (int i = 0; i < font.TexCount; i++)
                {
                    info = "page id=" + i + " file=\"" + font.FontName + "_" + i + ".dds\"\r\n";
                    sw.Write(info);
                }

                info = "chars count=" + font.glyph.CharCount + "\r\n";
                sw.Write(info);

                if (!font.NewFormat)
                {
                    for (int i = 0; i < font.glyph.CharCount; i++)
                    {
                        info = "char id=" + i + " x=" + font.glyph.chars[i].XStart + " y=" + font.glyph.chars[i].YStart;
                        info += " width=";

                        if (font.hasScaleValue)
                        {
                            info += font.glyph.chars[i].CharWidth;
                        }
                        else
                        {
                            info += font.glyph.chars[i].XEnd - font.glyph.chars[i].XStart;
                        }

                        info += " height=";

                        if (font.hasScaleValue)
                        {
                            info += font.glyph.chars[i].CharHeight;
                        }
                        else
                        {
                            info += font.glyph.chars[i].YEnd - font.glyph.chars[i].YStart;
                        }

                        info += " xoffset=0 yoffset=0 xadvance=";

                        if (font.hasScaleValue)
                        {
                            info += font.glyph.chars[i].CharWidth;
                        }
                        else
                        {
                            info += font.glyph.chars[i].XEnd - font.glyph.chars[i].XStart;
                        }

                        info += " page=" + font.glyph.chars[i].TexNum + " chnl=15\r\n";

                        sw.Write(info);
                    }
                }
                else
                {
                    for (int i = 0; i < font.glyph.CharCount; i++)
                    {
                        info = "char id=" + font.glyph.charsNew[i].charId + " x=" + font.glyph.charsNew[i].XStart + " y=" + font.glyph.charsNew[i].YStart;
                        float xOffset = rbNoKerning.Checked ? 0 : font.glyph.charsNew[i].XOffset;
                        float yOffset = rbNoKerning.Checked ? 0 : font.glyph.charsNew[i].YOffset;
                        float xAdvance = rbNoKerning.Checked ? font.glyph.charsNew[i].CharWidth : font.glyph.charsNew[i].XAdvance;

                        info += " width=" + font.glyph.charsNew[i].CharWidth + " height=" + font.glyph.charsNew[i].CharHeight;
                        info += " xoffset=" + xOffset + " yoffset=" + yOffset + " xadvance=";
                        info += xAdvance + " page=" + font.glyph.charsNew[i].TexNum + " chnl=" + font.glyph.charsNew[i].Channel + "\r\n";

                        sw.Write(info);
                    }
                }

                sw.Close();
                fs.Close();
            }
        }



        private void rbNoSwizzle_CheckedChanged(object sender, EventArgs e)
        {
            AppData.settings.swizzleXbox360 = false;
            AppData.settings.swizzlePS4 = false;
            AppData.settings.swizzleNintendoSwitch = false;
            AppData.settings.swizzlePSVita = false;
            AppData.settings.swizzleNintendoWii = false;
            Settings.SaveConfig(AppData.settings);

            ApplyPlatformToLoadedFont(2u);
        }

        private void rbPS4Swizzle_CheckedChanged(object sender, EventArgs e)
        {
            AppData.settings.swizzleXbox360 = false;
            AppData.settings.swizzlePS4 = true;
            AppData.settings.swizzleNintendoSwitch = false;
            AppData.settings.swizzlePSVita = false;
            AppData.settings.swizzleNintendoWii = false;
            Settings.SaveConfig(AppData.settings);

            ApplyPlatformToLoadedFont(11u);
        }

        private void rbSwitchSwizzle_CheckedChanged(object sender, EventArgs e)
        {
            AppData.settings.swizzleXbox360 = false;
            AppData.settings.swizzlePS4 = false;
            AppData.settings.swizzleNintendoSwitch = true;
            AppData.settings.swizzlePSVita = false;
            AppData.settings.swizzleNintendoWii = false;
            Settings.SaveConfig(AppData.settings);

            ApplyPlatformToLoadedFont(15u);
        }

        private void rbXbox360Swizzle_CheckedChanged(object sender, EventArgs e)
        {
            if (rbXbox360Swizzle.Checked)
            {
                AppData.settings.swizzleXbox360 = true;
                AppData.settings.swizzlePS4 = false;
                AppData.settings.swizzleNintendoSwitch = false;
                AppData.settings.swizzlePSVita = false;
                AppData.settings.swizzleNintendoWii = false;
                Settings.SaveConfig(AppData.settings);

                ApplyPlatformToLoadedFont(4u);
            }
        }

        private void rbPSVitaSwizzle_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPSVitaSwizzle.Checked)
            {
                AppData.settings.swizzlePSVita = true;
                AppData.settings.swizzlePS4 = false;
                AppData.settings.swizzleNintendoSwitch = false;
                AppData.settings.swizzleXbox360 = false;
                AppData.settings.swizzleNintendoWii = false;
                Settings.SaveConfig(AppData.settings);

                ApplyPlatformToLoadedFont(9u);
            }
        }

        private void rbWiiSwizzle_CheckedChanged(object sender, EventArgs e)
        {
            if (rbWiiSwizzle.Checked)
            {
                AppData.settings.swizzlePSVita = false;
                AppData.settings.swizzlePS4 = false;
                AppData.settings.swizzleNintendoSwitch = false;
                AppData.settings.swizzleXbox360 = false;
                AppData.settings.swizzleNintendoWii = true;
                Settings.SaveConfig(AppData.settings);

                // Wii has no font-platform equivalent in the texture header format,
                // so the loaded font is intentionally left untouched.
                ApplyPlatformToLoadedFont(0u);
            }
        }

        /// <summary>
        /// Applies the platform selected in the swizzle radio group to every texture that is
        /// already loaded, so that switching the radio before Save actually changes the output.
        ///
        /// Background: platform is only assigned when a DDS is imported
        /// (ReplaceTexture -> ResolveTargetPlatformForImportedDds). The radio buttons used to
        /// update AppData.settings only, so textures imported earlier kept their original
        /// platform no matter what was selected afterwards. The visible symptom was
        /// "I switched to PC before exporting and still got platform=15".
        ///
        /// The pixel path depends on this field too: ReplaceNewTextures only swizzles for the
        /// platform recorded here, so updating it is enough to change the written pixel encoding.
        /// Among the fonts examined, no other texture-header field differs between PC(2) and
        /// Switch(15), so nothing else needs rewriting.
        ///
        /// Pass 0 (or any unsupported id) to leave the loaded font alone.
        /// </summary>
        private void ApplyPlatformToLoadedFont(uint platform)
        {
            if (platform == 0u) return;
            if (font == null || !font.NewFormat || font.NewTex == null) return;
            if (font.NewTex.Length == 0) return;

            bool changed = false;
            foreach (var tex in font.NewTex)
            {
                if (tex == null) continue;
                if (tex.platform.platform == platform) continue;

                tex.platform.platform = platform;
                changed = true;

                // Re-run the per-platform header defaults so Switch-specific fields
                // (unknownFlags.block / HasOneValueTex / OneValue / 8-byte subBlocks)
                // get filled in when moving to Switch, and nothing is left half-set.
                EnsureNewTextureHeaderDefaults(tex);
            }

            if (!changed) return;

            edited = true;

            if (textBoxLogOutput != null && !textBoxLogOutput.IsDisposed)
            {
                textBoxLogOutput.AppendText(
                    $"[Platform] Applied platform={platform} to {font.NewTex.Length} texture(s). " +
                    "Pixel data will be swizzled for this platform on save.\r\n");
            }

            // Refresh the texture table so TexSize (which can change with the platform's
            // mip padding) is shown accurately before saving.
            fillTableofTextures(font);
            UpdateTexturePreview();
        }

        private void buttonSaveLogAs_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxLogOutput.Text))
            {
                MessageBox.Show("No log content to save.", "No Content",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Text files (*.txt)|*.txt|Log files (*.log)|*.log|All files (*.*)|*.*";
                saveFileDialog.Title = "Save Detection Log As";
                string prefix = isNewFontMode ? "BUILD" : "REPLACE";
                saveFileDialog.FileName = $"{prefix}_{DateTime.Now:yyyyMMdd_HHmmss}.txt";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        File.WriteAllText(saveFileDialog.FileName, textBoxLogOutput.Text, Encoding.UTF8);
                        MessageBox.Show($"Log saved successfully to:\n{saveFileDialog.FileName}", "Save Successful",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to save log:\n{ex.Message}", "Save Failed",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        // ======== FNT Adjust: Size ========
        private void buttonSizeApply_Click(object sender, EventArgs e)
        {
            if (font == null)
            {
                MessageBox.Show("No font loaded.", "Error");
                return;
            }
            if (float.TryParse(textBoxSizeAdj.Text, out float val))
            {
                font.FntInfoSize = val;
                fillTableofCoordinates(font, false);
                edited = true;
                textBoxLogOutput.AppendText($"[Size] Set FntInfoSize to {val}\r\n");
            }
        }

        // ======== FNT Adjust: LineHeight ========
        private void buttonLHApply_Click(object sender, EventArgs e)
        {
            if (font == null)
            {
                MessageBox.Show("No font loaded.", "Error");
                return;
            }
            if (float.TryParse(textBoxLineHeightAdj.Text, out float val))
            {
                font.FntLineHeight = val;
                fillTableofCoordinates(font, false);
                edited = true;
                textBoxLogOutput.AppendText($"[LineHgt] Set FntLineHeight to {val}\r\n");
            }
        }

        // ======== FNT Adjust: Height (batch) ========
        private void buttonHeightApply_Click(object sender, EventArgs e)
        {
            if (font == null)
            {
                MessageBox.Show("No font loaded.", "Error");
                return;
            }
            if (!float.TryParse(textBoxHeightAdj.Text, out float val)) return;

            for (int i = 0; i < font.glyph.CharCount; i++)
            {
                if (font.NewFormat && font.glyph.charsNew != null && font.glyph.charsNew[i] != null)
                    font.glyph.charsNew[i].CharHeight = val;
                else if (font.glyph.chars != null && font.glyph.chars[i] != null)
                    font.glyph.chars[i].CharHeight = val;
            }
            fillTableofCoordinates(font, false);
            edited = true;
            textBoxLogOutput.AppendText($"[Height] Set all chars CharHeight to {val}\r\n");
        }

        // ======== FNT Adjust: Channel (batch, NewFormat only) ========
        private void buttonChannelApply_Click(object sender, EventArgs e)
        {
            if (font == null)
            {
                MessageBox.Show("No font loaded.", "Error");
                return;
            }
            if (!int.TryParse(textBoxChannelAdj.Text, out int val)) return;
            if (!font.NewFormat || font.glyph.charsNew == null)
            {
                MessageBox.Show("Channel adjustment is only supported for New Format fonts.", "Info");
                return;
            }
            for (int i = 0; i < font.glyph.CharCount; i++)
            {
                if (font.glyph.charsNew[i] != null)
                    font.glyph.charsNew[i].Channel = val;
            }
            fillTableofCoordinates(font, false);
            edited = true;
            textBoxLogOutput.AppendText($"[Chnl] Set all chars Channel to {val}\r\n");
        }

        // ======== FNT Adjust: Base ========
        private void buttonBaseApply_Click(object sender, EventArgs e)
        {
            if (font == null)
            {
                MessageBox.Show("No font loaded.", "Error");
                return;
            }
            if (!float.TryParse(textBoxBaseAdj.Text, out float val)) return;
            font.FntBaseLine = val;
            edited = true;
            textBoxLogOutput.AppendText($"[Base] Set FntBaseLine (FNT base) to {val}\r\n");
        }

        // ======== FNT Adjust: YOffset ▲▼ (delta) ========
        private void buttonYoffsetUp_Click(object sender, EventArgs e)
        {
            if (font == null || font.glyph.charsNew == null)
            {
                MessageBox.Show("No font loaded.", "Error");
                return;
            }
            string input = textBoxYoffsetAdjust.Text.Trim();
            if (input.StartsWith("+")) input = input.Substring(1);
            if (!int.TryParse(input, out int adjustment))
            {
                MessageBox.Show("Invalid adjustment value. Enter a number (e.g., 1 or 5).", "Error");
                return;
            }
            for (int i = 0; i < font.glyph.CharCount; i++)
                font.glyph.charsNew[i].YOffset += adjustment;
            fillTableofCoordinates(font, false);
            edited = true;
            textBoxLogOutput.AppendText($"[YOffset] ▲ Applied +{adjustment} to all {font.glyph.CharCount} characters.\r\n");
        }

        private void buttonYoffsetDown_Click(object sender, EventArgs e)
        {
            if (font == null || font.glyph.charsNew == null)
            {
                MessageBox.Show("No font loaded.", "Error");
                return;
            }
            string input = textBoxYoffsetAdjust.Text.Trim();
            if (input.StartsWith("+")) input = input.Substring(1);
            if (!int.TryParse(input, out int adjustment))
            {
                MessageBox.Show("Invalid adjustment value. Enter a number (e.g., 1 or 5).", "Error");
                return;
            }
            for (int i = 0; i < font.glyph.CharCount; i++)
                font.glyph.charsNew[i].YOffset -= adjustment;
            fillTableofCoordinates(font, false);
            edited = true;
            textBoxLogOutput.AppendText($"[YOffset] ▼ Applied -{adjustment} to all {font.glyph.CharCount} characters.\r\n");
        }

        // ======== FNT Adjust: Y Adj ▲▼ (delta) ========
        private void buttonYadjUp_Click(object sender, EventArgs e)
        {
            if (font == null)
            {
                MessageBox.Show("No font loaded.", "Error");
                return;
            }
            string input = textBoxYAdjust.Text.Trim();
            if (input.StartsWith("+")) input = input.Substring(1);
            if (!int.TryParse(input, out int adjustment))
            {
                MessageBox.Show("Invalid Y Adj value. Enter a number (e.g., 1 or 5).", "Error");
                return;
            }
            if (font.NewFormat)
            {
                for (int i = 0; i < font.glyph.CharCount; i++)
                {
                    if (font.glyph.charsNew[i] == null) continue;
                    font.glyph.charsNew[i].YStart += adjustment;
                    font.glyph.charsNew[i].YEnd += adjustment;
                }
            }
            else
            {
                for (int i = 0; i < font.glyph.CharCount; i++)
                {
                    font.glyph.chars[i].YStart += adjustment;
                    font.glyph.chars[i].YEnd += adjustment;
                }
            }
            fillTableofCoordinates(font, false);
            edited = true;
            textBoxLogOutput.AppendText($"[Y Adj] ▲ Applied +{adjustment} to all {font.glyph.CharCount} characters.\r\n");
        }

        private void buttonYadjDown_Click(object sender, EventArgs e)
        {
            if (font == null)
            {
                MessageBox.Show("No font loaded.", "Error");
                return;
            }
            string input = textBoxYAdjust.Text.Trim();
            if (input.StartsWith("+")) input = input.Substring(1);
            if (!int.TryParse(input, out int adjustment))
            {
                MessageBox.Show("Invalid Y Adj value. Enter a number (e.g., 1 or 5).", "Error");
                return;
            }
            if (font.NewFormat)
            {
                for (int i = 0; i < font.glyph.CharCount; i++)
                {
                    if (font.glyph.charsNew[i] == null) continue;
                    font.glyph.charsNew[i].YStart -= adjustment;
                    font.glyph.charsNew[i].YEnd -= adjustment;
                }
            }
            else
            {
                for (int i = 0; i < font.glyph.CharCount; i++)
                {
                    font.glyph.chars[i].YStart -= adjustment;
                    font.glyph.chars[i].YEnd -= adjustment;
                }
            }
            fillTableofCoordinates(font, false);
            edited = true;
            textBoxLogOutput.AppendText($"[Y Adj] ▼ Applied -{adjustment} to all {font.glyph.CharCount} characters.\r\n");
        }

        /// <summary>
        /// Auto-populate FNT Adjust fields from current font data.
        /// Call after font open or FNT import.
        /// </summary>
        internal void PopulateFntAdjustFields()
        {
            if (font == null) return;

            // Size — FNT info size (point size) if available, otherwise FntLineHeight
            textBoxSizeAdj.Text = (font.FntInfoSize > 0 ? font.FntInfoSize : font.FntLineHeight).ToString("0.###");

            // LineHeight — same as Size (FNT common lineHeight)
            textBoxLineHeightAdj.Text = font.FntLineHeight.ToString("0.###");

            // Height — from first character
            float firstCharHeight = 0;
            if (font.NewFormat && font.glyph.charsNew != null && font.glyph.charsNew.Length > 0 && font.glyph.charsNew[0] != null)
            {
                firstCharHeight = font.glyph.charsNew[0].CharHeight;
            }
            else if (!font.NewFormat && font.glyph.chars != null && font.glyph.chars.Length > 0 && font.glyph.chars[0] != null)
            {
                firstCharHeight = font.glyph.chars[0].CharHeight;
            }
            textBoxHeightAdj.Text = firstCharHeight.ToString("0.###");

            // Channel — from first character (NewFormat only)
            int firstChannel = 0;
            if (font.NewFormat && font.glyph.charsNew != null && font.glyph.charsNew.Length > 0 && font.glyph.charsNew[0] != null)
            {
                firstChannel = font.glyph.charsNew[0].Channel;
            }
            textBoxChannelAdj.Text = firstChannel.ToString();

            // Base — FNT baseline (FntBaseLine in 5VSM/6VSM)
            textBoxBaseAdj.Text = font.FntBaseLine.ToString("0.###");
        }
        // ======== Texture Preview Zoom & Pan ========
        private Point _panStart;
        private bool _isPanning;

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            int w = pictureBoxTexturePreview.Width + 64;
            int h = pictureBoxTexturePreview.Height + 64;
            if (w > 1024) w = 1024;
            if (h > 1024) h = 1024;
            pictureBoxTexturePreview.Width = w;
            pictureBoxTexturePreview.Height = h;
            ClampPictureBoxPosition();
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            int w = pictureBoxTexturePreview.Width - 64;
            int h = pictureBoxTexturePreview.Height - 64;
            if (w < panelTexturePreview.Width) w = panelTexturePreview.Width;
            if (h < panelTexturePreview.Height) h = panelTexturePreview.Height;
            if (w < 128) w = 128;
            if (h < 128) h = 128;
            pictureBoxTexturePreview.Width = w;
            pictureBoxTexturePreview.Height = h;
            ClampPictureBoxPosition();
        }

        private void ClampPictureBoxPosition()
        {
            int x = pictureBoxTexturePreview.Left;
            int y = pictureBoxTexturePreview.Top;
            int maxX = pictureBoxTexturePreview.Width - panelTexturePreview.Width;
            int maxY = pictureBoxTexturePreview.Height - panelTexturePreview.Height;
            if (x > 0) x = 0;
            if (x < -maxX) x = -maxX;
            if (y > 0) y = 0;
            if (y < -maxY) y = -maxY;
            if (x != pictureBoxTexturePreview.Left || y != pictureBoxTexturePreview.Top)
            {
                pictureBoxTexturePreview.Left = x;
                pictureBoxTexturePreview.Top = y;
                // Reset delta base to prevent oscillation at boundary
                _panStart = pictureBoxTexturePreview.PointToClient(Control.MousePosition);
            }
        }

        private void pictureBoxTexturePreview_MouseDown(object sender, MouseEventArgs e)
        {
            if (pictureBoxTexturePreview.Width > panelTexturePreview.Width ||
                pictureBoxTexturePreview.Height > panelTexturePreview.Height)
            {
                _isPanning = true;
                _panStart = e.Location;
                pictureBoxTexturePreview.Cursor = Cursors.SizeAll;
            }
        }

        private void pictureBoxTexturePreview_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isPanning) return;
            int dx = e.X - _panStart.X;
            int dy = e.Y - _panStart.Y;
            if (dx == 0 && dy == 0) return;
            pictureBoxTexturePreview.Left += dx;
            pictureBoxTexturePreview.Top += dy;
            ClampPictureBoxPosition();
        }

        private void pictureBoxTexturePreview_MouseUp(object sender, MouseEventArgs e)
        {
            _isPanning = false;
            pictureBoxTexturePreview.Cursor = Cursors.Default;
        }
    }
}
