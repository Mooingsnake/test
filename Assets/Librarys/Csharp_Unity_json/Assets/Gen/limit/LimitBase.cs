
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



namespace cfg.limit
{

public abstract partial class LimitBase :  Bright.Config.BeanBase 
{
    public LimitBase(JSONNode _json) 
    {
    }

    public LimitBase() 
    {
    }

    public static LimitBase DeserializeLimitBase(JSONNode _json)
    {
        string type = _json["__type__"];
        switch (type)
        {
            case "DailyLimit": return new limit.DailyLimit(_json);
            case "MultiDayLimit": return new limit.MultiDayLimit(_json);
            case "WeeklyLimit": return new limit.WeeklyLimit(_json);
            case "MonthlyLimit": return new limit.MonthlyLimit(_json);
            case "CoolDown": return new limit.CoolDown(_json);
            case "GroupCoolDown": return new limit.GroupCoolDown(_json);
            default: throw new SerializationException();
        }
    }



    public virtual void Resolve(Dictionary<string, object> _tables)
    {
    }

    public virtual void TranslateText(System.Func<string, string, string> translator)
    {
    }

    public override string ToString()
    {
        return "{ "
        + "}";
    }
    }
}
