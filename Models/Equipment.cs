namespace EquipmentRentalApi.Models
{
    public class Equipment
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Category { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string Condition { get; set; } = "Good";
        public decimal RentalPrice { get; set; }
        public bool IsAvailable { get; set; }
    }
}
