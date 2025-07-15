//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Lifeprojects.de">
//     Class: Program
//     Copyright © Lifeprojects.de 2025
// </copyright>
// <Template>
// 	Version 2.0.2025.0, 28.4.2025
// </Template>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>14.07.2025 19:01:09</date>
//
// <summary>
// Konsolen Applikation zur Darstellung eines Cusom DataTable und Custom DataRow
// </summary>
//-----------------------------------------------------------------------

namespace Console.CustomDataTable
{
    /* Imports from NET Framework */
    using System;
    using System.Data;

    public class Program
    {
        private static void Main(string[] args)
        {
            do
            {
                Console.Clear();
                Console.WriteLine("1. Custom DataTable");
                Console.WriteLine("X. Beenden");
                Console.WriteLine("Wählen Sie einen Menüpunkt oder 'x' für beenden");
                ConsoleKey key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.X)
                {
                    Environment.Exit(0);
                }
                else
                {
                    if (key == ConsoleKey.D1)
                    {
                        MenuPoint1();
                    }
                }
            }
            while (true);
        }

        private static void MenuPoint1()
        {
            Console.Clear();

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

            DataView dv = table.AsDataView(DataViewRowState.CurrentRows);
            int dvCount = dv.Count;

            MyDataRow row1 = (MyDataRow)table.Rows[1];
            row1.SetField<string>("TextTyp", "Hallo");
            MyDataRow cloneRow = table.Clone(1);
            int count = table.Count;
            table.WriteXml(Path.Combine(AppContext.BaseDirectory, "TestCustomTable.xlm"));
            table.WriteToJson(Path.Combine(AppContext.BaseDirectory, "TestCustomTable.json"));

            foreach (MyDataRow myRow in table.Rows)
            {
                DataRowState rowstate = myRow.RowState;
                Guid colValue = myRow.Field<Guid>("Id");
                Guid id = myRow.Id;
                Console.WriteLine($"{colValue}; {id}; Datum: {myRow.DatumTyp}");
            }

            Console.WriteLine("eine Taste drücken für zurück!");
            Console.ReadKey();
        }

        private static void OnMyDataRowChanged(MyDataTable sender, MyDataRowChangedEventArgs args)
        {
            /*
            if (args.Row.TextTyp != string.Empty)
            {
                throw new ApplicationException("Die Spalte 'TextTyp' muß mit string.Empty initalisiert werden.");
            }
            */
        }
    }
}
