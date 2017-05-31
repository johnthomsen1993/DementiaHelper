﻿using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace DementiaHelper.Extensions
{
    /// <summary>
    /// https://developer.xamarin.com/guides/xamarin-forms/advanced/localization/
    /// </summary>

    [ContentProperty("Text")]
    public class TranslateExtension : IMarkupExtension
    {
        readonly CultureInfo ci;
        const string ResourceId = "DementiaHelper.Resx.AppResources";

        public TranslateExtension()
        {

            if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)

            {
                ci = DependencyService.Get<Resx.ILocalize>().GetCurrentCultureInfo();
            }
        }

        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text == null)
                return "";

            ResourceManager resmgr = new ResourceManager(ResourceId
                                , typeof(TranslateExtension).GetTypeInfo().Assembly);

            var translation = resmgr.GetString(Text, ci);

            if (translation == null)
            {
                #if DEBUG
                throw new ArgumentException (
                    String.Format ("Key '{0}' was not found in resources '{1}' for culture '{2}'.", Text, ResourceId, ci.Name),
                    "Text");
                #else
                translation = Text; // HACK: returns the key, which GETS DISPLAYED TO THE USER
                #endif
            }
            return translation;
        }
    }
}
