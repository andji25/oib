using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

public class JsonRepository<T> : IRepository<T> where T : class
{
    private readonly string _path;

    public JsonRepository(string path)
    {
        _path = path;
        if (!File.Exists(_path))
            File.WriteAllText(_path, "[]");
    }

    public IEnumerable<T> GetAll()
    {
        try
        {
            var json = File.ReadAllText(_path);
            return JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
        }
        catch
        {
            return new List<T>();
        }
    }

    public void Add(T item)
    {
        var list = GetAll().ToList();
        list.Add(item);
        Save(list);
    }

    public void Update(T item)
    {
        var list = GetAll().ToList();
        var idProp = typeof(T).GetProperty("Id");
        var id = idProp.GetValue(item);

        for (int i = 0; i < list.Count; i++)
        {
            if (idProp.GetValue(list[i]).Equals(id))
            {
                list[i] = item;
                break;
            }
        }
        Save(list);
    }

    private void Save(List<T> list)
    {
        File.WriteAllText(
            _path,
            JsonConvert.SerializeObject(list, Formatting.Indented)
        );
    }
}
