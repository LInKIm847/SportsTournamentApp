

using System;

using System.IO;

using Microsoft.Data.SqlClient;
 
namespace SportsTournamentApp

{

    internal class Program

    {

        // Change Server if your SQL Server instance is different.

        static string connectionString =

            @"Server=localhost;Database=SportsTournamentDB;Integrated Security=True;TrustServerCertificate=True;";
 
        static void Main(string[] args)

        {

            Player player1 = new Player("Lebo", "Football", 20, 0);

            Player player2 = new Player("Thandi", "Athletics", 17, 0);
 
            player1.DisplayDetails();

            player2.DisplayDetails();
 
            bool running = true;
 
            while (running)

            {

                Console.WriteLine("\nCOMMUNITY SPORTS TOURNAMENT SYSTEM");

                Console.WriteLine("1. Register and display player");

                Console.WriteLine("2. Calculate match points");

                Console.WriteLine("3. Check player eligibility");

                Console.WriteLine("4. Exit");

                Console.Write("Enter your choice: ");
 
                if (!int.TryParse(Console.ReadLine(), out int choice))

                {

                    Console.WriteLine("Please enter a valid number.");

                    continue;

                }
 
                switch (choice)

                {

                    case 1:

                    {

                        Console.Write("Enter player name: ");

                        string name = Console.ReadLine() ?? "";
 
                        Console.Write("Enter sport: ");

                        string sport = Console.ReadLine() ?? "";
 
                        Console.Write("Enter age: ");
 
                        if (!int.TryParse(Console.ReadLine(), out int age)

                            || age < 1)

                        {

                            Console.WriteLine("Enter a valid age.");

                            break;

                        }
 
                        Player player = new Player(name, sport, age, 0);

                        player.DisplayDetails();

                        break;

                    }
 
                    case 2:

                    {

                        Console.Write("Enter match category (1, 2 or 3): ");
 
                        if (!int.TryParse(Console.ReadLine(), out int difficulty))

                        {

                            Console.WriteLine("Enter a valid number.");

                            break;

                        }
 
                        int points = CalculatePoints(difficulty);
 
                        if (points == -1)

                        {

                            Console.WriteLine("Invalid match category.");

                            break;

                        }
 
                        Console.WriteLine($"Points awarded: {points}");

                        break;

                    }
 
                    case 3:

                    {

                        Console.Write("Enter player name: ");

                        string name = Console.ReadLine() ?? "";
 
                        Console.Write("Enter sport: ");

                        string sport = Console.ReadLine() ?? "";
 
                        Console.Write("Enter age: ");
 
                        if (!int.TryParse(Console.ReadLine(), out int age)

                            || age < 1)

                        {

                            Console.WriteLine("Enter a valid age.");

                            break;

                        }
 
                        Player player = new Player(name, sport, age, 0);
 
                        Console.Write("Enter category (1, 2 or 3): ");
 
                        if (!int.TryParse(Console.ReadLine(), out int difficulty))

                        {

                            Console.WriteLine("Enter a valid number.");

                            break;

                        }
 
                        if (CheckEligibility(player, difficulty))

                        {

                            int points = CalculatePoints(difficulty);

                            player = new Player(

                                player.PlayerName, player.Sport,

                                player.Age, points);
 
                            Console.WriteLine("Player is eligible.");

                            player.DisplayDetails();
 
                            string category = GetCategoryName(difficulty);

                            SaveMatchReport(player, category, points);

                            SaveMatchToDatabase(
                                player.PlayerName, player.Sport, category, points);

                        }

                        else

                        {

                            Console.WriteLine(

                                "Player is not eligible or the category is invalid.");

                        }
 
                        break;

                    }
 
                    case 4:

                        running = false;

                        Console.WriteLine("Goodbye!");

                        break;
 
                    default:

                        Console.WriteLine("Invalid option. Try again.");

                        break;

                }

            }

        }
 
        static int CalculatePoints(int difficulty)

        {

            if (difficulty == 1)

                return 50;
 
            if (difficulty == 2)

                return 100;
 
            if (difficulty == 3)

                return 200;
 
            return -1;

        }
 
        static bool CheckEligibility(Player player, int difficulty)

        {

            if (difficulty == 1 && player.Age >= 12)

                return true;
 
            if (difficulty == 2 && player.Age >= 16)

                return true;
 
            if (difficulty == 3 && player.Age >= 18)

                return true;
 
            return false;

        }

        static string GetCategoryName(int difficulty)
        {
            return difficulty switch
            {
                1 => "Junior",
                2 => "Intermediate",
                3 => "Senior",
                _ => throw new ArgumentOutOfRangeException(nameof(difficulty))
            };
        }
 
        static void SaveMatchReport(

            Player player, string category, int points)

        {

            try

            {

                string report =

                    $"Player Name: {player.PlayerName}{Environment.NewLine}" +

                    $"Sport: {player.Sport}{Environment.NewLine}" +

                    $"Age: {player.Age}{Environment.NewLine}" +

                    $"Match Category: {category}{Environment.NewLine}" +

                    $"Points Awarded: {points}{Environment.NewLine}" +

                    "--------------------------" + Environment.NewLine;
 
                File.AppendAllText("MatchReport.txt", report);
 
                Console.WriteLine("Match report saved.");

            }

            catch (IOException ex)

            {

                Console.WriteLine($"File error: {ex.Message}");

            }

            catch (UnauthorizedAccessException ex)

            {

                Console.WriteLine($"Access denied: {ex.Message}");

            }

        }
 
        static void SaveMatchToDatabase(
            string playerName, string sport, string category, int points)

        {

            try

            {

                using SqlConnection connection =

                    new SqlConnection(connectionString);
 
                connection.Open();
 
                string query = @"

                    INSERT INTO MatchResults

                    (PlayerName, Sport, MatchCategory, PointsAwarded)

                    VALUES

                    (@PlayerName, @Sport, @MatchCategory, @PointsAwarded)";
 
                using SqlCommand command =

                    new SqlCommand(query, connection);
 
                command.Parameters.AddWithValue(
                    "@PlayerName", playerName);

                command.Parameters.AddWithValue(
                    "@Sport", sport);

                command.Parameters.AddWithValue(

                    "@MatchCategory", category);

                command.Parameters.AddWithValue(

                    "@PointsAwarded", points);
 
                command.ExecuteNonQuery();
 
                Console.WriteLine("Match saved to the database.");

            }

            catch (SqlException ex)

            {

                Console.WriteLine($"Database error: {ex.Message}");

            }

        }
 
        static void DisplayAllMatches()

        {

            try

            {

                using SqlConnection connection =

                    new SqlConnection(connectionString);
 
                connection.Open();
 
                string query = @"

                    SELECT MatchID, PlayerName, Sport,

                           MatchCategory, PointsAwarded

                    FROM MatchResults";
 
                using SqlCommand command =

                    new SqlCommand(query, connection);
 
                using SqlDataReader reader = command.ExecuteReader();
 
                while (reader.Read())

                {

                    Console.WriteLine(

                        $"ID: {reader["MatchID"]}, " +

                        $"Player: {reader["PlayerName"]}, " +

                        $"Sport: {reader["Sport"]}, " +

                        $"Category: {reader["MatchCategory"]}, " +

                        $"Points: {reader["PointsAwarded"]}");

                }

            }

            catch (SqlException ex)

            {

                Console.WriteLine($"Database error: {ex.Message}");

            }

        }

    }

}

 
