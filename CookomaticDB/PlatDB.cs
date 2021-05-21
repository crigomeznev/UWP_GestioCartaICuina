using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Diagnostics;
using System.Text;
using Windows.UI.Xaml.Media.Imaging;

namespace CookomaticDB
{
    public class PlatDB
    {
        private long codi;
        private string nom;
        private string descripcioMD;
        private decimal preu;
        private BitmapImage foto;
        private bool disponible;

        public PlatDB(long codi, string nom, string descripcioMD, decimal preu, BitmapImage foto, bool disponible)
        {
            Codi = codi;
            Nom = nom;
            DescripcioMD = descripcioMD;
            Preu = preu;
            Foto = foto;
            Disponible = disponible;
        }

        public PlatDB()
        {
        }

        public long Codi { get => codi; set => codi = value; }
        public string Nom { get => nom; set => nom = value; }
        public string DescripcioMD { get => descripcioMD; set => descripcioMD = value; }
        public decimal Preu { get => preu; set => preu = value; }
        public BitmapImage Foto { get => foto; set => foto = value; }
        public bool Disponible { get => disponible; set => disponible = value; }
        //private List<LiniaEscandall> escandall;



        public static ObservableCollection<PlatDB> GetPlats()
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
                            ObservableCollection<PlatDB> plats = new ObservableCollection<PlatDB>();
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

                                PlatDB plat = new PlatDB(codi, nom, descripcioMD, preu, null, disponible);
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

        public static ObservableCollection<PlatDB> GetPlatsPerCategoria(long categoria)
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
                                $@"select * from plat
                                    where categoria = @pCategoria";
                            DBUtils.crearParametre(consulta, "pCategoria", System.Data.DbType.Int64, categoria);


                            // B) llançar la consulta
                            DbDataReader reader = consulta.ExecuteReader();

                            // C) recórrer els resultats de la consulta
                            ObservableCollection<PlatDB> plats = new ObservableCollection<PlatDB>();
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

                                PlatDB plat = new PlatDB(codi, nom, descripcioMD, preu, null, disponible);
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

        // TODO: podríem tenir una funció que filtrés alhora per varis camps, o només per un, depenent si s'informés el camp o no?
        public static ObservableCollection<PlatDB> GetPlatsPerNom(string nomPlat)
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
                                $@"select * from plat
                                    where upper(nom) like @pNomPlat";
                            DBUtils.crearParametre(consulta, "pNomPlat", System.Data.DbType.String, "%" + nomPlat + "%");


                            // B) llançar la consulta
                            DbDataReader reader = consulta.ExecuteReader();

                            // C) recórrer els resultats de la consulta
                            ObservableCollection<PlatDB> plats = new ObservableCollection<PlatDB>();
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

                                PlatDB plat = new PlatDB(codi, nom, descripcioMD, preu, null, disponible);
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
    }
}
