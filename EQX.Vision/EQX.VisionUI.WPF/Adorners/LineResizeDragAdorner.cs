using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EQX.VisionUI.WPF.Adorners
{
    public class LineResizeDragAdorner : Adorner
    {
        #region Constants
        readonly Size ThumbSize = new Size(10, 10);
        readonly Size MinSize = new Size(5, 5);
        readonly int ThumbMargin = -1;
        #endregion
        VisualCollection AdornerVisuals;


        Thumb thumb_Start;

        Thumb thumb_End;

        Thumb thumb_Center;

        TextBlock label_Name;

        public string Description { get; set; }

        public event EventHandler AdornerArranged;

        void ThumbInit()
        {
            thumb_Start = new Thumb()
            {
                Tag = "Start",
                Visibility = Visibility.Visible,
            };
            thumb_End = new Thumb()
            {
                Tag = "End",
                Visibility = Visibility.Visible,
            };

            thumb_Center = new Thumb()
            {
                Style = (Style)FindResource("LineBorderThumbStyle"),
                Tag = "Center",
                Cursor = Cursors.SizeAll,
                Focusable = true,
            };

            thumb_Start.DragDelta += Thumb_DragDelta;
            thumb_End.DragDelta += Thumb_DragDelta;
            thumb_Center.DragDelta += Thumb_DragDelta;
        }
        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var element = (FrameworkElement)AdornedElement;

            double horizontalChange = e.HorizontalChange;
            double vericalChange = e.VerticalChange;

            switch (((Thumb)sender).Tag)
            {
                case "Start":
                    (element as Line).X1 += e.HorizontalChange;
                    (element as Line).Y1 += e.VerticalChange;

                    break;
                case "End":
                    (element as Line).X2 += e.HorizontalChange;
                    (element as Line).Y2 += e.VerticalChange;

                    break;
                case "Center":
                    break;
                default:
                    return;
            }
        }

        public LineResizeDragAdorner(UIElement adornedElement) :
            base(adornedElement)
        {
            ThumbInit();
            LabelInit();

            AdornerVisuals = new VisualCollection(this)
            {
                label_Name,
                thumb_Center,

                thumb_Start,
                thumb_End
            };
        }
        void LabelInit()
        {
            label_Name = new TextBlock();

            label_Name.FontSize = 10;
            label_Name.Foreground = Brushes.Black;
        }

        #region Override(s)
        protected override Visual GetVisualChild(int index)
        {
            return AdornerVisuals[index];
        }

        protected override int VisualChildrenCount => AdornerVisuals.Count;

        protected override Size ArrangeOverride(Size finalSize)
        {
            //thumb_Center.Width = AdornedElement.DesiredSize.Width + (ThumbSize.Width) * 2;
            //thumb_Center.Height = AdornedElement.DesiredSize.Height + (ThumbSize.Height) * 2;
            thumb_Center.Arrange(
                new Rect(
                    x: 0,
                    y: 0,
                    width: Math.Max(AdornedElement.DesiredSize.Width + ThumbMargin * 2, 0),
                    height: Math.Max(AdornedElement.DesiredSize.Height + ThumbMargin * 2, 0))
                );
            thumb_Start.Arrange(
                new Rect(
                    x: (AdornedElement as Line).X1 - ThumbSize.Width / 2,
                    y: (AdornedElement as Line).Y1 - ThumbSize.Width / 2,
                    width: ThumbSize.Width,
                    height: ThumbSize.Height)
                );
            thumb_End.Arrange(
                new Rect(
                    x: (AdornedElement as Line).X2 - ThumbSize.Width / 2,
                    y: (AdornedElement as Line).Y2 - ThumbSize.Width / 2,
                    width: ThumbSize.Width,
                    height: ThumbSize.Height)
                );

            label_Name.Text = Description;
            label_Name.Arrange(
                new Rect(
                    x: (AdornedElement as Line).X1 - 10,
                    y: (AdornedElement as Line).Y1 - 10,
                    width: AdornedElement.DesiredSize.Width,
                    height: 50)
                );

            OnAdornerArranged();

            return base.ArrangeOverride(finalSize);
        }

        private void OnAdornerArranged()
        {
            AdornerArranged?.Invoke(this, EventArgs.Empty);
        }
        #endregion

    }
}
