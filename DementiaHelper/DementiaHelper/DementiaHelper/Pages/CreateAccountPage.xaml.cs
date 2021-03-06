﻿using DementiaHelper.Resx;
using DementiaHelper.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DementiaHelper.Pages
{
    public partial class CreateAccountPage : ContentPage
    {


        private double width;
        private double height;
        public CreateAccountPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// how to handle rotation changes in xamarin.forms have been adapted from
        /// https://developer.xamarin.com/guides/xamarin-forms/user-interface/layouts/device-orientation/
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (width != this.width || height != this.height)
            {
                this.width = width;
                this.height = height;
                if (width > height)
                {
                    if (Device.Idiom == TargetIdiom.Phone)
                    {
                        outerStack.Orientation = StackOrientation.Horizontal;
                        secondInnerStack.VerticalOptions = LayoutOptions.StartAndExpand;
                        imageElderlyHands.HeightRequest = 0;
                        RolePickerStackLayout.Orientation = StackOrientation.Horizontal;
                    }
                    else if (Device.Idiom == TargetIdiom.Tablet)
                    {
                        outerStackTablet.Orientation = StackOrientation.Horizontal;
                        secondInnerStackTablet.VerticalOptions = LayoutOptions.StartAndExpand;
                        //outerStackTablet
                        imageElderlyHandsTablet.HeightRequest = 350;
                    }


                }
                else
                {
                    if (Device.Idiom == TargetIdiom.Phone)
                    {
                        RolePickerStackLayout.Orientation = StackOrientation.Vertical;
                        outerStack.Orientation = StackOrientation.Vertical;
                        secondInnerStack.VerticalOptions = LayoutOptions.CenterAndExpand;
                        imageElderlyHands.HeightRequest = 200;
                    }
                    else if (Device.Idiom == TargetIdiom.Tablet)
                    {
                        outerStackTablet.Orientation = StackOrientation.Vertical;
                        secondInnerStackTablet.VerticalOptions = LayoutOptions.CenterAndExpand;
                        imageElderlyHandsTablet.HeightRequest = 400;
                    }

                }
            }
        }
    }
}