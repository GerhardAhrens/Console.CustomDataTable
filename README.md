# Custom DataTable und Custom DataRow

![NET](https://img.shields.io/badge/NET-8.0-green.svg)
![License](https://img.shields.io/badge/License-MIT-blue.svg)
![VS2022](https://img.shields.io/badge/Visual%20Studio-2022-white.svg)
![Version](https://img.shields.io/badge/Version-1.0.2025.1-yellow.svg)]

Wie viele Klassen kann auch vom DataTable als auch vom DataRow abgeleitet werden. Durch diese Ableitung ergeben sich vielefältige Erweiterungsmöglichkeiten.

Verwendung eines Custom DataTable
```csharp
MyDataTable table = new MyDataTable();
table.TableName = "CustomDataTable";
table.MyDataRowChanged += new MyDataRowChangedDlgt(OnMyDataRowChanged);
MyDataRow row = table.GetNewRow();
row.DatumTyp = new DateTime(1960,6,28);
table.Add(row);

row = table.GetNewRow();
row.DatumTyp = new DateTime(2025, 7, 14);
table.Add(row);
table.AcceptChanges();

MyDataRow row1 = (MyDataRow)table.Rows[1];
row1.SetField<string>("TextTyp", "Hallo");
MyDataRow cloneRow = table.Clone(1);
int count = table.Count;
table.WriteXml(Path.Combine(AppContext.BaseDirectory, "TestCustomTable.xlm"));

foreach (MyDataRow myRow in table.Rows)
{
    DataRowState rowstate = myRow.RowState;
    Guid colValue = myRow.Field<Guid>("Id");
    Guid id = myRow.Id;
    Console.WriteLine($"{colValue}; {id}; Datum: {myRow.DatumTyp}");
}
```
