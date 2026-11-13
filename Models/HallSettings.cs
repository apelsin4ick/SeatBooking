namespace Variant09.Models;

public class HallSettings
{
    public string HallName { get; set; }

    public int SeatCount { get; set; }

    public HallSettings(string hallName, int seatCount)
    {
        HallName = hallName;
        SeatCount = seatCount;
    }
}