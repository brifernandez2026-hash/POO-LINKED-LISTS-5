namespace Backend;

public class DoublyLinkedList<T> where T : notnull, IComparable<T>
{
    private DoubleNode<T>? head;
    private DoubleNode<T>? tail;
    private bool isDescending;

    public int Count { get; private set; }

    public void Add(T value)
    {
        // Keep the list ascending when inserting.
        if (isDescending)
        {
            ReverseLinks();
            isDescending = false;
        }

        var node = new DoubleNode<T>(value);

        if (head == null)
        {
            head = node;
            tail = node;
            Count++;
            return;
        }

        var current = head;

        while (current != null && current.Data.CompareTo(value) <= 0)
        {
            current = current.Next;
        }

        if (current == head)
        {
            node.Next = head;
            head.Prev = node;
            head = node;
        }
        else if (current == null)
        {
            node.Prev = tail;
            tail!.Next = node;
            tail = node;
        }
        else
        {
            node.Prev = current.Prev;
            node.Next = current;
            current.Prev!.Next = node;
            current.Prev = node;
        }

        Count++;
    }

    public IEnumerable<T> Forward()
    {
        var current = head;

        while (current != null)
        {
            yield return current.Data;
            current = current.Next;
        }
    }

    public IEnumerable<T> Backward()
    {
        var current = tail;

        while (current != null)
        {
            yield return current.Data;
            current = current.Prev;
        }
    }

    public void SortDescending()
    {
        if (!isDescending)
        {
            ReverseLinks();
            isDescending = true;
        }
    }

    private void ReverseLinks()
    {
        // Reverse the links between nodes.
        var current = head;

        while (current != null)
        {
            var next = current.Next;
            current.Next = current.Prev;
            current.Prev = next;
            current = next;
        }

        (head, tail) = (tail, head);
    }

    public Dictionary<T, int> GetFrequencies()
    {
        var frequencies = new Dictionary<T, int>();

        foreach (var value in Forward())
        {
            if (frequencies.ContainsKey(value))
                frequencies[value]++;
            else
                frequencies[value] = 1;
        }

        return frequencies;
    }

    public List<T> GetModes()
    {
        var frequencies = GetFrequencies();
        var modes = new List<T>();
        int highestCount = 0;

        foreach (var item in frequencies)
        {
            if (item.Value > highestCount)
            {
                highestCount = item.Value;
                modes.Clear();
                modes.Add(item.Key);
            }
            else if (item.Value == highestCount)
            {
                modes.Add(item.Key);
            }
        }

        return modes;
    }

    public bool Exists(T value)
    {
        var current = head;

        while (current != null)
        {
            if (EqualityComparer<T>.Default.Equals(current.Data, value))
                return true;

            current = current.Next;
        }

        return false;
    }

    public bool RemoveFirst(T value)
    {
        var current = head;

        while (current != null)
        {
            if (EqualityComparer<T>.Default.Equals(current.Data, value))
            {
                RemoveNode(current);
                return true;
            }

            current = current.Next;
        }

        return false;
    }

    public int RemoveAll(T value)
    {
        int removed = 0;
        var current = head;

        while (current != null)
        {
            var next = current.Next;

            if (EqualityComparer<T>.Default.Equals(current.Data, value))
            {
                RemoveNode(current);
                removed++;
            }

            current = next;
        }

        return removed;
    }

    private void RemoveNode(DoubleNode<T> node)
    {
        // Update the links after removing a node.
        if (node.Prev == null)
            head = node.Next;
        else
            node.Prev.Next = node.Next;

        if (node.Next == null)
            tail = node.Prev;
        else
            node.Next.Prev = node.Prev;

        node.Next = null;
        node.Prev = null;
        Count--;
    }
}
