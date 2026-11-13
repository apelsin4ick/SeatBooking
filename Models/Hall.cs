namespace Variant09.Models;

public class Hall
{
    public string Name { get; set; }
    public List<Seat> Seats { get; set; }

    public Hall(string name, int seatCount)
    {
        Name = name;
        Seats = new List<Seat>();

        for (int i = 1; i <= seatCount; i++)
        {
            Seats.Add(new Seat(i));
        }
    }

    public List<Seat> GetFreeSeats()
    {
        return Seats
            .Where(seat => seat.Status == SeatStatus.Free)
            .ToList();
    }
}