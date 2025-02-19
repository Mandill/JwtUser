namespace JwtUser.Modal
{
    public class CustomerModal
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public decimal? CreditLimit { get; set; }
        public bool? IsActive { get; set; }
        public string? StatusName { get; set; }

    }
}
