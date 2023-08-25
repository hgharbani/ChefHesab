using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Dto.define.FoodStuff
{
    public class SnapFoodStufCategory
    {
        public class Rootobject
        {
            public Metadata metadata { get; set; }
            public Result[] results { get; set; }
        }

        public class Metadata
        {
            public Pagination pagination { get; set; }
            public Filter filter { get; set; }
            public Sort sort { get; set; }
            public object query_id { get; set; }
            public object[] breadcrumb { get; set; }
            public bool index { get; set; }
        }

        public class Pagination
        {
            public int total { get; set; }
            public int limit { get; set; }
            public int offset { get; set; }
            public int count { get; set; }
        }

        public class Filter
        {
            public Option[] options { get; set; }
            public Applied applied { get; set; }
        }

        public class Applied
        {
            public string[] categories { get; set; }
            public bool availability { get; set; }
            public bool discount_having { get; set; }
            public Price price { get; set; }
            public string term { get; set; }
            public object[] tags { get; set; }
        }

        public class Price
        {
            public object min { get; set; }
            public object max { get; set; }
        }

        public class Option
        {
            public string name { get; set; }
            public string label { get; set; }
            public string type { get; set; }
            public Data data { get; set; }
        }

        public class Data
        {
            public Option1[] options { get; set; }
            public int min { get; set; }
            public int max { get; set; }
        }

        public class Option1
        {
            public int id { get; set; }
            public string title { get; set; }
            public string slug { get; set; }
            public Child[] children { get; set; }
            public string english_title { get; set; }
        }

        public class Child
        {
            public int id { get; set; }
            public string title { get; set; }
            public string slug { get; set; }
            public Child1[] children { get; set; }
        }

        public class Child1
        {
            public int id { get; set; }
            public string title { get; set; }
            public string slug { get; set; }
        }

        public class Sort
        {
            public Option2[] options { get; set; }
            public string applied { get; set; }
        }

        public class Option2
        {
            public string key { get; set; }
            public string label { get; set; }
        }

        public class Result
        {
            public int id { get; set; }
            public string title { get; set; }
            public string subtitle { get; set; }
            public string description { get; set; }
            public object content { get; set; }
            public int max_order_cap { get; set; }
            public int price { get; set; }
            public int discount_percent { get; set; }
            public int discounted_price { get; set; }
            public bool has_alternative { get; set; }
            public Image[] images { get; set; }
            public Brand brand { get; set; }
            public bool needs_server_approval { get; set; }
            public Tag[] tags { get; set; }
            public int is_bundle { get; set; }
            public object priority { get; set; }
            public object[] badges { get; set; }
            public object[] coupons { get; set; }
            public object[] smartCoupon { get; set; }
            public string bannerColor { get; set; }
            public string bannerDescription { get; set; }
            public string bannerImage { get; set; }
            public string bannerPattern { get; set; }
            public string pureTitle { get; set; }
        }

        public class Brand
        {
            public int id { get; set; }
            public string title { get; set; }
            public string slug { get; set; }
            public string english_title { get; set; }
        }

        public class Image
        {
            public string image { get; set; }
            public string thumb { get; set; }
        }

        public class Tag
        {
            public int id { get; set; }
            public string title { get; set; }
            public object slug { get; set; }
        }

    }
}
