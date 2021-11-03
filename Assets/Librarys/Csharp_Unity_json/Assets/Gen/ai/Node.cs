
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

public abstract partial class Node :  Bright.Config.BeanBase 
{
    public Node(JSONNode _json) 
    {
        { if(!_json["id"].IsNumber) { throw new SerializationException(); }  Id = _json["id"]; }
        { if(!_json["node_name"].IsString) { throw new SerializationException(); }  NodeName = _json["node_name"]; }
    }

    public Node(int id, string node_name ) 
    {
        this.Id = id;
        this.NodeName = node_name;
    }

    public static Node DeserializeNode(JSONNode _json)
    {
        string type = _json["__type__"];
        switch (type)
        {
            case "UeSetDefaultFocus": return new ai.UeSetDefaultFocus(_json);
            case "ExecuteTimeStatistic": return new ai.ExecuteTimeStatistic(_json);
            case "ChooseTarget": return new ai.ChooseTarget(_json);
            case "KeepFaceTarget": return new ai.KeepFaceTarget(_json);
            case "GetOwnerPlayer": return new ai.GetOwnerPlayer(_json);
            case "UpdateDailyBehaviorProps": return new ai.UpdateDailyBehaviorProps(_json);
            case "UeLoop": return new ai.UeLoop(_json);
            case "UeCooldown": return new ai.UeCooldown(_json);
            case "UeTimeLimit": return new ai.UeTimeLimit(_json);
            case "UeBlackboard": return new ai.UeBlackboard(_json);
            case "UeForceSuccess": return new ai.UeForceSuccess(_json);
            case "IsAtLocation": return new ai.IsAtLocation(_json);
            case "DistanceLessThan": return new ai.DistanceLessThan(_json);
            case "Sequence": return new ai.Sequence(_json);
            case "Selector": return new ai.Selector(_json);
            case "SimpleParallel": return new ai.SimpleParallel(_json);
            case "UeWait": return new ai.UeWait(_json);
            case "UeWaitBlackboardTime": return new ai.UeWaitBlackboardTime(_json);
            case "MoveToTarget": return new ai.MoveToTarget(_json);
            case "ChooseSkill": return new ai.ChooseSkill(_json);
            case "MoveToRandomLocation": return new ai.MoveToRandomLocation(_json);
            case "MoveToLocation": return new ai.MoveToLocation(_json);
            case "DebugPrint": return new ai.DebugPrint(_json);
            default: throw new SerializationException();
        }
    }

    public int Id { get; private set; }
    public string NodeName { get; private set; }


    public virtual void Resolve(Dictionary<string, object> _tables)
    {
    }

    public virtual void TranslateText(System.Func<string, string, string> translator)
    {
    }

    public override string ToString()
    {
        return "{ "
        + "Id:" + Id + ","
        + "NodeName:" + NodeName + ","
        + "}";
    }
    }
}