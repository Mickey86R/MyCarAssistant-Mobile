using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Plugin.LocalNotification;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using MyCarAssistant.Models;

namespace MyCarAssistant
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RedactTO : ContentPage
    {
        T_O tO;

        public RedactTO()
        {
            InitializeComponent();
        }

        public RedactTO(T_O tO)
        {

            InitializeComponent();

            this.tO = tO;

            if (tO != null)
                FillingFields();
        }

        void FillingFields()
        {
            Name.Text = tO.Name;
            Summ.Text = tO.Sum.ToString();
            Mileage.Text = tO.Mileage.ToString();
            DatePicker.Date = tO.Date;
            TimePicker.Time = tO.Date.Date.TimeOfDay;
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (tO != null)
            {
                tO.Name = Name.Text.ToString();
                tO.Sum = Convert.ToInt32(Summ.Text);
                tO.Mileage = Convert.ToInt32(Mileage.Text);
                tO.Date = DatePicker.Date + TimePicker.Time;
            }

            this.tO = await MyCarService.RegistrationNewTO(tO);

            AddNotify(tO.Name, tO.Date);

            await Navigation.PopModalAsync();
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private void RedactRecordButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new TORecord(this.tO));
        }

        private void PhotoButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new TOPhoto(tO.ID));
        }

        void AddNotify(string nameOfTO, DateTime dateTO)
        {
            string title = nameOfTO;

            DateTime
                notiDateStart,
                notiDateStop = tO.Date.AddDays(1);

            if((dateTO - DateTime.Now) <= TimeSpan.FromDays(3))
            {
                notiDateStart = DateTime.Now.Date.AddDays(1);
            }
            else
                notiDateStart = tO.Date.AddDays(-3);

            var noti0 = new NotificationRequest
            {
                BadgeNumber = 0,
                Description = $"У Вас запранировано ТО на {dateTO.Date.Day}.{dateTO.Date.Month}.{dateTO.Date.Year}",
                Title = title,
                Schedule = new NotificationRequestSchedule
                {
                    RepeatType = NotificationRepeat.TimeInterval,
                    NotifyTime = notiDateStart,
                    NotifyRepeatInterval = TimeSpan.FromDays(1),
                    NotifyAutoCancelTime = notiDateStop
                },  
                CategoryType = NotificationCategoryType.Reminder,
                NotificationId = tO.ID
            };

            var notiNow = new NotificationRequest
            {
                BadgeNumber = 10,
                Description = $"У Вас запланировано новое ТО на {dateTO.Date.Day}.{dateTO.Date.Month}.{dateTO.Date.Year}",
                Title = title,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now.AddSeconds(3)
                },
                CategoryType = NotificationCategoryType.Event,
                NotificationId = 0
            };

            NotificationCenter.Current.Show(notiNow);
            NotificationCenter.Current.Show(noti0);
        }
    }
}