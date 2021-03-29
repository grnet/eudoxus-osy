using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web;
using EudoxusOsy.BusinessModel;

namespace EudoxusOsy.Portal
{
    public static class ControlExtensions
    {
        #region [ GridView ]

        public static GridViewColumn FindByName(this GridViewColumnCollection columns, string name)
        {
            foreach (GridViewColumn item in columns)
            {
                if (item.Name == name)
                    return item;
            }
            return null;
        }

        #endregion

        #region [ TextControl ]

        public static string GetText(this ITextControl tc)
        {
            return tc.Text.ToNull();
        }

        public static int? GetInteger(this ITextControl tc)
        {
            int value;
            if (Int32.TryParse(tc.Text.ToNull(), out value))
                return value;
            else
                return null;
        }

        public static double? GetDouble(this ITextControl tc)
        {
            double value;
            if (Double.TryParse(tc.Text.ToNull(), out value))
                return value;
            else
                return null;
        }

        public static decimal? GetDecimal(this ITextControl tc)
        {
            decimal value;
            if (Decimal.TryParse(tc.Text.ToNull(), out value))
                return value;
            else
                return null;
        }

        public static string GetText(this HtmlInputText tb)
        {
            return tb.Value.ToNull();
        }

        public static int? GetInteger(this HtmlInputText tb)
        {
            int value;
            if (Int32.TryParse(tb.Value.ToNull(), out value))
                return value;
            else
                return null;
        }

        #endregion

        #region [ ComboBox ]

        public static string GetSelectedString(this ASPxComboBox cb)
        {
            if (cb.Value != null)
                return cb.Value.ToString().ToNull();
            else
                return null;
        }

        public static int? GetSelectedInteger(this ASPxComboBox cb)
        {
            int value;
            if (cb.Value != null && Int32.TryParse(cb.Value.ToString(), out value))
                return value;
            else
                return null;
        }

        public static int? GetSelectedInteger(this ASPxRadioButtonList cb)
        {
            int value;
            if (cb.Value != null && Int32.TryParse(cb.Value.ToString(), out value))
                return value;
            else
                return null;
        }

        public static bool? GetSelectedBoolean(this ASPxComboBox cb)
        {
            bool? value = null;

            if (cb.Value != null)
            {
                if (cb.Value.ToString() == "1")
                    value = true;
                else if (cb.Value.ToString() == "0")
                    value = false;
            }

            return value;
        }

        public static bool? GetSelectedBoolean(this ASPxRadioButtonList cb)
        {
            bool? value = null;

            if (cb.Value != null)
            {
                if (cb.Value.ToString() == "1")
                    value = true;
                else if (cb.Value.ToString() == "0")
                    value = false;
            }

            return value;
        }

        public static int? ToInt(this bool? input)
        {
            if (input.HasValue)
            {
                return Convert.ToInt32(input.Value);
            }

            return null;
        }

        public static TEnum? GetSelectedEnum<TEnum>(this ASPxComboBox cb) where TEnum : struct
        {
            TEnum value;
            if (cb.Value != null && Enum.TryParse<TEnum>(cb.Value.ToString(), out value))
                return value;
            else
                return null;
        }

        public static string GetSelectedString(this ListControl cb)
        {
            if (cb.SelectedValue != null)
                return cb.SelectedValue.ToNull();
            else
                return null;
        }

        public static int? GetSelectedInteger(this ListControl cb)
        {
            int value;
            if (!String.IsNullOrWhiteSpace(cb.SelectedValue) && Int32.TryParse(cb.SelectedValue, out value))
                return value;
            else
                return null;
        }

        public static bool? GetSelectedBoolean(this ListControl cb)
        {
            bool? value = null;

            if (!String.IsNullOrWhiteSpace(cb.SelectedValue))
            {
                if (cb.SelectedValue == "1")
                    value = true;
                else if (cb.SelectedValue == "0")
                    value = false;
            }

            return value;
        }

        public static TEnum? GetSelectedEnum<TEnum>(this ListControl cb) where TEnum : struct
        {
            TEnum value;
            if (!String.IsNullOrWhiteSpace(cb.SelectedValue) && Enum.TryParse<TEnum>(cb.SelectedValue, out value))
                return value;
            else
                return null;
        }

        #endregion

        #region [ DateEdit ]

        public static DateTime? GetDate(this ASPxDateEdit de)
        {
            if (de.Value != null)
                return de.Date;
            else
                return null;
        }

        #endregion

        #region [ DropDownEdit ]

        public static string GetStringValue(this ASPxDropDownEdit ddx)
        {
            if (ddx.Value != null)
                return ddx.Value.ToString().ToNull();
            else
                return null;
        }

        public static int? GetIntegerValue(this ASPxDropDownEdit ddx)
        {
            int value;
            if (ddx.Value != null && Int32.TryParse(ddx.Value.ToString(), out value))
                return value;
            else
                return null;
        }

        #endregion

        #region [ HiddenField ]

        public static int? GetInteger(this HiddenField hf)
        {
            int value;
            if (Int32.TryParse(hf.Value, out value))
                return value;
            else
                return null;
        }

        #endregion

        public static string GetDOY(this Supplier supplier)
        {
            string doy = String.Empty;

            if (supplier.PaymentPfoID == EudoxusOsyConstants.FOREIGN_PFO_ID)
            {
                doy = supplier.PaymentPfo;
            }
            else if(supplier.PaymentPfoID.HasValue)
            {
                var pfo = CacheManager.GetOrderedPublicFinancialOffices()
                    .FirstOrDefault(x => x.ID == supplier.PaymentPfoID.Value);

                if (pfo != null)
                {
                    doy =  pfo.Name;
                }                
            }

            return doy;
        }
    }
}