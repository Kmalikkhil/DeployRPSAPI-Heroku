using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RPSGameAPI.models;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace RPSGameAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RPSGameController : ControllerBase
    {
        public List<Game> RoundsData = new List<Game>();
        Game NewGame = new Game();

        //Can use this code part for one by one round results
        /*
        [HttpPost]

        public Game GetSelection(Player PlayerSelectionAngular){

            this.NewGame.PlayerSelection(PlayerSelectionAngular.PlayerChoice, PlayerSelectionAngular.PlayerName,PlayerSelectionAngular.currentRound, PlayerSelectionAngular.gameMaxRounds);
            this.NewGame.GenerateGameRoundWinner();
            this.NewGame.GenerateGameResult();
            this.NewGame.AddUserToDb();
            this.NewGame.AddGameToDb(PlayerSelectionAngular.gameMaxRounds);
            this.NewGame.AddRoundToDb();
            // this.NewGame.AddRoundToDb(PlayerSelectionAngular.PlayerName,PlayerSelectionAngular.currentRound,PlayerSelectionAngular.PlayerChoice);
            return NewGame;
        }
        */

         //This part is for SqlConnection Builder
        SqlConnectionStringBuilder stringBuilder = new SqlConnectionStringBuilder();
        IConfiguration configuration;
        string connectionString = "";
        public RPSGameController(IConfiguration iConfig) {
            this.configuration = iConfig;

            //use the SqlConnectionStringBuilder to create our connection string
            this.stringBuilder.DataSource = this.configuration.GetSection("DBConnectionString").GetSection("Url").Value;
            this.stringBuilder.InitialCatalog = this.configuration.GetSection("DBConnectionString").GetSection("Database").Value;
            this.stringBuilder.UserID = this.configuration.GetSection("DBConnectionString").GetSection("User").Value;
            this.stringBuilder.Password = this.configuration.GetSection("DBConnectionString").GetSection("Password").Value;

            this.connectionString = stringBuilder.ConnectionString;
            //this.NewGame.connectionString1 = stringBuilder.ConnectionString;

        }


        [HttpPost("postrounds")]

        public List<Game> GetRoundsResutls(Player[] PlayerSelectionAngular)
        {   
            DateTime CDT = DateTime.Now;
            int i = 0;
            foreach (var Player in PlayerSelectionAngular)
            {
                this.NewGame = new Game();
                this.NewGame.PlayerSelection(PlayerSelectionAngular[i].PlayerChoice, PlayerSelectionAngular[i].PlayerName, PlayerSelectionAngular[i].currentRound, PlayerSelectionAngular[i].gameMaxRounds);
                this.NewGame.GenerateGameRoundWinner();
                RoundsData.Add(NewGame);
                this.NewGame.AddUserToDb(this.connectionString);
                i++;
            }

            //Find the individual round results
            foreach (var res in RoundsData)
            {
                if (res.GameResult == "Win")
                {
                    this.NewGame.userScore++;
                }
                else if (res.GameResult == "Lose")
                {
                    this.NewGame.cpuScore++;
                }
            }

            this.NewGame.GenerateGameResult();
            this.NewGame.AddGameToDb(PlayerSelectionAngular[0].gameMaxRounds, this.connectionString, CDT);

            //Add the individual round data into database
            foreach (var rounditem in RoundsData)
            {
                this.NewGame.AddRoundToDb(rounditem.PlayerName, rounditem.currentRound, rounditem.PlayerChoice, rounditem.CPUChoice, this.connectionString, CDT);
            }

            return RoundsData;

        }


        // [HttpGet("GetFinalResult")]
        // public string GetRequestAmount() {
        //     // this.NewGame.GenerateGameResult();
        //     return this.NewGame.finalGameResult;

        // }



        [HttpGet("Leaderboard")]

        public List<LeaderboardResponse> TestConnection()
        {

            return NewGame.getLeaderboardData(this.connectionString);

        }

        [HttpGet("test")]

        public string Test()
        {

            return "Test";

        }


    }
}
