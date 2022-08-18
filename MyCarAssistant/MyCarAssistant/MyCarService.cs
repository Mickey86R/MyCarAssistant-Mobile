using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using MyCarAssistant.Models;

namespace MyCarAssistant
{
    // Данный класс предназначен для работы с API
    public class MyCarService
    {
        const string Url = "http://elegant-newton.37-140-192-136.plesk.page/MyCar_API/"; // обращайте внимание на конечный слеш

        // настройки для десериализации для нечувствительности к регистру символов
        static JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };

        // настройка клиента
        private static HttpClient GetHttpClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        //-------------------------------------ПОЛУЧЕНИЕ

        // Получение пользователя из БД
        public async static Task<User> GetUserFromDB(string login, string password)
        {
            // Создание Http клиента
            HttpClient client = new HttpClient();
            // Формирование запроса к API
            var response = await client.GetAsync(Url + "IsUserInDB/" + login + ";" + password);

            // Получение и десериализация списка автомобилей
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseResult = JsonSerializer.Deserialize<User>(responseContent, options);

            return responseResult;
        }

        // Подтверждение существования пользователя в БД
        public static bool IsUserInDB(string login)
        {
            // Создание клиента
            HttpClient client = new HttpClient();
            // Формирование запроса к API
            var result = client.GetAsync(Url + "IsUserAvailable/" + login);

            // Ожидание получения результата
            while (!result.IsCompleted) ;

            // Т.к. API вернёт булево значение, то ответ десериализуется и возвращается пользователю
            bool available = JsonSerializer.Deserialize<bool>(result.Result.Content.ReadAsStringAsync().Result, options);

            return available;
        }

        // Получение списка автомобилей, принадлежащих пользователю по его ID
        public async static Task<List<Auto>> GetAutosFromUserID(int userID)
        {
            // Создание клиента
            HttpClient client = GetHttpClient();
            // Формирование запроса
            var response = await client.GetAsync(Url + "GetAutoFromUser/" + userID);

            // Получение и десериализация списка автомобилей
            var responseContent = await response.Content.ReadAsStringAsync();
            var responseResult = JsonSerializer.Deserialize<List<Auto>>(responseContent, options);

            return responseResult;
        }

        //-------------------------------------РЕГИСТРАЦИЯ

        // Добавление нового пользователя
        public static async Task<User> RegistrationNewUser(User user)
        {
            // Создание клиента
            HttpClient client = GetHttpClient();
            // Формирование запроса к API
            var response = client.PostAsync(Url + "PostUser/",
                new StringContent(
                    JsonSerializer.Serialize(user, options),
                    Encoding.UTF8, "application/json"));

            var responseContent = await response.Result.Content.ReadAsStringAsync();
            var responseResult = JsonSerializer.Deserialize<User>(responseContent, options);

            // Возврат десериализованного экземпляра пользователя
            return responseResult;
        }

        // Регистрация нового автомобиля, принадлежащего конкретному пользователю
        public static async Task<Auto> RegistrationNewAuto(Auto auto)
        {
            // Создание клиента
            HttpClient client = GetHttpClient();
            // Формирование запроса к API
            var response = client.PostAsync(
                Url + "PostAuto/",
                new StringContent(
                    JsonSerializer.Serialize(auto, options),
                    Encoding.UTF8, "application/json"));

            // Десериализация полученного ответа в экземпляр автомобиля
            var responseContent = await response.Result.Content.ReadAsStringAsync();
            var responseResult = JsonSerializer.Deserialize<Auto>(responseContent, options);

            return responseResult;
        }

        public static async Task CreateRelation(int autoID, int userID)
        {
            // Создание клиента
            HttpClient client = GetHttpClient();

            // Запрос на создание связи автомобиля и пользователя
            client = GetHttpClient();
            var response = client.PostAsync(Url + $"CreateRelation/{autoID};{userID}",
                new StringContent(
                    JsonSerializer.Serialize("", options),
                    Encoding.UTF8, "application/json"));

            // Десериализация полученного ответа в экземпляр автомобиля
            await response.Result.Content.ReadAsStringAsync();
        }

        // Регистрация новой затраты
        public static async Task<Expens> RegistrationNewExpens(Expens expens)
        {
            var client = new HttpClient();

            var response = client.PostAsync(
                Url + "PostExpens/",
                new StringContent(
                    JsonSerializer.Serialize(expens, options),
                    Encoding.UTF8,
                    "application/json"));

            var responseContent = await response.Result.Content.ReadAsStringAsync();
            var responseResult = JsonSerializer.Deserialize<Expens>(responseContent, options);

            return responseResult;
        }

        // Регистрация новой затраты
        public static async Task<T_O> RegistrationNewTO(T_O TO)
        {
            var client = new HttpClient();

            var response = client.PostAsync(
                Url + "PostTO/",
                new StringContent(
                    JsonSerializer.Serialize(TO, options),
                    Encoding.UTF8,
                    "application/json"));

            var responseContent = await response.Result.Content.ReadAsStringAsync();
            var responseResult = JsonSerializer.Deserialize<T_O>(responseContent, options);

            return responseResult;
        }

        // Регистрация новой записи
        public static async Task<Record> RegistrationNewRecord(Record record)
        {
            var client = new HttpClient();

            var response = client.PostAsync(
                Url + "PostRecord/",
                new StringContent(
                    JsonSerializer.Serialize(record, options),
                    Encoding.UTF8,
                    "application/json"));

            var responseContent = await response.Result.Content.ReadAsStringAsync();
            var responseResult = JsonSerializer.Deserialize<Record>(responseContent, options);

            return responseResult;
        }

        //-------------------------------------РЕДАКТИРОВАНИЕ

        // Редактирование параметров пользователя
        public static async Task<User> UpdateUser(User user)
        {
            // Создание клиента
            HttpClient client = GetHttpClient();
            // Формирование запроса
            var response = client.PostAsync(
                Url + "PostUser/",
                new StringContent(
                    JsonSerializer.Serialize(user, options),
                    Encoding.UTF8, "application/json"));

            var responseContent = await response.Result.Content.ReadAsStringAsync();
            var responseResult = JsonSerializer.Deserialize<User>(responseContent, options);

            // Возврат десериализованного экземпляра пользователя
            return responseResult;
        }

        // Обновление параметров автомобиля
        public static async Task<Auto> UpdateAuto(Auto auto)
        {
            HttpClient client = GetHttpClient();
            var response = await client.PostAsync(Url + "PostAuto/",
                new StringContent(
                    JsonSerializer.Serialize(auto, options),
                    Encoding.UTF8, "application/json"));

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseResult = JsonSerializer.Deserialize<Auto>(responseContent, options);

            // Возврат десериализованного экземпляра автомобиля
            return responseResult;
        }

        //-------------------------------------УДАЛЕНИЕ

        // Удаление пользователя по его ID
        public async Task<User> DeleteUser(int id)
        {
            // Создание клиента
            HttpClient client = GetHttpClient();
            // Формирование запроса
            var response = client.DeleteAsync(Url + $"DelUser/{id}");

            var responseContent = await response.Result.Content.ReadAsStringAsync();
            var responseResult = JsonSerializer.Deserialize<User>(responseContent, options);

            // Возврат десериализованного экземпляра автомобиля
            return responseResult;
        }

        // Удаление авто по его ID
        public static async Task<Auto> DeleteAuto(int id)
        {
            // Создание клиента
            HttpClient client = GetHttpClient();
            // Формирование запроса
            var response = await client.GetAsync(Url + $"DeleteAuto/{id}");

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseResult = JsonSerializer.Deserialize<Auto>(responseContent, options);

            // Возврат десериализованного экземпляра автомобиля
            return responseResult;
        }

        // Удаление ТО по его ID
        public static async Task<T_O> DeleteTO(int id)
        {
            // Создание клиента
            HttpClient client = GetHttpClient();
            // Формирование запроса
            var response = client.GetAsync(Url + $"DeleteTO/{id}");

            var responseContent = await response.Result.Content.ReadAsStringAsync();
            var responseResult = JsonSerializer.Deserialize<T_O>(responseContent, options);

            // Возврат десериализованного экземпляра ТО
            return responseResult;
        }

        // Удаление затраты по её ID
        public static async Task<Expens> DeleteExpens(int id)
        {
            // Создание клиента
            HttpClient client = GetHttpClient();
            // Формирование запроса
            var response = client.GetAsync(Url + $"DeleteExpens/{id}");

            var responseContent = await response.Result.Content.ReadAsStringAsync();
            var responseResult = JsonSerializer.Deserialize<Expens>(responseContent, options);

            // Возврат десериализованного экземпляра затраты
            return responseResult;
        }

        // Удаление записи по её ID
        public static async Task<Record> DeleteRecord(int id)
        {
            // Создание клиента
            HttpClient client = GetHttpClient();
            // Формирование запроса
            var response = client.GetAsync(Url + $"DeleteRecord/{id}");

            var responseContent = await response.Result.Content.ReadAsStringAsync();
            var responseResult = JsonSerializer.Deserialize<Record>(responseContent, options);

            // Возврат десериализованного экземпляра записи
            return responseResult;
        }
    }
}



