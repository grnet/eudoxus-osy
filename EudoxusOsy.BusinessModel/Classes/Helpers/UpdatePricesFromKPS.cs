using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EudoxusOsy.BusinessModel.Interfaces;
using EudoxusOsy.Utils;
using iTextSharp.text;
using Imis.Domain;
using Org.BouncyCastle.Asn1.Cms;

namespace EudoxusOsy.BusinessModel.Classes.Helpers
{
    public class UpdatePricesFromKPS
    {
        private bool _isDemo;

        public UpdatePricesFromKPS(bool isDemo)
        {
            _isDemo = isDemo;
        }

        public string ManageNewBooks()
        {   
            StringBuilder sb = new StringBuilder();
            GetBooksResponse booksResponse = GetNewData(_isDemo);

            if (booksResponse != null && booksResponse.books != null)
            {
                sb.AppendLine("Συνολικά νέα βιβλία από ΚΠΣ : " + booksResponse.books.Count);

                using (IUnitOfWork uow = UnitOfWorkFactory.Create())
                {
                    UpdateBooksHelper.ProcessNewBooksResponse(uow, sb, booksResponse);
                }
            }
            else
            {
                sb.Append("Σφάλμα στην επικοινωνία με το ΚΠΣ");
            }
            /*
            //GenerateBookChangesReport(UpdatedPriceBooks)
            var reportEmail = EmailFactory.GetServiceBookChangesReport("Ενημέρωση τιμής από ΚΠΣ", UpdateBooksHelper.GenerateBookChangesReport(null));
            uow.MarkAsNew(reportEmail);
            uow.Commit();
            EmailQueueWorker.Current.AddEmailDispatchToQueue(reportEmail.ID);
            */

            return sb.ToString();
        }

        public string ManageSpecificBooks(string booksList)
        {
            bool ok;
            StringBuilder sb = new StringBuilder();

            List<Book> updatedBooks = null;
            List<Archive> updatedBookPrices = null;

            //Stage 1
          
            var specificBooksResponse = BookServicesClients.GetSpecificBooks(booksList);
            
            ok = specificBooksResponse != null;
            
            if (ok && specificBooksResponse.books != null)
            {
                sb.AppendLine("Συνολικά βιβλία από ΚΠΣ : " + specificBooksResponse.books.Count);
                
                var bookTempArchives = specificBooksResponse.books.SelectMany(x => x.archives)
                    .Where(x => x.year > 2015);
                ok = SaveDataToArchive(bookTempArchives.ToList());
                sb.Append("Stage 1 COMPLETED\n");

                ok = UpdateData(specificBooksResponse, sb, out updatedBooks);
                sb.Append("Stage 2 COMPLETED\n");

                ok = UpdatePrices(sb, out updatedBookPrices);
                sb.Append("Stage 3 COMPLETED\n");
            }
            else
            {
                sb.Append("Σφάλμα στην επικοινωνία με το ΚΠΣ (Stage 1)\n");                
            }
            
            return sb.ToString();
        }

        public string ManageModifiedBooksStage1()
        {
            bool ok;
            StringBuilder sb = new StringBuilder();

            List<Book> updatedBooks = null;
            List<Archive> updatedBookPrices = null;

            //Stage 1
            //Get Data from service and save them to local file
            GetBooksResponse booksResponse = GetModifiedData(_isDemo);
            ok = booksResponse != null;
            
            if (ok && booksResponse.books != null)
            {
                sb.AppendLine("Συνολικά βιβλία από ΚΠΣ : " + booksResponse.books.Count);
                
                var bookTempArchives = booksResponse.books.SelectMany(x => x.archives)
                    .Where(x => x.year > 2015);
                ok = SaveDataToArchive(bookTempArchives.ToList());
                sb.Append("Stage 1 COMPLETED\n");
            }
            else
            {
                sb.Append("Σφάλμα στην επικοινωνία με το ΚΠΣ (Stage 1)\n");                
            }
            
            return sb.ToString();
        }
        public string ManageModifiedBooksStage2()
        {
            bool ok;
            StringBuilder sb = new StringBuilder();

            List<Book> updatedBooks = null;
            List<Archive> updatedBookPrices = null;

            GetBooksResponse booksResponse = GetModifiedData(_isDemo);
            ok = booksResponse != null;
            //Stage 2
            //Update books data only (NOT Prices)
            if (ok)
            {
                ok = UpdateData(booksResponse, sb, out updatedBooks);
                sb.Append("Stage 2 COMPLETED\n");
            }
            else
            {
                sb.Append("Σφάλμα κατά την ενημέρωση δεδομένων των βιβλίων (Stage 2)\n");               
            }

            return sb.ToString();
        }
        public string ManageModifiedBooksStage3()
        {
            bool ok;
            StringBuilder sb = new StringBuilder();

            List<Book> updatedBooks = null;
            List<Archive> updatedBookPrices = null;

           
            //Stage 2
            //Update Book Prices            
            ok = UpdatePrices(sb, out updatedBookPrices);


            if (ok)
            {
                sb.Append("Stage 3 COMPLETED\n");
            }
            else
            {
                sb.Append("Σφάλμα κατά την ενημέρωση τιμών των βιβλίων (Stage 3)\n");
            }

            /*
             * updatedBooks
             * updatedBooksPrices
            //GenerateBookChangesReport(UpdatedPriceBooks)
            var reportEmail = EmailFactory.GetServiceBookChangesReport("Ενημέρωση τιμής από ΚΠΣ", UpdateBooksHelper.GenerateBookChangesReport(null));
            uow.MarkAsNew(reportEmail);
            uow.Commit();
            EmailQueueWorker.Current.AddEmailDispatchToQueue(reportEmail.ID);
            */

            return sb.ToString();
        }
        public GetBooksResponse GetNewData(bool isDemo)
        {

            GetBooksResponse booksResponse = new GetBooksResponse(); ;
            GetBooksResponseSlim booksResponseSlim = null;

            if (!isDemo && BookServicesClients.GetNewBooksToLocalFile())
            {
                try
                {                    
                    booksResponseSlim = BookServicesClients.GetBooksFromFile(Config.SystemNewBooksFile);
                }
                catch (Exception ex)
                {
                    LogHelper.LogError(ex, typeof(UpdatePricesFromKPS), "GetNewData " + ((ex.InnerException != null ? ex.InnerException.Message != null : false) ? ex.InnerException.Message : ex.Message));
                }
                
            }
            else if (isDemo)
            {                
                booksResponseSlim = BookServicesClients.GetBooksFromFile(Config.ManualNewBooksFile);
            }

            if (booksResponseSlim != null && booksResponseSlim.books != null)
            {
                booksResponse.numResults = booksResponseSlim.numResults;
                booksResponse.books = new List<BookDTO>();
                booksResponseSlim.books.ForEach(x => booksResponse.books.Add(x.ToBookDTO()));
            }

            return booksResponse;
        }
        public GetBooksResponse GetModifiedData(bool isDemo)
        {
            GetBooksResponse booksResponse = new GetBooksResponse();
            GetBooksResponseSlim booksResponseSlim = null;

            if (!isDemo && BookServicesClients.GetModifiedBooksToLocalFile())
            {
                try
                {
                    booksResponseSlim = BookServicesClients.GetBooksFromFile(Config.SystemModifiedBooksFile);
                }
                catch (Exception ex)
                {
                    LogHelper.LogError(ex, typeof(UpdatePricesFromKPS),
                        "GetModifiedData " + ((ex.InnerException != null ? ex.InnerException.Message != null : false)
                            ? ex.InnerException.Message
                            : ex.Message));
                }
            }
            else if(isDemo)
            {
                booksResponseSlim = BookServicesClients.GetBooksFromFile(Config.ManualModifiedBooksFile);
            }

            if (booksResponseSlim!= null && booksResponseSlim.books != null )
            {
                booksResponse.numResults = booksResponseSlim.numResults;
                booksResponse.books = new List<BookDTO>();
                booksResponseSlim.books.ForEach(x => booksResponse.books.Add(x.ToBookDTO()));
            }
            
            return booksResponse;
        }
        public bool UpdateData(IGetBookResponse booksResponse, StringBuilder sb, out List<Book> updatedBooks)
        {
            updatedBooks = new List<Book>();

            bool ok = false;            
            string stringResult = " \r\n";

            try
            {
                using (IUnitOfWork uow = UnitOfWorkFactory.Create())
                {                                      
                    if (booksResponse != null && booksResponse.books != null && booksResponse.books.Count > 0)
                    {
                        List<BookDTO> booksForUpdate = booksResponse.books;
                        
                        int i = 0;
                        while (i * 5000 < booksForUpdate.Count)
                        {
                            var limitedBooksForUpdate = booksForUpdate.Skip((i + 1) * 5000 - 5000).Take(5000).ToList();
                            List<int> udpateIds = limitedBooksForUpdate.Select(y => y.id).ToList();
                            List<Book> books = new BookRepository(uow).LoadAll().Where(x => udpateIds.Contains(x.BookKpsID)).ToList();
                            
                            books.ForEach(x =>
                            {
                                var book = limitedBooksForUpdate.FirstOrDefault(y => y.id == x.BookKpsID);
                                x.UpdateBookFromDto(book);
                            });
                            uow.Commit();

                            updatedBooks.AddRange(books);
                            i++;
                        }                                                                
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex, typeof(UpdatePricesFromKPS),
                    (ex.InnerException != null ? ex.InnerException.Message != null : false)
                        ? ex.InnerException.Message
                        : ex.Message);
            }
            sb.Append(stringResult);

            return ok;
        }
        public bool UpdatePrices(StringBuilder sb, out List<Archive> updatedArchives)
        {
            updatedArchives = new List<Archive>();
            bool ok = false;
            string stringResult = "";

            try
            {
                using (IUnitOfWork uow = UnitOfWorkFactory.Create())
                {
                    List<Archive> archivesForProcess = new ArchiveRepository(uow).GetActive();

                    if (archivesForProcess != null && archivesForProcess.Count > 0)
                    {
                        int i = 0;
                        while (i * 5000 < archivesForProcess.Count)
                        {
                            var limitedArchives = archivesForProcess.Skip((i + 1) * 5000 - 5000).Take(5000).ToList();

                            stringResult += UpdateBooksHelper.ProcessModifiedBooksResponse(uow, limitedArchives);
                            updatedArchives.AddRange(limitedArchives);
                            i++;
                        }
                    }
                }
                ok = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex, typeof(UpdatePricesFromKPS), (ex.InnerException != null ? ex.InnerException.Message != null : false) ? ex.InnerException.Message : ex.Message);
            }
            sb.Append(stringResult);

            return ok;                                                        
        }
        public bool SaveDataToArchive(List<ArchiveDTO> archiveDtos)
        {
            bool ok = false;
            
            try
            {   
                using (IUnitOfWork uow = UnitOfWorkFactory.Create())
                {                                                            
                    int i = 0;
                    foreach (var archiveDTO in archiveDtos)
                    {                        
                        var newArchive = new TempArchive()
                        {
                            BookID = archiveDTO.bookId,
                            CheckedPrice = archiveDTO.checkedPrice,
                            DecisionNumber = archiveDTO.decisionNumber,
                            KPSArchiveID = archiveDTO.id,
                            LastUpdate = archiveDTO.lastUpdate,
                            Price = archiveDTO.price,
                            PriceComments = archiveDTO.priceComments,
                            SuggestedPrice = archiveDTO.suggestedPrice,
                            Fek = archiveDTO.fek,
                            Year = archiveDTO.year
                        };
                        uow.MarkAsNew(newArchive);
                        i++;

                        if (i == 5000)
                        {
                            uow.Commit();
                            i = 0;
                        }
                    }
                    uow.Commit();

                }

                using (var ctx = UnitOfWorkFactory.Create())
                {
                    ((DBEntities)ctx).CommandTimeout = 600;
                    ((DBEntities)ctx).FillArchiveFromTempArchive();
                }
                ok = true;
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex, typeof(UpdatePricesFromKPS), "SaveDataToArchive " + ( (ex.InnerException != null ? ex.InnerException.Message != null : false) ? ex.InnerException.Message : ex.Message));
            }

            return ok;
        }                
    }
}
