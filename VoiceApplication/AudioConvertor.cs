using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoiceApplication
{
    class AudioConvertor
    {
        private AudioConvertor() 
        {
        }

        /// <summary>
        /// 将MP3格式byte[]转为 WAV 格式byte[]
        /// </summary>
        /// <param name="inputStream"></param>
        /// <returns></returns>
        public static byte[] ConvertMp3ToWav(byte[] inputStream)
        {
            using var memoryStream = new MemoryStream(inputStream);
            using var mp3FileReader = new Mp3FileReader(memoryStream);
            using var pcmStream = WaveFormatConversionStream.CreatePcmStream(mp3FileReader);
            using var outputMemoryStream = new MemoryStream();
            WaveFileWriter.WriteWavFileToStream(outputMemoryStream, pcmStream);
            return outputMemoryStream.ToArray();
        }

        /// <summary>
        /// 将MP3格式byte[]转为 PCM 格式byte[]
        /// </summary>
        /// <param name="inputStream"></param>
        /// <returns></returns>
        public static byte[] ConvertMp3ToPcm(byte[] inputStream)
        {
            using var memoryStream = new MemoryStream(inputStream);
            using var mp3FileReader = new Mp3FileReader(memoryStream);
            using var pcmStream = WaveFormatConversionStream.CreatePcmStream(mp3FileReader);
            using var outputMemoryStream = new MemoryStream();
            {
                byte[] buffer = new byte[pcmStream.WaveFormat.AverageBytesPerSecond];
                int bytesRead;
                while ((bytesRead = pcmStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    outputMemoryStream.Write(buffer, 0, bytesRead);
                }
            }
            return outputMemoryStream.ToArray();
        }

    }
}
