using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Dto.food.FoodProvider
{
    public class FoodProviderPrintVm
    {
        public string Companytitle { get; set; }
        public string CategoryName { get; set; }
        public List<FoodVm> FoodVm { get; set; } = new List<FoodVm>();

    }
    public class FoodVm
    {
        public string FoodName { get; set; }
        public int Amount { get; set; }
        public double? TotalPric { get; set; }
        public List<StuffPricevm> StuffPricevm { get; set; }=new List<StuffPricevm>();
        public List<Additionalcostvm> AdditionalcostVm { get; set; }=new List<Additionalcostvm>();
    }

    public class StuffPricevm
    {
        public string StuffPriceTitle { get; set; }
        public double? Amount { get; set; }
        public double? Price { get; set; }
        public double? TotalPrice { get; set; }
        public string Unit { get; set; }
    }
    public class Additionalcostvm
    {
        public Guid Id { get; set; }
        public Guid AdditionalCostId { get; set; }
        public string AdditionalCostTitle { get; set; }
        public Guid FoodProviderId { get; set; }
        public string FoodProviderTitle { get; set; }
        public string Title { get; set; }
        public long Price { get; set; }
        public bool IsShowRatio { get; set; }
        public double Cost { get; set; }
        public double TotalPrice { get; set; }
    }
}

