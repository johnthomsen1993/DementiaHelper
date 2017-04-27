//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Collections.Specialized;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Xamarin.Forms;

//namespace DementiaHelper.Renders.CustomControlRenders
//{
//    public class CustomImageGallery : ScrollView
//    {
//        readonly StackLayout _imageStack;

//        public CustomImageGallery()
//        {
//            this.Orientation = ScrollOrientation.Horizontal;

//            _imageStack = new StackLayout
//            {
//                Orientation = StackOrientation.Horizontal
//            };

//            this.Content = _imageStack;
//        }

//        public IList<View> Children => _imageStack.Children;


//        public static readonly BindableProperty ItemsSourceProperty =
//            BindableProperty.Create(
//                nameof(ItemsSource),
//                typeof(IList),
//                typeof(CustomImageGallery),
//                default(IList),
//                BindingMode.TwoWay,
//                propertyChanging: (bindable, oldValue, newValue) =>
//                {
//                    ((CustomImageGallery)bindable).ItemsSourceChanging();
//                },
//                propertyChanged: (bindableObject, oldValue, newValue) =>
//                {
//                    ((CustomImageGallery)bindableObject).ItemsSourceChanged(bindableObject, (IList)oldValue, (IList)newValue);
//                });

//        public IList ItemsSource
//        {
//            get { return (IList)GetValue(ItemsSourceProperty); }
//            set { SetValue(ItemsSourceProperty, value); }
//        }

//        void ItemsSourceChanging()
//        {
//            if (ItemsSource == null)
//                return;
//        }

//        void ItemsSourceChanged(BindableObject bindable, IList oldValue, IList newValue)
//        {
//            if (ItemsSource == null)
//                return;

//            if (newValue is INotifyCollectionChanged notifyCollection)
//            {
//                notifyCollection.CollectionChanged += (sender, args) =>
//                {
//                    if (args.NewItems != null)
//                    {
//                        foreach (var newItem in args.NewItems)
//                        {

//                            var view = (View)ItemTemplate.CreateContent();
//                            var bindableObject = (BindableObject)view;
//                            if (bindableObject != null)
//                                bindableObject.BindingContext = newItem;
//                            _imageStack.Children.Add(view);
//                        }
//                    }
//                    if (args.OldItems != null)
//                    {
//                        not supported
//                        _imageStack.Children.RemoveAt(args.OldStartingIndex);
//                    }
//                };
//            }

//        }

//        public DataTemplate ItemTemplate
//        {
//            get;
//            set;
//        }

//        public static readonly BindableProperty SelectedItemProperty =
//              BindableProperty.Create(
//                  nameof(SelectedItem),
//                  typeof(object),
//                  typeof(CustomImageGallery),
//                  null,
//                  BindingMode.TwoWay,
//                  propertyChanged: (bindable, oldValue, newValue) =>
//                  {
//                      ((CustomImageGallery)bindable).UpdateSelectedIndex();
//                  }
//              );

//        public object SelectedItem
//        {
//            get => GetValue(SelectedItemProperty);
//            set => SetValue(SelectedItemProperty, value);
//            }

//        void UpdateSelectedIndex()
//        {
//            if (SelectedItem == BindingContext)
//                return;

//            SelectedIndex = Children
//                .Select(c => c.BindingContext)
//                .ToList()
//                .IndexOf(SelectedItem);

//        }

//        public static readonly BindableProperty SelectedIndexProperty =
//            BindableProperty.Create(
//                nameof(SelectedIndex),
//                typeof(int),
//                typeof(CustomImageGallery),
//                0,
//                BindingMode.TwoWay,
//                propertyChanged: (bindable, oldValue, newValue) =>
//                {
//                    ((CustomImageGallery)bindable).UpdateSelectedItem();
//                });

//        public int SelectedIndex
//        {
//            get { return (int)GetValue(SelectedIndexProperty); }
//            set { SetValue(SelectedIndexProperty, value); }
//        }

//        void UpdateSelectedItem() =>
//            SelectedItem = SelectedIndex > -1 ? Children[SelectedIndex].BindingContext : null;
//    }
//}

