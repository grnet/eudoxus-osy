using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using EudoxusOsy.BusinessModel;

namespace EudoxusOsy.Portal
{
    namespace CacheManagerExtensions
    {
        public static class PrivateExtensions
        {
            public static City GetCity(this int id)
            {
                return CacheManager.Cities.Get(id);
            }

            public static Prefecture GetPrefecture(this int id)
            {
                return CacheManager.Prefectures.Get(id);
            }
        }
    }

    public class CacheManager
    {
        static CacheManager()
        {
            Phases.GetItems();
            Institutions.GetItems();
            Departments.GetItems();
            Cities.GetItems();
            Prefectures.GetItems();
            PublicFinancialOffices.GetItems();
            ApplicationData.GetItems();
        }
        
        private static Lazy<HashSet<string>> _changedProviderUsers = new Lazy<HashSet<string>>(() => new HashSet<string>(StringComparer.OrdinalIgnoreCase), true);
        public static HashSet<string> ChangedProviderUsers
        {
            get
            {
                return _changedProviderUsers.Value;
            }
        }

        public static EudoxusOsyCacheManager<Phase> Phases
        {
            get { return EudoxusOsyCacheManager<Phase>.Current; }
        }

        public static EudoxusOsyCacheManager<Institution> Institutions
        {
            get { return EudoxusOsyCacheManager<Institution>.Current; }
        }

        public static EudoxusOsyCacheManager<Department> Departments
        {
            get { return EudoxusOsyCacheManager<Department>.Current; }
        }

        public static EudoxusOsyCacheManager<City> Cities
        {
            get { return EudoxusOsyCacheManager<City>.Current; }
        }

        public static EudoxusOsyCacheManager<Prefecture> Prefectures
        {
            get { return EudoxusOsyCacheManager<Prefecture>.Current; }
        }

        public static EudoxusOsyCacheManager<PublicFinancialOffice> PublicFinancialOffices
        {
            get { return EudoxusOsyCacheManager<PublicFinancialOffice>.Current; }
        }

        public static EudoxusOsyCacheManager<ApplicationData> ApplicationData
        {
            get { return EudoxusOsyCacheManager<ApplicationData>.Current; }
        }

        #region [ Ordered Lists ]

        public static List<Institution> GetOrderedInstitutions()
        {
            return EudoxusOsyCacheManager<Institution>.Current
                    .GetItems()
                    .Where(x => x.ID != 0)
                    .OrderBy(x => x.Name)
                    .ToList();
        }

        public static List<Department> GetOrderedDepartments(int institutionID)
        {
            return EudoxusOsyCacheManager<Department>.Current
                        .GetItems()
                        .Where(x => x.InstitutionID == institutionID)
                        .OrderBy(x => x.Name)
                        .ToList();
        }

        public static List<Prefecture> GetOrderedPrefectures()
        {
            return EudoxusOsyCacheManager<Prefecture>.Current
                    .GetItems()
                    .Where(x => x.ID != 0)
                    .OrderBy(x => x.Name)
                    .ToList();
        }

        public static List<City> GetOrderedCities(int prefectureID)
        {
            return EudoxusOsyCacheManager<City>.Current
                        .GetItems()
                        .Where(x => x.PrefectureID == prefectureID)
                        .OrderBy(x => x.Name)
                        .ToList();
        }

        public static List<PublicFinancialOffice> GetOrderedPublicFinancialOffices()
        {
            return EudoxusOsyCacheManager<PublicFinancialOffice>.Current
                        .GetItems()                        
                        .OrderBy(x => x.Name)
                        .ToList();
        }

        public static List<Department> GetLibraries()
        {
            return EudoxusOsyCacheManager<Department>.Current
                        .GetItems()
                        .Where(x => x.LibraryKpsID != null)
                        .ToList();
        }

        #endregion

        public static void Initialize()
        {
            Phases.GetItems();
            Institutions.GetItems();
            Prefectures.GetItems();
            Cities.GetItems();
            Departments.GetItems();
            PublicFinancialOffices.GetItems();
        }

        public static void Refresh()
        {
            Phases.Refresh();
            Institutions.Refresh();
            Prefectures.Refresh();
            Cities.Refresh();
            Departments.Refresh();
            PublicFinancialOffices.Refresh();
        }
    }
}
