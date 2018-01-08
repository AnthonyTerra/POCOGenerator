using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Db.Extensions
{
    public static class DataTableExtensions
    {
        public static IEnumerable<T> Cast<T>(this DataTable table, Func<string, Type, object, object> valueHandler = null) where T : new()
        {
            return ToEnumerable<T>(table, () => new T(), valueHandler);
        }

        public static IEnumerable<T> Cast<T>(this DataTable table, Func<T> instanceHandler, Func<string, Type, object, object> valueHandler = null)
        {
            return ToEnumerable<T>(table, instanceHandler, valueHandler);
        }

        public static T[] ToArray<T>(this DataTable table, Func<string, Type, object, object> valueHandler = null) where T : new()
        {
            return ToEnumerable<T>(table, () => new T(), valueHandler);
        }

        public static T[] ToArray<T>(this DataTable table, Func<T> instanceHandler, Func<string, Type, object, object> valueHandler = null)
        {
            return ToEnumerable<T>(table, instanceHandler, valueHandler);
        }

        public static List<T> ToList<T>(this DataTable table, Func<string, Type, object, object> valueHandler = null) where T : new()
        {
            return ToEnumerable<T>(table, () => new T(), valueHandler).ToList<T>();
        }

        public static List<T> ToList<T>(this DataTable table, Func<T> instanceHandler, Func<string, Type, object, object> valueHandler = null)
        {
            return ToEnumerable<T>(table, instanceHandler, valueHandler).ToList<T>();
        }

        private static T[] ToEnumerable<T>(DataTable table, Func<T> instanceHandler, Func<string, Type, object, object> valueHandler)
        {
            if (table == null)
                return null;

            if (table.Rows.Count == 0)
                return new T[0];

            Type type = typeof(T);

            var columns =
                type.GetFields(BindingFlags.Public | BindingFlags.Instance)
                    .Select(f => new
                    {
                        ColumnName = f.Name,
                        ColumnType = f.FieldType,
                        IsField = true,
                        MemberInfo = (MemberInfo)f
                    })
                    .Union(
                        type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                            .Where(p => p.CanWrite)
                            .Where(p => p.CanRead)
                            .Where(p => p.GetGetMethod(true).IsPublic)
                            .Where(p => p.GetIndexParameters().Length == 0)
                            .Select(p => new
                            {
                                ColumnName = p.Name,
                                ColumnType = p.PropertyType,
                                IsField = false,
                                MemberInfo = (MemberInfo)p
                            })
                    )
                    .Where(c => table.Columns.Contains(c.ColumnName)); // columns exist

            T[] instances = new T[table.Rows.Count];

            int index = 0;
            foreach (DataRow row in table.Rows)
            {
                T instance = instanceHandler();

                foreach (var column in columns)
                {
                    object value = row[column.ColumnName];
                    if (valueHandler != null)
                        value = valueHandler(column.ColumnName, column.ColumnType, value);

                    if (value is DBNull)
                    {
                        value = null;
                    }
                    else if (value != null && column.ColumnType != typeof(System.Type))
                    {
                        if (value.GetType() != column.ColumnType)
                        {
                            if (column.ColumnType.IsGenericType && column.ColumnType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                if (value.GetType() != Nullable.GetUnderlyingType(column.ColumnType))
                                    value = Convert.ChangeType(value, Nullable.GetUnderlyingType(column.ColumnType));
                            }
                            else
                            {
                                value = Convert.ChangeType(value, column.ColumnType);
                            }
                        }
                    }

                    if (column.IsField)
                        ((FieldInfo)column.MemberInfo).SetValue(instance, value);
                    else
                        ((PropertyInfo)column.MemberInfo).SetValue(instance, value, null);
                }

                instances[index++] = instance;
            }

            return instances;
        }
    }
}
