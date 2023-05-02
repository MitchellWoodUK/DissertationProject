namespace DissertationProject.Models
{
    public class ChartViewModel
    {
        public List<FamilyMembersModel> Members { get; set; }
        public List<FamilyBillModel> Bills { get; set; }
        public List<FamilyBudgetModel> Budgets { get; set; }
        public List<FamilyTransactionModel> Transactions { get; set; }
        public float Profit { get; set; }

        public float Expenses { get; set; }
    }
}
