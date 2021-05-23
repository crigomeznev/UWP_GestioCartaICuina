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
    public class LiniaComandaDB
    {
        private int num;
        private int quantitat;
        private EstatLinia estat;
        private PlatDB item;

        public LiniaComandaDB()
        {
        }

        public LiniaComandaDB(int num, int quantitat, EstatLinia estat, PlatDB item)
        {
            Num = num;
            Quantitat = quantitat;
            Estat = estat;
            Item = item;
        }

        public int Num { get => num; set => num = value; }
        public int Quantitat { get => quantitat; set => quantitat = value; }
        public EstatLinia Estat { get => estat; set => estat = value; }
        public PlatDB Item { get => item; set => item = value; }

        public static ObservableCollection<LiniaComandaDB> GetLinies()
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
                            consulta.CommandText = "select * from linia_comanda";

                            // B) llançar la consulta
                            DbDataReader reader = consulta.ExecuteReader();

                            // C) recórrer els resultats de la consulta
                            ObservableCollection<LiniaComandaDB> linies = new ObservableCollection<LiniaComandaDB>();
                            while (reader.Read())
                            {
                                int numAux = reader.GetInt32(reader.GetOrdinal("num"));
                                int qtatAux = reader.GetInt32(reader.GetOrdinal("qtat"));
                                string estatAux = reader.GetString(reader.GetOrdinal("estat"));

                                /*
                                var list = new List<YourEnumType>();

                                var field = reader["DBFieldName"] != DBNull.Value ? reader["DBFieldName"].ToString()
                                                                                   : "";
                                var myField = (YourEnumType)Enum.Parse(typeof(YourEnumType), field);
                                */
                                var estat = (EstatLinia)Enum.Parse(typeof(EstatLinia), estatAux);
                                // TODO: agafar foto de la bd
                                //BitmapImage foto;

                                //if (!reader.IsDBNull(reader.GetOrdinal("image_path")))
                                //    image_path = reader.GetString(reader.GetOrdinal("image_path"));

                                LiniaComandaDB linia = new LiniaComandaDB(numAux, qtatAux, estat, null);
                                linies.Add(linia);
                            }
                            return linies;
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

        public static ObservableCollection<LiniaComandaDB> GetLiniesPerPlat(PlatDB plat)
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
                            consulta.CommandText = "select * from linia_comanda where plat = @plat";
                            DBUtils.crearParametre(consulta, "plat", System.Data.DbType.Int64, plat.Codi);

                            // B) llançar la consulta
                            DbDataReader reader = consulta.ExecuteReader();

                            // C) recórrer els resultats de la consulta
                            ObservableCollection<LiniaComandaDB> linies = new ObservableCollection<LiniaComandaDB>();
                            while (reader.Read())
                            {
                                int numAux = reader.GetInt32(reader.GetOrdinal("num"));
                                int qtatAux = reader.GetInt32(reader.GetOrdinal("quantitat"));
                                string estatAux = reader.GetString(reader.GetOrdinal("estat"));

                                /*
                                var list = new List<YourEnumType>();

                                var field = reader["DBFieldName"] != DBNull.Value ? reader["DBFieldName"].ToString()
                                                                                   : "";
                                var myField = (YourEnumType)Enum.Parse(typeof(YourEnumType), field);
                                */
                                var estat = (EstatLinia)Enum.Parse(typeof(EstatLinia), estatAux);
                                // TODO: agafar foto de la bd
                                //BitmapImage foto;

                                //if (!reader.IsDBNull(reader.GetOrdinal("image_path")))
                                //    image_path = reader.GetString(reader.GetOrdinal("image_path"));

                                LiniaComandaDB linia = new LiniaComandaDB(numAux, qtatAux, estat, null);
                                linies.Add(linia);
                            }
                            return linies;
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

        public static int GetNumLiniesPerPlat(PlatDB plat)
        {
            Int32 numLinies = 0;
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
                            consulta.CommandText = "select count(*) as num_linies from linia_comanda where plat = @plat";
                            DBUtils.crearParametre(consulta, "plat", System.Data.DbType.Int64, plat.Codi);

                            // B) llançar la consulta
                            var r = consulta.ExecuteScalar();
                            numLinies = (Int32)((long)r);
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
            return numLinies;
        }

        public int getImport()
        {
            return 0;
        }

        public override string ToString()
        {
            return num + " " + quantitat;
        }

    }
}
