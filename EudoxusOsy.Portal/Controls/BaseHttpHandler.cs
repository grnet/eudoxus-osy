using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Imis.Domain;
using EudoxusOsy.BusinessModel;

namespace EudoxusOsy.Portal.Controls
{
    public abstract class BaseHttpHandler : IHttpHandler
    {
        private IUnitOfWork _UnitOfWork = null;
        protected IUnitOfWork UnitOfWork
        {
            get { return (_UnitOfWork ?? (_UnitOfWork = UnitOfWorkFactory.Create())); }
            set { _UnitOfWork = value; }
        }

        protected HttpContext Context { get; private set; }

        protected HttpRequest Request { get { return Context.Request; } }

        protected HttpResponse Response { get { return Context.Response; } }

        protected IPrincipal User { get { return Context.User; } }

        protected abstract void DoProcessRequest();

        #region [ IHttpHandler Members ]

        bool IHttpHandler.IsReusable
        {
            get { return false; }
        }

        void IHttpHandler.ProcessRequest(HttpContext context)
        {
            Context = context;
            DoProcessRequest();
        }

        #endregion

        #region [ Notify ]

        private const string s_notifyKey = "_imis_notifyMsg";

        protected void NotifyOnNextRequest(string notifyMsg)
        {
            Context.Session[s_notifyKey] = notifyMsg;
        }

        protected void RedirectAndNotify(string redirectUrl, string notifyMsg)
        {
            Context.Session[s_notifyKey] = notifyMsg;
            Response.Redirect(redirectUrl, true);
        }

        #endregion
    }
}