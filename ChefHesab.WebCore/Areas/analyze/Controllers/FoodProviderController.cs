using ChefHesab.Dto.define.FoodStuff;
using ChefHesab.Dto.food.FoodProvider;
using ChefHesab.Share.Extiontions;
using ChefHesab.Share.model;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace ChefHesab.WebCore.Areas.analyze.Controllers
{
    [Area("analyze")]
    public class FoodProviderController : BaseController
    {
        private readonly ApiExtention _apiExtention;
        private readonly IConfiguration _configuration;
        public FoodProviderController(ApiExtention apiExtention, IConfiguration configuration)
        {
            this._apiExtention = apiExtention;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GetAllByKendoFilter([FromBody] SearchfoodProvider request)
        {
            var result = await _apiExtention.PostDataToApiAsync<SearchfoodProvider, dynamic>($"{_configuration["ChefHesabApi"]}api/FoodProviderApi/GetAllByKendoFilter", request);

            return Json(result);
        }

        public IActionResult Create()
        {
            var model = new CreateFoodProviderVM();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm]CreateFoodProviderVM model)
        {

            var result = new ChefResult();
            if (ModelState.IsValid)
            {
                result = await _apiExtention.PutDataToApiAsync<CreateFoodProviderVM, ChefResult>($"{_configuration["ChefHesabApi"]}api/FoodProviderApi/Add", model);
                if (!result.IsSuccess)
                {
                    ErrorNotifications(result.Errors, true, true);
                    return InvokeNotifications(false);
                }
                SuccessNotification("اطلاعات با موفقیت ذخیره شد");

                return InvokeNotifications(true);
            }
            else
            {
                var ListErrors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0).SelectMany(a => a).ToList();
                ErrorNotifications(ListErrors.Select(a => a.ErrorMessage).ToList(), true, true);
                return InvokeNotifications(false);
            }
        }
        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _apiExtention.GetServiceAsync<CreateFoodProviderVM>($"{_configuration["ChefHesabApi"]}api/FoodProviderApi/GetForEdit?id={id}");

            return View(result);
          
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] CreateFoodProviderVM model)
        {

            var result = new ChefResult();
            if (ModelState.IsValid)
            {
                result = await _apiExtention.PutDataToApiAsync<CreateFoodProviderVM, ChefResult>($"{_configuration["ChefHesabApi"]}api/FoodProviderApi/Edit", model);
                if (!result.IsSuccess)
                {
                    ErrorNotifications(result.Errors, true, true);
                    return InvokeNotifications(false);
                }
                SuccessNotification("اطلاعات با موفقیت ذخیره شد");

                return InvokeNotifications(true);
            }
            else
            {
                var ListErrors = ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0).SelectMany(a => a).ToList();
                ErrorNotifications(ListErrors.Select(a => a.ErrorMessage).ToList(), true, true);
                return InvokeNotifications(false);
            }
        }

        public async Task<IActionResult> CostMeal(Guid id)
        {
            var result = await _apiExtention.GetServiceAsync<FoodProviderVM>($"{_configuration["ChefHesabApi"]}api/FoodProviderApi/GetHeaderAnalyzeFood?id={id}");

            return View(result);
        }

        public async Task<IActionResult> HeaderFoodAnalyze(Guid id)
        {
            var result = await _apiExtention.GetServiceAsync<FoodProviderVM>($"{_configuration["ChefHesabApi"]}api/FoodProviderApi/GetForEdit?id={id}");

            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetAnalysicFood([FromForm] SearchfoodProvider request)
        {
            var result = await _apiExtention.PostDataToApiAsync<SearchfoodProvider, List<FoodProviderPrintVm>>($"{_configuration["ChefHesabApi"]}api/FoodProviderApi/GetFoodProviderPrintVms", request);
            return Json(GetExcelFile(result));
        }

        private string GetExcelFile(List<FoodProviderPrintVm> foodProviderPrintVms)
        {
            var company= foodProviderPrintVms.First().Companytitle;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage package = new ExcelPackage())
            {
                
         
  
                foreach (var model in foodProviderPrintVms)
                {
                    var worksheet = package.Workbook.Worksheets.Add(model.CategoryName);
                    worksheet.View.RightToLeft = true;
                    worksheet.Columns[3].Width = 70;
                    worksheet.Cells[1, 1, 1, 7].Merge = true;
                    worksheet.Cells[1, 1, 1, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    worksheet.Cells[1, 1, 1, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 1, 1, 7].Style.Font.Name = "B Titr";
                    worksheet.Cells[1, 1, 1, 7].Style.Fill.PatternType=ExcelFillStyle.Solid;
                    worksheet.Cells[1, 1, 1, 7].Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                    worksheet.Cells[1, 1].Value = " جدول آنالیز منوهای غذایی شرکت "+ company;
                    worksheet.Rows[1].Height = 50;

                    var count = 0;
                    var rowstart = 2;

                    foreach (var food in model.FoodVm)
                    {
                        count++;
                        var adiotionalCount = food.AdditionalcostVm.Count;
                        var stuffPriceCount = food.StuffPricevm.Count;
                        var foodCountRow = rowstart + adiotionalCount + stuffPriceCount +2;
                        worksheet.Cells[rowstart, 1, foodCountRow, 7].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        worksheet.Cells[rowstart, 1, foodCountRow, 7].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        worksheet.Cells[rowstart, 1, foodCountRow, 7].Style.Font.Name = "B Nazanin";

                        worksheet.Cells[rowstart, 1].Value = "ردیف";
                        worksheet.Cells[rowstart, 1].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                        worksheet.Cells[rowstart, 1].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                        worksheet.Cells[rowstart, 1].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                        worksheet.Cells[rowstart, 1].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                        worksheet.Cells[rowstart, 1].Style.Font.Bold = true;

                        worksheet.Cells[rowstart, 2].Value = "نام غذا";
                        worksheet.Cells[rowstart, 2].Style.TextRotation = 90;
                        worksheet.Cells[rowstart, 2].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                        worksheet.Cells[rowstart, 2].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                        worksheet.Cells[rowstart, 2].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                        worksheet.Cells[rowstart, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                        worksheet.Cells[rowstart, 2].Style.Font.Bold = true;

                        worksheet.Cells[rowstart, 3].Value = "مواد اولیه";
                        worksheet.Cells[rowstart, 3].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                        worksheet.Cells[rowstart, 3].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                        worksheet.Cells[rowstart, 3].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                        worksheet.Cells[rowstart, 3].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                      

                        worksheet.Cells[rowstart, 4].Value = "واحد";
                        worksheet.Cells[rowstart, 4].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                        worksheet.Cells[rowstart, 4].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                        worksheet.Cells[rowstart, 4].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                        worksheet.Cells[rowstart, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                        worksheet.Cells[rowstart, 4].Style.Font.Bold = true;

                        worksheet.Cells[rowstart, 5].Value = "مقدار";
                        worksheet.Cells[rowstart, 5].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                        worksheet.Cells[rowstart, 5].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                        worksheet.Cells[rowstart, 5].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                        worksheet.Cells[rowstart, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                        worksheet.Cells[rowstart, 5].Style.Font.Bold = true;

                        worksheet.Cells[rowstart, 6].Value = "فی";
                        worksheet.Cells[rowstart, 6].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                        worksheet.Cells[rowstart, 6].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                        worksheet.Cells[rowstart, 6].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                        worksheet.Cells[rowstart, 6].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                        worksheet.Cells[rowstart, 6].Style.Font.Bold = true;

                        worksheet.Cells[rowstart, 7].Value = "مبلغ نهایی";
                        worksheet.Cells[rowstart, 7].Style.Border.Right.Style = ExcelBorderStyle.Medium;
                        worksheet.Cells[rowstart, 7].Style.Border.Left.Style = ExcelBorderStyle.Medium;
                        worksheet.Cells[rowstart, 7].Style.Border.Top.Style = ExcelBorderStyle.Medium;
                        worksheet.Cells[rowstart, 7].Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                        worksheet.Cells[rowstart, 7].Style.Font.Bold = true;

                        rowstart = rowstart + 1;
                     

                    
                        worksheet.Cells[rowstart,1, foodCountRow, 1].Merge=true;
                        worksheet.Cells[rowstart, 2, foodCountRow, 2].Merge = true;

                

                        worksheet.Cells[rowstart, 1, foodCountRow, 1].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[rowstart, 1, foodCountRow, 1].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[rowstart, 1, foodCountRow, 1].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[rowstart, 1, foodCountRow, 1].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[rowstart, 2, foodCountRow, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[rowstart, 2, foodCountRow, 2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[rowstart, 2, foodCountRow, 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[rowstart, 2, foodCountRow, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[rowstart, 1].Value = count;
                        worksheet.Cells[rowstart, 1].Style.Font.Bold = true;
                        worksheet.Cells[rowstart, 2].Value = food.FoodName;
                        worksheet.Cells[rowstart, 2].Style.Font.Bold = true;
                        worksheet.Cells[rowstart, 2].Style.TextRotation = 90;

                        foreach (var IngredinsFoods in food.StuffPricevm)
                        {
                          
                           
                            worksheet.Cells[rowstart, 3].Value = IngredinsFoods.StuffPriceTitle;
                            worksheet.Cells[rowstart, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[rowstart, 3].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[rowstart, 3].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[rowstart, 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;



                            worksheet.Cells[rowstart, 4].Value = IngredinsFoods.Unit;
                            worksheet.Cells[rowstart, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[rowstart, 4].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[rowstart, 4].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[rowstart, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[rowstart, 5].Value = IngredinsFoods.Amount;
                            worksheet.Cells[rowstart, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[rowstart, 5].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[rowstart, 5].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[rowstart, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[rowstart, 6].Value = IngredinsFoods.Price;
                            worksheet.Cells[rowstart, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[rowstart, 6].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[rowstart, 6].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[rowstart, 6].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;



                            worksheet.Cells[rowstart, 7].Value = IngredinsFoods.TotalPrice;
                            worksheet.Cells[rowstart, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[rowstart, 7].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[rowstart, 7].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[rowstart, 7].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;


                            rowstart = rowstart + 1;

                        }
                        worksheet.Cells[rowstart, 3, rowstart, 6].Merge = true;
                        worksheet.Cells[rowstart, 3, rowstart, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[rowstart, 3, rowstart, 6].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[rowstart, 3, rowstart, 6].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[rowstart, 3, rowstart, 6].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;




                        worksheet.Cells[rowstart, 3].Value = "جمع";
                        worksheet.Cells[rowstart, 3].Style.Font.Bold = true;
                        worksheet.Cells[rowstart, 7].Value = food.StuffPricevm.Sum(a=>a.TotalPrice);
                        worksheet.Cells[rowstart, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[rowstart, 7].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[rowstart, 7].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[rowstart, 7].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[rowstart, 7].Style.Font.Bold = true;
                        foreach (var AdditionalCostFoods in food.AdditionalcostVm)
                        {
                            rowstart = rowstart + 1;
                        
                            worksheet.Cells[rowstart,3, rowstart, 5].Merge = true;
                            worksheet.Cells[rowstart, 3, rowstart, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[rowstart, 3, rowstart, 5].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[rowstart, 3, rowstart, 5].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[rowstart, 3, rowstart, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[rowstart, 3].Value = AdditionalCostFoods.Title;

                            worksheet.Cells[rowstart, 6].Value = AdditionalCostFoods.Price;
                            worksheet.Cells[rowstart, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[rowstart, 6].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[rowstart, 6].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[rowstart, 6].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                            worksheet.Cells[rowstart, 7].Value = AdditionalCostFoods.TotalPrice;
                            worksheet.Cells[rowstart, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[rowstart, 7].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[rowstart, 7].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            worksheet.Cells[rowstart, 7].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        }
                        rowstart = rowstart + 1;
                        worksheet.Cells[rowstart, 3, rowstart, 6].Merge = true;
                        worksheet.Cells[rowstart, 3, rowstart, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[rowstart, 3, rowstart, 6].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[rowstart, 3, rowstart, 6].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[rowstart, 3, rowstart, 6].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[rowstart, 3].Value = "جمع کل هزینه هر پرسیون (ناخالص)";
                        worksheet.Cells[rowstart, 3].Style.Font.Bold = true;

                        worksheet.Cells[rowstart, 7].Value = food.TotalPric;
                        worksheet.Cells[rowstart, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[rowstart, 7].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[rowstart, 7].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[rowstart, 7].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        worksheet.Cells[rowstart, 7].Style.Font.Bold = true;

                       
                        rowstart = rowstart + 10;
                    }
                }
                using (var memoryStream = new MemoryStream())
                {
                    package.SaveAs(memoryStream);
                    string base64 = Convert.ToBase64String(memoryStream.ToArray());
                    return base64; // Send this Base64 string to the client
                }
            }
        }

    }
}

