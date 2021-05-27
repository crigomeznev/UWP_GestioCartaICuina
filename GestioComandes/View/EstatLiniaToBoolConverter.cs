using CookomaticDB.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace GestioComandes.View
{
    public class EstatLiniaToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                EstatLinia estat = (EstatLinia)value;
                if (estat.Equals(EstatLinia.PREPARADA))
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            bool val = (bool)value;
            EstatLinia estat;
            if (val) estat = EstatLinia.PREPARADA;
            else estat = EstatLinia.EN_PREPARACIO;

            return estat;
        }




        // TODO BORRAR
        public async Task<object> ConvertAsync(object value, Type targetType, object parameter, string language)
        {
            try
            {
                byte[] img_ba = (byte[])value;
                BitmapImage bimage = await ImageFromBytes(img_ba);
                return bimage;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }
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


        //public Byte[] ImageToByte(BitmapImage bitmapImage)
        //{
        //    byte[] data = null;

        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        bitmapImage.Save(ms);
        //        data = ms.ToArray();
        //    }
        //}

        public async Task<BitmapImage> ByteToBitmapImageAsync(byte[] array)
        {
            //This task will return aimage from provided the byte[].  
            using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
            {
                using (DataWriter writer = new DataWriter(stream.GetOutputStreamAt(0)))
                {
                    writer.WriteBytes(array);
                    await writer.StoreAsync();
                }
                BitmapImage image = new BitmapImage();
                await image.SetSourceAsync(stream);
                return image;
            }
        }


        // Image to byte array
//using(var inputStream = await file.OpenSequentialReadAsync())  
//{  
//    var readStream = inputStream.AsStreamForRead();
//    byte[] buffer = newbyte[readStream.Length];
//    await readStream.ReadAsync(buffer, 0, buffer.Length);
//    returnbuffer;  
//}  


        //public async Task<BitmapImage> ImageFromBytes(Byte[] bytes)
        //{
        //    BitmapImage image = new BitmapImage();
        //    using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
        //    {
        //        await stream.WriteAsync(bytes.AsBuffer());
        //        stream.Seek(0);
        //        await image.SetSourceAsync(stream);
        //    }
        //    return image;
        //}

        //public static Image ByteArrayToImage(byte[] bytes)
        //{
        //    //var obj = new ImageConvertor();
        //    //System.Drawing.Bitmap bmpPostedImage =
        //    //    new System.Drawing.Bitmap(FileUploadControl.PostedFile.InputStream);
        //    //byte[] imageByteArray = obj.ConvertImageToByteArray(bmpPostedImage, ".png");


        //    Image imageIn = ImageConvertor.ConvertByteArrayToImage(bytes);

        //    return imageIn;
        //    //string saveImagePath = Server.MapPath("~/images/") + "Image1.png";
        //    //File.WriteAllBytes(saveImagePath, imageByteArray);
        //    //StatusLabel.Text = "Upload status: File uploaded!'";
        //    //StatusLabel.ForeColor = Color.Red;
        //}
    }
}
