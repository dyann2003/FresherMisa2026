using System;
using System.Collections.Generic;
using System.Text;

namespace FresherMisa2026.Entities.Extensions
{
    public class ConfigTable : Attribute
    {
        public bool HasDeletedColumn { get; set; } = false;

        public string UniqueColumns { get; set; } = string.Empty;

        public string TableName { get; set; } = string.Empty;

        public ConfigTable(string tableName = "", bool hasDeletedColumn = false, string uniqueColumns = "")
        {
            TableName = tableName;

            HasDeletedColumn = hasDeletedColumn;

            UniqueColumns = uniqueColumns;
        }
    }
}
