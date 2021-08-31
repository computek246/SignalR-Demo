using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRDemo.Server.Helpers
{
    public static class ConvertionExtension
    {

        public static T To<T>(this object entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            return ConvertValue<T>(entity);
        }

        public static T ConvertValue<T>(object value)
        {
            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}
