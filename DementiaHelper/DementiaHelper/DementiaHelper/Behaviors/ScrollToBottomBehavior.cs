using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DementiaHelper.Behaviors
{
    public class ListViewPagningBehavior : BehaviorBase<Xamarin.Forms.ListView>
    {

        public ListView AssociatedObject { get; private set; }
        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                nameof(ItemsSource),
                typeof(IList),
                typeof(ListViewPagningBehavior),
                default(IList),
                BindingMode.TwoWay,
                propertyChanging: (bindable, oldValue, newValue) =>
                {
                    ((ListViewPagningBehavior)bindable).ItemsSourceChanging();
                },
                propertyChanged: (bindableObject, oldValue, newValue) =>
                {
                    ((ListViewPagningBehavior)bindableObject).ItemsSourceChanged(bindableObject, (IList)oldValue, (IList)newValue);
                });



        void ItemsSourceChanging()
        {
            if (ItemsSource == null)
                return;
        }
    
        void ItemsSourceChanged(BindableObject bindable, IList oldValue, IList newValue)
        {
            if (ItemsSource == null)
                return;
            var notifyCollection = (INotifyCollectionChanged)newValue;
            if (notifyCollection != null)
            {
                var last = newValue.Cast<object>().LastOrDefault();
                AssociatedObject.ScrollTo(last, ScrollToPosition.MakeVisible, true);

            }
        }
        protected override void OnAttachedTo(ListView bindable)
        {
            base.OnAttachedTo(bindable);

            AssociatedObject = bindable;
        }

        protected override void OnDetachingFrom(ListView bindable)
        {
            base.OnDetachingFrom(bindable);


            AssociatedObject = null;
        }

        protected void OnBindingContextChanged(object sender, EventArgs e)
        {
            OnBindingContextChanged();
        }

    }
}

