using NAudio.Wave;
using System.IO;

public static class AudioConverter
{
    public static string ConvertMp3ToWav(string mp3FilePath, string outputDirectory)
    {
        string wavFilePath = Path.Combine(outputDirectory, Path.GetFileNameWithoutExtension(mp3FilePath) + ".wav");

        using (Mp3FileReader mp3Reader = new Mp3FileReader(mp3FilePath))
        using (WaveStream pcmStream = WaveFormatConversionStream.CreatePcmStream(mp3Reader))
        using (WaveFileWriter waveWriter = new WaveFileWriter(wavFilePath, pcmStream.WaveFormat))
        {
            pcmStream.CopyTo(waveWriter);
        }

        return wavFilePath;
    }
}
