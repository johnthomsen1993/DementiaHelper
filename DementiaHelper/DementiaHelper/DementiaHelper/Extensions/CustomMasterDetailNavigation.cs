using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FreshMvvm
{
    /// <summary>
    /// Adapted from https://github.com/rid00z/FreshMvvm/blob/master/src/FreshMvvm/NavigationContainers/FreshMasterDetailNavigationContainer.cs
    /// mainly changed CreateMenuPage functionality
    /// </summary>
    public class CustomMasterDetailNavigation : MasterDetailPage, IFreshNavigationService
    {

        List<Page> _pagesInner = new List<Page>();
        Dictionary<string, Page> _pages = new Dictionary<string, Page>();
        ContentPage _menuPage;
        ObservableCollection<string> _pageNames = new ObservableCollection<string>();

        public Dictionary<string, Page> Pages { get { return _pages; } }
        protected ObservableCollection<string> PageNames { get { return _pageNames; } }

        public CustomMasterDetailNavigation() : this(Constants.DefaultNavigationServiceName)
        {
        }

        public CustomMasterDetailNavigation(string navigationServiceName)
        {
            NavigationServiceName = navigationServiceName;
            RegisterNavigation();
        }

        public void Init(string menuTitle, string menuIcon = null)
        {
            CreateMenuPage(menuTitle, menuIcon);
            RegisterNavigation();
        }

        protected virtual void RegisterNavigation()
        {
            FreshIOC.Container.Register<IFreshNavigationService>(this, NavigationServiceName);
        }

        public virtual void AddPage<T>(string title, object data = null) where T : FreshBasePageModel
        {
            var page = FreshPageModelResolver.ResolvePageModel<T>(data);
            page.GetModel().CurrentNavigationServiceName = NavigationServiceName;
            _pagesInner.Add(page);
            var navigationContainer = CreateContainerPage(page);
            _pages.Add(title, navigationContainer);
            _pageNames.Add(title);
            if (_pages.Count == 1)
                Detail = navigationContainer;
        }

        internal Page CreateContainerPageSafe(Page page)
        {
            if (page is NavigationPage || page is MasterDetailPage || page is TabbedPage)
                return page;

            return CreateContainerPage(page);
        }

        protected virtual Page CreateContainerPage(Page page)
        {
            return new NavigationPage(page);
        }

        protected virtual void CreateMenuPage(string menuPageTitle, string menuIcon = null)
        {
            _menuPage = new ContentPage();
            _menuPage.Title = menuPageTitle;
            var listView = new ListView();
            listView.ItemsSource = _pageNames;
            listView.HasUnevenRows = true;
            listView.ItemTemplate = new DataTemplate(() => {
                var ViewCell = new ViewCell();
                var myLabel = new Label();
                myLabel.SetBinding(Label.TextProperty, ".");
                if (Device.Idiom == TargetIdiom.Tablet)
                {
                    myLabel.FontSize = 36;
                    myLabel.FontAttributes = FontAttributes.Bold;
                }else {
                    myLabel.FontSize = 20;
                    myLabel.FontAttributes = FontAttributes.Bold;
                }
                ViewCell.View = myLabel;

                return ViewCell;
            });
            listView.ItemSelected += (sender, args) => {
                if (_pages.ContainsKey((string)args.SelectedItem))
                {
                    Detail = _pages[(string)args.SelectedItem];
                }
                IsPresented = false;
            };

            _menuPage.Content = listView;

            var navPage = new NavigationPage(_menuPage) { Title = "Menu" };
            
            if (!string.IsNullOrEmpty(menuIcon))
                navPage.Icon = menuIcon;

            Master = navPage;
        }

        public Task PushPage(Page page, FreshBasePageModel model, bool modal = false, bool animate = true)
        {
            if (modal)
                return Navigation.PushModalAsync(CreateContainerPageSafe(page));
            return (Detail as NavigationPage).PushAsync(page, animate); 
        }

        public Task PopPage(bool modal = false, bool animate = true)
        {
            if (modal)
                return Navigation.PopModalAsync(animate);
            return (Detail as NavigationPage).PopAsync(animate);            
        }

        public Task PopToRoot(bool animate = true)
        {
            return (Detail as NavigationPage).PopToRootAsync(animate);
        }

        public string NavigationServiceName { get; private set; }

        public void NotifyChildrenPageWasPopped()
        {
            if (Master is NavigationPage)
                ((NavigationPage)Master).NotifyAllChildrenPopped();
            foreach (var page in this.Pages.Values)
            {
                if (page is NavigationPage)
                    ((NavigationPage)page).NotifyAllChildrenPopped();
            }
        }

        public Task<FreshBasePageModel> SwitchSelectedRootPageModel<T>() where T : FreshBasePageModel
        {
            var tabIndex = _pagesInner.FindIndex(o => o.GetModel().GetType().FullName == typeof(T).FullName);

            Detail = _pages.Values.ElementAt(tabIndex); ;

            return Task.FromResult((Detail as NavigationPage).CurrentPage.GetModel());
        }
    }
}

