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
        private ObservableCollection<LiniaComandaDB> linies;

        public ComandaDB()
        {
        }

        public ComandaDB(long codi, DateTime data, int taula, CambrerDB cambrer, ObservableCollection<LiniaComandaDB> linies)
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

        public void AddLinia(LiniaComandaDB linia)
        {
            linies.Add(linia);
        }

        // retorna true si s'ha esborrat la linia, fals si no s'ha trobat
        public bool RemoveLinia(LiniaComandaDB linia)
        {
            return linies.Remove(linia);
        }

        public IEnumerator<LiniaComandaDB> IteLinies()
        {
            foreach (LiniaComandaDB linia in linies)
            {
                yield return linia;
            }
            //yield return default(LiniaComandaDB);
        }
        public ObservableCollection<LiniaComandaDB> Linies { get => linies; set => linies = value; }

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
                            consulta.CommandText = "select * from comanda";

                            // B) llançar la consulta
                            DbDataReader reader = consulta.ExecuteReader();

                            // C) recórrer els resultats de la consulta
                            ObservableCollection<ComandaDB> comandes = new ObservableCollection<ComandaDB>();
                            while (reader.Read())
                            {
                                long codi = reader.GetInt64(reader.GetOrdinal("codi"));
                                DateTime data = reader.GetDateTime(reader.GetOrdinal("data"));

                                // POC EFICIENT: Obrim una nova connexió a la bd per anar a buscar el cambrer!
                                long codiCambrer = reader.GetInt64(reader.GetOrdinal("cambrer"));
                                CambrerDB cambrer = CambrerDB.GetCambrerPerCodi(codiCambrer);

                                // TODO: taula
                                // POC EFICIENT: Obrim una nova connexió a la bd per anar a buscar el cambrer!
                                ObservableCollection<LiniaComandaDB> linies = LiniaComandaDB.GetLiniesPerCodiComanda(codi);


                                ComandaDB comanda = new ComandaDB(codi, data, 0, cambrer, linies);
                                comandes.Add(comanda);
                            }
                            return comandes;
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
