# Custom DataTable und Custom DataRow

![NET](https://img.shields.io/badge/NET-8.0-green.svg)
![License](https://img.shields.io/badge/License-MIT-blue.svg)
![VS2022](https://img.shields.io/badge/Visual%20Studio-2022-white.svg)
![Version](https://img.shields.io/badge/Version-1.0.2025.1-yellow.svg)

Wie viele Klassen kann auch vom DataTable als auch vom DataRow abgeleitet werden. Durch diese Ableitung ergeben sich vielefältige Erweiterungsmöglichkeiten.

Das Beispielprojekt zeigt die Möglichkeiten einer Custom DataTable und einer Custom DataRow auf. Es werden auch einige Erweiterungsmethoden gezeigt, welche die Arbeit mit DataTables und DataRows erleichtern.

## Zwei Aspekte werden in diesem Beispielprojekt gezeigt

 - Erstellen einer Custom DataTable und einer Custom DataRow
 - Extension Methods für System.CodeDom.Compiler zum generieren von Code aus einem DataTable

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

## Source Generator
In diesem beispiel wird auch ein Source Generator verwendet. Dieser erstellt automatisch die Custom DataRow Klasse, welche von der Custom DataTable Klasse verwendet wird. Es muss nur die Custom DataTable Klasse erstellt werden und der Source Generator erstellt automatisch die Custom DataRow Klasse.
```csharp
string result = table.DataTableToCode();
```

Ergebnis des Source Generator
```csharp
public class CustomDataTableTableAsClass : System.Data.DataRow
{
    public CustomDataTableTableAsClass(System.Data.DataRowBuilder builder) : base(builder)
    {
        this.Id = Guid.NewGuid();
    }

    public System.Guid Id	{ get; set; }
    public string TextTyp	{ get; set; }
    public System.DateTime DatumTyp	{ get; set; }
    public double DoubleTyp	{ get; set; }
    public decimal DecimalTyp	{ get; set; }
    public int IntTyp	{ get; set; }
    public System.Nullable<int> NullIntTyp	{ get; set; }

    public override string ToString()
    {
        return "Hallo aus der generierten Klasse";
    }
}
```

Im Prinzip kann mit dem CodeDom Compiler Code jede Form von Klassen generiert werden. Es muss nur die entsprechende Logik im Source Generator erstellt werden.

# Versionshistorie

![Version](https://img.shields.io/badge/Version-1.0.2025.1-yellow.svg)
- Migration auf NET 10
- Extensions für die Verwendung des CodeDom Generators hinzugefügt
