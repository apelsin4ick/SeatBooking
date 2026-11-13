using Variant09.Models;
using Variant09.Services;

namespace Variant09;

internal static class Program
{
    private static async Task Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("СИСТЕМА БРОНИРОВАНИЯ МЕСТ");
        Console.WriteLine("========================");

        // TODO 10: загрузка настроек
        HallSettings settings = HallSettingsStore.Load();

        Hall hall = new Hall(
            settings.HallName,
            settings.SeatCount);

        BookingService service = new BookingService(hall);

        await using var waitingList =
            new WaitingListService(service);

        Console.WriteLine(
            $"Зал: {hall.Name}");

        Console.WriteLine(
            $"Количество мест: {hall.Seats.Count}");

        // TODO 1: отображение свободных мест
        Console.WriteLine("\nСвободные места:");

        foreach (var seat in hall.GetFreeSeats())
        {
            Console.WriteLine($"Место {seat.Number}");
        }

        // TODO 2: бронирование и отмена
        Console.WriteLine("\nБРОНИРОВАНИЕ");

        await service.BookSeatAsync(1, "Иван");
        await service.BookSeatAsync(2, "Анна");

        // TODO 3 и 4: конкурентное бронирование и lock
        Console.WriteLine("\nКОНКУРЕНТНОЕ БРОНИРОВАНИЕ");

        var tasks = Enumerable.Range(1, 5)
            .Select(i => service.BookSeatAsync(
                3, $"Пользователь {i}"));

        var results = await Task.WhenAll(tasks);

        Console.WriteLine(
            $"Успешных бронирований места 3: " +
            $"{results.Count(x => x)}");

        // TODO 5: SemaphoreSlim
        Console.WriteLine(
            "\nSemaphoreSlim ограничивает количество " +
            "одновременно обрабатываемых запросов тремя.");

        // TODO 6: PLINQ
        Console.WriteLine("\nПРОВЕРКА ЗАНЯТОСТИ МЕСТ");

        foreach (var seat in service.GetSeatStatusesParallel())
        {
            Console.WriteLine(
                $"Место {seat.Number}: {seat.Status}");
        }

        // TODO 7: журнал
        Console.WriteLine("\nЖУРНАЛ БРОНИРОВАНИЙ");

        foreach (var entry in service.GetJournal())
        {
            Console.WriteLine(
                $"{entry.Timestamp:HH:mm:ss} | " +
                $"{entry.Action} | " +
                $"Место {entry.SeatNumber} | " +
                $"{entry.Customer}");
        }

        // TODO 8: очередь ожидания
        Console.WriteLine("\nОЧЕРЕДЬ ОЖИДАНИЯ");

        await service.BookSeatAsync(4, "Первый клиент");

        var waitingTask = waitingList.AddToQueueAsync(
            4, "Клиент из очереди");

        await Task.Delay(300);

        await service.CancelBookingAsync(4, "Первый клиент");

        bool waitingResult = await waitingTask.WaitAsync(
            TimeSpan.FromSeconds(5));

        Console.WriteLine(
            waitingResult
                ? "Клиент из очереди получил место."
                : "Не удалось забронировать место.");

        // TODO 9: статистика
        Console.WriteLine("\nСТАТИСТИКА");

        var stats = service.GetStatistics();

        Console.WriteLine($"Всего мест: {stats.TotalSeats}");
        Console.WriteLine($"Свободно: {stats.FreeSeats}");
        Console.WriteLine($"Занято: {stats.BookedSeats}");

        Console.WriteLine(
            $"Бронирований: {stats.TotalBookings}");

        Console.WriteLine(
            $"Отмен: {stats.TotalCancellations}");

        // TODO 10: сохранение настроек
        Console.WriteLine("\nСОХРАНЕНИЕ НАСТРОЕК");

        HallSettingsStore.Save(
            new HallSettings(hall.Name, hall.Seats.Count));

        Console.WriteLine("\nРабота программы завершена.");

        Console.ReadLine();
    }
}