using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Ksc.HR.DTO.Define.excel
{
    public enum AnalysisMode
    {
        [Display(Name = "تحلیل تک فایل")]
        SingleFile,

        [Display(Name = "مقایسه دو فایل")]
        CompareFiles,

        [Display(Name = "تحلیل ستون خاص")]
        ColumnAnalysis
    }

    public enum DuplicateDetectionMode
    {
        [Display(Name = "مقادیر تکراری")]
        DuplicateValues,

        [Display(Name = "مقادیر یکتا")]
        UniqueValues
    }

    public class AnalysisSettings
    {
        [Required(ErrorMessage = "لطفاً نوع تحلیل را انتخاب کنید")]
        [Display(Name = "نوع تحلیل")]
        public AnalysisMode Mode { get; set; } = AnalysisMode.SingleFile;

        // تنظیمات فایل اول
        [Required(ErrorMessage = "لطفاً فایل اول را انتخاب کنید")]
        [Display(Name = "فایل اول")]
        public IFormFile PrimaryFile { get; set; }

        //[Display(Name = "شیت‌های فایل اول")]
        //public List<SheetSelection> PrimaryFileSheets { get; set; } = new List<SheetSelection>();

        // تنظیمات فایل دوم (در حالت مقایسه)
        [Display(Name = "فایل دوم")]
        public IFormFile SecondaryFile { get; set; }

        //[Display(Name = "شیت‌های فایل دوم")]
        //public List<SheetSelection> SecondaryFileSheets { get; set; } = new List<SheetSelection>();

        // تنظیمات تحلیل ستون خاص
        [Display(Name = "ستون مورد نظر")]
        public string TargetColumn { get; set; }

        [Display(Name = "نوع تشخیص")]
        public DuplicateDetectionMode DetectionMode { get; set; } = DuplicateDetectionMode.DuplicateValues;

        // تنظیمات هایلایت
        [Display(Name = "تنظیمات هایلایت")]
        public HighlightOptions HighlightOptions { get; set; } = new HighlightOptions();

    
        public List<SheetSelection> PrimaryFileSheets { get; set; } = new List<SheetSelection>();

    
        public List<SheetSelection> SecondaryFileSheets { get; set; } = new List<SheetSelection>();
    }

    public class SheetSelection
    {
        public string SheetName { get; set; }
        public string ColumnName { get; set; }
        public bool IsSelected { get; set; }
    }

    public class HighlightOptions
    {
        [Display(Name = "هایلایت در فایل اول")]
        public bool HighlightPrimary { get; set; } = true;

        [Display(Name = "هایلایت در فایل دوم")]
        public bool HighlightSecondary { get; set; } = true;

        [Display(Name = "رنگ هایلایت")]
        public string HighlightColor { get; set; } = "Yellow";

        [Display(Name = "رنگ هایلایت دوم")]
        public string SecondaryHighlightColor { get; set; } = "Pink";
    }

    public class AnalysisResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<DuplicateItem> Duplicates { get; set; } = new List<DuplicateItem>();
        public List<UniqueItem> UniqueValues { get; set; } = new List<UniqueItem>();
        public string OriginalFileName { get; set; }
        public string ComparedFileName { get; set; }
        public AnalysisSettings Settings { get; set; }
    }

    public class DuplicateItem
    {
        public string Value { get; set; }
        public int Count { get; set; }
        public List<DataLocation> Locations { get; set; } = new List<DataLocation>();
    }

    public class UniqueItem
    {
        public string Value { get; set; }
        public DataLocation Location { get; set; }
    }

    public class DataLocation
    {
        public string FileName { get; set; }
        public string SheetName { get; set; }
        public string ColumnName { get; set; }
        public string CellAddress { get; set; }
        public int RowNumber { get; set; }
        public bool IsPrimaryFile { get; set; }
    }
}
