# Custom DataTable und Custom DataRow

![NET](https://img.shields.io/badge/NET-8.0-green.svg)
![License](https://img.shields.io/badge/License-MIT-blue.svg)
![VS2022](https://img.shields.io/badge/Visual%20Studio-2022-white.svg)
![Version](https://img.shields.io/badge/Version-1.0.2025.1-yellow.svg)]

Wie viele Klassen kann auch vom DataTable als auch vom DataRow abgeleitet werden. Durch diese Ableitung ergeben sich vielefältige Erweiterungsmöglichkeiten.

**Erstellen einer Custom DataTable**
```csharp
MyDataTable table = new MyDataTable();
table.TableName = "CustomDataTable";
table.MyDataRowChanged += new MyDataRowChanged(OnMyDataRowChanged);
```

**Eine neue Row hinzufügen**</br>
Da es nun auch eine Custom DataRow gibt, können nach aussen normale Properties verwendet werden.
```csharp
MyDataRow row = table.GetNewRow();
row.DatumTyp = new DateTime(1960,6,28);
table.Add(row);

row = table.GetNewRow();
row.DatumTyp = new DateTime(2025, 7, 14);
row.IntTyp = 42;
table.Add(row);
table.AcceptChanges();
```

**DataTable kann als DataView zurückgegeben werden**</br>
Über DataView können daten aus dem DataTable auch sortiert oder über einen Filter zurückgegeben werden.
```csharp
DataView dv = table.AsDataView(DataViewRowState.CurrentRows);
int dvCount = dv.Count;

DataView dv2 = table.AsDataView(rowFilter: "IntTyp = 42");
int dv2Count = dv2.Count;
```

**Clone einer DataRow erstellen**
```csharp
MyDataRow cloneRow = table.Clone(1, true);
cloneRow.TextTyp = "hallo Charlie";
table.AcceptChanges();
```
**DataTable als JSON Speichern**</br>
*Hinweis* beim schreiben der JSON Datei gehen alle Typ-Informationen verloren. Es wird alles nur als String gespeichert.
```csharp
table.WriteJson(Path.Combine(AppContext.BaseDirectory, "TestCustomTable.json"));
```

**DataTable aus einem JSON File erstellen**</br>
*Hinweis* beim Lesen der JSON Datei wird eine DataTable erstellt bei dem alle Columns als String festgelegt werden. Daher sind keine weiteren Typ-Informationen vorhanden.
```csharp
DataTable dtJson = table.ReadJson(Path.Combine(AppContext.BaseDirectory, "TestCustomTable.json"));
```
