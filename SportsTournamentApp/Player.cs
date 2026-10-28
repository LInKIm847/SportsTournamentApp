namespace SportsTournamentApp

{

    public class Player

    {

        public string PlayerName { get; set; }

        public string Sport { get; set; }

        public int Age { get; set; }

        public int Points { get; set; }
 
        public Player(string playerName, string sport, int age, int points)

        {

            PlayerName = playerName;

            Sport = sport;

            Age = age;

            Points = points;

        }
 
        public void DisplayDetails()

        {

            Console.WriteLine($"Player Name: {PlayerName}");

            Console.WriteLine($"Sport: {Sport}");

            Console.WriteLine($"Age: {Age}");

            Console.WriteLine($"Points: {Points}");

            Console.WriteLine("--------------------------");

        }

    }

}
 