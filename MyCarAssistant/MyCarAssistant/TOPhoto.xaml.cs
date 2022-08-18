using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Essentials;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyCarAssistant
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TOPhoto : ContentPage
    {
        int to_id;

        public TOPhoto()
        {
            InitializeComponent();
        }

        public TOPhoto(int TO_ID)
        {
            InitializeComponent();

            this.to_id = TO_ID;

            if (FileManager.IsPhotoExist($"{to_id}.jpg"))
            {
                string pathPTS = Path.Combine(FileSystem.AppDataDirectory, $"{to_id}.jpg");
                IMG.Source = ImageSource.FromFile(pathPTS);
            }
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void AddPhotoButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions());

                // для примера сохраняем файл в локальном хранилище
                var newFile = Path.Combine(FileSystem.AppDataDirectory, $"{to_id}.jpg");
                using (var stream = await photo.OpenReadAsync())
                {
                    using (var newStream = File.Create(newFile))
                        await stream.CopyToAsync(newStream);
                }


                // загружаем в ImageView
                IMG.Source = ImageSource.FromFile(newFile);
            }
            catch (Exception ex)
            {
                //await DisplayAlert("Сообщение об ошибке", ex.Message, "OK");
            }
        }
    }
}