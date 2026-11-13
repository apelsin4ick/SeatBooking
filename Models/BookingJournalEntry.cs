namespace Variant09.Models;

public class BookingJournalEntry
{
    public DateTime Timestamp { get; set; }

    public string Action { get; set; }

    public int SeatNumber { get; set; }

    public string Customer { get; set; }

    public BookingJournalEntry(
        string action,
        int seatNumber,
        string customer)
    {
        Timestamp = DateTime.Now;
        Action = action;
        SeatNumber = seatNumber;
        Customer = customer;
    }
}