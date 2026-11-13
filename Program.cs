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

        Console.WriteLine("Конкурентное бронирование");
        Console.WriteLine("=========================");

        var tasks = Enumerable.Range(1, 5)
            .Select(i => service.BookWithRaceCondition(
                1, $"Пользователь {i}"));

        var results = await Task.WhenAll(tasks);

        Console.WriteLine();

        Console.WriteLine(
            $"Успешных попыток: {results.Count(x => x)}");

        Console.WriteLine(
            $"Статус места 1: {hall.Seats[0].Status}");

        Console.ReadLine();
    }
}