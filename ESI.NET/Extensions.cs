using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace ESI.NET
{
    public static class Extensions
    {
        public static string ToEsiValue(this Enum e)
        {
            var enums = e.ToString();
            if (enums.Contains(", "))
            {
                var values = enums.Replace(" ", "").Split(',');
                var newValues = new List<string>();
                for (int i = 0; i < values.Length; i++)
                    newValues.Add(Enum.Parse(e.GetType(), values[i]).GetType().GetTypeInfo().DeclaredMembers.SingleOrDefault(x => x.Name == values[i].ToString())
                    ?.GetCustomAttribute<EnumMemberAttribute>(true)?.Value);

                return string.Join(",", newValues);
            }
            else
                return e.GetType().GetTypeInfo().DeclaredMembers.SingleOrDefault(x => x.Name == e.ToString())
                    ?.GetCustomAttribute<EnumMemberAttribute>(false)?.Value;
        }
    }
}
