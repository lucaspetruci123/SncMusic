using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SncMusic
{
    public partial class FrmAluno : Form
    {
        public FrmAluno()
        {
            InitializeComponent();
           
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            string sexo;
            if (rdbMasculino.Checked) sexo = "M";
            else sexo = "F";// resolve o sexo
            mskCPF.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            mskTelefone.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;

            Aluno aluno = new Aluno(txtNome.Text, mskCPF.Text, sexo, txtEmail.Text, mskTelefone.Text);
            aluno.Inserir();

            MessageBox.Show("Aluno Gravado com sucesso!");
            LimparControles();

        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            // se o text do botão for igual a "..."
            if (btnBuscar.Text == "...")
            {
                //alterar o texto do botão para "Buscar"
                btnBuscar.Text = "Buscar";
                //tornar o txtid Enable true
                txtId.Enabled = true;
                //tornar o txtid readonly false
                txtId.ReadOnly = false;
                //colocar o foco (cursor) no txtid e limpe
                txtId.Focus();
                txtId.Clear();
            }
            //senão
            else
            {
                //se txtid for diferente de vazio então consulte o aluno
                if (txtId.Text != string.Empty)
                {
                    //consulte o aluno
                    var comm = Banco.Abrir();
                    comm.CommandText = "select * from tb_aluno where id_aluno = "+ txtId.Text;
                    var dr = comm.ExecuteReader();
                    while (dr.Read())
                    {
                        txtNome.Text = dr.GetString(1);
                        txtEmail.Text = dr.GetString(4);
                        mskCPF.Text = dr.GetString(2);                 
                        if (dr.GetString(3) == "M")
                            rdbMasculino.Checked = true;
                        else
                            rdbFeminino.Checked = true;

                    }
                    //altere o texto do botão para "..."
                    btnBuscar.Text = "...";
                    //tornar o txtid Enable false
                    txtId.Enabled = false;
                    //tornar o txtid readonly true
                    txtId.ReadOnly = true;
                }

                
            }

        }

        private void txtId_TextChanged(object sender, EventArgs e)
        {
            if (txtId.Text != string.Empty)
            {
                btnAlterar.Enabled = true;
            }
            else
            {
                btnAlterar.Enabled = false;
            }
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            string sexo;
            if (rdbMasculino.Checked) sexo = "M";
            else sexo = "F";// resolve o sexo
            mskTelefone.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            var comm = Banco.Abrir();
            comm.CommandText = "update tb_aluno set nome_aluno = '"+txtNome.Text+"'," +
                " sexo_aluno = '"+sexo+ "', telefone_aluno = '"+mskTelefone.Text +
                "' where id_aluno = "+txtId.Text;
            comm.ExecuteNonQuery();
            comm.Connection.Close();
            MessageBox.Show("Dados do aluno alterados com sucesso!");
            LimparControles();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            DialogResult msg = MessageBox.Show(
                "Deseja realmente excluir o aluno?",
                "Excluir Aluno...",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (msg == DialogResult.Yes && txtId.Text != string.Empty)
            {
                var comm = Banco.Abrir();
                comm.CommandText = "delete from tb_aluno where id_aluno = " + txtId.Text;
                comm.ExecuteNonQuery();
                MessageBox.Show("Aluno excluído com sucesso!");
                LimparControles();
            }
           
        }
        private void LimparControles()
        {
            txtId.Clear();
            txtNome.Clear();
            txtEmail.Clear();
            mskCPF.Clear();
            mskTelefone.Clear();
            rdbFeminino.Checked = false;
            rdbMasculino.Checked = false;
            txtNome.Focus();
        }
        
    }
}
