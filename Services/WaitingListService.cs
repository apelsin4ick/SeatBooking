using System.Threading.Channels;

namespace Variant09.Services;

public class WaitingListService : IAsyncDisposable
{
    private record WaitingRequest(
        int SeatNumber,
        string Customer,
        TaskCompletionSource<bool> Completion);

    private readonly BookingService _bookingService;

    private readonly Channel<WaitingRequest> _channel =
        Channel.CreateUnbounded<WaitingRequest>();

    private readonly SemaphoreSlim _seatReleased = new(0);

    private readonly CancellationTokenSource _cancellation = new();

    private readonly Task _worker;

    public WaitingListService(BookingService bookingService)
    {
        _bookingService = bookingService;

        _bookingService.SeatReleased = NotifySeatReleased;

        _worker = ProcessQueueAsync();
    }

    public async Task<bool> AddToQueueAsync(
        int seatNumber, string customer)
    {
        var completion = new TaskCompletionSource<bool>(
            TaskCreationOptions.RunContinuationsAsynchronously);

        await _channel.Writer.WriteAsync(
            new WaitingRequest(seatNumber, customer, completion));

        return await completion.Task;
    }

    private async Task ProcessQueueAsync()
    {
        try
        {
            await foreach (var request in
                _channel.Reader.ReadAllAsync(_cancellation.Token))
            {
                while (!_cancellation.IsCancellationRequested)
                {
                    bool success = await _bookingService.BookSeatAsync(
                        request.SeatNumber,
                        request.Customer);

                    if (success)
                    {
                        request.Completion.TrySetResult(true);
                        break;
                    }

                    await _seatReleased.WaitAsync(_cancellation.Token);
                }
            }
        }
        catch (OperationCanceledException)
            when (_cancellation.IsCancellationRequested)
        {
            // Завершение обработчика очереди.
        }
        finally
        {
            while (_channel.Reader.TryRead(out var request))
            {
                request.Completion.TrySetResult(false);
            }
        }
    }

    private void NotifySeatReleased()
    {
        _seatReleased.Release();
    }

    public async ValueTask DisposeAsync()
    {
        _bookingService.SeatReleased = null;

        _channel.Writer.TryComplete();
        _cancellation.Cancel();

        try
        {
            await _worker;
        }
        finally
        {
            _seatReleased.Dispose();
            _cancellation.Dispose();
        }
    }
}