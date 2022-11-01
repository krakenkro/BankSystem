namespace BankSystemBota.Models
{
    public class DepositInfo
    {
        public Guid Id { get; set; }
        public int Balance { get; set; }
        public int Income { get; set; }
        public int Outcome { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
