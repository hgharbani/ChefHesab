namespace ChefHesab.Dto.food.FoodProvider
{
    public class CreateFoodProviderVM
    {
        public Guid Id { get; set; }
        public Guid ContractCompanyId { get; set; }
        public Guid FoodStuffId { get; set; }
        public bool Active { get; set; } // Active
        public DateTime? InsertDate { get; set; } // InsertDate
        public long? AmountRequested { get; set; } // AmountRequested

    }
}
