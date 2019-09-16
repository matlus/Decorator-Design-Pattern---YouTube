using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DecoratorDesignPattern
{
    internal abstract class ThumbnailerBase
    {
        public Image GenerateThumbnail()
        {
            return GenerateThumbnailCore();
        }

        protected Image CreateImage(int width, int height, Color backgroundColor, Color foregroundColor, string text)
        {
            var bitmap = new Bitmap(width, height);
            try
            {
                using (var graphics = Graphics.FromImage(bitmap))
                using (var backgroundBrush = new SolidBrush(backgroundColor))
                using (var paintBrush = new SolidBrush(foregroundColor))
                using (var font = new Font("Arial", 18f))
                {
                    graphics.FillRectangle(backgroundBrush, 0, 0, bitmap.Width, bitmap.Height);
                    graphics.DrawString(text, font, paintBrush, 10, 10);
                    return bitmap;
                }
            }
            catch (Exception)
            {
                bitmap.Dispose();
                throw;
            }
        }

        protected abstract Image GenerateThumbnailCore();
    }

    internal sealed class ThumbnailerAudio : ThumbnailerBase
    {
        protected override Image GenerateThumbnailCore()
        {
            return CreateImage(300, 300, Color.DarkOliveGreen, Color.White, "I am an Audio Thumbnail");
        }
    }

    internal sealed class ThumbnailerVideo : ThumbnailerBase
    {
        protected override Image GenerateThumbnailCore()
        {
            return CreateImage(480, 270, Color.DarkBlue, Color.WhiteSmoke, "I am a Video Thumbnail");
        }
    }

    internal sealed class ThumbnailerImage : ThumbnailerBase
    {
        protected override Image GenerateThumbnailCore()
        {
            Bitmap bitmap = new Bitmap(@"..\..\IoTInThePalmOfYourHand.png");
            Graphics graphics = null;
            try
            {
                var requiredWith = 640;
                var requiredHeight = requiredWith * bitmap.Height / bitmap.Width;

                var thumbnail = new Bitmap(requiredWith, requiredHeight);
                graphics = Graphics.FromImage(thumbnail);
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.DrawImage(bitmap, 0, 0, requiredWith, requiredHeight);
                return thumbnail;
            }
            finally
            {
                graphics?.Dispose();
                bitmap.Dispose();
            }
        }
    }

    internal sealed class ThumbnailerDecoratorWatermark : ThumbnailerBase, IDisposable
    {
        private const string WatermarkImageFilename = @"..\..\ProgrammingWithIntent.png";
        private readonly Image _watermarkImage = Bitmap.FromFile(WatermarkImageFilename);
        private bool _disposed;

        private readonly ThumbnailerBase _thumbnailer;

        public ThumbnailerDecoratorWatermark(ThumbnailerBase thumbnailer)
        {
            _thumbnailer = thumbnailer;
        }
        protected override Image GenerateThumbnailCore()
        {
            return DecorateWithWatermark(_thumbnailer.GenerateThumbnail(), _watermarkImage);
        }

        public static Image DecorateWithWatermark(Image primaryImage, Image watermarkImage)
        {
            using (Graphics imageGraphics = Graphics.FromImage(primaryImage))
            {
                var margin = 10;
                int x = margin;
                int y = primaryImage.Height - (watermarkImage.Height + margin);

                imageGraphics.DrawImage(watermarkImage, x, y, watermarkImage.Width, watermarkImage.Height);
            }

            return primaryImage;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool isDisposing)
        {
            if (isDisposing && !_disposed)
            {
                _watermarkImage.Dispose();
                _disposed = true;
            }
        }
    }
}