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
    public partial class TORecord : ContentPage
    {
        T_O TO;

        public TORecord()
        {
            InitializeComponent();
        }

        public TORecord(T_O tO)
        {
            InitializeComponent();

            this.TO = tO;
            RecordEditor.Text = TO.Record;
        }

        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            TO.Record = RecordEditor.Text;

            Navigation.PopModalAsync();
        }

        private void CancelButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}