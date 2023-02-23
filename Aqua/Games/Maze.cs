using Cosmos.Debug.Kernel;
using System;
using System.Collections.Generic;
using static System.Console;

namespace Aqua.Games
{
    public class Player
    {
        public int x { get; set; }
        public int y { get; set; }

        private string playerMarker;
        private ConsoleColor playerColor;

        public int wins { get; set; } = 0;
        public int coins { get; set; } = 0;

        public Player(int x, int y, string playerMarker, ConsoleColor playerColor)
        {
            this.x = x;
            this.y = y;
            this.playerMarker = playerMarker;
            this.playerColor = playerColor;
        }

        public void Draw()
        {
            ForegroundColor = playerColor;
            SetCursorPosition(x, y);

            Write(playerMarker);
        }

        public void Movement(Player player, int mode)
        {
            SetCursorPosition(player.x, player.y);
            Write(' ');
            // Beep(1500, 75);
            switch (mode)
            {
                case 0:
                    player.x--;
                    break;

                case 1:
                    player.x++;
                    break;

                case 2:
                    player.y--;
                    break;

                case 3:
                    player.y++;
                    break;
            }

            Maze.enemy.Move();
        }
    }

    public class Enemy
    {
        public int x { get; set; }
        public int y { get; set; }

        public Enemy(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void Draw()
        {
            ForegroundColor = ConsoleColor.Red;
            SetCursorPosition(x, y);

            Write("0");
        }

        public void Move()
        {
            Random random = new Random();
            int direction = random.Next(0, 4);

            SetCursorPosition(x, y);
            Write(' ');

            switch (direction)
            {
                case 0:
                    if (Maze.world.CheckEnemyCollisions(x-1, y)) x--;
                    break;
                case 1:
                    if (Maze.world.CheckEnemyCollisions(x+1, y)) x++;
                    break;
                case 2:
                    if (Maze.world.CheckEnemyCollisions(x, y-1)) y--;
                    break;
                case 3:
                    if (Maze.world.CheckEnemyCollisions(x, y+1)) y++;
                    break;
            }
        }
    }

    public class World
    {
        private List<List<string>> grid = new List<List<string>>() { };
        private int rows, cols;

        public World(int rows, int cols)
        {
            this.rows = rows;
            this.cols = cols;

            this.grid = new List<List<string>>();
            for (int i = 0; i < this.rows; i++)
            {
                List<string> row = new List<string>();
                for (int j = 0; j < this.cols; j++)
                {
                    row.Add("");
                }
                this.grid.Add(row);
            }
        }

        static int posX, posY;
        public List<List<string>> Draw()
        {
            Random random = new Random();

            for (int y = 0; y < this.rows; y++)
            {
                for (int x = 0; x < this.cols; x++)
                {
                    if (y == 0 || y == this.rows - 1 || x == 0 || x == this.cols - 1)
                    {
                        ForegroundColor = ConsoleColor.Gray;
                        grid[y][x] = "#";

                        ResetColor();
                    }
                    else
                    {
                        int value = random.Next(12);
                        if (value == 0)
                        {
                            grid[y][x] = "|";
                            ForegroundColor = ConsoleColor.Gray;
                        }
                        else if (value == 1)
                        {
                            grid[y][x] = "-";
                            ForegroundColor = ConsoleColor.Gray;
                        }
                        else if (value >= 3 && value <= 4)
                        {
                            grid[y][x] = ".";
                            ForegroundColor = ConsoleColor.Yellow;
                        }
                        else
                        {
                            grid[y][x] = " ";
                            ForegroundColor = ConsoleColor.White;
                        }
                    }
                    Write(grid[y][x]);
                }
                WriteLine();
            }

            posX = random.Next(2, rows - 2);
            posY = random.Next(2, cols - 2);

            return grid;
        }

        public void Update()
        {
            grid[posY][this.rows - 2] = "X";

            ForegroundColor = ConsoleColor.Green;
            SetCursorPosition(this.rows - 2, posY);
            Write(grid[posY][this.rows - 2]);

            ResetColor();
        }

        public bool CheckPlayerCollisions(int x, int y, Player player)
        {
            if (x == 0 || x == this.cols - 1 || y == 0 || y == this.rows - 1)
                return false;

            if (grid[y][x] == ".")
            {
                player.coins++;
                grid[y][x] = " ";
                Beep(2500, 50);
            }
            else if (grid[y][x] == "X" && Maze.rand <= player.coins)
            {
                player.wins++;
                Maze.Regenerate(player, Maze.world);
            }
            else if (grid[y][x] == "X" && Maze.rand > player.coins)
                return false;
            else if (x == Maze.enemy.x && y == Maze.enemy.y)
                return false;

            return grid[y][x] == " ";
        }

        public bool CheckEnemyCollisions(int x, int y)
        {
            if (x == 0 || x == this.cols - 1 || y == 0 || y == this.rows - 1)
                return false;

            return grid[y][x] == " ";
        }
    }

    public class Maze
    {
        static int width = 48, height = 48;
        static string symbol = "O";
        static bool running = true;

        static Random random = new Random();
        public static int rand = random.Next(25, 75);

        public static World world = new World(width, height);
        static Player player = new Player(1, 1, symbol, ConsoleColor.Cyan);
        public static Enemy enemy;

        static List<List<string>> drawnWorld = world.Draw();
        static int posMode;

        public static void Draw(World world, Player player, List<List<string>> drawnWorld)
        {
            var oldCurPos = GetCursorPosition();

            player.Draw();
            enemy.Draw(); 
            world.Update();

            Instructions();

            SetCursorPosition(oldCurPos.Left, oldCurPos.Top);
            ResetColor();
            CursorVisible = false;
        }

        private static void Instructions()
        {
            // Required coin amount indicator.
            SetCursorPosition(0, WindowHeight - 8);
            ForegroundColor = ConsoleColor.Cyan;
            Write("You need ");

            ForegroundColor = ConsoleColor.White;
            if ((rand - player.coins) > 0)
                Write($"{rand - player.coins} ");
            else if ((rand - player.coins) <= 0)
                Write("0 ");

            ForegroundColor = ConsoleColor.Cyan;
            Write("coins to go to the next level.");

            // Copyright and controls stuff.
            SetCursorPosition(0, WindowHeight - 6);
            ForegroundColor = ConsoleColor.Cyan;
            Write("A-Mazed | for the Aqua System | (C) 2023");

            SetCursorPosition(0, WindowHeight - 5);
            ForegroundColor = ConsoleColor.Gray;
            Write("Press the \"");

            ForegroundColor = ConsoleColor.White;
            Write("R");

            ForegroundColor = ConsoleColor.Gray;
            Write("\" key if you want to reset your maze progress.");

            SetCursorPosition(0, WindowHeight - 4);
            ForegroundColor = ConsoleColor.Gray;
            Write("Press the \"");

            ForegroundColor = ConsoleColor.White;
            Write("Escape");

            ForegroundColor = ConsoleColor.Gray;
            Write("\" key if you want to quit the game.");

            // HUD
            SetCursorPosition(0, WindowHeight - 2);
            ForegroundColor = ConsoleColor.Yellow;
            Write("Coins : ");

            ForegroundColor = ConsoleColor.White;
            Write(player.coins);

            SetCursorPosition(0, WindowHeight - 1);
            ForegroundColor = ConsoleColor.Green;
            Write("Wins : ");

            ForegroundColor = ConsoleColor.White;
            Write(player.wins);
        }

        public static void HandleInput(Player player, World world)
        {
            ConsoleKey input = ReadKey(true).Key;
            switch (input)
            {
                case ConsoleKey.LeftArrow:
                    if (world.CheckPlayerCollisions(player.x - 1, player.y, player))
                        player.Movement(player, 0);
                    else
                        Beep(100, 75);
                    break;

                case ConsoleKey.RightArrow:
                    if (world.CheckPlayerCollisions(player.x + 1, player.y, player))
                        player.Movement(player, 1);
                    else
                        Beep(100, 75);
                    break;

                case ConsoleKey.UpArrow:
                    if (world.CheckPlayerCollisions(player.x, player.y - 1, player))
                        player.Movement(player, 2);
                    else
                        Beep(100, 75);
                    break;

                case ConsoleKey.DownArrow:
                    if (world.CheckPlayerCollisions(player.x, player.y + 1, player))
                        player.Movement(player, 3);
                    else
                        Beep(100, 75);
                    break;

                case ConsoleKey.R:
                    Regenerate(player, world);
                    break;

                case ConsoleKey.Escape:
                    CursorVisible = true;
                    CursorSize = 50;
                    running = false;
                    Clear();
                    break;

                default:
                    break;
            }
        }

        public static void Regenerate(Player player, World world)
        {
            Clear();
            player.x = 1;
            player.y = 1;
            player.coins = 0;
            rand = random.Next(25, 75);

            posMode = random.Next(0, 2);
            switch (posMode)
            {
                case 0:
                    enemy = new(width - 2, height - 2);
                    break;

                case 1:
                    enemy = new(2, height - 2);
                    break;

                case 2:
                    enemy = new(width - 2, 2);
                    break;
            }

            drawnWorld = world.Draw();
        }

        public static void Game(int difficulty)
        {
            Random random = new Random();
            Clear();

            posMode = random.Next(0, 2);
            switch (posMode)
            {
                case 0:
                    enemy = new(width - 2, height - 2);
                    break;

                case 1:
                    enemy = new(2, height - 2);
                    break;

                case 2:
                    enemy = new(width - 2, 2);
                    break;
            }

            while (running)
            {
                Draw(world, player, drawnWorld);
                HandleInput(player, world);
            }
        }
    }
}
