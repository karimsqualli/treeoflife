

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data;


namespace TreeOfLife.Database
{
    public class SQL : IDisposable
    {
        SQLiteConnection _Connection = null;
        List<string> _Tables = null;
        string _TaxonTable = null;
        bool _Verbose = true;

        public SQL()
        {
            _Connection = new SQLiteConnection("Data Source=TreeOfLife.db;Version=3;");
            _Connection.Open();
            _Tables = GetTables();

            // temp code to remove existent table
            if (_Tables.Contains("Taxons"))
            {
                string removeTableQuery = "DROP TABLE Taxons";
                ExecuteCommand(removeTableQuery);
                _Tables = GetTables();
            }

            if (!_Tables.Contains("Taxons")) 
            {
                string createTableQuery = "CREATE TABLE Taxons(Name CHAR UNIQUE, PRIMARY KEY(Name))";
                ExecuteCommand(createTableQuery);
                _Tables = GetTables();
            }

            if (_Tables.Contains("Taxons"))
            {
                _TaxonTable = "taxons";

                if (_Verbose)
                {
                    Console.WriteLine( "Database taxons.db opening successfull" );
                
                    SQLiteDataReader r = ExecuteCommand("SELECT * FROM " + _TaxonTable);
                    if (r != null)
                    {
                        for (var i = 0; i < r.FieldCount; i++)
                            Console.WriteLine(r.GetName(i));
                    }
                }
            }
            else
            {
                _Connection.Close();
                _Connection.Dispose();
                _Connection = null;
            }
        }

        public void Dispose()
        {
            if (_Connection != null)
            {
                _Connection.Close();
                _Connection.Dispose();
                _Connection = null;
            }
        }

        public bool Valid { get { return _Connection != null && _TaxonTable != null; } }

        public SQLiteDataReader ExecuteCommand(string _query)
        {
            try
            {
                using (SQLiteCommand cmd = new SQLiteCommand(_query, _Connection))
                {
                    return cmd.ExecuteReader();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public DataTable GetDataTable(string _query)
        {
            SQLiteDataReader rdr = ExecuteCommand( _query );
            if (rdr == null) return null;

            DataTable dt = new DataTable();
            try
            {
                dt.Load(rdr);
                return dt;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public List<string> GetTables()
        {
            String query = "SELECT name FROM sqlite_master WHERE type = 'table' ORDER BY 1";
            DataTable dt = GetDataTable(query);
            
            List<string> list = new List<string>();
            foreach (DataRow row in dt.Rows)
                list.Add(row.ItemArray[0].ToString());
            return list;
        }

        public void addTaxon( string _Latin )
        {
            if (!Valid) return;

            if (taxonExist(_Latin)) return;

            string query = "INSERT INTO " + _TaxonTable + " (NomLatin) VALUES ('" + _Latin + "');";
            SQLiteCommand cmd = new SQLiteCommand(query, _Connection);
            int result = cmd.ExecuteNonQuery();
            if (_Verbose)
                Console.WriteLine("addTaxon result = " + result.ToString());
        }

        public void addTaxonRecursive(TaxonTreeNode _current)
        {
            addTaxon(_current.Desc.RefAllNames);
            foreach (TaxonTreeNode child in _current.Children)
                addTaxonRecursive(child);
        }

        public bool taxonExist(string _Latin)
        {
            if (!Valid) return false;
            string query = "SELECT NomLatin from " + _TaxonTable + " where NomLatin='" + _Latin + "'";
            DataTable dt = GetDataTable(query);
            int result = dt.Rows.Count;
            if (_Verbose)
                Console.WriteLine("taxonExist result = " + result.ToString());
            return result == 1;
        }
    
    }

}
