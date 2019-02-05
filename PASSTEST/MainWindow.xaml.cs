using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using PASSTEST.DataAccessLayer;

namespace PASSTEST
{
    public partial class MainWindow : Window
    {
        private string legajo;
        //private IQueryable<Productos> listaproductos;
        //private IQueryable<Modelos> listamodelos;
        private bool skipevent = false;

        public MainWindow(string legajo, string nombre, string apellido)
        {
            InitializeComponent();
            this.legajo = legajo;
            LabelUserData.Content = ("(" + legajo + ")" + " " + nombre + " " + apellido).ToUpper();
            CargarProductosModelos();
            NuevoPedido();
            GridEnable.IsEnabled = true;
        }

        private void CargarProductosModelos()
        {
            PRDB database = new PRDB();
            //listamodelos = database.Modelos.Select(s => s);
            //listaproductos = database.Productos.Select(s => s);
        }

        private void ButtonCambiarPassword_Click(object sender, RoutedEventArgs e)
        {
            RenewPasswordWindow rpw = new RenewPasswordWindow(legajo);
            rpw.ShowDialog();
        }

        private void ButtonNuevo_Click(object sender, RoutedEventArgs e)
        {
            NuevoPedido();
        }

        private void Desloguearse_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("¿Desea desloguearse?", "Ingreso de Pedidos", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.OK)
            {
                LoginWindow lw = new LoginWindow();
                lw.Show();
                Close();
            }

        }

        private void ComboBoxModelo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ComboBoxModelo.SelectedItem == null)
            {
                return;
            }


            //string query = (from a in listaproductos
            //                join c in listamodelos
            //                on a.ID_Producto equals c.FK_ID_Producto
            //                where c.NombreModelo == ComboBoxModelo.SelectedValue.ToString()
            //                select a.NombreProducto).First();


            //lo siguiente es lo mismo en lambda expression. prefiero LINQ que para este caso es mas legible.
            //var query = database.Productos.Join(database.Modelos, a => a.ID_Producto, c => c.FK_ID_Producto, (a, c) => new { a = a, c = c }).Where(w => (w.c.NombreModelo == ComboBoxModelo.SelectedValue.ToString())).Select(s => s.a.NombreProducto).First().ToString();

            //LabelProductos.Content = query;

            LabelProducto.Foreground = Brushes.Black;
            TextBoxCantidad.IsEnabled = true;
            LabelCantidad.Foreground = Brushes.Black;
            TextBoxCantidad.Focus();
        }

        private void TextBoxCantidad_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBoxCantidad.Text))
            {
                return;
            }

            if (e.Key == Key.Return)
            {
                ComboBoxEstado.IsEnabled = true;
                LabelEstado.Foreground = Brushes.Black;
                ComboBoxEstado.IsDropDownOpen = true;
                ComboBoxEstado.Focus();
            }
        }

        private void TextBoxCantidad_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^\d$");
            bool valid = regex.IsMatch(TextBoxCantidad.Text);
            if (!valid)
            {
                return;////////////////////////////////////////////////////////
            }
        }

        private void ComboBoxEstado_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            CheckBoxReproceso.IsEnabled = true;
            CheckBoxReproceso.Foreground = Brushes.Black;
            CheckBoxReproceso.Focus();
            ButtonGuardar.IsEnabled = true;
        }

        private void ComboBoxEstado_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                CheckBoxReproceso.Focus();
            }
        }

        private void ButtonGuardar_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Desea ingresar el pedido " + TextBoxNumeroPedido.Text + "?", "Ingreso de Pedidos", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            //if (result == MessageBoxResult.OK)
            //{
            //    PRDB database = new PRDB();
            //    Pedidos np = new Pedidos() { Cantidad = int.Parse(TextBoxCantidad.Text), FK_ID_Modelo = 3, FK_ID_Producto = 1, NumeroPedido = TextBoxNumeroPedido.Text, Reproceso = "NULL" };
            //    database.Pedidos.Add(np);
            //    try
            //    {
            //    database.SaveChanges();
            //    MessageBox.Show("El pedido se ingreso correctamente!", "Ingreso de Pedidos", MessageBoxButton.OK, MessageBoxImage.Information);
            //    NuevoPedido();
            //    }
            //    catch (System.Data.Entity.Infrastructure.DbUpdateException)
            //    {
            //        MessageBox.Show("El Pedido ya existe en la Base de Datos!", "Ingreso de Pedidos", MessageBoxButton.OK, MessageBoxImage.Error);
            //    }
            //}
        }

        private void CheckBoxReproceso_Checked(object sender, RoutedEventArgs e)
        {
            LabelReproceso.Foreground = Brushes.Black;
            TextBoxNumeroReproceso.IsEnabled = true;
            TextBoxNumeroReproceso.Focus();
            ButtonGuardar.IsEnabled = false;
        }

        private void CheckBoxReproceso_Unchecked(object sender, RoutedEventArgs e)
        {
            LabelReproceso.Foreground = Brushes.LightGray;
            TextBoxNumeroReproceso.Text = null;
            TextBoxNumeroReproceso.IsEnabled = false;
            ButtonGuardar.IsEnabled = true;
        }

        private void CheckBoxReproceso_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                ButtonGuardar.Focus();
            }

            if (e.Key == Key.Return)
            {
                ButtonGuardar.Focus();
            }
        }

        private void TextBoxNumeroPedido_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (skipevent)
            {
                return;
            }
            ValidarCantidadCaracteres("nuevo", TextBoxNumeroPedido.Text.ToUpper());
        }

        private void TextBoxNumeroPedido_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                NuevoPedido();
            }

            if (e.Key == Key.Return)
            {
                ValidarCantidadCaracteres("nuevo", TextBoxNumeroPedido.Text);
            }
        }

        private void TextBoxNumeroReproceso_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (skipevent)
            {
                return;
            }
            ValidarCantidadCaracteres("reproceso", TextBoxNumeroReproceso.Text.ToUpper());
        }

        private void TextBoxNumeroReproceso_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                CheckBoxReproceso.IsChecked = false;
                CheckBoxReproceso.Focus();
            }

            if (e.Key == Key.Return)
            {
                ValidarCantidadCaracteres("reproceso", TextBoxNumeroReproceso.Text);
            }
        }

        private void ValidarCantidadCaracteres(string tipo, string numeropedido)
        {
            if (numeropedido.Length == 11)
            {
                ValidarFormatoNumeroPedido(tipo, numeropedido);
            }
        }

        private void ValidarFormatoNumeroPedido(string tipo, string numeropedido)
        {
            Regex regex = new Regex(@"^\d{7}[a-zA-Z]-\d{2}$");
            bool valid = regex.IsMatch(numeropedido);
            if (valid)
            {
                skipevent = true;
                TextBoxNumeroPedido.Text = TextBoxNumeroPedido.Text.ToUpper();
                TextBoxNumeroReproceso.Text = TextBoxNumeroReproceso.Text.ToUpper();
                skipevent = false;
                switch (tipo)
                {
                    case "nuevo":
                        LabelModelo.Foreground = Brushes.Black;
                        //var modelos = listamodelos.Select(s => s.NombreModelo).ToList();
                        //ComboBoxModelo.ItemsSource = modelos;
                        ComboBoxModelo.IsEnabled = true;
                        ComboBoxModelo.IsDropDownOpen = true;
                        ComboBoxModelo.Focus();
                        break;

                    case "reproceso":
                        BuscarReproceso(numeropedido);
                        break;
                }
            }
            else
            {
                MensajeFormatoIncorrecto(tipo);
            }
        }

        private void MensajeFormatoIncorrecto(string tipo)
        {
            MessageBox.Show("El formato es incorrecto!", "Ingresar Pedido", MessageBoxButton.OK, MessageBoxImage.Exclamation);

            switch (tipo)
            {
                case "nuevo":
                    NuevoPedido();
                    break;

                case "reproceso":
                    TextBoxNumeroReproceso.Text = null;
                    break;
            }
        }

        private void BuscarReproceso(string numeropedido)
        {
            PRDB database = new PRDB();
            //bool ExisteNumeroPedido = database.Pedidos.Any(a => a.NumeroPedido == numeropedido);
            //if (ExisteNumeroPedido)
            //{
            //    ButtonGuardar.IsEnabled = true;
            //    ButtonGuardar.Focus();
            //}
            //else
            //{
            //    MessageBox.Show("No se encontró el pedido " + numeropedido + " en la base de datos.", "Asignar Reproceso", MessageBoxButton.OK, MessageBoxImage.Error);
            //    TextBoxNumeroReproceso.Text = null;
            //    ButtonGuardar.IsEnabled = false;
            //}
        }

        private void NuevoPedido()
        {
            BorrarCampos();
            TextBoxNumeroPedido.Focus();
        }

        private void BorrarCampos()
        {
            CheckBoxReproceso.IsChecked = false;
            TextBoxNumeroPedido.Text = null;
            TextBoxCantidad.Text = null;
            TextBoxNumeroReproceso.Text = null;
            LabelProductos.Content = null;

            ComboBoxModelo.SelectedItem = null;
            ComboBoxEstado.SelectedItem = null;

            CheckBoxReproceso.IsEnabled = false;
            TextBoxCantidad.IsEnabled = false;
            TextBoxNumeroReproceso.IsEnabled = false;

            ComboBoxModelo.IsEnabled = false;
            ComboBoxEstado.IsEnabled = false;

            CheckBoxReproceso.Foreground = Brushes.LightGray;
            LabelProducto.Foreground = Brushes.LightGray;
            LabelModelo.Foreground = Brushes.LightGray;
            LabelCantidad.Foreground = Brushes.LightGray;
            LabelEstado.Foreground = Brushes.LightGray;
            LabelReproceso.Foreground = Brushes.LightGray;

            ButtonGuardar.IsEnabled = false;
        }

 
        private void ButtonEditar_Click(object sender, RoutedEventArgs e)
        {
            OrderSearchWindow osw = new OrderSearchWindow();
            string result = osw.DialogResult();
            MessageBox.Show(result);

        }

        private void ButtonEliminar_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}