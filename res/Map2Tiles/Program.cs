using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Cryptography;
using System.Diagnostics;
using System.IO;

namespace Map2Tiles
{
	class Program
	{
		static void Main(string[] args)
		{
			MapToTiles(8);
		}

		static void MapToTiles(int t)
		{
			var md5 = MD5.Create();
			var converter = new ImageConverter();

			var img = Image.FromFile("map01.png");
			//var pcs = new Image[img.Size.Height / t, img.Size.Width / t];
			var tiles = new List<KeyValuePair<string, Image>>();
			Bitmap bmp;
			Graphics graphics;
			string key;

			for (var y = 0; y < img.Size.Height / t; y++)
			{
				for (var x = 0; x < img.Size.Width / t; x++)
				{
					bmp = new Bitmap(t, t);

					using (graphics = Graphics.FromImage(bmp))
					{
						graphics.DrawImage(img, new Rectangle(0, 0, t, t), new Rectangle(t * x, t * y, t, t), GraphicsUnit.Pixel);
					}

					key = BitConverter.ToString(md5.ComputeHash(converter.ConvertTo(bmp, typeof(byte[])) as byte[]));

					if (tiles.Any(p => p.Key == key))
					{
						continue;
					}

					tiles.Add(new KeyValuePair<string, Image>(key, bmp));
				}
			}

			var lines = (int)Math.Floor(tiles.Count / 32d);
			var tileImage = new Bitmap(32 * t, lines * t);
			var n = 0;
			graphics = Graphics.FromImage(tileImage);

			for (var y = 0; y < lines; y++)
			{
				for (var x = 0; x < 32 && n < tiles.Count; x++)
				{
					graphics.DrawImage(tiles[y * t + x].Value, new Rectangle(x * t, y * t, t, t), new Rectangle(0, 0, t, t), GraphicsUnit.Pixel);
				}
			}

			tileImage.Save("map01-tileset.png", ImageFormat.Png);

			using (var sw = new StreamWriter("map01-tileset.txt"))
			{
				foreach (var tile in tiles)
				{
					sw.WriteLine(tile.Key);
				}
			}
		}
		
		static void stand()
		{
			var src = Image.FromFile("stand2-1.png");
			var tgt = new Bitmap(720, 128);
			var g = Graphics.FromImage(tgt);
			var n = 0; var nx = 0; var ny = 0;

			for (var y = 0; y < 96; y++)
			{
				if (n > 1 && n % 8 == 0)
				{
					nx = 0;
					ny++;
				}
				
				for (var x = 0; x < 1; x++)
				{

					g.DrawImage(src, new Rectangle(nx * 32, ny * 32 + 0, 32, 8), new Rectangle(x * 32, y * 8, 32, 8), GraphicsUnit.Pixel);
					g.DrawImage(src, new Rectangle(nx * 32, ny * 32 + 8, 32, 8), new Rectangle(x * 32 + 64, y * 8, 32, 8), GraphicsUnit.Pixel);
					g.DrawImage(src, new Rectangle(nx * 32, ny * 32 + 16, 32, 8), new Rectangle(x * 32 + 32, y * 8, 32, 8), GraphicsUnit.Pixel);
					g.DrawImage(src, new Rectangle(nx * 32, ny * 32 + 24, 32, 8), new Rectangle(x * 32 + 96, y * 8, 32, 8), GraphicsUnit.Pixel);
				}
					n++;
					nx++;
			}

			tgt.Save("1-1.png", ImageFormat.Png);
		}
		
		static void horseman()
		{
			var src = Image.FromFile("1.png");
			var tgt = new Bitmap(256, 128);
			var g = Graphics.FromImage(tgt);
			var n = 0; var nx = 0; var ny = 0;

			for (var y = 0; y < 96; y++)
			{
					if (n > 1 && n % 8 == 0)
					{
						nx = 0;
						ny++;
					}
				for (var x = 0; x < 1; x++)
				{

					g.DrawImage(src, new Rectangle(nx * 32, ny * 32 + 0, 32, 8), new Rectangle(x * 32, y * 8, 32, 8), GraphicsUnit.Pixel);
					g.DrawImage(src, new Rectangle(nx * 32, ny * 32 + 8, 32, 8), new Rectangle(x * 32 + 64, y * 8, 32, 8), GraphicsUnit.Pixel);
					g.DrawImage(src, new Rectangle(nx * 32, ny * 32 + 16, 32, 8), new Rectangle(x * 32 + 32, y * 8, 32, 8), GraphicsUnit.Pixel);
					g.DrawImage(src, new Rectangle(nx * 32, ny * 32 + 24, 32, 8), new Rectangle(x * 32 + 96, y * 8, 32, 8), GraphicsUnit.Pixel);
				}
					n++;
					nx++;
			}

			tgt.Save("1-1.png", ImageFormat.Png);
		}
		
		
		static void flyingman()
		{
			var src = Image.FromFile("2.png");
			var tgt = new Bitmap(256, 96);
			var g = Graphics.FromImage(tgt);
			var n = 0; var nx = 0; var ny = 0;

			for (var y = 0; y < 16; y++)
			{
				for (var x = 0; x < 1; x++)
				{
					if (n > 1 && n % 8 == 0)
					{
						nx = 0;
						ny++;
					}

					g.DrawImage(src, new Rectangle(nx * 32, ny * 48 + 8, 32, 8), new Rectangle(x * 32, y * 8, 32, 8), GraphicsUnit.Pixel);
					g.DrawImage(src, new Rectangle(nx * 32, ny * 48 + 16, 32, 8), new Rectangle(x * 32 + 64, y * 8, 32, 8), GraphicsUnit.Pixel);
					g.DrawImage(src, new Rectangle(nx * 32, ny * 48 + 24, 32, 8), new Rectangle(x * 32 + 32, y * 8, 32, 8), GraphicsUnit.Pixel);
					g.DrawImage(src, new Rectangle(nx * 32, ny * 48 + 32, 32, 8), new Rectangle(x * 32 + 96, y * 8, 32, 8), GraphicsUnit.Pixel);
					n++;
					nx++;
				}
			}

			tgt.Save("2-1.png", ImageFormat.Png);
		}
		
		static void temp()
		{
			var src = Image.FromFile("stand3.png");
			var tgt = new Bitmap(768, 120);
			var g = Graphics.FromImage(tgt);
			var n = 11; var nx = n * 48; var ny = 0;

			for (var y = 0; y < 15; y++)
			{
				for (var x = 0; x < 5; x++)
				{
					if (n > 1 && n % 16 == 0)
					{
						nx = 0;
						ny++;
					}

					g.DrawImage(src, new Rectangle(nx * 48, ny * 24 + 4, 48, 24), new Rectangle(x * 48, y * 24, 48, 24), GraphicsUnit.Pixel);
					n++;
					nx++;
				}
			}

			tgt.Save("stand3-1.png", ImageFormat.Png);
		}
	}
}
