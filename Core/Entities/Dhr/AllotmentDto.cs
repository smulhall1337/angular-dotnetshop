namespace Core.Entities.Dhr
{
    public class Allotment
    {
        public DateTime TransactionDate { get; set; }
        public DateTime AvailableDate { get; set; }
        public string PriorMonthBenifit { get; set; }
        public string ClaimPayment { get; set; }
        public string IssueAmount { get; set; }
        public string InitialAmount { get; set; }
        public string IssuanceType { get; set; }
    }
}
