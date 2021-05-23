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
    public class ComandaDB
    {
        private long codi;
        private DateTime data;
        private int taula;
        private CambrerDB cambrer;
        private List<LiniaComandaDB> linies;

        public ComandaDB()
        {
        }

        public ComandaDB(long codi, DateTime data, int taula, CambrerDB cambrer, List<LiniaComandaDB> linies)
        {
            Codi = codi;
            Data = data;
            Taula = taula;
            Cambrer = cambrer;
            //Linies = linies;
            this.linies = linies;
        }

        public long Codi { get => codi; set => codi = value; }
        public DateTime Data { get => data; set => data = value; }
        public int Taula { get => taula; set => taula = value; }
        public CambrerDB Cambrer { get => cambrer; set => cambrer = value; }
        //public List<LiniaComandaDB> Linies { get => linies; set => linies = value; }

        /*
                public static ObservableCollection<ComandaDB> GetComandes()
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
                                    consulta.CommandText = "select * from plat";

                                    // B) llançar la consulta
                                    DbDataReader reader = consulta.ExecuteReader();

                                    // C) recórrer els resultats de la consulta
                                    ObservableCollection<ComandaDB> plats = new ObservableCollection<ComandaDB>();
                                    while (reader.Read())
                                    {
                                        long codi = reader.GetInt64(reader.GetOrdinal("codi"));
                                        string nom = reader.GetString(reader.GetOrdinal("nom"));

                                        string descripcioMD = "";
                                        if (!reader.IsDBNull(reader.GetOrdinal("descripcio_MD")))
                                            descripcioMD = reader.GetString(reader.GetOrdinal("descripcio_md"));

                                        decimal preu = reader.GetDecimal(reader.GetOrdinal("preu"));
                                        bool disponible = reader.GetBoolean(reader.GetOrdinal("disponible"));

                                        // TODO: agafar foto de la bd
                                        //BitmapImage foto;

                                        //if (!reader.IsDBNull(reader.GetOrdinal("image_path")))
                                        //    image_path = reader.GetString(reader.GetOrdinal("image_path"));

                                        ComandaDB plat = new ComandaDB(codi, nom, descripcioMD, preu, null, disponible);
                                        plats.Add(plat);
                                    }
                                    return plats;
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
        */

        public decimal getBaseImposable()
        {
            return 0;
        }

        public decimal getIVA()
        {
            return 0;
        }







    }
}
