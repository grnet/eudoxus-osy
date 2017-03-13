﻿using System;
using System.Collections.Generic;
using Imis.Domain.EF;
using Imis.Domain.EF.Extensions;
using System.Data.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace EudoxusOsy.BusinessModel
{
    public class PaymentOrderRepository : DomainRepository<DBEntities, PaymentOrder, int>
    {
        #region [ Base .ctors ]

        public PaymentOrderRepository() : base() { }

        public PaymentOrderRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        protected ObjectQuery<PaymentOrder> BasePaymentOrderQuery
        {
            get
            {
                return (ObjectQuery<PaymentOrder>)BaseQuery.Where(x => x.IsActive == true);
            }
        }

        #endregion

        public PaymentOrder FindByGroupID(int groupID)
        {
            return BasePaymentOrderQuery
                    .Where(x => x.GroupID == groupID)
                    .OrderByDescending(x => x.ID)
                    .FirstOrDefault();
        }

        public List<PaymentOrder> FindSentByOfficeSlipDate(DateTime officeSlipDate)
        {
            return BasePaymentOrderQuery
                .Include(x=> x.CatalogGroup.Supplier)
                .Include(x=> x.CatalogGroup.Catalogs)
                .Include(x=> x.CatalogGroup.Deduction)
                    .Where(x=> x.OfficeSlipDate == officeSlipDate && x.StateInt == (int)enPaymentOrderState.Sent).ToList();
        }

        public int FindMaxOfficeSlipNumber(int year)
        {
            var maxValue = new CatalogGroupLogRepository().FindMaxOfficeSlipNumber(year).ToString();
            if (maxValue == "0")
            {
                maxValue = BasePaymentOrderQuery.Max(x => x.OfficeSlipNumber).ToString();
                if (maxValue.Substring(0, 4) != year.ToString())
                {
                    maxValue = "0";
                }
                else
                {
                    maxValue = maxValue.Substring(4, maxValue.Length - 4);
                }
            }
            return Convert.ToInt32(maxValue);
        }
    }
}
