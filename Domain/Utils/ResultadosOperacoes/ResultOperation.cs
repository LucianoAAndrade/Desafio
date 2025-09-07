namespace Domain.Utils.OperationResult
{
    public class ResultOperation<T> : ResultOperationBase
    {
        public T Modelo { get; set; }

        public ResultOperation()
        {
            base.Sucesso = false;
        }
    }
}