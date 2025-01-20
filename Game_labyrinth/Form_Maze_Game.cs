using System;
using System.Drawing;
using System.Windows.Forms;

namespace Game_labyrinth
{
	public partial class Form_Maze_Game : Form
	{
		//public Form_Maze_Game()
		//{
		//	InitializeComponent();
		//}

		private void Form_Maze_Game_Load( object sender, EventArgs e )
		{

		}

		private const int CellSize = 40; // Размер клетки
		private const int Rows = 10; // Количество строк
		private const int Cols = 10; // Количество столбцов
		// E - Exit / ВЫХОД
		// '#' - Стена
		// ' ' - Проход

		// Ручной (самодельный) лабиринт

		private char[,] maze = {
			{ '#', '#', '#', '#', '#', '#', '#', '#', '#', '#' },
			{ '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#' },
			{ '#', ' ', '#', '#', ' ', '#', '#', ' ', '#', '#' },
			{ '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#' },
			{ '#', ' ', '#', '#', '#', '#', '#', ' ', ' ', '#' },
			{ '#', ' ', '#', '#', '#', '#', '#', ' ', ' ', '#' },
			{ '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#' },
			{ '#', ' ', '#', '#', '#', ' ', '#', '#', '#', '#' },
			{ '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'E', '#' },
			{ '#', '#', '#', '#', '#', '#', '#', '#', '#', '#' }

		};

		private int playerX = 1; // Начальная позиция игрока по X
		private int playerY = 1; // Начальная позиция игрока по Y
		private Label winLabel; // Элемент для отображения сообщения о выигрыше

		public Form_Maze_Game()
		{
			this.Text = "Maze Game";
			this.ClientSize = new Size( Cols * CellSize, Rows * CellSize );
			this.DoubleBuffered = true;
			this.KeyDown += new KeyEventHandler( OnKeyDown );

			// Инициализация элемента для сообщения о выигрыше
			winLabel = new Label();
			winLabel.Text = "Вы выиграли!";
			winLabel.Location = new Point( 10, 10 ); // Положение текста
			winLabel.AutoSize = true;
			winLabel.Visible = false; // Скрываем текст до победы
			this.Controls.Add( winLabel );
		}

		protected override void OnPaint( PaintEventArgs e )
		{
			base.OnPaint( e );
			DrawMaze( e.Graphics );
		}

		private void DrawMaze( Graphics g )
		{
			for (int y = 0; y < Rows; y++)
			{
				for (int x = 0; x < Cols; x++)
				{
					if (maze[y, x] == '#')
					{
						g.FillRectangle( Brushes.Black, x * CellSize, y * CellSize, CellSize, CellSize );
					}
					else if (maze[y, x] == 'E')
					{
						g.FillRectangle( Brushes.Green, x * CellSize, y * CellSize, CellSize, CellSize );
					}
				}
			}
			g.FillRectangle( Brushes.Blue, playerX * CellSize, playerY * CellSize, CellSize, CellSize );
		}

		private void OnKeyDown( object sender, KeyEventArgs e )
		{
			int newX = playerX;
			int newY = playerY;

			switch (e.KeyCode)
			{
				case Keys.W:
				newY--;
				break;
				case Keys.S:
				newY++;
				break;
				case Keys.A:
				newX--;
				break;
				case Keys.D:
				newX++;
				break;
			}

			if (maze[newY, newX] != '#') // Проверка на столкновение со стеной
			{
				playerX = newX;
				playerY = newY;
			}

			if (maze[playerY, playerX] == 'E') // Проверка на выход
			{
				winLabel.Visible = true; // Показываем текст победы
				MessageBox.Show( "Вы выиграли!" ); // Дополнительное сообщение
				//Application.Exit(); // Закрываем приложение
			}

			Invalidate(); // Перерисовываем форму
		}

	}
}