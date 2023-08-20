using ChefHesab.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Application.Interface.define
{
    public interface IPersonalService
    {
        Task<List<Personal>> GetAllProducts();
    }
}
