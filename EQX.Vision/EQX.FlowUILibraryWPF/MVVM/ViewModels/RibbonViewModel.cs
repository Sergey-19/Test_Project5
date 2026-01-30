using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EQX.FlowUILibraryWPF.MVVM.ViewModels
{
    public class RibbonViewModel : ObservableObject
    {
        public EventHandler ShowAddFlowWindowEvent;
        public EventHandler SaveAsFlowEvent;
        public EventHandler SaveFlowEvent;
        public EventHandler LoadFlowEvent;
        public ICommand AddNewFlowCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    ShowAddFlowWindowEvent?.Invoke(this, EventArgs.Empty);
                });
            }
        }

        public ICommand SaveAsFlowCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    SaveAsFlowEvent?.Invoke(this, EventArgs.Empty);
                });
            }
        }

        public ICommand SaveFlowCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    SaveFlowEvent?.Invoke(this, EventArgs.Empty);
                });
            }
        }

        public ICommand OpenFlowCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    LoadFlowEvent?.Invoke(this, EventArgs.Empty);
                });
            }
        }
    }
}
