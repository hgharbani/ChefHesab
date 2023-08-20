using ChefHesab.Data.Presentition.Context;
using ChefHesab.Domain;
using ChefHesab.Domain.Peresentition.IRepositories;
using ChefHesab.Data.Presentition.Reositories.generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChefHesab.Application.Interface.security;

namespace ChefHesab.Data.Presentition.Reositories.define
{
    public class AuthenticateService : IAuthenticateService
    {
        public IChefHesabUnitOfWork _unitOfWork;

        public AuthenticateService(IChefHesabUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void get(int id)
        {

        }

    }
}
