using System;
using System.Linq;
using DementiaHelper.Droid;
using DementiaHelper.Services;
using Xamarin.Forms;
[assembly: Dependency(typeof(NativeService))]
namespace DementiaHelper.Droid
{
    public class NativeService : INativeService
    {
        /// <summary>
        /// Found out how to close a Xamarin.Forms application https://stackoverflow.com/questions/40254023/how-to-exit-the-app-in-xamarin-forms
        /// </summary>
        public void CloseApp()
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
    }
}