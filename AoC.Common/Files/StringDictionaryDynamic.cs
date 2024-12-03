using System.Dynamic;

namespace AoC.Common.Files;

public class StringDictionaryDynamic(Dictionary<string, string?> properties) : DynamicObject
{
    public override IEnumerable<string> GetDynamicMemberNames() => properties.Keys;

    public override bool TryGetMember(GetMemberBinder binder, out object? result)
    {
        var success = properties.TryGetValue(binder.Name, out string? realResult);
        result = realResult;

        return success;
    }

    public override bool TrySetMember(SetMemberBinder binder, object? value)
    {
        if (properties.ContainsKey(binder.Name))
        {
            properties[binder.Name] = value?.ToString();
            return true;
        }

        return false;
    }
}
