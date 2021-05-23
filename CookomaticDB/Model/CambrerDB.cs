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

        public static ObservableCollection<CambrerDB> GetCambrers()
        {
            try
            {
                using (CookomaticDB context = new CookomaticDB())
                {
                    using (var connexio = context.Database.GetDbConnection())
                    {
                        connexio.Open();

                        using (DbCommand consulta = connexio.CreateCommand())
                        {
                            // A) definir la consulta
                            consulta.CommandText = "select * from cambrer";

                            // B) llançar la consulta
                            DbDataReader reader = consulta.ExecuteReader();

                            // C) recórrer els resultats de la consulta
                            ObservableCollection<CambrerDB> cambrers = new ObservableCollection<CambrerDB>();
                            while (reader.Read())
                            {
                                long codi = reader.GetInt64(reader.GetOrdinal("codi"));
                                string nom = reader.GetString(reader.GetOrdinal("nom"));
                                string cognom1 = reader.GetString(reader.GetOrdinal("cognom1"));
                                string cognom2 = "";
                                if (!reader.IsDBNull(reader.GetOrdinal("cognom2")))
                                    cognom2 = reader.GetString(reader.GetOrdinal("cognom2"));
                                string user = reader.GetString(reader.GetOrdinal("user"));
                                string password = reader.GetString(reader.GetOrdinal("password"));

                                CambrerDB cambrer = new CambrerDB(codi, nom, cognom1, cognom2, user, password);
                                cambrers.Add(cambrer);
                            }
                            return cambrers;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // deixar missatge al log
                Debug.WriteLine(ex.Message);
                Debug.WriteLine(ex.StackTrace);
            }
            return null;
        }


        public static CambrerDB GetCambrerPerCodi(long pCodi)
        {
            CambrerDB cambrer = null;
            try
            {
                using (CookomaticDB context = new CookomaticDB())
                {
                    using (var connexio = context.Database.GetDbConnection())
                    {
                        connexio.Open();

                        using (DbCommand consulta = connexio.CreateCommand())
                        {
                            // A) definir la consulta
                            consulta.CommandText = "select * from cambrer where codi = @codi";
                            DBUtils.crearParametre(consulta, "codi", System.Data.DbType.Int64, pCodi);

                            // B) llançar la consulta
                            DbDataReader reader = consulta.ExecuteReader();

                            if (reader.Read())
                            {
                                long codi = reader.GetInt64(reader.GetOrdinal("codi"));
                                string nom = reader.GetString(reader.GetOrdinal("nom"));
                                string cognom1 = reader.GetString(reader.GetOrdinal("cognom1"));
                                string cognom2 = "";
                                if (!reader.IsDBNull(reader.GetOrdinal("cognom2")))
                                    cognom2 = reader.GetString(reader.GetOrdinal("cognom2"));
                                string user = reader.GetString(reader.GetOrdinal("user"));
                                string password = reader.GetString(reader.GetOrdinal("password"));

                                cambrer = new CambrerDB(codi, nom, cognom1, cognom2, user, password);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                // deixar missatge al log
            }
            return cambrer;
        }

    }
}
