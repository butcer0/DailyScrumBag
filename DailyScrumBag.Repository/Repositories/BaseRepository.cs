using DailyScrumBag.Repository.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DailyScrumBag.Repository.Repositories
{
    public class BaseRepository
    {
        #region protected Variables

        private readonly DbContext _dbContext;

        #endregion

        #region Constructor

        public BaseRepository(DbContext dbContext, ILogger logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException("dbContext");
            Logger = logger;
        }

        #endregion
        #region Public / Protected Properties

        public UserContext CurrentUser { get; set; }

        protected ILogger Logger { get; set; }

    
        #endregion

        #region IBaseRepository Implementation

        public virtual async Task<int> SaveAync()
        {
            if (_dbContext != null)
            {
                return await _dbContext.SaveChangesAsync();
            }

            return 0;
        }

        #endregion

        #region IDisposable Implementation

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        #endregion
    }
}
