using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using SQLite;

namespace GameTracker.Models
{
    public class Game
    {
        // --- ข้อมูลที่ได้มาจาก RAWG API ---

        [PrimaryKey]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("background_image")]
        public string BackgroundImage { get; set; }

        [JsonPropertyName("rating")]
        public double Rating { get; set; }

        // --- ข้อมูลสำหรับบันทึกในแอปของเราเอง (Local Data) ---

        public string Platform { get; set; }
        public int Progress { get; set; }
        public string PlayStatus { get; set; }
        public bool IsFavorite { get; set; }
    }

    public class GameResponse
    {
        [JsonPropertyName("results")]
        public List<Game> Results { get; set; }
    }
}