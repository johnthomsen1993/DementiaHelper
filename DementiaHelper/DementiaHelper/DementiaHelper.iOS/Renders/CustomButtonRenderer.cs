
using DementiaHelper.IOS.Renders;
using DementiaHelper.Renders.CustomControlRenders;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomButtonRender), typeof(CustomButtonRenderer))]
namespace DementiaHelper.IOS.Renders
{
    public class CustomButtonRenderer : ButtonRenderer
    {
        /// <summary>
        /// http://www.wintellect.com/devcenter/jprosise/supercharging-xamarin-forms-with-custom-renderers-part-2
        /// </summary>
        /// <param name="e"></param>
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);

            //if (Control != null)
            //{
            //    var button = e.NewElement;

            //    button.SizeChanged += (s, args) =>
            //    {
            //        var radius = Math.Min(button.Width, button.Height) / 2.0;
            //        button.BorderRadius = (int)(radius);
            //    };
            //}

        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
          
        }
    }
}