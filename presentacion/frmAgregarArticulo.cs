using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace presentacion
{
    public partial class frmAgregarArticulo : Form
    {
        private Articulo articulo = null;
        public frmAgregarArticulo()
        {
            InitializeComponent();
        }

        public frmAgregarArticulo(Articulo Articulo)
        {
            InitializeComponent();
            this.articulo=Articulo;
            Text = "Modificar un articulo existente.";
        }

        private void btnAgregarArticulo_Click(object sender, EventArgs e)
        {
            //Articulo articulo = new Articulo();
            ArticuloNegocio negocio = new ArticuloNegocio();    
            try 
            {
                if (articulo == null)
                    articulo = new Articulo();

                articulo.Codigo = txbCodigoArticulo.Text;
                articulo.Nombre = txbNombre.Text;
                articulo.Descripcion = txbDescripcion.Text;
                articulo.Marca = (Marca)cboMarca.SelectedItem;   
                articulo.Categoria = (Categoria)cboCategoria.SelectedItem;
                articulo.Precio = decimal.Parse(txbPrecio.Text);
                articulo.ImagenUrl = txtUrlImagen.Text;    

                if (articulo.id != 0)
                {
                    negocio.Modificar(articulo);
                    MessageBox.Show("Modificado exitosamente");
                }
                else
                {
                    negocio.Agregar(articulo);
                    MessageBox.Show("Agregado exitosamente");
                }

            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.ToString());
            }
            finally { Close(); }
        }

        private void MenuPrincipal_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmAgregarArticulo_Load(object sender, EventArgs e)
        {
            MarcaNegocio marca = new MarcaNegocio();
            CategoriaNegocio categoria = new CategoriaNegocio();
            try
            {
                cboCategoria.DataSource = categoria.Listar();
                cboCategoria.ValueMember = "Id";
                cboCategoria.DisplayMember = "Descripcion";

                cboMarca.DataSource = marca.Listar();
                cboMarca.ValueMember = "Id";
                cboMarca.DisplayMember = "Descripcion";

                if (articulo != null)
                {
                    txbCodigoArticulo.Text = articulo.Codigo;
                    txbNombre.Text = articulo.Nombre;
                    txbDescripcion.Text = articulo.Descripcion;
                    txbPrecio.Text = articulo.Precio.ToString();
                    txtUrlImagen.Text = articulo.ImagenUrl;
                    CargarImagen(articulo.ImagenUrl);

                    cboCategoria.SelectedValue = articulo.Categoria.Id;
                    cboMarca.SelectedValue = articulo.Marca.Id;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }

        }

        private void txtUrlImagen_Leave(object sender, EventArgs e)
        {
            CargarImagen(txtUrlImagen.Text);
        }

        private void CargarImagen(string imagen)
        {
            try
            {
                pbxVistaPrevia.Load(imagen);
            }

            catch (Exception ex) { pbxVistaPrevia.Load("https://t3.ftcdn.net/jpg/05/03/24/40/360_F_503244059_fRjgerSXBfOYZqTpei4oqyEpQrhbpOML.jpg"); }
        }
    }
}
