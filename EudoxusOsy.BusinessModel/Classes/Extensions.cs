using Imis.Domain;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace EudoxusOsy.BusinessModel
{
    public static class Extensions
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static void EnsureLoad(this IRelatedEnd relatedEnd)
        {
            if (!relatedEnd.IsLoaded)
                relatedEnd.Load();
        }

        public static string SubstringByLength(this string s, int length, bool addDots = false)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;

            if (s.Length > length)
            {
                var result = s.Substring(0, length);
                return addDots ? result + "..." : result;
            }
            else
            {
                return s;
            }
        }

        public static Dictionary<string, object> ToDictionary(this NameValueCollection values)
        {
            var dict = new Dictionary<string, object>();
            foreach (string item in values)
            {
                int intValue;
                if (int.TryParse(values[item], out intValue))
                {
                    dict.Add(item, intValue);
                    continue;
                }

                bool boolValue;
                if (bool.TryParse(values[item], out boolValue))
                {
                    dict.Add(item, boolValue);
                    continue;
                }

                double floatValue;
                if (double.TryParse(values[item], out floatValue))
                {
                    dict.Add(item, floatValue);
                    continue;
                }

                Guid guidValue;
                if (Guid.TryParse(values[item], out guidValue))
                {
                    dict.Add(item, guidValue);
                    continue;
                }

                DateTime dtValue;
                if (DateTime.TryParse(values[item], out dtValue))
                {
                    dict.Add(item, dtValue);
                    continue;
                }

                dict.Add(item, values[item]);
            }
            return dict;
        }

        public static InvoiceItemCollection GetInvoicesToCollection(this CatalogGroup group)
        {
            InvoiceItemCollection collection = new InvoiceItemCollection();

            foreach (var invoice in group.Invoices)
            {
                var item = new InvoiceItem()
                {
                    InvoiceID = invoice.ID,
                    InvoiceNumber = invoice.InvoiceNumber,
                    Date = invoice.InvoiceDate,
                    Amount = invoice.InvoiceValue
                };

                collection.Add(item);
            }


            return collection;
        }

        public static decimal? GetSupplierMoneyDebt(this Supplier supplier, int phaseID)
        {
            decimal? debt = 0m;

            debt = new CatalogRepository().GetSupplierMoneyDebt(supplier.ID, phaseID);

            return debt;
        }

        /// <summary>
        /// Get the Catalogs of the current book for the given phase
        /// </summary>
        /// <param name="book"></param>
        /// <param name="phaseID"></param>
        /// <param name="uow"></param>
        /// <returns></returns>
        public static List<BookGroupInfo> GetCatalogsOfBook(this Book book, int phaseID, IUnitOfWork uow)
        {

            var catalogs = new CatalogRepository(uow).FindByBookID(book.ID);

            List<BookGroupInfo> bookInfos = new List<BookGroupInfo>();

            foreach (var catalog in catalogs)
            {
                if (catalog != null && (phaseID == 0 || catalog.PhaseID == phaseID))
                {
                    var sentDate = catalog.GroupID.HasValue && catalog.CatalogGroup.State == enCatalogGroupState.Sent
                        ? catalog.CatalogGroup.PaymentOrders.FirstOrDefault(
                            x => x.IsActive == true && x.OfficeSlipDate.HasValue).OfficeSlipDate
                        : null;

                    var bookInfo = new BookGroupInfo()
                    {
                        BookID = catalog.Book.BookKpsID.ToString(),
                        GroupID = catalog.GroupID.HasValue ? catalog.GroupID.ToString() : string.Empty,
                        Title = catalog.Book.Title,
                        Department = EudoxusOsyCacheManager<Department>.Current.Get(catalog.DepartmentID).Name,
                        Institution =
                            catalog.GroupID.HasValue
                                ? EudoxusOsyCacheManager<Institution>.Current.Get(catalog.CatalogGroup.InstitutionID)
                                    .Name
                                : string.Empty,
                        BookPrice = catalog.BookPrice.Price.ToString("c"),
                        PaymentPrice =
                            (catalog.BookCount > 0 ? (decimal)catalog.Amount / catalog.BookCount : 0m).ToString("c"),
                        Publisher = catalog.Book.Publisher,
                        GroupState = catalog.GroupID.HasValue ? catalog.CatalogGroup.State.GetLabel() : string.Empty,
                        BookCount = catalog.BookCount.ToString(),
                        Year = catalog.Phase.Year.ToString(),
                        PhaseID = catalog.PhaseID.ToString(),
                        SentAt = sentDate.HasValue ? sentDate.Value.ToShortDateString() : string.Empty,
                        SupplierCode = catalog.Book.SupplierCode.ToString()
                    };
                    bookInfos.Add(bookInfo);
                }
            }

            return bookInfos;
        }

        /// <summary>
        /// Computes the specific discountPercentage of a book according to its FirstRegistrationYear
        /// Για κάθε σύγγραμμα πέραν του δεκάτου έκτου εξαμήνου από την πρώτη ανάρτηση στον «ΕΥΔΟΞΟ», υπολογίζεται έκπτωση 2% ανά έτος η οποία δεν μπορεί να υπερβεί το 10% επί της τιμής που ισχύει βάσει του ως άνω πίνακα.
        /// Θυμίζω ότι το ΚΠΣ θα στέλνει το έτος πρώτης ανάρτησης του βιβλίου.Συνεπώς βάσει αυτού θα πρέπει να υπολογίζει το ΟΣΥ πόσα εξάμηνα έχουν παρέλθει από την πρώτη ανάρτηση. 
        /// π.χ.έτος πρώτης ανάρτησης 2010. Το χειμερινό εξάμηνο του 2016-17 το βιβλίο θα βρίσκεται στο 13ο εξάμηνο ενώ στο εαρινό στο 14ο.
        /// </summary>
        /// <param name="book"></param>
        /// <param name="year"></param>
        /// <param name="uow"></param>
        /// <returns></returns>
        public static decimal GetBookDiscount(this Book book, int year)
        {
            decimal perYearDiscountPercentage = 1m;
            if (book.FirstRegistrationYear.HasValue && year > 2015 && year - book.FirstRegistrationYear > 8)
            {
                var yearDifference = year - book.FirstRegistrationYear - 8;
                yearDifference = yearDifference > 5 ? 5 : yearDifference;
                perYearDiscountPercentage = 1m - (0.02m * yearDifference.Value);
            }
            //else if (year < 2015)

            return perYearDiscountPercentage;
        }

        public static CatalogLog CreateCatalogLog(this Catalog catalog, enCatalogLogAction action, string username,
            int userID, int? oldBookCount = null, decimal? oldCatalogPrice = null)
        {
            var catalogLog = new CatalogLog();
            catalogLog.Action = action;
            catalogLog.CreatedAt = DateTime.Today;
            catalogLog.CreatedBy = username;

            if (action != enCatalogLogAction.Delete)
            {
                catalogLog.CatalogID = catalog.ID;
            }

            var operations = new PhpDataObject();
            switch (action)
            {
                case enCatalogLogAction.Create:
                case enCatalogLogAction.Rollback:
                    operations.BookCount = catalog.BookCount.ToString();
                    operations.BookId = catalog.BookID.ToString();
                    operations.BookPriceID = catalog.BookPriceID.Value.ToString();
                    operations.CreatorID = userID.ToString();
                    operations.DepartmentID = catalog.DepartmentID.ToString();
                    operations.DiscountID = catalog.DiscountID.ToString();
                    operations.Percentage = catalog.Percentage.Value.ToString();
                    operations.NewCatalogID = catalog.ID.ToString();
                    operations.PhaseID = catalog.PhaseID.Value.ToString();
                    operations.Price = (-catalog.Amount.Value).ToString();
                    operations.SupplierID = catalog.SupplierID.ToString();
                    operations.CatalogID = catalog.ID.ToString();

                    if (action == enCatalogLogAction.Rollback)
                    {
                        operations.uBookKpsID = catalog.Book.BookKpsID.ToString();
                        operations.uPhaseID = catalog.PhaseID.ToString();
                        operations.uBooksCount = catalog.BookCount.ToString();
                        if (catalog.Department.SecretaryKpsID.HasValue)
                        {
                            operations.uSecretariatKpsID = catalog.Department.SecretaryKpsID.ToString();
                        }
                        operations.uSupplierKpsID = catalog.Supplier.SupplierKpsID.ToString();
                    }

                    break;
                case enCatalogLogAction.Delete:
                    operations.CatalogID = catalog.ID.ToString();
                    operations.CreatorID = userID.ToString();
                    break;
                case enCatalogLogAction.Edit:
                    operations.OldBookCount = oldBookCount.ToString();
                    operations.OldCatalogPrice = oldCatalogPrice;
                    operations.BookWasPriced = catalog.BookPriceID.HasValue ? 1 : 0;
                    operations.UserBookCount = catalog.BookCount.ToString();
                    operations.NewCatalogPrice = catalog.Amount;
                    break;
            }

            catalogLog.Operations = new PhpSerializer().Serialize(operations.GetData());

            return catalogLog;
        }

        public static DataTable ToDataTable<TResult>(this IEnumerable<TResult> value) where TResult : class
        {
            List<PropertyInfo> pList = new List<PropertyInfo>();

            Type type = typeof(TResult);
            DataTable dt = new DataTable();

            Array.ForEach<PropertyInfo>(type.GetProperties(), p =>
            {
                pList.Add(p);
                dt.Columns.Add(p.Name, Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType);
            });

            foreach (var item in value)
            {

                DataRow row = dt.NewRow();

                pList.ForEach(p => row[p.Name] = p.GetValue(item));

                dt.Rows.Add(row);
            }
            return dt;
        }


        public static IList<Invoice> GetPossiblyPaid(this BankTransfer bankTransfer)
        {
            var minDate = bankTransfer.InvoiceDate.AddDays(3);
            var maxDate = bankTransfer.InvoiceDate.AddDays(-3);
            var minCost = bankTransfer.InvoiceValue - 0.001m;
            var maxCost = bankTransfer.InvoiceValue + 0.001m;

            List<Invoice> invoices = new List<Invoice>();

            Criteria<Invoice> criteria = new Criteria<Invoice>();
            criteria.Expression = criteria.Expression.Where(x => x.InvoiceDate, minDate,
                Imis.Domain.EF.Search.enCriteriaOperator.GreaterThan);
            criteria.Expression = criteria.Expression.Where(x => x.InvoiceDate, maxDate,
                Imis.Domain.EF.Search.enCriteriaOperator.LessThan);
            criteria.Expression = criteria.Expression.Where(x => x.InvoiceValue, minCost,
                Imis.Domain.EF.Search.enCriteriaOperator.GreaterThan);
            criteria.Expression = criteria.Expression.Where(x => x.InvoiceValue, maxCost,
                Imis.Domain.EF.Search.enCriteriaOperator.LessThan);
            criteria.Expression = criteria.Expression.Where(x => x.CatalogGroup.SupplierID, bankTransfer.SupplierID);
            criteria.Expression = criteria.Expression.Where(x => x.IsActive, true);
            criteria.Expression = criteria.Expression.Where(x => x.CatalogGroup.IsActive, true);
            criteria.Include(x => x.CatalogGroup);
            criteria.UsePaging = false;

            int resultCount = 0;
            var result = new InvoiceRepository().FindWithCriteria(criteria, out resultCount);

            if (resultCount > 0)
            {
                result.ForEach(x =>
                {
                    if (x.CatalogGroup.State == enCatalogGroupState.Sent)
                    {
                        invoices.Add(x);
                    }
                });
            }

            return invoices;
        }

        public static BankTransfer GetPossiblyTransferred(this Invoice invoice, bool updateCatalogs = false)
        {
            BankTransfer result = null;
            var minDate = invoice.InvoiceDate.AddDays(3);
            var maxDate = invoice.InvoiceDate.AddDays(-3);
            var minCost = invoice.InvoiceValue - 0.001m;
            var maxCost = invoice.InvoiceValue + 0.001m;

            using (IUnitOfWork uow = UnitOfWorkFactory.Create())
            {
                Criteria<BankTransfer> criteria = new Criteria<BankTransfer>();
                criteria.Expression = criteria.Expression.Where(x => x.InvoiceDate, minDate,
                    Imis.Domain.EF.Search.enCriteriaOperator.GreaterThan);
                criteria.Expression = criteria.Expression.Where(x => x.InvoiceDate, maxDate,
                    Imis.Domain.EF.Search.enCriteriaOperator.LessThan);
                criteria.Expression = criteria.Expression.Where(x => x.InvoiceValue, minCost,
                    Imis.Domain.EF.Search.enCriteriaOperator.GreaterThan);
                criteria.Expression = criteria.Expression.Where(x => x.InvoiceValue, maxCost,
                    Imis.Domain.EF.Search.enCriteriaOperator.LessThan);
                criteria.Expression = criteria.Expression.Where(x => x.SupplierID, invoice.CatalogGroup.SupplierID);
                criteria.Expression = criteria.Expression.Where(x => x.IsActive, true);
                criteria.Include(x => x.Bank);
                criteria.Expression = criteria.Expression.Where(x => x.BankID, invoice.CatalogGroup.BankID);
                criteria.Expression = criteria.Expression.Where(x => x.Bank.IsActive, true);
                criteria.UsePaging = false;

                int resultCount = 0;
                result = new BankTransferRepository(uow).FindWithCriteria(criteria, out resultCount).FirstOrDefault();

                if (resultCount > 0 && updateCatalogs && result != null)
                {
                    var currentGroup = invoice.CatalogGroup;
                    if (!currentGroup.IsTransfered)
                    {
                        currentGroup.IsTransfered = true;
                        currentGroup.BankID = result.ID;
                    }
                    uow.Commit();
                }
            }

            return result;
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string ordering, params object[] values)
        {
            if (string.IsNullOrEmpty(ordering))
            {
                ordering = "ID ASC";
            }
            var type = typeof(T);
            var property = type.GetProperty(ordering.Replace("ASC", "").Replace("DESC", "").Trim());
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable),
                ordering.IndexOf(" ASC") > 0 ? "OrderBy" : "OrderByDescending", new Type[] { type, property.PropertyType },
                source.Expression, Expression.Quote(orderByExp));
            return source.Provider.CreateQuery<T>(resultExp);
        }

        public static Catalog Reverse(this Catalog selectedCatalog, BookSupplier bookSupplier)
        {
            var newCatalog = new Catalog();
            newCatalog.BookID = selectedCatalog.BookID;
            newCatalog.BookPriceID = selectedCatalog.BookPriceID;
            newCatalog.SupplierID = selectedCatalog.SupplierID;
            newCatalog.DepartmentID = selectedCatalog.DepartmentID;
            newCatalog.DiscountID = selectedCatalog.DiscountID;
            newCatalog.Amount = -selectedCatalog.Amount;
            newCatalog.BookCount = selectedCatalog.BookCount;
            newCatalog.PhaseID = selectedCatalog.PhaseID;
            newCatalog.CatalogTypeInt = (int)enCatalogType.Reversal;
            newCatalog.OriginalPhaseID = selectedCatalog.OriginalPhaseID;
            newCatalog.State = enCatalogState.FromMove;
            newCatalog.OldCatalogID = selectedCatalog.ID;
            newCatalog.CreatedAt = DateTime.Today;
            newCatalog.CreatedBy = HttpContext.Current.User.Identity.Name;
            newCatalog.Percentage = bookSupplier != null ? bookSupplier.Percentage : selectedCatalog.Percentage;
            newCatalog.Status = enCatalogStatus.Active;
            newCatalog.State = enCatalogState.Normal;
            newCatalog.IsBookActive = selectedCatalog.IsBookActive;
            return newCatalog;
        }

        public static Catalog Move(this Catalog selectedCatalog, int PhaseID)
        {
            var newCatalog = new Catalog();
            newCatalog.BookID = selectedCatalog.BookID;
            newCatalog.BookPriceID = selectedCatalog.BookPriceID;
            newCatalog.SupplierID = selectedCatalog.SupplierID;
            newCatalog.DepartmentID = selectedCatalog.DepartmentID;
            newCatalog.DiscountID = selectedCatalog.DiscountID;
            newCatalog.Amount = selectedCatalog.Amount;
            newCatalog.BookCount = selectedCatalog.BookCount;
            newCatalog.PhaseID = PhaseID;
            newCatalog.CatalogTypeInt = (int)selectedCatalog.CatalogType;
            newCatalog.OriginalPhaseID = selectedCatalog.OriginalPhaseID;
            newCatalog.State = enCatalogState.FromMove;
            newCatalog.OldCatalogID = selectedCatalog.ID;
            newCatalog.CreatedAt = selectedCatalog.CreatedAt;
            newCatalog.CreatedBy = selectedCatalog.CreatedBy;
            newCatalog.Percentage = selectedCatalog.Percentage;
            newCatalog.Status = enCatalogStatus.Active;
            newCatalog.State = enCatalogState.Normal;
            newCatalog.IsBookActive = selectedCatalog.IsBookActive;

            if (selectedCatalog.State == enCatalogState.Normal)
            {
                selectedCatalog.State = enCatalogState.Moved;
            }
            else if (selectedCatalog.State == enCatalogState.FromMove)
            {
                selectedCatalog.State = enCatalogState.Canceled;
            }
            return newCatalog;
        }

        public static decimal RecalculateAmount(this Catalog selectedCatalog, decimal? newPrice)
        {
            return selectedCatalog.BookCount *
                     Math.Round(selectedCatalog.Discount.DiscountPercentage *
                    (newPrice.HasValue ? newPrice.Value : selectedCatalog.BookPrice.Price) *
                    (selectedCatalog.Percentage.Value / 100) *
                    selectedCatalog.Book.FirstRegistrationYearDiscountPercentage(selectedCatalog.PhaseID.Value), 2);
        }

        public static Book ToOsyBook(this BookDTO x)
        {
            var newBook = new Book()
            {
                BookKpsID = x.id,
                Author = x.authors,
                BookType = enBookType.Regular,
                IsActive = x.active,
                Title = x.title,
                Subtitle = x.subtitle,
                ISBN = x.isbn,
                //FirstRegistrationYear
                SupplierCode = x.supplierCode,
                CreatedAt = DateTime.Now,
                CreatedBy = "getNewBooksService"
            };
            return newBook;
        }

        public static Book UpdateBookFromDto(this Book book, BookDTO dto)
        {
            //book.BookType = dto.B,[BookType] dto.kindBook
            book.Title = dto.title;
            book.Subtitle = dto.subtitle;
            book.Author = dto.authors;
            book.Publisher = dto.publisher;
            book.Pages = dto.pages;
            book.ISBN = dto.isbn;
            book.FirstRegistrationYear = dto.firstRegistrationYear.HasValue ? dto.firstRegistrationYear : null;
            book.SupplierCode = dto.supplierCode;
            book.IsActive = dto.active;
            //[PendingCommitteePriceVerification]

            return book;
        }

        public static BookPriceChange BookPriceChangeFromDto(this Book book, BookDTO dto)
        {
            BookPriceChange bookPriceChange = new BookPriceChange();

            bookPriceChange.BookID = book.ID;
            bookPriceChange.SuggestedPrice = dto.suggestedPrice;
            bookPriceChange.Price = dto.price.HasValue ? dto.price.Value : 0m;
            bookPriceChange.DecisionNumber = dto.decisionNumber ?? "default Value";
            bookPriceChange.PriceComments = dto.priceComments ?? string.Empty;
            bookPriceChange.PriceChecked = dto.checkedPrice;

            bookPriceChange.CreatedAt = DateTime.Now;
            bookPriceChange.CreatedBy = "sysadmin";
            return bookPriceChange;
        }

        public static decimal FirstRegistrationYearDiscountPercentage(this Book book, int phaseID)
        {
            var currentPhase = EudoxusOsyCacheManager<Phase>.Current.Get(phaseID);
            if (book.FirstRegistrationYear.HasValue && currentPhase.Year - book.FirstRegistrationYear > 8)
            {
                return 1 - Math.Min(currentPhase.Year - book.FirstRegistrationYear.Value - 8, 5) * 0.02m;
            }
            return 1;
        }

    }
}
