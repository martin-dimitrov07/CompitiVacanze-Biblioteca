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

                if(!reader.IsDBNull(1))
                {
                    utenti.Last().DataNascita = reader.GetDateTime(1);
                }
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

        public List<Libro>? GetLibriNonPrenotati(int idCliente)
        {
            string query = $"SELECT l.* FROM Libri l WHERE NOT EXISTS( SELECT 1 FROM Prenotazioni p WHERE p.IdLibro = l.IdLibro AND p.IdUtente = {idCliente})" +
                $"OR EXISTS( SELECT 1 FROM Prestiti pres INNER JOIN Prenotazioni pren ON pren.IdPrenotazione = pres.IdPrenotazione WHERE pren.IdLibro = l.IdLibro AND pren.IdUtente = {idCliente} AND pres.DataFine < GETDATE())";

            var reader = _db.ExecuteReader(query);
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

        public List<Prenotazione>? GetPrenotazioni(string strWhere, SqlParameter[]? parameters = null)
        {
            string query = "";

            if (strWhere == "")
                query = $"SELECT * FROM Prenotazioni";
            else
                query = $"SELECT * FROM Prenotazioni WHERE {strWhere}";

            var reader = _db.ExecuteReader(query, parameters);
            var prenotazioni = new List<Prenotazione>();

            while (reader.Read())
            {
                prenotazioni.Add(new Prenotazione
                {
                    IdPrenotazione = reader.GetInt32(0),
                    IdUtente = reader.GetInt32(1),
                    IdLibro = reader.GetInt32(2),
                });
            }

            if (prenotazioni.Count == 0)
            {
                return null;
            }
            return prenotazioni;
        }

        public List<Prestito>? GetPrestiti(string strWhere, SqlParameter[]? parameters = null)
        {
            string query = "";
            if (strWhere == "")
                query = $"SELECT * FROM Prestiti";
            else
                query = $"SELECT * FROM Prestiti WHERE {strWhere}";
            var reader = _db.ExecuteReader(query, parameters);
            var prestiti = new List<Prestito>();
            while (reader.Read())
            {
                prestiti.Add(new Prestito
                {
                    IdPrestito = reader.GetInt32(0),
                    IdPrenotazione = reader.GetInt32(1),
                    DataInizio = reader.GetDateTime(2),
                    DataFine = reader.GetDateTime(3),
                });
            }
            if (prestiti.Count == 0)
            {
                return null;
            }
            return prestiti;
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

        public int InsertElement(string nameTable, object element)
        {
            var fields = new List<string>();
            var paramNames = new List<string>();
            var sqlParams = new List<SqlParameter>();

            foreach (var prop in element.GetType().GetProperties())
            {
                // Salta le colonne [Key] (es. ID autoincrementale)
                var isIdentity = Attribute.IsDefined(prop, typeof(System.ComponentModel.DataAnnotations.KeyAttribute));
                if (isIdentity)
                    continue;

                var value = prop.GetValue(element);
                if (value != null)
                {
                    string field = prop.Name;
                    string paramName = $"@{field}";

                    fields.Add(field);
                    paramNames.Add(paramName);
                    sqlParams.Add(new SqlParameter(paramName, value));
                }
            }

            string query = "";

            if(nameTable == "Prenotazioni")
            {
                query = $"INSERT INTO {nameTable} ({string.Join(", ", fields)}) OUTPUT INSERTED.IdPrenotazione VALUES ({string.Join(", ", paramNames)})";
                return Convert.ToInt32(_db.ExecuteScalar(query, sqlParams.ToArray()));
            }
            else
            {
                query = $"INSERT INTO {nameTable} ({string.Join(", ", fields)}) VALUES ({string.Join(", ", paramNames)})";
                return _db.ExecuteNonQuery(query, sqlParams.ToArray());
            }
        }


        public int UpdateElement(string nameTable, object element, string strWhere, SqlParameter[]? parameters = null)
        {
            var setClauses = new List<string>();
            var sqlParams = new List<SqlParameter>();

            foreach (var prop in element.GetType().GetProperties())
            {
                var isIdentity = Attribute.IsDefined(prop, typeof(System.ComponentModel.DataAnnotations.KeyAttribute));
                if (isIdentity)
                    continue;

                var value = prop.GetValue(element);
                if (value != null)
                {
                    string paramName = $"@{prop.Name}";
                    setClauses.Add($"{prop.Name} = {paramName}");
                    sqlParams.Add(new SqlParameter(paramName, value));
                }
            }

            if (parameters != null)
            {
                sqlParams.AddRange(parameters);
            }

            string query = $"UPDATE {nameTable} SET {string.Join(", ", setClauses)} WHERE {strWhere}";

            return _db.ExecuteNonQuery(query, sqlParams.ToArray());
        }


        public int DeleteElement(string nameTable, string strWhere, SqlParameter[]? parameters = null)
        {
            string query = $"DELETE FROM {nameTable} WHERE {strWhere}";
            return _db.ExecuteNonQuery(query, parameters);
        }
    }
}
