namespace TTG_Tools.ClassesStructs
{
    public class FontClass
    {
            // Replayable tail that official 6VSM/5VSM fonts carry AFTER the LastZero byte.
            // Together with LastZero = 0x30 it forms the 23-byte block observed verbatim in every
            // TWD S1/S2 and The Wolf Among Us sample:
            //   30 30 30 | 08 00 00 00 00 | 00 00 00 00 00 00 08 00 | 00 00 00 00 00 00 00
            //   LastZero     ^ 22 bytes here ^
            // Semantics are unknown (likely serialized bools + two 8-byte sub-blocks), so this is
            // only ever copied, never rebuilt from runtime values.
            public static readonly byte[] DefaultTrailingBlockAfterTextureHeaders =
            {
                0x30, 0x30, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00
            };

            public class ClassFont
            {
                //For games since The Wolf Among Us
                public int headerSize; //Calculation of coordinates and texture headers
                public uint texSize; //Size of textures

                public string[] elements;
                public byte[][] binElements;

                //Before Poker Night 2 game
                public class TRect
                {
                    public int TexNum; //CurrentTexture
                    public float XStart;
                    public float XEnd;
                    public float YStart;
                    public float YEnd;
                    public float CharWidth;
                    public float CharHeight;

                    public TRect() { }
                }

                //Since Poker Night 2 game
                public class TRectNew
                {
                    public uint charId;
                    public int TexNum;
                    public int Channel;
                    public float XStart;
                    public float XEnd;
                    public float YStart;
                    public float YEnd;
                    public float CharWidth;
                    public float CharHeight;
                    public float XOffset;
                    public float YOffset;
                    public float XAdvance;

                    public TRectNew() { }
                }

                public struct GlyphInfo
                {
                    public int BlockCoordSize; //Size of block class TRect
                    public int CharCount; //Count characters (before Poker Night 2 default was 256)
                    public TRect[] chars; //Table of characters
                    public TRectNew[] charsNew; //For Poker Night 2 and newer games
                    public int Pages; //Number of texture pages
                }

                public bool blockSize;
                public bool hasScaleValue; //Use since Hector games
                public bool NewFormat;
                public bool hasOneFloatValue; //Stupid fix for newest Telltale's games
                public bool hasLineHeight;

                public string FontName;
                public float halfValue; //Shows in some fonts
                public float oneValue; //Scale font. Use since Hector and newer games
                public byte One;
                // FNT common base — the baseline the glyphs sit on. Stored at metrics offset 4
                // in the .font file. Physically this can never exceed FntLineHeight; that
                // invariant is what DetectSwappedMetrics uses to recognise files written by
                // older builds which stored the two values the other way round.
                public float FntBaseLine;
                public float FntInfoSize; // FNT info size attribute (point size), preserved across import/export
                public byte[] feedFace; //some data from WD: Michonne
                // FNT common lineHeight — vertical distance between two baselines. Stored at
                // metrics offset 0 in the .font file.
                public float FntLineHeight; // FNT common lineHeight — line spacing (prevents lines being too close/overlapping)
                public float lineHeight; //For Back to the Future for consoles PS4 and Xbox360
                public int BlockTexSize; //Size of block Textures
                public int TexCount; //Count textures
                public byte LastZero; //Something zero value byte after textures header

                // Opaque trailer that sits between the texture headers and the pixel data.
                // Captured verbatim on Open so Save can reproduce it byte-for-byte.
                // Official samples have either 1 byte (Batman, tool-generated files) or
                // 23 bytes (TWD S1/S2, The Wolf Among Us). Semantics are still unknown, so the
                // block is never rebuilt — only replayed. null means "nothing captured yet".
                public byte[] trailingBlockAfterTextureHeaders;

                // True when Open found FntBaseLine > FntLineHeight, i.e. the two metrics floats
                // were stored in the order older builds used (base first, lineHeight second).
                // Such files are still read with correct field names (the values end up in the
                // right members), but this flag lets Save write them back in the same legacy
                // order so an untouched Open -> Save stays byte-identical.
                public bool metricsStoredSwapped;

                public GlyphInfo glyph;
                public TextureClass.OldT3Texture[] tex;
                public TextureClass.NewT3Texture[] NewTex;
                public string[] texPaths; //Paths to texture files

                public ClassFont() { }

                /// <summary>
                /// Detects whether the two metrics floats are stored in the legacy (swapped)
                /// order, using the fact that a baseline can never sit below the next baseline:
                /// FntLineHeight must be &gt;= FntBaseLine. When the value read at the lineHeight
                /// offset is smaller than the value at the base offset, the file was almost
                /// certainly written by an older build that stored base first.
                ///
                /// A perfectly square font (lineHeight == base) is ambiguous and is treated as
                /// normal order. A zero-ish lineHeight is ignored because 5VSM fonts that omit
                /// the base float read as 0 there, which would look "swapped" by the inequality.
                /// </summary>
                public static bool AreMetricsSwapped(float lineHeightValue, float baseValue)
                {
                    if (lineHeightValue <= 0f) return false;
                    if (baseValue <= 0f) return false;
                    return baseValue > lineHeightValue;
                }
            }
    }
}
