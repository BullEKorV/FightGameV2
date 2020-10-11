using System;
using System.Collections.Generic;

namespace Fight
{
    class Program
    {
        static Random rnd = new Random();
        static List<string> attackTypes = new List<string>() { "light", "heavy", "magic", "heal", "stun" };
        static int maxPlayers;
        static List<string> playerNames = new List<string>();
        static List<int> playerHealth = new List<int>();
        static List<bool> isHuman = new List<bool>();
        static List<bool> isStunned = new List<bool>();
        static List<bool> isDead = new List<bool>();
        static int currentPlayer;
        static int targetPlayer;
        static bool endApp = false;
        static bool gameActive = true;

        public static void Main(string[] args)
        {
            while (!endApp)
            {
                Setup();

                while (gameActive)
                {
                    GameLogic();
                }
            }
        }

        static void StatusBar()
        {
            int x = Console.CursorLeft;
            int y = Console.CursorTop;
            Console.CursorTop = Console.WindowTop;
            Console.CursorLeft = Console.WindowLeft;
            for (int i = 0; i < maxPlayers; i++)
            {
                if (isStunned[i]) Console.ForegroundColor = ConsoleColor.Red;
                else if (isDead[i]) Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(playerNames[i]);
                Console.ResetColor();
                Console.Write("'s health: ");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(playerHealth[i] + "       ");
                Console.ResetColor();
            }

            // Restore previous position
            Console.SetCursorPosition(x, y + 1);
        }
        static void GameLogic()
        {
            Console.Clear();

            //player 1

            StatusBar();
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write("\n" + playerNames[currentPlayer]);
            Console.ResetColor();
            Console.WriteLine("'s turn!\n");

            if (isStunned[currentPlayer])
            {
                int recoveryRate = rnd.Next(0, 2);
                if (recoveryRate == 0)
                {
                    isStunned[currentPlayer] = false;
                    Console.WriteLine("You're no longer stunned");
                }
                else Console.WriteLine("You're still stunned");
            }
            StatusBar();

            if (!isStunned[currentPlayer] && !isDead[currentPlayer])
            {
                if (isHuman[currentPlayer]) PlayerAttack();
                else if (!isHuman[currentPlayer]) BotAttack();
            }

            StatusBar();

            CheckLose();
            Console.Write("\nContinue...");
        }

        static void PlayerAttack()
        {
            //choose attack to perform
            Console.WriteLine("What attack do you wanna perform?");
            Console.Write("Available attacks: ");
            foreach (string i in attackTypes) Console.Write($"| {i} ");
            Console.WriteLine("");
            string attackType = Console.ReadLine().ToLower();
            while (!attackTypes.Contains(attackType))
            {
                Console.Write("Available attacks: ");
                foreach (string i in attackTypes) Console.Write($"{i}, ");
                Console.WriteLine("");
                attackType = Console.ReadLine().ToLower();
            }

            //choose who to attack
            Console.WriteLine("\nWho do you wanna attack?");
            Console.Write("Available players: ");
            for (int i = 0; i < maxPlayers; i++)
            {
                Console.Write($"| {i + 1}. {playerNames[i]} ");
                if (i == currentPlayer) Console.Write("(you) ");
            }
            Console.WriteLine("");
            string response = "";
            response = Console.ReadLine();
            while (!playerNames.Contains(response) && !(int.TryParse(response, out targetPlayer)))
            {
                Console.Write("Available players: ");
                for (int i = 0; i < maxPlayers; i++)
                {
                    if (i != currentPlayer) Console.Write($"| {i}. {playerNames[i]} ");
                }
                Console.WriteLine("");
                response = Console.ReadLine();
            }
            if (int.TryParse(response, out targetPlayer)) targetPlayer = int.Parse(response) - 1;
            else targetPlayer = playerNames.IndexOf(response);
            Console.WriteLine("");
            AttackScript(attackType);
        }
        static void BotAttack()
        {
            string attackType = "";
            //choose target player
            targetPlayer = rnd.Next(0, maxPlayers);
            while (targetPlayer == currentPlayer || isDead[targetPlayer]) targetPlayer = rnd.Next(0, maxPlayers);
            //choose attack
            int attackTypeInt = rnd.Next(0, 16);
            if (attackTypeInt >= 0 && attackTypeInt <= 4) attackType = "light";
            else if (attackTypeInt >= 5 && attackTypeInt <= 7) attackType = "heavy";
            else if (attackTypeInt >= 8 && attackTypeInt <= 10) attackType = "magic";
            else if (attackTypeInt >= 11 && attackTypeInt <= 13) attackType = "heal";
            else if (attackTypeInt >= 14 && attackTypeInt <= 15) attackType = "stun";
            AttackScript(attackType);
        }
        static void AttackScript(string attackType)
        {
            int damage = 0;
            switch (attackType)
            {
                //light attack
                case "light":
                    int hitChance = rnd.Next(0, 10);
                    if (hitChance < 9)
                    {
                        damage = rnd.Next(3, 6);
                        int critChance = rnd.Next(0, 5);
                        if (critChance == 0)
                        {
                            damage *= 2;
                            Console.Write($"Your light attack hit {playerNames[targetPlayer]} dealing ");
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write(damage);
                            Console.ResetColor();
                            Console.WriteLine(" in critical damage!");
                        }
                        else
                        {
                            Console.Write("Your light attack dealt ");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(damage);
                            Console.ResetColor();
                            Console.WriteLine($" damage to {playerNames[targetPlayer]}");
                        }
                        playerHealth[targetPlayer] -= damage;
                    }
                    else Console.WriteLine("Your light attack missed!");
                    break;
                case "heavy":
                    //heavy attack
                    hitChance = rnd.Next(0, 16);
                    if (hitChance < 8)
                    {
                        damage = rnd.Next(9, 13);
                        int critChance = rnd.Next(0, 10);
                        if (critChance == 0)
                        {
                            damage *= 2;
                            Console.Write("Your heavy attack hit dealing ");
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.Write(damage);
                            Console.ResetColor();
                            Console.WriteLine($" in critical damage to {playerNames[targetPlayer]}!");
                        }
                        else
                        {
                            Console.Write("Your heavy attack dealt ");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(damage);
                            Console.ResetColor();
                            Console.WriteLine($" damage to {playerNames[targetPlayer]}");
                        }
                        playerHealth[targetPlayer] -= damage;
                    }
                    else Console.WriteLine("Your heavy attack missed!");
                    break;
                case "magic":
                    //magic attack
                    damage = rnd.Next(6, 10);
                    hitChance = rnd.Next(0, 5);
                    if (hitChance < 4)
                    {
                        Console.Write("Your magic spell inflicted ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(damage);
                        Console.ResetColor();
                        Console.WriteLine($" damage to {playerNames[targetPlayer]}!");
                        playerHealth[targetPlayer] -= damage;
                    }
                    else
                    {
                        damage *= 2;
                        Console.Write("Your accidentally hit yourself with magic and lost ");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(damage);
                        Console.ResetColor();
                        Console.WriteLine(" in self damage!");
                        playerHealth[currentPlayer] -= damage;
                    }
                    break;
                case "heal":
                    //healing yourself
                    damage = rnd.Next(2, 8);
                    if (!isHuman[currentPlayer]) targetPlayer = currentPlayer;
                    if (currentPlayer == targetPlayer)
                    {
                        Console.Write("You healed yourself for ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(damage);
                        Console.ResetColor();
                        Console.WriteLine(" health");
                    }
                    else if (currentPlayer != targetPlayer)
                    {
                        Console.Write($"You healed {playerNames[targetPlayer]} for ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(damage);
                        Console.ResetColor();
                        Console.WriteLine(" health");
                    }
                    playerHealth[targetPlayer] += damage;
                    break;
                case "stun":
                    hitChance = rnd.Next(0, 3);
                    if (hitChance == 0)
                    {
                        Console.WriteLine($"Succesfully stunned {playerNames[targetPlayer]}");
                        isStunned[targetPlayer] = true;

                    }
                    else if (hitChance > 0)
                    {
                        damage = rnd.Next(3, 6);
                        Console.Write("You accidentally stunned yourself and hit yourself for ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(damage);
                        Console.ResetColor();
                        Console.WriteLine(" health");
                        playerHealth[currentPlayer] -= damage;
                        isStunned[currentPlayer] = true;
                    }
                    break;
                default:
                    Console.WriteLine("No attack selected");
                    break;
            }
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
            int playersLeft = maxPlayers;
            for (int i = 0; i < maxPlayers; i++)
            {
                if (playerHealth[i] <= 0)
                {
                    playerHealth[i] = 0;
                    if (!isDead[i]) Console.WriteLine($"{playerNames[i]} died!");
                    isDead[i] = true;
                    isStunned[i] = false;
                    playersLeft--;
                }
            }
            if (playersLeft == 1)
            {
                gameActive = false;
                int winner = isDead.IndexOf(false);
                Console.WriteLine($"\n{playerNames[winner]} won!");
            }
            if (!gameActive)
            {
                Console.WriteLine("Do you want to play again? y/n?");
                string response = Console.ReadLine().ToLower();
                while (!(response == "y" || response == "n"))
                {
                    Console.WriteLine("Please answer with y/n");
                    response = Console.ReadLine().ToLower();
                }
                if (response == "n") endApp = true;
            }
            else if (gameActive)
            {
                Console.ReadKey();
                if (currentPlayer < maxPlayers - 1) currentPlayer++;
                else currentPlayer = 0;
                Console.WriteLine(currentPlayer);
            }
        }
        static void Setup()
        {
            //reset values
            playerNames.Clear();
            playerHealth.Clear();
            isHuman.Clear();
            isStunned.Clear();
            isDead.Clear();
            currentPlayer = 0;
            gameActive = true;

            //setup the game
            Console.Clear();
            Console.WriteLine("Welcome to fight!");
            //choose opponent
            Console.WriteLine("How many players?");
            Console.WriteLine("Enter amount of players: ");
            string response = Console.ReadLine();
            while (!int.TryParse(response, out maxPlayers))
            {
                Console.WriteLine("Please enter a number");
                response = Console.ReadLine();
            }
            maxPlayers = int.Parse(response);
            for (int i = 0; i < maxPlayers; i++)
            {
                isStunned.Add(false);
                isDead.Add(false);
            }
            //select max health for players
            Console.WriteLine("How much hp do you wanna play with? Leave blank if standard");
            response = Console.ReadLine();
            int maxHealthPlayers;
            while (!int.TryParse(response, out maxHealthPlayers))
            {
                if ((response == "" || response == " ")) break;
                Console.WriteLine("Enter a number or leave blank for standard");
                response = Console.ReadLine();
            }
            if (response == "" || response == " ") maxHealthPlayers = 50;
            else maxHealthPlayers = int.Parse(response);

            for (int i = 0; i < maxPlayers; i++)
            {
                playerHealth.Add(maxHealthPlayers);
            }
            //choose names
            for (int i = 0; i < maxPlayers; i++)
            {
                Console.WriteLine($"Enter Player {i + 1}'s name or leave blank for bot");
                response = Console.ReadLine().ToLower();
                while ((response.Length > 10 || response.Length < 3) && response != "")
                {
                    Console.WriteLine("Name cannot be shorter than 3 characters or longer than 12.");
                    response = Console.ReadLine();
                }
                if (response == "")
                {
                    playerNames.Add("bot " + (i + 1));
                    isHuman.Add(false);
                }
                else
                {
                    playerNames.Add(response);
                    isHuman.Add(true);
                }
            }
        }
    }
}