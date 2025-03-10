using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace MyAspNetBackend.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("email")]
        public string Email { get; set; } = string.Empty;

        [BsonElement("password")]
        public string Password { get; set; } = string.Empty;

        [BsonElement("username")]
        public string Username { get; set; } = string.Empty;

        [BsonElement("full_name")]
        public string FullName { get; set; } = string.Empty;

        [BsonElement("avatar_url")]
        public string AvatarUrl { get; set; } = string.Empty;

        [BsonElement("bio")]
        public string? Bio { get; set; }

        [BsonElement("behavior")]
        public int Behavior { get; set; } = 0;

        [BsonElement("gender")]
        public string? Gender { get; set; }

        [BsonElement("age")]
        public int? Age { get; set; }

        [BsonElement("location")]
        public GeoLocation Location { get; set; } = new();

        [BsonElement("followers")]
        public List<string> Followers { get; set; } = new();

        [BsonElement("following")]
        public List<string> Following { get; set; } = new();

        [BsonElement("is_online")]
        public bool IsOnline { get; set; } = false;

        [BsonElement("blocked_users")]
        public List<string> BlockedUsers { get; set; } = new();

        [BsonElement("preferences")]
        public UserPreferences Preferences { get; set; } = new();

        [BsonElement("refreshToken")]
        public string? RefreshToken { get; set; }

        [BsonElement("refreshTokenExpires")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? RefreshTokenExpires { get; set; }

        [BsonElement("created_at")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("last_online")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime? LastOnline { get; set; }
    }

    public class GeoLocation
    {
        [BsonElement("type")]
        public string Type { get; set; } = "Point";

        [BsonElement("coordinates")]
        public List<double> Coordinates { get; set; } = new() { 0.0, 0.0 };
    }

    public class UserPreferences
    {
        [BsonElement("notifications_enabled")]
        public bool NotificationsEnabled { get; set; } = true;

        [BsonElement("show_location_based_suggestions")]
        public bool ShowLocationBasedSuggestions { get; set; } = true;
    }
}
