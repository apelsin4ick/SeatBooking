using Variant09.Models;
using Variant09.Services;

namespace Variant09;

internal static class Program
{
    private static async Task Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Hall hall = new Hall("Большой зал", 10);
        BookingService service = new BookingService(hall);

        await service.BookSeatAsync(1, "Иван");
        await service.BookSeatAsync(2, "Анна");
        await service.CancelBookingAsync(1, "Иван");

        Console.WriteLine("\nЖурнал бронирований");
        Console.WriteLine("===================");

        foreach (var entry in service.GetJournal())
        {
            Console.WriteLine(
                $"{entry.Timestamp:HH:mm:ss} | " +
                $"{entry.Action} | " +
                $"Место: {entry.SeatNumber} | " +
                $"Клиент: {entry.Customer}");
        }

        Console.ReadLine();
    }
}