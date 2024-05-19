namespace CurrencyConverter.Utils
{
    public static class RetryHelper
    {
        public static async Task<T> Retry<T>(Func<Task<T>> action, int maxRetryAttempts = 3, int delayMilliseconds = 1000)
        {
            int retryCount = 0;

            do
            {
                try
                {
                    T result = await action();
                    return result;
                }
                catch(TimeoutException)
                {
                    retryCount++;
                    await Task.Delay(delayMilliseconds);

                }
            } while (retryCount < maxRetryAttempts);

            throw new Exception($"Failed to execute action after {maxRetryAttempts} attempts.");
        }
    }
}
