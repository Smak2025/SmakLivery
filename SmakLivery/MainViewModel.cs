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
        private ObservableCollection<Order> orders { get; } = new ObservableCollection<Order>();

        public async Task LoadOrdersAsync()
        {
            try
            {
                var data = await DbHelper.GetOrders();
                orders.Clear();
                foreach (var order in data)
                {
                    orders.Add(order);
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
