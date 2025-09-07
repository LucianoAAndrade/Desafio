using Domain.Enums;

namespace Application.Responses.Emprestimo
{
    public class GetAllEmprestimosResponse
    {
        public int IdEmprestimo { get; set; }
        public string TituloLivro { get; set; }
        public string AutorLivro { get; set; }
        public DateTime DataEmprestimo { get; set; }
        public DateTime? DataDevolucao { get; set; }
        public string Status { get; set; }
    }
}
