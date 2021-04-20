using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Reflection;

namespace KURSOVA.Models
{
    public static class Constants
    {
        public const string Operator = "Оператор мережі";
        public const string RestAdmin = "Адміністратор ресторану ";
        public const string DelivAdmin = "Адміністратор служби доставки";

        public static List<string> GetListOfDescription<T>() where T : struct
        {
            Type t = typeof(T);
            return !t.IsEnum ? null : Enum.GetValues(t).Cast<Enum>().Select(x => x.GetDescription()).ToList();
        }

        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
            /* how to use
                MyEnum x = MyEnum.NeedMoreCoffee;
                string description = x.GetDescription();
            */

        }
    }

    public enum OrderStatus : int
    { 
        [Display(Name = "Формується")]
        [Description( "Формується")]
        Created =1,

        [Display(Name = "Готується")]
        [Description( "Готується")]
        InProcess =2,

        [Display(Name = "Готове до відправлення")]
        [Description( "Готове до відправлення")]
        Completed = 3,

        [Display(Name = "Доставляється")]
        [Description( "Доставляється")]
        Shipping = 4,

        [Display(Name = "Відмінено")]
        [Description( "Відмінено")]
        Canceled = 5,

        [Display(Name = "Доставлено")]
        [Description( "Доставлено")]
        Closed = 6

    }
    public enum DeliveryStatus : int
    {
        [Display(Name = "Формується")]
        [Description( "Формується")]
        Created = 1,     

        [Display(Name = "Доставляється")]
        [Description( "Доставляється")]
        Shipping = 2,

        [Display(Name = "Відмінено")]
        [Description( "Відмінено")]
        Canceled = 3,
        [Display(Name = "Доставлено")]
        [Description( "Доставлено")]
        Done = 4         

    }
    

}
