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
