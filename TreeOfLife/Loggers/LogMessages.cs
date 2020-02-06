using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;

namespace TreeOfLife
{
    //===========================================================
    // One message
    //
    public class LogMessage
    {
        public LogMessage(string _tag, string _message, MessageLevelEnum _level = MessageLevelEnum.Information )
        {
            Tag = _tag;
            Message = _message;
            Time = DateTime.Now;
            Color = Color.Black;
            Level = _level;
        }
        
        public string Message = "";
        public string Tag = "";
        public Color Color;
        public MessageLevelEnum Level = 0;
        public DateTime Time;
    }

    public class LogMessages : Logger, IList
    {
        private List<LogMessage> _Messages = new List<LogMessage>();
        public List<LogMessage> Messages { get { return _Messages; } }

        public event EventHandler OnLogMessagesChanged = null;
        public void SendChangedEvent()
        {
            OnLogMessagesChanged?.Invoke(this, new EventArgs());
        }

        protected override void InternalWrite(MessageLevelEnum _level, string _tag, string _message)
        {
            _Messages.Add(new LogMessage(_tag, _message, _level));
            SendChangedEvent();
        }

        #region Implementation of IEnumerable
        public IEnumerator<LogMessage> GetEnumerator() { return _Messages.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        #endregion

        #region Implementation of ICollection

        public int Count { get { return _Messages.Count; } }
        public bool IsSynchronized { get; }
        public object SyncRoot { get; }
        public void CopyTo(Array array, int arrayIndex) { throw new NotImplementedException(); }

        #endregion

        #region Implementation of IList

        public object this[int index]
        {
            get { return _Messages[index]; }
            set { _Messages[index] = value as LogMessage; }
        }

        public bool IsFixedSize { get { return false; } }
        public bool IsReadOnly { get { return false; } }

        public int Add(object item)
        {
            if (item is LogMessage)
            {
                _Messages.Add(item as LogMessage);
                return 1;
            }
            return 0;
        }

        public void Clear() { _Messages.Clear(); }
        public bool Contains(object item) { return _Messages.Contains(item); }
        public int IndexOf(object item) { return _Messages.IndexOf(item as LogMessage); }
        public void Insert(int index, object item) { if (item is LogMessage) _Messages.Insert(index, item as LogMessage); }
        public void Remove(object item) { _Messages.Remove(item as LogMessage); }
        public void RemoveAt(int index) { _Messages.RemoveAt(index); }
        
        #endregion
    }

    // TODO a virer d'ici
    public class LogListBox : ListBox
    {
        public LogListBox()
            : base()
        {
            DrawMode = DrawMode.OwnerDrawVariable;
            BackColor = Color.DimGray;

            leftText.LineAlignment = StringAlignment.Center;
            leftText.Alignment = StringAlignment.Near;

            rightText.LineAlignment = StringAlignment.Center;
            rightText.Alignment = StringAlignment.Far;

            centeredText.LineAlignment = StringAlignment.Center;
            centeredText.Alignment = StringAlignment.Center;
        }

        LogMessage GetMessageAt(int _index)
        {
            if (_index < 0 || _index >= Items.Count)
                return null;
            return Items[_index] as LogMessage;
        }

        int _HeightMinimum = 0;
        int _WidthTime = 50;
        int _WidthTag = 50;
        int _WidthInter = 4;
        int _HeightInter = 4;

        StringFormat centeredText = new StringFormat();
        StringFormat rightText = new StringFormat();
        StringFormat leftText = new StringFormat();

        protected override void OnMeasureItem(MeasureItemEventArgs e)
        {
            base.OnMeasureItem(e);

            if (_HeightMinimum == 0)
                _HeightMinimum = (int) (e.Graphics.MeasureString( "fg", Font).Height + 0.5);

            LogMessage message = GetMessageAt( e.Index );
            if (message == null) 
                return;

            SizeF size = e.Graphics.MeasureString(message.Message, Font);
            e.ItemHeight = (int)(size.Height + 0.5) + _HeightInter * 2;
            if (e.ItemHeight > 255) e.ItemHeight = 255;
            if (e.ItemHeight < _HeightMinimum) e.ItemHeight = _HeightMinimum;

            e.ItemWidth = _WidthTime + _WidthInter + _WidthTag + _WidthInter + (int)(size.Width + 0.5);
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            e.Graphics.FillRectangle( (e.Index & 1) == 1 ? Brushes.Gray : Brushes.DimGray, e.Bounds );

            LogMessage message = GetMessageAt( e.Index );
            if (message != null) 
            {
                Rectangle R = e.Bounds;
                R.Width = _WidthTime;
                e.Graphics.DrawString(message.Time.ToString("HH:mm:ss"), Font, Brushes.Wheat, R, rightText);

                R.X = R.Right + _WidthInter;
                R.Width = _WidthTag;
                //e.Graphics.DrawString(message.Tag, Font, Brushes.Wheat, R, centeredText);
                e.Graphics.DrawString(message.Tag, Font, Brushes.Black, R, centeredText);
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(50, Color.Wheat)), R);

                R.X = R.Right + _WidthInter;
                R.Width = e.Bounds.Width - _WidthTime - _WidthInter - _WidthTag - _WidthInter;
                e.Graphics.DrawString(message.Message, Font, Brushes.Wheat, R, leftText);
            }

            if ( (e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                Rectangle R = e.Bounds;
                R.Inflate(-1, -1);
                e.Graphics.DrawRectangle(Pens.Silver, R);
            }
        }
    }
}
