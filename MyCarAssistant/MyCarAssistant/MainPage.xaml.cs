using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using MyCarAssistant.Models;
using Android.App;


namespace MyCarAssistant
{
    public partial class MainPage : Shell
    {
        public User user;
        List<Auto> autos;

        bool
            isNewUser = false,
            isNewAuto = false,
            isTOChanged = false,
            isExpensChanged = false,
            isRecordChanged = false,
            isRedactUser = false,
            isRedactAuto = false,
            isDelAuto = false;

        int selectedAuto = 0;

        public MainPage()
        {
            InitializeComponent();

            GetUserAndAutos();
        }

        async void GetUserAndAutos()
        {
            // получаем пользователя
            if (FileManager.IsUserAvailable())
            {
                user = FileManager.GetUserFromFile();
                if (FileManager.IsAutoAvailable())
                {
                    autos = FileManager.GetAutosFromFile();
                }
                else
                {
                    autos = new List<Auto>();
                    autos.AddRange(await MyCarService.GetAutosFromUserID(user.ID));
                    FileManager.SaveAutos(autos);
                }

                FillingPicker();
                FillingAllFieldsWithoutPicker();
            }
            else
            {
                isNewUser = true;
                await Navigation.PushModalAsync(new AuthorizationPage(this));
            }
        }

        async Task UpdateAll()
        {
            user = await MyCarService.GetUserFromDB(user.Login, user.Password);
            autos = await MyCarService.GetAutosFromUserID(user.ID);

            FileManager.SaveUser(user);
            FileManager.SaveAutos(autos);

            FillingAllFieldsWithoutPicker();
        }

        //======================================================================== НАПОЛНЕНИЕ!
        void FillingAllFieldsWithoutPicker()
        {
            FillingMainPage();
            FillingProfile();
            FillingRecords();
            FillingTOs();
            FillingExpenses();
            FillingDataPhoto();
            FillingRedactAuto();
        }

        void FillingPicker()
        {
            picker.ItemsSource = autos;

            selectedAuto = 0;
            picker.SelectedIndex = selectedAuto;
        }

        void FillingMainPage()
        {
            Auto currentAuto = autos[selectedAuto];

            currentVINLabel.Text = currentAuto.VIN;

            currentMarkLabel.Text = currentAuto.Mark;
            currentModelLabel.Text = currentAuto.Model;
            currentYearLabel.Text = currentAuto.Year.ToString();

            currentCapaPowerLabel.Text = currentAuto.EngineCapacity.ToString() + "/" + currentAuto.Power.ToString();

            currentMileageLabel.Text = currentAuto.Mileage.ToString();

            int?
                mileageGeneral = 0,
                mileageGeneralExp = 0,
                mileageGeneralTO = 0;

            if (currentAuto.Expenses.Count == 0)
            {
                currentMileageGeneralLabel.Text = 0.ToString();
                currentValueGeneralLabel.Text = 0.ToString();
            }
            else
            {
                mileageGeneralExp = (currentAuto.Expenses.Max(x => x.Mileage) -
                                            currentAuto.Expenses.Min(x => x.Mileage));

                currentValueGeneralLabel.Text = currentAuto.Expenses.Sum(v => v.Volume).ToString();
            }

            if (currentAuto.T_Os.Count != 0)
            {
                mileageGeneralTO = ((currentAuto.T_Os).Max(x => x.Mileage) -
                                            currentAuto.T_Os.Min(x => x.Mileage));
            }

            if (mileageGeneralTO > mileageGeneralExp) mileageGeneral = mileageGeneralTO;
            else mileageGeneral = mileageGeneralExp;

            currentMileageGeneralLabel.Text = mileageGeneral.ToString();
        }

        void FillingDataPhoto()
        {
            if (FileManager.IsPhotoExist($"PTS.{autos[selectedAuto].VIN}.jpg"))
            {
                string pathPTS = Path.Combine(FileSystem.AppDataDirectory, $"PTS.{autos[selectedAuto].VIN}.jpg");
                PTSIMG.Source = ImageSource.FromFile(pathPTS);
            }
            else PTSIMG.Source = null;
            if (FileManager.IsPhotoExist($"STR.{autos[selectedAuto].VIN}.jpg"))
            {
                string pathSTR = Path.Combine(FileSystem.AppDataDirectory, $"STR.{autos[selectedAuto].VIN}.jpg");
                STRIMG.Source = ImageSource.FromFile(pathSTR);
            }
            else STRIMG.Source = null;
        }

        void FillingTOs()
        {
            TONewCollectionView.ItemsSource = autos[selectedAuto].T_Os.Where(a => a.Date >= DateTime.Today);
            TOOldCollectionView.ItemsSource = autos[selectedAuto].T_Os.Where(a => a.Date < DateTime.Today);
        }

        void FillingRecords()
        {
            RecordCollectionView.ItemsSource = autos[selectedAuto].Records.OrderBy(r => r.ID);
        }

        void FillingExpenses()
        {
            ExpensCollectionView.ItemsSource = autos[selectedAuto].Expenses.OrderBy(e => e.Date);
            SummOfExpanses.Text = (autos[selectedAuto].Expenses.Sum(e => e.Sum)).ToString();
        }

        // Вкладки бокового меню
        void FillingProfile()
        {
            userLoginEntry.Text = user.Login;
            userPasswordEntry.Text = user.Password;
            userEmailEntry.Text = user.Email;
        }

        void FillingRedactAuto()
        {
            redactAutoMarkEntry.Text = autos[selectedAuto].Mark;
            redactAutoModelEntry.Text = autos[selectedAuto].Model;
            redactAutoYearEntry.Text = autos[selectedAuto].Year.ToString();
            redactAutoEngineCapacityEntry.Text = autos[selectedAuto].EngineCapacity.ToString();
            redactAutoPowerEntry.Text = autos[selectedAuto].Power.ToString();

            redactAutoVINEntry.Text = autos[selectedAuto].VIN.ToString();

            redactAutoMileageEntry.Text = autos[selectedAuto].Mileage.ToString();
        }

        //======================================================================== PUSH/POP MODALASYNC/ASYNC

        protected override async void OnAppearing() // Вызов происходит в момент закрытия страницы, открытой с MainPage
        {
            base.OnAppearing();

            if (isTOChanged)
            {
                isTOChanged = false;

                await UpdateAll();
                FillingTOs();
            }
            if (isExpensChanged)
            {
                isExpensChanged = false;

                await UpdateAll();
                FillingExpenses();
            }
            if (isRecordChanged)
            {
                isRecordChanged = false;

                await UpdateAll();
                FillingRecords();
            }
            if (isNewAuto || isDelAuto || isRedactAuto)
            {
                isNewAuto = false;
                isDelAuto = false;
                isRedactAuto = false;

                if (isDelAuto)
                {
                    string s = "К сожалению, на данный момент выпадающий список автомобилей не обновляется при работе приложения. " +
                            "Чтобы отобразились изменения, пожалуйста, перезапустите приложение.\n\n" +
                            "Благодарим Вас за использование приложения и приносим извинения за неудобства.\n" +
                            "Также мы открыты к предложениям по модификации приложения! Адрес указан в рзделе 'О проекте'.";

                    await DisplayAlert("Внимание!", s, "Спасибо за информацию!");
                }

                await UpdateAll();
            }
            if (isNewUser)
            {
                isNewUser = false;

                GetUserAndAutos();
                //FillingAllFieldsWithoutPicker();
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        //======================================================================== ОБРАБОТЧИКИ СОБЫТИЙ

        // кнопка "обновить"
        private async void RefreshImageButton_Clicked(object sender, EventArgs e)
        {
            await UpdateAll();
            FillingAllFieldsWithoutPicker();
        }

        // выбор в пикере
        private async void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedAuto = picker.SelectedIndex;

            var selectedAutomobil = picker.SelectedItem;
            FillingAllFieldsWithoutPicker();
        }

        //======================================================================== БОКОВОЕ ОКНО РЕДАКТИРОВАНИЯ АВТОМОБИЛЯ

        // Сохранить изменения
        private async void SaveRedactAutoButton_Clicked(object sender, EventArgs e)
        {
            Auto currentAuto = autos[selectedAuto];

            currentAuto.Mark = redactAutoMarkEntry.Text;
            currentAuto.Model = redactAutoModelEntry.Text;
            currentAuto.Year = Convert.ToInt32(redactAutoYearEntry.Text);
            currentAuto.EngineCapacity = (float)(Convert.ToDouble(redactAutoEngineCapacityEntry.Text));

            currentAuto.VIN = redactAutoVINEntry.Text;

            currentAuto.Mileage = Convert.ToInt32(redactAutoMileageEntry.Text);

            FileManager.SaveAutos(autos);
            await MyCarService.UpdateAuto(currentAuto);

            FillingMainPage();
        }

        // Отменить изменения
        private void CancelRedactAutoButton_Clicked(object sender, EventArgs e)
        {
            FillingRedactAuto();
        }

        // Удалить текущий автомобиль
        private async void DeleteRedactAutoButton_Clicked(object sender, EventArgs e)
        {
            isDelAuto = true;

            int id = autos[selectedAuto].ID;
            int currentAuto = selectedAuto;

            picker.SelectedIndex = 0;
            //picker.Items.Remove(selectedItem);
            autos.RemoveAt(currentAuto);

            FileManager.SaveAutos(autos);
            var delAuto = await MyCarService.DeleteAuto(id);

            FillingPicker();
            await DisplayAlert("Удалено!", delAuto.ToString(), "Ok");
        }


        //======================================================================== БОКОВОЕ ОКНО РЕДАКТИРОВАНИЯ ПРОФИЛЯ

        // СОБЫТИЯ С ОКНА РЕДАКТИРОВАНИЯ ПРОФИЛЯ

        private async void RedactUserButton_Clicked(object sender, EventArgs e)
        {
            isRedactUser = true;
            await Navigation.PushModalAsync(new RedactUser(user));
        }

        private void ExitProfileButton_Clicked(object sender, EventArgs e)
        {
            FileManager.DeleteUser();

            string s = "К сожалению, на данный момент повторная авторизация пользователя работает через повторное открытие приложения. " +
                            "Чтобы авторизоваться, пожалуйста, перезапустите приложение.\n\n" +
                            "Благодарим Вас за использование приложения и приносим извинения за неудобства.\n" +
                            "Также мы открыты к предложениям по модификации приложения! Адрес указан в рзделе 'О проекте'.";

            DisplayAlert("Внимание!", s, "Спасибо за информацию!");

            GetUserAndAutos(); // для запуска окна авторизации
        }

        //======================================================================== ГЛАВНАЯ СТРАНИЦА
        // Добавить авто
        private async void AddNewAutoButton_Clicked(object sender, EventArgs e)
        {
            isNewAuto = true;
            await Navigation.PushModalAsync(new NewAuto(user.ID));
        }

        // Редактировать текущий авто
        private async void RedactCurrentAutoButton_Clicked(object sender, EventArgs e)
        {
            isRedactAuto = true;
            await Navigation.PushModalAsync(new RedactAuto(autos[selectedAuto]));
        }

        //======================================================================== ВКЛАДКА ФОТО

        // ДОБАВЛЕНИЕ ФОТО
        // ПТС
        private async void AddPTSImageButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions());

                // для примера сохраняем файл в локальном хранилище
                var newFile = Path.Combine(FileSystem.AppDataDirectory, $"PTS.{autos[selectedAuto].VIN}.jpg");
                using (var stream = await photo.OpenReadAsync())
                {
                    using (var newStream = File.Create(newFile))
                        await stream.CopyToAsync(newStream);
                }

                // загружаем в ImageView
                PTSIMG.Source = ImageSource.FromFile(newFile);
            }
            catch (Exception ex)
            {
                //await DisplayAlert("Сообщение об ошибке", ex.Message, "OK");
            }
        }

        // Страховка
        private async void AddSTRImageButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync(new MediaPickerOptions());

                // для примера сохраняем файл в локальном хранилище
                var newFile = Path.Combine(FileSystem.AppDataDirectory, $"STR.{autos[selectedAuto].VIN}.jpg");
                using (var stream = await photo.OpenReadAsync())
                {
                    using (var newStream = File.Create(newFile))
                        await stream.CopyToAsync(newStream);
                }

                // загружаем в ImageView
                STRIMG.Source = ImageSource.FromFile(newFile);
            }
            catch (Exception ex)
            {
                //await DisplayAlert("Сообщение об ошибке", ex.Message, "OK");
            }
        }

        //======================================================================== ВКЛАДКИ ТО, РАСХОДЫ, ЗАПИСИ 

        // СОБЫТИЯ С ОКНА ОБСЛУЖИВАНИЙ

        private async void NewTOImageButton_Clicked(object sender, EventArgs e)
        {
            isTOChanged = true;
            await Navigation.PushModalAsync(new RedactTO(new T_O { AutoID = autos[selectedAuto].ID, Date = DateTime.Now, Record = "" }));
        }

        private async void TOCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isTOChanged = true;
            T_O item = ((CollectionView)sender).SelectedItem as T_O;
            await Navigation.PushModalAsync(new RedactTO(item));
        }

        private async void TODeleteSwipeItem_Clicked(object sender, EventArgs e)
        {
            isTOChanged = true;

            var item = (sender as BindableObject).BindingContext as T_O; ;

            await MyCarService.DeleteTO(item.ID);
            autos[selectedAuto].T_Os.Remove(item);

            FillingTOs();
        }

        // СОБЫТИЯ С ОКНА ЗАТРАТ

        private async void NewExpensButton_Clicked(object sender, EventArgs e)
        {
            isExpensChanged = true;
            await Navigation.PushModalAsync(new RedactExpens(new Expens { AutoID = autos[selectedAuto].ID, Date = DateTime.Now, Text = "" }));
        }

        private async void ExpensCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isExpensChanged = true;
            Expens item = ((CollectionView)sender).SelectedItem as Expens;
            await Navigation.PushModalAsync(new RedactExpens(item));
        }

        private async void ExpensDeleteSwipeItem_Clicked(object sender, EventArgs e)
        {
            isExpensChanged = true;

            var item = (sender as BindableObject).BindingContext as Expens; ;

            await MyCarService.DeleteExpens(item.ID);
            autos[selectedAuto].Expenses.Remove(item);

            FillingExpenses();
        }

        // СОБЫТИЯ С ОКНА ЗАПИСЕЙ

        private async void NewRecordButton_Clicked(object sender, EventArgs e)
        {
            isRecordChanged = true;
            await Navigation.PushModalAsync(new RedactRecord(new Record { AutoID = autos[selectedAuto].ID, Text = "", Phone = "", Address = "" }));
        }

        private async void RecordCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isRecordChanged = true;
            Record item = ((CollectionView)sender).SelectedItem as Record;
            await Navigation.PushModalAsync(new RedactRecord(item));
        }

        private async void RecordDeleteSwipeItem_Clicked(object sender, EventArgs e)
        {
            isRecordChanged = true;

            var item = (sender as BindableObject).BindingContext as Record; ;

            await MyCarService.DeleteRecord(item.ID);
            autos[selectedAuto].Records.Remove(item);

            FillingRecords();
        }

    }
}