using System.ComponentModel.DataAnnotations;

namespace appLogin.ViewModel
{
    public class AlterarSenhaViewModel
    {
        [Display(Name = "Senha atual")]
        [Required(ErrorMessage = "Informe a senha atual")]
        [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres")]
        [DataType(DataType.Password)]
        public string senhaAtual { get; set; }

        [Display(Name = "Nova senha")]
        [Required(ErrorMessage = "Informe a nova senha")]
        [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres")]
        [DataType(DataType.Password)]
        public string novaSenha { get; set; }

        [Display(Name = "Confirmar senha")]
        [Required(ErrorMessage = "Confirme a senha")]
        [DataType(DataType.Password)]
        [Compare(nameof(novaSenha), ErrorMessage = "As senhas não coincidem")]
        public string confirmaSenha { get; set; }
    }
}