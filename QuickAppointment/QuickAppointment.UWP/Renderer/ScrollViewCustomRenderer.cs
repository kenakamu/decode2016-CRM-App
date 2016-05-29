using QuickAppointment.UWP.Renderer;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(ScrollView), typeof(ScrollViewCustomRenderer))]

namespace QuickAppointment.UWP.Renderer
{
    /// <summary>
    /// ScrollViewer 用のカスタムレンダラー
    /// UWP は ScrollView をタッチ操作でスクロールできないため、このカスタムレンダラーで処理
    /// </summary>
    public class ScrollViewCustomRenderer : ScrollViewRenderer
    {
        double currentX;
        double movedX;
        double currentY;
        double movedY;
        ScrollViewer sv;
        ScrollView xsv;

        protected override void OnElementChanged(ElementChangedEventArgs<ScrollView> e)
        {
            base.OnElementChanged(e);
            xsv = e.NewElement as ScrollView;
            sv = Control as ScrollViewer;
            sv.ManipulationMode = Windows.UI.Xaml.Input.ManipulationModes.All;
            sv.ManipulationStarted += Sv_ManipulationStarted;
            sv.ManipulationCompleted += Sv_ManipulationCompleted;
        }

        private void Sv_ManipulationDelta(object sender, Windows.UI.Xaml.Input.ManipulationDeltaRoutedEventArgs e)
        {
            movedX = e.Position.X;
            movedY = e.Position.Y;
            if (xsv.Orientation == ScrollOrientation.Horizontal)
                xsv.ScrollToAsync((movedX - currentX) * -1, 0, true);
            else
                xsv.ScrollToAsync(0, (movedY + currentY) * -1, true);
        }

        private void Sv_ManipulationCompleted(object sender, Windows.UI.Xaml.Input.ManipulationCompletedRoutedEventArgs e)
        {
            movedX = e.Position.X;
            movedY = e.Position.Y;
            if (xsv.Orientation == ScrollOrientation.Horizontal)
                xsv.ScrollToAsync((movedX - currentX) * -1, 0, true);
            else
                xsv.ScrollToAsync(0, (movedY + currentY) * -1, true);
        }

        private void Sv_ManipulationStarted(object sender, Windows.UI.Xaml.Input.ManipulationStartedRoutedEventArgs e)
        {
            currentX = e.Position.X;
            currentY = e.Position.Y;
        }
    }
}
