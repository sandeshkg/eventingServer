using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace Utilities
{
    public class DataTableProcesses
    {
        public string GetThisColumnValue(string columnName, DataRow row)
        {
            return (row.Table.Columns.Contains(columnName)) ? row[columnName].ToString() : string.Empty;
        }

        
    }
}
