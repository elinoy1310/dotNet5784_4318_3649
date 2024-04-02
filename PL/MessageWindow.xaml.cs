﻿using System;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow : Window
    {
        string code;
        public MessageWindow()
        {
            InitializeComponent();
        }

        private void pb_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if(sender as PasswordBox is not null)
            code = (sender as PasswordBox)!.Password;
        }
    }
}
