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

namespace Papuce
{
    public partial class LogIn : Window
    {
        private List<User> korisnici = new List<User>();
        public LogIn()
        {
            InitializeComponent();
            User admin = new User("Admin", "admin", UserType.Admin);
            korisnici.Add(admin);
            User guest = new User("Korisnik", "korisnik", UserType.Guest);
            korisnici.Add(guest);

            UsernameTextBox.Text = "unesite username";
            UsernameTextBox.Foreground = Brushes.LightSlateGray;

            PasswordBox.Visibility = Visibility.Hidden;
            PasswordBox.IsEnabled = false;
            PasswordBoxReplace.IsEnabled = true;
            PasswordBoxReplace.Visibility = Visibility.Visible;
            PasswordBoxReplace.Text = "unesite sifru";
            PasswordBoxReplace.Foreground = Brushes.LightSlateGray;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;
            UserType userType = UserType.Guest; //po defaultu guest nalog iako imamo jedan al dobro
            if(username == "Admin")
            {
                userType = UserType.Admin;
            }

            User user = korisnici.FirstOrDefault(u => u.Username == username && u.Password == password && u.Type == userType);

            if (user != null)
            {
                bool rol = false;
                if (user.Type == UserType.Admin)
                    rol = true;
                MainWindow main = new MainWindow(rol);
                main.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string target = "C:/Users/milen/Desktop/inz.upt.PROJ/Papuce/Papuce/Slike/Sajt.html";
            System.Diagnostics.Process.Start(target);
            this.Close();
        }

        private void UsernameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if(UsernameTextBox.Text.Trim() == "")
            {
                UsernameTextBox.Text = "unesite username";
                UsernameTextBox.Foreground = Brushes.LightSlateGray;
            }
        }

        private void UsernameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (UsernameTextBox.Text == "unesite username" && UsernameTextBox.Foreground == Brushes.LightSlateGray)
            {
                UsernameTextBox.Text = "";
                UsernameTextBox.Foreground = Brushes.Black;
            }
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Password.Trim() == "")
            {
                PasswordBox.Visibility = Visibility.Hidden;
                PasswordBox.IsEnabled = false;
                PasswordBoxReplace.IsEnabled = true;
                PasswordBoxReplace.Visibility = Visibility.Visible;
            }
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
           
                PasswordBoxReplace.Visibility = Visibility.Hidden;
                PasswordBoxReplace.IsEnabled = false;
                PasswordBox.IsEnabled = true;
                PasswordBox.Visibility = Visibility.Visible;
                PasswordBox.Focus();
        }
    }
}
