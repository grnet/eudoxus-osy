using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web;
using EudoxusOsy.BusinessModel;
using Imis.Domain;

namespace EudoxusOsy.Portal.InternalServices
{
    public class BaseInternalService : IDisposable
    {
        protected IUnitOfWork UnitOfWork { get; private set; }

        public BaseInternalService() : this(UnitOfWorkFactory.Create()) { }

        public BaseInternalService(IUnitOfWork uow) { UnitOfWork = uow; }

        protected ServiceResponse Success()
        {
            return new ServiceResponse { Success = true };
        }

        protected ServiceResponse Failure(string message = "")
        {
            return new ServiceResponse
            {
                Success = false,
                Message = message
            };
        }

        protected ServiceResponse<T> Success<T>(T value)
        {
            return new ServiceResponse<T> { Success = true, Value = value };
        }

        protected ServiceResponse<T> Failure<T>(T value = default(T), string message = "")
        {
            return new ServiceResponse<T>
            {
                Success = false,
                Value = value,
                Message = message
            };
        }

        public void Dispose()
        {
            UnitOfWork.Dispose();
        }
    }
}