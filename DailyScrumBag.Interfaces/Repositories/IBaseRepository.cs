using DailyScrumBag.Interfaces.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DailyScrumBag.Interfaces.Repositories
{
    public interface IBaseRepository : IDisposable
    {

        IUserContext CurrentUser { get; set; }

        Task<int> SaveAync();

    }
}
