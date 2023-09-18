using ChefHesab.Application.Interface;
using ChefHesab.Application.Interface.define;
using ChefHesab.Data.Presentition.Context;
using ChefHesab.Data.Presentition.Reositories.generic;
using ChefHesab.Dto.define.FoodCategory;
using ChefHesab.Dto.define.FoodStuff;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ChefHesab.Dto.define.FoodStuff.SnapFoodStufCategory;

namespace ChefHesab.Domain.Peresentition.IRepositories.define
{
    /// <summary>
    /// دسته بندی غذاها
    /// </summary>
    public class FoodCategoryService : IFoodCategoryService
    {
        public IChefHesabUnitOfWork _unitOfWork;

        public FoodCategoryService(IChefHesabUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddFoodCategory(Rootobject model)
        {
            try
            {
                var getSnapCategory = model.metadata.filter.options.First(a => a.name == "categories");

                var parents = getSnapCategory.data.options.ToList();
                foreach (var parent in parents)
                {
                    var parentitem = new FoodCategory()
                    {
                        Title = parent.title,
                        SnapId = parent.id,
                        Active = true,
                        InverseParent = new List<FoodCategory>()
                    };
                    if (parent.children.Any())
                    {
                        var child1 = parent.children.Where(a => a.children == null).ToList();
                        foreach (var item in child1)
                        {
                            var foodCategory = new FoodCategory()
                            {
                                Title = item.title,
                                SnapId = item.id,
                                Active = true,
                            };
                            parentitem.InverseParent.Add(foodCategory);
                        }

                        var child2 = parent.children.Where(a => a.children != null && a.children.Any())
                                 .SelectMany(a => a.children).ToList();
                        foreach (var item in child2)
                        {
                            var foodCategory = new FoodCategory()
                            {
                                Title = item.title,
                                SnapId = item.id,
                                Active = true,
                            };
                            parentitem.InverseParent.Add(foodCategory);
                        }
                    }
                    _unitOfWork.FoodCategoryRepository.Insert(parentitem);

                }
                await _unitOfWork.SaveAsync();
                return true;



            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public List<FoodCategory> GetAll()
        {
            var foodCategory = new List<int>() {278343,278344,
                278345,278346,278347,278348,
                278349,278350,278352,278355, };
            return _unitOfWork.FoodCategoryRepository.GetAllQueryble().Include(a => a.Parent)
                .Where(a => foodCategory.Contains(a.Parent.SnapId.Value)&&a.ParentId.HasValue).ToList();
        }


    }
}
