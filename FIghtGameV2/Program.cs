using System;
using System.Linq;

namespace Fight
{
    class Program
    {
        static Random rnd = new Random();
        static int round;
        static int p1Health;
        static int p2Health;
        static bool endApp = false;
        static bool gameActive = true;

        public static void Main(string[] args)
        {
            while (!endApp)
            {
                //reset game data
                round = 1;
                p1Health = 50;
                p2Health = 50;
                gameActive = true;

                Console.WriteLine("Welcome to fight. Press any key to continue");
                Console.ReadKey();

                while (gameActive)
                {
                    Console.WriteLine($"Round {round}!");
                    //player 1
                    int damage = attack();
                    p1Health -= damage;
                    Console.WriteLine($"Player 2 hit Player 1 for {damage} damage\n");
                    Console.WriteLine($"Player 1 health remaining: {p1Health}\n");
                    checkLose();
                    if (gameActive == false) break;
                    Console.ReadKey();

                    //player 2
                    damage = attack();
                    p2Health -= damage;
                    Console.WriteLine($"Player 1 hit Player 2 for {damage} damage\n");
                    Console.WriteLine($"Player 2 health remaining: {p2Health}\n");
                    checkLose();
                    if (gameActive == false) break;
                    Console.ReadKey();
                    round++;
                }
            }
        }
        static int attack()
        {
            int damage = rnd.Next(1, 10);
            return damage;
        }

        static bool checkHealth(int x)
        {
            if (x <= 0)
            {
                return true;
            }
            else return false;
        }
        static void checkLose()
        {
            if (checkHealth(p1Health) || checkHealth(p2Health))
            {
                if (checkHealth(p1Health) && checkHealth(p2Health)) Console.WriteLine("Both lost");
                else if (checkHealth(p1Health)) Console.WriteLine("Player 1 lost");
                else if (checkHealth(p2Health)) Console.WriteLine("Player 2 lost");
                Console.WriteLine("Do you want to play again? Y/N?");
                string response = Console.ReadLine().ToLower();
                while (!(response == "y" || response == "n"))
                {
                    Console.WriteLine("Please enter Y/N");
                    response = Console.ReadLine().ToLower();
                }
                if (response == "n") endApp = true;
                gameActive = false;
            }
        }
    }
}