using System;
using System.Text;


public struct Util
{
	private const string SPACING = ", ";

	public const string WAV = "*.wav";
	public const string WAV_MP3 = "*.wav|*.mp3";


	public static bool isMP3(string path)
	{
		return path.EndsWith(".mp3");
	}


	public static string GetTimeRange(float from, float to)
	{
		return string.Format("{0} - {1}", GetFormattedTime(from), GetFormattedTime(to));
	}


	public static string GetFormattedTime(float seconds)
	{
		TimeSpan time = TimeSpan.FromSeconds(seconds);
		return string.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds);
	}


	public static string GetFileDescription(string path)
	{
		TagLib.Tag tags = TagLib.File.Create(path).Tag;

		StringBuilder sb = new StringBuilder();
		string artists = string.Join("", tags.AlbumArtists);
		if (MoreThanTwo(artists)) {
			sb.Append(artists);
		}

		string performers = string.Join("", tags.Performers);
		if (MoreThanTwo(performers)) {
			sb.Append(sb.Length > 0 ? SPACING : string.Empty);
			sb.Append(performers);
		}

		if (MoreThanTwo(tags.Title)) {
			sb.Append(sb.Length > 0 ? SPACING : string.Empty);
			sb.Append(tags.Title);
		}

		if (MoreThanTwo(tags.Year.ToString())) {
			sb.Append(sb.Length > 0 ? SPACING : string.Empty);
			sb.Append(tags.Year);
		}

		return sb.Length > 0 ? sb.ToString() : string.Empty;
	}


	private static bool MoreThanTwo(string value)
	{
		return value != null && value.Length > 2;
	}

}
