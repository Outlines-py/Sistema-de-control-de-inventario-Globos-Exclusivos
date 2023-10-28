using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public partial class frmProductosAdmin : Form
    {
        private DataTable dataTable = new DataTable();
        private SqlDataAdapter dataAdapter;
        private ErrorProvider errorProvider;
        public frmProductosAdmin()
        {
            InitializeComponent();
            dataGridView1.CellClick += dataGridView1_CellClick;
            //btnIngresar.Enabled = false;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                    // Obtener los valores de las celdas y mostrarlos en los TextBox
                    txtEditarNombre.Text = row.Cells["Nombre"].Value.ToString();
                    txtEditarDesc.Text = row.Cells["Descripcion"].Value.ToString();
                    txtEditarPrecio.Text = row.Cells["Precio"].Value.ToString();
                    txtEditarStock.Text = row.Cells["Stock"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
            }
        }

        private void frmProductosAdmin_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'entregafinalDataSet1.Producto' Puede moverla o quitarla según sea necesario.
            this.productoTableAdapter.Fill(this.entregafinalDataSet1.Producto);
            this.ControlBox = false;
            try
            {
                // Configurar el DataGridView
                dataGridView1.DataSource = dataTable;

                // Obtener los datos iniciales desde la base de datos
                GetDataFromDatabase();

                // TODO: esta línea de código carga datos en la tabla 'entregafinalDataSet1.Producto' Puede moverla o quitarla según sea necesario.
                this.productoTableAdapter.Fill(this.entregafinalDataSet1.Producto);
                this.ControlBox = false;

                // Crear el ErrorProvider
                this.errorProvider = new ErrorProvider();
                this.errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;

                // Asignar los mensajes de error a los TextBox
                this.errorProvider.SetIconPadding(this.txtNombre, -20);
                this.errorProvider.SetIconAlignment(this.txtNombre, ErrorIconAlignment.MiddleRight);
                this.errorProvider.SetIconPadding(this.txtDesc, -20);
                this.errorProvider.SetIconAlignment(this.txtDesc, ErrorIconAlignment.MiddleRight);
                this.errorProvider.SetIconPadding(this.txtPrecio, -20);
                this.errorProvider.SetIconAlignment(this.txtPrecio, ErrorIconAlignment.MiddleRight);
                this.errorProvider.SetIconPadding(this.txtStock, -20);
                this.errorProvider.SetIconAlignment(this.txtStock, ErrorIconAlignment.MiddleRight);
            } catch (Exception ex)
            {
                MessageBox.Show("Error de conexión: " + ex.Message, "Base de datos no encontrada");
            }
            
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            dataTable.Clear();
            GetDataFromDatabase();
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmMain MenuPrincipal = new frmMain();
            MenuPrincipal.Show();

        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                bool hayError = false;

                // Verificar si los campos están vacíos
                //Nombre
                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    this.errorProvider.SetError(txtNombre, "Debe llenar este campo");
                    hayError = true;
                }
                else
                {
                    this.errorProvider.Clear();
                }
                //Descripcion
                if (string.IsNullOrWhiteSpace(txtDesc.Text))
                {
                    this.errorProvider.SetError(txtDesc, "Debe llenar este campo");
                    hayError = true;
                }
                else
                {
                    this.errorProvider.Clear();
                }
                //Precio
                if (string.IsNullOrWhiteSpace(txtPrecio.Text))
                {
                    this.errorProvider.SetError(txtPrecio, "Debe llenar este campo");
                    hayError = true;
                }
                else
                {
                    this.errorProvider.Clear();
                }
                //Stock
                if (string.IsNullOrWhiteSpace(txtStock.Text))
                {
                    this.errorProvider.SetError(txtStock, "Debe llenar este campo");
                    hayError = true;
                }
                else
                {
                    this.errorProvider.Clear();
                }

                if (hayError)
                {
                    // Mostrar mensaje de error si algún campo está vacío
                    MessageBox.Show("Debe llenar todos los campos");
                    return;
                }

                string precioTexto = txtPrecio.Text;
                string stockTexto = txtStock.Text;
                decimal precioDecimal;
                int stock;

                bool stockValido = int.TryParse(stockTexto, out stock);
                bool precioValido = decimal.TryParse(precioTexto, out precioDecimal);

                if (stockValido && precioValido)
                {
                    string connectionString = "server=localhost;database=entregafinal; integrated security = true"; 
                    string query = "INSERT INTO Producto (Nombre, Descripcion, Precio, CantidadEnStock) VALUES (@Nombre, @Descripcion, @Precio, @CantidadEnStock)"; 
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@nombre", txtNombre.Text); 
                            command.Parameters.AddWithValue("@Descripcion", txtDesc.Text); 
                            command.Parameters.AddWithValue("@Precio", precioDecimal);
                            command.Parameters.AddWithValue("@CantidadEnStock", stock);

                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                    }
                }
                MessageBox.Show("Datos agregados correctamente. actualice la tabla");
                txtNombre.Clear();
                txtDesc.Clear();
                txtPrecio.Clear();
                txtStock.Clear();
                //btnIngresar.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de conexión: " + ex.Message, "Error en la manipulacion de datos");
                this.Hide();
            }
        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {
            string input = txtNombre.Text;
            // Validar que solo se permitan caracteres de texto
            if (!Regex.IsMatch(input, "^[a-zA-Z ]*$"))
            {                
                MessageBox.Show("El nombre solo debe contener letras y espacios", "Tipo de dato invalido para campo Nombre");
                txtNombre.Text = string.Empty;
            }
        }

        private void txtPrecio_TextChanged(object sender, EventArgs e)
        {
            string input = txtPrecio.Text;

            // Validar que solo se permitan números decimales con un máximo de 2 decimales
            if (!string.IsNullOrEmpty(input))
            {
                if (!Regex.IsMatch(input, @"^\d+(\.\d{0,2})?$"))
                {
                    MessageBox.Show("El precio debe ser un número decimal con un máximo de 2 decimales (por ejemplo, 10.00)", "Tipo de dato invalido para campo Precio");
                    txtPrecio.Text = string.Empty;
                }
            }
        }

        private void txtStock_TextChanged(object sender, EventArgs e)
        {
            string input = txtStock.Text;

            // Validar que solo se permitan números enteros
            if (!Regex.IsMatch(input, "^[0-9]*$"))
            {                
                MessageBox.Show("El stock debe ser un número entero", "Tipo de dato invalido para campo Stock");
                txtStock.Text = string.Empty;
            }
        }
        private void GetDataFromDatabase()
        {
            try
            {
                string connectionString = "server=localhost;database=entregafinal; integrated security = true"; // Reemplaza esto con tu cadena de conexión
                string query = "SELECT * FROM Producto"; // Reemplaza NombreTabla con el nombre real de tu tabla

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    dataAdapter = new SqlDataAdapter(query, connection);

                    // Llenar el DataTable con los datos del SqlDataAdapter
                    dataAdapter.Fill(dataTable);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de conexión: " + ex.Message, "Error Importando la data");
            }
        }

        //boton de editar
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                bool hayError = false;

                // Verificar si los campos están vacíos
                if (string.IsNullOrWhiteSpace(txtEditarNombre.Text))
                {
                    errorProvider.SetError(txtEditarNombre, "Debe llenar este campo");
                    hayError = true;
                }
                else
                {
                    errorProvider.Clear();
                }

                if (string.IsNullOrWhiteSpace(txtEditarDesc.Text))
                {
                    errorProvider.SetError(txtEditarDesc, "Debe llenar este campo");
                    hayError = true;
                }
                else
                {
                    errorProvider.SetError(txtEditarDesc, null);
                }

                if (string.IsNullOrWhiteSpace(txtEditarPrecio.Text))
                {
                    errorProvider.SetError(txtEditarPrecio, "Debe llenar este campo");
                    hayError = true;
                }
                else
                {
                    errorProvider.SetError(txtEditarStock, null);
                }

                if (string.IsNullOrWhiteSpace(txtEditarStock.Text))
                {
                    errorProvider.SetError(txtEditarStock, "Debe llenar este campo");
                    hayError = true;
                }
                else
                {
                    errorProvider.SetError(txtEditarStock, null);
                }

                if (hayError)
                {
                    // Mostrar mensaje de error si algún campo está vacío
                    MessageBox.Show("Debe llenar todos los campos", "Formulario incompleto");
                    return;
                }
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    //int rowIndex = dataGridView1.SelectedRows[0].Index;
                    //int id = Convert.ToInt32(dataTable.Rows[rowIndex]["Id"]);

                    int rowIndex = dataGridView1.SelectedRows[0].Index;
                    int id = Convert.ToInt32(dataTable.Rows[rowIndex]["Id"]);

                    string precioTexto = txtEditarPrecio.Text;
                    string stockTexto = txtEditarStock.Text;

                    string nuevoValorNombre = txtEditarNombre.Text;
                    string nuevoValorDesc = txtEditarDesc.Text;
                    string nuevoValorPrecio = txtEditarPrecio.Text;
                    string nuevoValorStock = txtEditarStock.Text;

                    string connectionString = "server=localhost;database=entregafinal; integrated security = true";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        string sqlUpdate = "UPDATE Producto SET Nombre = @NuevoValorNombre, Descripcion = @NuevoValorDescripcion, Precio = @NuevoValorPrecio, CantidadEnStock = @NuevoValorStock WHERE Id = @Id";

                        using (SqlCommand command = new SqlCommand(sqlUpdate, connection))
                        {
                            command.Parameters.AddWithValue("@NuevoValorNombre", nuevoValorNombre);
                            command.Parameters.AddWithValue("@NuevoValorDescripcion", nuevoValorDesc);
                            command.Parameters.AddWithValue("@NuevoValorPrecio", nuevoValorPrecio);
                            command.Parameters.AddWithValue("@NuevoValorStock", nuevoValorStock);
                            command.Parameters.AddWithValue("@Id", id);

                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                    }
                    // Actualizar el DataGridView con los nuevos valores editados
                    dataTable.Rows[rowIndex]["Nombre"] = nuevoValorNombre;
                    dataTable.Rows[rowIndex]["Descripcion"] = nuevoValorDesc;
                    dataTable.Rows[rowIndex]["Precio"] = nuevoValorPrecio;
                    dataTable.Rows[rowIndex]["CantidadEnStock"] = nuevoValorStock;

                    MessageBox.Show("Datos editados correctamente.");
                    txtEditarNombre.Clear();
                    txtEditarDesc.Clear();
                    txtEditarPrecio.Clear();
                    txtEditarStock.Clear();
                    //btnEditar.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de conexión: " + ex.Message, "Error en la manipulacion de datos");
                this.Hide();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try 
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int rowIndex = dataGridView1.SelectedRows[0].Index;
                    int id = Convert.ToInt32(dataTable.Rows[rowIndex]["Id"]);
                    string connectionString = "server=localhost;database=entregafinal; integrated security = true";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {

                        string sqlDelete = "DELETE FROM Producto WHERE Id = @Id";


                        using (SqlCommand command = new SqlCommand(sqlDelete, connection))
                        {
                            command.Parameters.AddWithValue("@Id", id);
                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                    }
                    // Eliminar la fila correspondiente del DataTable
                    dataTable.Rows.RemoveAt(rowIndex);
                    MessageBox.Show("Datos eliminados correctamente.");
                    txtEditarNombre.Clear();
                    txtEditarDesc.Clear();
                    txtEditarPrecio.Clear();
                    txtEditarStock.Clear();
                }
            } catch (Exception ex)
            {
                MessageBox.Show("" + ex.Message);
            }
        }
    }
}
