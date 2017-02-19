<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Drawing.dll</Reference>
  <Namespace>System.Diagnostics</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Drawing.Imaging</Namespace>
  <Namespace>System.IO</Namespace>
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

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
			var src = Image.FromFile(@"C:\Users\Eric\Documents\GitHub\fe-test\res\Map2Tiles\bin\Debug\stand3.png");
			var tmp = new Bitmap(16 * 8 * 39, 8);
			var tgt = new Bitmap(16 * 15, 24 * 7);
			var g = Graphics.FromImage(tmp);
			var n = 0; var nx = 0; var ny = 0;

			for (var y = 0; y < 39; y++)
			{
				for (var x = 0; x < 8; x++)
				{
					g.DrawImage(src, new Rectangle(n * 16, 0, 16, 8), new Rectangle(x * 16, y * 8, 16, 8), GraphicsUnit.Pixel);
					n++;
					nx++;
				}
			}
			
			tmp.Save(@"C:\Users\Eric\Documents\GitHub\fe-test\res\Map2Tiles\bin\Debug\stand3-1.png", ImageFormat.Png);
			g = Graphics.FromImage(tgt);
			n = nx = ny = 0;
			
			for (var i = 0; i < 8 * 39; i++)
			{
				if (n > 0 && n % 15 == 0)
				{
					nx = 0;
					ny++;
				}
							
				g.DrawImage(tmp, new Rectangle(nx * 16, ny * 24, 16, 8), new Rectangle(i * 48 + 32, 0, 16, 8), GraphicsUnit.Pixel);
				g.DrawImage(tmp, new Rectangle(nx * 16, ny * 24 + 8, 16, 8), new Rectangle(i * 48, 0, 16, 8), GraphicsUnit.Pixel);
				g.DrawImage(tmp, new Rectangle(nx * 16, ny * 24 + 16, 16, 8), new Rectangle(i * 48 + 16, 0, 16, 8), GraphicsUnit.Pixel);
				
				n++;
				nx++;
			}

			tgt.Save(@"C:\Users\Eric\Documents\GitHub\fe-test\res\Map2Tiles\bin\Debug\stand3-2.png", ImageFormat.Png);
		}