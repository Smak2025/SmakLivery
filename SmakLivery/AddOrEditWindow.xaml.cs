using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SmakLivery
{
    /// <summary>
    /// Логика взаимодействия для AddOrEditWindow.xaml
    /// </summary>
    public partial class AddOrEditWindow : Window
    {
        private AddEditViewModel viewModel;
        public AddOrEditWindow(Order order)
        {
            viewModel = new AddEditViewModel(order);
            InitializeComponent();
            DataContext = viewModel;
            Form.DataContext = viewModel.Order;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            viewModel.SaveCommand.Execute(null);

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            viewModel.CancelCommand.Execute(null);
            Close();
        }
    }
}
