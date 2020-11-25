using System;

namespace RPSGameAPI.models
{
    public class LeaderboardResponse
    {
        public string username { get; set; }
        public double winRatio { get; set; }
        public int gamesPlayed { get; set; }
        public int roundsPlayed { get; set; }
        public string last5Games { get; set; }


    }
}

// Code to switch to local db in appsettings.json
/*
"DbConnectionString": {
    "Url": "KMVM01",
    "Database": "RockPaperScissorsGame_db",
    "User": "km",
    "Password": "KM@123"
  },

// Code to switch to Azure db in appsettings.json
"DbConnectionString": {
    "Url": "kmazuredb.database.windows.net",
    "Database": "RockPaperScissorsGame_DB",
    "User": "km",
    "Password": "SA@12345"
  },
*/