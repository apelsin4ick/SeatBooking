using Variant09.Models;

namespace Variant09.Services;

public class BookingService
{
    private readonly Hall _hall;

    public BookingService(Hall hall)
    {
        _hall = hall;
    }

    public bool BookSeat(int seatNumber, string customer)
    {
        var seat = _hall.Seats
            .FirstOrDefault(s => s.Number == seatNumber);

        if (seat == null)
        {
            Console.WriteLine("Место не найдено.");
            return false;
        }

        if (seat.Status == SeatStatus.Booked)
        {
            Console.WriteLine("Место уже занято.");
            return false;
        }

        seat.Status = SeatStatus.Booked;

        Console.WriteLine(
            $"{customer} забронировал место {seatNumber}.");

        return true;
    }

    public bool CancelBooking(int seatNumber, string customer)
    {
        var seat = _hall.Seats
            .FirstOrDefault(s => s.Number == seatNumber);

        if (seat == null || seat.Status == SeatStatus.Free)
        {
            Console.WriteLine("Бронирование не найдено.");
            return false;
        }

        seat.Status = SeatStatus.Free;

        Console.WriteLine(
            $"{customer} отменил бронирование места {seatNumber}.");

        return true;
    }
}