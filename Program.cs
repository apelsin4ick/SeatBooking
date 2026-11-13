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
        await service.BookSeatAsync(3, "Петр");

        await service.CancelBookingAsync(2, "Анна");

        Console.WriteLine("Статистика бронирований");
        Console.WriteLine("=======================");

        var stats = service.GetStatistics();

        Console.WriteLine($"Всего мест: {stats.TotalSeats}");
        Console.WriteLine($"Свободно: {stats.FreeSeats}");
        Console.WriteLine($"Занято: {stats.BookedSeats}");

        Console.WriteLine(
            $"Всего бронирований: {stats.TotalBookings}");

        Console.WriteLine(
            $"Всего отмен: {stats.TotalCancellations}");

        Console.ReadLine();
    }
}