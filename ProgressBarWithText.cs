/*
 * User: WuTao
 * Date: 2016/11/25 20:25
 * Note: 
 */
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ProgressBarWithText
{
    public partial class ProgressBarWithText : UserControl
	{
		#region 构造函数
		public ProgressBarWithText()
		{
			InitializeComponent();
			this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint,true);
            this.DoubleBuffered = true;

            #region  初始化所有值
            //最大值
            _maxValue = 100;
            //当前值
			_value = 0;
            //文本对齐方式
			_position = TextAlignmentStyle.Center;
            //进度条背景色
            _BackColor = Color.FromArgb(255, 230, 230, 230);
            //前景色
			_ForeColor = Color.Green;
            //背景矩形
			_BackRect = this.ClientRectangle;
            //前景矩形
			_ForeRect = this.ClientRectangle;
			_Rect = this.ClientRectangle;
            //字体
			_Font = new Font(FontFamily.GenericSansSerif,9);
            //不使用浮点数
            _UseDecimal = false;
            //文本呈现方式
            _DisplayMode = ValueDisplayMode.Percent;
            //尺寸
            this.Size = new Size(50, 20);
            ShowText = true;
            #endregion

            //宽度改变时重绘
            this.SizeChanged += (sender, e) => OnPaint(new PaintEventArgs(this.CreateGraphics(),this.ClientRectangle));
			
		}
		#endregion
		
		#region 字段列表
		//最大值
		private int _maxValue = 100;
		//当前值
		private int _value = 0;
		//背景矩形
		private Rectangle _BackRect;
		//前景矩形
		private Rectangle _ForeRect;
		//背景矩形颜色
		private Color _BackColor;
		//前景矩形颜色
		private Color _ForeColor;
		//字体
		private Font _Font;
		//文本所在的位置
		private TextAlignmentStyle _position;
		//客户区域矩形
		private Rectangle _Rect;
		//是否使用浮点数显示进度
		private bool _UseDecimal;
        //进度条值显示方式
        private ValueDisplayMode _DisplayMode;
        //是否显示文本
        private bool _ShowText;

        #endregion

        #region 枚举值
        /// <summary>
        /// 文本的对齐方式
        /// </summary>
        public enum TextAlignmentStyle : byte
        {
            Left = 0,
            Right,
            Center
        };

        /// <summary>
        /// 文本的显示方式，值还是百分比
        /// </summary>
        public enum ValueDisplayMode : byte
        {
            Value = 1,
            Percent = 0
        };
        #endregion

        #region 隐藏不必要的属性
        //隐藏BackgroundImage属性，并且在智能感知中隐藏
        [Browsable(false),EditorBrowsable(EditorBrowsableState.Never)]
        public new Image BackgroundImage 
        {
	        get;set;
        }
        //隐藏BackgroundImageLayout属性
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new ImageLayout BackgroundImageLayout
		{
			get;set;
		}
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new Font Font
        {
            set;get;
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new Color BackColor
        {
            set;
            get;
        }

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public new Color ForeColor
        {
            set;get;
        }

        #endregion

        #region 自定义的属性
        [DefaultValue("1.0"),ReadOnly(true),Browsable(false)]
        public string Version
        {
            get { return "1.0" + ", " + System.IO.File.GetLastWriteTime(this.GetType().Assembly.Location).ToString(); }
        }

        [DefaultValue("W.T"),ReadOnly(true), Browsable(false)]
        public string Author
        {
            get { return "W.T"; }
        }

		[Description("是否使用浮点数显示百分比"),Category("行为")]
		public bool UseDecimal
		{
			get { return _UseDecimal; }
			set
            {
                _UseDecimal = value;
                this.OnPaint(new PaintEventArgs(this.CreateGraphics(), this.ClientRectangle));
            }
		}

        [Description("是否显示文本。"),Category("外观")]
        public bool ShowText
        {
            get { return _ShowText; }
            set
            {
                _ShowText = value;
                this.OnPaint(new PaintEventArgs(this.CreateGraphics(), this.ClientRectangle));
            }
        }
		
		/// <summary>
		/// 设置进度条上文本的对齐方式
		/// </summary>
		[System.ComponentModel.Description("文本的对齐方式")]
        [Category("外观")]
		public TextAlignmentStyle TextAlignment
		{
			get {return _position; }
			set
            {
                _position = value;
                this.OnPaint(new PaintEventArgs(this.CreateGraphics(), this.ClientRectangle));
            }
		}
		
		/// <summary>
		/// 进度条的最大值
		/// </summary>
		public int MaxValue
		{
			get {return _maxValue;}
			set
			{
				if(value <= 0)
					_maxValue = 1;
				if(value < _value)
					_maxValue = _value;
				_maxValue = value;
                this.OnPaint(new PaintEventArgs(this.CreateGraphics(), this.ClientRectangle));
            }
		}
		
		/// <summary>
		/// 进度条的值
		/// </summary>
		public int Value
		{
			set 
			{
                if (value >= 0 && value <= _maxValue)
                    _value = value;
                else if (value > _maxValue)
                    _value = _maxValue;
                else
                    return;
                this.OnPaint(new PaintEventArgs(this.CreateGraphics(),this.ClientRectangle));
			}
			get {return _value;}
		}
		
		/// <summary>
		/// 进度条的前景色，覆盖继承的virtual属性
		/// </summary>
		[Description("进度条的前景色")]
        [Category("外观")]
        public Color ForegroundColor
		{
			get { return _ForeColor;}
			set
            {
                _ForeColor = value;
                this.OnPaint(new PaintEventArgs(this.CreateGraphics(), this.ClientRectangle));
            }
		}
		
		/// <summary>
		/// 进度条的背景色
		/// </summary>
		[Description("进度条的背景色")]
        [Category("外观")]
        public Color BackgroundColor
		{
			get {return _BackColor;}
			set
            {
                _BackColor = value;
                this.OnPaint(new PaintEventArgs(this.CreateGraphics(), this.ClientRectangle));
                
            }
		}

        /// <summary>
        /// 进度条上显示文本的字体
        /// </summary>
        [Category("外观")]
        public Font SetFont
		{
			get { return _Font; }
			set
            {
                _Font = value;
                this.OnPaint(new PaintEventArgs(this.CreateGraphics(), this.ClientRectangle));
            }
		}
        /// <summary>
        /// 进度条值的显示方式，文本还是值
        /// </summary>
        public ValueDisplayMode DisplayMode
        {
            get { return _DisplayMode; }
            set
            {
                _DisplayMode = value;
                this.OnPaint(new PaintEventArgs(this.CreateGraphics(), this.ClientRectangle));
            }
        }
        #endregion

        #region 重写Paint方法
        Graphics g;
		protected override void OnPaint(PaintEventArgs pe)
		{
			g = pe.Graphics;
			_Rect = this.ClientRectangle;
			_BackRect = this.ClientRectangle;
			_ForeRect = this.ClientRectangle;
			
			//前景矩形的宽度 = 背景矩形的宽度 / 进度条的最大值 * 进度条的当前值
			_ForeRect.Width = (int)((decimal)_BackRect.Width / _maxValue * _value);
            //_ForeRect.Height = _BackRect.Height - 2;
            //_ForeRect.Y = _BackRect.Y + 1;

			//绘制背景矩形
			g.FillRectangle(new SolidBrush(_BackColor),_BackRect);
			//绘制前景矩形
			g.FillRectangle(new SolidBrush(_ForeColor),_ForeRect);
            //要绘制的文本
            string strCurrentPercent = "";
            
            if (_UseDecimal == true && _DisplayMode == ValueDisplayMode.Percent )
            {
                strCurrentPercent = string.Format("{0:F2}%",(decimal)_value / _maxValue * 100);
            }
            else if(_UseDecimal == false && _DisplayMode == ValueDisplayMode.Percent)
            {
                strCurrentPercent = string.Format("{0:F0}%", (decimal)_value / _maxValue * 100);
            }
            else if(_DisplayMode == ValueDisplayMode.Value)
            {
                strCurrentPercent = string.Format("{0}/{1}",_value,_maxValue);
            }
            else
            {
                strCurrentPercent = string.Format("{0}/{1}", _value, _maxValue);
            }
            if(_ShowText == true)
            { 
                //文本的尺寸
			    SizeF textSize = g.MeasureString(strCurrentPercent,_Font);
                //var textSize = path.GetBounds();
			    //绘制文本
			    if(_position == TextAlignmentStyle.Left)
			    {
				    //左对齐
				    g.DrawString(strCurrentPercent, _Font,Brushes.Black,
				                  _BackRect.X,_BackRect.Height / 2 - textSize.Height / 2);
			    }
			    else if(_position == TextAlignmentStyle.Right)
			    {
				    //右对齐
				    g.DrawString(strCurrentPercent, _Font,Brushes.Black,
				                 _BackRect.Width - textSize.Width,_BackRect.Height / 2 - textSize.Height / 2);
			    }
			    else
			    {
				    //居中对齐
				    g.DrawString(strCurrentPercent, _Font, Brushes.Black,
				                _BackRect.Width / 2 - textSize.Width / 2, _BackRect.Height / 2 - textSize.Height / 2);
			    }
            }
            base.OnPaint(pe);
		}
        #endregion
    }
}