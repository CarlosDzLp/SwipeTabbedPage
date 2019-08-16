using System;
using SwipeTabbedPage.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly:ExportRenderer(typeof(TabbedPage),typeof(SwipeTabbedPageRenderer))]
namespace SwipeTabbedPage.iOS
{
    public class SwipeTabbedPageRenderer : TabbedRenderer
    {
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            NativeView.AddGestureRecognizer(
                new UISwipeGestureRecognizer
                (() => SelectedNextTab(1))
                {
                    Direction = UISwipeGestureRecognizerDirection.Left,
                    ShouldRecognizeSimultaneously = ShouldRecognizeSimultaneously
                });

            NativeView.AddGestureRecognizer(
                new UISwipeGestureRecognizer
                (() => SelectedNextTab(-1))
                {
                    Direction = UISwipeGestureRecognizerDirection.Right,
                    ShouldRecognizeSimultaneously = ShouldRecognizeSimultaneously
                });
        }
        void SelectedNextTab(int direction)
        {
            int nextIndex = MultiPage<Page>.GetIndex(Tabbed.CurrentPage) + direction;
            if (nextIndex < 0 || nextIndex >= Tabbed.Children.Count) return;
            var nextPage = Tabbed.Children[nextIndex];
            UIView.Transition(
                Platform.GetRenderer(Tabbed.CurrentPage).NativeView,
                Platform.GetRenderer(nextPage).NativeView,
                0.05,
                UIViewAnimationOptions.TransitionCrossDissolve,
                null);
            Tabbed.CurrentPage = nextPage;
        }
        private static bool ShouldRecognizeSimultaneously(UIGestureRecognizer gestureRecognizer,
            UIGestureRecognizer otherGestureRecognizer)
        {
            return gestureRecognizer != otherGestureRecognizer;
        }

    }
}
