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
        public void CloseApp()
        {
            Process.GetCurrentProcess().CloseMainWindow();
            Process.GetCurrentProcess().Close();
        }
    }
}