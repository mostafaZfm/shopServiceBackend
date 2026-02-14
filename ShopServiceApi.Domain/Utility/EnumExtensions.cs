using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;

namespace ShopServiceApi.Domain.Utility
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())[0]
                            .GetCustomAttribute<DisplayAttribute>()?
                            .Name ?? enumValue.ToString();
        }
    }
}
