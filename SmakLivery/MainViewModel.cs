using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text;

namespace SmakLivery
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<OrderWithCourier> Orders { get; } = new ObservableCollection<OrderWithCourier>();
        private RelayCommand deleteOrder;
        
        public RelayCommand DeleteOrder => deleteOrder;
        public RelayCommand AddOrder { get; init; }
        public RelayCommand EditOrder { get; init; }
        public OrderWithCourier? SelectedOrder { get; set; }

        public MainViewModel()
        {
            AddOrder = new RelayCommand(
                async (o) =>
                {
                    var order = new Order() { Address = "Какой-то адрес" };
                    var addWnd = new AddOrEditWindow(order);
                    addWnd.ShowDialog();
                    if (addWnd.ResultOk)
                    {
                        await LoadOrdersAsync();
                    }
                }
            );
            EditOrder = new RelayCommand(
                async (o) =>
                {
                    //var order = new Order() { Address = "Какой-то адрес" };
                    
                    var addWnd = new AddOrEditWindow(SelectedOrder?.ToOrder()?? new Order());
                    addWnd.ShowDialog();
                    if (addWnd.ResultOk)
                    {
                        await LoadOrdersAsync();
                    }
                },
                (o) => SelectedOrder is not null
            );
            deleteOrder = new RelayCommand(
                async (o) => {
                    if (o is OrderWithCourier order)
                    {
                        await DbHelper.DeleteOrderByIdAsync(order.Id);
                        Orders.Remove(order);
                    }
                },
                (o) => { return o is Order; }
            );
        }

        public async Task LoadOrdersAsync()
        {
            try
            {
                var data = await DbHelper.GetOrders();
                Orders.Clear();
                foreach (var order in data)
                {
                    Orders.Add(order);
                }
            }
            catch { }
        }
        
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

   
}
