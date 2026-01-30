using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EQX.Core.Vision.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EQX.FlowUILibraryWPF.MVVM.ViewModels
{
    public class ToolSettingWindowViewModel : ObservableObject
    {
        public EventHandler RunEvent;
        private IVisionTool _visionTool;
        public IVisionTool VisionTool
        {
            get 
            {
                return _visionTool; 
            }
            set 
            {
                _visionTool = value;
                OnPropertyChanged();
            }
        }

        public ToolSettingWindowViewModel(IVisionTool visionTool)
        {
            VisionTool = visionTool;
        }

        public ICommand RunCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    RunEvent?.Invoke(this, EventArgs.Empty);
                });
            }
        }
    }
}
