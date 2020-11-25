namespace RPSGameAPI.models
{
    public class Player
    {
        public string PlayerChoice { get; set; }

        public string PlayerName { get; set; }
        public int currentRound { get; set; }
        public int gameMaxRounds { get; set; }

        //Constructor
        public Player(string playerchoice, string playername, int currentround, int gamemaxrounds)
        {
            this.PlayerChoice = playerchoice;
            this.PlayerName = playername;
            this.currentRound = currentround;
            this.gameMaxRounds = gamemaxrounds;
        }
        //Default Constructor   
        public Player()
        {
            this.PlayerChoice = null;
        }

    }
}