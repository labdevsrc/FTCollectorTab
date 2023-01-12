using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using FTCollectorApp.Model;
using FTCollectorApp.Service;
using FTCollectorApp.View.SitesPage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FTCollectorApp.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EquipmenReturnPage : ContentPage
    {
        
        public EquipmenReturnPage()
        {
            InitializeComponent();
        }

        private async void SaveBtn_Clicked(object sender, EventArgs e)
        {

            IsBusy = true;
            try
            {
                Console.WriteLine($"Start capture stream from {App.SignatureFileLocation}");
                Stream image = await signaturePad.GetImageStreamAsync( SignaturePad.Forms.SignatureImageFormat.Png);
                using (FileStream fs = new FileStream(App.SignatureFileLocation, FileMode.Create, FileAccess.Write))
                {
                    image.CopyTo(fs);
                    
                }


                // add text on signature
                /*using (Graphics graphics = Graphics.FromImage())
                {
                    using (Font arialFont = new Font("Arial", 10))
                    {
                        graphics.DrawString("Test", arialFont, Brushes.Blue, firstLocation);
                    }
                }*/
            }
            catch(Exception exp)
            {
                Console.WriteLine($"Exception {exp.ToString()}");

            }

            var KVsEquipmentCheckIn = new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>("uid", (Session.uid).ToString()),
                new KeyValuePair<string, string>("la", Session.live_lattitude),
                new KeyValuePair<string, string>("lo", Session.live_longitude),
                new KeyValuePair<string, string>("hrs", ""),
                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                new KeyValuePair<string, string>("jobnum", Session.jobnum),
                };


            var result = await CloudDBService.PostEquipmentCheckIn(KVsEquipmentCheckIn);
            





            var fileTransferUtility = new TransferUtility(Constants.ACCES_KEY_ID, Constants.SECRET_ACCESS_KEY, RegionEndpoint.USEast2);
            try
            {
                await fileTransferUtility.UploadAsync(App.SignatureFileLocation, Constants.BUCKET_NAME);
            }
            catch (Exception exp)
            {
                Console.WriteLine($"Exception {exp.ToString()}");
            }

            IsBusy = false;
        }

        private void ClearBtn_Clicked(object sender, EventArgs e)
        {
            signaturePad.Clear();
        }


        /*private MediaFile _mediaFile;
        private async void UploadFile_Clicked(object sender, EventArgs e)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(_mediaFile.GetStream()),
                "\"file\"",
                $"\"{_mediaFile.Path}\"");
            var httpClient = new HttpClient();
            var uploadServiceBaseAddress = "http://collector.fibertrak.com/FTService/images";  
            var httpResponseMessage = await httpClient.PostAsync(uploadServiceBaseAddress, content);
            RemotePathLabel.Text = await httpResponseMessage.Content.ReadAsStringAsync();
        }*/
    }
}