using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace CookomaticDB
{
    public class DBUtils
    {
        public static void crearParametre(DbCommand consulta, String nom, DbType type, object valor)
        {
            DbParameter p = consulta.CreateParameter();
            p.ParameterName = nom;
            p.DbType = type;
            p.Value = valor;
            consulta.Parameters.Add(p);
        }
    }
}
