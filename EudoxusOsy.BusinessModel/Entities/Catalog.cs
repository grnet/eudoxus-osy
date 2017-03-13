using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EudoxusOsy.BusinessModel
{
    public partial class Catalog
    {
        public enCatalogState State
        {
            get { return (enCatalogState)StateInt; }
            set
            {
                if (StateInt != (int)value)
                    StateInt = (int)value;
            }
        }

        public enCatalogStatus Status
        {
            get { return (enCatalogStatus)StatusInt; }
            set
            {
                if (StatusInt != (int)value)
                    StatusInt = (int)value;
            }
        }

        public bool IsDeleteAllowed
        {
            get
            {
                if (GroupID == null && Status == enCatalogStatus.Active && CatalogType != enCatalogType.Automatic)
                {
                    return true;
                }
                return false;
            }
        }

        public enCatalogType CatalogType
        {
            get
            {
                return (enCatalogType)CatalogTypeInt;
            }
            set
            {
                if(CatalogTypeInt != (int)value)
                {
                    CatalogTypeInt = (int)value;
                }
            }
        }

        public string BookTitle
        {
            get
            {
                return Book.Title;
            }
        }

        public string BookAuthor
        {
            get
            {
                return Book.Author;
            }
        }

        public string DepartmentName
        {
            get
            {
                return Department.Name;
            }
        }
    }
}
