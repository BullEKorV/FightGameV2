using System;
using System.Linq;

namespace Fight
{
    class Program
    {
        static Random rnd = new Random();
        static int round;
        static bool versus;
        static string p1Name;
        static string p2Name;
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

                Console.WriteLine("Welcome to fight!");
                Console.WriteLine("Do you want to play agains a Bot or another player? Bot/Player?");
                {
                    string response = Console.ReadLine().ToLower();
                    while (!(response == "bot" || response == "player"))
                    {
                        Console.WriteLine("Please enter Bot/Player");
                        response = Console.ReadLine().ToLower();
                    }
                    if (response == "player") versus = true;
                    else
                    {
                        versus = false;
                        p2Name = "Opponent";
                    }
                }
                //setup names
                Console.WriteLine("Enter Player 1's name");
                p1Name = Console.ReadLine();
                while (p1Name.Length > 12 || p1Name.Length < 3)
                {
                    Console.WriteLine("Name cannot be shorter than 3 characters or longer than 12.");
                    p1Name = Console.ReadLine();
                }
                if (versus)
                {
                    Console.WriteLine("Enter Player 2's name");
                    p2Name = Console.ReadLine();
                    while (p2Name.Length > 12 || p2Name.Length < 3)
                    {
                        Console.WriteLine("Name cannot be shorter than 3 characters or longer than 12.");
                        p2Name = Console.ReadLine();
                    }
                }
                Console.ReadKey();

                while (gameActive)
                {
                    Console.Clear();
                    Console.WriteLine($"Round {round}!");
                    //player 1
                    Console.WriteLine($"{p1Name}'s turn!");
                    //player damage calc
                    int damage = PlayerAttack();
                    p2Health -= damage;

                    if (damage != 0) Console.WriteLine($"\n{p1Name} hit {p2Name} for {damage} damage\n");
                    Console.WriteLine($"{p2Name} health remaining: {p2Health}\n");
                    CheckLose();
                    if (gameActive == false) break;
                    Console.ReadKey();

                    //player 2 or bot
                    Console.WriteLine($"{p2Name}'s turn!");
                    //player damage calc
                    if (versus) damage = PlayerAttack();
                    else damage = BotAttack();
                    p1Health -= damage;

                    if (damage != 0) Console.WriteLine($"\n{p2Name} hit {p1Name} for {damage} damage\n");
                    Console.WriteLine($"{p1Name} health remaining: {p1Health}\n");
                    CheckLose();
                    if (gameActive == false) break;
                    Console.ReadKey();
                    round++;
                }
            }
        }
        static int PlayerAttack()
        {
            int damage;
            Console.WriteLine("What attack do you wanna perform? Heavy/Light?");
            string attackType = Console.ReadLine().ToLower();
            while (!(attackType == "heavy" || attackType == "light"))
            {
                Console.WriteLine("Heavy/Light?");
                attackType = Console.ReadLine().ToLower();
            }
            //light attack
            if (attackType == "light")
            {
                Console.Write("You used a light attack");
                int hitChance = rnd.Next(0, 10);
                if (hitChance < 9)
                {
                    damage = rnd.Next(1, 5);
                    return damage;
                }
                else Console.WriteLine(", but missed!");
            }
            //heavy attack
            else if (attackType == "heavy")
            {
                Console.Write("You used a heavy attack");
                int hitChance = rnd.Next(0, 15);
                if (hitChance < 8)
                {
                    damage = rnd.Next(5, 10);
                    return damage;
                }
                else Console.WriteLine(", but missed!");
            }
            return 0;
        }
        static int BotAttack()
        {
            int damage;
            int attackType = rnd.Next(0, 10);
            //light attack
            if (attackType >= 0 && attackType <= 6)
            {
                Console.Write($"{p2Name} used a light attack");
                int hitChance = rnd.Next(0, 10);
                if (hitChance < 9)
                {
                    damage = rnd.Next(1, 5);
                    return damage;
                }
                else Console.WriteLine(", but missed!");
            }
            //heavy attack
            else if (attackType >= 7 && attackType <= 9)
            {
                Console.Write($"{p2Name} used a heavy attack");
                int hitChance = rnd.Next(0, 15);
                if (hitChance < 8)
                {
                    damage = rnd.Next(5, 10);
                    return damage;
                }
                else Console.WriteLine(", but missed!");
            }
            return 0;
        }

        static bool CheckHealth(int x)
        {
            if (x <= 0)
            {
                return true;
            }
            else return false;
        }
        static void CheckLose()
        {
            if (CheckHealth(p1Health) || CheckHealth(p2Health))
            {
                if (CheckHealth(p1Health) && CheckHealth(p2Health)) Console.WriteLine("Both lost");
                else if (CheckHealth(p1Health)) Console.WriteLine($"{p1Name} lost");
                else if (CheckHealth(p2Health)) Console.WriteLine($"{p2Name} lost");
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