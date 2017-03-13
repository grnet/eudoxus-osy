using Imis.Domain.EF;
using Imis.Domain.EF.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace EudoxusOsy.BusinessModel
{
    public class SupplierRepository : DomainRepository<DBEntities, Supplier, int>
    {
        #region [ Base .ctors ]

        public SupplierRepository() : base() { }

        public SupplierRepository(Imis.Domain.IUnitOfWork uow) : base(uow) { }

        #endregion

        public Supplier FindByID(int id)
        {
            return BaseQuery
                    .FirstOrDefault(x => x.ID == id);
        }

        public Supplier FindByKpsID(int id, params Expression<Func<Supplier, object>>[] includeExpressions)
        {
            var query = BaseQuery;

            if (includeExpressions.Length > 0)
            {
                foreach (var item in includeExpressions)
                    query = query.Include(item);
            }

            return query
                    .Where(x => x.SupplierKpsID == id)
                    .FirstOrDefault();
        }


        public Supplier FindByUsername(string username, params Expression<Func<Supplier, object>>[] includeExpressions)
        {
            var query = BaseQuery;

            query = query.Include(x => x.Reporter);

            if (includeExpressions.Length > 0)
            {
                foreach (var item in includeExpressions)
                    query = query.Include(item);
            }

            return query
                    .Where(x => x.Reporter.Username == username)
                    .FirstOrDefault();
        }

        public Supplier FindByEmail(string email)
        {
            return BaseQuery
                    .Where(x => x.Email == email)
                    .FirstOrDefault();
        }

        public IList<Supplier> GetAllActive()
        {
            return BaseQuery.Where(x => x.StatusInt == (int)enSupplierStatus.Active).ToList();
        }

        public List<CoAuthors> GetCoAuthors(int phaseID)
        {
            var ctx = GetCurrentObjectContext();
            return ctx.GetCoAuthors(phaseID).ToList();
        }

        public List<SuppliersNoLogisticBooks> GetSuppliersNoLogisticBooks(int phaseID)
        {
            var ctx = GetCurrentObjectContext();
            return ctx.SuppliersNoLogisticBooks(phaseID).ToList();
        }

        public List<Commitments> GetCommitments(int phaseID)
        {
            var ctx = GetCurrentObjectContext();
            return ctx.ExportCommitmentsRegistry(phaseID).ToList();
        }

        public IQueryable<SupplierFullStatistics> GetCurrentPhaseStatistics(int? supplierKpsID, string afm, string name, int? supplierType, out int recordCount)
        {
            var ctx = GetCurrentObjectContext();
            var returnValue = new System.Data.Objects.ObjectParameter("Count", 0);
            var list = ctx.GetSupplierFullStatistics(supplierKpsID, afm, name, supplierType, returnValue).ToList().AsQueryable();

            recordCount = (int)returnValue.Value;
            return list;
        }

        public IQueryable<SuppliersStatsForExport> GetSupplierStatsForExport(int phaseID)
        {
            var ctx = GetCurrentObjectContext();
            var list = ctx.GetSuppliersStatsForExport(phaseID).ToList().AsQueryable();

            return list;
        }

        public List<SyncPublisherDto> GetManuallyInsertedSuppliers()
        {
            SqlConnection sqlConnection1 = new SqlConnection(ConfigurationManager.ConnectionStrings["LocalSqlServer"].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "SELECT MissingSuppliers.*, Pass.Password, Pass.PasswordSalt FROM MissingSuppliers JOIN Pass ON Pass.username = MissingSuppliers.username";
            cmd.CommandType = CommandType.Text;
            cmd.Connection = sqlConnection1;

            sqlConnection1.Open();

            reader = cmd.ExecuteReader();
            // Data is accessible through the DataReader object here.
            List<SyncPublisherDto> dtos = new List<SyncPublisherDto>();

            while (reader.Read())
            {
                SyncPublisherDto dto = new SyncPublisherDto()
                {
                    PublisherKpsID = (int)reader["ID"],
                    ContactName = (string)reader["ContactName"],
                    ContactPhone = (string)reader["ContactPhone"],
                    ContactMobilePhone = (string)reader["ContactMobilePhone"],
                    ContactEmail = (string)reader["ContactEmail"],
                    PublisherType = (int)reader["PublisherType"],
                    PublisherTradeName = (string)reader["PublisherTradeName"],
                    PublisherName = (string)reader["PublisherName"],
                    PublisherAFM = (string)reader["PublisherAFM"],
                    Email = (string)reader["Email"],
                    IsActivated = (int)reader["IsActivated"] == 1,
                    VerificationStatus = (int)reader["VerificationStatus"],
                    Username = (string)reader["UserName"],
                    Password = (string)reader["Password"],
                    PasswordSalt = (string)reader["PasswordSalt"],
                    PublisherDOY = "TEST"

                };


                dtos.Add(dto);
            }

            sqlConnection1.Close();


            return dtos;
        }
    }
}
