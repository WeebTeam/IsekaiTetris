/*
 * Created by SharpDevelop.
 * User: user
 * Date: 08.10.2013
 * Time: 2:46
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Tetris
{
	public class TetrisField
	{
		public int TilesWidth { get; private set; }
		public int TilesHeight { get; private set; }
		
		private TileType[,] Tiles;
		
		public Color BackColor, BorderColor;
		
		public TileType this[int row, int col]
		{
			get
			{
				try
				{
					return Tiles[row, col];
				}
				catch(IndexOutOfRangeException)
				{
					return TileType.Wall;
				}
			}
		}
		
		
		public TetrisField(int height, int width)
		{
			TilesWidth=width;
			TilesHeight=height;
			Tiles=new TileType[height, width];
			
			BorderColor=Color.FromArgb(192, 192, 192);
			BackColor=Color.FromArgb(240, 240, 240);
			
			for(int row=0; row<TilesHeight; row++)
			{
				for(int col=0; col<TilesWidth; col++)
				{
					Tiles[row, col]=TileType.Empty;
				}
			}
		}


        //=========[Modification of the cells]===
        //МОДИФИКАЦИИ ЯЧЕЕК


        public bool SetCell(int row, int col, TileType type)
		{
			try
			{
				Tiles[row, col]=type;
				return true;
			}
			catch(IndexOutOfRangeException)
			{
				return false;
			}
		}

        /// <summary>
        /// Помещает фигуру f на поле
        /// Place f on the field
        /// </summary>
        /// <returns>Количество клеток фигуры, которое удалось поместить на поле</returns>
        /// The number of cells placed on the field
        public int SetFigure(Figure f, bool rewrite)
		{
			
			int res=4;
			try
			{
				if(Tiles[f.YC, f.XC]==TileType.Empty || rewrite)
				{
					Tiles[f.YC, f.XC]=f.Type;
				}
				else --res;
			}
			catch(IndexOutOfRangeException)
			{ --res; }
			
			
			try
			{
				if(Tiles[f.Y1, f.X1]==TileType.Empty || rewrite)
				{
					Tiles[f.Y1, f.X1]=f.Type;
				}
				else --res;
			}
			catch(IndexOutOfRangeException)
			{ --res; }
			
			try
			{
				if(Tiles[f.Y2, f.X2]==TileType.Empty || rewrite)
				{
					Tiles[f.Y2, f.X2]=f.Type;
				}
				else --res;
			}
			catch(IndexOutOfRangeException)
			{ --res; }
			
			try
			{
				if(Tiles[f.Y3, f.X3]==TileType.Empty || rewrite)
				{
					Tiles[f.Y3, f.X3]=f.Type;
				}
				else --res;
			}
			catch(IndexOutOfRangeException)
			{ --res; }
			
			return res;
		}
		public bool IsEmpty(Figure f)
		{
			if(this[f.YC, f.XC]!=TileType.Empty) return false;
			if(this[f.Y1, f.X1]!=TileType.Empty) return false;
			if(this[f.Y2, f.X2]!=TileType.Empty) return false;
			if(this[f.Y3, f.X3]!=TileType.Empty) return false;
			return true;
		}
		public bool IsEmpty(int row, int col)
		{
			if(this[row, col]!=TileType.Empty) return false;
			return true;
		}
		protected void EraseFigure(Figure f)
		{
			f.Type=TileType.Empty;
			SetFigure(f, true);
		}
		
		/// <summary>
		/// Осуществляет сдвиг выбранной ячейки вниз, если это возможно
        /// Shifts the cells down if possible
		/// </summary>
		/// <param name="row">Строка</param>
        /// row /line / string
		/// <param name="col">Столбец</param>
        /// Column
		/// <returns>Возможность дальнейшего сдвига</returns>
        /// Possibility of future shifting the cells?
		protected bool MoveDown(int row, int col)
		{
			if(Tiles[row, col]!=TileType.Empty)
			{
				TileType below=Tiles[row+1, col];
				if(below==TileType.Empty)
				{
					Tiles[row+1, col]=Tiles[row, col];
					Tiles[row, col]=TileType.Empty;
				}
				return Tiles[row+2, col]==TileType.Empty; //Still can move it down
			}
			return false;
		}
		/// <summary>
		/// Передвигает совокупность ячеек вниз
        /// Moves down the set of cells
		/// </summary>
		/// <param name="f">Фигура - совокупность ячеек</param>
        /// Figure - a collection of cells
		/// <returns>Успех сдвига</returns>
        /// Shift success
		protected bool MoveDown(Figure f)
		{
			Figure lower=f.MoveDown();
			
			f.Type=TileType.Empty;
			SetFigure(f, true);
			
			if(IsEmpty(lower))
			{
				// свободно, двигаем вниз
                // free, moving down
				SetFigure(lower, false);
				return true;
			}
			else
			{
				// занято, останавливаем
                //busy, stop
				f.Type=lower.Type;
				SetFigure(f, false);
				return false;
			}
			
			
			/*bool able=CanMoveDown(f);
			if(able)
			{
				MoveDown(f.YC, f.XC); MoveDown(f.Y1, f.X1);
				MoveDown(f.Y2, f.X2); MoveDown(f.Y3, f.X3);
				able=CanMoveDown(f.MoveDown());
			}
			return able;*/
		}
		
		protected bool CanMoveDown(int row, int col)
		{
			return Tiles[row+1, col]==TileType.Empty;
		}
		
		protected bool CanMoveDown(Figure f)
		{
			Figure lower=f.MoveDown();
			
			f.Type=TileType.Empty;
			SetFigure(f, true);
			
			bool able=IsEmpty(lower);
			
			f.Type=lower.Type;
			SetFigure(f, false);
			
			return able;
			
		}
		
		/// <summary>
		/// Осуществляет сдвиг выбранной ячейки вправо, если это возможно
        /// Shift the cells to the right, if possible
		/// </summary>
		/// <param name="row">Строка</param>
        /// row
		/// <param name="col">Столбец</param>
        /// column
		/// <returns>Возможность дальнейшего сдвига</returns>
        /// Possibility of furthur shift
		protected bool MoveRight(int row, int col)
		{
			if(Tiles[row, col]!=TileType.Empty)
			{
				TileType below=Tiles[row, col+1];
				if(below==TileType.Empty)
				{
					Tiles[row, col+1]=Tiles[row, col];
					Tiles[row, col]=TileType.Empty;
				}
				return Tiles[row, col+1]==TileType.Empty; //Still can move it right
			}
			return false;
		}
		/// <summary>
		/// Передвигает совокупность ячеек вправо
        /// move the collection of cells to the right
		/// </summary>
		/// <param name="f">Фигура - совокупность ячеек</param>
        /// figure - a collection of cells
		/// <returns>Успешность сдвига</returns>
		protected bool MoveRight(Figure f)
		{
			Figure moved=f.MoveRight();
			f.Type=TileType.Empty;
			SetFigure(f, true);
			if(IsEmpty(moved))
			{
				SetFigure(moved, false);
				return true;
			}
			f.Type=moved.Type;
			SetFigure(f, false);
			return false;
		}
		
		protected bool CanMoveRight(int row, int col)
		{
			return Tiles[row, col+1]==TileType.Empty;
		}
		protected bool CanMoveRight(Figure f)
		{
			return CanMoveRight(f.YC, f.XC) && CanMoveRight(f.Y1, f.X1)
				 && CanMoveRight(f.Y2, f.X2) && CanMoveRight(f.Y3, f.X3);
		}


		/// <summary>
		/// Осуществляет сдвиг выбранной ячейки влево, если это возможно
        /// shift the cells to left, if possible
		/// </summary>
		/// <param name="row">Строка</param>
		/// <param name="col">Столбец</param>
		/// <returns>Возможность дальнейшего сдвига</returns>
        /// Possibility of furthur shift
		protected bool MoveLeft(int row, int col)
		{
			if(Tiles[row, col]!=TileType.Empty)
			{
				TileType below=Tiles[row, col-1];
				if(below==TileType.Empty)
				{
					Tiles[row, col-1]=Tiles[row, col];
					Tiles[row, col]=TileType.Empty;
				}
				return Tiles[row, col-1]==TileType.Empty; //Still can move it left
			}
			return false;
		}
		/// <summary>
		/// Передвигает совокупность ячеек влево
        /// Move the collection of cells to the left
		/// </summary>
		/// <param name="f">Фигура - совокупность ячеек</param>
        /// figure - collection of cells
		/// <returns>Успешность сдвига</returns>
        /// Sucess
		protected bool MoveLeft(Figure f)
		{
			Figure moved=f.MoveLeft();
			f.Type=TileType.Empty;
			SetFigure(f, true);
			if(IsEmpty(moved))
			{
				SetFigure(moved, false);
				return true;
			}
			f.Type=moved.Type;
			SetFigure(f, false);
			return false;
		}
		
		public bool CanMoveLeft(int row, int col)
		{
			return Tiles[row, col+1]==TileType.Empty;
		}
		public bool CanMoveLeft(Figure f)
		{
			return CanMoveLeft(f.YC, f.XC) && CanMoveLeft(f.Y1, f.X1)
				 && CanMoveLeft(f.Y2, f.X2) && CanMoveLeft(f.Y3, f.X3);
		}
		
		protected Figure RotateFigure(Figure f)
		{
			Figure rotated=f.Rotate(), rotated2;
			f.Type=TileType.Empty;
			SetFigure(f, true);
			f.Type=rotated.Type;
			
			if(IsEmpty(rotated))
			{
				SetFigure(rotated, false);
				return rotated;
			}
			//неудача, фигура наткнулась на препятствие, нужно сместить её
            //failure, the figure came across an obstacle, need to shift it
			//вниз
            // down
			rotated2=rotated.MoveDown();
			if(IsEmpty(rotated2))
			{
				SetFigure(rotated2, false);
				return rotated2;
			}
			//вправо
            //right
			rotated2=rotated.MoveRight();
			if(IsEmpty(rotated2))
			{
				SetFigure(rotated2, false);
				return rotated2;
			}
			//влево
            //left
			rotated2=rotated.MoveLeft();
			if(IsEmpty(rotated2))
			{
				SetFigure(rotated2, false);
				return rotated2;
			}
			//тотальная неудача, я сдаюсь
            //total failure, give up
			SetFigure(f, false);
			return Figure.Zero;
		}

        /// <summary>
        /// Удаляет заполненные ряды и поля со смещением всех лежащих выше вниз
        /// remove filled rows and columns with the offset of all  lying above down??(clear lines ?)
        /// </summary>
        /// <returns>Количество уничтоженных ячеек</returns>
        /// Number of destroyed cells
        public int RemoveFullRows()
		{
			//Список заполненных рядов к удалению
            //List of cleared rows to delete
			List<int> FullRows=new List<int>();
			
			for(int row=0; row<TilesHeight; row++)
			{
				bool fullrow=true;
				for(int col=0; col<TilesWidth; col++)
				{
					if(Tiles[row, col]==TileType.Empty)
					{
						fullrow=false;
						break;
					}
				}
				if(fullrow)
				{
					FullRows.Add(row);
				}
			}
			
			//Удаляем со смещением всех остальных рядов вниз
            //Remove with all other rows down
			foreach(int frow in FullRows)
			{
				for(int row=frow-1; row>0; row--)
				{
					//смещаем [row]->[row+1]
                    //shift [row]->[row+1]
					for(int col=0; col<TilesWidth; col++)
					{
						Tiles[row+1, col]=Tiles[row, col];
						if(IsRowEmpty(row+1)) //прошлый ряд пуст, следовательно, все выше лежащие так же пусты
                                              //last row is empty, therefore, all above lying are empty
							break;
					}
				}
			}
			
			return TilesWidth*FullRows.Count; //возвращаем количество уничтоженных клеток
            //return the number of destroyed cells
		}
		
		private bool IsRowEmpty(int row)
		{
			for(int col=0; col<TilesWidth; col++)
			{
				if(Tiles[row, col]!=TileType.Empty)
					return false;
			}
			return true;
		}
		
		public virtual void Clear()
		{
			for(int row=0; row<TilesHeight; row++)
			{
				for(int col=0; col<TilesWidth; col++)
				{
					SetCell(row, col, TileType.Empty);
				}
			}
		}
		
		
//=========[ ОТРИСОВКА ]===
//DRAWING
		public const int TileSide=20;
		
		public virtual void Paint(Graphics g)
		{
			Pen border=new Pen(BorderColor, 2F);
			SolidBrush fone=new SolidBrush(BackColor);
			
			g.DrawRectangle(border, 4, 4, TilesWidth*TileSide+2, TilesHeight*TileSide+2);
			g.FillRectangle(fone, 5, 5, TilesWidth*TileSide, TilesHeight*TileSide);
			
			for(int row=0; row<TilesHeight; row++)
			{
				for(int col=0; col<TilesWidth; col++)
				{
					Rectangle tile=new Rectangle(5+col*TileSide, 5+row*TileSide, TileSide, TileSide);
					switch(Tiles[row, col])
					{
						case TileType.Blue:
							if(Blue==null) g.FillRectangle(Brushes.Blue, tile);
							else g.DrawImage(Blue, tile);
							break;
						case TileType.Green:
							if(Green==null) g.FillRectangle(Brushes.Green, tile);
							else g.DrawImage(Green, tile);
							break;
						case TileType.Yellow:
							if(Yellow==null) g.FillRectangle(Brushes.Yellow, tile);
							else g.DrawImage(Yellow, tile);
							break;
						case TileType.Purple:
							if(Purple==null) g.FillRectangle(Brushes.Purple, tile);
							else g.DrawImage(Purple, tile);
							break;
						case TileType.Orange:
							if(Orange==null) g.FillRectangle(Brushes.Orange, tile);
							else g.DrawImage(Orange, tile);
							break;
						case TileType.Red:
							if(Red==null) g.FillRectangle(Brushes.Red, tile);
							else g.DrawImage(Red, tile);
							break;
						case TileType.LightBlue:
							if(LightBlue==null) g.FillRectangle(Brushes.LightBlue, tile);
							else g.DrawImage(LightBlue, tile);
							break;
					}
				}
			}
		}
//=========[ КАРТИНКИ ]===
//PICTURES
		public static Bitmap Red, Green, Blue, Yellow, Orange, Purple, LightBlue;
	}
	
	
	public class GameField: TetrisField
	{
		public Figure Current;
		/// <summary>
		/// Если текущая фигура упала и можно добавлять новую, имеет значение false
        /// if the current shape has fallen  and you can get a new shape, it is false
		/// </summary>
		public bool IsFigureFalling { get; private set; }
		public bool ShowTips;
		
		
		public GameField(int height, int width): base(height, width)
		{
			IsFigureFalling=false;
			ShowTips=true;
			Current=Figure.Zero;
		}
		
		
		/// <summary>
		/// Помещает новую фигуру на верх поля
        /// Places a new shape on the top
		/// </summary>
		/// <param name="f">Новая фигура</param>
        /// new figure
		/// <returns>true, если фигуру удалось полностью поместить на поле, иначе - false</returns>
        /// true, if the figure  could be completely placed on the field, otherwise -false
		public bool PlaceFigure(Figure f)
		{
			f=f.MoveTo(0, TilesWidth/2-1);
			int scs=SetFigure(f, false);
			Current=f;
			if(scs!=4) //game over!
				return false;
			IsFigureFalling=true;
			return true;
		}
		
		/// <summary>
		/// Заменяет текущую фигуру на новую
        /// replaces the current shape with a new one
		/// </summary>
		/// <param name="nfig">Новая фигура</param>
        /// new figure
		/// <returns>Предыдущую фигуру или Figure.Zero, если новую фигуру не удалось поместить</returns>
        /// previous figure or figure.zero, if the new figure could not be found
		public Figure ChangeFigure(Figure nfig)
		{
			if(Current==Figure.Zero) return Current;
			Figure old = Current;
			EraseFigure(old);
			if(!PlaceFigure(nfig))
				return Figure.Zero;
			return old;
		}
		
		/// <summary>
		/// Поворачивает текущую фигуру по часовой стрелке
        /// rotates the current shape clockwise
		/// </summary>
		/// <returns>true в случае успеха и false - в случае неудачи</returns>
        /// true if successful and false if unsuccessful
		public bool RotateFigure()
		{
			if(Current==Figure.Zero) return false;
			Figure t=RotateFigure(Current);
			if(t!=Figure.Zero)
			{
				Current=t;
				return true;
			}
			return false;
		}
        /// <summary>
        /// Смещает фигуру влево
        /// shifts the figure to the left
        /// </summary>
        /// <returns>true в случае успеха и false - в случае неудачи</returns>
        /// true if successful and false if unsuccessful
        public bool MoveLeft()
		{
			if(Current==Figure.Zero) return false;
			if(MoveLeft(Current))
			{
				Current=Current.MoveLeft();
				return true;
			}
			return false;
		}
        /// <summary>
        /// Смещает фигуру вправо
        /// shift shape to the right
        /// </summary>
        /// <returns>true в случае успеха и false - в случае неудачи</returns>
        /// true if successful and false if unsuccessful
        public bool MoveRight()
		{
			if(Current==Figure.Zero) return false;
			if(MoveRight(Current))
			{
				Current=Current.MoveRight();
				return true;
			}
			return false;
		}
        /// <summary>
        /// Смещает фигуру вниз
        /// shift the shape down
        /// </summary>
        /// <returns>true в случае успеха и false - в случае неудачи</returns>
        /// true if successful and false if unsuccessful
        public bool MoveDown()
		{
			if(Current==Figure.Zero) return false;
			if(MoveDown(Current))
			{
				Current=Current.MoveDown();
				return true;
			}
			return false;
		}
        /// <summary>
        /// Смещает фигуру вниз до предела
        /// shift the shape down to the limit(ground?)
        /// </summary>
        /// <returns>true в случае успеха и false - в случае неудачи</returns>
        /// true if successful and false if unsuccessful
        public bool Drop()
		{
			if(Current==Figure.Zero) return false;
			while(Current!=Figure.Zero)
				DoStep();
			return true;
		}
		
		public void DoStep()
		{
			if(Current!=Figure.Zero)
			{
				IsFigureFalling=MoveDown(Current);	//пытаемся сдвинуть вниз //trying to move down
                                                                            
				if(IsFigureFalling)	//успех? //goodluck?
				{
					Current=Current.MoveDown(); // да, передвигаем указатель // yes we move the pointer down?
				}
				else
					Current=Figure.Zero; //нет, двигать больше некуда, фигура опустилась //no there is nowhere to move, the figure is lowered
			}
			else
				IsFigureFalling=false;
		}
		
		/// <summary>
		/// Очищает игровое поле
        /// clears the play field
		/// </summary>
		public override void Clear()
		{
			base.Clear();
			
			Current=Figure.Zero;
			IsFigureFalling=false;
		}
		
		public override void Paint(Graphics g)
		{
			base.Paint(g);
			
			if(ShowTips && IsFigureFalling)
			{
				Figure tip=Current;
				//временно удаляем текущую фигуру, потом всё вернём
                //temporary delete the current shape, then return everything
				EraseFigure(Current);
				
				while(IsEmpty(tip)) //смещаем ниже, пока не натолкнёмся на препятствие
				{
					tip=tip.MoveDown();
				}
                //натолкнулись, надо фигуру в последнее свободное место (оно выше)
                //come across, you need a figure in the last free space (it is higher) no idea what isit saying
                tip = tip.MoveUp();
				
				//возвращаем, как и обещалось
                //return, as promised
				SetFigure(Current, false);
				
				Point[] cells=new Point[]
				{
					new Point(tip.XC, tip.YC), new Point(tip.X1, tip.Y1),
					new Point(tip.X2, tip.Y2), new Point(tip.X3, tip.Y3)
				};
				
				SolidBrush b=new SolidBrush(Color.FromArgb(32, 192, 0, 0));
				Pen p=new Pen(Color.FromArgb(128, 192, 0, 0), 1);
				
				foreach(Point cell in cells)
				{
					if(!IsEmpty(cell.Y, cell.X)) continue;
					Rectangle tile = new Rectangle(6+cell.X*TileSide, 6+cell.Y*TileSide, TileSide-2, TileSide-2);
					g.FillRectangle(b, tile);
				}
			}
		}
	}
	
	

	public struct Figure
	{
		//ячейки фигуры
        //shape cells
		public int XC { get; private set; }
		public int YC { get; private set; }
		
		public int X1 { get; private set; }
		public int Y1 { get; private set; }
		
		public int X2 { get; private set; }
		public int Y2 { get; private set; }
		
		public int X3 { get; private set; }
		public int Y3 { get; private set; }
		
		public TileType Type;
		
		public static readonly Figure Zero=new Figure(TileType.Empty);
		
		
		public Figure(TileType type):this()
		{
			Type=type;
			XC=0;
			YC=0;
			switch(type)
			{
				//создаём форму фигуры согласно её цвету
                //create a shape according to its colour
				case TileType.Blue: // I
					X1=XC-1; X2=XC+1; X3=XC+2;
					Y1=YC; Y2=YC; Y3=YC;
					break;
				case TileType.LightBlue: // L
					X1=XC-1; X2=XC-1; X3=XC+1;
					Y1=YC+1; Y2=YC; Y3=YC;
					break;
				case TileType.Green: // Z
					X1=XC-1; X2=XC; X3=XC+1;
					Y1=YC; Y2=YC+1; Y3=YC+1;
					break;
				case TileType.Orange: // Г
					X1=XC-1; X2=XC+1; X3=XC+1;
					Y1=YC; Y2=YC; Y3=YC+1;
					break;
				case TileType.Purple: // T
					X1=XC-1; X2=XC; X3=XC+1;
					Y1=YC; Y2=YC+1; Y3=YC;
					break;
				case TileType.Red: // S
					X1=XC-1; X2=XC; X3=XC+1;
					Y1=YC+1; Y2=YC+1; Y3=YC;
					break;
				case TileType.Yellow: // [ ]
					X1=XC+1; X2=XC; X3=XC+1;
					Y1=YC; Y2=YC+1; Y3=YC+1;
					break;
				case TileType.Empty: // zero
					X3=X2=X1=XC=0;
					Y3=Y2=Y1=YC=0;
					break;
				default:
					X3=X2=X1=XC=0;
					Y3=Y2=Y1=YC=0;
					break;
			}
		}
		
		/// <summary>
		/// Смещает фигуру вниз
        /// shifts the shape down
		/// </summary>
		/// <returns>Смещённую фигуру</returns>
        /// biased figure
		public Figure MoveDown()
		{
			return MoveTo(YC+1, XC);
		}
		/// <summary>
		/// Смещает фигуру вверх
        /// shifts the shape up
		/// </summary>
		/// <returns>Смещённую фигуру</returns>
        /// biased figure
		public Figure MoveUp()
		{
			return MoveTo(YC-1, XC);
		}
		
		/// <summary>
		/// Смещает фигуру вправо
        /// shifts the shape to the right
		/// </summary>
		/// <returns>Смещённую фигуру</returns>
        /// biased figure
		public Figure MoveRight()
		{
			return MoveTo(YC, XC+1);
		}
		
		/// <summary>
		/// Смещает фигуру влево
        /// shifts the figure to the left
		/// </summary>
		/// <returns>Смещённую фигуру</returns>
        /// biased figure
		public Figure MoveLeft()
		{
			return MoveTo(YC, XC-1);
		}
		
		/// <summary>
		/// Перемещает фигуру в положение x=col, y=row
        /// moves the shape to x=col, y=row
		/// </summary>
		/// <returns>Перемещённую фигуру</returns>
        /// moved figure
		public Figure MoveTo(int row, int col)
		{
			int dx=col-XC, dy=row-YC;
			Figure res=new Figure(this.Type);
			res.XC=col; res.YC=row;
			res.X1=X1+dx; res.Y1=Y1+dy;
			res.X2=X2+dx; res.Y2=Y2+dy;
			res.X3=X3+dx; res.Y3=Y3+dy;
			return res;
		}
		
//======[ Поворот ]===
//turn

		//формулы для поворота клеток относительно центра
        //formula for rotating cells relative to the center
		//(немного математики)
        //some maths
		private int RotateCol(int col)
		{
			return YC-XC+col;
		}
		private int RotateRow(int row)
		{
			return XC-row+YC;
		}

		/// <summary>
		/// Осуществляет поворот фигуры по часовой стрелке на 90 градусов
        /// turns the shape clockwise 90 degrees
		/// </summary>
		/// <returns>Повёрнутую фигуру</returns>
        /// rotated figure
		public Figure Rotate()
		{
			Figure res=Clone();
			res.X1=RotateRow(Y1); res.Y1=RotateCol(X1);
			res.X2=RotateRow(Y2); res.Y2=RotateCol(X2);
			res.X3=RotateRow(Y3); res.Y3=RotateCol(X3);
			return res;
		}
				
		public static bool operator ==(Figure f1, Figure f2)
		{
			return f1.Type==f2.Type && f1.XC==f2.XC && f1.YC==f2.YC &&
				f1.X1==f2.X1 && f1.X2==f2.X2 && f1.X3==f2.X3 && 
				f1.Y1==f2.Y1 && f1.Y2==f2.Y2 && f1.Y3==f2.Y3;
		}
		public static bool operator !=(Figure f1, Figure f2)
		{
			return !(f1==f2);
		}
		
		private Figure Clone()
		{
			Figure res=new Figure(this.Type);
			res.XC=XC; res.YC=YC; res.X1=X1; res.Y1=Y1;
			res.X2=X2; res.Y2=Y2; res.X3=X3; res.Y3=Y3;
			return res;
		}
		
		
		private static Random rnd=new Random();
		/// <summary>
		/// Возвращает случайную фигуру
        /// returns a random shape
		/// </summary>
		public static Figure RandomFigure()
		{
			return new Figure((TileType)rnd.Next(1, 8));
		}
	}
	
	public enum TileType { Empty, Red, Green, Blue, Yellow, Orange, Purple, LightBlue, Wall }
}
