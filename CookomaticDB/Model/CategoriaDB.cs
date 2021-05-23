using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace CookomaticDB.Model
{
    public class CategoriaDB
    {
        private long codi;
        private string nom;
        private Color color;

        public CategoriaDB(long codi, string nom, Color color)
        {
            Codi = codi;
            Nom = nom;
            Color = color;
        }

        public CategoriaDB()
        {
        }

        public long Codi { get => codi; set => codi = value; }
        public string Nom { get => nom; set => nom = value; }
        public Color Color { get => color; set => color = value; }

        public override string ToString()
        {
            return Nom;
        }

        public static ObservableCollection<CategoriaDB> GetCategories()
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
                            consulta.CommandText = "select * from categoria";

                            // B) llançar la consulta
                            DbDataReader reader = consulta.ExecuteReader();

                            // C) recórrer els resultats de la consulta
                            ObservableCollection<CategoriaDB> categories = new ObservableCollection<CategoriaDB>();
                            while (reader.Read())
                            {
                                long codi = reader.GetInt64(reader.GetOrdinal("codi"));
                                string nom = reader.GetString(reader.GetOrdinal("nom"));

                                // TODO: get color de la BD
                                //Color color;

                                //if (!reader.IsDBNull(reader.GetOrdinal("image_path")))
                                //    image_path = reader.GetString(reader.GetOrdinal("image_path"));

                                CategoriaDB categoria = new CategoriaDB(codi, nom, Color.Red);
                                categories.Add(categoria);
                            }
                            return categories;
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
