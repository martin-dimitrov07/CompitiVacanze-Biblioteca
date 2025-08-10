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

        public object? GetElements(string nameTable, string strWhere, SqlParameter[]? parameters = null)
        {
            string query = $"SELECT * FROM {nameTable} WHERE {strWhere}";

            var reader = _db.ExecuteReader(query, parameters);

            var elements = new List<object>();

            while (reader.Read())
            {
                elements.Add(reader[0]);
            }

            if(elements.Count == 0)
            {
                return null;
            }

            return elements;
        }

        public int InsertElement(string nameTable, object element, SqlParameter[]? parameters = null)
        {
            string fields = "";
            string values = "";

            foreach (var prop in element.GetType().GetProperties())
            {
                fields += $"{prop.Name}, ";
                if (prop.GetValue(element) is string)
                    values += $"'{prop.GetValue(element)}', ";
                else
                    values += $"{prop.GetValue(element)}, ";
            }

            string query = $"INSERT INTO {nameTable} ({fields.TrimEnd(',', ' ')}) VALUES ({values.TrimEnd(',', ' ')})";

            return _db.ExecuteNonQuery(query, parameters);
        }

        public int UpdateElement(string nameTable, object element, string strWhere, SqlParameter[]? parameters = null)
        {
            string setClause = "";
            foreach (var prop in element.GetType().GetProperties())
            {
                if (prop.GetValue(element) is string)
                    setClause += $"{prop.Name} = '{prop.GetValue(element)}', ";
                else
                    setClause += $"{prop.Name} = {prop.GetValue(element)}, ";
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
