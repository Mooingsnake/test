
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using Bright.Serialization;
using System.Collections.Generic;
using SimpleJSON;



namespace cfg.test
{

public sealed partial class DemoSingletonType :  Bright.Config.BeanBase 
{
    public DemoSingletonType(JSONNode _json) 
    {
        { if(!_json["id"].IsNumber) { throw new SerializationException(); }  Id = _json["id"]; }
        { if(!_json["name"]["key"].IsString) { throw new SerializationException(); }  Name_l10n_key = _json["name"]["key"]; if(!_json["name"]["text"].IsString) { throw new SerializationException(); }  Name = _json["name"]["text"]; }
        { if(!_json["date"].IsObject) { throw new SerializationException(); }  Date = test.DemoDynamic.DeserializeDemoDynamic(_json["date"]); }
    }

    public DemoSingletonType(int id, string name, test.DemoDynamic date ) 
    {
        this.Id = id;
        this.Name = name;
        this.Date = date;
    }

    public static DemoSingletonType DeserializeDemoSingletonType(JSONNode _json)
    {
        return new test.DemoSingletonType(_json);
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public string Name_l10n_key { get; }
    public test.DemoDynamic Date { get; private set; }

    public const int ID = 539196998;
    public override int GetTypeId() => ID;

    public  void Resolve(Dictionary<string, object> _tables)
    {
        Date?.Resolve(_tables);
    }

    public  void TranslateText(System.Func<string, string, string> translator)
    {
        Name = translator(Name_l10n_key, Name);
        Date?.TranslateText(translator);
    }

    public override string ToString()
    {
        return "{ "
        + "Id:" + Id + ","
        + "Name:" + Name + ","
        + "Date:" + Date + ","
        + "}";
    }
    }
}
