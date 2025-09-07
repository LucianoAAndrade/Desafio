using Domain.Enums;

namespace Domain.Models
{
    public class EmprestimoModel : EntityModel
    {
        public int LivroId { get; set; }
        public virtual LivroModel Livro { get; set; }
        public DateTime DataEmprestimo { get; set; }
        public DateTime? DataDevolucao { get; set; }
        public StatusLivroEnum Status { get; set; }

        public EmprestimoModel() { }

        public EmprestimoModel(int livroId, DateTime dataEmprestimo,
            DateTime? dataDevolucoa, StatusLivroEnum status, LivroModel livro)
        {
            LivroId = livroId;
            DataEmprestimo = dataEmprestimo;
            DataDevolucao = dataDevolucoa;
            Status = status;
            Livro = livro;
        }

        public EmprestimoModel(int idEmprestimo, DateTime dataDevolucao,
            StatusLivroEnum status, LivroModel livro)
        {
            Id = idEmprestimo;
            DataEmprestimo = DateTime.Now;
            DataDevolucao = dataDevolucao;
            Status = status;
            Livro = livro;
        }
    }
}
