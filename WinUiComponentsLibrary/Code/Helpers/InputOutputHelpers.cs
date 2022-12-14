using AppHelpersStd20;
using AppHelpersStd20.Extensions;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using WinRT.Interop;

namespace WinUiComponentsLibrary.Code.Helpers
{
    /// <summary>
    /// Encapsule un ensemble de méthode visant à faciliter la gestion des fichiers, des flux et des dossiers sous WinUi
    /// </summary>
    public class InputOutputHelpers
    {
        /// <summary>
        /// Affiche une boite de dialogue permettant à l'utilisateur de sélectionner un fichier avec une contrainte imposée.
        /// </summary>
        /// <param name="fileTypeFilter"></param>
        /// <param name="pickerViewMode"></param>
        /// <param name="pickerLocationId"></param>
        /// <returns>Un objet <see cref="StorageFile"/> </returns>
        public static async Task<StorageFile> OpenFileAsync(Window window, IEnumerable<string> fileTypeFilter, PickerViewMode pickerViewMode = PickerViewMode.Thumbnail, PickerLocationId pickerLocationId = PickerLocationId.Downloads)
        {
            try
            {
                if (window == null)
                    throw new ArgumentNullException(nameof(window));

                if (fileTypeFilter == null || !fileTypeFilter.Any())
                    throw new ArgumentNullException(nameof(fileTypeFilter));

                FileOpenPicker openPicker = new();
                // Retrieve the window handle (HWND) of the current WinUI 3 window.
                IntPtr hWnd = WindowNative.GetWindowHandle(window);

                // Initialize the folder picker with the window handle (HWND).
                InitializeWithWindow.Initialize(openPicker, hWnd);

                openPicker.ViewMode = pickerViewMode;
                openPicker.SuggestedStartLocation = pickerLocationId;

                foreach (var fileType in fileTypeFilter)
                {
                    openPicker.FileTypeFilter.Add(fileType); //ex: ".jpg";
                }

                StorageFile file = await openPicker.PickSingleFileAsync();
                if (file == null)
                {
                    throw new Exception("Le fichier n'a pas pû être récupérer.");
                }

                return file;
            }
            catch (Exception ex)
            {
                Logs.Log(className: nameof(InputOutputHelpers), exception: ex);
                return null;
            }
        }

        /// <summary>
        /// Affiche une boite de dialogue permettant à l'utilisateur de sélectionner plusieurs fichiers avec une contrainte imposée.
        /// </summary>
        /// <param name="fileTypeFilter"></param>
        /// <param name="pickerViewMode"></param>
        /// <param name="pickerLocationId"></param>
        /// <returns>Un objet <see cref="StorageFile"/></returns>
        public static async Task<IEnumerable<StorageFile>> OpenFilesAsync(Window window, IEnumerable<string> fileTypeFilter, PickerViewMode pickerViewMode = PickerViewMode.Thumbnail, PickerLocationId pickerLocationId = PickerLocationId.Downloads)
        {
            try
            {
                if (window == null)
                    throw new ArgumentNullException(nameof(window));

                if (fileTypeFilter == null || !fileTypeFilter.Any())
                    throw new ArgumentNullException(nameof(fileTypeFilter));

                FileOpenPicker openPicker = new();

                // Retrieve the window handle (HWND) of the current WinUI 3 window.
                IntPtr hWnd = WindowNative.GetWindowHandle(window);

                // Initialize the folder picker with the window handle (HWND).
                WinRT.Interop.InitializeWithWindow.Initialize(openPicker, hWnd);

                openPicker.ViewMode = pickerViewMode;
                openPicker.SuggestedStartLocation = pickerLocationId;

                foreach (var fileType in fileTypeFilter)
                {
                    openPicker.FileTypeFilter.Add(fileType); //ex: ".jpg";
                }

                IReadOnlyList<StorageFile> files = await openPicker.PickMultipleFilesAsync();
                if (files == null || files.Count == 0)
                {
                    throw new Exception("Les fichiers n'ont pas pû être récupéré.");

                }

                return files;
            }
            catch (Exception ex)
            {
                Logs.Log(className: nameof(InputOutputHelpers), exception: ex);
                return Enumerable.Empty<StorageFile>();
            }
        }

        /// <summary>
        /// Affiche une boite de dialogue permettant à l'utilisateur de sauvegarder un fichier sur un périphérique de stockage avec une contrainte imposée.
        /// </summary>
        /// <param name="suggestedFileName"></param>
        /// <param name="fileTypeFilter"></param>
        /// <param name="pickerLocationId"></param>
        /// <param name="_window"></param>
        /// <returns>Un objet <see cref="StorageFile"/></returns>
        public static async Task<StorageFile> SaveFileAsync(Window window, string suggestedFileName, Dictionary<string, IList<string>> fileTypeFilter, PickerLocationId pickerLocationId = PickerLocationId.Downloads)
        {
            try
            {
                if (window == null)
                    throw new ArgumentNullException(nameof(window));

                if (fileTypeFilter == null || !fileTypeFilter.Any())
                    throw new ArgumentNullException(nameof(fileTypeFilter));

                FileSavePicker savePicker = new();

                // Retrieve the window handle (HWND) of the current WinUI 3 window.
                IntPtr hWnd = WindowNative.GetWindowHandle(window);

                // Initialize the folder picker with the window handle (HWND).
                WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hWnd);

                savePicker.SuggestedStartLocation = pickerLocationId;
                savePicker.SuggestedFileName = suggestedFileName;

                foreach (var fileType in fileTypeFilter)
                {
                    savePicker.FileTypeChoices.Add(fileType); //ex: ".jpg";
                }
                StorageFile file = await savePicker.PickSaveFileAsync();
                if (file == null)
                {
                    throw new Exception("Le fichier n'a pas pû être récupérer.");
                }

                return file;
            }
            catch (Exception ex)
            {
                Logs.Log(className: nameof(InputOutputHelpers), exception: ex);
                return null;
            }
        }

        /// <summary>
        /// Affiche une boite de dialogue permettant à l'utilisateur de sélectionner un dossier.
        /// </summary>
        /// <param name="pickerViewMode"></param>
        /// <param name="pickerLocationId"></param>
        /// <returns>Un objet <see cref="StorageFolder"/> </returns>
        public static async Task<StorageFolder> OpenFolderAsync(Window window, PickerViewMode pickerViewMode = PickerViewMode.Thumbnail, PickerLocationId pickerLocationId = PickerLocationId.Downloads)
        {
            try
            {
                if (window == null)
                    throw new ArgumentNullException(nameof(window));

                FolderPicker folderPicker = new();

                // Retrieve the window handle (HWND) of the current WinUI 3 window.
                IntPtr hWnd = WindowNative.GetWindowHandle(window);

                // Initialize the folder picker with the window handle (HWND).
                InitializeWithWindow.Initialize(folderPicker, hWnd);

                folderPicker.FileTypeFilter.Add("*");
                folderPicker.SuggestedStartLocation = pickerLocationId;
                folderPicker.ViewMode = pickerViewMode;

                StorageFolder folder = await folderPicker.PickSingleFolderAsync();
                if (folder == null)
                {
                    throw new Exception("Le dossier n'a pas pû être récupérer.");
                }

                return folder;
            }
            catch (Exception ex)
            {
                Logs.Log(className: nameof(InputOutputHelpers), exception: ex);
                return null;
            }
        }

        public static async Task<MediaSource> VideoFromFileAsync(string videoFile)
        {
            try
            {
                if (videoFile.IsStringNullOrEmptyOrWhiteSpace())
                {
#warning "Logging"
                    return null;
                }

                MediaSource mediaSource = null;

                if (videoFile.StartsWith("ms-appx:///"))
                {
                    Uri uri = new (videoFile);
                    mediaSource = MediaSource.CreateFromUri(uri);

                    return mediaSource;
                }
                else if (videoFile.StartsWith("http") || videoFile.StartsWith("ftp"))
                {
                    //var uri = new System.Uri(videoFile);
                    //var randomAccessStreamReference = RandomAccessStreamReference.CreateFromUri(uri);
                    //using (IRandomAccessStream stream = await randomAccessStreamReference.OpenReadAsync())
                    //{
                    //    await image.SetSourceAsync(stream);
                    //}
                    //return image;
                }
                else if (System.IO.File.Exists(videoFile))
                {
                    StorageFile storageFile = await StorageFile.GetFileFromPathAsync(videoFile);
                    mediaSource = MediaSource.CreateFromStorageFile(storageFile);
                    return mediaSource;
                }

                return null;
            }
            catch (Exception ex)
            {
                Logs.Log(className: nameof(InputOutputHelpers), exception: ex);
                return null;
            }

        }

        public async static Task<byte[]> FileToBytesAsync(string file)
        {
            if (file.IsStringNullOrEmptyOrWhiteSpace() || !System.IO.File.Exists(file))
                return null;

            using FileStream fileStream = File.OpenRead(file);
            using MemoryStream memoryStream = new();
            await fileStream.CopyToAsync(memoryStream);
            byte[] byteArray = memoryStream.ToArray();

            return byteArray;
        }


        public async static Task<byte[]> FileToBytesAsync(StorageFile storageFile)
        {
            if (storageFile == null)
                return null;

            using (Stream stream = await storageFile.OpenStreamForReadAsync())
            {
                using (var memoryStream = new MemoryStream())
                {

                    await stream.CopyToAsync(memoryStream);
                    byte[] result = memoryStream.ToArray();
                    return result;
                }
            }
        }

        public async static Task<byte[]> ImageToBytesAsync(BitmapImage image)
        {
            if (image == null || image.UriSource == null)
                return null;

            RandomAccessStreamReference streamRef = RandomAccessStreamReference.CreateFromUri(image.UriSource);
            IRandomAccessStreamWithContentType streamWithContent = await streamRef.OpenReadAsync();
            byte[] buffer = new byte[streamWithContent.Size];
            await streamWithContent.ReadAsync(buffer.AsBuffer(), (uint)streamWithContent.Size, InputStreamOptions.None);
            return buffer;
        }

        public async static Task<BitmapImage> ImageFromBytes(byte[] bytes)
        {
            BitmapImage image = new();
            using (InMemoryRandomAccessStream stream = new())
            {
                await stream.WriteAsync(bytes.AsBuffer());
                stream.Seek(0);
                await image.SetSourceAsync(stream);
            }
            return image;
        }

        public static async Task<BitmapImage> BitmapImageFromFileAsync(string imageFileName)
        {
            try
            {
                if (imageFileName.IsStringNullOrEmptyOrWhiteSpace())
                {
#warning "Logging"
                    return null;
                }

                BitmapImage image = new ();

                if (imageFileName.StartsWith("ms-appx:///"))
                {
                    var uri = new System.Uri(imageFileName);
                    StorageFile storageFile = await StorageFile.GetFileFromApplicationUriAsync(uri);
                    using (IRandomAccessStream stream = await storageFile.OpenAsync(FileAccessMode.Read))
                    {
                        await image.SetSourceAsync(stream);
                    }
                    image.UriSource = uri;
                    return image;
                    //return new BitmapImage(new Uri(imageFileName));
                }
                else if (imageFileName.StartsWith("http") || imageFileName.StartsWith("ftp"))
                {
                    var uri = new System.Uri(imageFileName);
                    var randomAccessStreamReference = RandomAccessStreamReference.CreateFromUri(uri);
                    using (IRandomAccessStream stream = await randomAccessStreamReference.OpenReadAsync())
                    {
                        await image.SetSourceAsync(stream);
                    }
                    image.UriSource = uri;
                    return image;
                }
                else if (System.IO.File.Exists(imageFileName))
                {
                    StorageFile storageFile = await StorageFile.GetFileFromPathAsync(imageFileName);
                    using (IRandomAccessStream stream = await storageFile.OpenAsync(FileAccessMode.Read))
                    {
                        await image.SetSourceAsync(stream);
                    }

                    return image;
                }

                return null;
            }
            catch (Exception ex)
            {
                Logs.Log(className: nameof(InputOutputHelpers), exception: ex);
                return null;
            }

        }

        public static async Task<BitmapImage> BitmapImageFromFileAsync(StorageFile imageFile)
        {
            try
            {
                if (imageFile == null || !imageFile.IsAvailable)
                {
#warning "Logging"
                    return null;
                }

                BitmapImage image = new();

                using (IRandomAccessStream stream = await imageFile.OpenAsync(FileAccessMode.Read))
                {
                    await image.SetSourceAsync(stream);
                }
                return image;
            }
            catch (Exception ex)
            {
                Logs.Log(className: nameof(InputOutputHelpers), exception: ex);
                return null;
            }
        }

        public static async Task<ImageSource> ImageFromFileAsync(string imageFileName)
        {
            try
            {
                if (imageFileName.IsStringNullOrEmptyOrWhiteSpace())
                {
#warning "Logging"
                    return null;
                }

                ImageSource imageSource = null;

                if (imageFileName.StartsWith("ms-appx:///"))
                {
                    var uri = new System.Uri(imageFileName);
                    StorageFile storageFile = await StorageFile.GetFileFromApplicationUriAsync(uri);
                    using (IRandomAccessStream stream = await storageFile.OpenAsync(FileAccessMode.Read))
                    {
                        if (System.IO.Path.GetExtension(storageFile.Name)?.ToLower() == ".svg")
                        {
                            imageSource = new SvgImageSource();
                            await ((SvgImageSource)imageSource).SetSourceAsync(stream);
                            ((SvgImageSource)imageSource).UriSource = uri;
                        }
                        else
                        {
                            imageSource = new BitmapImage();
                            await ((BitmapImage)imageSource).SetSourceAsync(stream);
                            ((BitmapImage)imageSource).UriSource = uri;
                        }
                    }
                    return imageSource;
                }
                else if (imageFileName.StartsWith("http") || imageFileName.StartsWith("ftp"))
                {
                    Uri uri = new System.Uri(imageFileName);
                    RandomAccessStreamReference randomAccessStreamReference = RandomAccessStreamReference.CreateFromUri(uri);
                    using (IRandomAccessStream stream = await randomAccessStreamReference.OpenReadAsync())
                    {
                        if (System.IO.Path.GetExtension(imageFileName)?.ToLower() == ".svg")
                        {
                            imageSource = new SvgImageSource();
                            await ((SvgImageSource)imageSource).SetSourceAsync(stream);
                            ((SvgImageSource)imageSource).UriSource = uri;
                        }
                        else
                        {
                            imageSource = new BitmapImage();
                            await ((BitmapImage)imageSource).SetSourceAsync(stream);
                            ((BitmapImage)imageSource).UriSource = uri;
                        }
                    }
                    return imageSource;
                }
                else if (System.IO.File.Exists(imageFileName))
                {
                    System.Uri uri = new System.Uri(imageFileName);
                    StorageFile storageFile = await StorageFile.GetFileFromPathAsync(imageFileName);
                    using (IRandomAccessStream stream = await storageFile.OpenAsync(FileAccessMode.Read))
                    {
                        if (System.IO.Path.GetExtension(imageFileName)?.ToLower() == ".svg")
                        {
                            imageSource = new SvgImageSource();
                            await ((SvgImageSource)imageSource).SetSourceAsync(stream);
                            ((SvgImageSource)imageSource).UriSource = uri;
                        }
                        else
                        {
                            imageSource = new BitmapImage();
                            await ((BitmapImage)imageSource).SetSourceAsync(stream);
                            ((BitmapImage)imageSource).UriSource = uri;
                        }
                    }

                    return imageSource;
                }

                return null;
            }
            catch (Exception ex)
            {
                Logs.Log(className: nameof(InputOutputHelpers), exception: ex);
                return null;
            }

        }

    }
}
