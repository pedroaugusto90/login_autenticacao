using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace appLogin.Models
{
    public class Usuario
    {

        public int UsuarioId { get; set; }

        [Required]
        [MaxLength(100)]
        public string UsuNome { get; set; }

        [Required]
        [MaxLength(50)]
        [Remote("SelectLogin", "Autenticacao", ErrorMessage = "O login já existe.")]
        public string Login { get; set; }

        [Required]
        [MaxLength(100)]
        public string Senha { get; set; }

        MySqlConnection conexao = new MySqlConnection(ConfigurationManager.ConnectionStrings["conexao"].ConnectionString);
        MySqlCommand command = new MySqlCommand();

        public void InsertUsuario(Usuario usuario)
        {
            conexao.Open();
            command.CommandText = "call spInsertUsuario(@UsuNome,@Login,@Senha);";
            command.Parameters.Add("@UsuNome", MySqlDbType.VarChar).Value = usuario.UsuNome;
            command.Parameters.Add("@Login", MySqlDbType.VarChar).Value = usuario.Login;
            command.Parameters.Add("@Senha", MySqlDbType.VarChar).Value = usuario.Senha;
            command.Connection = conexao;
            command.ExecuteNonQuery();
            conexao.Close();
        }
        public string SelectLogin(string vLogin)
        {
            conexao.Open();
            command.CommandText = "call spSelectLogin(@Login);";
            command.Parameters.Add("@Login", MySqlDbType.VarChar).Value = vLogin;
            command.Connection = conexao;
            string Login = (string)command.ExecuteScalar();
            conexao.Close();
            if (Login == null)
                Login = "";
            return Login;
        }

        public Usuario SelectUsuario(string vLogin)
        {
            conexao.Open();
            command.CommandText = "call spSelectUsuario(@Login);";
            command.Parameters.Add("@Login", MySqlDbType.VarChar).Value = vLogin;
            
            command.Connection = conexao;
            var readUsuario = command.ExecuteReader();
            var TempUsuario = new Usuario();

            if (readUsuario.Read())
            {
                TempUsuario.UsuarioId = int.Parse(readUsuario["UsuarioID"].ToString());
                TempUsuario.UsuNome = readUsuario["UsuNome"].ToString();
                TempUsuario.Login = readUsuario["Login"].ToString();
                TempUsuario.Senha = readUsuario["Senha"].ToString();
            };
            readUsuario.Close();
            conexao.Close();
            return TempUsuario;

        }

        public void UpdateSenha (Usuario usuario)
        {
            conexao.Open();
            command.CommandText = "call spUpdateSenha(@Login, @Senha);";
            command.Parameters.Add("@Login", MySqlDbType.VarChar).Value = usuario.Login;
            command.Parameters.Add("@Senha", MySqlDbType.VarChar).Value = usuario.Senha;
            command.Connection = conexao;
            command.ExecuteNonQuery();
            conexao.Close();
        }

    }
}

