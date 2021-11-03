
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

public sealed partial class ExcelFromJsonMultiRow :  Bright.Config.BeanBase 
{
    public ExcelFromJsonMultiRow(JSONNode _json) 
    {
        { if(!_json["id"].IsNumber) { throw new SerializationException(); }  Id = _json["id"]; }
        { if(!_json["x"].IsNumber) { throw new SerializationException(); }  X = _json["x"]; }
        { var _json1 = _json["items"]; if(!_json1.IsArray) { throw new SerializationException(); } Items = new System.Collections.Generic.List<test.TestRow>(_json1.Count); foreach(JSONNode __e in _json1.Children) { test.TestRow __v;  { if(!__e.IsObject) { throw new SerializationException(); }  __v = test.TestRow.DeserializeTestRow(__e); }  Items.Add(__v); }   }
    }

    public ExcelFromJsonMultiRow(int id, int x, System.Collections.Generic.List<test.TestRow> items ) 
    {
        this.Id = id;
        this.X = x;
        this.Items = items;
    }

    public static ExcelFromJsonMultiRow DeserializeExcelFromJsonMultiRow(JSONNode _json)
    {
        return new test.ExcelFromJsonMultiRow(_json);
    }

    public int Id { get; private set; }
    public int X { get; private set; }
    public System.Collections.Generic.List<test.TestRow> Items { get; private set; }

    public const int ID = 715335694;
    public override int GetTypeId() => ID;

    public  void Resolve(Dictionary<string, object> _tables)
    {
        foreach(var _e in Items) { _e?.Resolve(_tables); }
    }

    public  void TranslateText(System.Func<string, string, string> translator)
    {
        foreach(var _e in Items) { _e?.TranslateText(translator); }
    }

    public override string ToString()
    {
        return "{ "
        + "Id:" + Id + ","
        + "X:" + X + ","
        + "Items:" + Bright.Common.StringUtil.CollectionToString(Items) + ","
        + "}";
    }
    }
}
