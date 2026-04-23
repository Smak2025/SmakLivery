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
        public ObservableCollection<Order> Orders { get; } = new ObservableCollection<Order>();
        private RelayCommand deleteOrder;
        public RelayCommand DeleteOrder => deleteOrder;
        public RelayCommand AddOrder { get; init; }

        public MainViewModel()
        {
            AddOrder = new RelayCommand(
                async (o) =>
                {
                    var order = new Order() { Address = "Какой-то адрес" };
                    var addWnd = new AddOrEditWindow(order);
                    addWnd.ShowDialog();
                }
            );
            deleteOrder = new RelayCommand(
                async (o) => {
                    if (o is Order order)
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
