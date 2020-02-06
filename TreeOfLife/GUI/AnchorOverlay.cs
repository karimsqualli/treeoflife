using System.Drawing;
using System.Windows.Forms;

namespace TreeOfLife.GUI
{
    //=============================================================================================
    // AnchorOverlay
    //
    internal sealed class AnchorOverlay : Form
    {
        //---------------------------------------------------------------------------------------
        public AnchorOverlay()
        {
            InitializeComponent();
        }

        //---------------------------------------------------------------------------------------
        private PictureBox _PictureInactive;
        private PictureBox _PictureCenter;
        private PictureBox _PictureLeft;
        private PictureBox _PictureRight;
        private PictureBox _PictureTop;
        private PictureBox _PictureBottom;

        //---------------------------------------------------------------------------------------
        private void InitializeComponent()
        {
            SuspendLayout();
            
            BackColor = Color.Yellow;
            TransparencyKey = Color.Yellow;
            FormBorderStyle = FormBorderStyle.None;
            ControlBox = false;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Overlay";
            Opacity = 0.8;
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Text = "Overlay";
            TopMost = true;
            Height = 90;
            Width = 90;

            _PictureInactive = new PictureBox
            {
                Image = global::TreeOfLife.Properties.Resources.DockIndicator_Inactive,
                Location = new System.Drawing.Point(1, 1),
                Size = new System.Drawing.Size(88, 88)
            };
            Controls.Add(_PictureInactive);

            _PictureCenter = new PictureBox
            {
                Image = global::TreeOfLife.Properties.Resources.DockIndicator_Center,
                Location = new System.Drawing.Point(1, 1),
                Size = new System.Drawing.Size(88, 88),
                Visible = false
            };
            Controls.Add(_PictureCenter);

            _PictureLeft = new PictureBox
            {
                Image = global::TreeOfLife.Properties.Resources.DockIndicator_Left,
                Location = new System.Drawing.Point(1, 1),
                Size = new System.Drawing.Size(88, 88),
                Visible = false
            };
            Controls.Add(_PictureLeft);

            _PictureRight = new PictureBox
            {
                Image = global::TreeOfLife.Properties.Resources.DockIndicator_Right,
                Location = new System.Drawing.Point(1, 1),
                Size = new System.Drawing.Size(88, 88),
                Visible = false
            };
            Controls.Add(_PictureRight);

            _PictureTop = new PictureBox
            {
                Image = global::TreeOfLife.Properties.Resources.DockIndicator_Top,
                Location = new System.Drawing.Point(1, 1),
                Size = new System.Drawing.Size(88, 88),
                Visible = false
            };
            Controls.Add(_PictureTop);

            _PictureBottom = new PictureBox
            {
                Image = global::TreeOfLife.Properties.Resources.DockIndicator_Bottom,
                Location = new System.Drawing.Point(1, 1),
                Size = new System.Drawing.Size(88, 88),
                Visible = false
            };
            Controls.Add(_PictureBottom);
            
            ResumeLayout(false);
        }

        //---------------------------------------------------------------------------------------
        ControlContainerInterface _Container = null;
        public void ShowOn(ControlContainerInterface _on)
        {
            _Container = _on;

            Rectangle R = _on.GetControl().ClientRectangle;
            R = _on.GetControl().RectangleToScreen(R);

            Show();
            int X = R.Left + R.Width / 2;
            int Y = R.Top + R.Height / 2;
            Left = X - 45;
            Top = Y - 45;
        }

        //---------------------------------------------------------------------------------------
        public void HitTest()
        {
            Point pos = PointToClient(Cursor.Position);
            int X = -1;
            if (pos.X > 0)
            {
                if (pos.X < 30) X = 0;
                else if (pos.X < 60) X = 1;
                else if (pos.X < 90) X = 2;
            }

            int Y = -1;
            if (pos.Y > 0)
            {
                if (pos.Y < 30) Y = 0;
                else if (pos.Y < 60) Y = 1;
                else if (pos.Y < 90) Y = 2;
            }

            PictureBox picture = _PictureInactive;
            if (X == 0 && Y == 1) picture = _PictureLeft;
            else if (X == 1 && Y == 0) picture = _PictureTop;
            else if (X == 1 && Y == 1) picture = _PictureCenter;
            else if (X == 1 && Y == 2) picture = _PictureBottom;
            else if (X == 2 && Y == 1) picture = _PictureRight;

            if (picture.Visible == false)
            {
                if (_PictureInactive.Visible) _PictureInactive.Visible = false;
                if (_PictureCenter.Visible) _PictureCenter.Visible = false;
                if (_PictureLeft.Visible) _PictureLeft.Visible = false;
                if (_PictureTop.Visible) _PictureTop.Visible = false;
                if (_PictureBottom.Visible) _PictureBottom.Visible = false;
                if (_PictureRight.Visible) _PictureRight.Visible = false;
                picture.Visible = true;
            }
        }

        //---------------------------------------------------------------------------------------
        public DockStyle HitTestResult(out GUI.ControlContainerInterface _in  )
        {
            _in = _Container;
            if (_in == null) return DockStyle.None;

            if (_PictureCenter.Visible) return DockStyle.Fill;
            if (_PictureLeft.Visible) return DockStyle.Left;
            if (_PictureRight.Visible) return DockStyle.Right;
            if (_PictureTop.Visible) return DockStyle.Top;
            if (_PictureBottom.Visible) return DockStyle.Bottom;
            return DockStyle.None;
        }
    }
}