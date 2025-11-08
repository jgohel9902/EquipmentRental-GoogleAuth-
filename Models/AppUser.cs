namespace EquipmentRentalApi.Models
{
    public class AppUser
    {
        public int Id { get; set; }
        public string Email { get; set; } = default!;
        public string Role { get; set; } = "User";
        public string? ExternalProvider { get; set; } = "Google";
        public string? ExternalId { get; set; }
    }
}
