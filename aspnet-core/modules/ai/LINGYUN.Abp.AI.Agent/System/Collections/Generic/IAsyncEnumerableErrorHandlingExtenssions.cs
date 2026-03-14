using System.Runtime.CompilerServices;
using System.Threading;

namespace System.Collections.Generic;

internal static class IAsyncEnumerableErrorHandlingExtenssions
{
    public static async IAsyncEnumerable<T> WithErrorHandling<T>(
        this IAsyncEnumerable<T> source,
        Action<Exception> onError,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var enumerator = source.GetAsyncEnumerator(cancellationToken);
        await using (enumerator)
        {
            while (true)
            {
                try
                {
                    if (!await enumerator.MoveNextAsync())
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    onError(ex);
                    yield break;
                }

                yield return enumerator.Current;
            }
        }
    }
}
