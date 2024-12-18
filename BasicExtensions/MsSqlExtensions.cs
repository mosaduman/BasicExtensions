
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BasicExtensions.Attribute;

namespace BasicExtensions
{
    public static class MsSqlExtensions
    {
        public static string CreateSqlTableControlScript<T>() where T : class, new() => "IF(EXISTS\n(\n    SELECT 1\n    FROM INFORMATION_SCHEMA.TABLES\n    WHERE TABLE_SCHEMA = 'dbo'\n          AND TABLE_NAME = '" + typeof(T).Name + "'\n))\n    BEGIN\n        " + MsSqlExtensions.GetTableAlter<T>() + "    END;\n    ELSE\n    BEGIN\n       " + MsSqlExtensions.GetTableCreate<T>() + "\n    END;";

        private static string GetTableCreate<T>() where T : class, new()
        {
            Type type = typeof(T);
            PropertyInfo[] array = ((IEnumerable<PropertyInfo>)type.GetProperties()).Where<PropertyInfo>((Func<PropertyInfo, bool>)(s =>
            {
                object[] customAttributes = s.GetCustomAttributes(typeof(IgnoreColumnAttribute), false);
                return customAttributes == null || customAttributes.Length == 0;
            })).ToArray<PropertyInfo>();
            string str = "CREATE TABLE [" + type.Name + "] ( \n";
            for (int index = 0; index < array.Length; ++index)
            {
                string columnScript = MsSqlExtensions.GetColumnScript(array[index]);
                if (!string.IsNullOrWhiteSpace(columnScript))
                {
                    str += columnScript;
                    if (array.Length - 1 != index)
                        str += " , \n";
                }
            }
            return str + " );";
        }

        private static string GetTableAlter<T>()
        {
            Type type = typeof(T);
            PropertyInfo[] array = ((IEnumerable<PropertyInfo>)type.GetProperties()).Where<PropertyInfo>((Func<PropertyInfo, bool>)(s =>
            {
                object[] customAttributes = s.GetCustomAttributes(typeof(IgnoreColumnAttribute), false);
                return customAttributes == null || customAttributes.Length == 0;
            })).ToArray<PropertyInfo>();
            string tableAlter = "";
            for (int index = 0; index < array.Length; ++index)
            {
                PropertyInfo property = array[index];
                string alterColumn = MsSqlExtensions.GetAlterColumn(type.Name, property);
                if (!string.IsNullOrWhiteSpace(alterColumn))
                    tableAlter = tableAlter + alterColumn + " \n";
            }
            return tableAlter;
        }

        private static bool IsNullable(PropertyInfo property) => (property.GetCustomAttributes(typeof(RequiredColumnAttribute), false) == null || property.GetCustomAttributes(typeof(RequiredColumnAttribute), false).Length == 0) && (property.PropertyType.IsGenericType && property.PropertyType.Name.ToLower().Contains("nullable") || property.PropertyType.Name == typeof(string).Name);

        private static string GetPropertyName(PropertyInfo property)
        {
            if (!property.PropertyType.IsGenericType)
                return property.PropertyType.Name;
            return !property.PropertyType.Name.ToLower().Contains("nullable") ? "" : property.PropertyType.GenericTypeArguments[0].Name;
        }

        private static bool IsEnumType(PropertyInfo property)
        {
            if (!property.PropertyType.IsGenericType)
                return property.PropertyType.IsEnum;
            return property.PropertyType.Name.ToLower().Contains("nullable") && property.PropertyType.GenericTypeArguments[0].IsEnum;
        }

        private static bool IsValueType(PropertyInfo property)
        {
            if (!property.PropertyType.IsGenericType)
                return property.PropertyType.IsValueType;
            return property.PropertyType.Name.ToLower().Contains("nullable") && property.PropertyType.GenericTypeArguments[0].IsValueType;
        }

        private static string GetColumnType(PropertyInfo property)
        {
            if (property.CanRead && property.CanWrite)
            {
                if (MsSqlExtensions.IsEnumType(property))
                    return "INT";
                if (MsSqlExtensions.IsValueType(property))
                {
                    string propertyName = MsSqlExtensions.GetPropertyName(property);
                    if (propertyName == typeof(DateTime).Name)
                        return "DATETIME";
                    if (propertyName == typeof(int).Name || propertyName == typeof(short).Name)
                        return "INT";
                    if (propertyName == typeof(long).Name)
                        return "BIGINT";
                    if (propertyName == typeof(double).Name || propertyName == typeof(float).Name)
                        return "FLOAT";
                    if (propertyName == typeof(Decimal).Name)
                        return "DECIMAL";
                    if (propertyName == typeof(bool).Name)
                        return "BIT";
                }
                else if (property.PropertyType.Name == typeof(string).Name)
                    return "NVARCHAR";
            }
            return string.Empty;
        }

        private static string GetColumnScript(PropertyInfo property)
        {
            if (string.IsNullOrWhiteSpace(MsSqlExtensions.GetColumnType(property)))
                return "";
            return " [" + property.Name + "] [" + MsSqlExtensions.GetColumnType(property) + "] " + MsSqlExtensions.GetColumnTypeLength(property) + " " + (MsSqlExtensions.IsNullable(property) ? "" : "NOT") + " NULL ";
        }

        private static string GetColumnTypeLength(PropertyInfo property)
        {
            if (property.CanRead && property.CanWrite)
            {
                if (property.PropertyType.Name == typeof(string).Name)
                {
                    object[] customAttributes = property.GetCustomAttributes(typeof(ColumnLengthAttribute), false);
                    int num = 64;
                    if (customAttributes != null && customAttributes.Length != 0)
                        num = (customAttributes[0] as ColumnLengthAttribute).Length;
                    return string.Format(" ({0}) ", (object)num);
                }
                if (MsSqlExtensions.GetPropertyName(property) == typeof(Decimal).Name)
                {
                    object[] customAttributes = property.GetCustomAttributes(typeof(ColumnLengthAttribute), false);
                    int num1 = 15;
                    int num2 = 3;
                    if (customAttributes != null && customAttributes.Length != 0)
                    {
                        num1 = (customAttributes[0] as ColumnLengthAttribute).Precision;
                        num2 = (customAttributes[0] as ColumnLengthAttribute).Scale;
                    }
                    return string.Format(" ({0},{1}) ", (object)num1, (object)num2);
                }
            }
            return "";
        }

        private static string GetAlterColumn(string tableName, PropertyInfo property)
        {
            string columnType = MsSqlExtensions.GetColumnType(property);
            if (string.IsNullOrWhiteSpace(columnType))
                return "";
            string name = property.Name;
            string str1 = string.Empty;
            string str2 = columnType;
            bool flag = MsSqlExtensions.IsNullable(property);
            if (columnType == "NVARCHAR")
            {
                object[] customAttributes = property.GetCustomAttributes(typeof(ColumnLengthAttribute), false);
                int num = 64;
                if (customAttributes != null && customAttributes.Length != 0)
                    num = (customAttributes[0] as ColumnLengthAttribute).Length;
                str1 = string.Format("AND CHARACTER_MAXIMUM_LENGTH = {0}", (object)num);
                str2 = string.Format("NVARCHAR ({0})", (object)num);
            }
            else if (columnType == "DECIMAL")
            {
                object[] customAttributes = property.GetCustomAttributes(typeof(ColumnLengthAttribute), false);
                int num1 = 15;
                int num2 = 3;
                if (customAttributes != null && customAttributes.Length != 0)
                {
                    num1 = (customAttributes[0] as ColumnLengthAttribute).Precision;
                    num2 = (customAttributes[0] as ColumnLengthAttribute).Scale;
                }
                str1 = string.Format("AND NUMERIC_PRECISION = {0} AND NUMERIC_SCALE = {1}", (object)num1, (object)num2);
                str2 = string.Format("DECIMAL ({0},{1})", (object)num1, (object)num2);
            }
            return "IF(EXISTS(SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE table_schema = 'dbo' AND table_name = '" + tableName + "' AND COLUMN_NAME = '" + name + "'))\nBEGIN\n    IF(NOT EXISTS( SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE table_schema = 'dbo' AND table_name = '" + tableName + "' AND COLUMN_NAME = '" + name + "' AND DATA_TYPE = '" + MsSqlExtensions.GetColumnType(property) + "' AND IS_NULLABLE='" + (flag ? "YES" : "NO") + "' " + str1 + " )) \n    BEGIN\n        ALTER TABLE " + tableName + " ALTER COLUMN " + name + " " + str2 + " " + (flag ? "" : "NOT") + " NULL;\n    END;\nEND;\nELSE\nBEGIN\n    ALTER TABLE " + tableName + " ADD " + name + " " + str2 + " " + (flag ? "" : "NOT") + " NULL;\nEND;";
        }
    }
}
