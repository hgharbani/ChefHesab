using ChefHesab.Application.Interface;
using ChefHesab.Application.Interface.define;
using ChefHesab.Data.Presentition.Context;
using ChefHesab.Data.Presentition.Reositories.generic;
using ChefHesab.Dto.define.FoodCategory;
using ChefHesab.Dto.define.FoodStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ChefHesab.Dto.define.FoodStuff.SnapFoodStufCategory;

namespace ChefHesab.Domain.Peresentition.IRepositories.define
{
    /// <summary>
    /// مواد اولیه
    /// </summary>
    public class FoodStuffService : IFoodStuffService
    {
        public IChefHesabUnitOfWork _unitOfWork;

        public FoodStuffService(IChefHesabUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
       
        public async Task<bool> AddFoodStuffFromSnap(Rootobject model,Guid? categoryId)
        {
            try
            {
                var result = model.results.ToList();
                
             var listfood=new List<FoodStuff>();
                foreach (var item in result)
                {
                    var brandtitle = item.brand.title;
                    var subtitle = item.subtitle;
                    var foodCategory = new FoodStuff()
                    {
                        Title = item.pureTitle.Replace(brandtitle,"").Trim() ??item.title.Replace(brandtitle, "")
                        .Replace(subtitle, "").Trim(),
                        FoodCategoryId= categoryId

                    };
                    listfood.Add(foodCategory);
                }
                var distinglist = listfood.DistinctBy(a => a.Title).ToList();
                _unitOfWork.FoodStuffRepository.Insert(distinglist);
                var idsave=await _unitOfWork.SaveAsync();
                if (idsave > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
               
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public async Task<List<FoodStuffVM>> GetFoodStuff()
        {
            var result=_unitOfWork.FoodStuffRepository.GetAllQuery().Select(a=>new FoodStuffVM
            {
                Title=a.Title,
                CategoryTitle=a.FoodCategory.Title,
                FoodCategoryId = a.FoodCategoryId,
                Id=a.Id

            }).ToList();

            return result;
        }

    }
}
