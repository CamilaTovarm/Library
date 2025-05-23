namespace Library.Models
{
    public class Returns
    {
        public int ReturnId { get; set; }
        public DateTime ReturnDate { get; set; }
        public Loans Loans { get; set; }
        public int LoanId { get; set; }
        public float FineImposed { get; set; }
        public DateTime CreateDate { get; set; }
        public bool State { get; set; }
    }
}
