using ChefHesab.Share;

namespace ChefHesab.Data.Presentition.Reositories
{
    public interface IDynamicFormulRepositories
    {
        double CalculateLeaveDays(string formulaString, List<FormulsParametes> formulsParametes);
    }
}