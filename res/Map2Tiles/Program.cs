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
			dosomething();
		}

		static void MapToTiles()
		{
			var md5 = MD5.Create();
			var converter = new ImageConverter();

			var img = Image.FromFile("map01.png");
			var tileWidth = 16;
			var tileHeight = 16;
			var pcs = new Image[img.Size.Height / tileHeight, img.Size.Width / tileWidth];
			var tiles = new List<KeyValuePair<string, Image>>();
			Graphics graphics;
			string key;

			for (var x = 0; x < pcs.GetLength(0); x++)
			{
				for (var y = 0; y < pcs.GetLength(1); y++)
				{
					pcs[x, y] = new Bitmap(tileWidth, tileHeight);

					using (graphics = Graphics.FromImage(pcs[x, y]))
					{
						graphics.DrawImage(img, new Rectangle(0, 0, tileWidth, tileHeight), new Rectangle(16 * x, 16 * y, 16, 16), GraphicsUnit.Pixel);
					}

					key = BitConverter.ToString(md5.ComputeHash(converter.ConvertTo(pcs[x, y], typeof(byte[])) as byte[]));

					if (tiles.Any(p => p.Key == key))
					{
						continue;
					}

					tiles.Add(new KeyValuePair<string, Image>(key, pcs[x, y]));
				}
			}

			var lines = (int)Math.Floor(tiles.Count / 16d);
			var tileImage = new Bitmap(16 * 16, lines * 16);
			var n = 0;
			graphics = Graphics.FromImage(tileImage);

			for (var y = 0; y < lines; y++)
			{
				for (var x = 0; x < 16 && n < tiles.Count; x++)
				{
					graphics.DrawImage(tiles[y * 16 + x].Value, new Rectangle(x * 16, y * 16, tileWidth, tileHeight), new Rectangle(0, 0, tileWidth, tileWidth), GraphicsUnit.Pixel);
				}
			}

			tileImage.Save("map01-tileset.png", ImageFormat.Png);

			using (var sw = new StreamWriter("map01-tileset.txt"))
			{
				foreach (var t in tiles)
				{
					sw.WriteLine(t.Key);
				}
			}
		}

		static void dosomething()
		{
			var src = Image.FromFile("1.png");
			var tgt = new Bitmap(256, 64);
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

					g.DrawImage(src, new Rectangle(nx * 32, ny * 32, 32, 8), new Rectangle(x * 32, y * 8, 32, 8), GraphicsUnit.Pixel);
					g.DrawImage(src, new Rectangle(nx * 32, ny * 32 + 8, 32, 8), new Rectangle(x * 32 + 32, y * 8, 32, 8), GraphicsUnit.Pixel);
					g.DrawImage(src, new Rectangle(nx * 32, ny * 32 + 16, 32, 8), new Rectangle(x * 32 + 64, y * 8, 32, 8), GraphicsUnit.Pixel);
					g.DrawImage(src, new Rectangle(nx * 32, ny * 32 + 24, 32, 8), new Rectangle(x * 32 + 96, y * 8, 32, 8), GraphicsUnit.Pixel);
					n++;
					nx++;
				}
			}

			tgt.Save("1-1.png", ImageFormat.Png);
		}
	}
}
