using System;
using System.Drawing;
using System.Windows.Forms;

namespace ArtilleryStrike
{
    public class ConfirmationButton : Button, IToggleable
    {
        public Color DeselectedColor { get; set; }
        public Color SelectedColor { get; set; }

        public ConfirmationButton() : base()
        {
            SelectedColor = Color.Yellow;
            DeselectedColor = base.BackColor;
        }

        protected override void OnClick(EventArgs e)
        {
            if (BackColor == SelectedColor)
            {
                BackColor = DeselectedColor;
                base.OnClick(e);
            }
            else
            {
                BackColor = SelectedColor;
            }
        }

        protected override void OnLeave(EventArgs e)
        {
            BackColor = DeselectedColor;
            base.OnLeave(e);
        }

    }
}
