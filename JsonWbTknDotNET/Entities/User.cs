namespace JsonWbTknDotNET.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public string? UserName { get; set; } = string.Empty;

        public string? PassHash { get; set; } = string.Empty;

        public string? Role { get; set; } = string.Empty;
    }
}
