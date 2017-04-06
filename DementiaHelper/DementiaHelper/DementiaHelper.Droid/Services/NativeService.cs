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
        public void CloseApp()
        {
            Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
        }
    }
}