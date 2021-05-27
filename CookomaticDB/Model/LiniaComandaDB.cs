using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using Windows.UI.Xaml.Media.Imaging;

namespace CookomaticDB.Model
{
    public class LiniaComandaDB : INotifyPropertyChanged
    {
        // INotifyPropertyChanged
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChange([CallerMemberName] string propertyname = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
        #endregion

        private int num;
        private int quantitat;
        private EstatLinia estat;
        private PlatDB item;

        //afegit
        private ComandaDB comanda;

        public LiniaComandaDB()
        {
        }

        public LiniaComandaDB(int num, int quantitat, EstatLinia estat, PlatDB item, ComandaDB comanda)
        {
            Num = num;
            Quantitat = quantitat;
            Estat = estat;
            Item = item;
            Comanda = comanda;
        }

        public int Num { get => num; set => num = value; }
        public int Quantitat { get => quantitat; set => quantitat = value; }
        public EstatLinia Estat
        {
            get => estat;
            set 
            { 
                estat = value;

                RaisePropertyChange();
                //RaisePropertyChange("Estat");
            }
        }
        public PlatDB Item { get => item; set => item = value; }
        public ComandaDB Comanda { get => comanda; set => comanda = value; }

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

                                // TODO: afegir ref a comanda
                                LiniaComandaDB linia = new LiniaComandaDB(numAux, qtatAux, estat, null, null);
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

        public static ObservableCollection<LiniaComandaDB> GetLiniesPerCodiComanda(long codiComanda)
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
                            consulta.CommandText = "select * from linia_comanda where comanda = @comanda order by estat";
                            DBUtils.crearParametre(consulta, "comanda", System.Data.DbType.Int64, codiComanda);

                            // B) llançar la consulta
                            DbDataReader reader = consulta.ExecuteReader();

                            // C) recórrer els resultats de la consulta
                            ObservableCollection<LiniaComandaDB> linies = new ObservableCollection<LiniaComandaDB>();
                            while (reader.Read())
                            {
                                int numAux = reader.GetInt32(reader.GetOrdinal("num"));
                                int qtatAux = reader.GetInt32(reader.GetOrdinal("quantitat"));
                                string estatAux = reader.GetString(reader.GetOrdinal("estat"));
                                var estat = (EstatLinia)Enum.Parse(typeof(EstatLinia), estatAux);

                                // POC EFICIENT: una altra connexio
                                PlatDB plat = PlatDB.GetPlatPerCodi(reader.GetInt64(reader.GetOrdinal("plat")));

                                // TODO: afegir ref a comanda
                                LiniaComandaDB linia = new LiniaComandaDB(numAux, qtatAux, estat, plat, null);
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

        public static ObservableCollection<LiniaComandaDB> GetLiniesPerComanda(ComandaDB comanda)
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
                            consulta.CommandText = "select * from linia_comanda where comanda = @comanda order by estat";
                            DBUtils.crearParametre(consulta, "comanda", System.Data.DbType.Int64, comanda.Codi);

                            // B) llançar la consulta
                            DbDataReader reader = consulta.ExecuteReader();

                            // C) recórrer els resultats de la consulta
                            ObservableCollection<LiniaComandaDB> linies = new ObservableCollection<LiniaComandaDB>();
                            while (reader.Read())
                            {
                                int numAux = reader.GetInt32(reader.GetOrdinal("num"));
                                int qtatAux = reader.GetInt32(reader.GetOrdinal("quantitat"));
                                string estatAux = reader.GetString(reader.GetOrdinal("estat"));
                                var estat = (EstatLinia)Enum.Parse(typeof(EstatLinia), estatAux);

                                // POC EFICIENT: una altra connexio
                                PlatDB plat = PlatDB.GetPlatPerCodi(reader.GetInt64(reader.GetOrdinal("plat")));

                                LiniaComandaDB linia = new LiniaComandaDB(numAux, qtatAux, estat, plat, comanda);
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

                                // TODO: afegir ref a comanda
                                LiniaComandaDB linia = new LiniaComandaDB(numAux, qtatAux, estat, null, null);
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
                            // No deixarem modificar del plat: Codi i categoria
                            /*
                            COMANDA
NUM
PLAT
QUANTITAT
ESTAT
                            */
                                // PK de LINIA_COMANDA: COMANDA I NUM -> MAI actualitzarem aquests valors
                            consulta.CommandText = $@"
                                    UPDATE LINIA_COMANDA
                                    SET PLAT = @PLAT, QUANTITAT = @QUANTITAT, ESTAT = @ESTAT
                                    WHERE COMANDA = @COMANDA AND NUM = @NUM";
                            DBUtils.crearParametre(consulta, "PLAT", System.Data.DbType.Int64, this.Item.Codi);
                            DBUtils.crearParametre(consulta, "QUANTITAT", System.Data.DbType.Int32, this.Quantitat);
                            DBUtils.crearParametre(consulta, "ESTAT", System.Data.DbType.String, this.Estat.ToString());
                            DBUtils.crearParametre(consulta, "COMANDA", System.Data.DbType.Int64, this.Comanda.Codi);
                            DBUtils.crearParametre(consulta, "NUM", System.Data.DbType.Int32, this.Num);

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
