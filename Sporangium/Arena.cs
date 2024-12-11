using System.Collections.Generic;

namespace Sporangium;
public class Arena<T> where T : notnull
{
    public int Count { get; private set; }
    public T? this[in ID id] => Contains(id) ? _entries[id.Index].Item : default;

    public bool Contains(in ID id) => id.IsValid && id.Version == _entries[id.Index].Version;

    public ID Add(T item)
    {
        Count++;
        if (_removed.TryDequeue(out int index))
        {
            Entry entry = _entries[index];
            _entries[index] = entry.Set(item);
            return new(index, entry.Version);
        }
        _entries.Add(new(0, item));
        return new(_entries.Count - 1, 0);
    }

    public bool Remove(in ID id)
    {
        if (Contains(id))
        {
            _entries[id.Index] = _entries[id.Index].Reset();
            _removed.Enqueue(id.Index);
            return true;
        }
        return false;
    }

    private readonly List<Entry> _entries = [default];
    private readonly Queue<int> _removed = [];

    private readonly record struct Entry(int Version, T? Item)
    {
        public Entry Set(T item) => new(Version, item);
        public Entry Reset() => new(Version + 1, default);
    }
}
