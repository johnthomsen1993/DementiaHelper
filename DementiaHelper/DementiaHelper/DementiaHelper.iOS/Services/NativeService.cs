using System;
using AVFoundation;
using Xamarin.Forms;
using DementiaHelper.iOS;
using System.Diagnostics;
using DementiaHelper.Services;

[assembly: Dependency(typeof(NativeService))]
namespace DementiaHelper.iOS
{
    public class NativeService : INativeService
    {
        /// <summary>
        /// Found out how to close a Xamarin.Forms application https://stackoverflow.com/questions/40254023/how-to-exit-the-app-in-xamarin-forms
        /// </summary>
        public void CloseApp()
        {
            Process.GetCurrentProcess().CloseMainWindow();
            Process.GetCurrentProcess().Close();
        }
    }
}