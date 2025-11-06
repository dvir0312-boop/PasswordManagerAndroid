using Android.Graphics;

namespace EmptyProject2025Extended.Data
{
    internal class DBHelperService
    {
        // convert from bitmap to byte array
        public byte[] BitmapToByte(Bitmap bitmap)
        {
            MemoryStream stream = new MemoryStream();
            bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
            return stream.ToArray();
        }

        // convert from byte array to bitmap
        public Bitmap ByteToImage(byte[] image)
        {
            return BitmapFactory.DecodeByteArray(image, 0, image.Length);
        }
        public void 
    }
}