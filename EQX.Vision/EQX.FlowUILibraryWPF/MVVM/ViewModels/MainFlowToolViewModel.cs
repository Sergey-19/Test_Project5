using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EQX.FlowUILibraryWPF.MVVM.Views;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace EQX.FlowUILibraryWPF.MVVM.ViewModels
{
    public class MainFlowToolViewModel : ObservableObject
    {
        #region Properties
        public FlowViewModel CurrentFlow
        {
            get
            {
                return _currentFlow ?? (_currentFlow = new FlowViewModel("Default"));
            }
            set
            {
                _currentFlow = value;
                OnPropertyChanged(nameof(CurrentFlow));
            }
        }
        public ToolListViewModel ToolListVM
        {
            get
            {
                return _toolListVM ?? (_toolListVM = new ToolListViewModel());
            }
        }
        public RibbonViewModel RibbonVM
        {
            get
            {
                return _ribbonVM ?? (_ribbonVM = new RibbonViewModel());
            }
        }
        public ObservableCollection<FlowViewModel> FlowList { get; set; } = new ObservableCollection<FlowViewModel>() { new FlowViewModel("Default") };
        #endregion

        #region Constructor
        public MainFlowToolViewModel()
        {
            ToolListVM.AddToolEvent += AddToolEventHandler;

            RibbonVM.ShowAddFlowWindowEvent += ShowAddFlowWindowEventHandler;
            RibbonVM.SaveAsFlowEvent += SaveAsFlowEventHandler;
            RibbonVM.LoadFlowEvent += LoadFlowEventHandler;
            CurrentFlow = FlowList.First();
        }

        #endregion
        #region Commands
        public ICommand DeleteFlowCommand
        {
            get
            {
                return new RelayCommand<object>(execute: (o) =>
                {
                    FlowList.Remove((o as FlowViewModel));
                },
                canExecute: (o) => true);
            }
        }
        public ICommand RenameFlowCommand
        {
            get
            {
                return new RelayCommand<object>(execute: (o) =>
                {
                    AddFlowWindowView addFlowWindowView = new AddFlowWindowView();
                    if (addFlowWindowView.ShowDialog() == true)
                    {
                        if (addFlowWindowView.FlowName == null) return;

                        (o as FlowViewModel).FlowName = addFlowWindowView.FlowName;
                    }
                },
                canExecute: (o) => true);
            }
        }
        #endregion

        #region EventHandler
        private void AddToolEventHandler(object sender, EventArgs e)
        {
            if (CurrentFlow == null) return;
            CurrentFlow.AddTool((sender as Button).Content.ToString());
        }
        private void ShowAddFlowWindowEventHandler(object sender, EventArgs e)
        {
            AddFlowWindowView addFlowWindowView = new AddFlowWindowView();
            if (addFlowWindowView.ShowDialog() == true)
            {
                FlowList.Add(new FlowViewModel(addFlowWindowView.FlowName));
                CurrentFlow = FlowList.Last();
            }
        }
        private void SaveAsFlowEventHandler(object sender, EventArgs e)
        {
            CurrentFlow.SaveAs();
        }
        private void LoadFlowEventHandler(object sender, EventArgs e)
        {
            OpenFileDialog openfileDialog = new OpenFileDialog();
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                openfileDialog.Filter = "Text File |*.txt";
            }

            if (openfileDialog.ShowDialog() == true)
            {
                if (openfileDialog.FileNames.Length > 1) return;
                string serializationString = File.ReadAllText(openfileDialog.FileName);

                var settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                };
                FlowViewModel flowViewModel = JsonConvert.DeserializeObject<FlowViewModel>(serializationString, settings);

                flowViewModel.FilePath = openfileDialog.FileName;
                FlowList.Add(flowViewModel);

                CurrentFlow = FlowList.Last();
            }
        }
        #endregion

        #region Privates
        private FlowViewModel _currentFlow;
        private ToolListViewModel _toolListVM;
        private RibbonViewModel _ribbonVM;
        #endregion
    }
}
