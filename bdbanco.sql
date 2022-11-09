create database bdbanco;
use bdbanco;

create table tbUsuario(
	UsuarioID int primary key auto_increment,
    UsuNome varchar(100) unique not null,
    Login varchar(50) unique not null,
    Senha varchar(100) not null
);

-- método InsertUsuario
delimiter $$
create procedure spInsertUsuario(vNome varchar(100), vLogin varchar(50), vSenha varchar(100))
begin
	insert into tbUsuario (UsuNome, Login, Senha)
	values (vNome, vLogin, vSenha);
End $$

-- método SelectLogin
delimiter $$
create procedure spSelectLogin(vLogin varchar(50))
begin
	select Login from tbUsuario where Login = vLogin;
End $$

-- método SelectUsuario
delimiter $$
create procedure spSelectUsuario(vLogin varchar(50))
begin
	select * from tbUsuario where Login = vLogin;
End $$

-- método UpdateSenha
delimiter $$
create procedure spUpdateSenha(vLogin varchar(50), vSenha varchar(100))
begin
	update tbUsuario set Senha = vSenha where Login = vLogin;
End $$

select * from tbusuario;