using Imis.Domain;

namespace EudoxusOsy.BusinessModel
{
    public static class UnitOfWorkFactory
    {
        public static IUnitOfWork Create()
        {
            var ctx = new DBEntities();
            ctx.MetadataWorkspace.LoadFromAssembly(ctx.GetType().Assembly);

            return ctx;
        }
    }
}
