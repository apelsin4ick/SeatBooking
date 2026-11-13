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

        Console.WriteLine("Проверка SemaphoreSlim");
        Console.WriteLine("======================");

        var tasks = Enumerable.Range(1, 10)
            .Select(i => service.BookSeatAsync(
                i, $"Пользователь {i}"));

        var results = await Task.WhenAll(tasks);

        Console.WriteLine();

        Console.WriteLine(
            $"Успешных бронирований: {results.Count(x => x)}");

        Console.ReadLine();
    }
}