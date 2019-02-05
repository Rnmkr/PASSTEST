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
    public partial class NewPasswordWindow : Window
    {
        private string Legajo;

        public NewPasswordWindow(string legajo)
        {
            InitializeComponent();
            this.Legajo = legajo;
            PasswordBox1.Focus();

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
            if (string.IsNullOrWhiteSpace(PasswordBox1.Password) && string.IsNullOrWhiteSpace(PasswordBox2.Password))
            {
                return;
            }

            if (PasswordBox1.Password == PasswordBox2.Password)
            {
                string hashedPassword = PasswordHasher.Hash(PasswordBox1.Password);
                PRDB database = new PRDB();
                string leg = Legajo;
                var updatedRecord = database.Usuarios.First(f => f.LegajoUsuario == leg);
                updatedRecord.HashedPassword = hashedPassword;
                database.SaveChanges();

                MessageBox.Show("La contraseña se cambió exitosamente!", "Cambiar Contraseña", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Las contraseñas no coinciden!", "Cambiar Contraseña", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    }
}
