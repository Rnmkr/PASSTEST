using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using PASSTEST.DataAccessLayer;

namespace PASSTEST
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            TextBoxLegajo.Focus();
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            TryLogin();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            TurnBoxesBackgroundWhite();

            if (e.Key == Key.Return)
            {
                TryLogin();
            }
        }

        private void Box_GotFocus(object sender, RoutedEventArgs e)
        {
            TurnBoxesBackgroundWhite();
        }

        private void TurnBoxesBackgroundWhite()
        {
            TextBoxLegajo.Background = Brushes.White;
            PasswordBoxPassword.Background = Brushes.White;
        }

        private void TryLogin()
        {
            if (IsDataFilled())
            {
                TryLogin(TextBoxLegajo.Text, PasswordBoxPassword.Password);
            }
        }

        private bool IsDataFilled()
        {
            if (string.IsNullOrWhiteSpace(TextBoxLegajo.Text))
            {
                TextBoxLegajo.Focus();
                TextBoxLegajo.Background = Brushes.Red;
                return false;
            }

            if (string.IsNullOrWhiteSpace(PasswordBoxPassword.Password))
            {
                PasswordBoxPassword.Focus();
                PasswordBoxPassword.Background = Brushes.Red;
                return false;
            }

            TurnBoxesBackgroundWhite();
            return true;
        }

        private void TryLogin(string legajo, string password)
        {
            //get array with Nombre, Apellido and HashedPassword from database
            string[] userData = GetUserData(legajo);
            if (userData[0] == null)
            {
                return;
            }
            string nombre = userData[0];
 

            string apellido = userData[1];
            string hashedPassword = userData[2];

            //if hashed password is null, show new password dialog
            if (string.IsNullOrWhiteSpace(hashedPassword))
            {
                NewPasswordWindow npw = new NewPasswordWindow(legajo);
                PasswordBoxPassword.Password = null;
                Hide();
                npw.ShowDialog();
                Show();
                return;
            }

            //if password matches show main window
            if (PasswordHasher.Verify(password, hashedPassword))
            {
                MainWindow mw = new MainWindow(legajo, nombre, apellido);
                mw.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Contraseña incorrectax!" + Environment.NewLine + Environment.NewLine + "Si no recuerda su contraseña consulte al Administrador.", "Ingreso de Usuario", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                PasswordBoxPassword.Password = null;
                PasswordBoxPassword.Focus();
            }
        }

        private string[] GetUserData(string legajo)
        {
            //***implement if ping successful***
            PRDB database = new PRDB();

            try
            {
                //get Nombre and Apellido from Usuarios table
                string leg = legajo;
                var user = database.Usuarios.First(f => f.LegajoUsuario == leg);

                ////get HashedPassword from Passwords table
                //var hashedPassword = database.Usuarios.First(f => f.FK_Legajo == legajo);

                ////get access levels from Permisos table
                //var accessLevel = database.Permisos.First(f => f.FK_Legajo == legajo);

                //store and return all data in string array
                string[] UserData = new string[] { user.NombreUsuario, user.ApellidoUsuario, user.HashedPassword };

                return UserData;
            }
            catch (System.InvalidOperationException)
            {
                //if Legajo not found in database
                MessageBox.Show("No se encontro el usuario en la base de datos o no esta habilitado para este tipo de operaciones." + Environment.NewLine + Environment.NewLine + "Si le fue asignado el puesto, solicite al Administrador que habilite su legajo para Ingreso de Pedidos.", "Ingreso de Usuario", MessageBoxButton.OK, MessageBoxImage.Warning);
                TextBoxLegajo.Text = null;
                PasswordBoxPassword.Password = null;
                TextBoxLegajo.Focus();

                string[] UserData = new string[] { null };
                return UserData;
            }
        }

        private bool GetUserAccessLevels(string legajo)
        {

            return false;
        }
    }
}





