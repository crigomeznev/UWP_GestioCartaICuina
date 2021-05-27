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
        //private CategoriaDB categoria;
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
        public BitmapImage Foto
        {
            get => foto;
            set 
            {
                // Quan modifiquem la foto Bitmap aplicarem els canvis a la foto byte-array
                foto = value;
                //foto.


                FotoBa = ImageToByteArray(value);
            }
        }
        public PlatDB PlatOriginal { get => platOriginal; set => platOriginal = value; }

        public async Task iniFotoAsync()
        {
            // Construim BitmapImage a partir de la byte[]
            Foto = await ImageFromBytes(fotoBa);
        }






        public static byte[] ImageToByteArray(BitmapImage imageSource)
        {
            byte[] buffer = null;
            using (MemoryStream ms = new MemoryStream())
            {
                //WriteableBitmap wb = new WriteableBitmap(imageSource);
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

        //public static async Task<BitmapImage> ImageFromBytes(Byte[] bytes)
        //{
        //    BitmapImage image = new BitmapImage();
        //    WriteableBitmap writeableBitmap;
        //    using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
        //    {
        //        writeableBitmap = StreamToWriteableBitmap(stream);

        //        //await stream.WriteAsync(bytes.AsBuffer());
        //        //stream.Seek(0);
        //        //await image.SetSourceAsync(stream);
        //    }
        //    image = (BitmapImage)writeableBitmap;
        //    return image;
        //}


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


        //public BitmapImage Convert(WriteableBitmap wb)
        //{
        //    EditableImage imageData = new EditableImage(wb.PixelWidth, wb.PixelHeight);

        //    for (int y = 0; y < wb.PixelHeight; ++y)
        //    {
        //        for (int x = 0; x < wb.PixelWidth; ++x)
        //        {
        //            int pixel = wb.Pixels[wb.PixelWidth * y + x];
        //            imageData.SetPixel(x, y,
        //            (byte)((pixel >> 16) & 0xFF),
        //            (byte)((pixel >> 8) & 0xFF),
        //            (byte)(pixel & 0xFF), (byte)((pixel >> 24) & 0xFF)
        //            );
        //        }
        //    }

        //    MemoryStream pngStream = imageData.GetStream();

        //    BitmapImage bi = new BitmapImage();
        //    bi.SetSource(pngStream);

        //    return bi;
        //}


        //public async Task<Image> ConvertBitmapToXamarinImage(WriteableBitmap bitmap)
        //{
        //    byte[] pixels;

        //    using (var stream = bitmap.PixelBuffer.AsStream())
        //    {
        //        pixels = new byte[(uint)stream.Length];
        //        await stream.ReadAsync(pixels, 0, pixels.Length);
        //    }

        //    var raStream = new InMemoryRandomAccessStream();

        //    // Encode pixels into stream
        //    var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, raStream);
        //    encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied, (uint)bitmap.PixelWidth, (uint)bitmap.PixelHeight, 96, 96, pixels);
        //    await encoder.FlushAsync();

        //    return new Image() { Source = ImageSource.FromStream(() => raStream.AsStreamForRead()) };

        //}
    }
}
