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
                //runs the setup script
                Setup();
                while (gameActive)
                {
                    //runs the game
                    GameLogic();
                }
            }
        }
        static void StatusBar()
        {
            //moves the cursor to top to write the statusbar
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

            //player / bot script
            if (!isDead[currentPlayer])
            {
                StatusBar();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write("\n" + playerNames[currentPlayer]);
                Console.ResetColor();
                Console.WriteLine("'s turn!\n");

                //decide if player / bot gets unstunned
                if (isStunned[currentPlayer])
                {
                    int recoveryRate = rnd.Next(0, 2);
                    if (recoveryRate == 0)
                    {
                        isStunned[currentPlayer] = false;
                        Console.WriteLine("\nYou're no longer stunned");
                    }
                    else Console.WriteLine("\nYou're still stunned");
                }
                StatusBar();

                //run player / bot attack script
                if (!isStunned[currentPlayer])
                {
                    if (isHuman[currentPlayer]) PlayerAttack();
                    else if (!isHuman[currentPlayer]) BotAttack();
                }
                StatusBar();
            }
            CheckLose();
        }

        static void PlayerAttack()
        {
            //player 
            Console.WriteLine("What attack do you wanna perform?");
            Console.Write("Available attacks: ");
            foreach (string i in attackTypes) Console.Write($"| {i} ");
            Console.WriteLine("");
            string attackType = Console.ReadLine().ToLower();
            while (!attackTypes.Contains(attackType))
            {
                Console.Write("Available attacks: ");
                foreach (string i in attackTypes) Console.Write($"| {i} ");
                Console.WriteLine("");
                attackType = Console.ReadLine().ToLower();
            }

            //choose who to attack
            Console.WriteLine("\nWho do you wanna attack?");
            Console.Write("Available players: ");
            for (int i = 0; i < maxPlayers; i++)
            {
                if (!isDead[i]) Console.Write($"| {i + 1}. {playerNames[i]} ");
                if (i == currentPlayer) Console.Write("(you) ");
            }
            Console.WriteLine("");
            string response;
            response = Console.ReadLine();
            while (!playerNames.Contains(response) && !(int.TryParse(response, out targetPlayer)))
            {
                Console.Write("Available players: ");
                for (int i = 0; i < maxPlayers; i++)
                {
                    if (!isDead[i]) Console.Write($"| {i + 1}. {playerNames[i]} ");
                    if (i == currentPlayer) Console.Write("(you) ");
                }
                Console.WriteLine("");
                response = Console.ReadLine();
            }
            if (int.TryParse(response, out targetPlayer))
            {
                if (targetPlayer > maxPlayers) targetPlayer = maxPlayers - 1;
                else if (targetPlayer < 1) targetPlayer = 0;
                else targetPlayer = int.Parse(response) - 1;
            }
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
                //performs a light attack
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
                    //performs a heavy attack
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
                    //performs a magic attack
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
                    //heals yourself
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
                    if (!isDead[targetPlayer]) playerHealth[targetPlayer] += damage;
                    break;
                case "stun":
                    //performs stun
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
                        Console.ForegroundColor = ConsoleColor.Red;
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
            if (gameActive)
            {
                int playersLeft = maxPlayers;
                for (int i = 0; i < maxPlayers; i++)
                {
                    if (CheckHealth(playerHealth[i]))
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
                    Console.WriteLine($"\n{playerNames[isDead.IndexOf(false)]} won!");
                }
                StatusBar();
                if (!isDead[currentPlayer])
                {
                    Console.Write("\nContinue...");
                    Console.ReadKey();
                }
                if (currentPlayer < maxPlayers - 1) currentPlayer++;
                else currentPlayer = 0;
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

            Console.Clear();
            Console.WriteLine("Welcome to fight!");
            //choose ammount of players and bots
            Console.WriteLine("Choose ammount of players and/or bots to play\n");
            Console.Write("Enter amount: ");
            string response = Console.ReadLine();
            while (!int.TryParse(response, out maxPlayers))
            {
                Console.Write("Enter amount: ");
                response = Console.ReadLine();
            }
            maxPlayers = int.Parse(response);
            for (int i = 0; i < maxPlayers; i++)
            {
                isStunned.Add(false);
                isDead.Add(false);
            }
            //select starting health for players and/or bots
            Console.Clear();
            Console.WriteLine("Choose the ammount of health to start with\n");
            Console.Write("Enter amount or leave blank for 50hp: ");
            response = Console.ReadLine();
            int maxHealthPlayers;
            while (!int.TryParse(response, out maxHealthPlayers))
            {
                if ((response == "" || response == " ")) break;
                Console.Write("Enter amount or leave blank for 50hp: ");
                response = Console.ReadLine();
            }
            if (response == "" || response == " ") maxHealthPlayers = 50;
            else maxHealthPlayers = int.Parse(response);

            for (int i = 0; i < maxPlayers; i++)
            {
                playerHealth.Add(maxHealthPlayers);
            }
            //choose names for players and/or bots
            Console.Clear();
            Console.WriteLine("Choose names for the players or leave blank for a bots\n");
            for (int i = 0; i < maxPlayers; i++)
            {
                Console.Write($"Player {i + 1}: ");
                response = Console.ReadLine().ToLower();
                while ((response.Length > 10 || response.Length < 3) && response != "")
                {
                    Console.WriteLine("Name cannot be shorter than 3 or longer than 12 characters");
                    Console.Write($"Player {i + 1}: ");
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