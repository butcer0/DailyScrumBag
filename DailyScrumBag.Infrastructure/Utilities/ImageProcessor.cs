using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DailyScrumBag.Infrastructure.Utilities
{
    /// <summary>
    /// ImageProcessor, handles logic related to Initialization.
    /// </summary>
    public class ImageProcessor
    {
        // Maximum width of the image generated from the point array
        const int IMAGE_MAX_WIDTH = 4096;

        // Maximum height of the image generated from the point array
        const int IMAGE_MAX_HEIGHT = 4096;

        // Maximum horizontal size (width) to resize an image
        const int IMAGE_MAX_HSIZE = 4096;

        // Maximum vertical size (height) to resize an image
        const int IMAGE_MAX_VSIZE = 4096;

        /// <summary>
        /// Get Signature Image from Point Array, Resize the Image and Convert to a specific format
        /// </summary>
        /// <param name="imagePointArrayString">Signature Image Point Array String</param>
        /// <param name="format">Image Format</param>
        /// <param name="hSize">Signature Width</param>
        /// <param name="vSize">Signature Height</param>
        /// <returns>Returns the Image Blob (Byte Array) of the Signature</returns>
        public static Byte[] GetSignatureImageFromPointArray(string imagePointArrayString, string format = "bmp2", int hSize = 304, int vSize = 64)
        {
            // input checks
            if (string.IsNullOrEmpty(imagePointArrayString))
            {
                throw new Exception("Invalid point array image");
            }

            if (string.IsNullOrEmpty(format))
            {
                throw new Exception("Invalid format");
            }

            if (hSize > IMAGE_MAX_HSIZE)
            {
                throw new Exception("Invalid image width");
            }

            if (vSize > IMAGE_MAX_VSIZE)
            {
                throw new Exception("Invalid image height");
            }

            if (format == "bmp" || format == "bitmap")
            {
                format = "bmp2";
            }
            else if (format == "jpg")
            {
                format = "jpeg";
            }
            else if (format == "tif")
            {
                format = "tiff";
            }

            // Draw Bitmap Image from the Point Array
            Bitmap image = DrawImageFromPointArray(imagePointArrayString, IMAGE_MAX_WIDTH, IMAGE_MAX_HEIGHT);

            // Resize the Image
            image = ResizeImage(image, hSize, vSize);

            // Convert the Image into specific format
            Byte[] imageBlob = ConvertImageIntoSpecificFormat(image, format);

            // Convert Image into Monochrome for other images than TIFF [only for bmp images: bmp2, bmp3 and bmp4 (called bmp), JPEG, PNG etc]
            if (format.ToLower() != "tiff")
            {
                Bitmap monochromeImage = ConvertImageIntoMonochrome(imageBlob);
                imageBlob = GetImageBlob(monochromeImage, format);
            }

            return imageBlob;
        }

        /// <summary>
        /// Converts the input signature text into Bitmap image, resizes it, converts it to bmp and monochrome
        /// </summary>
        /// <param name="base64EncodedSignatureText">Base64 Encoded Signature Text</param>
        /// <param name="format">Image Format</param>
        /// <returns>Returns the Image Blob (Byte Array) of the Signature</returns>
        public static Byte[] ProcessSignatureImage(string base64EncodedSignatureText, string format = "bmp2")
        {
            // Input checks
            if (string.IsNullOrEmpty(base64EncodedSignatureText))
            {
                throw new Exception("Invalid image");
            }

            if (string.IsNullOrEmpty(format))
            {
                throw new Exception("Invalid format");
            }

            if (format == "bmp" || format == "bitmap")
            {
                format = "bmp2";
            }
            else if (format == "jpg")
            {
                format = "jpeg";
            }
            else if (format == "tif")
            {
                format = "tiff";
            }

            // Load the image content from string
            Bitmap image = ConvertBase64StringToImage(base64EncodedSignatureText);

            // Scale the image
            if (format == "tiff")
            {
                // As per McKesson's specs, tiff signature should be 380 by 100
                ResizeImage(image, 380, 100);
            }
            else
            {
                // As per MerchantSoft 8 specs, bmp signature should be 304 by 64
                ResizeImage(image, 304, 64);
            }

            // Convert image to specific format - default is old bmp2 format (no alpha channel, maximum compatibility with windows and access)
            Byte[] imageBlob = ConvertImageIntoSpecificFormat(image, format);

            // Convert Image into Monochrome for other images than TIFF [only for bmp images: bmp2, bmp3 and bmp4 (called bmp), JPEG, PNG etc]
            if (format.ToLower() != "tiff")
            {
                Bitmap monochromeImage = ConvertImageIntoMonochrome(imageBlob);
                imageBlob = GetImageBlob(monochromeImage, format);
            }

            return imageBlob;
        }

        /// <summary>
        /// Draws the Image from Point Array Text
        /// </summary>
        /// <param name="pointArrayString">Point Array String</param>
        /// <param name="xMax">Maximum Image Width</param>
        /// <param name="yMax">Maximum Image Height</param>
        /// <returns>Returns the Bitmap Image</returns>
        public static Bitmap DrawImageFromPointArray(string pointArrayString, int xMax, int yMax)
        {
            // get x,y pairs into an array by splitting on the points separator
            var points = pointArrayString.Split('^');

            // get max x and y
            var width = 0;
            var height = 0;

            foreach (var point in points)
            {
                // coordinates are separated by ,
                string[] coord = point.Split(',');

                // need 2 coordinates per point both numerical, otherwise skip it
                if (coord.Count() == 2 && IsNumeric(coord[0]) && IsNumeric(coord[1]))
                {
                    var x = (int)Math.Round(Convert.ToDecimal(coord[0]), 0);
                    var y = (int)Math.Round(Convert.ToDecimal(coord[1]), 0);

                    // ignore pen down/up and stay within limits
                    if (x != 65535 && y != 65535 && x <= xMax && y <= yMax)
                    {
                        width = (x > width) ? x : width;
                        height = (y > height) ? y : height;
                    }
                }
            }

            // determine the size of the image
            width = (width > xMax) ? xMax : width;
            height = (height > yMax) ? yMax : height;

            // make sure the image width and height are non negative
            width = (width < 0) ? 0 : width;
            height = (height < 0) ? 0 : height;

            // add some padding around the image
            width += 10;
            height += 10;

            // draw the image
            Bitmap image = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(image);
            SolidBrush whiteBrush = new SolidBrush(Color.White);
            graphics.FillRectangle(whiteBrush, 0, 0, width, height);

            // draw a line for each couple of points
            var i = 0;

            while (i < points.Count())
            {
                string[] coord = points[i].Split(',');

                // need 2 coordinates per point both numerical, otherwise skip it
                if (coord.Count() == 2 && IsNumeric(coord[0]) && IsNumeric(coord[1]))
                {
                    var x1 = (int)Math.Round(Convert.ToDecimal(coord[0]), 0);
                    var y1 = (int)Math.Round(Convert.ToDecimal(coord[1]), 0);

                    // skip pen-up and pen-down points (65535,65535)
                    if (x1 != 65535 && y1 != 65535)
                    {
                        // check if there is a next point to connect with a line
                        if (points[i + 1] != null)
                        {
                            var coord2 = points[i + 1].Split(',');

                            var x2 = 0;
                            var y2 = 0;

                            // need 2 coordinates per point both numerical, otherwise skip it
                            if (coord2.Count() == 2 && IsNumeric(coord2[0]) && IsNumeric(coord2[1]))
                            {
                                x2 = (int)Math.Round(Convert.ToDecimal(coord2[0]), 0);
                                y2 = (int)Math.Round(Convert.ToDecimal(coord2[1]), 0);
                            }

                            // skip pen-up and pen-down points (65535,65535)
                            if (x2 != 65535 && y2 != 65535)
                            {
                                graphics.DrawLine(new Pen(Color.Black, 2), x1, y1, x2, y2);
                            }
                        }
                    }
                }

                i++;
            }

            return image;
        }

        /// <summary>
        /// Converts the Image into specific format
        /// </summary>
        /// <param name="image">Bitmap Image</param>
        /// <param name="format">Image Type</param>
        /// <returns>Returns the Byte Array of the image after converting it into specific format</returns>
        // TODO: 1. Strip all profiles and comments from image to further shrink the size. This remove Exif (Exchangeable image file format) meta data from image.
        //       2. Set Image Channel Depth to "1".
        public static Byte[] ConvertImageIntoSpecificFormat(Bitmap image, string imageFormat)
        {
            Byte[] imageBlob = null;

            using (MemoryStream ms = new MemoryStream())
            {
                Bitmap bmp = new Bitmap(image);

                if (imageFormat.ToLower() == "jpeg")
                {
                    // Set compression to JPEG (lossy), compression quality = 85
                    ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                    EncoderParameters encoderParameters = new EncoderParameters(2);
                    EncoderParameter qualityEncoderParameter = new EncoderParameter(Encoder.Quality, 85L);  // Set the compression quality of the image to 85
                    EncoderParameter colorEncoderParameter = new EncoderParameter(Encoder.ColorDepth, 1L);  // Set the color depth of the image to 1 bit per pixel
                    encoderParameters.Param[0] = qualityEncoderParameter;
                    encoderParameters.Param[1] = colorEncoderParameter;

                    bmp.Save(ms, jpgEncoder, encoderParameters);
                }
                else if (imageFormat.ToLower() == "tiff")
                {
                    // Set image format to TIFF
                    ImageCodecInfo tiffEncoder = GetEncoder(ImageFormat.Tiff);
                    EncoderParameters encoderParameters = new EncoderParameters(2);
                    EncoderParameter colorEncoderParameter = new EncoderParameter(Encoder.ColorDepth, 1L);  // Set the color depth of the image to 1 bit per pixel
                    EncoderParameter compressionEncoderParameter = new EncoderParameter(Encoder.Compression, (long)EncoderValue.CompressionCCITT4);  // Set the TIFF Compression to CCITT Group 4
                    encoderParameters.Param[0] = colorEncoderParameter;
                    encoderParameters.Param[1] = compressionEncoderParameter;

                    bmp.Save(ms, tiffEncoder, encoderParameters);
                }
                else  // 'bmp2' image format will be defaulted to PNG
                {
                    // Set image format to PNG
                    ImageCodecInfo pngEncoder = GetEncoder(ImageFormat.Png);
                    EncoderParameters encoderParameters = new EncoderParameters(1);
                    EncoderParameter colorEncoderParameter = new EncoderParameter(Encoder.ColorDepth, 1L);  // Set the color depth of the image to 24 bits per pixel
                    encoderParameters.Param[0] = colorEncoderParameter;

                    bmp.Save(ms, pngEncoder, encoderParameters);
                }

                imageBlob = ms.ToArray();
            }

            return imageBlob;
        }

        /// <summary>
        /// Gets the Encoder for the given Image Format
        /// </summary>
        /// <param name="format">Image Format</param>
        /// <returns>Returns the Encoder for the given Image Format</returns>
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        /// <summary>
        /// Converts the image into Monochrome (to 2 colors black & white)
        /// </summary>
        /// <param name="imageBlob">Image Byte Array</param>
        /// <returns>Returns the Bitmap image in black & white</returns>
        public static Bitmap ConvertImageIntoMonochrome(Byte[] imageBlob)
        {
            Bitmap image = null;

            using (MemoryStream ms = new MemoryStream(imageBlob, 0, imageBlob.Length))
            {
                int rgb;
                Color pixelColor;

                ms.Write(imageBlob, 0, imageBlob.Length);  // Convert byte[] to Image
                image = (Bitmap)Image.FromStream(ms, true);

                for (int y = 0; y < image.Height; y++)
                {
                    for (int x = 0; x < image.Width; x++)
                    {
                        pixelColor = image.GetPixel(x, y);
                        rgb = (int)((pixelColor.R * .3) + (pixelColor.G * .59) + (pixelColor.B * .11));
                        image.SetPixel(x, y, Color.FromArgb(rgb, rgb, rgb));
                    }
                }
            }

            return image;
        }

        private static Byte[] GetImageBlob(Bitmap image, string imageFormat)
        {
            Byte[] imageBlob = null;

            using (MemoryStream ms = new MemoryStream())
            {
                Bitmap bmp = new Bitmap(image);

                if (imageFormat.ToLower() == "jpeg")
                {
                    bmp.Save(ms, ImageFormat.Jpeg);
                }
                else if (imageFormat.ToLower() == "tiff")
                {
                    bmp.Save(ms, ImageFormat.Tiff);
                }
                else  // 'bmp2' image format will be defaulted to PNG
                {
                    bmp.Save(ms, ImageFormat.Png);
                }

                imageBlob = ms.ToArray();
            }

            return imageBlob;
        }

        /// <summary>
        /// Resizes the Bitmap image into given Width & Height
        /// </summary>
        /// <param name="originalImage">Original Bitmap Image</param>
        /// <param name="newWidth">New Width</param>
        /// <param name="newHeight">New Height</param>
        /// <returns>Returns the resized version of the given image</returns>
        public static Bitmap ResizeImage(Image originalImage, int newWidth, int newHeight)
        {
            Size newSize = new Size(newWidth, newHeight);
            Bitmap image = new Bitmap(originalImage, newSize);
            return image;
        }

        /// <summary>
        /// Encodes the given Image Byte Array into Base64 String
        /// </summary>
        /// <param name="imageBlob">Image Byte Array</param>
        /// <returns>Returns the Encoded Base64 String</returns>
        public static string Base64Encode(Byte[] imageBlob)
        {
            return Convert.ToBase64String(imageBlob, 0, imageBlob.Length);
        }

        /// <summary>
        /// Encodes the given string value into Base64 
        /// </summary>
        /// <param name="imageString">Image string to be Encoded</param>
        /// <returns>Returns the Encoded Base64 String</returns>
        public static string Base64Encode(string imageString)
        {
            var imageBlob = System.Text.Encoding.UTF8.GetBytes(imageString);
            return Convert.ToBase64String(imageBlob);
        }

        /// <summary>
        /// Decodes the Base64 Encoded String
        /// </summary>
        /// <param name="base64EncodedData">Base64 Encoded String</param>
        /// <returns>Returns the Decoded String</returns>
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <summary>
        /// Converts the Base64 Encoded String to Bitmap image
        /// </summary>
        /// <param name="base64EncodedString">Base64 Encoded String</param>
        /// <returns>Returns the converted Bitmap image</returns>
        public static Bitmap ConvertBase64StringToImage(string base64EncodedString)
        {
            Bitmap image = null;
            byte[] imageBlob = Convert.FromBase64String(base64EncodedString);  // Convert Base64 String to byte[]

            using (MemoryStream ms = new MemoryStream(imageBlob, 0, imageBlob.Length))
            {
                // Convert byte[] to Image
                ms.Write(imageBlob, 0, imageBlob.Length);
                image = (Bitmap)Image.FromStream(ms, true);
            }

            return image;
        }

        /// <summary>
        /// Checks if the given value is a decimal number
        /// </summary>
        /// <param name="value">Input Value</param>
        /// <returns>Returns 'True' if the input value is a decimal number, else 'False'</returns>
        public static bool IsNumeric(string value)
        {
            Regex reNum = new Regex(@"^[1-9]\d*(\.\d+)?$");
            return reNum.Match(value).Success;
        }
    }
}
