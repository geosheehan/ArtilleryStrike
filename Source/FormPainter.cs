using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace ArtilleryStrike
{
    public static class FormPainter
    {
        public enum Style
        { 
            Neutral     = 'N',
            Colonial    = 'C',
            Warden      = 'W'
        }

        public static ColorPalette.BasePalette PaintStyle(Form form, char style )
        {
            ColorPalette.BasePalette palette = new ColorPalette.BasePalette();
            List<Control> controls = new List<Control> { form };

            // Commonalities in style
            GetControls(form, controls);

            switch ((Style)style)
            {
                case Style.Neutral:
                    palette = new ColorPalette.Neutral();
                    break;

                case Style.Colonial:
                    palette = new ColorPalette.Colonial();
                    break;

                case Style.Warden:
                    palette = new ColorPalette.Warden();
                    break;
            }
            Paint(controls, palette);
            form.Invalidate();

            return palette;
        }

        private static bool FontInstalled(string font_name)
        {
            InstalledFontCollection fonts = new InstalledFontCollection();
            foreach (FontFamily family in fonts.Families)
            {
                if (family.Name == font_name)
                {
                    return true;
                }
            }
            return false;
        }

        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
            IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);


        public static void GetControls(Control root, List<Control> controls)
        {
            foreach (Control child in root.Controls)
            {
                controls.Add(child);
                GetControls(child, controls);
            }
        }

        private static void Paint(List<Control> controls, ColorPalette.BasePalette palette)
        {
            foreach (Control control in controls)
            {
                if (control is Form)
                {
                    control.BackColor = palette.FormBackGround;
                }
                else if (control is Panel)
                {
                    if (control is System.ComponentModel.IExtenderProvider)
                    {
                        control.BackColor = control.Parent.BackColor;
                    }
                    else
                    {
                        control.BackColor = palette.PanelBackGround;
                    }
                }
                else if (control is Button)
                {
                    Button button = control as Button;
                    button.BackColor = palette.ButtonBackGround;
                    button.FlatAppearance.BorderColor = palette.ButtonBorder;
                    if (button is IToggleable)
                    {
                        IToggleable toggleable = button as IToggleable;
                        toggleable.SelectedColor = palette.Accent;
                        toggleable.DeselectedColor = palette.ButtonBackGround;
                    }
                }
                else if (control is NumericUpDown)
                {
                    control.BackColor = palette.FieldBackGround;
                }
                else
                {
                    control.BackColor = control.Parent.BackColor;
                }
                control.ForeColor = palette.Text;
            }
        }

    }

    namespace ColorPalette
    {
        public class BasePalette
        {
            // Static properties
            public Color Text { get; protected set; }
            public Color Accent { get; protected set; }

            // Dynamic properties
            public Color FormBackGround { get; protected set; }
            public Color PanelBackGround { get; protected set; }
            public Color NumberBoxBackGround { get; protected set; }
            public Color ButtonBackGround { get; protected set; }
            public Color ButtonBorder { get; protected set; }
            public Color FieldBackGround { get; protected set; }
            
            public BasePalette()
            {
                Text = Color.White;
                Accent = Color.FromArgb(231, 124, 72);
            }
        }

        public class Neutral : BasePalette
        {
            public Neutral() : base()
            {
                FormBackGround = Color.FromArgb(100, 100, 100);
                PanelBackGround = Color.FromArgb(51, 51, 51);
                ButtonBackGround = Color.FromArgb(68, 68, 68);
                ButtonBorder = ButtonBackGround;
                FieldBackGround = FormBackGround;
            }
        }

        public class Colonial : BasePalette
        {
            public Colonial() : base()
            {
                FormBackGround = Color.FromArgb(81, 108, 75);
                //PanelBackGround = Color.Black;
                ButtonBackGround = Color.FromArgb(68, 68, 68);
                FieldBackGround = FormBackGround;
            }
        }

        public class Warden : BasePalette
        {
            public Warden() : base()
            {
                FormBackGround = Color.FromArgb(35, 86, 131);
                //PanelBackGround = Color.Black;
                ButtonBackGround = Color.FromArgb(68, 68, 68);
                FieldBackGround = FormBackGround;
            }
        }

    }
}
