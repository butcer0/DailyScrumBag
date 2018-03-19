using DailyScrumBag.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace DailyScrumBag.Repository.Helpers
{
    public static class DSDBContextHelpers
    {
        internal static DSDBContext _db;
        
        static DSDBContextHelpers()
        {

        }
        public static DSDBContext GetDSDBContext()
        {
            return _db;
        }

        public static void SetDSDBContext(DSDBContext db)
        {
            _db = db;
        }
    }
}
