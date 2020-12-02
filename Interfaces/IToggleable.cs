using System.Drawing;

namespace ArtilleryStrike
{
    interface IToggleable
    {
        Color DeselectedColor { get; set; }
        Color SelectedColor { get; set; }
    }
}
