using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WebMatrix.Data;

public static class Db_Stuff
{

    static private Database get_db_con()
    {
        string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database.mdf;Integrated Security=True";
        string provider = "System.Data.SqlClient";
        return Database.OpenConnectionString(connectionString, provider);
    }

    public abstract class Taarten_Data
    {
        public ReadOnlyCollection<Onderdeel> type_cake;
        public ReadOnlyCollection<Onderdeel> bekleding;
        public ReadOnlyCollection<Onderdeel> vulling;
        public ReadOnlyCollection<Onderdeel> nivea_van_decoratie;
    }

    public class Onderdeel
    {
        public string naam;
        public int prijs_in_cent;
        public string beschrijving;
        public Onderdeel(string naam, int prijs_in_cent, string beschrijving = "")
        {
            this.naam = naam;
            this.prijs_in_cent = prijs_in_cent;
            this.beschrijving = beschrijving;
        }

        public override string ToString()
        {
            return this.naam + " " + this.prijs_in_cent.ToString();
        }
    }

    public class Test_Taarten_Data : Taarten_Data
    {
        public Test_Taarten_Data()
        {
            type_cake = new ReadOnlyCollection<Onderdeel>(new List<Onderdeel>
                {new Onderdeel("gewone taart", 100),
                    new Onderdeel("Chocolade taart", 300)
                }
            );

            bekleding = new ReadOnlyCollection<Onderdeel>(
                new List<Onderdeel>
                {
                    new Onderdeel("geen", 0),
                    new Onderdeel("marsepein", 300)
                });

            vulling = new ReadOnlyCollection<Onderdeel>(
                new List<Onderdeel>
                {
                    new Onderdeel("jam", 100),
                    new Onderdeel("chocolade", 300)
                });


            nivea_van_decoratie = new ReadOnlyCollection<Onderdeel>(
                new List<Onderdeel>
                {
                    new Onderdeel("geen decoratie", 0),
                    new Onderdeel("veel decoratie", 500)
                });
        }
    }

    public class Mssql_Taarten_Data : Taarten_Data
    {
        Database Db = get_db_con();

        public Mssql_Taarten_Data()
        {
            type_cake = get_onderdelen_van_type("CAKE");
            bekleding = get_onderdelen_van_type("BEKLEDING");
            vulling = get_onderdelen_van_type("VULLING");
            nivea_van_decoratie = get_onderdelen_van_type("NIV_DECO");
            Db.Close();
        }

        private ReadOnlyCollection<Onderdeel> get_onderdelen_van_type(string type)
        {
            string query = "SELECT Naam, Prijs, Beschrijving FROM Taart_Onderdelen WHERE UPPER(Type) = UPPER(@0)";
            var data = Db.Query(query, type).ToArray();
            var ret = new List<Onderdeel>();
            foreach (var i in data)
            {
                string beschrijving = i.Beschrijving;
                if (beschrijving == null)
                {
                    beschrijving = "";
                }
                ret.Add(new Onderdeel(i.Naam, i.Prijs, beschrijving));
            }
            return ret.AsReadOnly();
        }
    }

    public abstract class Admin_Db_methods
    {
        public Taarten_Data data;

        protected Database db = get_db_con();

        public abstract bool insert_onderdeel(string type, Onderdeel onderdeel);

        public abstract bool update_prijs_onderdeel(string type, string item_naam, int nieuwe_prijs);

        public abstract bool delete_onderdeel(string type, string item_naam);

        public abstract void update_data();

        public bool check_affected_row_count(int r_affected)
        {
            if (r_affected > 1)
            {
                Utils.print("Error meerdere rows veranderd");
                return false;
            }
            else if (r_affected == 0)
            {
                Utils.print("Error geen rows veranderd");
                return false;
            }
            return true;
        }
    }

    public class Mssql_Admin_Db_methods : Admin_Db_methods
    {
        public Mssql_Admin_Db_methods()
        { update_data(); }

        public override bool delete_onderdeel(string type, string item_naam)
        {
            bool ret = check_affected_row_count(db.Execute("DELETE FROM Taart_Onderdelen WHERE UPPER(Type) = UPPER(@0) and UPPER(Naam) = UPPER(@1)", type, item_naam));
            update_data();
            return ret;
        }

        public override bool insert_onderdeel(string type, Onderdeel onderdeel)
        {
            if (db.Query("SELECT * FROM Taart_Onderdelen where UPPER(Type) = UPPER(@0) AND UPPER(Naam) = UPPER(@1)", type, onderdeel.naam).ToArray().Length > 0)
            {
                Utils.print("Error onderdeel staat al in database");
                return false;
            }
            bool ret = check_affected_row_count(db.Execute("INSERT INTO Taart_Onderdelen (Type, Naam, Prijs, Beschrijving) VALUES (@0, @1, @2, @3)",
                type, onderdeel.naam, onderdeel.prijs_in_cent, onderdeel.beschrijving));
            update_data();
            return ret;
        }

        public override bool update_prijs_onderdeel(string type, string item_naam, int nieuwe_prijs)
        {
            bool ret = check_affected_row_count(db.Execute("UPDATE Taart_Onderdelen SET Prijs = @0 WHERE UPPER(Type) = UPPER(@1) AND UPPER(Naam) = UPPER(@2)", nieuwe_prijs, type, item_naam));
            update_data();
            return ret;
        }

        public override void update_data()
        {
            this.data = new Mssql_Taarten_Data();
        }
    }
}
