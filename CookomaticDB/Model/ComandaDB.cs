using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Text;
using Windows.UI.Xaml.Media.Imaging;

namespace CookomaticDB.Model
{
    public class ComandaDB : INotifyPropertyChanged
    {
        private long codi;
        private DateTime data;
        private int taula;
        private CambrerDB cambrer;
        private ObservableCollection<LiniaComandaDB> linies;

        //afegit
        private bool finalitzada;

        public event PropertyChangedEventHandler PropertyChanged;

        public ComandaDB()
        {
        }

        public ComandaDB(long codi, DateTime data, int taula, CambrerDB cambrer, ObservableCollection<LiniaComandaDB> linies, bool finalitzada)
        {
            Codi = codi;
            Data = data;
            Taula = taula;
            Cambrer = cambrer;
            this.linies = linies;
            this.finalitzada = finalitzada;
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
        }
        public ObservableCollection<LiniaComandaDB> Linies { get => linies; set => linies = value; }
        public bool Finalitzada { get => finalitzada; set => finalitzada = value; }




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
                            consulta.CommandText = "select * from comanda order by data desc";

                            // B) llançar la consulta
                            DbDataReader reader = consulta.ExecuteReader();

                            // C) recórrer els resultats de la consulta
                            ObservableCollection<ComandaDB> comandes = new ObservableCollection<ComandaDB>();
                            while (reader.Read())
                            {
                                long codi = reader.GetInt64(reader.GetOrdinal("codi"));
                                DateTime data = reader.GetDateTime(reader.GetOrdinal("data"));
                                int taula = reader.GetInt32(reader.GetOrdinal("taula"));

                                // POC EFICIENT: Obrim una nova connexió a la bd per anar a buscar el cambrer!
                                long codiCambrer = reader.GetInt64(reader.GetOrdinal("cambrer"));
                                CambrerDB cambrer = CambrerDB.GetCambrerPerCodi(codiCambrer);

                                bool finalitzada = reader.GetBoolean(reader.GetOrdinal("finalitzada"));

                                // POC EFICIENT: Obrim una nova connexió a la bd per anar a buscar el cambrer!
                                //ObservableCollection<LiniaComandaDB> linies = LiniaComandaDB.GetLiniesPerCodiComanda(codi);
                                ObservableCollection<LiniaComandaDB> linies = null;
                                ComandaDB comanda = new ComandaDB(codi, data, taula, cambrer, linies, finalitzada);
                                comanda.Linies = LiniaComandaDB.GetLiniesPerComanda(comanda);

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
        
        public static ObservableCollection<ComandaDB> GetComandesPerFinalitzada(bool pFinalitzada)
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
                            consulta.CommandText = 
                                $@"select * from comanda
                                    where finalitzada = @finalitzada
                                    order by data desc";
                            DBUtils.crearParametre(consulta, "finalitzada", System.Data.DbType.Boolean, pFinalitzada); // filtrar: nomes veure les finalitzades

                            // B) llançar la consulta
                            DbDataReader reader = consulta.ExecuteReader();

                            // C) recórrer els resultats de la consulta
                            ObservableCollection<ComandaDB> comandes = new ObservableCollection<ComandaDB>();
                            while (reader.Read())
                            {
                                long codi = reader.GetInt64(reader.GetOrdinal("codi"));
                                DateTime data = reader.GetDateTime(reader.GetOrdinal("data"));
                                int taula = reader.GetInt32(reader.GetOrdinal("taula"));

                                // POC EFICIENT: Obrim una nova connexió a la bd per anar a buscar el cambrer!
                                long codiCambrer = reader.GetInt64(reader.GetOrdinal("cambrer"));
                                CambrerDB cambrer = CambrerDB.GetCambrerPerCodi(codiCambrer);

                                bool finalitzada = reader.GetBoolean(reader.GetOrdinal("finalitzada"));

                                // TODO: taula
                                // POC EFICIENT: Obrim una nova connexió a la bd per anar a buscar el cambrer!
                                //ObservableCollection<LiniaComandaDB> linies = LiniaComandaDB.GetLiniesPerCodiComanda(codi);
                                ObservableCollection<LiniaComandaDB> linies = null;
                                ComandaDB comanda = new ComandaDB(codi, data, taula, cambrer, linies, finalitzada);
                                comanda.Linies = LiniaComandaDB.GetLiniesPerComanda(comanda);

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



        public bool Update()
        {
            DbTransaction trans = null;
            try
            {
                using (CookomaticDB context = new CookomaticDB())
                {
                    using (var connexio = context.Database.GetDbConnection())
                    {
                        connexio.Open();
                        using (DbCommand consulta = connexio.CreateCommand())
                        {
                            trans = connexio.BeginTransaction();
                            consulta.Transaction = trans;

                            // PK de COMANDA: CODI -> MAI actualitzarem aquest valor
                            // DATA, TAULA: no hauriem de deixar canviar aquest valor
                            consulta.CommandText = $@"
                                    UPDATE COMANDA
                                    SET DATA = @DATA, TAULA = @TAULA, CAMBRER = @CAMBRER, FINALITZADA = @FINALITZADA
                                    WHERE CODI = @CODI";
                            DBUtils.crearParametre(consulta, "DATA", System.Data.DbType.DateTime, this.Data);
                            DBUtils.crearParametre(consulta, "TAULA", System.Data.DbType.Int32, this.Taula);
                            DBUtils.crearParametre(consulta, "CAMBRER", System.Data.DbType.Int64, this.Cambrer.Codi);
                            DBUtils.crearParametre(consulta, "FINALITZADA", System.Data.DbType.Boolean, this.Finalitzada);
                            DBUtils.crearParametre(consulta, "CODI", System.Data.DbType.Int64, this.Codi);

                            // B) llançar la consulta
                            int filesAfectades = consulta.ExecuteNonQuery();
                            if (filesAfectades != 1)
                            {
                                trans.Rollback();
                            }
                            else
                            {
                                trans.Commit();
                                return true;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Deixar registre al log (coming soon)
                Debug.WriteLine(ex);
            }

            return false;
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
