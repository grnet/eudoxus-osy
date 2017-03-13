using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using EudoxusOsy.BusinessModel;
using Imis.Domain;

namespace EudoxusOsy.Portal.DataSources
{
    public class Prefectures : BaseDataSource<Prefecture>
    {
        [DataObjectMethod(DataObjectMethodType.Select)]
        public IEnumerable<Prefecture> GetAll()
        {
            return CacheManager.Prefectures.GetItems().Where(x => x.ID != 0);
        }
    }
}
