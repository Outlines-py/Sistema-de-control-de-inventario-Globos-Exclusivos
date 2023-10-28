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

namespace Test
{
    public partial class Form1 : Form
    {
        private ErrorProvider errorProvider;
        SqlConnection conn = new SqlConnection("server=localhost;database=entregafinal; integrated security = true");
        public Form1()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            bool hayError = false;

            // Verificar si los campos están vacíos
            if (string.IsNullOrWhiteSpace(txtUserName.Text))
            {
                this.errorProvider.SetError(txtUserName, "Debe llenar este campo");
                hayError = true;
            }
            else
            {
                this.errorProvider.Clear();
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                this.errorProvider.SetError(txtPassword, "Debe llenar este campo");
                hayError = true;
            }
            else
            {
                this.errorProvider.SetError(txtPassword, null);
            }

            if (hayError)
            {
                // Mostrar mensaje de error si algún campo está vacío
                MessageBox.Show("Debe llenar todos los campos");
                return;
            }

            string username = txtUserName.Text;
            string password = txtPassword.Text;
            string query = "SELECT COUNT(1) FROM Usuario WHERE Nombre = @Username AND Contrasena = @Password";
            try
            {
                conn.Open();
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);
                int count = (int)command.ExecuteScalar();
                if (count == 1)
                {

                    if (GetUserType(username) == "Administrador")
                    {
                        MessageBox.Show("Se va a iniciar sesion", "Bienvenido Administrador", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        frmMain mainForm = new frmMain(); // Crea una instancia del formulario principal
                        this.Hide(); // Oculta el formulario de inicio de sesión actual
                        mainForm.Show(); // Muestra el formulario principa
                    }
                    else if (GetUserType(username) == "Vendedor")
                    {
                        MessageBox.Show("Se va a iniciar sesion", "Bienvenido Vendedor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        frmMainVendedor vendedorForm = new frmMainVendedor(); // Crea una instancia del formulario principal
                        this.Hide(); // Oculta el formulario de inicio de sesión actual
                        vendedorForm.Show(); // Muestra el formulario principa
                    }
                }
                else
                {
                    MessageBox.Show("El usuario o contraseña no son correctos", "Error", MessageBoxButtons.OK);
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de conexión: " + ex.Message);
            }
            
        }
        private String GetUserType(string username)
        {
            string userTypeQuery = "SELECT tipousuario FROM Usuario WHERE Nombre = @Username";
            SqlCommand userTypeCommand = new SqlCommand(userTypeQuery, conn);
            userTypeCommand.Parameters.AddWithValue("@Username", username);
            string tipoUsuario = userTypeCommand.ExecuteScalar().ToString();
            Console.WriteLine("Tipo Usuario: " + tipoUsuario);
            string usertype = tipoUsuario;
            Console.WriteLine(usertype.GetType().Name);

            return usertype;
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // Crear el ErrorProvider
            this.errorProvider = new ErrorProvider();
            this.errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            // Asignar los mensajes de error a los TextBox
            this.errorProvider.SetIconPadding(this.txtUserName, -20);
            this.errorProvider.SetIconAlignment(this.txtUserName, ErrorIconAlignment.MiddleRight);
            this.errorProvider.SetIconPadding(this.txtPassword, -20);
            this.errorProvider.SetIconAlignment(this.txtPassword, ErrorIconAlignment.MiddleRight);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}

