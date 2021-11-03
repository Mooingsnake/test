
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



namespace cfg.ai
{

public sealed partial class UeTimeLimit :  ai.Decorator 
{
    public UeTimeLimit(JSONNode _json)  : base(_json) 
    {
        { if(!_json["limit_time"].IsNumber) { throw new SerializationException(); }  LimitTime = _json["limit_time"]; }
    }

    public UeTimeLimit(int id, string node_name, ai.EFlowAbortMode flow_abort_mode, float limit_time )  : base(id,node_name,flow_abort_mode) 
    {
        this.LimitTime = limit_time;
    }

    public static UeTimeLimit DeserializeUeTimeLimit(JSONNode _json)
    {
        return new ai.UeTimeLimit(_json);
    }

    public float LimitTime { get; private set; }

    public const int ID = 338469720;
    public override int GetTypeId() => ID;

    public override void Resolve(Dictionary<string, object> _tables)
    {
        base.Resolve(_tables);
    }

    public override void TranslateText(System.Func<string, string, string> translator)
    {
        base.TranslateText(translator);
    }

    public override string ToString()
    {
        return "{ "
        + "Id:" + Id + ","
        + "NodeName:" + NodeName + ","
        + "FlowAbortMode:" + FlowAbortMode + ","
        + "LimitTime:" + LimitTime + ","
        + "}";
    }
    }
}
