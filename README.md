# Maze_Games
Игра на C# (C Sharp) лабиринт, ручная и автоматическая генерация лабиринта, алгоритм «Глубокий поиск с возвратом» (Depth-First Search, DFS).


1) Ручной лабиринт. File - Game_labyrinth

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Game_labyrinth
{
	public partial class Form_Maze_Game : Form
	{

		private const int CellSize = 40; // Размер клетки
		private const int Rows = 11; // Количество строк
		private const int Cols = 11; // Количество столбцов

		private char[,] maze; // Лабиринт

		private int playerX; // Начальная позиция игрока по X
		private int playerY; // Начальная позиция игрока по Y
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

			GenerateMaze(); // Генерация лабиринта
			playerX = 1; // Начальная позиция игрока по X
			playerY = 1; // Начальная позиция игрока по Y

		}
		//  Метод, который создает лабиринт с помощью алгоритма глубокого поиска.
		//  Он инициализирует все клетки как стены и затем постепенно открывает их, пока не будет построен лабиринт.
		// Maze Structure: Лабиринт представлен как двумерный массив,
		// где # — это стена, а ' ' — проходимая клетка.
		// В конце генерации устанавливается выход 'E'.
		private void GenerateMaze()
		{
			maze = new char[Rows, Cols];

			// Инициализация лабиринта стенами
			for (int y = 0; y < Rows; y++)
				for (int x = 0; x < Cols; x++)
					maze[y, x] = '#';

			// Генерация лабиринта
			Random rand = new Random();
			Stack<Point> stack = new Stack<Point>();
			int startX = 1, startY = 1;
			maze[startY, startX] = ' '; // Начальная точка

			stack.Push( new Point( startX, startY ) );

			while (stack.Count > 0)
			{
				Point current = stack.Pop();
				List<Point> neighbors = new List<Point>();

				// Проверка соседей
				foreach (Point offset in new Point[] { new Point( 2, 0 ), new Point( -2, 0 ), new Point( 0, 2 ), new Point( 0, -2 ) })
				{
					int nx = current.X + offset.X;
					int ny = current.Y + offset.Y;

					if (nx > 0 && nx < Cols && ny > 0 && ny < Rows && maze[ny, nx] == '#')
					{
						neighbors.Add( new Point( nx, ny ) );
					}
				}

				if (neighbors.Count > 0)
				{
					stack.Push( current );

					// Выбор случайного соседа
					Point next = neighbors[rand.Next( neighbors.Count )];
					maze[current.Y + (next.Y - current.Y) / 2, current.X + (next.X - current.X) / 2] = ' ';
					maze[next.Y, next.X] = ' ';
					stack.Push( next );
				}
			}

			// Установка выхода
			maze[Rows - 2, Cols - 2] = 'E'; // Выход из лабиринта
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
		//  Игрок управляется с помощью клавиш W, A, S, D.
		//  Если игрок достигает выхода, появляется сообщение о выигрыше.
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
				Application.Exit(); // Закрываем приложение
			}

			Invalidate(); // Перерисовываем форму
		}

	}
}

=================================================================================
3) Алгоритм «Глубокий поиск с возвратом» (Depth-First Search, DFS). File - Game_labyrinth_DFS_algorithm

using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Game_labyrinth
{
	public partial class Form_Maze_Game : Form
	{

		private const int CellSize = 40; // Размер клетки
		private const int Rows = 11; // Количество строк
		private const int Cols = 11; // Количество столбцов

		private char[,] maze; // Лабиринт

		private int playerX; // Начальная позиция игрока по X
		private int playerY; // Начальная позиция игрока по Y
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

			GenerateMaze(); // Генерация лабиринта
			playerX = 1; // Начальная позиция игрока по X
			playerY = 1; // Начальная позиция игрока по Y

		}
		//  Метод, который создает лабиринт с помощью алгоритма глубокого поиска.
		//  Он инициализирует все клетки как стены и затем постепенно открывает их, пока не будет построен лабиринт.
		// Maze Structure: Лабиринт представлен как двумерный массив,
		// где # — это стена, а ' ' — проходимая клетка.
		// В конце генерации устанавливается выход 'E'.
		private void GenerateMaze()
		{
			maze = new char[Rows, Cols];

			// Инициализация лабиринта стенами
			for (int y = 0; y < Rows; y++)
				for (int x = 0; x < Cols; x++)
					maze[y, x] = '#';

			// Генерация лабиринта
			Random rand = new Random();
			Stack<Point> stack = new Stack<Point>();
			int startX = 1, startY = 1;
			maze[startY, startX] = ' '; // Начальная точка

			stack.Push( new Point( startX, startY ) );

			while (stack.Count > 0)
			{
				Point current = stack.Pop();
				List<Point> neighbors = new List<Point>();

				// Проверка соседей
				foreach (Point offset in new Point[] { new Point( 2, 0 ), new Point( -2, 0 ), new Point( 0, 2 ), new Point( 0, -2 ) })
				{
					int nx = current.X + offset.X;
					int ny = current.Y + offset.Y;

					if (nx > 0 && nx < Cols && ny > 0 && ny < Rows && maze[ny, nx] == '#')
					{
						neighbors.Add( new Point( nx, ny ) );
					}
				}

				if (neighbors.Count > 0)
				{
					stack.Push( current );

					// Выбор случайного соседа
					Point next = neighbors[rand.Next( neighbors.Count )];
					maze[current.Y + (next.Y - current.Y) / 2, current.X + (next.X - current.X) / 2] = ' ';
					maze[next.Y, next.X] = ' ';
					stack.Push( next );
				}
			}

			// Установка выхода
			maze[Rows - 2, Cols - 2] = 'E'; // Выход из лабиринта
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
		//  Игрок управляется с помощью клавиш W, A, S, D.
		//  Если игрок достигает выхода, появляется сообщение о выигрыше.
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
				Application.Exit(); // Закрываем приложение
			}

			Invalidate(); // Перерисовываем форму
		}

	}
}
