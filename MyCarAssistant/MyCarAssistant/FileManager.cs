using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;

using MyCarAssistant.Models;

namespace MyCarAssistant
{
    // Данный класс предназначен для работы с файлами, которые будут созданы в процессе использования приложения
    public class FileManager
    {
        // Путь к папке, в которой будут храниться необходимые файлы
        static string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData); 

        // Получение экземпляра пользователя
        public static User GetUserFromFile()
        {
            // загружем текст из файла
            string s = File.ReadAllText(Path.Combine(folderPath, "UserInfo"));
            // Десериализуем полученный объект 
            User user = JsonSerializer.Deserialize<User>(s);

            return user;
        }

        // Получение списка автомобилей из файла
        public static List<Auto> GetAutosFromFile()
        {
            // загружем текст из файла
            string s = File.ReadAllText(Path.Combine(folderPath, "AutosInfo"));
            // Десериализуем полученный объект 
            List<Auto> autos = (List<Auto>)JsonSerializer.Deserialize<IEnumerable<Auto>>(s);

            return autos;
        }

        // сохранение пользователя в файл
        public static void SaveUser(User user)
        {
            // Обозначение имени файла, хранящего информацию о пользователе
            string filename = "UserInfo";
            
            // Сериализация экземпляра
            string str = JsonSerializer.Serialize(user);

            // Перезапись файла
            File.WriteAllText(Path.Combine(folderPath, filename), str);
        }

        public static void SaveAutos(List<Auto> listAutos)
        {
            // Обозначение имени файла, хранящего информацию об автомобилях
            string filename = "AutosInfo";

            // Сериализация списка автомобилей
            string str = JsonSerializer.Serialize(listAutos);

            // Перезапись файла
            File.WriteAllText(Path.Combine(folderPath, filename), str);
        }

        // Имеется ли сохранённый пользователь
        public static bool IsUserAvailable()
        {
            // Обозначение имени файла, хранящего информацию о пользователе
            string filename = "UserInfo";
            
            // Имеется файл - имеется пользователь
            if (File.Exists(Path.Combine(folderPath, filename)))
                return true;
            return false;
        }

        // Имеются ли сохранённые автомобили
        public static bool IsAutoAvailable()
        {
            // Обозначение имени файла, хранящего информацию об автомобилях
            string filename = "AutosInfo";
            
            // Имеется файл - имеются автомобили
            if (File.Exists(Path.Combine(folderPath, filename)))
                return true;
            return false;
        }

        // Удаление с устройства пользователя и данных принадлежащих ему автомобилей
        public static void DeleteUser()
        {
            // удаляем файл с инфомрацией о пользователе
            File.Delete(Path.Combine(folderPath, "UserInfo"));
            // удаляем файл с информацией об автомобилях
            File.Delete(Path.Combine(folderPath, "AutosInfo"));
        }

        public static bool IsPhotoExist(string filename)
        {
            var path = Path.Combine(FileSystem.AppDataDirectory, filename);
            bool result = File.Exists(path);
            return result;
        }
    }
}
