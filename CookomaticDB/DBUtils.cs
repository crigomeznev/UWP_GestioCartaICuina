using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

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

        public static void LlegeixFoto(DbDataReader reader, out byte[] valor, string nomColumna, byte[] valorPerDefecte = null)
        {
            valor = valorPerDefecte;
            int ordinal = reader.GetOrdinal(nomColumna);
            if (!reader.IsDBNull(ordinal))
            {
                Type t = reader.GetFieldType(reader.GetOrdinal(nomColumna));
                valor = (byte[])reader.GetValue(ordinal);
            }
        }




        // TODO: moure a imageconvertor
        //public static Image ConvertByteArrayToImage(byte[] byteArrayIn)
        //{
        //    using (MemoryStream ms = new MemoryStream(byteArrayIn))
        //    {
        //        return Image.FromStream(ms);
        //    }
        //}





    }
}
