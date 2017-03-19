using System.Drawing;
using System.Drawing.Imaging;
using GLFWSharpie;

namespace TestApplication.Engine
{
    public class Texture
    {
        private readonly uint textureObject;

        public Texture(string filepath)
        {
            // Load Image
            Bitmap bitmap = new Bitmap(filepath);
            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            // Texture settings
            uint[] generatedTextures = new uint[1];
            Gl.GenTextures(1, generatedTextures);
            Gl.CheckForError();
            textureObject = generatedTextures[0];

            Gl.BindTexture(TextureTarget.Texture2D, textureObject);
            Gl.CheckForError();

            Gl.TexParameter((int)TextureTarget.Texture2D, (int)TextureParameter.TextureWrapS, (int)TextureParameterWrapMode.MirroredRepeat);
            Gl.CheckForError();

            Gl.TexParameter((int)TextureTarget.Texture2D, (int)TextureParameter.TextureWrapT, (int)TextureParameterWrapMode.MirroredRepeat);
            Gl.CheckForError();

            // TODO: Read up on wtf min + mag filters do, THEY MADE MY TEXTURE WORK!
            Gl.TexParameter((int)TextureTarget.Texture2D, (int)TextureParameter.TextureMinFilter, (float)TextureParameterMinifyingMode.Linear);
            Gl.CheckForError();

            Gl.TexParameter((int)TextureTarget.Texture2D, (int)TextureParameter.TextureMagFilter, (float)TextureParameterMinifyingMode.Linear);
            Gl.CheckForError();

            // TODO: Read up on pixel store. Texture wouldn't render without this setting
            Gl.PixelStore((int)PixelStoreParameter.UnpackAlignment, 1);
            Gl.CheckForError();

            // Upload texture data
            Gl.TexImage2D(TextureTarget.Texture2D, 0, InternalPixelFormat.RGB, (uint)bitmap.Width, (uint)bitmap.Height,
                0, GLFWSharpie.PixelFormat.RGB, PixelType.UnsignedByte, bitmapData.Scan0);
            Gl.CheckForError();

            // TODO. Wtf does mipmap do? Texture wouldn't show without it!
            Gl.BindTexture(TextureTarget.Texture2D, 0);
            Gl.CheckForError();

            bitmap.UnlockBits(bitmapData);
        }

        public void Activate()
        {
            Gl.ActiveTexture(ActiveTextureTarget.Texture0);
            Gl.CheckForError();

            Gl.BindTexture(TextureTarget.Texture2D, textureObject);
            Gl.CheckForError();
        }

        public void DeActivate()
        {
            Gl.BindTexture(TextureTarget.Texture2D, textureObject);
            Gl.CheckForError();
        }
    }
}