namespace API.Dto
{
    public class IncomeExpenseEntryDto
    {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public MemberIncomeExpense[] EarnedIncomes { get; set; }
            public MemberIncomeExpense[] UnearnedIncomes { get; set; }
            public MemberIncomeExpense[] ChildSupportDeductions { get; set; }
            public decimal TotalEarned { get; set; }
            public decimal TotalUnearned { get; set; }
            public decimal TotalChildSupport { get; set; }



    }
     public class MemberIncomeExpense
    {
        public decimal Amount { get; set; }
        public string Type { get; set; }
    }
}
