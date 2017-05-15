using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DementiaHelper.PageModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DementiaHelper.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatTabPage : TabbedPage
    {
        public ChatTabPage()
        {
            InitializeComponent();
        }
    }
}
