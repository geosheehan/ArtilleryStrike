using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ArtilleryStrike
{
    public class RenamableButton : Button, IToggleable
    {
        public Color SelectedColor { get; set; }
        public Color DeselectedColor { get; set; }
        public override Color ForeColor { get => base.ForeColor; set { base.ForeColor = m_textbox.ForeColor = value; } }

        private uint? m_num = null;
        private bool? m_is_checked = null;

        private TextBox m_textbox;

        public RenamableButton() : base()
        {
            Text = "renamableButton";
            SelectedColor = Color.Yellow;
            DeselectedColor = base.BackColor;

            InitializeLabel();
            Controls.Add(m_textbox);

            SetStyle(ControlStyles.StandardClick | ControlStyles.StandardDoubleClick, true);
        }

        public uint GetNumber() => m_num ?? 0;
        public void SetNumber(uint value)
        {
            if (m_num == null)
            {
                m_num = value;
                Text = "Location " + m_num.ToString();
                if (m_textbox != null)
                {
                    m_textbox.Text = Text;
                }
            }
        }

        public bool Checked
        {
            get => m_is_checked ?? false;
            set { m_is_checked = value; Invalidate(); }
        }
        
        private void InitializeLabel()
        {
            m_textbox = m_textbox == null ? new TextBox() : m_textbox;

            m_textbox.Enabled = false;
            m_textbox.Visible = false;

            m_textbox.ForeColor = ForeColor;
            m_textbox.BackColor = BackColor;
            m_textbox.BorderStyle = BorderStyle.None;
            m_textbox.Cursor = Cursor;
            m_textbox.AutoSize = false;
            m_textbox.Text = Text;
            m_textbox.TextAlign = HorizontalAlignment.Center;
            m_textbox.MaxLength = 20;
            m_textbox.TabStop = false;
            m_textbox.Leave += new EventHandler(OnLabelLeave);
        }

        public bool InitCheckedStatus()
        {
            bool initialized = false;
            if (Parent != null)
            {
                initialized = true;
                List<RenamableButton> buttons = Parent.Controls.OfType<RenamableButton>().ToList();

                uint num = 1;
                foreach (RenamableButton button in buttons)
                {
                    button.SetNumber(num++);
                    button.Text = "Location " + button.GetNumber().ToString();
                    button.m_textbox.Text = button.Text;
                }

                if (buttons.All(button => !button.Checked))
                {
                    buttons[0].Checked = true;
                    buttons[0].UpdateButtons();
                }
            }
            return initialized;
        }

        private void UpdateButtons()
        {
            if (Parent != null)
            {
                List<RenamableButton> buttons = Parent.Controls.OfType<RenamableButton>().ToList();
                foreach (RenamableButton button in buttons)
                {
                    if (button != this)
                    {
                        button.Checked = !Checked;
                    }
                    button.UpdateColor(button.Checked);
                }
            }
        }

        public RenamableButton ActiveButton()
        {
            RenamableButton active_button = null;
            if (Parent != null)
            {
                List<RenamableButton> buttons = Parent.Controls.OfType<RenamableButton>().ToList();
                foreach (RenamableButton button in buttons)
                {
                    if (button.Checked)
                    {
                        active_button = button;
                        break;
                    }
                }
            }
            return active_button;
        }

        private void UpdateColor(bool highlighted)
        {
            Color display = (highlighted) ? SelectedColor : DeselectedColor;
            FlatAppearance.BorderColor = display;
            BackColor = display;
            m_textbox.BackColor = display;
        }

        protected override void OnClick(EventArgs e)
        {
            Checked = true;
            UpdateButtons();
            base.OnClick(e);
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
            m_textbox.Font = Font;
            CenterText();
            m_textbox.Enabled = true;
            m_textbox.Visible = true;
            m_textbox.Focus();
            m_textbox.SelectAll();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            m_textbox.Height = Height;
            m_textbox.Width = Width;
        }

        public void Clear()
        {
            m_textbox.Text = "";
            OnLabelLeave(m_textbox, new EventArgs());
        }

        private void OnLabelLeave(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb != null)
            {
                if (tb.Text.Length == 0)
                {
                    tb.Text = "Location " + GetNumber().ToString();
                }
                Text = tb.Text;

                tb.Enabled = false;
                tb.Visible = false;
            }
        }

        private void CenterText()
        {
            int text_height = TextRenderer.MeasureText(m_textbox.Text, m_textbox.Font).Height;
            int text_width = m_textbox.Width;

            int textbox_x = ( Width -  text_width) / 2;
            int textbox_y = (Height - text_height) / 2;

            m_textbox.Location = new Point(textbox_x, textbox_y);
        }
    }
}
