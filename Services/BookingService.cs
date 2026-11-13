using Variant09.Models;

namespace Variant09.Services;

public class BookingService
{
    private readonly Hall _hall;
    private readonly object _lockObject = new();

    private readonly SemaphoreSlim _semaphore = new(3, 3);

    public BookingService(Hall hall)
    {
        _hall = hall;
    }

    public async Task<bool> BookSeatAsync(
        int seatNumber, string customer)
    {
        await _semaphore.WaitAsync();

        try
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
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<bool> CancelBookingAsync(
        int seatNumber, string customer)
    {
        await _semaphore.WaitAsync();

        try
        {
            lock (_lockObject)
            {
                var seat = _hall.Seats
                    .FirstOrDefault(s => s.Number == seatNumber);

                if (seat == null || seat.Status == SeatStatus.Free)
                    return false;

                seat.Status = SeatStatus.Free;

                Console.WriteLine(
                    $"{customer} отменил место {seatNumber}");

                return true;
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }
}