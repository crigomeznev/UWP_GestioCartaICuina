using CookomaticDB.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
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
        private byte[] fotoBa;
        private BitmapImage foto;
        private PlatDB platOriginal;

        public PlatViewModel(long codi, string nom, string descripcioMD, decimal preu, bool disponible, byte[] fotoBa, PlatDB platOriginal)
        {
            Codi = codi;
            Nom = nom;
            DescripcioMD = descripcioMD;
            Preu = preu;
            Disponible = disponible;
            FotoBa = fotoBa;
            PlatOriginal = platOriginal;
        }

        public PlatViewModel(PlatDB platOriginal)
        {
            PlatOriginal = platOriginal;

            Codi = platOriginal.Codi;
            Nom = platOriginal.Nom;
            DescripcioMD = platOriginal.DescripcioMD;
            Preu = platOriginal.Preu;
            Disponible = platOriginal.Disponible;
            FotoBa = platOriginal.Foto;
        }

        public PlatViewModel()
        {
        }



        // En tots els setters, el viewModel actualitzarà l'objecte DB
        public long Codi
        {
            get => codi;
            set
            {
                codi = value;
                if (platOriginal != null)
                    platOriginal.Codi = value;
            }
        }
        public string Nom
        {
            get => nom;
            set 
            {
                if (value == null || ((string)value).Length == 0) value = "Sense nom";
                nom = value;
                if (platOriginal!=null)
                    platOriginal.Nom = value;
            }
        }
        public string DescripcioMD
        {
            get => descripcioMD;
            set
            {
                descripcioMD = value;
                if (platOriginal != null)
                    platOriginal.DescripcioMD = value;
            }
        }
        public decimal Preu
        {
            get => preu;
            set 
            {
                preu = value;
                if (platOriginal != null)
                    platOriginal.Preu = value;
            }
        }
        public bool Disponible 
        { 
            get => disponible;
            set
            { 
                disponible = value;
                if (platOriginal != null)
                    platOriginal.Disponible = value;
            }
        }
        public byte[] FotoBa
        {
            get => fotoBa;
            set 
            { 
                fotoBa = value;
                if (platOriginal != null)
                    platOriginal.Foto = value;
            }
        }

        // Propietats només de PlatViewModel

        // Per mostrar la imatge en la UI
        public BitmapImage Foto
        {
            get => foto;
            set 
            {
                // Quan modifiquem la foto Bitmap aplicarem els canvis a la foto byte-array
                foto = value;
                //FotoBa = ImageToByteArray(value);
            }
        }
        public PlatDB PlatOriginal { get => platOriginal; set => platOriginal = value; }

        public async Task iniFotoAsync()
        {
            // Construim BitmapImage a partir de la byte[]
            if (FotoBa != null)
                Foto = await ImageFromBytes(fotoBa);
        }






        public static byte[] ImageToByteArray(BitmapImage imageSource)
        {
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                WriteableBitmap wb = new WriteableBitmap(imageSource.PixelWidth, imageSource.PixelHeight);
                Stream s1 = wb.PixelBuffer.AsStream();
                s1.CopyTo(ms);

                buffer = ms.ToArray();
            }
            return buffer;
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


        public static WriteableBitmap StreamToWriteableBitmap(IRandomAccessStream stream)
        {
            // initialize with 1,1 to get the current size of the image
            var writeableBmp = new WriteableBitmap(1, 1);
            writeableBmp.SetSource(stream);

            // and create it again because otherwise the WB isn't fully initialized and decoding
            // results in a IndexOutOfRange
            writeableBmp = new WriteableBitmap(writeableBmp.PixelWidth, writeableBmp.PixelHeight);

            stream.Seek(0);
            writeableBmp.SetSource(stream);

            return writeableBmp;
        }


    }
}
