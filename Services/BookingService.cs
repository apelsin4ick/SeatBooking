using Variant09.Models;
using System.Collections.Concurrent;
namespace Variant09.Services;

public class BookingService
{
    public Action? SeatReleased { get; set; }
    private readonly Hall _hall;
    private readonly object _lockObject = new();

    private readonly SemaphoreSlim _semaphore = new(3, 3);

    private readonly ConcurrentQueue<BookingJournalEntry> _journal = new();

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
                
                _journal.Enqueue(
                new BookingJournalEntry("Бронирование", seatNumber, customer));
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
                
                _journal.Enqueue(
                new BookingJournalEntry("Отмена", seatNumber, customer));
                return true;

                SeatReleased?.Invoke();
            }
        }
        finally
        {
            _semaphore.Release();
        }

    }
    public List<Seat> GetSeatStatusesParallel()
    {
        Seat[] snapshot;

        lock (_lockObject)
        {
            snapshot = _hall.Seats.ToArray();
        }

        return snapshot
            .AsParallel()
            .OrderBy(seat => seat.Number)
            .ToList();
    }
    public List<BookingJournalEntry> GetJournal()
    {
        return _journal
            .ToArray()
            .OrderBy(entry => entry.Timestamp)
            .ToList();
    }
}