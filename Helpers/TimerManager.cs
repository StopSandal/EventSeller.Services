using EventSeller.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace EventSeller.Services.Helpers
{
    /// <summary>
    /// The TimerManager class manages the registration, execution, and cancellation of timed tasks.
    /// It allows you to register methods to be executed after a specified delay.
    /// The timers are tracked by service type and a unique key.
    /// </summary>
    /// <typeparam name="TKey">The type of the key used to identify and manage timers.</typeparam>
    public class TimerManager<TKey> : ITimerManager<TKey>
    {
        private readonly ConcurrentDictionary<Type, ConcurrentDictionary<TKey, CancellationTokenSource>> _serviceTimers
            = new ConcurrentDictionary<Type, ConcurrentDictionary<TKey, CancellationTokenSource>>();

        private readonly ILogger<TimerManager<TKey>> _logger;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the TimerManager class.
        /// </summary>
        /// <param name="logger">The logger instance for logging timer operations.</param>
        /// <param name="serviceProvider">The service provider for resolving service dependencies.</param>
        public TimerManager(ILogger<TimerManager<TKey>> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc/>
        public void RegisterTimer<TServiceInterface>(TKey key, Expression<Func<TServiceInterface, Task>> methodExpression, int delayMinutes) where TServiceInterface : class
        {
            var (method, methodArgs) = MethodUtils.ExtractMethodInfoAndArgs(methodExpression);

            var serviceType = typeof(TServiceInterface);
            var timers = _serviceTimers.GetOrAdd(serviceType, _ => new ConcurrentDictionary<TKey, CancellationTokenSource>());

            if (timers.TryGetValue(key, out var existingCts))
            {
                _logger.LogInformation("RegisterTimerAsync: Timer for key {Key} already exists. Restarting the timer.", key);

                existingCts.Cancel();
                existingCts.Dispose();
                timers.TryRemove(key, out _);
            }
            else
            {
                _logger.LogInformation("RegisterTimerAsync: No existing timer found for key {Key}. Registering a new timer.", key);
            }

            var cts = new CancellationTokenSource();
            var timerTask = Task.Delay(TimeSpan.FromMinutes(delayMinutes), cts.Token).ContinueWith(async _ =>
            {
                if (!cts.Token.IsCancellationRequested)
                {
                    _logger.LogInformation("Timer for key {Key} has expired. Executing the action.", key);
                    try
                    {
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var service = scope.ServiceProvider.GetRequiredService<TServiceInterface>();

                            await (Task)method.Invoke(service, methodArgs);
                            _logger.LogInformation("Action executed successfully for key {Key}.", key);
                        }
                        _logger.LogInformation("Action executed successfully for key {Key}.", key);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error executing action for key {Key}.", key);
                    }
                    finally
                    {
                        await CancelTimerAsync<TServiceInterface>(key);
                        _logger.LogInformation("Timer for key {Key} has been removed from the dictionary.", key);
                    }
                }
                else
                {
                    _logger.LogInformation("Timer for key {Key} was cancelled before expiration.", key);
                }
            }, TaskScheduler.Default);

            timers[key] = cts;
            _logger.LogInformation("Timer for key {Key} has been registered with a delay of {DelayMinutes} minutes.", key, delayMinutes);
        }

        /// <inheritdoc/>
        public Task CancelTimerAsync<TServiceInterface>(TKey key)
        {
            var serviceType = typeof(TServiceInterface);
            if (_serviceTimers.TryGetValue(serviceType, out var timers))
            {
                if (timers.TryRemove(key, out var cts))
                {
                    _logger.LogInformation("CancelTimerAsync: Cancelling and disposing timer for key {Key}.", key);
                    cts.Cancel();
                    cts.Dispose();
                }
                else
                {
                    _logger.LogWarning("CancelTimerAsync: No timer found for key {Key} to cancel.", key);
                }
            }
            else
            {
                _logger.LogWarning("CancelTimerAsync: No timers found for service type {ServiceType}.", serviceType);
            }

            return Task.CompletedTask;
        }
    }

}
