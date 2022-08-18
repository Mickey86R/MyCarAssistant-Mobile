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
    public partial class RedactUser : ContentPage
    {
        User user;

        public RedactUser()
        {
            InitializeComponent();
        }

        public RedactUser(User user)
        {
            InitializeComponent();

            this.user = user;

            FillingFields();
        }

        void FillingFields()
        {
            LoginUserEntry.Text = user.Login;
            PasswordUserEntry.Text = user.Password;
            EmailUserEntry.Text = user.Email;
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            user.Login = LoginUserEntry.Text;
            user.Password = PasswordUserEntry.Text.GetHashCode().ToString();
            user.Email = EmailUserEntry.Text;

            FileManager.SaveUser(user);
            this.user = await MyCarService.UpdateUser(user);
            
            await Navigation.PopModalAsync();
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private void PasswordUserEntry_Focused(object sender, FocusEventArgs e)
        {
            (sender as Entry).Text = "";
        }
    }
}