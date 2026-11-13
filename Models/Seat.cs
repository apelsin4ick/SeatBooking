namespace Variant09.Models;

public enum SeatStatus
{
    Free,
    Booked
}

public class Seat
{
    public int Number { get; set; }
    public SeatStatus Status { get; set; }

    public Seat(int number)
    {
        Number = number;
        Status = SeatStatus.Free;
    }
}