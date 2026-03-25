namespace Workcraft.ViewModels
{
    public class CompanyInfoViewModel
    {
        public string? CompanyName { get; set; }
        public string? Industry { get; set; }
        public string? CompanySize { get; set; }
        public string? Website { get; set; }

        public string? LegalName { get; set; }
        public string? TaxId { get; set; }
        public string? ContactEmail { get; set; }
        public string? Address { get; set; }

        public bool IsVerified { get; set; }
    }
}