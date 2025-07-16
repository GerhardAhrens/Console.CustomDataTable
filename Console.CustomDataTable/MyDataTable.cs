//-----------------------------------------------------------------------
// <copyright file="MyDataTable.cs" company="Lifeprojects.de">
//     Class: MyDataTable
//     Copyright © Lifeprojects.de 2024
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>26.08.2024 20:31:37</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace Console.CustomDataTable
{
    using System;
    using System.Data;
    using System.Reflection;
    using System.Text.Json;
    using System.Text.RegularExpressions;

    using static System.Text.Json.JsonElement;

    public delegate void MyDataRowChanged(MyDataTable sender, MyDataRowChangedEventArgs args);

    public class MyDataRowChangedEventArgs
    {
        protected DataRowAction action;
        protected MyDataRow row;

        public DataRowAction Action
        {
            get { return action; }
        }

        public MyDataRow Row
        {
            get { return row; }
        }

        public MyDataRowChangedEventArgs(DataRowAction action, MyDataRow row)
        {
            this.action = action;
            this.row = row;
        }

        public override string ToString()
        {
            return $"$Action:{this.Action}; Row Column:{this.Row.ItemArray.Length}";
        }
    }

    public class MyDataTable : DataTable
    {
        public event MyDataRowChanged MyDataRowChanged;

        public MyDataTable()
        {
            PropertyInfo[] piArray = this.GetRowType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo pinfo in piArray)
            {
                if (pinfo.DeclaringType.Name == this.GetRowType().Name)
                {
                    base.Columns.Add(new DataColumn(pinfo.Name, pinfo.PropertyType));
                }
            }
        }

        public MyDataRow this[int idx]
        {
            get 
            {
                return (MyDataRow)base.Rows[idx]; 
            }
        }

        public int Count
        {
            get { return base.Rows.Count; }
        }

        public void Add(MyDataRow row)
        {
            base.Rows.Add(row);
        }

        public void AddClone(MyDataRow row)
        {
            base.ImportRow(row);
        }

        public void Remove(MyDataRow row)
        {
            base.Rows.Remove(row);
        }

        public MyDataRow GetNewRow()
        {
            MyDataRow row = (MyDataRow)NewRow();

            return row;
        }

        public MyDataRow Clone(int rowPos, bool newId = false)
        {
            MyDataRow row = (MyDataRow)NewRow();
            row.ItemArray = base.Rows[rowPos].ItemArray;
            if (newId == true)
            {
                row.SetField<Guid>("Id", Guid.NewGuid());
            }

            base.Rows.Add(row);
            base.AcceptChanges();
            return row;
        }

        public MyDataRow Clone(MyDataRow rowOriginal, bool newId = false)
        {
            MyDataRow row = (MyDataRow)NewRow();
            row.ItemArray = rowOriginal.ItemArray;
            if (newId == true)
            {
                row.SetField<Guid>("Id", Guid.NewGuid());
            }

            base.Rows.Add(row);
            base.AcceptChanges();
            return row;
        }

        public DataView AsDataView()
        {
            return new DataView(this);
        }

        public DataView AsDataView(DataViewRowState rowState)
        {
            return new DataView(this,null,null, rowState);
        }

        public DataView AsDataView(string rowFilter = null, string sortColumn = null, DataViewRowState rowState = DataViewRowState.Unchanged)
        {
            return new DataView(this, rowFilter, sortColumn, rowState);
        }

        public void WriteJson(string jsonFilename)
        {
            if (this == null)
            {
                return;
            }

            var data = this.Rows.OfType<DataRow>().Select(row => this.Columns.OfType<DataColumn>().ToDictionary(col => col.ColumnName, c => row[c]));

            string jsonTest = System.Text.Json.JsonSerializer.Serialize(data);
            File.WriteAllText(jsonFilename, jsonTest);
        }

        public DataTable ReadJson(string jsonFilename)
        {
            DataTable dataTable = new();
            if (string.IsNullOrWhiteSpace(jsonFilename))
            {
                return dataTable;
            }

            string jsonString = File.ReadAllText(jsonFilename);

            JsonElement data = JsonSerializer.Deserialize<JsonElement>(jsonString);
            if (data.ValueKind != JsonValueKind.Array)
            {
                return dataTable;
            }

            ArrayEnumerator dataArray = data.EnumerateArray();
            JsonElement firstObject = dataArray.First();
            ObjectEnumerator firstRowObject = firstObject.EnumerateObject();
            foreach (JsonProperty element in firstRowObject)
            {
                dataTable.Columns.Add(element.Name);
            }

            foreach (JsonElement obj in dataArray)
            {
                ObjectEnumerator objProperties = obj.EnumerateObject();
                DataRow newRow = dataTable.NewRow();
                foreach (JsonProperty item in objProperties)
                {
                    newRow[item.Name] = item.Value;
                }

                dataTable.Rows.Add(newRow);
            }

            return dataTable;
        }

        protected override Type GetRowType()
        {
            return typeof(MyDataRow);
        }

        protected override DataRow NewRowFromBuilder(DataRowBuilder builder)
        {
            return new MyDataRow(builder);
        }

        protected override void OnRowChanged(DataRowChangeEventArgs e)
        {
            base.OnRowChanged(e);
            MyDataRowChangedEventArgs args = new MyDataRowChangedEventArgs(e.Action, (MyDataRow)e.Row);
            OnMyDataRowRowChanged(args);
        }

        protected virtual void OnMyDataRowRowChanged(MyDataRowChangedEventArgs args)
        {
            if (this.MyDataRowChanged != null)
            {
                this.MyDataRowChanged(this, args);
            }
        }
    }
}
