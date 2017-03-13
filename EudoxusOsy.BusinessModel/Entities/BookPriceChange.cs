namespace EudoxusOsy.BusinessModel
{
    public partial class BookPriceChange
    {
        public bool IsApproved
        {
            get { return Approved == null ? false : Approved.Value; }
        }
    }
}
