
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



namespace cfg.blueprint
{

public sealed partial class EnumField :  Bright.Config.BeanBase 
{
    public EnumField(JSONNode _json) 
    {
        { if(!_json["name"].IsString) { throw new SerializationException(); }  Name = _json["name"]; }
        { if(!_json["value"].IsNumber) { throw new SerializationException(); }  Value = _json["value"]; }
    }

    public EnumField(string name, int value ) 
    {
        this.Name = name;
        this.Value = value;
    }

    public static EnumField DeserializeEnumField(JSONNode _json)
    {
        return new blueprint.EnumField(_json);
    }

    public string Name { get; private set; }
    public int Value { get; private set; }

    public const int ID = 1830049470;
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
        + "Name:" + Name + ","
        + "Value:" + Value + ","
        + "}";
    }
    }
}