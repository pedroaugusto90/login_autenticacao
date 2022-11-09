using appLogin.Models;
using appLogin.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using appLogin.Utils;
using System.Security.Claims;

namespace appLogin.Controllers
{
    public class AutenticacaoController : Controller
    {
        // GET: Autenticacao
        [HttpGet]
        public ActionResult Insert()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Insert(CadastroUsuarioViewModel viewmodel)
        {
            if(!ModelState.IsValid)
                return View(viewmodel);

            Usuario novousuario = new Usuario
            {
                UsuNome = viewmodel.UsuNome,
                Login = viewmodel.Login,
                Senha = Hash.GerarHash(viewmodel.Senha)
            };
            novousuario.InsertUsuario(novousuario);
            TempData["MensagemLogin"] = "Parabéns! Seu cadastro foi realizado. Faça o Login.";

            return RedirectToAction("Login","Autenticacao");
        }

        public ActionResult SelectLogin(string Login)
        {
            bool LoginExists;
            string login = new Usuario().SelectLogin(Login);

            if (login.Length == 0)
                LoginExists = false;
            else
                LoginExists = true;

            return Json(!LoginExists, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Login(string ReturnUrl)
        {
            var viewmodel = new LoginViewModel
            {
                UrlRetorno = ReturnUrl
            };


            return View(viewmodel);
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewmodel);
            }
            Usuario usuario = new Usuario();
            usuario = usuario.SelectUsuario(viewmodel.Login);

            if (usuario == null | usuario.Login != viewmodel.Login)
            {
                ModelState.AddModelError("Login", "Login Incorreto");
                return View(viewmodel);
            }

            if (usuario.Senha != Hash.GerarHash(viewmodel.Senha))
            {
                ModelState.AddModelError("Senha", "Senha Incorreta");
                return View(viewmodel);
            }

            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name,usuario.Login),
                new Claim("Login",usuario.Login)
            }, "AppAplicationCookie");

            Request.GetOwinContext().Authentication.SignIn(identity);

            if (!String.IsNullOrWhiteSpace(viewmodel.UrlRetorno) || Url.IsLocalUrl(viewmodel.UrlRetorno))
                return Redirect(viewmodel.UrlRetorno);
            else
                return RedirectToAction("Index","Home");
        }

        public ActionResult Logout()
        {
            Request.GetOwinContext().Authentication.SignOut("AppAplicationCookie");
            return RedirectToAction("Index","Home");
        }

        [Authorize]
        public ActionResult AlterarSenha()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult AlterarSenha(AlterarSenhaViewModel viewmodel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }



            var identity = User.Identity as ClaimsIdentity;
            var login = identity.Claims.FirstOrDefault(c => c.Type == "Login").Value;



            Usuario usuario = new Usuario();
            usuario = usuario.SelectUsuario(login);



            if (Hash.GerarHash(viewmodel.novaSenha) == usuario.Senha)
            {
                ModelState.AddModelError("SenhaNova", "A senha antiga e a nova não deve ser a mesma");
                return View();
            }



            if (Hash.GerarHash(viewmodel.senhaAtual) != usuario.Senha)
            {
                ModelState.AddModelError("SenhaAtual", "A senha atual está incorreta");
                return View();
            }

            usuario.Senha = Hash.GerarHash(viewmodel.novaSenha);

            usuario.UpdateSenha(usuario);

            TempData["MensagemLogin"] = "Senha alterada com sucesso!";
            return RedirectToAction("Index", "Home");
        }

    }
}