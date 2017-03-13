using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using Microsoft.AspNet.SignalR;
using EudoxusOsy.BusinessModel;

namespace EudoxusOsy.Portal.Hubs
{
    //public class BackofficeHub : Hub
    //{
    //    public void Hello()
    //    {
    //        Clients.All.hello();
    //    }

    //    public void LockProvider(int providerID)
    //    {
    //        try
    //        {
    //            Clients.Others.lockProvider(providerID, Thread.CurrentPrincipal.Identity.Name);
    //        }
    //        catch (Exception ex)
    //        {
    //            Clients.Caller.onError(ex.Message);
    //        }
    //    }

    //    public void UnlockProvider(int providerID)
    //    {
    //        try
    //        {
    //            using (var uow = UnitOfWorkFactory.Create())
    //            {
    //                var provider = new ProviderRepository(uow).Load(providerID, x => x.Reporter.CertificationDetail);
    //                var lockSrv = new LockService(uow, provider.Reporter.CertificationDetail);

    //                if (lockSrv.Release())
    //                    Clients.Others.unlockProvider(providerID);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Clients.Caller.onError(ex.Message);
    //        }
    //    }

    //    public void LockOffer(int offerID)
    //    {
    //        try
    //        {
    //            Clients.Others.lockOffer(offerID, Thread.CurrentPrincipal.Identity.Name);
    //        }
    //        catch (Exception ex)
    //        {
    //            Clients.Caller.onError(ex.Message);
    //        }
    //    }

    //    public void UnlockOffer(int offerID)
    //    {
    //        try
    //        {
    //            using (var uow = UnitOfWorkFactory.Create())
    //            {
    //                var offer = new OfferRepository(uow).Load(offerID);
    //                var lockSrv = new LockService(uow, offer);

    //                if (lockSrv.Release())
    //                    Clients.Others.unlockOffer(offerID);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            Clients.Caller.onError(ex.Message);
    //        }
    //    }
    //}
}