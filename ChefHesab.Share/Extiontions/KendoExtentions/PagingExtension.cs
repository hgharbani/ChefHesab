using System.Linq;

using ChefHesab.Share.model.KendoModel;

namespace ChefHesab.Share.Extiontions.KendoExtentions
{
    public static class PagingExtension
    {
        public static IQueryable<T> Page<T>(this IQueryable<T> data, int skip, int take)
        {
            return data
                .Skip(skip)
                .Take(take);
        }
    }
}
