using System;
using System.Windows.Forms;

namespace ArtilleryStrike
{
    public partial class PreferenceForm : Form
    {
        private new Form ParentForm { get; set; }

        public PreferenceForm(Form parent)
        {
            InitializeComponent();
            if (parent != null)
            {
                ParentForm = parent;
                double focus_opacity = Properties.Settings.Default.OptionFocusOpacity;
                double unfocus_opacity = Properties.Settings.Default.OptionUnfocusOpacity;
                m_tb_foc_transparancy.Value = (int)(100 - (focus_opacity * 100));
                m_tb_unfoc_transparancy.Enabled = ParentForm.TopMost;
                m_tb_unfoc_transparancy.Value = (int)(100 - (unfocus_opacity * 100));
            }
        }

        protected override void OnShown(EventArgs e)
        {
            if (ParentForm != null)
            {
                ParentForm.Opacity = (100 - m_tb_foc_transparancy.Value) / 100.0;
            }
            base.OnShown(e);
        }

        private void OnUpdateTransparancy(object sender, EventArgs e)
        {
            if (sender is TrackBar)
            {
                if ((sender as TrackBar) == m_tb_foc_transparancy)
                {
                    ParentForm.Opacity = (100 - m_tb_foc_transparancy.Value) / 100.0;
                    Properties.Settings.Default.OptionFocusOpacity = ParentForm.Opacity;
                    m_lbl_foc_tran_percent.Text = m_tb_foc_transparancy.Value.ToString() + "%";
                }
                else if ((sender as TrackBar) == m_tb_unfoc_transparancy)
                {
                    ParentForm.Opacity = (100 - m_tb_unfoc_transparancy.Value) / 100.0;
                    Properties.Settings.Default.OptionUnfocusOpacity = ParentForm.Opacity;
                    m_lbl_unfoc_tran_percent.Text = m_tb_unfoc_transparancy.Value.ToString() + "%";
                }
                Properties.Settings.Default.Save();
            }
        }

        private void OnChangeTeam(object sender, EventArgs e)
        {
            ComboBox team = (ComboBox)sender;
            char new_team = team.SelectedItem.ToString()[0];

            Properties.Settings.Default.OptionTeam = new_team;
            Properties.Settings.Default.Save();
            FormPainter.PaintStyle(ParentForm, new_team);
        }
    }
}
