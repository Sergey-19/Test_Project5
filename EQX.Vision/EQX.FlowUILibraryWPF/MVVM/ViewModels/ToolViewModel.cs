using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EQX.Core.Vision.Tool;
using EQX.FlowUILibraryWPF.Models;
using EQX.FlowUILibraryWPF.MVVM.Views;
using Newtonsoft.Json;
using System.Windows;
using System.Windows.Input;

namespace EQX.FlowUILibraryWPF.MVVM.ViewModels
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ToolViewModel : ObservableObject
    {
        #region Events
        public EventHandler RunEvent;
        public EventHandler OnToolMouseMoveEvent;
        public EventHandler PointInputMouseDownEvent;
        public EventHandler PointInputMouseMoveEvent;
        public EventHandler PointOutputMouseDownEvent;
        public EventHandler PointOutputMouseMoveEvent;
        public EventHandler DeleteToolEvent;
        #endregion

        #region Commands
        public ICommand OnToolMouseDownCommand { get; set; }
        public ICommand OnToolMouseMoveCommand { get; set; }
        public ICommand PointInputMouseLeftButtonDownCommand { get;set; }
        public ICommand PointInputMouseMoveCommand { get; set; }
        public ICommand PointOutputMouseLeftButtonDownCommand { get; set; }
        public ICommand PointOutputMouseMoveCommand { get; set; }
        #endregion

        #region Properties
        [JsonProperty]
        public IVisionTool VisionTool { get; }
        [JsonProperty]
        public PositionTool Position { get; set; }
        public string ToolName => VisionTool.Name;
        [JsonProperty]
        public int Id { get; set; }
        #endregion

        #region Constructor
        public ToolViewModel(IVisionTool visionTool)
        {
            Position = new PositionTool();
            VisionTool = visionTool;

            OnToolMouseDownCommand = new RelayCommand<MouseButtonEventArgs>(OnToolMouseDown);
            OnToolMouseMoveCommand = new RelayCommand<MouseEventArgs>(OnToolMouseMove);
            PointInputMouseLeftButtonDownCommand = new RelayCommand<MouseButtonEventArgs>(PointInputMouseLeftButtonDown);
            PointInputMouseMoveCommand = new RelayCommand<MouseEventArgs>(PointInputMouseMove);
            PointOutputMouseLeftButtonDownCommand = new RelayCommand<MouseButtonEventArgs>(PointOutputMouseLeftButtonDown);
            PointOutputMouseMoveCommand = new RelayCommand<MouseEventArgs>(PointOutputMouseMove);
        }

        [Newtonsoft.Json.JsonConstructor]
        public ToolViewModel(IVisionTool visionTool, PositionTool position)
        {
            VisionTool = visionTool;
            Position = position;

            OnToolMouseDownCommand = new RelayCommand<MouseButtonEventArgs>(OnToolMouseDown);
            OnToolMouseMoveCommand = new RelayCommand<MouseEventArgs>(OnToolMouseMove);
            PointInputMouseLeftButtonDownCommand = new RelayCommand<MouseButtonEventArgs>(PointInputMouseLeftButtonDown);
            PointInputMouseMoveCommand = new RelayCommand<MouseEventArgs>(PointInputMouseMove);
            PointOutputMouseLeftButtonDownCommand = new RelayCommand<MouseButtonEventArgs>(PointOutputMouseLeftButtonDown);
            PointOutputMouseMoveCommand = new RelayCommand<MouseEventArgs>(PointOutputMouseMove);
        }
        #endregion

        #region Public Methods
        public Point GetCenterInputPoint(int index)
        {
            if (VisionTool.Inputs.Keys?.Count() >= VisionTool.Outputs.Keys?.Count())
            {
                return new Point(Position.Left - 7.5 + 2, Position.Top + 15 * (index) + 20 * (index - 1) + 15 / 2 + 30);
            }
            else
            {
                return new Point(Position.Left - 7.5 + 2, (double)(Position.Top + ((VisionTool.Outputs.Keys!.Count()) * 35 + 7 - VisionTool.Inputs.Keys!.Count() * 35) / 2.0 + 35.0 / 2 * index + 20 * (index - 1) + 30));
            }
        }
        public Point GetCenterOutputPoint(int index)
        {
            if (VisionTool.Inputs.Keys?.Count() <= VisionTool.Outputs.Keys?.Count())
            {
                return new Point(Position.Left + 200 + 7.5 - 2, Position.Top + 15 * (index) + 20 * (index - 1) + 15 / 2 + 30);
            }
            else
            {
                return new Point(Position.Left + 200 + 7.5 - 2, (double)(Position.Top + ((VisionTool.Inputs.Keys!.Count()) * 35 + 7 - VisionTool.Outputs.Keys!.Count() * 35) / 2.0 + 35.0 / 2 * index + 20 * (index - 1) + 30));
            }
        }
        public ICommand DeleteToolCommand
        {
            get
            {
                return new RelayCommand(DeleteTool);
            }
        }
        #endregion

        #region Private Methods
        private void OnToolMouseDown(MouseButtonEventArgs e)
        {
            if(e.ClickCount == 2)
            {
                ToolSettingWindowView toolSettingWindow = new ToolSettingWindowView();
                toolSettingWindow.DataContext = new ToolSettingWindowViewModel(VisionTool);
                (toolSettingWindow.DataContext as ToolSettingWindowViewModel).RunEvent += Run;
                toolSettingWindow.Show();
            }
        }
        private void OnToolMouseMove(MouseEventArgs e)
        {
            OnToolMouseMoveEvent?.Invoke(this, e);
            e.Handled = true;
        }
        private void PointInputMouseLeftButtonDown(MouseEventArgs e)
        {
            PointInputMouseDownEvent?.Invoke(this, e);
            e.Handled = true;
        }

        private void PointInputMouseMove(MouseEventArgs e)
        {
            PointInputMouseMoveEvent?.Invoke(this, e);
            e.Handled = true;
        }

        private void PointOutputMouseLeftButtonDown(MouseEventArgs e)
        {
            PointOutputMouseDownEvent?.Invoke(this, e);
            e.Handled = true;
        }

        private void PointOutputMouseMove(MouseEventArgs e)
        {
            PointOutputMouseMoveEvent?.Invoke(this, e);
            e.Handled = true;
        }

        private void DeleteTool()
        {
            DeleteToolEvent?.Invoke(this, EventArgs.Empty);
        }
        private void Run(object sender, EventArgs e)
        {
            RunEvent?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Privates
        #endregion
    }
}
