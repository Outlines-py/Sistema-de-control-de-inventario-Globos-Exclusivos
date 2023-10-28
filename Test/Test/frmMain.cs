using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnusuarios_Click(object sender, EventArgs e)
        {
            frmusuarios usuariosForm = new frmusuarios(); // Crea una instancia del formulario principal
            this.Hide(); // Oculta el formulario de inicio de sesión actual
            usuariosForm.Show(); // Muestra el formulario principa
        }

        private void btnproductos_Click(object sender, EventArgs e)
        {
            frmProductosAdmin aproductosForm = new frmProductosAdmin(); // Crea una instancia del formulario principal
            this.Hide(); // Oculta el formulario de inicio de sesión actual
            aproductosForm.Show(); // Muestra el formulario principa
        }

        //aqui cierra sesion --> boton de salir vv
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 loego = new Form1();
            loego.Show();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.ControlBox = false;
        }
    }
}
