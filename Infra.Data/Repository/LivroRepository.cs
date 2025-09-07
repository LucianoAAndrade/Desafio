using Domain.Interfeces;
using Domain.Models;
using Infra.Data.Config;
using Infra.Modelo.Repository;

namespace Infra.Data.Repository
{
    public class LivroRepository : Repository<LivroModel>, ILivroRepository
    {
        public LivroRepository(Context context) : base(context)
        {            
        }
    }
    
}
