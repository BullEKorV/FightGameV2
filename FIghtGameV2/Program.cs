using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Fight
{
    class Program
    {
        static List<string> attackTypes = new List<string>() { "light", "heavy", "magic", "heal", "stun" };
        static Random rnd = new Random();
        static bool versus;
        static bool player1Turn;
        static string playerTurn;
        static string p1Name;
        static string p2Name;
        static int maxHealthPlayers;
        static float p1Health;
        static float p2Health;
        static float inflictedDamage;
        static float selfDamage;
        static bool stunnedSelf;
        static bool stunnedOpponent;
        static bool p1Stunned;
        static bool p2Stunned;
        static bool endApp = false;
        static bool gameActive = true;

        public static void Main(string[] args)
        {
            while (!endApp)
            {
                //setup the game settings
                Setup();

                //reset game data
                player1Turn = true;
                p1Health = maxHealthPlayers;
                p2Health = maxHealthPlayers;
                gameActive = true;


                while (gameActive)
                {
                    //StatusBar();
                    GameLogic();
                }
            }
        }

        static void StatusBar()
        {
            int x = Console.CursorLeft;
            int y = Console.CursorTop;
            Console.CursorTop = Console.WindowTop;
            Console.CursorLeft = Console.WindowLeft + (Console.WindowWidth / 7) * 2;
            Console.Write($"{p1Name}'s health: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(p1Health + " ");
            Console.ResetColor();
            Console.CursorLeft += 10;
            Console.Write($"{p2Name}'s health: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(p2Health + " ");
            Console.ResetColor();
            // Restore previous position
            Console.SetCursorPosition(x, y + 2);
        }
        static void GameLogic()
        {
            Console.Clear();
            //player 1
            if (player1Turn)
            {
                StatusBar();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(p1Name);
                Console.ResetColor();
                Console.WriteLine("'s turn!\n");
                //player attack script
                if (!p1Stunned) PlayerAttack();
                //stun script
                p1Stunned = false;
                if (stunnedSelf) p1Stunned = true;
                else if (stunnedOpponent) p2Stunned = true;
                //player calculate health
                p2Health -= inflictedDamage;
                p1Health += selfDamage;
                StatusBar();

                CheckLose();
                Console.Write("\nContinue...");
                stunnedOpponent = false;
                stunnedSelf = false;
            }
            //player 2 or bot
            else if (!player1Turn)
            {
                StatusBar();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(p2Name);
                Console.ResetColor();
                Console.WriteLine("'s turn!\n");
                //player attack script
                if (!p2Stunned)
                {
                    if (versus) PlayerAttack();
                    else BotAttack();
                }
                p2Stunned = false;
                if (stunnedSelf) p1Stunned = true;
                else if (stunnedOpponent) p2Stunned = true;
                p1Health -= inflictedDamage;
                p2Health += selfDamage;
                StatusBar();

                CheckLose();
                Console.Write("\nContinue...");
                stunnedOpponent = false;
                stunnedSelf = false;
            }
        }
        static void PlayerAttack()
        {
            inflictedDamage = 0;
            selfDamage = 0;
            Console.WriteLine("What attack do you wanna perform?");
            Console.Write("Available attacks: ");
            //choose what attack
            foreach (string i in attackTypes) Console.Write($"{i}, ");
            Console.WriteLine("");
            string attackType = Console.ReadLine().ToLower();
            while (!attackTypes.Contains(attackType))
            {
                Console.Write("Available attacks: ");
                foreach (string i in attackTypes) Console.Write($"{i}, ");
                Console.WriteLine("");
                attackType = Console.ReadLine().ToLower();
            }
            Console.WriteLine("");
            switch (attackType)
            {
                //light attack
                case "light":
                    int hitChance = rnd.Next(0, 10);
                    if (hitChance < 9)
                    {
                        inflictedDamage = rnd.Next(3, 6);
                        int critChance = rnd.Next(0, 5);
                        if (critChance == 0)
                        {
                            inflictedDamage *= 2;
                            Console.Write("Your light attack hit dealing ");
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write(inflictedDamage);
                            Console.ResetColor();
                            Console.WriteLine(" in critical damage!");
                        }
                        else
                        {
                            Console.Write("Your light attack dealt ");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(inflictedDamage);
                            Console.ResetColor();
                            Console.WriteLine(" damage");
                        }
                    }
                    else Console.WriteLine("Your attack missed!");
                    break;
                case "heavy":
                    //heavy attack
                    hitChance = rnd.Next(0, 16);
                    if (hitChance < 8)
                    {
                        inflictedDamage = rnd.Next(7, 10);
                        int critChance = rnd.Next(0, 10);
                        if (critChance == 0)
                        {
                            inflictedDamage *= 2;
                            Console.Write("Your heavy attack hit dealing ");
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write(inflictedDamage);
                            Console.ResetColor();
                            Console.WriteLine(" in critical damage!");
                        }
                        else
                        {
                            Console.Write("Your heavy attack dealt ");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(inflictedDamage);
                            Console.ResetColor();
                            Console.WriteLine(" damage");
                        }
                    }
                    else Console.WriteLine("Your attack missed!");
                    break;
                case "magic":
                    //magic attack
                    inflictedDamage = rnd.Next(6, 10);
                    hitChance = rnd.Next(0, 5);
                    if (hitChance < 4)
                    {
                        Console.Write("Your magic spell inflicted ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(inflictedDamage);
                        Console.ResetColor();
                        Console.WriteLine(" damage to your opponent!");
                    }
                    else
                    {
                        Console.Write("Your accidentally hit yourself with magic and lost ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(inflictedDamage * 2);
                        Console.ResetColor();
                        Console.WriteLine(" in self damage!");
                        selfDamage = -inflictedDamage * 2;
                        inflictedDamage = 0;
                    }
                    break;
                case "heal":
                    //healing yourself
                    selfDamage = rnd.Next(1, 8);
                    Console.Write("You healed yourself for ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(selfDamage);
                    Console.ResetColor();
                    Console.WriteLine(" health");
                    break;
                case "stun":
                    hitChance = rnd.Next(0, 4);
                    if (hitChance <= 1)
                    {
                        Console.WriteLine("Succesfully stunned opponent");
                        stunnedOpponent = true;
                    }
                    else if (hitChance == 3)
                    {
                        Console.WriteLine("You stunned yourself");
                        stunnedSelf = true;
                    }
                    break;
            }
        }
        static int BotAttack()
        {
            int damage;
            int attackType = rnd.Next(0, 10);
            //light attack
            if (attackType >= 0 && attackType <= 6)
            {
                Console.Write($"{p2Name} used a light attack\n");
                int hitChance = rnd.Next(0, 10);
                if (hitChance < 9)
                {
                    damage = rnd.Next(1, 5);
                    int critChance = rnd.Next(0, 8);
                    if (critChance == 0)
                    {
                        damage *= 2;
                        Console.Write($"{p2Name}'s light attack hit dealing ");
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write(damage);
                        Console.ResetColor();
                        Console.WriteLine(" in critical damage!");
                    }
                    else
                    {
                        Console.Write($"{p2Name}'s light attack dealt ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(damage);
                        Console.ResetColor();
                        Console.WriteLine(" damage");
                    }
                    return damage;
                }
                else Console.WriteLine($"{p2Name}'s attack missed!");
            }
            //heavy attack
            else if (attackType >= 7 && attackType <= 9)
            {
                Console.Write($"{p2Name} used a heavy attack\n");
                int hitChance = rnd.Next(0, 15);
                if (hitChance < 8)
                {
                    damage = rnd.Next(5, 10);
                    int critChance = rnd.Next(0, 12);
                    if (critChance == 0)
                    {
                        damage *= 2;
                        Console.Write($"{p2Name}'s heavy attack hit dealing ");
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        Console.Write(damage);
                        Console.ResetColor();
                        Console.WriteLine(" in critical damage!");
                    }
                    else
                    {
                        Console.Write($"{p2Name}'s heavy attack dealt ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(damage);
                        Console.ResetColor();
                        Console.WriteLine(" damage");
                    }
                    return damage;
                }
                else Console.WriteLine($"{p2Name}'s attack missed!");
            }
            return 0;
        }

        static bool CheckHealth(float x)
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
                if (CheckHealth(p1Health)) Console.WriteLine($"{p1Name} lost");
                else if (CheckHealth(p2Health)) Console.WriteLine($"{p2Name} lost");
                else Console.WriteLine("You shouldn't be seeing this");
                Console.WriteLine("Do you want to play again? y/n?");
                string response = Console.ReadLine().ToLower();
                while (!(response == "y" || response == "n"))
                {
                    Console.WriteLine("Please answer with y/n");
                    response = Console.ReadLine().ToLower();
                }
                if (response == "n") endApp = true;
                gameActive = false;
            }
            else
            {
                Console.ReadKey();
                player1Turn = !player1Turn;
            }
        }
        static void Setup()
        {
            Console.Clear();
            Console.WriteLine("Welcome to fight!");
            //choose opponent
            Console.WriteLine("How do you want to play? Against another player or bot?");
            Console.WriteLine("Enter player/bot: ");
            string response = Console.ReadLine().ToLower();
            while (!(response == "bot" || response == "player"))
            {
                Console.WriteLine("Please enter player/bot");
                response = Console.ReadLine().ToLower();
            }
            if (response == "player") versus = true;
            else
            {
                versus = false;
                p2Name = "Opponent";
            }
            //select max health for players
            Console.WriteLine("How much hp do you wanna play with? Leave blank if standard");
            response = Console.ReadLine();
            while (!int.TryParse(response, out maxHealthPlayers))
            {
                if ((response == "" || response == " ")) break;
                Console.WriteLine("Enter a number or leave blank for standard");
                response = Console.ReadLine();
            }
            if (response == "" || response == " ") maxHealthPlayers = 50;
            else maxHealthPlayers = int.Parse(response);

            //choose names
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
        }
    }
}