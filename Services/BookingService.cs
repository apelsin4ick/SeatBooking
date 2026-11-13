using Variant09.Models;

namespace Variant09.Services;

public class BookingService
{
    private readonly Hall _hall;

    private readonly object _lockObject = new();

    public BookingService(Hall hall)
    {
        _hall = hall;
    }

    public bool BookSeat(int seatNumber, string customer)
    {
        lock (_lockObject)
        {
            var seat = _hall.Seats
                .FirstOrDefault(s => s.Number == seatNumber);

            if (seat == null || seat.Status == SeatStatus.Booked)
                return false;

            seat.Status = SeatStatus.Booked;

            Console.WriteLine(
                $"{customer} забронировал место {seatNumber}");

            return true;
        }
    }

    public bool CancelBooking(int seatNumber, string customer)
    {
        lock (_lockObject)
        {
            var seat = _hall.Seats
                .FirstOrDefault(s => s.Number == seatNumber);

            if (seat == null || seat.Status == SeatStatus.Free)
                return false;

            seat.Status = SeatStatus.Free;

            Console.WriteLine(
                $"{customer} отменил бронирование места {seatNumber}");

            return true;
        }
    }
}