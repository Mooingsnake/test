
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

public sealed partial class Test3 :  Bright.Config.BeanBase 
{
    public Test3(JSONNode _json) 
    {
        { if(!_json["x"].IsNumber) { throw new SerializationException(); }  X = _json["x"]; }
        { if(!_json["y"].IsNumber) { throw new SerializationException(); }  Y = _json["y"]; }
    }

    public Test3(int x, int y ) 
    {
        this.X = x;
        this.Y = y;
    }

    public static Test3 DeserializeTest3(JSONNode _json)
    {
        return new test.Test3(_json);
    }

    public int X { get; private set; }
    public int Y { get; private set; }

    public const int ID = 638540133;
    public override int GetTypeId() => ID;

    public  void Resolve(Dictionary<string, object> _tables)
    {
    }

    public  void TranslateText(System.Func<string, string, string> translator)
    {
    }

    public override string ToString()
    {
        return "{ "
        + "X:" + X + ","
        + "Y:" + Y + ","
        + "}";
    }
    }
}
