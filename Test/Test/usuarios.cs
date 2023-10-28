using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;


namespace Test
{
    public partial class frmusuarios : Form
    {
        private DataTable dataTable = new DataTable();
        private SqlDataAdapter dataAdapter;
        ErrorProvider errorProvider = new ErrorProvider();
        public frmusuarios()
        {
            InitializeComponent();
            btnagregar.Enabled = false;
            dataGridView1.CellClick += dataGridView1_CellClick;
            cmbEditarTipo.Enabled = false;
        }
        
        private void frmusuarios_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
            try
            {
                // Configurar el DataGridView
                dataGridView1.DataSource = dataTable;

                // Obtener los datos iniciales desde la base de datos
                GetDataFromDatabase();
                // TODO: esta línea de código carga datos en la tabla 'entregafinalDataSet.Usuario' Puede moverla o quitarla según sea necesario.
                this.usuarioTableAdapter.Fill(this.entregafinalDataSet.Usuario);
                cmbtipo.Items.Clear();
                cmbtipo.Items.Add("Administrador");
                cmbtipo.Items.Add("Vendedor");

                // Crear el ErrorProvider
                this.errorProvider = new ErrorProvider();
                this.errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;

                // Asignar los mensajes de error a los TextBox
                errorProvider.SetIconPadding(this.txtNombre, -20);
                errorProvider.SetIconAlignment(this.txtNombre, ErrorIconAlignment.MiddleRight);
                errorProvider.SetIconPadding(this.txtPassword, -20);
                errorProvider.SetIconAlignment(this.txtPassword, ErrorIconAlignment.MiddleRight);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de conexión: " + ex.Message, "Error agregando la data");
                this.Hide();
            }
        }

        private void GetDataFromDatabase()
        {
            try
            {
                string connectionString = "server=localhost;database=entregafinal; integrated security = true"; // Reemplaza esto con tu cadena de conexión
                string query = "SELECT * FROM Usuario"; // Reemplaza NombreTabla con el nombre real de tu tabla

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


        private void cmbtipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbtipo.SelectedItem != null)
            {
                this.btnagregar.Enabled = true;
            }
            else
            {
                btnagregar.Enabled = false;
            }
        }

        private void btnagregar_Click(object sender, EventArgs e)
        {
            try
            {
                bool hayError = false;

                // Verificar si los campos están vacíos
                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                {
                    errorProvider.SetError(txtNombre, "Debe llenar este campo");
                    hayError = true;
                }
                else
                {
                    errorProvider.Clear();
                }

                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    errorProvider.SetError(txtPassword, "Debe llenar este campo");
                    hayError = true;
                }
                else
                {
                    errorProvider.SetError(txtPassword, null);
                }

                if (hayError)
                {
                    // Mostrar mensaje de error si algún campo está vacío
                    MessageBox.Show("Debe llenar todos los campos", "Formulario incompleto");
                    return;
                }
                string connectionString = "server=localhost;database=entregafinal; integrated security = true"; // Reemplaza esto con tu cadena de conexión
                string query = "INSERT INTO Usuario (Nombre, Contrasena, tipousuario) VALUES (@nombre, @password, @rol)"; // Reemplaza NombreTabla, Campo1, Campo2 con los nombres reales
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@nombre", txtNombre.Text); // Reemplaza txtCampo1 con el nombre real de tu control
                        command.Parameters.AddWithValue("@password", txtPassword.Text); // Reemplaza txtCampo2 con el nombre real de tu control
                        command.Parameters.AddWithValue("@rol", cmbtipo.SelectedItem.ToString());

                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Datos agregados correctamente. actualice la tabla");
                cmbtipo.Items.Clear();
                cmbtipo.Items.Add("Administrador");
                cmbtipo.Items.Add("Vendedor");
                txtNombre.Clear();
                txtPassword.Clear();
                btnagregar.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de conexión: " + ex.Message, "Error en la manipulacion de datos");
                this.Hide();
                /*frmMain menuInicio = new frmMain();
                menuInicio.Show();*/
            }

        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            dataTable.Clear();
            GetDataFromDatabase();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmMain mainForm = new frmMain(); // Crea una instancia del formulario principal
            this.Hide(); // Oculta el formulario de inicio de sesión actual
            mainForm.Show(); // Muestra el formulario principa
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int rowIndex = dataGridView1.SelectedRows[0].Index;
                int id = Convert.ToInt32(dataTable.Rows[rowIndex]["Id"]);
                string connectionString = "server=localhost;database=entregafinal; integrated security = true";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sqlDelete = "DELETE FROM Usuario WHERE Id = @Id";

                    using (SqlCommand command = new SqlCommand(sqlDelete, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
                dataTable.Rows.RemoveAt(rowIndex);
                MessageBox.Show("Datos eliminados correctamente.");
                cmbEditarTipo.Items.Clear();
                cmbEditarTipo.Items.Add("Administrador");
                cmbEditarTipo.Items.Add("Vendedor");
                cmbEditarTipo.Enabled = false;
                txtEditarNombre.Clear();
                txtEditarPassword.Clear();
                btnEditar.Enabled = false;
            }
        }

        private void btnEliminar_MouseHover(object sender, EventArgs e)
        {
            btnEliminar.ForeColor = Color.Cyan;
        }

        private void btnEliminar_MouseLeave(object sender, EventArgs e)
        {
            btnEliminar.ForeColor = Color.White;
        }

        private void btnEditar_Click(object sender, EventArgs e)
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

                if (string.IsNullOrWhiteSpace(txtEditarPassword.Text))
                {
                    errorProvider.SetError(txtEditarPassword, "Debe llenar este campo");
                    hayError = true;
                }
                else
                {
                    errorProvider.SetError(txtEditarPassword, null);
                }

                if (hayError)
                {
                    // Mostrar mensaje de error si algún campo está vacío
                    MessageBox.Show("Debe llenar todos los campos", "Formulario incompleto");
                    return;
                }

                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int rowIndex = dataGridView1.SelectedRows[0].Index;
                    int id = Convert.ToInt32(dataTable.Rows[rowIndex]["Id"]);

                    // Obtener los nuevos valores de los TextBox
                    string nuevoValorNombre = txtEditarNombre.Text;
                    string nuevoValorContrasena = txtEditarPassword.Text;
                    string nuevoValorRol = cmbEditarTipo.SelectedItem.ToString();

                    // Establecer la cadena de conexión a tu base de datos
                    string connectionString = "server=localhost;database=entregafinal; integrated security = true";

                    // Crear la conexión
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        // Construir la instrucción SQL de actualización
                        string sqlUpdate = "UPDATE Usuario SET Nombre = @NuevoValorNombre, Contrasena = @NuevoValorContrasena, tipousuario = @NuevoValortipousuario WHERE Id = @Id";

                        // Crear el comando SqlCommand
                        using (SqlCommand command = new SqlCommand(sqlUpdate, connection))
                        {
                            // Agregar parámetros al comando SqlCommand
                            command.Parameters.AddWithValue("@NuevoValorNombre", nuevoValorNombre);
                            command.Parameters.AddWithValue("@NuevoValorContrasena", nuevoValorContrasena);
                            command.Parameters.AddWithValue("@NuevoValortipousuario", nuevoValorRol);
                            command.Parameters.AddWithValue("@Id", id);

                            // Abrir la conexión
                            connection.Open();

                            // Ejecutar el comando SqlCommand para actualizar el registro en la base de datos
                            command.ExecuteNonQuery();
                        }
                    }
                    // Actualizar el DataGridView con los nuevos valores editados
                    dataTable.Rows[rowIndex]["Nombre"] = nuevoValorNombre;
                    dataTable.Rows[rowIndex]["Contrasena"] = nuevoValorContrasena;
                    dataTable.Rows[rowIndex]["tipousuario"] = nuevoValorRol;

                    MessageBox.Show("Datos editados correctamente.");
                    cmbEditarTipo.Items.Clear();
                    cmbEditarTipo.Items.Add("Administrador");
                    cmbEditarTipo.Items.Add("Vendedor");
                    txtEditarNombre.Clear();
                    txtEditarPassword.Clear();
                    btnEditar.Enabled = false;
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error de conexión: " + ex.Message, "Error en la manipulacion de datos");
                this.Hide();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                btnEditar.Enabled = true;
                cmbEditarTipo.Enabled = true;
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Obtener los valores de las celdas y mostrarlos en los TextBox
                txtEditarNombre.Text = row.Cells["nombreDataGridViewTextBoxColumn"].Value.ToString();
                txtEditarPassword.Text = row.Cells["contrasenaDataGridViewTextBoxColumn"].Value.ToString();
                cmbEditarTipo.Text = row.Cells["tipousuarioDataGridViewTextBoxColumn"].Value.ToString();
            }
        }
    }
}
