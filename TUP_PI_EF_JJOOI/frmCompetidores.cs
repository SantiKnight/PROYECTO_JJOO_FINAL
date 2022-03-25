using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

//CURSO – LEGAJO – APELLIDO – NOMBRE

namespace TUP_PI_EF_JJOOI
{
    public partial class frmCompetidores : Form
    {
        SqlConnection conexion = new SqlConnection(@"Data Source=localhost;Initial Catalog=JJOO;Integrated Security=True");

        SqlCommand comando = new SqlCommand();

        bool bandera = false;
        public frmCompetidores()
        {
            InitializeComponent();
        }

        private DataTable consultaSQL(string consultaSQL)
        {
            conexion.Open();
            comando.Connection = conexion;
            comando.CommandType = CommandType.Text;
            comando.CommandText = consultaSQL;
            DataTable tabla = new DataTable();
            tabla.Load(comando.ExecuteReader());
            conexion.Close();
            return tabla;
        }

        private void actualizarSQL(string consultaSQL)
        {
            conexion.Open();
            comando.Connection = conexion;
            comando.CommandType = CommandType.Text;
            comando.CommandText = consultaSQL;
            comando.ExecuteNonQuery();
            conexion.Close();

        }
        private void cargarLista(ListBox list, string nombreTabla)
        {
            DataTable tabla = consultaSQL("Select numero, TRIM(str(numero)+'-'+nombre) as info from " +nombreTabla);
            list.DataSource = tabla;
            list.DisplayMember = tabla.Columns[1].ColumnName;
            list.ValueMember = tabla.Columns[0].ColumnName;
        }

        private void cargarCombo(ComboBox combo, string nombreTabla)
        {
            DataTable tabla = consultaSQL("Select * from " + nombreTabla + " Order by 2");
            combo.DataSource = tabla;
            combo.DisplayMember = tabla.Columns[1].ColumnName;
            combo.ValueMember = tabla.Columns[0].ColumnName;
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        private void frmCompetidores_Load(object sender, EventArgs e)
        {
            cargarCombo(cboDisciplina, "Disciplinas");
            cargarLista(lstCompetidores, "competidores");
            cboDisciplina.SelectedIndex = -1;
            habilitar(false);
        }

        private void limpiar()
        {
            txtNumero.Text = "";
            txtNombre.Text = "";
            cboDisciplina.SelectedIndex = -1;
            rbtMasculino.Checked = false;
            rbtFemenino.Checked = false;
            dtpFechaNacimiento.Value = DateTime.Today;
            txtNumero.Focus();
        }

        private void habilitar(bool x)
        {
            txtNumero.Enabled = x;
            txtNombre.Enabled = x;
            cboDisciplina.Enabled = x;
            dtpFechaNacimiento.Enabled = x;
            rbtMasculino.Enabled = x;
            rbtFemenino.Enabled = x;
            btnGrabar.Enabled = x;
            btnNuevo.Enabled = !x;
            btnCancelar.Enabled = x;
            btnEditar.Enabled = !x;
            btnBorrar.Enabled = !x;
            lstCompetidores.Enabled = !x;
        }


        private bool validarCampos()
        {
            if (txtNumero.Text == "")
            {
                MessageBox.Show("Debe ingresar un número...");
                txtNumero.Focus();
                return false;
            }
            else
            {
                try
                {
                    int.Parse(txtNumero.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Debe ingresar valores numéricos en número...");
                    txtNumero.Focus();
                    return false;
                }
            }
            if (txtNombre.Text == string.Empty)
            {
                MessageBox.Show("Debe ingresar un nombre...");
                txtNombre.Focus();
                return false;
            }

            if (cboDisciplina.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar una disciplina...");
                cboDisciplina.Focus();
                return false;
            }
            if (rbtMasculino.Checked == false && rbtFemenino.Checked == false)
            {
                MessageBox.Show("Debe seleccionar un sexo...");
                rbtMasculino.Focus();
                return false;
            }
            if (string.IsNullOrEmpty(dtpFechaNacimiento.Text))
            {
                MessageBox.Show("Debe seleccionar una fecha de nacimiento...");
                dtpFechaNacimiento.Focus();
                return false;
            }

            return true;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            limpiar();
            habilitar(true);
            txtNumero.Focus();
            bandera = true;
        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            if (validarCampos()) 
            {
                string insertSql = "";
                Competidor p = new Competidor();
                p.pNumero = int.Parse(txtNumero.Text);
                p.pNombre = txtNombre.Text;
                p.pDisciplina = Convert.ToInt32(cboDisciplina.SelectedValue);
                if (rbtMasculino.Checked)
                    p.pSexo = "M";
                else
                    p.pSexo = "F";
                p.pFechaNacimiento = dtpFechaNacimiento.Value;
                if (bandera)
                {
                    insertSql = $"Insert into Competidores values({ p.pNumero},'{p.pNombre}', { p.pDisciplina},'{p.pSexo}','{p.pFechaNacimiento.ToString("yyyy/MM/dd")}')";
                    MessageBox.Show("Su registro se insertó con exito");
                }
                else
                {
                    insertSql = "Update Competidores set nombre = '" + p.pNombre + "', " + "sexo= '" + p.pSexo + "'," + "fechaNacimiento='" + p.pFechaNacimiento.ToString("yyyy/MM/dd") + "'" + " Where numero=" + p.pNumero;
                    MessageBox.Show("Se actualizo su registro con exito");
                }
                actualizarSQL(insertSql);
                cargarLista(lstCompetidores, "Competidores");
                habilitar(false);
                limpiar();

            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            habilitar(true);
            bandera = false;
            DataTable tabla = consultaSQL("Select * FROM COMPETIDORES");
            for (int i = 0; i < tabla.Rows.Count; i++)
            {
                if (lstCompetidores.SelectedValue.ToString() == Convert.ToString(tabla.Rows[i][0]))
                {
                    txtNumero.Enabled = false;
                    txtNumero.Text = Convert.ToString(tabla.Rows[i][0]);
                    txtNombre.Text = Convert.ToString(tabla.Rows[i][1]);
                    cboDisciplina.SelectedIndex = Convert.ToInt32(tabla.Rows[i][2]);
                    rbtMasculino.Checked = true;
                    dtpFechaNacimiento.Value = Convert.ToDateTime(tabla.Rows[i][4]);
                }
            }
            cargarLista(lstCompetidores, "Competidores");
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            habilitar(false);
            limpiar();
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Seguro de abandonar la aplicación ?",
                "SALIR", MessageBoxButtons.YesNo, MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                this.Close();
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            DataTable tabla = consultaSQL("Select numero, nombre FROM COMPETIDORES");
            for (int i = 0; i < tabla.Rows.Count; i++)
            {
                if (lstCompetidores.SelectedValue.ToString() == Convert.ToString(tabla.Rows[i][0]))
                {
                    DataTable _tabla2 = consultaSQL("Delete competidores where numero = " + Convert.ToString(tabla.Rows[i][0]));
                    i = tabla.Rows.Count;
                    MessageBox.Show("Se borro su registro con exito!");
                }
            }
            cargarLista(lstCompetidores, "Competidores");
        }
    }
}
