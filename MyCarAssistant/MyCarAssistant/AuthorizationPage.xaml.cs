using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MyCarAssistant.Models;

namespace MyCarAssistant
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthorizationPage : ContentPage
    {
        MainPage motherPage; // ссылка на главную страницу для передачи данных

        public AuthorizationPage()
        {
            InitializeComponent();
        }
        public AuthorizationPage(MainPage page) // Конструктор со ссылкой на главную страницу 
        {
            InitializeComponent();

            motherPage = page;
        }


        private async void AuthorizationButton_Clicked(object sender, EventArgs e) // Авторизация пользователя
        {
            // считывание логина и пароля
            string login = loginEntry.Text;
            string password = passwordEntry.Text.GetHashCode().ToString();

            // Получение пользователя из БД по логину и паролю
            var user = await MyCarService.GetUserFromDB(login, password);

            // полученный ответ может быть либо null, либо может иметь поля null.
            // Это сделано для того, чтобы дать корректный ответ пользователю -
            // зарегистрирвоан пользователь с таким логином в системе или же необходимо пройти регистрацию
            if (user != null)
            {
                if (user.Login != null)
                {
                    FileManager.SaveUser(user);
                    await Navigation.PopModalAsync();
                }
                else
                    await DisplayAlert("Ошибка авторизации", "Введён не верный пароль", "Ок");
            }
            else
                await DisplayAlert("Ошибка авторизации", "Пользователь не существует", "Ок");
        }

        private async void RegistrationButton_Clicked(object sender, EventArgs e) // Регистрация пользователя
        {
            // считывание логина и пароля
            string login = loginEntry.Text;
            string password = passwordEntry.Text.GetHashCode().ToString();

            // Если пользователя с таким логином не существует,
            // то происходит создание нового пользователя и регистрация его в БД
            if (!MyCarService.IsUserInDB(login))
            {
                // Создание пользователя
                User user = new User { Login = login, Password = password };
                
                // Регистрация
                User newuser = await MyCarService.RegistrationNewUser(user);
                
                // Отправка на главную страницу данных пользователя
                FileManager.SaveUser(user);
                await Navigation.PopModalAsync(); // Закрытие текущего окна
            }
            else
                await DisplayAlert("Ошибка", "Пользователь с такими данными уже существует", "Ок");

        }
    }
}