using LcrSimulator.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace LcrSimulator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Properties["Dices"] = new Dices();
            Properties["Rand"] = new Random();  //new Random(Environment.TickCount)

            new MainWindow { DataContext = new MainWindowViewModel(new LcrGame()) }.Show();
        }
    }


}
