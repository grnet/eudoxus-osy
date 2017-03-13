using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using DevExpress.Web;

namespace EudoxusOsy.Portal.Controls
{
    [ParseChildren(true)]
    [PersistChildren(false)]
    [Themeable(true)]
    public abstract class BaseGridViewUserControl : UserControl
    {
        #region [ Properties ]

        public bool DataSourceForceStandardPaging
        {
            get { return Grid.DataSourceForceStandardPaging; }
            set { Grid.DataSourceForceStandardPaging = value; }
        }

        public string ClientInstanceName
        {
            get { return Grid.ClientInstanceName; }
            set { Grid.ClientInstanceName = value; }
        }

        public string DataSourceID
        {
            get { return Grid.DataSourceID; }
            set { Grid.DataSourceID = value; }
        }

        public object DataSource
        {
            get { return Grid.DataSource; }
            set { Grid.DataSource = value; }
        }

        public int PageIndex
        {
            get { return Grid.PageIndex; }
            set { Grid.PageIndex = value; }
        }

        public GridViewPagerMode PagingMode
        {
            get { return Grid.SettingsPager.Mode; }
            set { Grid.SettingsPager.Mode = value; }
        }

        public int PageSize
        {
            get { return Grid.SettingsPager.PageSize; }
            set { Grid.SettingsPager.PageSize = value; }
        }

        public bool AllowSorting
        {
            get { return Grid.SettingsBehavior.AllowSort; }
            set { Grid.SettingsBehavior.AllowSort = value; }
        }

        public bool GridClientVisible
        {
            get { return Grid.ClientVisible; }
            set { Grid.ClientVisible = value; }
        }

        [TemplateInstance(TemplateInstance.Multiple)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [TemplateContainer(typeof(GridViewColumn))]
        public List<GridViewColumn> Columns { get; set; }

        [TemplateInstance(TemplateInstance.Single)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        [TemplateContainer(typeof(GridViewClientSideEvents))]
        public GridViewClientSideEvents ClientSideEvents { get { return Grid.ClientSideEvents; } }

        public GridViewHeaderStyle HeaderStyle { get { return Grid.Styles.Header; } }

        public abstract ASPxGridView Grid { get; }

        #endregion

        #region [ Methods ]

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (Columns != null)
            {
                foreach (var item in Columns)
                {
                    if (Grid.Columns.FindByName(item.Name) == null)
                        Grid.Columns.Add(item);
                }
            }
        }

        public object GetRow(int visibleIndex)
        {
            return Grid.GetRow(visibleIndex);
        }

        public object GetRowValues(int visibleIndex, params string[] fieldNames)
        {
            return Grid.GetRowValues(visibleIndex, fieldNames);
        }

        public object GetRowValuesByKeyValue(object keyValue, params string[] fieldNames)
        {
            return Grid.GetRowValuesByKeyValue(keyValue, fieldNames);
        }

        #endregion

        #region [ Events ]

        public event ASPxGridViewCustomCallbackEventHandler CustomCallback
        {
            add { Grid.CustomCallback += value; }
            remove { Grid.CustomCallback -= value; }
        }

        public event ASPxGridViewCustomDataCallbackEventHandler CustomDataCallback
        {
            add { Grid.CustomDataCallback += value; }
            remove { Grid.CustomDataCallback -= value; }
        }

        public event ASPxGridViewClientJSPropertiesEventHandler CustomJSProperties
        {
            add { Grid.CustomJSProperties += value; }
            remove { Grid.CustomJSProperties -= value; }
        }

        public event ASPxGridViewTableRowEventHandler HtmlRowPrepared
        {
            add { Grid.HtmlRowPrepared += value; }
            remove { Grid.HtmlRowPrepared -= value; }
        }

        public event ASPxGridViewTableDataCellEventHandler HtmlCellPrepared
        {
            add { Grid.HtmlDataCellPrepared += value; }
            remove { Grid.HtmlDataCellPrepared -= value; }
        }

        public event ASPxGridViewCommandButtonEventHandler CommandButtonInitialize
        {
            add { Grid.CommandButtonInitialize += value; }
            remove { Grid.CommandButtonInitialize -= value; }
        }

        #endregion
    }
}