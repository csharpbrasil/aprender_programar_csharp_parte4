using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgramarCSharpComBancoDados
{
    public partial class Form2 : Form
    {
        private int Id;
        public Form2(int id = 0)
        {
            this.Id = id;
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            var dataTable = new DataTable();

            // Retorna para a variavel a ConnectionString configurada no App.Config
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;

            // Cria uma instancia de conexão com o banco de dados
            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                // Abre a conexão
                connection.Open();

                // Cria uma instancia do command
                using (System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand())
                {
                    // Comando SQL que será executado
                    var _sqlQuery = "SELECT * FROM CLIENTES WHERE ID = @ID";

                    command.Connection = connection;
                    command.CommandText = _sqlQuery;
                    command.Parameters.AddWithValue("ID", this.Id);

                    // Adiciona o resultado em um DataTable
                    using (System.Data.SqlClient.SqlDataAdapter adapter = new System.Data.SqlClient.SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }

                // Fecha conexão
                connection.Close();
            }

            if (dataTable != null && dataTable.Rows.Count > 0) {
                var row = dataTable.Rows[0];
                txtNome.Text = row["NOME"].ToString();
                txtDataNascimento.Value = Convert.ToDateTime(row["DATA_NASCIMENTO"].ToString());
                txtEmail.Text = row["EMAIL"].ToString();
            }

        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            var nome = txtNome.Text;
            var dataNascimento = txtDataNascimento.Value;
            var email = txtEmail.Text;

            // Retorna para a variavel a ConnectionString configurada no App.Config
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;

            // Cria uma instancia de conexão com o banco de dados
            using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                // Abre a conexão
                connection.Open();

                // Cria uma instancia do command
                using (System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand())
                {
                    command.Connection = connection;

                    #region Comando SQL que será executado

                    var _sqlQuery = string.Empty;

                    // Se foi passado o Id, é para editar
                    if (this.Id > 0)
                    {
                        _sqlQuery = "UPDATE CLIENTES SET NOME = @NOME, EMAIL = @EMAIL, DATA_NASCIMENTO = @DATA_NASCIMENTO WHERE ID = @ID";


                        command.Parameters.AddWithValue("id", this.Id);
                    }
                    else
                    {
                        _sqlQuery = "INSERT INTO CLIENTES(NOME, DATA_NASCIMENTO, EMAIL) VALUES(@NOME, @DATA_NASCIMENTO, @EMAIL)";
                    }

                    command.Parameters.AddWithValue("NOME", nome);
                    command.Parameters.AddWithValue("DATA_NASCIMENTO", dataNascimento);
                    command.Parameters.AddWithValue("EMAIL", email);

                    command.CommandText = _sqlQuery;

                    #endregion

                    // Executa a query
                    command.ExecuteNonQuery();
                }

                // Fecha conexão
                connection.Close();
            }

            // Fecha o formulario
            this.Close();
        }
    }
}
