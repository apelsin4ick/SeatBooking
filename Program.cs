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

        Console.WriteLine("Проверка защиты через lock");
        Console.WriteLine("==========================");

        var tasks = Enumerable.Range(1, 5)
            .Select(i => Task.Run(() =>
                service.BookSeat(1, $"Пользователь {i}")));

        var results = await Task.WhenAll(tasks);

        Console.WriteLine();

        Console.WriteLine(
            $"Успешных бронирований: {results.Count(x => x)}");

        Console.WriteLine(
            $"Статус места: {hall.Seats[0].Status}");

        Console.ReadLine();
    }
}