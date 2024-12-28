using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using SkiaSharp;

namespace ImageProcessing
{
	public class TextOverlay
	{
		public string Text { get; set; }
		public int X { get; set; }
		public int Y { get; set; }
		public string FontPath { get; set; }
		public float FontSize { get; set; }
		public string TextColor { get; set; }
		public float MaxWidth { get; set; } 

		public TextOverlay()
		{
			Text = string.Empty;
			X = 0;
			Y = 0;
			FontPath = string.Empty;
			FontSize = 24;
			TextColor = "#000000";
			MaxWidth = 100;
		}

		public TextOverlay(string text, int x, int y)
		{
			Text = text;
			X = x;
			Y = y;
			FontPath = string.Empty;
			FontSize = 24;
			TextColor = "#000000";
			MaxWidth = 100;
		}

		public TextOverlay(string text, int x, int y, string fontPath, float fontSize, string textColor, float maxWidth = 0)
		{
			Text = text;
			X = x;
			Y = y;
			FontPath = fontPath;
			FontSize = fontSize;
			TextColor = textColor;
			MaxWidth = maxWidth;
		}
	}

	public class ResultImageProcessing
	{
		public int StatusCode { get; set; }
		public string Message { get; set; } = string.Empty;
	}

	public class ProcessingAddTextsToImage
	{
		public string ImageSrcPath { get; set; }
		public string ImageDestPath { get; set; }
		public List<TextOverlay> Texts { get; set; }

		public ProcessingAddTextsToImage()
		{
			ImageSrcPath = string.Empty;
			ImageDestPath = string.Empty;
			Texts = new List<TextOverlay>();
		}

		public ProcessingAddTextsToImage(string imageSrcPath, string imageDestPath, List<TextOverlay> texts)
		{
			ImageSrcPath = imageSrcPath;
			ImageDestPath = imageDestPath;
			Texts = texts;
		}

		public ResultImageProcessing AddTextsToImage()
		{
			var result = new ResultImageProcessing { StatusCode = 200 };

			try
			{
				if (!File.Exists(ImageSrcPath))
				{
					result.StatusCode = 404;
					result.Message = "Source image file not found.";
					return result;
				}

				var outputDir = Path.GetDirectoryName(ImageDestPath);
				if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
				{
					Directory.CreateDirectory(outputDir);
				}

				using (var inputStream = File.OpenRead(ImageSrcPath))
				using (var bitmap = SKBitmap.Decode(inputStream))
				using (var canvas = new SKCanvas(bitmap))
				{
					foreach (var text in Texts)
					{
						SKColor color;
						if (text.TextColor.StartsWith("#"))
						{
							color = SKColor.Parse(text.TextColor);
						}
						else if (text.TextColor.Contains(","))
						{
							var parts = text.TextColor.Split(',');
							if (parts.Length == 3)
							{
								color = new SKColor(byte.Parse(parts[0]), byte.Parse(parts[1]), byte.Parse(parts[2]));
							}
							else if (parts.Length == 4)
							{
								color = new SKColor(byte.Parse(parts[0]), byte.Parse(parts[1]), byte.Parse(parts[2]), byte.Parse(parts[3]));
							}
							else
							{
								result.Message = "Invalid color format.";
								return result;
							}
						}
						else
						{
							result.Message = "Invalid color format.";
							return result;
						}

						using (var paint = new SKPaint())
						{
							paint.Color = color;
							paint.IsAntialias = true;
							paint.TextSize = text.FontSize;

							if (!string.IsNullOrEmpty(text.FontPath) && File.Exists(text.FontPath))
							{
								var typeface = SKTypeface.FromFile(text.FontPath);
								paint.Typeface = typeface;
							}
							else
							{
								paint.Typeface = SKTypeface.Default;
							}

							var lines = WrapText(text.Text, paint, text.MaxWidth);
							var lineHeight = paint.FontMetrics.CapHeight;
                            var lineSpacing = lineHeight / 3;

                            for (int i = 0; i < lines.Count; i++)
							{
								var line = lines[i];
								var textWidth = paint.MeasureText(line);
								var startX = text.X - (textWidth / 2);
                                var startY = text.Y + (i * (lineHeight + lineSpacing));
                                canvas.DrawText(line, startX, startY, paint);
							}
						}
					}

					using (var outputStream = File.OpenWrite(ImageDestPath))
					{
						bitmap.Encode(outputStream, SKEncodedImageFormat.Png, 100);
					}
				}

				result.Message = "Image processed successfully.";
			}
			catch (Exception ex)
			{
				result.StatusCode = 500;
				result.Message = $"An error occurred: {ex.Message}";
			}

			return result;
		}

		private List<string> WrapText(string text, SKPaint paint, float maxWidth)
		{
			var lines = new List<string>();
			if (maxWidth <= 0)
			{
				lines.Add(text);
				return lines;
			}

			var words = text.Split(' ');
			var currentLine = string.Empty;

			foreach (var word in words)
			{
				var testLine = string.IsNullOrEmpty(currentLine) ? word : currentLine + " " + word;
				var testWidth = paint.MeasureText(testLine);

				if (testWidth > maxWidth)
				{
					if (!string.IsNullOrEmpty(currentLine))
					{
						lines.Add(currentLine);
					}
					currentLine = word;
				}
				else
				{
					currentLine = testLine;
				}
			}

			if (!string.IsNullOrEmpty(currentLine))
			{
				lines.Add(currentLine);
			}

			return lines;
		}
	}
}