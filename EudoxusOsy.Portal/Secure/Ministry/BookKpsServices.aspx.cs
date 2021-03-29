using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using EudoxusOsy.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EudoxusOsy.BusinessModel.Classes.Helpers;
using EudoxusOsy.BusinessModel.Interfaces;

namespace EudoxusOsy.Portal.Secure.Ministry
{
    public partial class BookKpsServices : BaseEntityPortalPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void ReportProgress(int progress)
        {
            lblPRogress.Text = string.Format("{0} % completed \r\n", progress);
        }
        //UPDATE FROM SERVICE
        protected async void btnGetModifiedBooks2_Click(object sender, EventArgs e)
        {
            try
            {
                UpdatePricesFromKPS kps = new UpdatePricesFromKPS(false);

                await Task.Run(() =>
                {
                    txtResults.Text += kps.ManageModifiedBooksStage1();
                    //Notify("Η ενημέρωση των βιβλίων ολοκληρώθηκε επιτυχώς");
                }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Notify(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                txtResults.Text += ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
        }
        
        /// <summary>
        /// The new books get inserted in the OSY database, the bookPrice entry is also created
        /// If the book already exists, then its fields gets updated (not the price) and its status in KPS
        /// is set to "Modified" so that a possible price change will be processed by the GetModifiedBooks algorithm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected async void btnGetNewBooks_Click(object sender, EventArgs e)
        {            
            try
            {
                UpdatePricesFromKPS kps = new UpdatePricesFromKPS(false);

                await Task.Run(() =>
                    {
                    txtResults.Text += kps.ManageNewBooks();
                        //Notify("Η εισαγωγή των βιβλίων ολοκληρώθηκε επιτυχώς");
                    }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Notify(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                txtResults.Text += ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }            
        }

        //UPDATE FROM FILE
        protected async void btnUpdateNewBooksDirectly_Click(object sender, EventArgs e)
        {
            StringBuilder result = new StringBuilder();
            var newBooksResponse = BookServicesClients.GetNewBooks();
            BookRepository bookRepository = new BookRepository(UnitOfWork);
            List<Book> books = new List<Book>();

            result.Append("get new books Started \r\n");
            LogHelper.LogMessage<UpdateBooksHelper>("DIRECT updateNewBooks started");

            await Task.Run(() =>
            {
                newBooksResponse.books.ForEach(x =>
                {
                    var book = bookRepository.FindByBookKpsID(x.id).FirstOrDefault();
                    if (book != null)
                    {
                        result.AppendFormat("book kps: {0}, book osy: {1} found. \r\n", x.id, book.ID);

                        if (Config.EnableKPSUpdate)
                        {

                            UpdateBookStatusRequest updateRequest = new UpdateBookStatusRequest();
                            updateRequest.bsaId = book.ID;
                            updateRequest.id = book.BookKpsID;
                            updateRequest.bsaStatus = "Full";
                            BookServicesClients.UpdateBookStatus(updateRequest);
                            result.AppendFormat("book update in KPS: osyID: {0}, kpsID: {1} \r\n", book.ID, book.BookKpsID);
                        }
                    }
                    else
                    {
                        result.AppendFormat("book kps: {0} NOT found. \r\n", x.id);
                    }
                });
            }).ConfigureAwait(false);

            txtResults.Text = result.ToString();
        }
        protected async void btnUpdateModifiedBooksDirectly_Click(object sender, EventArgs e)
        {
            StringBuilder result = new StringBuilder();
            var modifiedBooksResponse = BookServicesClients.GetModifiedBooks();
            BookRepository bookRepository = new BookRepository(UnitOfWork);
            List<Book> books = new List<Book>();

            result.Append("get modified books Started \r\n");
            LogHelper.LogMessage<UpdateBooksHelper>("DIRECT updateModifiedBooks started");

            await Task.Run(() =>
            {
                modifiedBooksResponse.books.ForEach(x =>
                {
                    var book = bookRepository.FindByBookKpsID((int)x.id).FirstOrDefault();
                    if (book != null)
                    {
                        result.AppendFormat("book kps: {0}, book osy: {1} found. \r\n", x.id, book.ID);

                        if (Config.EnableKPSUpdate)
                        {
                            UpdateBookStatusRequest updateRequest = new UpdateBookStatusRequest();
                            updateRequest.bsaId = book.ID;
                            updateRequest.id = book.BookKpsID;
                            updateRequest.bsaStatus = "Full";
                            BookServicesClients.UpdateBookStatus(updateRequest);
                            result.AppendFormat("book update in KPS: osyID: {0}, kpsID: {1} \r\n", book.ID, book.BookKpsID);
                        }

                    }
                    else
                    {
                        result.AppendFormat("book kps: {0} NOT found. \r\n", x.id);
                    }
                });
            }).ConfigureAwait(false);


            txtResults.Text = result.ToString();
        }
        //---------------------------------------------------------------
        protected async void btnRunStats_OnClick(object sender, EventArgs e)
        {
            StringBuilder result = new StringBuilder();
            result.Append("Refresh Statistics Started \r\n");
            LogHelper.LogMessage<UpdateBooksHelper>("DIRECT refresh of Statistics started");

            await Task.Run(() =>
            {
                using (var ctx = UnitOfWorkFactory.Create())
                {
                    ((DBEntities)ctx).CommandTimeout = 600;
                    ((DBEntities)ctx).CacheStats(0);
                    ((DBEntities)ctx).Rest_PP();
                    ((DBEntities)ctx).SuppliersFullStatistics_PP();
                }
                result.Append("Refresh Statistics Finished \r\n");
            }).ConfigureAwait(false);

            txtResults.Text = result.ToString();
        }

        protected async void btnUpdateModifiedBooks_OnClick(object sender, EventArgs e)
        {
            try
            {
                UpdatePricesFromKPS kps = new UpdatePricesFromKPS(false);

                await Task.Run(() =>
                {
                    txtResults.Text += kps.ManageModifiedBooksStage2();
                    //Notify("Η ενημέρωση των βιβλίων ολοκληρώθηκε επιτυχώς");
                }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Notify(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                txtResults.Text += ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
        }

        protected async void btnUpdateModifiedBooksPrices_OnClick(object sender, EventArgs e)
        {
            try
            {
                UpdatePricesFromKPS kps = new UpdatePricesFromKPS(false);

                await Task.Run(() =>
                {
                    txtResults.Text += kps.ManageModifiedBooksStage3();
                    //Notify("Η ενημέρωση των βιβλίων ολοκληρώθηκε επιτυχώς");
                }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Notify(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                txtResults.Text += ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
        }

        protected async void btnGetSpecificBooks_OnClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBooksList.Text))
            {
                return;
            }
                
            try
            {
                UpdatePricesFromKPS kps = new UpdatePricesFromKPS(false);

                await Task.Run(() =>
                {
                    txtResults.Text += kps.ManageSpecificBooks(txtBooksList.Text);                    
                }).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Notify(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                txtResults.Text += ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            }
        }
    }
}
