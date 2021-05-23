using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace CookomaticDB
{
    public class ImageConvertor
    {
        public ImageConvertor()
        {
        }
        public Image ConvertByteArrayToImage(byte[] byteArrayIn)
        {
            using (MemoryStream ms = new MemoryStream(byteArrayIn))
            {
                return Image.FromStream(ms);
            }
        }
        public byte[] ConvertImageToByteArray(Image image, string extension)
        {
            using (var memoryStream = new MemoryStream())
            {
                switch (extension)
                {
                    case ".jpeg":
                    case ".jpg":
                        image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case ".png":
                        image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case ".gif":
                        image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                }
                return memoryStream.ToArray();
            }
        }


        //public BitmapImage ImageFromBuffer(Byte[] bytes)
        //{
        //    MemoryStream stream = new MemoryStream(bytes);
        //    BitmapImage image = new BitmapImage();

        //    image.

        //    image.SetSource(stream);
        //    image.BeginInit();
        //    image.StreamSource = stream;
        //    image.EndInit();
        //    return image;
        //}
    }
}
