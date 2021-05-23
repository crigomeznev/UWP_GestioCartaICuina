using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using Windows.UI.Xaml.Media.Imaging;

namespace CookomaticDB.Model
{
    public class PlatDB
    {
        private long codi;
        private string nom;
        private string descripcioMD;
        private decimal preu;
        private BitmapImage foto;
        private Image foto2;
        private bool disponible;
        private CategoriaDB categoria;

        public PlatDB(long codi, string nom, string descripcioMD, decimal preu, BitmapImage foto, bool disponible, CategoriaDB categoria)
        {
            Codi = codi;
            Nom = nom;
            DescripcioMD = descripcioMD;
            Preu = preu;
            Foto = foto;
            Disponible = disponible;
            Categoria = categoria;
        }
        
        //public PlatDB(long codi, string nom, string descripcioMD, decimal preu, Image foto2, bool disponible, CategoriaDB categoria)
        //{
        //    Codi = codi;
        //    Nom = nom;
        //    DescripcioMD = descripcioMD;
        //    Preu = preu;
        //    Foto2 = foto2;
        //    Disponible = disponible;
        //    Categoria = categoria;
        //}

        public PlatDB()
        {
        }

        public long Codi { get => codi; set => codi = value; }
        public string Nom { get => nom; set => nom = value; }
        public string DescripcioMD { get => descripcioMD; set => descripcioMD = value; }
        public decimal Preu { get => preu; set => preu = value; }
        public BitmapImage Foto { get => foto; set => foto = value; }
        public bool Disponible { get => disponible; set => disponible = value; }
        public CategoriaDB Categoria { get => categoria; set => categoria = value; }
        public Image Foto2 { get => foto2; set => foto2 = value; }

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
                                //byte[] foto_ba;
                                //DBUtils.LlegeixFoto(reader, out foto_ba, "foto");
                                //ImageConvertor ic = new ImageConvertor();
                                //Image foto = ic.ConvertByteArrayToImage(foto_ba);


                                PlatDB plat = new PlatDB(codi, nom, descripcioMD, preu, null, disponible, null);
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

        public static ObservableCollection<PlatDB> GetPlatsPerCategoria(CategoriaDB categoria)
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
                            DBUtils.crearParametre(consulta, "pCategoria", System.Data.DbType.Int64, categoria.Codi);


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
                                //byte[] foto_ba;
                                //DBUtils.LlegeixFoto(reader, out foto_ba, "foto");
                                //ImageConvertor ic = new ImageConvertor();
                                //Image foto = ic.ConvertByteArrayToImage(foto_ba);

                                PlatDB plat = new PlatDB(codi, nom, descripcioMD, preu, null, disponible, null);
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
                                //byte[] foto_ba;
                                //DBUtils.LlegeixFoto(reader, out foto_ba, "foto");
                                //ImageConvertor ic = new ImageConvertor();
                                //Image foto = ic.ConvertByteArrayToImage(foto_ba);


                                PlatDB plat = new PlatDB(codi, nom, descripcioMD, preu, null, disponible, null);
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



        public bool Delete()
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

                            // la taula afectada és PLAT i té un comportament de on delete = CASCADE sobre línies d'escandall
                            consulta.CommandText = $@"delete from plat where codi = @codi";
                            DBUtils.crearParametre(consulta, "codi", System.Data.DbType.Int32, this.Codi);

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
            }
            return false;
        }

        public bool Insert()
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
                            // A) definir la consulta


                            consulta.CommandText = $@"
                                INSERT INTO PLAT (NOM, DESCRIPCIO_MD, PREU, FOTO, DISPONIBLE, CATEGORIA)
                                VALUES (@nom, @descripcioMD, @preu, @foto, @disponible, @categoria);
                            ";


                            DBUtils.crearParametre(consulta, "nom", System.Data.DbType.String, this.Nom);
                            DBUtils.crearParametre(consulta, "descripcioMD", System.Data.DbType.String, this.DescripcioMD);
                            DBUtils.crearParametre(consulta, "preu", System.Data.DbType.Decimal, this.Preu);
                            DBUtils.crearParametre(consulta, "foto", System.Data.DbType.Object, this.Foto);
                            DBUtils.crearParametre(consulta, "disponible", System.Data.DbType.Boolean, this.Disponible);
                            DBUtils.crearParametre(consulta, "categoria", System.Data.DbType.Int64, this.Categoria.Codi);

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
            }

            return false;
        }

    }
}
