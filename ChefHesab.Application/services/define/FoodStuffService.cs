using AutoMapper;
using ChefHesab.Application.Interface.define;
using ChefHesab.Domain;
using ChefHesab.Domain.Peresentition.IRepositories;
using ChefHesab.Dto.define.FoodStuff;
using ChefHesab.Dto.food.StuffPrice;
using ChefHesab.Share.Extiontions.KendoExtentions;
using ChefHesab.Share.model;
using ChefHesab.Share.model.KendoModel.Response;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using OfficeOpenXml;
using System.Data.SqlClient;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.IO;
using System.Linq.Dynamic;
using ChefHesab.Dto.define.ContractingCompany;

namespace ChefHesab.Application.services.define
{
    /// <summary>
    /// مواد اولیه
    /// </summary>
    public class FoodStuffService : IFoodStuffService
    {
        public IChefHesabUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public FoodStuffService(IChefHesabUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public dynamic GetFoodStuffPriceWithCompany(FoodStuffSearch request)

        {
            var FoodStuff = _unitOfWork.FoodStuffRepository.GetAllQuery().AsNoTracking();
            if (request.FoodCategoryId.HasValue&& request.FoodCategoryId.Value>0)
            {
                var foodCategoryId = request.FoodCategoryId.Value;
                FoodStuff = FoodStuff.Where(a => a.FoodCategoryId == foodCategoryId);
            }

          var  query= FoodStuff.Select(a => new
                {
                    a.Id,
                    a.Title,
                    FoodCategoryTitle= a.FoodCategory.Title,
                    a.FoodCategoryId,
                    StuffPrices = a.StuffPrices.Where(c => c.CompanyId == request.CompanyId)
                }).OrderBy(a=>a.FoodCategoryTitle);
          
            int total = query.Count();
            IList resultData;
            bool isGrouped = false;

            var aggregates = new Dictionary<string, Dictionary<string, string>>();
            var data = query.Select(a => new FoodStuffVM()
            {
                Id = a.Id,
                Title = a.Title,
                Price = a.StuffPrices.Any() ? a.StuffPrices.FirstOrDefault().Price : 0,
                AmountPercent = a.StuffPrices.Any() ? a.StuffPrices.FirstOrDefault().AmountPercent : 0,
                TotalPrice = a.StuffPrices.Any() ? a.StuffPrices.FirstOrDefault().TotalPrice : 0,
                StuffPricesId = a.StuffPrices.Any() ? a.StuffPrices.FirstOrDefault().Id : null,
                FoodCategoryTitle = a.FoodCategoryTitle,
                FoodCategoryId = a.FoodCategoryId.Value,
                CompanyId= request.CompanyId.Value,
               
            });
            if (request.Sorts != null)
            {
                data = data.Sort(request.Sorts);
            }

            if (request.Filter != null)
            {
                data = data.Filter(request.Filter);
                total = data.Count();
            }

            if (request.Aggregates != null)
            {
                aggregates = data.CalculateAggregates(request.Aggregates);
            }

            if (request.Take > 0)
            {
                data = data.Page(request.Skip, request.Take);
            }

            if (request.Groups != null && request.Groups.Count > 0 && !request.GroupPaging)
            {
                resultData = data.Group(request.Groups).Cast<Group>().ToList();
                isGrouped = true;
            }
            else
            {
                resultData = data.ToList();
            }

            var result = new Response(resultData, aggregates, total, isGrouped).ToResult();
            return result;
        }


        public dynamic GetFoodStuffWithCategory(FoodfSearch request)

        {
            var FoodStuff = _unitOfWork.FoodStuffRepository.GetAllQuery().AsNoTracking();
            if (request.FoodCategoryId.HasValue && request.FoodCategoryId.Value > 0)
            {
                var foodCategoryId = request.FoodCategoryId.Value;
                FoodStuff = FoodStuff.Where(a => a.FoodCategoryId == foodCategoryId);
            }

            var query = FoodStuff.Select(a => new
            {
                a.Id,
                a.Title,
                FoodCategoryTitle = a.FoodCategory.Title,
                a.FoodCategoryId,
            }).OrderBy(a => a.FoodCategoryTitle);

            int total = query.Count();
            IList resultData;
            bool isGrouped = false;

            var aggregates = new Dictionary<string, Dictionary<string, string>>();
            var data = query.Select(a => new FoodStuffVM()
            {
                Id = a.Id,
                Title = a.Title,
                FoodCategoryTitle = a.FoodCategoryTitle,
                FoodCategoryId = a.FoodCategoryId.Value,

            });
            if (request.Sorts != null)
            {
                data = data.Sort(request.Sorts);
            }

            if (request.Filter != null)
            {
                data = data.Filter(request.Filter);
                total = data.Count();
            }

            if (request.Aggregates != null)
            {
                aggregates = data.CalculateAggregates(request.Aggregates);
            }

            if (request.Take > 0)
            {
                data = data.Page(request.Skip, request.Take);
            }

            if (request.Groups != null && request.Groups.Count > 0 && !request.GroupPaging)
            {
                resultData = data.Group(request.Groups).Cast<Group>().ToList();
                isGrouped = true;
            }
            else
            {
                resultData = data.ToList();
            }

            var result = new Response(resultData, aggregates, total, isGrouped).ToResult();
            return result;
        }



        private async Task<bool> IsDuplicate(CreateFoodStuffVM contractingCompany)
        {
            if (!string.IsNullOrEmpty(contractingCompany.Id.ToString()))
            {
                return await _unitOfWork.FoodStuffRepository.Any(a => a.Id != contractingCompany.Id && a.Title == contractingCompany.Title
                && a.FoodCategoryId == contractingCompany.FoodCategoryId
               );

            }
            else
            {
                return await _unitOfWork.FoodStuffRepository.Any(a => a.Title == contractingCompany.Title && a.FoodCategoryId == contractingCompany.FoodCategoryId
                );
            }

        }

        public async Task<ChefResult> Add(CreateFoodStuffVM model)
        {
            var result = new ChefResult();
            try
            {

                if (await IsDuplicate(model))
                {
                    result.AddError("داده وارد شده تکراری می باشد");
                    return result;
                }
               model.Id = Guid.NewGuid();
                var mapper = _mapper.Map<FoodStuff>(model);
          
               await _unitOfWork.FoodStuffRepository.AddAsync(mapper);

                var idsave = await _unitOfWork.SaveAsync();
                return result;
            }
            catch (Exception ex)
            {
                result.AddError("عملیات نا موفق بود");
                return result;
            }

        }
        public async Task<ChefResult> Edit(CreateFoodStuffVM model)
        {
            var result = new ChefResult();
            try
            {

                if (await IsDuplicate(model))
                {
                    result.AddError("داده وارد شده تکراری می باشد");
                    return result;
                }
                var find = _unitOfWork.FoodStuffRepository.Select(a => a.Id == model.Id);
                if (find == null)
                {
                    result.AddError("داده مورد نظر حذف شده است");
                    return result;
                }

                var mapper = _mapper.Map<CreateFoodStuffVM, FoodStuff>(model, find);
               _unitOfWork.FoodStuffRepository.Update(mapper);
                await _unitOfWork.SaveAsync();
                return result;
            }
            catch (Exception ex)
            {
                result.AddError("عملیات نا موفق بود");
                return result;
            }

        }
        public async Task<bool> AddFoodStuffFromSnap(SnapFoodStufCategory.Rootobject model, long categoryId)
        {
            try
            {
                var result = model.results.ToList();

                var listfood = new List<FoodStuff>();
                foreach (var item in result)
                {
                    var brandtitle = item.brand.title;
                    var subtitle = item.subtitle;
                    var foodCategory = new FoodStuff()
                    {
                        Title = item.pureTitle.Replace(brandtitle, "").Trim() ?? item.title.Replace(brandtitle, "")
                        .Replace(subtitle, "").Trim(),
                        FoodCategoryId = categoryId

                    };
                    listfood.Add(foodCategory);
                }
                var distinglist = listfood.DistinctBy(a => a.Title).ToList();
                await _unitOfWork.FoodStuffRepository.AddRange(distinglist);
                var idsave = await _unitOfWork.SaveAsync();
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
                    FoodCategoryId = a.FoodCategoryId.Value,
                    Id = a.Id

                }).ToList();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }



        }

        public async Task<CreateFoodStuffVM> GetFoodStuffForEdit(Guid id)
        {
            try
            {
                var result = _unitOfWork.FoodStuffRepository.GetAllQuery().Where(a=>a.Id==id).Select(a => new CreateFoodStuffVM
                {
                    Title = a.Title,
                    FoodCategoryId = a.FoodCategoryId.Value,
                    Id = a.Id

                }).FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }



        }

        public async Task<ChefResult> GetExcelFileAndSaveInSql()
        {
            var result = new ChefResult();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            var foodCategories=new List<FoodCategory>();
            using (var package = new ExcelPackage(new FileInfo(@"D:/analys.xlsx")))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                try
                {
                   for (int i = 1; i <= worksheet.Columns.EndColumn; i = i +5)
                    {
                        if (worksheet.Cells[1, i ].Value==null) break;

                        if (string.IsNullOrEmpty(worksheet.Cells[1, i].Value.ToString())) break;
                        #region foodCategory
                        var foodCategory = new FoodCategory();
                       
                        foodCategory.Title = worksheet.Cells[1, i].Value.ToString();
                        foodCategory.Active = true;
                        foodCategory.ParentId = 1;
                        foodCategory.CategoryId=new Random().Next();    
                        #endregion
                        #region foodStuff
                        for (int row = 3; row <= worksheet.Dimension.Rows; row++)
                        {
                            var foodstuff=new FoodStuff();
                            if (worksheet.Cells[row, i+1].Value==null) break;
                            if (worksheet.Cells[row, i + 1].Value.ToString()=="") break;
                            // Read values from Excel
                            foodstuff.Title = worksheet.Cells[row, i + 1].Value.ToString();
                            foodCategory.FoodStuffs.Add(foodstuff);
                        }
                        foodCategories.Add(foodCategory);

                        #endregion
                        // Iterate through the rows and columns

                    } 
                    
               await _unitOfWork.FoodCategoryRepository.AddRange(foodCategories);
                var id=  _unitOfWork.SaveAsync();
                   
                }
                catch (Exception ex) {

                    return result;
                }
                return result;
            }
        }
    }
}

