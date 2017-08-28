using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

// Usings je Platform

#if __IOS__
using System.Drawing;
using UIKit;
using CoreGraphics;
#endif


#if __ANDROID__
using Android.Graphics;
#endif

namespace PeopleApp.Core
{
    public static class ImageResizer
    {
        static ImageResizer()
        {
        }


        public static byte[] ResizeImage(byte[] imageData, float width, float height)
        {
#if __IOS__
            return ResizeImageIOS(imageData, width, height);
#endif
#if __ANDROID__
            return ResizeImageAndroid(imageData, width, height);
#endif
        }
        //
#if __IOS__

        public static byte[] ResizeImageIOS(byte[] imageData, float width, float height)
        {
            // Load the bitmap
            UIImage originalImage = ImageFromByteArray(imageData);
            //
            var Hoehe = originalImage.Size.Height;
            var Breite = originalImage.Size.Width;
            //
            nfloat ZielHoehe = 0;
            nfloat ZielBreite = 0;
            //

            if (Hoehe > Breite) // Höhe (71 für Avatar) ist Master
            {
                ZielHoehe = height;
                nfloat teiler = Hoehe / height;
                ZielBreite = Breite / teiler;
            }
            else // Breite (61 for Avatar) ist Master
            {
                ZielBreite = width;
                nfloat teiler = Breite / width;
                ZielHoehe = Hoehe / teiler;
            }
            //
            width = (float)ZielBreite;
            height = (float)ZielHoehe;
            //
            UIGraphics.BeginImageContext(new SizeF(width, height));
            originalImage.Draw(new RectangleF(0, 0, width, height));
            var resizedImage = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();
            //
            var bytesImagen = resizedImage.AsJPEG().ToArray();
            resizedImage.Dispose();
            return bytesImagen;
        }
        //
        public static UIKit.UIImage ImageFromByteArray(byte[] data)
        {
            if (data == null)
            {
                return null;
            }
            //
            UIKit.UIImage image;
            try
            {
                image = new UIKit.UIImage(Foundation.NSData.FromArray(data));
            }
            catch (Exception e)
            {
                Console.WriteLine("Image load failed: " + e.Message);
                return null;
            }
            return image;
        }
#endif
        //
#if __ANDROID__
        public static byte[] ResizeImageAndroid(byte[] imageData, float width, float height)
        {
            // Load the bitmap 
            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
            //
            float targetedHeight = 0;
            float targetedWidth = 0;
            //
            var Height = originalImage.Height;
            var Width = originalImage.Width;
            //
            if (Height > Width) // Height (71 for avatar) is master
            {
                targetedHeight = height;
                float teiler = Height / height;
                targetedWidth = Width / teiler;
            }
            else // Width (61 for avatar) is master
            {
                targetedWidth = width;
                float divider = Width / width;
                targetedHeight = Height / divider;
            }
            //
            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)targetedWidth, (int)targetedHeight, false);
            // 
            using (MemoryStream ms = new MemoryStream())
            {
                resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
                return ms.ToArray();
            }
        }
#endif
    }
}