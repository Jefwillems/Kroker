namespace Kroker.Core.Extensions;

public static class QueueExtensions
{
    public static Queue<T> DequeueUntil<T>(this Queue<T> self, Func<T, bool> predicate)
    {
        var queue = new Queue<T>();
        while (self.Count > 0 && !predicate(self.Peek()))
        {
            queue.Enqueue(self.Dequeue());
        }

        return queue;
    }
}