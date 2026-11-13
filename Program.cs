using Variant09.Models;

namespace Variant09;

internal static class Program
{
    private static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("Система бронирования мест");
        Console.WriteLine("=========================");

        Hall hall = new Hall("Большой зал", 10);

        Console.WriteLine($"Название зала: {hall.Name}");
        Console.WriteLine("Свободные места:");

        foreach (var seat in hall.GetFreeSeats())
        {
            Console.WriteLine($"Место {seat.Number}: {seat.Status}");
        }

        Console.ReadLine();
    }
}