using EudoxusOsy.BusinessModel;
using Imis.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace EudoxusOsy.Portal.DataSources
{
    public class FileSelfPublishers : BaseDataSource<FileSelfPublisher>
    {
        [DataObjectMethod(DataObjectMethodType.Select)]
        public IList<FileSelfPublisherInfo> FindInfoWithCriteria(Criteria<FileSelfPublisher> criteria, int startRowIndex, int maximumRows)
        {
            using (IUnitOfWork uow = UnitOfWorkFactory.Create())
            {
                var results = base.FindWithCriteria(criteria, startRowIndex, maximumRows, null);

                return results.Select(x => new FileSelfPublisherInfo() {
                    FileName = x.File.FileName,
                    ID = x.FileID,
                    PhaseID = x.PhaseID.ToString()
                }).ToList();
            }
        }
    }
}