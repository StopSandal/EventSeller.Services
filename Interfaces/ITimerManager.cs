using System.Linq.Expressions;

namespace EventSeller.Services.Interfaces
{
    public interface ITimerManager<TKey>
    {
        /// <summary>
        /// Registers a timer to execute a specified method on a service after a given delay.
        /// If a timer already exists for the given key, it will be restarted.
        /// </summary>
        /// <typeparam name="TServiceInterface">The type of the service interface containing the method to be executed.</typeparam>
        /// <param name="key">The key used to identify the timer.</param>
        /// <param name="methodExpression">An expression representing the method to be executed.</param>
        /// <param name="delayMinutes">The delay in minutes before the method is executed.</param>
        /// <example>
        /// <code>
        ///     RegisterTimer{IExampleService}(
        ///          KeyValue
        ///         , service => service.AnyMethod(arg1, arg2)
        ///         , 10);
        /// </code> 
        /// </example>

        void RegisterTimer<TServiceInterface>(TKey key, Expression<Func<TServiceInterface, Task>> methodExpression, int delayMinutes) where TServiceInterface : class;

        /// <summary>
        /// Cancels a registered timer for a specified service and key.
        /// </summary>
        /// <typeparam name="TServiceInterface">The type of the service whose timer needs to be cancelled.</typeparam>
        /// <param name="key">The key used to identify the timer.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        Task CancelTimerAsync<TServiceInterface>(TKey key);
    }
}
