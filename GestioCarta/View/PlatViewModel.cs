using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace GestioCarta.View
{
    public class PlatViewModel
    {
        private long codi;
        private string nom;
        private string descripcioMD;
        private decimal preu;
        private bool disponible;
        //private CategoriaDB categoria;
        private byte[] fotoBa;
        private BitmapImage foto;

        public PlatViewModel(long codi, string nom, string descripcioMD, decimal preu, bool disponible, byte[] fotoBa)
        {
            Codi = codi;
            Nom = nom;
            DescripcioMD = descripcioMD;
            Preu = preu;
            Disponible = disponible;
            FotoBa = fotoBa;
        }

        public long Codi { get => codi; set => codi = value; }
        public string Nom { get => nom; set => nom = value; }
        public string DescripcioMD { get => descripcioMD; set => descripcioMD = value; }
        public decimal Preu { get => preu; set => preu = value; }
        public bool Disponible { get => disponible; set => disponible = value; }
        public BitmapImage Foto { get => foto; set => foto = value; }
        public byte[] FotoBa { get => fotoBa; set => fotoBa = value; }

        public async Task iniFotoAsync()
        {
            // Construim BitmapImage a partir de la byte[]
            Foto = await ImageFromBytes(fotoBa);
        }








        public static async Task<BitmapImage> ImageFromBytes(Byte[] bytes)
        {
            BitmapImage image = new BitmapImage();
            using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
            {
                await stream.WriteAsync(bytes.AsBuffer());
                stream.Seek(0);
                await image.SetSourceAsync(stream);
            }
            return image;
        }
    }
}
