using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using PASSTEST.DataAccessLayer;

namespace PASSTEST
{
    /// <summary>
    /// Interaction logic for NewPassword.xaml
    /// </summary>
    public partial class RenewPasswordWindow : Window
    {
        private string Legajo;

        public RenewPasswordWindow(string legajo)
        {
            InitializeComponent();
            this.Legajo = legajo;
            PasswordBoxOld.Focus();

        }


        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                TryChangePassword();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TryChangePassword();
        }

        private void TryChangePassword()
        {
            if (string.IsNullOrWhiteSpace(PasswordBoxOld.Password) || string.IsNullOrWhiteSpace(PasswordBox1.Password) || string.IsNullOrWhiteSpace(PasswordBox2.Password))
            {
                return;
            }

            if (PasswordBox1.Password == PasswordBox2.Password)
            {
                string hashedPassword = PasswordHasher.Hash(PasswordBox1.Password);
                PRDB database = new PRDB();
                //var updatedRecord = database.Passwords.First(f => f.FK_Legajo == Legajo);

                //if (PasswordHasher.Verify(PasswordBoxOld.Password, updatedRecord.HashedPassword))
                //{
                //    updatedRecord.HashedPassword = hashedPassword;
                //    database.SaveChanges();
                //    MessageBox.Show("La contraseña se cambió exitosamente!", "Cambiar Contraseña", MessageBoxButton.OK, MessageBoxImage.Information);
                //    Close();
                //}
                //else
                //{
                //    MessageBox.Show("La contraseña actual no es correcta!", "Cambiar Contraseña", MessageBoxButton.OK, MessageBoxImage.Warning);
                //}

            }
            else
            {
                MessageBox.Show("Las contraseñas no coinciden!", "Cambiar Contraseña", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    }
}
