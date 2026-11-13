using Variant09.Models;
using Variant09.Services;

namespace Variant09;

internal static class Program
{
    private static async Task Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Hall hall = new Hall("Большой зал", 5);

        BookingService service = new BookingService(hall);

        await using var waitingList =
            new WaitingListService(service);

        Console.WriteLine("Очередь ожидания");
        Console.WriteLine("================");

        await service.BookSeatAsync(1, "Первый клиент");

        Console.WriteLine("Первый клиент занял место.");

        var waitingTask = waitingList.AddToQueueAsync(
            1, "Второй клиент");

        await Task.Delay(300);

        Console.WriteLine("Второй клиент ожидает.");

        await service.CancelBookingAsync(1, "Первый клиент");

        Console.WriteLine("Первый клиент освободил место.");

        bool result = await waitingTask.WaitAsync(
            TimeSpan.FromSeconds(5));

        Console.WriteLine(
            result
                ? "Второй клиент получил место!"
                : "Бронирование не удалось.");

        Console.ReadLine();
    }
}