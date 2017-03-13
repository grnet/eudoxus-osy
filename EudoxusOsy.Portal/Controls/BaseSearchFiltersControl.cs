using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace EudoxusOsy.Portal.Controls
{
    public abstract class BaseSearchFiltersControl<TFilters> : UserControl
    {
        public abstract TFilters GetSearchFilters();

        public abstract void SetSearchFilters(TFilters filters);
    }
}