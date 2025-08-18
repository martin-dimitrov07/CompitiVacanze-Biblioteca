using Biblioteca.Core.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblioteca.Data
{
    public class Repository
    {
        private readonly Database _db;

        public Repository(string connStr)
        {
            _db = new Database(connStr);
        }

        public List<Utente>? GetUtenti(string strWhere, SqlParameter[]? parameters = null)
        {
            string query = "";

            if (strWhere == "")
                query = $"SELECT * FROM Utenti";
            else
                query = $"SELECT * FROM Utenti WHERE {strWhere}";

            var reader = _db.ExecuteReader(query, parameters);
            var utenti = new List<Utente>();

            while (reader.Read())
            {
                utenti.Add(new Utente
                {
                    IdUtente = reader.GetInt32(0),
                    Nome = reader.GetString(2),
                    Cognome = reader.GetString(3),
                    Email = reader.GetString(4),
                    PasswordHash = reader.GetString(5)
                });
            }

            if (utenti.Count == 0)
            {
                return null;
            }
            return utenti;
        }

        public List<Libro>? GetLibri(string strWhere, SqlParameter[]? parameters = null)
        {
            string query = "";

            if (strWhere == "")
                query = "SELECT * FROM Libri";
            else
                query = $"SELECT * FROM Libri WHERE {strWhere}";

            var reader = _db.ExecuteReader(query, parameters);
            var libri = new List<Libro>();

            while (reader.Read())
            {
                var autore = GetAutori($"IdAutore = {reader.GetInt32(2)}", new SqlParameter[] { new SqlParameter("@IdAutore", reader.GetInt32(2)) }).FirstOrDefault();
                var nazione = GetNazioni($"IdPaese = {reader.GetInt32(4)}", new SqlParameter[] { new SqlParameter("@IdPaese", reader.GetInt32(4)) }).FirstOrDefault();
                var lingua = GetLingue($"IdLingua = {reader.GetInt32(5)}", new SqlParameter[] { new SqlParameter("@IdLingua", reader.GetInt32(5)) }).FirstOrDefault();

                libri.Add(new Libro
                {
                    IdLibro = reader.GetInt32(0),
                    Titolo = reader.GetString(1),
                    IdAutore = reader.GetInt32(2),
                    Autore = autore,
                    Anno = reader.GetInt32(3),
                    IdPaese = reader.GetInt32(4),
                    Paese = nazione,
                    IdLingua = reader.GetInt32(5),
                    Lingua = lingua,
                    Prezzo = reader.GetDecimal(6),
                    Pagine = reader.GetInt32(7),
                });
            }

            if (libri.Count == 0)
            {
                return null;
            }

            return libri;
        }

        public List<Autore>? GetAutori(string strWhere, SqlParameter[]? parameters = null)
        {
            string query = "";

            if (strWhere == "")
                query = $"SELECT * FROM Autori";
            else
                query = $"SELECT * FROM Autori WHERE {strWhere}";

            var reader = _db.ExecuteReader(query, parameters);
            var autori = new List<Autore>();

            while (reader.Read())
            {
                autori.Add(new Autore
                {
                    IdAutore = reader.GetInt32(0),
                    Nome = reader.GetString(1),
                    Cognome = reader.GetString(2),
                });
            }

            if (autori.Count == 0)
            {
                return null;
            }

            return autori;
        }

        public List<Nazione>? GetNazioni(string strWhere, SqlParameter[]? parameters = null)
        {
            string query = "";

            if (strWhere == "")
                query = $"SELECT * FROM Nazioni";
            else
                query = $"SELECT * FROM Nazioni WHERE {strWhere}";

            var reader = _db.ExecuteReader(query, parameters);
            var nazioni = new List<Nazione>();

            while (reader.Read())
            {
                nazioni.Add(new Nazione
                {
                    IdPaese = reader.GetInt32(0),
                    Nome = reader.GetString(1),
                });
            }

            if (nazioni.Count == 0)
            {
                return null;
            }

            return nazioni;
        }

        public List<Lingua>? GetLingue(string strWhere, SqlParameter[]? parameters = null)
        {
            string query = "";

            if (strWhere == "")
                query = $"SELECT * FROM Lingue";
            else
                query = $"SELECT * FROM Lingue WHERE {strWhere}";

            var reader = _db.ExecuteReader(query, parameters);
            var lingue = new List<Lingua>();
            while (reader.Read())
            {
                lingue.Add(new Lingua
                {
                    IdLingua = reader.GetInt32(0),
                    Nome = reader.GetString(1),
                });
            }
            if (lingue.Count == 0)
            {
                return null;
            }
            return lingue;
        }


        // NON FUNZIONA
        //public List<T> GetElements<T>(string nameTable, string strWhere, SqlParameter[]? parameters = null) // T è un tipo generico che deve essere specificato al momento della chiamata del metodo
        //    where T : new()   // <-- indica che T ha un costruttore senza parametri
        //{
        //    string query = $"SELECT * FROM {nameTable} WHERE {strWhere}";

        //    var reader = _db.ExecuteReader(query, parameters);

        //    var elements = new List<T>();

        //    while (reader.Read())
        //    {
        //        T item = new T();

        //        foreach(var prop in typeof(T).GetProperties())
        //        {
        //            if (reader.GetSchemaTable().Columns.Contains(prop.Name) && reader[prop.Name] != DBNull.Value)
        //            {
        //                prop.SetValue(item, reader[prop.Name]);
        //            }
        //        }

        //        elements.Add(item);
        //    }

        //    if(elements.Count == 0)
        //    {
        //        return null;
        //    }

        //    return elements;
        //}

        public int InsertElement(string nameTable, object element, SqlParameter[]? parameters = null)
        {
            string fields = "";
            string values = "";

            foreach (var prop in element.GetType().GetProperties())
            {
                // Salta le colonne autoincremento/identity
                var isIdentity = Attribute.IsDefined(prop, typeof(System.ComponentModel.DataAnnotations.KeyAttribute));
                if (isIdentity)
                    continue;

                if (prop.GetValue(element) != null)
                {
                    fields += $"{prop.Name}, ";
                    if (prop.GetValue(element) is string)
                        values += $"'{prop.GetValue(element)}', ";
                    else
                        values += $"{prop.GetValue(element)}, ";
                }
            }

            string query = $"INSERT INTO {nameTable} ({fields.TrimEnd(',', ' ')}) VALUES ({values.TrimEnd(',', ' ')})";

            return _db.ExecuteNonQuery(query, parameters);
        }

        public int UpdateElement(string nameTable, object element, string strWhere, SqlParameter[]? parameters = null)
        {
            string setClause = "";
            foreach (var prop in element.GetType().GetProperties())
            {
                // Salta le colonne autoincremento/identity
                var isIdentity = Attribute.IsDefined(prop, typeof(System.ComponentModel.DataAnnotations.KeyAttribute));
                if (isIdentity)
                    continue;

                if (prop.GetValue(element) != null)
                {
                    if (prop.GetValue(element) is string)
                        setClause += $"{prop.Name} = '{prop.GetValue(element)}', ";
                    else
                    {
                        if (prop.GetValue(element) is decimal)
                            setClause += $"{prop.Name} = {prop.GetValue(element).ToString().Replace(',', '.')}, ";
                        else
                            setClause += $"{prop.Name} = {prop.GetValue(element)}, ";
                    }
                }
            }
            string query = $"UPDATE {nameTable} SET {setClause.TrimEnd(',', ' ')} WHERE {strWhere}";
            return _db.ExecuteNonQuery(query, parameters);
        }

        public int DeleteElement(string nameTable, string strWhere, SqlParameter[]? parameters = null)
        {
            string query = $"DELETE FROM {nameTable} WHERE {strWhere}";
            return _db.ExecuteNonQuery(query, parameters);
        }
    }
}
