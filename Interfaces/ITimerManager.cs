using System.Linq.Expressions;

namespace EventSeller.Services.Interfaces
{
    public interface ITimerManager<TKey>
    {
        /// <summary>
        ///  MEthod
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="key"></param>
        /// <param name="methodExpression"></param>
        /// <param name="delayMinutes"></param>
        /// <remarks>
        ///     RegisterTimer<IExampleService>(
        ///          KeyValue
        ///         , service => service.AnyMethod(arg1, arg2)
        ///         , 10);
        /// </remarks>
        void RegisterTimer<TService>(TKey key, Expression<Func<TService, Task>> methodExpression, int delayMinutes) where TService : class;
        Task CancelTimerAsync<TService>(TKey key);
    }
}
