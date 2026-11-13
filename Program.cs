using Variant09.Models;
using Variant09.Services;

namespace Variant09;

internal static class Program
{
    private static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Hall hall = new Hall("Большой зал", 10);
        BookingService service = new BookingService(hall);

        Console.WriteLine("Система бронирования мест");
        Console.WriteLine("=========================");

        service.BookSeat(1, "Иван");
        service.BookSeat(2, "Анна");

        Console.WriteLine();

        service.CancelBooking(1, "Иван");

        Console.WriteLine("\nТекущее состояние зала:");

        foreach (var seat in hall.Seats)
        {
            Console.WriteLine(
                $"Место {seat.Number}: {seat.Status}");
        }

        Console.ReadLine();
    }
}