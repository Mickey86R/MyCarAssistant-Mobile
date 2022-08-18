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
    public partial class RedactAuto : ContentPage
    {
        Auto auto;

        public RedactAuto()
        {
            InitializeComponent();

            auto = new Auto();
        }

        public RedactAuto(Auto auto) // Конструктор с id пользователя, для создания связи нового авто с имеющимся пользователем
        {
            InitializeComponent();

            this.auto = auto;

            if (auto != null)
                FillingFields();
        }

        void FillingFields()
        {
            markEntry.Text = auto.Mark;
            modelEntry.Text = auto.Model;
            yearEntry.Text = auto.Year.ToString();
            mileageEntry.Text = auto.Mileage.ToString();
            powerEntry.Text = auto.Power.ToString(); ;
            engineCapacityEntry.Text = auto.EngineCapacity.ToString();
            vinEntry.Text = auto.VIN;
        }

        private async void RedactCarButton_Clicked(object sender, EventArgs e) // Добавить новый автомобиль
        {
            auto.Mark = markEntry.Text;
            auto.Model = modelEntry.Text;
            auto.Year = Convert.ToInt32(yearEntry.Text);
            auto.Mileage = Convert.ToInt32(mileageEntry.Text);
            auto.Power = Convert.ToInt32(powerEntry.Text);
            auto.EngineCapacity = (float)Convert.ToDouble(engineCapacityEntry.Text);
            auto.VIN = vinEntry.Text;

            await MyCarService.RegistrationNewAuto(auto); // Добавление нового авто в БД
            await Navigation.PopModalAsync(); // Закрытие текущего окна
        }

        private async void CancelButton_Clicked(object sender, EventArgs e) // Отменить добавление
        {
            await Navigation.PopModalAsync(); // Закрытие текущего окна
        }
    }
}