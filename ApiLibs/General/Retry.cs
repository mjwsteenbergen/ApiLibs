using System;
using System.Threading.Tasks;

namespace ApiLibs
{
    public class Retry
    {
        public static RetryBuilder<T> Of<T>(Func<Task<T>> function) => new(function);
    }

    public class RetryBuilder<T>
    {
        private readonly Func<Task<T>> func;

        public RetryBuilder(Func<Task<T>> f) => this.func = f;

        public Task<T> OrReturn(T value) => OrReturn(() => value);

        public async Task<T> OrReturn(Func<T> func)
        {
            try {
                return await this.func();
            } catch(Exception e) {
                Console.Error.Write("\"" + e.Message + "\" has occured. Returning default value");
                return func();
            }
        }
    }
}