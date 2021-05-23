using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Diagnostics;
using System.Text;
using Windows.UI.Xaml.Media.Imaging;

namespace CookomaticDB.Model
{
    public class CambrerDB
    {
        private long codi;
        private string nom;
        private string cognom1;
        private string cognom2;
        private string user;
        private string password;

        public CambrerDB(long codi, string nom, string cognom1, string cognom2, string user, string password)
        {
            Codi = codi;
            Nom = nom;
            Cognom1 = cognom1;
            Cognom2 = cognom2;
            User = user;
            Password = password;
        }

        public CambrerDB()
        {
        }

        public long Codi { get => codi; set => codi = value; }
        public string Nom { get => nom; set => nom = value; }
        public string Cognom1 { get => cognom1; set => cognom1 = value; }
        public string Cognom2 { get => cognom2; set => cognom2 = value; }
        public string User { get => user; set => user = value; }
        public string Password { get => password; set => password = value; }

        //public static ObservableCollection<CambrerDB> GetPlats()
        //{
        //    try
        //    {
        //        using (CookomaticDB context = new CookomaticDB())
        //        {
        //            using (var connexio = context.Database.GetDbConnection())
        //            {
        //                connexio.Open();

        //                using (DbCommand consulta = connexio.CreateCommand())
        //                {
        //                    // A) definir la consulta
        //                    consulta.CommandText = "select * from plat";

        //                    // B) llançar la consulta
        //                    DbDataReader reader = consulta.ExecuteReader();

        //                    // C) recórrer els resultats de la consulta
        //                    ObservableCollection<CambrerDB> plats = new ObservableCollection<CambrerDB>();
        //                    while (reader.Read())
        //                    {
        //                        long codi = reader.GetInt64(reader.GetOrdinal("codi"));
        //                        string nom = reader.GetString(reader.GetOrdinal("nom"));

        //                        string descripcioMD = "";
        //                        if (!reader.IsDBNull(reader.GetOrdinal("descripcio_MD")))
        //                            descripcioMD = reader.GetString(reader.GetOrdinal("descripcio_md"));

        //                        decimal preu = reader.GetDecimal(reader.GetOrdinal("preu"));
        //                        bool disponible = reader.GetBoolean(reader.GetOrdinal("disponible"));

        //                        // TODO: agafar foto de la bd
        //                        //BitmapImage foto;

        //                        //if (!reader.IsDBNull(reader.GetOrdinal("image_path")))
        //                        //    image_path = reader.GetString(reader.GetOrdinal("image_path"));

        //                        CambrerDB plat = new CambrerDB(codi, nom, descripcioMD, preu, null, disponible);
        //                        plats.Add(plat);
        //                    }
        //                    return plats;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // deixar missatge al log
        //        Debug.WriteLine(ex.Message);
        //        Debug.WriteLine(ex.StackTrace);
        //    }
        //    return null;
        //}
    }
}
