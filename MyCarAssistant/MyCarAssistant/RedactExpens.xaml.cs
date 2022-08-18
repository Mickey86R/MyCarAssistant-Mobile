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
    public partial class RedactExpens : ContentPage
    {
        Expens expens;

        public RedactExpens()
        {
            InitializeComponent();
        }

        public RedactExpens(Expens expens)
        {
            InitializeComponent();

            this.expens = expens;

            if (expens != null)
                FillingFields();
        }

        void FillingFields()
        {
            TextEntry.Text = expens.Text;
            SumEntry.Text = expens.Sum.ToString();
            MileageEntry.Text = expens.Mileage.ToString();
            CostEntry.Text = expens.Cost.ToString();
            VolumeEntry.Text = expens.Volume.ToString();
            DatePicker.Date = expens.Date;
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (expens != null)
            {
                expens.Text = TextEntry.Text;
                expens.Sum = (float)Convert.ToDouble(SumEntry.Text);
                expens.Mileage = Convert.ToInt32(MileageEntry.Text);
                expens.Cost = (float)Convert.ToDouble(CostEntry.Text);
                expens.Volume = (float)Convert.ToDouble(VolumeEntry.Text);
                expens.Date = DatePicker.Date;

            }

            await MyCarService.RegistrationNewExpens(expens);

            await Navigation.PopModalAsync();
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}