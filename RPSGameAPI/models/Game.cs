using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace RPSGameAPI.models
{
    public class Game
    {
        public string PlayerChoice { get; set; }
        public string CPUChoice { get; set; }
        public string GameResult { get; set; }
        public string PlayerName { get; set; }
        public int currentRound { get; set; }
        public int gameMaxRounds { get; set; }
        public int userScore;
        public int cpuScore;
        public string finalGameResult { get; set; }

        //public string connectionString1;

        public void GenerateCPUChoice()
        {
            string[] CPUChoiceOptions = { "Rock", "Paper", "Scissors" };
            Random random = new Random();
            this.CPUChoice = CPUChoiceOptions[random.Next(0, 3)];
        }

        public void PlayerSelection(string playerselection, string playername, int currentround, int gamemaxrounds)
        {
            // Player player = new Player(playerselection);
            this.PlayerChoice = playerselection;
            this.PlayerName = playername;
            this.currentRound = currentround;
            this.gameMaxRounds = gamemaxrounds;
        }

        public void GenerateGameRoundWinner()
        {

            this.GenerateCPUChoice();
            string combineSelections = this.PlayerChoice + this.CPUChoice;

            if (combineSelections == "RockScissors" || combineSelections == "PaperRock" || combineSelections == "ScissorsPaper")
            {
                this.GameResult = "Win";
                // this.userScore++;
            }
            else if (combineSelections == "RockPaper" || combineSelections == "ScissorsRock" || combineSelections == "PaperScissors")
            {
                this.GameResult = "Lose";
                // this.cpuscore++;
            }
            else
            {
                this.GameResult = "Draw";
            }

        }

        public void GenerateGameResult()
        {

            if (this.userScore > this.cpuScore)
            {
                this.finalGameResult = "Win";
            }
            else if (this.userScore < this.cpuScore)
            {
                this.finalGameResult = "Lose";
            }
            else
            {
                this.finalGameResult = "Draw";
            }

        }

        //This Section Below is for SQL integration

        //Adds user to the Player Table
        public string AddUserToDb(string connectionString)
        {

            //Connect to an SQL Server Database - Azure
            //string connectionString = @"Data Source=kmazuredb.database.windows.net;Initial Catalog=RockPaperScissors_DB;User ID=km;Password=SA@12345;Connect Timeout=30;Encrypt=True;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            //Connect to an SQL Server Database - Local
            //string connectionString = @"Data Source=KMVM01;User ID=km;Password=KM@123;Initial Catalog=RockPaperScissorsGame_db;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            SqlConnection conn = new SqlConnection(connectionString);

            string queryString = "INSERT INTO PLAYER VALUES ('" + this.PlayerName + "')";

            SqlCommand command = new SqlCommand(queryString, conn);
            conn.Open();

            try
            {
                var result = command.ExecuteNonQuery();
                return result.ToString();
            }
            catch (SqlException se)
            {
                return "user already exists " + se.Message;
            }

        }

        //Adds user to the Player Table
        public string AddGameToDb(int gamemaxrounds, string connectionString, DateTime cdt)
        {

            SqlConnection conn = new SqlConnection(connectionString);

            //string queryString = "INSERT INTO GAME VALUES ('" + this.PlayerName + "','" + DateTime.Now.ToString("MM/dd/yyyy hh:mm") + "'," + gamemaxrounds + ",'" + this.finalGameResult[0] + "')";
            string queryString = "INSERT INTO GAME VALUES ('" + this.PlayerName + "','" + cdt.ToString("MM/dd/yyyy hh:mm:ss") + "'," + gamemaxrounds + ",'" + this.finalGameResult[0] + "')";

            SqlCommand command = new SqlCommand(queryString, conn);
            conn.Open();

            try
            {
                var result = command.ExecuteNonQuery();
                return result.ToString();
            }
            catch (SqlException se)
            {
                return "user already exists " + se.Message;
            }

        }

        public string AddRoundToDb(string playername, int currentround, string playerchoice, string cpuchoice, string connectionString, DateTime cdt)
        {

            SqlConnection conn = new SqlConnection(connectionString);

            //string queryString = "INSERT INTO TURN VALUES ('" + playername + "','" + DateTime.Now.ToString("MM/dd/yyyy hh:mm") + "','" + currentround + "','" + playerchoice + "','" + cpuchoice + "')";
            string queryString = "INSERT INTO TURN VALUES ('" + playername + "','" + cdt.ToString("MM/dd/yyyy hh:mm:ss") + "','" + currentround + "','" + playerchoice + "','" + cpuchoice + "')";

            SqlCommand command = new SqlCommand(queryString, conn);
            conn.Open();

            try
            {
                var result = command.ExecuteNonQuery();
                return result.ToString();
            }
            catch (SqlException se)
            {
                return "user already exists " + se.Message;
            }

        }

        //Retrieve Leaderboard data
        public List<LeaderboardResponse> getLeaderboardData(string connectionString)
        {

            List<LeaderboardResponse> customers = new List<LeaderboardResponse>();

            SqlConnection conn = new SqlConnection(connectionString);

            string queryString = "Select * From LEADERBOARD";

            SqlCommand command = new SqlCommand(queryString, conn);
            conn.Open();

            string result = "";
            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result += reader[0] + " | " + reader[1] + reader[2] + reader[3] + reader[4] + "\n";

                    // ORM - Object Relation Mapping
                    customers.Add(
                        new LeaderboardResponse()
                        {
                            username = reader[0].ToString(),
                            winRatio = Math.Round(((double)reader[1]), 2),
                            gamesPlayed = (int)reader[2],
                            roundsPlayed = (int)reader[3],
                            last5Games = reader[4].ToString()
                        });
                }
            }

            return customers;
        }

    }

}