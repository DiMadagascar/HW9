using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Example_911
{
    class Program
    {
        static void Main(string[] args)
        {
            // Работа с сетью

            string url = @"audio_api.mp3";
            WebClient webClient = new WebClient() { Encoding = Encoding.UTF8 };
            webClient.BaseAddress = "https://ru.sefon.pro/mp3/";

            #region Скачивание mp3
            // "http://ksergey.ru/sb/audio_api.mp3";
            webClient.DownloadFile(@"https://sber-zvuk.com/top100", "апа.mp3");
            Console.WriteLine("Готово.");

            Console.ReadLine();

            #endregion


        }
    }
}
