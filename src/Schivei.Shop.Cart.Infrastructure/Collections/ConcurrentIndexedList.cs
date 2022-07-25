using System.Reflection;

namespace System.Collections.Concurrent;

/// <summary>
/// Property indexed object
/// </summary>
/// <typeparam name="T"></typeparam>
public class ConcurrentIndexedList<T>
{
    private readonly PropertyInfo[] _indexers;
    private readonly ConcurrentDictionary<int, T?> _data = new();

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="indexers"></param>
    public ConcurrentIndexedList(params string[] indexers) =>
        _indexers = typeof(T).GetProperties().Where(x => indexers.Contains(x.Name)).ToArray();

    /// <summary>
    /// Get / Set item to key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public T? this[object key]
    {
        get => Get(key);
        set => Set(key, value);
    }

    private void Set(object key, T? value)
    {
        if (value is null)
            throw new ArgumentNullException(nameof(value));

        if (!CheckKey(key, value))
            throw new ArgumentException("Invalid key", nameof(key));

        Add(value);
    }

    private bool CheckKey(object key, T value)
    {
        if (_data.ContainsKey(GetPointer(key)))
            return true;

        return _indexers.Any(i => i.GetValue(value) == key);
    }

    private T? Get(object key) =>
        _data.ContainsKey(GetPointer(key)) ? _data[GetPointer(key)] : default;

    /// <summary>
    /// Add item with all indexes
    /// </summary>
    /// <param name="value"></param>
    public void Add(T? value)
    {
        for (var i = 0; i < _indexers.Length; i++)
        {
            var index = _indexers[i];
            var key = index.GetValue(value);
            if (key is not null)
                _data.AddOrUpdate(GetPointer(key), _ => value, (_, __) => value);
        }
    }

    private static int GetPointer(object key) =>
        key.GetHashCode();

    /// <summary>
    /// Removes all keys and values
    /// </summary>
    public void Clear() =>
        _data.Clear();
}
