using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;
    


namespace Example_941
{
    class Program
    {
        private static string token = File.ReadAllText(@"D:\Обучение Скил\С Шарп\Исходники модуль 9\Theme_09\Example_931\Example_941\token.txt");
        //считываем токен с файла 
        private static TelegramBotClient client;
        private static string d_path = @"D:\Обучение Скил\С Шарп\Исходники модуль 9\Theme_09\Example_931\Example_941\bin\Debug\downloads";
        static void Main(string[] args)
        {
            client = new TelegramBotClient(token);
            var ne = client.GetMeAsync();
            
            client.StartReceiving();//запускаем обработчик событий 
            client.OnMessage += OnMessageHandler;//при получении нового сообщения запускаем метод обработки
            Console.ReadLine();
            client.StopReceiving();
            
            

        }

        private static async void OnMessageHandler(object sender, MessageEventArgs e)
        {
            string[] files;
                     

            
            //путь для загрузок
            var msg = e.Message;
            /*Если файл формата фото, документа или аудио, загружаем его в ранее указанный путь*/
            if (msg.Type == Telegram.Bot.Types.Enums.MessageType.Document)
            {
                DownLoad(msg.Document.FileId, msg.Document.FileName);
            }
            else if (msg.Type == Telegram.Bot.Types.Enums.MessageType.Photo)
            
            {
                DateTime datename = DateTime.Now;
               String sdate= datename.ToString("dd.MM.yyyy_hh.mm.ss");
                DownLoad(msg.Photo[msg.Photo.Length - 1].FileId, sdate + @".jpg");//имена файлов формируем по текущей дате
            }
            else if(msg.Type == Telegram.Bot.Types.Enums.MessageType.Audio)
            {
                DownLoad(msg.Audio.FileId, msg.Audio.Title+@".mp3"); ;
            }
            else
            {
                switch (msg.Text)
                {
                    case "/start":
                        /*Даем пользователю меню из 2-х пунктов для просмотра файлов, имещихся в каталоге загрузок 
                         * и скачивания необходимого файла*/
                        await client.SendTextMessageAsync(msg.Chat.Id, "Здравствуйте! Выберите необходимое действие:", 
                            replyMarkup: MainMenu());
                        break;
                    case "Открыть список загруженных файлов":
                        files = Directory.GetFiles(d_path);
                        foreach (string s in files)
                        {
                            await client.SendTextMessageAsync(msg.Chat.Id, Path.GetFileName(s));
                        }
                        break;
                    case "Скачать файл":
                        await client.SendTextMessageAsync(msg.Chat.Id, "Введите точное название файла");
                        
                        

                        break;
                    default:
                        string pathfile = d_path + @"\" + msg.Text;
                        FileInfo fileInf = new FileInfo(pathfile);
                        if (fileInf.Exists)
                        {
                            using (var fileStream = new FileStream(d_path + @"\" + msg.Text, FileMode.Open, FileAccess.Read, FileShare.Read))
                            {
                                await client.SendDocumentAsync(
                                    msg.Chat.Id, fileStream);
                            }
                        }
                        else
                        {
                            await client.SendTextMessageAsync(msg.Chat.Id, "Не найдено такой команды или файлы");
                        }
                        break;
                }
            } 
        }
        private static  IReplyMarkup MainMenu()//меню 
        {
            return new ReplyKeyboardMarkup
            {
                Keyboard = new List<List<KeyboardButton>>
               {
                   new List<KeyboardButton>{new KeyboardButton { Text="Открыть список загруженных файлов"},
                       new KeyboardButton {Text="Скачать файл" } }
               }
            };
            

                
        }
        static async void DownLoad(string fileId, string path)//метод для загрузки файлов
        {
            var file = await client.GetFileAsync(fileId);
            
            FileStream fs = new FileStream(d_path + "/"+path, FileMode.Create);
            await client.DownloadFileAsync(file.FilePath, fs);
            fs.Close();

            fs.Dispose();
        }

    }
}
