using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Caching;

namespace AngularJSAuthentication.API.Controllers
{
    public class EnterpriseCache
    {

        private static ICacheManager instance;

        private EnterpriseCache() { }

        public static ICacheManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = CacheFactory.GetCacheManager();
                }
                return instance;
            }
        }
    }
}