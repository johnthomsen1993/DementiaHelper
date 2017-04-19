using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using DementiaHelper.Renders.CustomControlRenders;
using DementiaHelper.Droid.Renders;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(ColorPickerCustomRender),typeof(ColorPickerRenderer))]
namespace DementiaHelper.Droid.Renders
{
   public class ColorPickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                var picker = e.NewElement;
                picker.TextColor = Color.Orange;
              

            }
        }
    }
}
