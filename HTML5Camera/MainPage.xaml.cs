using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Tasks;
using System.IO.IsolatedStorage;

namespace HTML5Camera
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Url of Home page
        private string MainUri = "/Html/index.html";

        CameraCaptureTask _cct = new CameraCaptureTask();
        String _userImage = "userImage.jpg";

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            _cct.Completed += cct_Completed;
        }

        void cct_Completed(object sender, PhotoResult e)
        {
            BitmapImage bi = new BitmapImage();
            bi.SetSource(e.ChosenPhoto);
            SaveImage(bi);
            
            //Tell the Browser.
            Browser.InvokeScript("displayImage", _userImage);
        }

        private void SaveImage(BitmapImage bi)
        {
            using (var isoStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var wb = new WriteableBitmap(bi);
                using (var isoFileStream = isoStore.CreateFile(_userImage))
                    Extensions.SaveJpeg(wb, isoFileStream, wb.PixelWidth, wb.PixelHeight, 0, 100);
            };
        }

        private void Browser_ScriptNotify(object sender, NotifyEventArgs e)
        {
            _cct.Show();
        }

        #region DEFAULT HANDLERS

        private void Browser_Loaded(object sender, RoutedEventArgs e)
        {
            // Add your URL here
            Browser.Navigate(new Uri(MainUri, UriKind.Relative));
        }

        // Navigates back in the web browser's navigation stack, not the applications.
        private void BackApplicationBar_Click(object sender, EventArgs e)
        {
            Browser.GoBack();
        }

        // Navigates forward in the web browser's navigation stack, not the applications.
        private void ForwardApplicationBar_Click(object sender, EventArgs e)
        {
            Browser.GoForward();
        }

        // Navigates to the initial "home" page.
        private void HomeMenuItem_Click(object sender, EventArgs e)
        {
            Browser.Navigate(new Uri(MainUri, UriKind.Relative));
        }

        // Handle navigation failures.
        private void Browser_NavigationFailed(object sender, System.Windows.Navigation.NavigationFailedEventArgs e)
        {
            MessageBox.Show("Navigation to this page failed, check your internet connection");
        }

        #endregion 
    }
}
