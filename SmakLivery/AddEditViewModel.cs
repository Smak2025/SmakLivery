using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SmakLivery
{
    public class AddEditViewModel
    {
        public Order Order { get; init; }
        public RelayCommand CancelCommand { get; init; }
        public RelayCommand SaveCommand { get; init; }
        public AddEditViewModel(Order order)
        {
            Order = order;
            CancelCommand = new RelayCommand(
                async (o) =>
                {
                }
            );
            SaveCommand = new RelayCommand(async (o) =>
            {

            });
        }
    }
}
