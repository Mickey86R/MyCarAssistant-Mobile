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
    public partial class RedactRecord : ContentPage
    {
        Record record;

        public RedactRecord()
        {
            InitializeComponent();
        }

        public RedactRecord(Record record)
        {
            InitializeComponent();

            this.record = record;

            if (record != null)
                FillingFields();
        }

        void FillingFields()
        {
            TextEditor.Text = record.Text;
            PhoneEntry.Text = record.Phone;
            AddressEditor.Text = record.Address;
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (record != null)
            {
                record.Text = TextEditor.Text;
                record.Phone = PhoneEntry.Text;
                record.Address = AddressEditor.Text;

            }

            await MyCarService.RegistrationNewRecord(record);

            await Navigation.PopModalAsync();
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}