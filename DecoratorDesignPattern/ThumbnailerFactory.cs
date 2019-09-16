namespace DecoratorDesignPattern
{
    internal static class ThumbnailerFactory
    {
        public static ThumbnailerBase CreateInstance(MimeType mimeType, bool decorateThumbnail)
        {
            ThumbnailerBase thumbnailer = null;

            switch (mimeType)
            {
                case MimeType.Audio:
                    thumbnailer = new ThumbnailerAudio();
                    break;
                case MimeType.Video:
                    thumbnailer = new ThumbnailerVideo();
                    break;
                case MimeType.Image:
                    thumbnailer = new ThumbnailerImage();
                    break;
                default:
                    break;
            }

            if (decorateThumbnail)
            {
                return new ThumbnailerDecoratorWatermark(thumbnailer);
            }
            else
            {
                return thumbnailer;
            }
        }
    }
}