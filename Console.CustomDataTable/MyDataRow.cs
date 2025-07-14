//-----------------------------------------------------------------------
// <copyright file="MyDataRow.cs" company="Lifeprojects.de">
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

    public class MyDataRow : DataRow
    {
        public Guid Id
        {
            get { return (Guid)base["Id"]; }
            set { base["Id"] = value; }
        }

        public string TextTyp
        {
            get { return (string)base["TextTyp"]; }
            set { base["TextTyp"] = value; }
        }

        public DateTime DatumTyp
        {
            get { return (DateTime)base["DatumTyp"]; }
            set { base["DatumTyp"] = value; }
        }

        public double DoubleTyp
        {
            get { return (double)base["DoubleTyp"]; }
            set { base["DoubleTyp"] = value; }
        }

        public decimal DecimalTyp
        {
            get { return (decimal)base["DecimalTyp"]; }
            set { base["DecimalTyp"] = value; }
        }

        public int IntTyp
        {
            get { return (int)base["IntTyp"]; }
            set { base["IntTyp"] = value; }
        }

        public int NullIntTyp
        {
            get { return (int)base["NullIntTyp"]; }
            set { base["NullIntTyp"] = value; }
        }

        internal MyDataRow(DataRowBuilder builder) : base(builder)
        {
            this.Id = Guid.NewGuid();
            this.TextTyp = string.Empty;
            this.DatumTyp = new DateTime(1900, 1, 1, 0, 0, 0, 0);
            this.DoubleTyp = 0.0;
            this.DecimalTyp = 0.0M;
            this.IntTyp = 0;
            this.NullIntTyp = int.MinValue;
        }
    }
}
