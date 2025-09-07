namespace Domain.Utils.OperationResult
{
    public class ResultOperationBase
    {
        private const string mensagemPadraoSucesso = "Operação realizada com sucesso.";

        private const string mensagemPadraoNaoSucesso = "Operação não realizada.";

        private string mensagemPrincipal = string.Empty;

        public bool Sucesso { get; set; }

        public int StatusCode { get; set; }

        public List<string> Mensagens { get; set; } = new List<string>();


        public string MensagemPrincipal
        {
            get
            {
                if (ResultOperationValidateUtil.IsNull(mensagemPrincipal))
                {
                    if (!Sucesso)
                    {
                        return mensagemPadraoNaoSucesso;
                    }

                    return mensagemPadraoSucesso;
                }

                return mensagemPrincipal;
            }
            set
            {
                mensagemPrincipal = value;
            }
        }

        public ResultOperationBase()
        {
            Sucesso = false;
            StatusCode = 200;
        }
    }
}