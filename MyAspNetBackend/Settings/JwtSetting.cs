namespace MyAspNetBackend.Settings;

public class JwtSettings
{
    public JwtSettings(string key, string issuer, string audience, int expiryMinutes)
    {
        Key = key;
        Issuer = issuer;
        Audience = audience;
        ExpiryMinutes = expiryMinutes;
    }

    public JwtSettings()
    {
    }

    public string Key { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpiryMinutes { get; set; }
}
