using DementiaHelper.Model;
using DementiaHelper.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DementiaHelper.PageModels
{
    [ImplementPropertyChanged]
    public class ImageGalleryPageModel : FreshMvvm.FreshBasePageModel
    {

        public ICommand PreviewImageCommand { get; set; }
        public ICommand CameraCommand { get; protected set; }

        public ImageGalleryPageModel()
        {
            Images = new ObservableCollection<GalleryImage>();
            CameraCommand = new Command(async () => await ExecuteCameraCommand(), () => CanExecuteCameraCommand());
            PreviewImageCommand = new Command<Guid>((img) => {
                var image = Images.Single(x => x.ImageId == img).OrgImage;
                PreviewImage = ImageSource.FromStream(() => new MemoryStream(image));
            });
        }

        public ObservableCollection<GalleryImage> Images { get; set; }

        public ImageSource PreviewImage { get; set; }

        public bool CanExecuteCameraCommand()
        {
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                return false;
            }
            return true;
        }

        public async Task ExecuteCameraCommand()
        {
            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions { PhotoSize = PhotoSize.Small });

            if (file == null)
                return;


            byte[] imageAsBytes = null;
            using (var memoryStream = new MemoryStream())
            {
                file.GetStream().CopyTo(memoryStream);
                file.Dispose();
                imageAsBytes = memoryStream.ToArray();
            }

            var resizer = DependencyService.Get<IImageResize>();

            imageAsBytes = resizer.ResizeImage(imageAsBytes, 1080, 1080);

            var imageSource = ImageSource.FromStream(() => new MemoryStream(imageAsBytes));

            Images.Add(new GalleryImage { Source = imageSource, OrgImage = imageAsBytes });
            return;
        }
    }
}
