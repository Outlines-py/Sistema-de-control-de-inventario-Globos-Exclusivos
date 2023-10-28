using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public partial class frmMainVendedor : Form
    {
        private DataTable dataTable = new DataTable();
        private SqlDataAdapter dataAdapter;
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
        public frmMainVendedor()
        {
            InitializeComponent();
        }

        private void frmMainVendedor_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'entregafinalDataSet1.Producto' Puede moverla o quitarla según sea necesario.
            this.productoTableAdapter.Fill(this.entregafinalDataSet1.Producto);
            // TODO: esta línea de código carga datos en la tabla 'entregafinalDataSet1.Producto' Puede moverla o quitarla según sea necesario.
            this.productoTableAdapter.Fill(this.entregafinalDataSet1.Producto);
            this.ControlBox = false;

        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 loego = new Form1();
            loego.Show();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            dataTable.Clear();
            GetDataFromDatabase();
        }
    }
}
