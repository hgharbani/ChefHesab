using ChefHesab.Application.Interface;
using ChefHesab.Application.Interface.define;
using ChefHesab.Data.Presentition.Context;
using ChefHesab.Data.Presentition.Reositories.generic;
using ChefHesab.Domain.Peresentition.IRepositories;
using ChefHesab.Dto.define.FoodCategory;
using ChefHesab.Dto.define.FoodStuff;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChefHesab.Dto.define.FoodStuff;
using ChefHesab.Domain;

namespace ChefHesab.Application.services.define
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
       
        public async Task<bool> AddFoodStuffFromSnap(SnapFoodStufCategory.Rootobject model,long categoryId)
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
               await _unitOfWork.FoodStuffRepository.AddRange(distinglist);
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
            try
            {
                var result = _unitOfWork.FoodStuffRepository.GetAllQuery().Select(a => new FoodStuffVM
                {
                    Title = a.Title,
                    CategoryTitle = a.FoodCategory.Title,
                    FoodCategoryId = a.FoodCategoryId,
                    Id = a.Id

                }).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            

     
        }

    }
}
