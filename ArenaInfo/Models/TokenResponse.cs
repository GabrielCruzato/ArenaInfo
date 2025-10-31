using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ArenaInfo.Models
{
    public class TokenResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
    }

    public class PvpLeaderboard
    {
        [JsonPropertyName("season")]
        public Season Season { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("bracket")]
        public Bracket Bracket { get; set; }

        [JsonPropertyName("entries")]
        public List<LeaderboardEntry> Entries { get; set; }
    }

    public class Season
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }

    public class PvpSeasonIndex
    {
        [JsonPropertyName("seasons")]
        public List<SeasonReference> Seasons { get; set; }

        [JsonPropertyName("current_season")]
        public SeasonReference CurrentSeason { get; set; }
    }

    public class SeasonReference
    {
        [JsonPropertyName("key")]
        public ApiLink Key { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }
    }

    public class ApiLink
    {
        [JsonPropertyName("href")]
        public string Href { get; set; }
    }

    public class Bracket
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class LeaderboardEntry
    {
        [JsonPropertyName("character")]
        public Character Character { get; set; }

        [JsonPropertyName("faction")]
        public Faction Faction { get; set; }

        [JsonPropertyName("rank")]
        public int Rank { get; set; }

        [JsonPropertyName("rating")]
        public int Rating { get; set; }

        [JsonPropertyName("season_match_statistics")]
        public MatchStatistics SeasonMatchStatistics { get; set; }
    }

    public class Character
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("realm")]
        public Realm Realm { get; set; }
    }

    public class Realm
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("slug")]
        public string Slug { get; set; }
    }

    public class Faction
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class MatchStatistics
    {
        [JsonPropertyName("played")]
        public int Played { get; set; }

        [JsonPropertyName("won")]
        public int Won { get; set; }

        [JsonPropertyName("lost")]
        public int Lost { get; set; }
    }
}