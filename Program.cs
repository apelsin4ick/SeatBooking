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
        await service.BookSeatAsync(3, "Анна");
        await service.BookSeatAsync(5, "Петр");

        Console.WriteLine("Параллельная проверка мест");
        Console.WriteLine("==========================");

        var seats = service.GetSeatStatusesParallel();

        foreach (var seat in seats)
        {
            Console.WriteLine(
                $"Место {seat.Number}: {seat.Status}");
        }

        Console.ReadLine();
    }
}