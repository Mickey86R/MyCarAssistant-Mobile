using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MyCarAssistant.Models;

namespace MyCarAssistant
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewAuto : ContentPage
    {
        int userID; // ID пользователя
        Auto auto;

        public NewAuto()
        {
            InitializeComponent();
        }

        public NewAuto(int userID) // Конструктор с id пользователя, для создания связи нового авто с имеющимся пользователем
        {
            InitializeComponent();

            this.userID = userID;
            auto = new Auto();
        }

        async Task ShowAlarm()
        {
            string s = "К сожалению, на данный момент выпадающий список автомобилей не обновляется при работе приложения. " +
                            "Чтобы отобразились изменения, пожалуйста, перезапустите приложение.\n\n" +
                            "Благодарим Вас за использование приложения и приносим извинения за неудобства.\n" +
                            "Также мы открыты к предложениям по модификации приложения! Адрес указан в рзделе 'О проекте'.";

            await DisplayAlert("Внимание!", s, "Спасибо за информацию!");
        }

        private async void AddCarButton_Clicked(object sender, EventArgs e) // Добавить новый автомобиль
        {
            auto.Mark = markEntry.Text;
            auto.Model = modelEntry.Text;
            auto.Year = Convert.ToInt32(yearEntry.Text);
            auto.Mileage = Convert.ToInt32(mileageEntry.Text);
            auto.Power = Convert.ToInt32(powerEntry.Text);
            auto.EngineCapacity = (float)Convert.ToDouble(engineCapacityEntry.Text);
            auto.VIN = vinEntry.Text;

            auto = await MyCarService.RegistrationNewAuto(auto); // Добавление нового авто в БД
            await MyCarService.CreateRelation(auto.ID, userID); // Создание связи
            await ShowAlarm();
            await Navigation.PopModalAsync(); // Закрытие текущего окна
        }

        private async void CancelButton_Clicked(object sender, EventArgs e) // Отменить добавление
        {
            await Navigation.PopModalAsync(); // Закрытие текущего окна
        }
    }
}