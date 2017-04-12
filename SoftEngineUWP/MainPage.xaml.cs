using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using SharpDX;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SoftEngineUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Device device;
        Mesh[] meshes;
        Camera mera = new Camera();

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Choose the back buffer resolution here
            WriteableBitmap bmp = new WriteableBitmap(640, 480);

            // Our Image XAML control
            frontBuffer.Source = bmp;

            device = new Device(bmp);
            meshes = await device.LoadJSONFileAsync("ms-appx:///Assets/monkey.babylon");
            mera.Position = new Vector3(0, 0, 10.0f);
            mera.Target = Vector3.Zero;

            // Registering to the XAML rendering loop
            CompositionTarget.Rendering += CompositionTarget_Rendering;
        }

        private void CompositionTarget_Rendering(object sender, object e)
        {
            device.Clear(0, 0, 0, 255);

            foreach (var mesh in meshes)
            {
                // rotating slightly the meshes during each frame rendered
                mesh.Rotation = new Vector3(mesh.Rotation.X + 0.01f, mesh.Rotation.Y + 0.01f, mesh.Rotation.Z);
            }

            // Doing the various matrix operations
            device.Render(mera, meshes);
            // Flushing the back buffer into the front buffer
            device.Present();
        }
    }
}
