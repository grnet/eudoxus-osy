using EudoxusOsy.BusinessModel;
using EudoxusOsy.Portal.Controls;
using System;

namespace EudoxusOsy.Portal.Secure.Ministry.EditorPopups
{
    public partial class EditCatalogPopup : BaseSecureEntityPortalPage<Catalog>
    {
        protected int CatalogID
        {
            get
            {
                return Convert.ToInt32(Request.QueryString["id"]);
            }
        }

        protected override void Fill()
        {
            Entity = new CatalogRepository(UnitOfWork).Load(CatalogID, x => x.Book);
        }

        protected override bool Authorize()
        {
            return EudoxusOsyRoleProvider.IsAuthorizedEditorUser();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsAuthorized)
            {
                ClientScript.RegisterStartupScript(GetType(), "closePopup", "window.parent.popUp.hide();", true);
            }

            if (Entity.CatalogType == enCatalogType.Reversal)
            {
                trBookCount.Visible = false;
                trAmount.Visible = true;
            }
        }

        protected void btnSubmitHidden_Click(object sender, EventArgs e)
        {
            int oldBookCount = Entity.BookCount;
            decimal? oldCatalogPrice = Entity.Amount;
            int newBookCount = Convert.ToInt32(txtBookCount.Value);
            decimal? newCatalogPrice = Convert.ToDecimal(txtAmount.Value);

            if (newBookCount > 0 && oldBookCount != newBookCount)
            {
                Entity.BookCount = newBookCount;
                RecalculateAmount();
            }
            else if (trAmount.Visible && oldCatalogPrice != newCatalogPrice)
            {
                if (newCatalogPrice > 0)
                {
                    newCatalogPrice = -newCatalogPrice;
                }
                Entity.Amount = newCatalogPrice;
            }

            UnitOfWork.Commit();

            var catalogLog = Entity.CreateCatalogLog(enCatalogLogAction.Edit, User.Identity.Name, User.Identity.ReporterID, oldBookCount, oldCatalogPrice);
            UnitOfWork.MarkAsNew(catalogLog);

            UnitOfWork.Commit();

            ClientScript.RegisterStartupScript(GetType(), "closePopup", "window.parent.cmdRefresh();window.parent.popUp.hide();", true);
        }

        private void RecalculateAmount()
        {
            if (Entity.BookPriceID.HasValue)
            {
                var bookPrice = new BookPriceRepository(UnitOfWork).Load(Entity.BookPriceID.Value);

                Entity.Amount = Entity.RecalculateAmount(bookPrice.Price);

                if (Entity.CatalogType == enCatalogType.Reversal)
                {
                    Entity.Amount = -Entity.Amount;
                }
            }
        }
    }
}