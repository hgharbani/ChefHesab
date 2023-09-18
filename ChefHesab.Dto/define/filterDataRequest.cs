using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Dto.define
{
    public class filterDataRequest
    {
        public int  pageNumber { get; set; }
        public int pageSize { get; set; }
     

        public int skip { get
            {
                return (pageNumber - 1) * pageSize;
            } }
        public filterDataRequest() { }
           
    }
}
