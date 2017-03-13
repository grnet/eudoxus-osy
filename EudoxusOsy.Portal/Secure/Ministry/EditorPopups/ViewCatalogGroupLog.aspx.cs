using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EudoxusOsy.BusinessModel;
using System.Drawing;
using EudoxusOsy.Portal.Controls;
using System.Xml.Linq;
using System.Xml;

namespace EudoxusOsy.Portal.Secure.Ministry.EditorPopups
{
    public partial class ViewCatalogGroupLog : BaseEntityPortalPage<CatalogGroup>
    {
        #region [ Entity Fill ]

        protected override void Fill()
        {
            int groupID;
            if (int.TryParse(Request.QueryString["id"], out groupID) && groupID > 0)
            {
                Entity = new CatalogGroupRepository(UnitOfWork).Load(groupID);
            }
            else
            {
                ClientScript.RegisterStartupScript(GetType(), "refreshParent", "window.parent.popUp.hide();", true);
            }
        }

        #endregion

        #region [ Page Events ]

        protected void Page_Load(object source, EventArgs e)
        {
        }

        #endregion  

        #region [ DataSource Events ]

        protected void odsCatalogGroupLogs_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            Criteria<CatalogGroupLog> criteria = new Criteria<CatalogGroupLog>();
            criteria.Expression = criteria.Expression.Where(x => x.GroupID, Entity.ID);

            e.InputParameters["criteria"] = criteria;
        }

        #endregion

        #region [ GridView Methods ]

        protected string GetCatalogGroupLogAction(CatalogGroupLog cLog)
        {
            var oldIsLocked = false;
            var newIsLocked = false;

            if (cLog.OldValuesXml != null)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(cLog.OldValuesXml);

                string xpath = "CatalogGroupChangeValues/IsLocked";
                var node = xmlDoc.SelectSingleNode(xpath);

                if (node != null)
                {
                    oldIsLocked = bool.Parse(node.ChildNodes.Item(0).Value);
                }
            }

            if (cLog.NewValuesXml != null)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(cLog.NewValuesXml);

                string xpath = "CatalogGroupChangeValues/IsLocked";
                var node = xmlDoc.SelectSingleNode(xpath);

                if (node != null)
                {
                    newIsLocked = bool.Parse(node.ChildNodes.Item(0).Value);
                }
            }

            if (cLog.OldState == enCatalogGroupState.New && cLog.NewState == enCatalogGroupState.Selected)
                return "Επιλογή για πληρωμή";

            if (cLog.OldState == enCatalogGroupState.Selected && cLog.NewState == enCatalogGroupState.New)
                return "Αναίρεση επιλογής για πληρωμή";

            if (cLog.OldState == enCatalogGroupState.Selected && cLog.NewState == enCatalogGroupState.Approved)
                return "Εγκεκριμένη για πληρωμή";

            if (cLog.OldState == enCatalogGroupState.Approved && cLog.NewState == enCatalogGroupState.Selected)
                return "Αναίρεση έγκρισης για πληρωμή";

            if (cLog.OldState == enCatalogGroupState.Approved && cLog.NewState == enCatalogGroupState.Sent)
                return "Αποστολή προς ΥΔΕ";

            if (cLog.OldState == enCatalogGroupState.Sent && cLog.NewState == enCatalogGroupState.Returned)
                return "Επιστροφή από ΥΔΕ";

            if (cLog.OldState == enCatalogGroupState.Returned && cLog.NewState == enCatalogGroupState.Sent)
                return "Αποστολή προς ΥΔΕ";

            if (cLog.OldState == cLog.NewState)
            {
                if (!oldIsLocked && newIsLocked)
                {
                    return "Κλείδωμα Κατάστασης";
                }

                if (oldIsLocked && !newIsLocked)
                {
                    return "Ξεκλείδωμα Κατάστασης";
                }
            }

            return string.Empty;
        }

        #endregion

        //protected void odsPaymentOrders_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        //{
        //    Criteria<PaymentOrder> criteria = new Criteria<PaymentOrder>();
        //    criteria.Expression = criteria.Expression.Where(x => x.GroupID, Entity.ID);

        //    e.InputParameters["criteria"] = criteria;
        //}
    }
}