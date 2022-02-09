using System;
using System.Diagnostics;

namespace Tetris
{
    static class Game
    {
        private static Stopwatch dropTimer = new Stopwatch();
        private static int dropRate = 300;

        private static Tetrominoe tet;
        private static Tetrominoe nexttet;

        static void Main()
        {

            initial_screen();

            init();
            draw();

            while ( !Grid.firstLineIsSomething())
            {
                System.Threading.Thread.Sleep(100);
                tet.clearScreen();
                Update();
                draw();
            }

            gameOver();
        }

        private static void draw()
        {
            Grid.draw();
            Hud.draw();
            tet.draw();
            nexttet.draw();
        }

        private static void initial_screen()
        {
            Console.Clear();
            Console.CursorVisible = false;
            Console.WriteLine("\t\t*** *** *** *** * ***");
            Console.WriteLine("\t\t *  *    *  * * * *  ");
            Console.WriteLine("\t\t *  **   *  *** * ***");
            Console.WriteLine("\t\t *  *    *  **  *   *");
            Console.WriteLine("\t\t *  ***  *  * * * ***");
            Console.WriteLine("\n\n\t\tPress any key");
            Console.ReadKey(true);
            Console.Clear();
        }
        private static void gameOver()
        {
            //string input;
            Console.SetCursorPosition(Grid.nCOL*2, Grid.nROW);
            Console.Write("Game Over");
            Console.ReadKey();
            /*do
            {
                Console.SetCursorPosition(Grid.nCol * 2, Grid.nRow + 1);
                Console.Write("Do you want to replay (YES/NO)?");
                input = Console.ReadLine().ToUpper();

            } while (input != "NO" && input != "YES");

            if (input == "YES")
            {
                Main();
            }*/

        }

        private static void init()
        {
            dropTimer = new Stopwatch();

            dropRate = 300;
            dropTimer.Start();

            tet = new Tetrominoe();
            tet.Spawn();
            nexttet = new Tetrominoe();

            Grid.clear();
        }

        private static void Update()
        {
            bool isDropped = false;
            int dropTime = (int)dropTimer.ElapsedMilliseconds;
            if (dropTime > dropRate)
            {
                dropTimer.Restart();
                isDropped = tet.Drop();
            }
            if (isDropped == true)
            {
                Grid.Locate(tet.GetLocation());
                tet = nexttet;
                nexttet = new Tetrominoe();
                tet.Spawn();
            }

            InputPlayer.Input(tet);

            Hud.ResetCombo();
            for (int i = 0; i < Grid.nROW; i++)
            {
                if (Grid.LineIsFull(i))
                {
                    Grid.RemoveLine(i);
                    Hud.UpLines();
                }
            }
            Hud.updateScore();
        }
    }
}
