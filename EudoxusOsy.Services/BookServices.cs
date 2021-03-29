using EudoxusOsy.BusinessModel;
using EudoxusOsy.BusinessModel.Services;
using EudoxusOsy.Services.Models;
using Imis.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace EudoxusOsy.Services
{
    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class BookServices : BaseService
    {
        [WebInvoke(UriTemplate = "putKpsBook", Method = "POST")]
        public ServiceResponse PutKpsBook(Stream finalXml)
        {
            try
            {
                StreamReader sr = new StreamReader(finalXml);
                var book = new Serializer<KpsBookDto>().Deserialize(sr.ReadToEnd());

                BookService.MapFromDto(book);

                LogCall(true, enStatusCode.OK);
                /*
                if (registrationInserted == true)
                {
                    return new ServiceResponse(true, enStatusCode.KpsBooksInsertionSucceeded);
                }
                else
                {
                    return new ServiceResponse(true, enStatusCode.KpsBooksInsertionFailed);
                }
                */
                return null;
            }
            catch (Exception ex)
            {
                return new ServiceResponse(true, enStatusCode.Errors, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }

        [OperationContract]
        [WebInvoke(ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json, UriTemplate = "coauthors", Method = "PUT")]
        public ServiceResponse InsertCoAuthors(CoAuthorsDto request)
        {
            try
            {
                List<Book> lstBook = new BookRepository().FindByBookKpsID((int)request.BookKpsID);
                List<CoAuthorDTO> coAuthorsDto = request.CoAuthors;
                StringBuilder sb = new StringBuilder();

                var currenPhase = new PhaseRepository().GetCurrentPhase();
                int year = currenPhase.Year;

                var coAuthorsInserted = false;

                Book book = lstBook.FirstOrDefault();

                if (book != null)
                {
                    int bookID = book.ID;

                    using (IUnitOfWork uow = UnitOfWorkFactory.Create())
                    {
                        BookSupplierRepository bsRepository = new BookSupplierRepository(uow);

                        List<BookSupplier> bookSuppliers = bsRepository.FindByManyByBookIDAndYear(bookID, year);

                        if (bookSuppliers != null && bookSuppliers.Any())
                        {
                            bookSuppliers.ForEach(x => x.IsActive = false);
                        }

                        foreach (var coAuthor in coAuthorsDto)
                        {
                            var currentSupplier = new SupplierRepository().FindByKpsID(coAuthor.CoAuthorID);
                            if (currentSupplier == null)
                            {
                                // log "suppliers does not exist is OSY"
                                sb.AppendFormat("supplier {0} does not exist in OSY \r\n", coAuthor.CoAuthorID);
                            }
                            else if (coAuthor.Percentage == 0)
                            {
                                //log "percentage for supplier is missing"
                                sb.AppendFormat("percentage is missing for coAuthor {0}  \r\n", coAuthor.CoAuthorID);
                            }
                            else
                            {
                                BookSupplier bookSupplier = new BookSupplier()
                                {
                                    BookID = bookID,
                                    SupplierID = currentSupplier.ID,
                                    Percentage = coAuthor.Percentage,
                                    Year = year,
                                    IsActive = true,
                                    CreatedAt = DateTime.Now,
                                    CreatedBy = "kpsService"
                                };

                                uow.MarkAsNew(bookSupplier);
                            }
                        }

                        uow.Commit();
                        coAuthorsInserted = true;
                    }
                }

                LogCall(true, enStatusCode.OK);

                if (coAuthorsInserted == true)
                {
                    if (sb.Length > 0)
                    {
                        return new ServiceResponse(true, enStatusCode.CoAuthorsInsertionSucceeded, sb.ToString());
                    }

                    return new ServiceResponse(true, enStatusCode.CoAuthorsInsertionSucceeded);
                }
                else
                {
                    if (sb.Length > 0)
                    {
                        return new ServiceResponse(true, enStatusCode.CoAuthorsInsertionFailed, sb.ToString());
                    }

                    return new ServiceResponse(true, enStatusCode.CoAuthorsInsertionFailed);
                }
            }
            catch (Exception ex)
            {
                return new ServiceResponse(true, enStatusCode.Errors, ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }

    }
}
