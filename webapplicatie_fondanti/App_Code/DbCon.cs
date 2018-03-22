using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;

public abstract class Taart_Data
{
    public ReadOnlyCollection<Onderdeel> type_cake;
    public ReadOnlyCollection<Onderdeel> bekleding;
    public ReadOnlyCollection<Onderdeel> vulling;
    public ReadOnlyCollection<Onderdeel> nivea_van_decoratie;
}



public abstract class Admin_Db_methods : Taart_Data
{
    //todo add db var
    //todo hoe indentificeer je items

    public abstract bool insert_onderdeel(string table_name, Onderdeel onderdeel);

    public abstract bool update_prijs_onderdeel(string table_naam, string item_naam, int nieuwe_prijs);

    public abstract bool delete_onderdeel(string table_naam, string item_naam);

}

public class Onderdeel
{
    public string naam;
    public int prijs_in_cent;
    public Onderdeel(string naam, int prijs_in_cent = 0)
    {
        this.naam = naam;
        this.prijs_in_cent = prijs_in_cent;
    }

    public override string ToString()
    {
        return this.naam + " " + this.prijs_in_cent.ToString();
    }
}

public static class Db_Utils
{
    public static void create_db(string naam)
    {
        //todo Make it so you can config the path.
        SQLiteConnection.CreateFile(naam + ".sqlite");
    }
}

public class Taarten_Data_Sqlite : Taart_Data
{
    public Taarten_Data_Sqlite()
    {
        SQLiteConnection db = new SQLiteConnection("Data Source=prijzen.db;Version=3;");
        string sql = "select * from taart_onderdelen";
        SQLiteCommand command = new SQLiteCommand(sql, db);
        SQLiteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Console.WriteLine("Type: " + reader["Type"] + "\tNaam: " + reader["Naam"]);
        }

    }


}



public class Test_Taarten_Data : Taart_Data
{
    public Test_Taarten_Data()
    {
        type_cake = new ReadOnlyCollection<Onderdeel>(new List<Onderdeel>
                {new Onderdeel("gewone taart", 100),
                    new Onderdeel("Chocolade taart", 300)
                });

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
