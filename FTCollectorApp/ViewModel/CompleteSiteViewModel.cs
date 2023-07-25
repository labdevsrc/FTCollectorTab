using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon.S3.Model;
using FTCollectorApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FTCollectorApp.Model;
using System.Threading.Tasks;
using SignaturePad.Forms;
using PCLStorage;

namespace FTCollectorApp.ViewModel
{
    public partial class CompleteSiteViewModel:ObservableObject
    {
        private readonly IAmazonS3 _awsS3Client;
        public Func<Task<byte[]>> SignatureFromStream { get; set; }
        //public Stream SignatureFromStream { get; set; }
        public byte[] Signature { get; set; }


        public CompleteSiteViewModel()
        {
            _awsS3Client = new AmazonS3Client(Constants.ACCES_KEY_ID, Constants.SECRET_ACCESS_KEY, RegionEndpoint.GetBySystemName(Constants.BUCKET_REGION));
        }


        [ObservableProperty] bool isBusy = false;

        [ObservableProperty] bool isEnableBtn = true;

        [ObservableProperty] string crewleader = Session.crew_leader;

        [ICommand]
        private async void Save()
        {

            ///////////////////////// S3 Bucket  Upload ////////////////////////////

            var fileTransferUtility = new TransferUtility(Constants.ACCES_KEY_ID, Constants.SECRET_ACCESS_KEY, RegionEndpoint.USEast2);

            //$fname=$owner."_".$tag."_".$user."_".$page."_".$longitude."_".$lattitude."_".date('Y-m-d-H-i-s').".png";
            try
            {
                Console.WriteLine($"Start capture stream from {App.SignatureFileLocation}");

                //Stream image  = await SignatureFromStream();
                Signature = await SignatureFromStream();

                //Stream image = await signaturePad.GetImageStreamAsync(SignaturePad.Forms.SignatureImageFormat.Png);

                var pictnaming = $"{Session.OwnerName}_{Session.tag_number}_signature_{Session.live_longitude}_{Session.live_lattitude}_{DateTime.Now.ToString("yyyy-M-d_HH-mm-ss")}_{Session.ownerkey}.png";
                IFolder rootFolder = FileSystem.Current.LocalStorage;
                IFolder folder = await rootFolder.CreateFolderAsync("images",
                    CreationCollisionOption.OpenIfExists);
                IFile file = await folder.CreateFileAsync(pictnaming,
                    CreationCollisionOption.ReplaceExisting);

                using (var fs = await file.OpenAsync(PCLStorage.FileAccess.ReadAndWrite))
                {
                    //await fs.WriteAsync(e.ImageData, 0, e.ImageData.Length);
                    //image.CopyTo(fs);
                }

                IsBusy = true;
                IsEnableBtn = false;
                await fileTransferUtility.UploadAsync(file.Path.ToString(), Constants.BUCKET_NAME);
                IsEnableBtn = true;
                IsBusy = false;

                if (IsFileExists(pictnaming))
                {
                    await Application.Current.MainPage.DisplayAlert("Upload OK", pictnaming + "DONE Uploading", "DONE");
                    var KVPair = keyvaluepair(pictnaming);
                    await CloudDBService.PostPictureSave(KVPair); // async upload to AWS table
                    //if (Session.stage.Equals("A"))
                    //    await Application.Current.MainPage.Navigation.PushAsync(new AsBuiltDocMenu());
                    //if (Session.stage.Equals("I"))
                    //    await Application.Current.MainPage.Navigation.PushAsync(new MainMenuInstall());
                }
                else
                {
                    Console.WriteLine();

                    await Application.Current.MainPage.DisplayAlert("Upload Error", "Picture NOT Found in S3 bucket. Check connection", "CONTINUE");
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine($"Exception {exp.ToString()}");
                await Application.Current.MainPage.DisplayAlert("Upload Fail", exp.ToString(), "CONTINUE");
            }


        }
        List<KeyValuePair<string, string>> keyvaluepair(string imgfname)
        {
            var keyValues = new List<KeyValuePair<string, string>>{
                new KeyValuePair<string, string>("uid", Session.uid.ToString()),
                new KeyValuePair<string, string>("owner_key", Session.ownerkey),
                new KeyValuePair<string, string>("OWNER_CD", Session.ownerCD), // 
                new KeyValuePair<string, string>("time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),  // 1
                new KeyValuePair<string, string>("tag", Session.tag_number),  // 2
                new KeyValuePair<string, string>("fname", imgfname),  // 3
                new KeyValuePair<string, string>("lattitude", Session.live_lattitude),  // 4
                new KeyValuePair<string, string>("longitude",  Session.live_longitude),  // 5
                new KeyValuePair<string, string>("page", Session.current_page),
            };


            return keyValues;

        }

        // Check if file name 
        public bool IsFileExists(string fileName)
        {
            try
            {
                GetObjectMetadataRequest request = new GetObjectMetadataRequest()
                {
                    BucketName = Constants.BUCKET_NAME,
                    Key = fileName
                };

                var response = _awsS3Client.GetObjectMetadataAsync(request).Result;

                return true;
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException is AmazonS3Exception awsEx)
                {
                    if (string.Equals(awsEx.ErrorCode, "NoSuchBucket"))
                        return false;

                    else if (string.Equals(awsEx.ErrorCode, "NotFound"))
                        return false;
                }

                throw;
            }
        }

    }
}
