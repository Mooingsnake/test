
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

public sealed partial class DemoD5 :  test.DemoDynamic 
{
    public DemoD5(JSONNode _json)  : base(_json) 
    {
        { if(!_json["time"].IsObject) { throw new SerializationException(); }  Time = test.DateTimeRange.DeserializeDateTimeRange(_json["time"]); }
    }

    public DemoD5(int x1, test.DateTimeRange time )  : base(x1) 
    {
        this.Time = time;
    }

    public static DemoD5 DeserializeDemoD5(JSONNode _json)
    {
        return new test.DemoD5(_json);
    }

    public test.DateTimeRange Time { get; private set; }

    public const int ID = -2138341744;
    public override int GetTypeId() => ID;

    public override void Resolve(Dictionary<string, object> _tables)
    {
        base.Resolve(_tables);
        Time?.Resolve(_tables);
    }

    public override void TranslateText(System.Func<string, string, string> translator)
    {
        base.TranslateText(translator);
        Time?.TranslateText(translator);
    }

    public override string ToString()
    {
        return "{ "
        + "X1:" + X1 + ","
        + "Time:" + Time + ","
        + "}";
    }
    }
}
