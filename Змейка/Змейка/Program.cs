using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace SnakeGame
{
    class Game
    {
        static bool Gameover = false;
        static int x = -10;
        static int y;
        static int speed;
        static int complexity;
        static Walls walls;
        static Snake snake;
        static void Main()
        {
            while (x < 0)
            {
                Console.WriteLine("Какую сложность игры хотите (1-Низкая,2-Средняя,3-Высокая)");
                complexity = Convert.ToInt32(Console.ReadLine());
                if (complexity == 1)
                {
                    x = 40;
                    y = 20;
                    speed = 300;
                    Console.Clear();
                    break;
                }
                else if (complexity == 2)
                {
                    speed = 200;
                    x = 50;
                    y = 30;
                    Console.Clear();
                    break;

                }
                else if (complexity == 3)
                {
                    speed = 100;
                    x = 50;
                    y = 50;
                    Console.Clear();
                    break;
                }
                else
                {
                    Console.WriteLine("Введите правильную сложность!");
                    x = -10;
                }
            } //Сложность игры
            Console.SetWindowSize(x + 1, y + 1);
            Console.SetBufferSize(x + 1, y + 1);
            Console.CursorVisible = false;
            walls = new Walls();
            walls.Wallsxy(x, y);
            snake = new Snake(x, y);
            Game game = new Game();
            ConsoleKeyInfo button = Console.ReadKey(true);
            ConsoleKey Cashbutton = ConsoleKey.V;
            while (Gameover == false)
            {

                if (Console.KeyAvailable)
                {
                    button = Console.ReadKey(true);
                    Cashbutton = button.Key;
                }
                else
                {
                    snake.Runsnake(button.Key, speed, ref Gameover);
                    if (walls.checkWalls(snake.GetHead()) == true)
                    {
                        Console.SetCursorPosition(x / 2 - 15, y / 2);
                        Console.WriteLine("Геймовер епта!");
                        Console.SetCursorPosition(x / 2 - 15, y / 2 - 1);
                        Console.WriteLine("Твоя змейка не такая уж и большая");
                        Console.SetCursorPosition(x / 2 - 15, y / 2 - 2);
                        Console.WriteLine($"{snake.shakeLeng()} счёт");
                        Gameover = true;
                    }
                    if (snake.checksnake() == true)
                    {
                        Console.SetCursorPosition(x / 2 - 15, y / 2);
                        Console.WriteLine("Геймовер епта!");
                        Console.SetCursorPosition(x / 2 - 15, y / 2 - 1);
                        Console.WriteLine("Твоя змейка не такая уж и большая");
                        Console.SetCursorPosition(x / 2 - 15, y / 2 - 2);
                        Console.WriteLine($"{snake.shakeLeng()} счёт");
                        Gameover = true;
                    }
                }
            }
            Console.ReadKey();
        }

    }
    struct Point
    {
        public int x { get; set; }
        public int y { get; set; }

        public char ch { get; set; }
        public static bool operator ==(Point a, Point b) => (a.x == b.x && a.y == b.y);
        public static bool operator !=(Point a, Point b) => (a.x != b.x || a.y != b.y);

        public static implicit operator Point((int, int, char ch) value) => new Point { x = value.Item1, y = value.Item2, ch = value.Item3 };
        public void Draw()
        {
            DrawPoint(ch);
        }
        public void Clear()
        {
            DrawPoint(' ');
        }
        private void DrawPoint(char _ch)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(_ch);
        }
    }
    class Walls
    {
        List<Point> wall = new List<Point>();
        public void Wallsxy(int x, int y)
        {
            DrawHorizontal(x, 0);
            DrawHorizontal(x, y);
            DrawVertical(0, y);
            DrawVertical(x, y);
        }
        private void DrawHorizontal(int x, int y)
        {
            for (int i = 0; i < x; ++i)
            {
                Console.SetCursorPosition(i, y);
                Point p = (i, y, '#');
                wall.Add(p);
                p.Draw();
            }

        }
        private void DrawVertical(int x, int y)
        {
            for (int i = 0; i < y; ++i)
            {
                Console.SetCursorPosition(x, i);
                Point p = (x, i, '#');
                wall.Add(p);
                p.Draw();
            }
        }
        public bool checkWalls(Point snake)
        {
            foreach (var v in wall)
            {
                if (v == snake)
                {
                    return true;
                }
            }
            return false;

        }

    }
    class FoodFactory
    {
        int x;
        int y;
        char ch;
        public void Variable(int x, int y, char ch)
        {
            this.x = x;
            this.y = y;
            this.ch = ch;
        }
        Point food;
        Random rnd = new Random();
        public Point CreateFood()
        {
            food = (rnd.Next(2, x - 2), rnd.Next(2, y - 2), ch);
            food.Draw();
            return food;
        }
        public bool Foodeat(Point p)
        {
            if (p == food)
            {
                return true;
            }
            return false;
        }
    }
    class Snake
    {
        FoodFactory factory = new FoodFactory();
        List<Point> snake = new List<Point>();
        public Point GetHead() => snake.Last();
        public int shakeLeng() => snake.Count();

        public Snake(int x, int y)
        {
            factory.Variable(x, y, '$');
            beginshake(x, y);
            factory.CreateFood();
        }
        private void beginshake(int x, int y)
        {
            Point p = (x / 2, y / 2, '*');
            snake.Add(p);
            p.Draw();

        }
        public void Runsnake(ConsoleKey button, int speed, ref bool Gameover)
        {

            if (button == ConsoleKey.W)
            {

                Thread.Sleep(speed);
                Point HeadSnake = (snake.Last());
                HeadSnake.y -= 1;
                snake.Add(HeadSnake);
                HeadSnake.Draw();
                if (factory.Foodeat(HeadSnake) == true)
                {
                    CreateFood();
                }
                else
                {
                    HeadSnake = (snake.First());
                    snake.Remove(HeadSnake);
                    HeadSnake.Clear();
                }


            }
            if (button == ConsoleKey.S)
            {
                Thread.Sleep(speed);
                Point p = (snake.Last());
                p.y += 1;
                snake.Add(p);
                p.Draw();
                if (factory.Foodeat(p) == true)
                {
                    CreateFood();
                }
                else
                {
                    p = (snake.First());
                    snake.Remove(p);
                    p.Clear();
                }
            }
            if (button == ConsoleKey.D)
            {
                Thread.Sleep(speed);
                Point p = (snake.Last());
                p.x += 1;
                snake.Add(p);
                p.Draw();
                if (factory.Foodeat(p) == true)
                {
                    CreateFood();

                }
                else
                {
                    p = (snake.First());
                    snake.Remove(p);
                    p.Clear();
                }
            }
            if (button == ConsoleKey.A)
            {
                Thread.Sleep(speed);
                Point p = (snake.Last());
                p.x -= 1;
                snake.Add(p);
                p.Draw();
                if (factory.Foodeat(p) == true)
                {
                    CreateFood();

                }
                else
                {
                    p = (snake.First());
                    snake.Remove(p);
                    p.Clear();
                }
            }
        }
        public void CreateFood()
        {
            factory.CreateFood();
        }
        public bool checksnake()
        {
            for (int i = 0; i < snake.Count - 1; i++)
            {
                if (snake.Last() == snake[i])
                {
                    return true;
                }
            }
            return false;
        }
    }

}