using System;
using System.Collections.Generic;
using PoddiForms.ViewModels;
using PoddiForms.Views;
using Xamarin.Forms;

namespace PoddiForms
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
        }

    }
}
