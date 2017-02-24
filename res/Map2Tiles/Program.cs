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
		class Tile
		{
			public string Key { get; set; }
			public int Index { get; set; }
			public Point Position { get; set; }
			public Bitmap Image { get; set; }
		}

		static void Main(string[] args)
		{
			MapToTiles(8);
		}

		static void MapToTiles(int t)
		{
			var md5 = SHA1.Create();
			var converter = new ImageConverter();

			var img = Image.FromFile("map01.png");
			var tiles = new List<Tile>();
			Bitmap bmp;
			Graphics graphics;
			string key;
			byte[] bytes;
			Point position;
			int n = 0;

			using (var sw = new StreamWriter("map01-tileset.txt"))
			{
				for (var y = 0; y < img.Size.Height / t; y++)
				{
					for (var x = 0; x < img.Size.Width / t; x++)
					{
						bmp = new Bitmap(t, t);

						using (graphics = Graphics.FromImage(bmp))
						{
							graphics.DrawImage(img, new Rectangle(0, 0, t, t), new Rectangle(t * x, t * y, t, t), GraphicsUnit.Pixel);
						}

						bytes = converter.ConvertTo(bmp, typeof(byte[])) as byte[];
						key = BitConverter.ToString(md5.ComputeHash(bytes.ToArray()));

						if (tiles.Any(p => p.Key == key))
						{
							continue;
						}

						position = new Point(x * t, y * t);
						tiles.Add(new Tile { Key = key, Position = position, Image = bmp });

						sw.Write(string.Format("0x{0:x4},", n));

						n++;
					}
				}
			}

			var lines = (int)Math.Floor(tiles.Count / 32d);
			var tileImage = new Bitmap(t * 32, t * lines);
			graphics = Graphics.FromImage(tileImage);

			for (var y = 0; y < lines; y++)
			{
				for (var x = 0; x < 32; x++)
				{
					graphics.DrawImage(tiles[y * 32 + x].Image, new Rectangle(x * t, y * t, t, t), new Rectangle(0, 0, t, t), GraphicsUnit.Pixel);
				}
			}

			tileImage.Save("map01-tileset.png", ImageFormat.Png);
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
