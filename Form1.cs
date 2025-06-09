using System.Diagnostics;

namespace ProjekatSkola
{
    public partial class Form1 : Form
    {
        private const int M = 16;
        private const int N = 16;
        private const int MINES = 40;
        private const int cellSize = 30;

        private bool[,] BOARD = new bool[M, N];
        private bool[,] TEMPBOARD = new bool[M + 2, N + 2];
        private int[,] PRE = new int[M, N];
        private bool[,] moves = new bool[M, N];

        public Form1()
        {
            InitializeComponent();
            InitBoard();
            //this.Width = 600;
            //this.Height = 600;
            this.ClientSize = new Size(cellSize * M, cellSize * N);
        }

        private void InitBoard()
        {
            Random rand = new Random();
            List<int> a = new List<int>();
            for (int i = 0; i < M * N; i++) 
                    a.Add(i);

            for (int c = 0; c < MINES; c++)
            {
                int pos = rand.Next(a.Count - c);
                int row = a[pos] / N;
                int col = a[pos] % N;
                BOARD[row, col] = true;
                (a[pos], a[a.Count - 1 - c]) = (a[a.Count - 1 - c], a[pos]);
            }
            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    TEMPBOARD[i + 1, j + 1] = BOARD[i, j];
                }
            }
            for (int i = 1; i <= M; i++)
            {
                for (int j = 1; j <= N; j++)
                {
                    if (TEMPBOARD[i, j]) PRE[i - 1, j - 1] = 8;
                    else
                    {
                        int ar = 0;
                        if (TEMPBOARD[i + 1, j]) ar++;
                        if (TEMPBOARD[i - 1, j]) ar++;
                        if (TEMPBOARD[i, j - 1]) ar++;
                        if (TEMPBOARD[i, j + 1]) ar++;
                        if (TEMPBOARD[i + 1, j + 1]) ar++;
                        if (TEMPBOARD[i - 1, j - 1]) ar++;
                        if (TEMPBOARD[i + 1, j - 1]) ar++;
                        if (TEMPBOARD[i - 1, j + 1]) ar++;
                        PRE[i - 1, j - 1] = ar;
                    }
                }
            }
        }

        private void RevealDFS(int r, int c)
        {
            if (r < 0 || r >= M || c < 0 || c >= N || moves[r, c])
            {
                return;
            }
            moves[r, c] = true;
            if (PRE[r, c] == 0)
            {
                for (int dr = -1; dr <= 1; dr++)
                {
                    for (int dc = -1; dc <= 1; dc++)
                    {
                        if (dr == 0 && dc == 0)
                        {
                            continue;
                        }
                        RevealDFS(r + dr, c + dc);
                    }
                }
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            for (int r = 0; r < M; r++)
            {
                for (int c = 0; c < N; c++)
                {
                    int x = c * cellSize;
                    int y = r * cellSize;
                    g.DrawRectangle(Pens.Black, x, y, cellSize, cellSize);
                    if (moves[r, c] && PRE[r, c] == 8)
                    {
                        Application.Exit();
                    }
                    else if (moves[r, c])
                    {
                        g.DrawString(PRE[r, c].ToString(), this.Font, Brushes.Black, x + 10, y + 8);
                    }
                }
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            int x = e.X / cellSize;
            int y = e.Y / cellSize;
            if (!moves[y, x])
            {
                RevealDFS(y, x);
                Invalidate();
            }
        }
    }
}
