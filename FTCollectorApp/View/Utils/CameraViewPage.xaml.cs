using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Xamarin.CommunityToolkit.UI.Views;
using System.IO;
using Amazon.S3.Transfer;
using Amazon;
using FTCollectorApp.Model;
using FTCollectorApp.Utils;
using PCLStorage;
using Amazon.S3.Model;
using Amazon.S3;
using FTCollectorApp.Service;

namespace FTCollectorApp.View.Utils
{
    public partial class CameraViewPage : BasePage
	{
		// Note: not all options of the CameraView are on this page, make sure to discover them for yourself!
		string fullpathFile;
		private readonly IAmazonS3 _awsS3Client;
		public CameraViewPage()
		{
			InitializeComponent();
			BindingContext = this;
			zoomLabel.Text = string.Format("Zoom: {0}", zoomSlider.Value);
			_awsS3Client = new AmazonS3Client(Constants.ACCES_KEY_ID, Constants.SECRET_ACCESS_KEY, RegionEndpoint.GetBySystemName(Constants.BUCKET_REGION));

		}

		void ZoomSlider_ValueChanged(object? sender, ValueChangedEventArgs e)
		{
			cameraView.Zoom = (float)zoomSlider.Value;
			zoomLabel.Text = string.Format("Zoom: {0}", Math.Round(zoomSlider.Value));
		}

		/*void VideoSwitch_Toggled(object? sender, ToggledEventArgs e)
		{
			var captureVideo = e.Value;

			if (captureVideo)
				cameraView.CaptureMode = CameraCaptureMode.Video;
			else
				cameraView.CaptureMode = CameraCaptureMode.Photo;

			previewPicture.IsVisible = !captureVideo;

			doCameraThings.Text = e.Value ? "Start Recording"
				: "Snap Picture";
		}*/

		// You can also set it to Default and External
		void FrontCameraSwitch_Toggled(object? sender, ToggledEventArgs e)
			=> cameraView.CameraOptions = e.Value ? CameraOptions.Front : CameraOptions.Back;

		// You can also set it to Torch (always on) and Auto
		void FlashSwitch_Toggled(object? sender, ToggledEventArgs e)
			=> cameraView.FlashMode = e.Value ? CameraFlashMode.On : CameraFlashMode.Off;

		void DoCameraThings_Clicked(object? sender, EventArgs e)
		{
			cameraView.Shutter();
			var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
			player.Load("camera.mp3");
			player.Play();
			//doCameraThings.Text = cameraView.CaptureMode == CameraCaptureMode.Video
			//	? "Stop Recording"
			//	: "Snap Picture";
		}

		void CameraView_OnAvailable(object? sender, bool e)
		{
			if (e)
			{
				zoomSlider.Value = cameraView.Zoom;
				var max = cameraView.MaxZoom;
				if (max > zoomSlider.Minimum && max > zoomSlider.Value)
					zoomSlider.Maximum = max;
				else
					zoomSlider.Maximum = zoomSlider.Minimum + 1; // if max == min throws exception
			}

			doCameraThings.IsEnabled = e;
			zoomSlider.IsEnabled = e;
		}


		bool isBusy = false;
		public bool IsBusy
        {
			get => isBusy;
            set
            {
				if (isBusy == value)
					return;
				isBusy = value;
				OnPropertyChanged(nameof(IsBusy));
            }

		}


		bool isEnableBtn = true;
		public bool IsEnableBtn
		{
			get => isEnableBtn;
			set
			{
				if (isEnableBtn == value)
					return;
				isEnableBtn = value;
				OnPropertyChanged(nameof(IsEnableBtn));
			}

		}

		async void CameraView_MediaCaptured(object? sender, MediaCapturedEventArgs e)
		{

			var fileTransferUtility = new TransferUtility(Constants.ACCES_KEY_ID, Constants.SECRET_ACCESS_KEY, RegionEndpoint.USEast2);
			try
			{
				var pictnaming = $"{Session.OwnerName}_{Session.tag_number}_{Session.current_page}_{Session.lattitude2}_{Session.longitude2}_{DateTime.Now.ToString("yyyy-M-d_HH-mm-ss")}_{Session.ownerkey}.png";
				IFolder rootFolder = FileSystem.Current.LocalStorage;
				IFolder folder = await rootFolder.CreateFolderAsync("images",
					CreationCollisionOption.OpenIfExists);
				IFile file = await folder.CreateFileAsync(pictnaming,
					CreationCollisionOption.ReplaceExisting);
				//await file.WriteAllTextAsync(e.ImageData)
				
				//fullpathFile = Path.Combine(App.ImageFileLocation, pictnaming);
				Console.WriteLine($"Start capture stream from {file.Path.ToString()}");
				//using (FileStream fs = new FileStream(folder.Path, FileMode.Create, FileAccess.Write))
				using(var fs = await file.OpenAsync(PCLStorage.FileAccess.ReadAndWrite))
				{
					await fs.WriteAsync(e.ImageData, 0, e.ImageData.Length);
				}
				
				IsBusy = true;
				IsEnableBtn = false;
				await fileTransferUtility.UploadAsync(file.Path.ToString(), Constants.BUCKET_NAME);
				IsEnableBtn = true;
				IsBusy = false;

				if (IsFileExists(pictnaming))
				{
					var result = await Application.Current.MainPage.DisplayAlert("Upload OK", pictnaming + "\nUploading DONE ", "TAKE MORE PICT", "BACK TO PAGE");
					if (!result)
					{
						var KVPair = keyvaluepair(pictnaming);
						await CloudDBService.PostPictureSave(KVPair); // async upload to AWS table
						await Application.Current.MainPage.Navigation.PopAsync();
					}
					else
						return;

				}
				else
				{
					Console.WriteLine();
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


		private void btnCamera(object sender, EventArgs e)
        {

        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {

        }

    }
}