using CRUDPessoas.modelo;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRUDPessoas.DAL
{
	

    public class PessoaDAO
    {
        public String mensagem;

        public void CadastrarPessoa(Pessoa pessoa)
        {
            try
            {
                SqlConnection conexao = Conexao.Conectar();
                string comandoSql = "INSERT INTO Pessoas (nome, rg, cpf) " +
                    "VALUES (@nome, @rg, @cpf)";

                using (SqlCommand comando = new SqlCommand(comandoSql, conexao))
                {
                    comando.Parameters.AddWithValue("@nome", pessoa.nome);
                    comando.Parameters.AddWithValue("@rg", pessoa.rg);
                    comando.Parameters.AddWithValue("@cpf", pessoa.cpf);

                    comando.ExecuteNonQuery();
                }

                Conexao.mensagem = "Pessoa cadastrada com sucesso.";
            }
            catch (Exception ex)
            {
                Conexao.mensagem = "Erro ao cadastrar pessoa: " + ex.Message;
            }
            finally
            {
                Conexao.Desconectar();
            }
        }

        public Pessoa PesquisarPessoaPorId(Pessoa pessoa)
        {
            try
            {
                SqlConnection conexao = Conexao.Conectar();
                string comandoSql = "SELECT id, nome, rg, cpf FROM Pessoas WHERE id = @id";

                using (SqlCommand comando = new SqlCommand(comandoSql, conexao))
                {
                    comando.Parameters.AddWithValue("@id", pessoa.id);

                    using (SqlDataReader leitor = comando.ExecuteReader())
                    {
                        if (leitor.Read())
                        {
                            pessoa.id = Convert.ToInt32(leitor["id"]);
                            pessoa.nome = leitor["nome"].ToString();
                            pessoa.rg = leitor["rg"].ToString();
                            pessoa.cpf = leitor["cpf"].ToString();
                        }
                    }
                }
                Conexao.mensagem = "Pesquisa realizada com sucesso.";
            }
            catch (Exception ex)
            {
                Conexao.mensagem = "Erro ao pesquisar pessoa: " + ex.Message;
            }
            finally
            {
                Conexao.Desconectar();
            }
            return pessoa;
        }

        public void EditarPessoa(Pessoa pessoa)
        {
            try
            {
                SqlConnection conexao = Conexao.Conectar();
                string comandoSql = "UPDATE Pessoas SET nome = @nome, rg = @rg, cpf = @cpf WHERE id = @id";

                using (SqlCommand comando = new SqlCommand(comandoSql, conexao))
                {
                    comando.Parameters.AddWithValue("@id", pessoa.id);
                    comando.Parameters.AddWithValue("@nome", pessoa.nome);
                    comando.Parameters.AddWithValue("@rg", pessoa.rg);
                    comando.Parameters.AddWithValue("@cpf", pessoa.cpf);

                    comando.ExecuteNonQuery();
                }

                Conexao.mensagem = "Pessoa editada com sucesso.";
            }
            catch (Exception ex)
            {
                Conexao.mensagem = "Erro ao editar pessoa: " + ex.Message;
            }
            finally
            {
                Conexao.Desconectar();
            }
        }
    }
}
