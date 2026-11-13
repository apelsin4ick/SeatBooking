using Microsoft.Win32;
using Variant09.Models;

namespace Variant09.Services;

public static class HallSettingsStore
{
    private const string RegistryPath =
        @"Software\Variant09\SeatBooking";

    public static HallSettings Load()
    {
        HallSettings defaults =
            new HallSettings("Большой зал", 10);

        if (!OperatingSystem.IsWindows())
            return defaults;

        try
        {
            using RegistryKey? key =
                Registry.CurrentUser.OpenSubKey(RegistryPath);

            if (key == null)
                return defaults;

            string name =
                key.GetValue("HallName") as string
                ?? defaults.HallName;

            int count = key.GetValue("SeatCount") is int value
                && value > 0
                ? value
                : defaults.SeatCount;

            return new HallSettings(name, count);
        }
        catch (UnauthorizedAccessException)
        {
            return defaults;
        }
        catch (System.Security.SecurityException)
        {
            return defaults;
        }
    }

    public static void Save(HallSettings settings)
    {
        if (!OperatingSystem.IsWindows())
        {
            Console.WriteLine(
                "Реестр Windows недоступен.");
            return;
        }

        try
        {
            using RegistryKey? key =
                Registry.CurrentUser.CreateSubKey(RegistryPath);

            if (key == null)
                throw new InvalidOperationException(
                    "Не удалось создать раздел реестра.");

            key.SetValue("HallName", settings.HallName);

            key.SetValue(
                "SeatCount",
                settings.SeatCount,
                RegistryValueKind.DWord);

            Console.WriteLine(
                "Настройки успешно сохранены.");
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine(
                $"Ошибка доступа к реестру: {ex.Message}");
        }
        catch (System.Security.SecurityException ex)
        {
            Console.WriteLine(
                $"Ошибка безопасности: {ex.Message}");
        }
    }
}