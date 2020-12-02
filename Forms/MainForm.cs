using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ArtilleryStrike
{
    public partial class MainForm : Form
    {
        private Properties.Settings m_settings = Properties.Settings.Default;

        private SizeF m_old_size = Properties.Settings.Default.DefaultSize;
        private Timer m_activation_timer;
        private ColorPalette.BasePalette palette;

        private static readonly string m_pop_timer = "10:00";
        private static readonly string[] m_teams = { "friend", "target" };

        private Dictionary<string, List<RenamableButton> > m_locations = new Dictionary<string, List<RenamableButton> >();
        private Dictionary<string, Dictionary<RenamableButton, LocationState>> m_states = new Dictionary<string, Dictionary<RenamableButton, LocationState>>();

        public MainForm()
        {
            InitializeComponent();

            m_btn_timer.Text = m_pop_timer;
            
            if (!string.IsNullOrEmpty(m_settings.TimeRemaining))
            {
                Text = Text + " - " + m_settings.TimeRemaining;
                AddActivationTimer();
            }

            palette = FormPainter.PaintStyle(this, m_settings.OptionTeam);

            Dictionary<string, Panel> button_panels = new Dictionary<string, Panel>();
            button_panels.Add(m_teams.First(), m_lyt_friend_buttons);
            button_panels.Add( m_teams.Last(), m_lyt_target_buttons);

            foreach (string team in m_teams)
            {
                // Construt inner structure
                m_locations[team] = new List<RenamableButton>();
                m_states[team] = new Dictionary<RenamableButton, LocationState>();

                // Fill location structure
                Panel button_panel = button_panels[team];
                m_locations[team].AddRange(button_panel.Controls.OfType<RenamableButton>());

                // Fill state structure
                foreach (RenamableButton key in m_locations[team])
                {
                    m_states[team].Add(key, new LocationState());
                }

                m_locations[team].First().InitCheckedStatus();
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            if (TopMost)
            {
                Opacity = m_settings.OptionFocusOpacity;
            }
            base.OnActivated(e);
        }

        protected override void OnDeactivate(EventArgs e)
        {
            try
            {
                if (TopMost)
                {
                    Opacity = m_settings.OptionUnfocusOpacity;
                }
            }
            catch (Exception) { }
            base.OnDeactivate(e);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool output = false;

            Dictionary<string, Keys> modifier = new Dictionary<string, Keys>();
            modifier[m_teams.First()] = Keys.Alt;
            modifier[m_teams.Last()]  = Keys.Control;

            Dictionary<string, List<Keys>> hotkeys = new Dictionary<string, List<Keys>>();
            foreach (string team in m_teams)
            {
                hotkeys[team] = new List<Keys>();
                for (int i = 1; i <= m_locations[team].Count; i++)
                {
                    hotkeys[team].Add((modifier[team] | Keys.NumPad0 + i));
                }
            }

            foreach(KeyValuePair<string, List<Keys>> entry in hotkeys)
            {
                if (entry.Value.Contains(keyData))
                {
                    int index = entry.Value.FindIndex(k => k == keyData);
                    m_locations[entry.Key][index].PerformClick();
                    output = true;
                    break;
                }
            }

            if (!output)
            {
                if (keyData == (Keys.Tab | Keys.Control))
                {
                    m_nud_old_distance.Focus();
                    output = true;
                }
                else
                {
                    output = base.ProcessCmdKey(ref msg, keyData);
                }
            }
            return output;
        }

        protected override void OnLoad(EventArgs e)
        {
            StartPosition = FormStartPosition.Manual;

            Size = m_settings.OptionSize == Size.Empty ? m_settings.DefaultSize : m_settings.OptionSize;

            // Make sure the majority of the form is on one of the user's screens, otherwise reset it to 0,0
            Location = (!Screen.AllScreens.Contains(Screen.FromControl(this))) ? new Point(0, 0) : m_settings.OptionLocation;
            Opacity = m_settings.OptionFocusOpacity;
            m_mi_front.Checked = m_settings.OptionTopMost;
            TopMost = m_settings.OptionTopMost;
            base.OnLoad(e);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            m_settings.OptionLocation = Location;
            m_settings.OptionSize = Size;
            if (!string.IsNullOrEmpty(m_settings.TimeRemaining))
            {
                m_settings.TimeRemaining = TimeRemaining();
            }
            m_settings.Save();

            base.OnFormClosing(e);
        }

        protected override void OnResize(EventArgs e)
        {
            float h = (Size.Height / m_old_size.Height);
            float w = (Size.Width / m_old_size.Width);

            List<Control> controls = new List<Control>();
            FormPainter.GetControls(this, controls);
            foreach (Control control in controls)
            {
                control.Font = new Font(control.Font.FontFamily, control.Font.SizeInPoints * h, control.Font.Style);
            }
            m_old_size = Size;
            base.OnResize(e);
        }
        
        private void OnSpotUpdateEnter(object sender, EventArgs e)
        {
            m_nud_old_distance.TabStop = true;
            m_nud_old_azimuth.TabStop = true;
            OnTextBoxEnter(sender, e);
        }

        private void OnSpotUpdateLeave(object sender, EventArgs e)
        {
            m_nud_old_distance.TabStop = false;
            m_nud_old_azimuth.TabStop = false;
        }

        private void OnTextBoxEnter(object sender, EventArgs e)
        {
            OnTextBoxClick(sender, e);
        }

        private void OnTextBoxClick(object sender, EventArgs e)
        {
            if (sender is NumericUpDown)
            {
                NumericUpDown up_down = sender as NumericUpDown;
                if (up_down != null)
                {
                    up_down.Select(0, up_down.Text.Length);
                }
            }
        }

        private void OnLoadLocationState(object sender, EventArgs e)
        {
            RenamableButton key = (RenamableButton)sender;
            if (key != null)
            {
                // Get my LocationState
                string control_set = "";
                LocationState location = null;
                foreach(string team in m_teams)
                {
                    if (key.Name.Contains(team))
                    {
                        control_set = team;
                        location = m_states[team][key];
                        break;
                    }
                }

                // Load state values into my Parent's boxes
                if (control_set == m_teams.First())
                {
                    m_nud_friend_distance.Value = (decimal)location.Distance;
                    m_nud_friend_azimuth.Value = (decimal)location.Azimuth;
                }
                else if (control_set == m_teams.Last())
                {
                    m_nud_target_distance.Value = (decimal)location.Distance;
                    m_nud_target_azimuth.Value = (decimal)location.Azimuth;
                }
            }
        }

        private void OnTextBoxValueUpdate(object sender, EventArgs e)
        {
            OnLocationSave(sender, e);

            string team = m_teams.First();
            string other = m_teams.Last();

            // Get friend location state
            RenamableButton key = m_locations[team].First().ActiveButton();
            LocationState friend = m_states[team][key];

            // Get target location state
            key = m_locations[other].First().ActiveButton();
            LocationState target = m_states[other][key];

            // run calculation
            LocationState result = LocationState.CalculateVector(friend, target);

            double distance = Math.Round(result.Distance, 1);

            Color result_text = (distance < 75 || distance > 150) ? (new ColorPalette.BasePalette()).Accent : Color.White;
            m_lbl_result_dist_value.ForeColor = m_lbl_result_azim_value.ForeColor = result_text;

            m_lbl_result_dist_value.Text = distance.ToString("F1");
            m_lbl_result_azim_value.Text = Math.Round(result.Azimuth, 1).ToString("F1");
        }

        private void OnLocationSave(object sender, EventArgs e)
        {
            NumericUpDown nud = (NumericUpDown)sender;
            if (nud.Parent != null)
            {
                RenamableButton button_key = null;
                Dictionary<RenamableButton, LocationState> state = null;
                foreach (string team in m_teams)
                {
                    if (nud.Name.Contains(team))
                    {
                        button_key =  m_locations[team].First().ActiveButton();
                        state = m_states[team];
                        break;
                    }
                }

                if (button_key != null && state != null)
                {
                    List<NumericUpDown> boxes = nud.Parent.Controls.OfType<NumericUpDown>().ToList();
                    if (nud == boxes.First())
                    {
                        state[button_key].Distance = (double)boxes.First().Value;
                    }
                    else if (nud == boxes.Last())
                    {
                        state[button_key].Azimuth = (double)boxes.Last().Value;
                    }
                    state[button_key].Default = false;
                }
            }
        }

        private void OnConfirm(object sender, EventArgs e)
        {
            if (sender is ConfirmationButton && (sender as ConfirmationButton) == m_cbtn_confirm)
            {
                LocationState new_spot = new LocationState(
                    (double)m_nud_old_distance.Value,
                    (double)((m_nud_old_azimuth.Value + 180) % 360) // Point the other way
                );

                foreach (string team in m_teams)
                {
                    foreach (RenamableButton key in m_locations[team])
                    {
                        LocationState location = m_states[team][key];
                        if (!location.Default)
                        {
                            location.Assign(LocationState.CalculateVector(new_spot, location));
                        }
                    }
                    m_locations[team].First().ActiveButton().PerformClick();
                }

                m_nud_old_distance.Value = 1;
                m_nud_old_azimuth.Value = 0;
            }
        }

        private void OnReset(object sender, EventArgs e)
        {
            if (sender is ConfirmationButton && (sender as ConfirmationButton) == m_cbtn_reset)
            {
                foreach (string team in m_teams)
                {
                    foreach (RenamableButton key in m_states[team].Keys)
                    {
                        key.Clear();
                        m_states[team][key].Clear();
                    }
                    m_locations[team].First().ActiveButton().PerformClick();
                }
                m_lbl_result_azim_value.ForeColor = Color.White;
                m_lbl_result_dist_value.ForeColor = Color.White;
            }
        }

        private void OnTimerStartReset(object sender, EventArgs e)
        {
            if (m_timer.Enabled)
            {
                // Reset
                m_btn_timer.Text = m_pop_timer;
                m_btn_timer.BackColor = palette.ButtonBackGround;
                m_btn_timer.FlatAppearance.BorderColor = palette.ButtonBorder;
            }
            else
            {
                int seconds = (int)TimeSpan.Parse("00:" + m_btn_timer.Text).TotalSeconds;
                seconds--;
                m_btn_timer.Text = TimeSpan.FromSeconds(seconds).ToString(@"mm\:ss");

                m_btn_timer.BackColor = palette.Accent;
                m_btn_timer.FlatAppearance.BorderColor = palette.Accent;
            }
            m_timer.Enabled = !m_timer.Enabled;
        }

        private void OnTick(object sender, EventArgs e)
        {
            int seconds = (int)TimeSpan.Parse("00:" + m_btn_timer.Text).TotalSeconds;
            seconds--;

            if (seconds > -1)
            {
                m_btn_timer.Text = TimeSpan.FromSeconds(seconds).ToString(@"mm\:ss");
            }
            else
            {
                m_btn_timer.PerformClick();
            }
        }

        private void OnBringToFront(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem && (sender as ToolStripMenuItem) == m_mi_front)
            {
                ToolStripMenuItem item = (ToolStripMenuItem)sender;
                item.Checked = !item.Checked;
                TopMost = item.Checked;
                m_settings.OptionTopMost = TopMost;
            }
        }

        private void OnShowPreference(object sender, EventArgs e)
        {
            PreferenceForm form = new PreferenceForm(this);
            form.TopMost = TopMost;
            form.ShowDialog();
        }

        private string TimeRemaining()
        {
            string[] title = Text.Split('-');
            return title[title.Length - 1].Trim();
        }

        private void AddActivationTimer()
        {
            m_activation_timer = new Timer(components);
            m_activation_timer.Interval = 1000;
            m_activation_timer.Tick += new EventHandler(OnActivationTick);
            m_activation_timer.Enabled = true;
        }

        private void OnActivationTick(object sender, EventArgs e)
        {
            TimeSpan ts;
            string[] title = Text.Split('-');
            string time = title[title.Length - 1].Trim();

            if(TimeSpan.TryParse(time, out ts))
            {
                int seconds = (int)ts.TotalSeconds;
                seconds--;

                if (seconds > -1)
                {
                    time = TimeSpan.FromSeconds(seconds).ToString(@"hh\:mm\:ss");
                    title[title.Length - 1] = " " + time;
                    Text = string.Join("-", title);
                }
                else
                {
                    m_activation_timer.Enabled = false;
                    m_settings.HasActivated = false;
                    m_settings.TimeRemaining = "";
                    MessageBox.Show("Your 24-hour period has expired.\r\nYou will need a new Activation Code to use this software again.\r\nApplication will close.", "Expired");
                    Close();
                }
            }
        }

        private void registerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(m_settings.TimeRemaining))
            {
                m_activation_timer.Enabled = false;

                m_settings.TimeRemaining = "";
                m_settings.OptionDisableRegistration = true;

                Text = Text.Split('-')[0].Trim();
            }
        }

    }
}
