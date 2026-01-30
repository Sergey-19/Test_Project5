using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EQX.FlowUILibraryWPF.MVVM.Views
{
    /// <summary>
    /// Interaction logic for AddFlowWindowView.xaml
    /// </summary>
    public partial class AddFlowWindowView : Window
    {
        public AddFlowWindowView()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        private string _flowName;

        public string FlowName
        {
            get { return _flowName; }
            set { _flowName = value; }
        }


        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            Close();
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            Close();
        }
    }
}
