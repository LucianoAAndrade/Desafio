using Application.Handlers;
using Application.Handlers.LivrosHendler;
using Application.Requests.Emprestimo;
using Application.Requests.Livros;
using Application.Responses.Emprestimo;
using Application.Responses.Livros;
using Domain.Interfeces;
using Domain.Utils.OperationResult;
using Infra.Data.Config;
using Infra.Data.Repository;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra.CrossCutting.IoC
{
    public static class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration? configuration = null)
        {
            if (configuration != null)
                services.AddSingleton(configuration);
            #region Repositories
            services.AddScoped<ILivroRepository, LivroRepository>();
            services.AddScoped<IEmprestimoRepository, EmprestimoRepository>();
            #endregion

            #region Handlers

            #region Livro
            services.AddScoped<IRequestHandler<GetAllLivrosRequest, ResultOperation<List<GetAllLivrosResponse>>>, GetAllLivrosHandler>();
            services.AddScoped<IRequestHandler<GetLivroIdRequest, ResultOperation<GetLivroIdResponse>>, GetLivroIdHandler>();
            services.AddScoped<IRequestHandler<PostLivroRequest, ResultOperationBase>, PostLivroHandler>();
            #endregion

            #region Emprestimo
            services.AddScoped<IRequestHandler<GetAllEmprestimosRequest, ResultOperation<List<GetAllEmprestimosResponse>>>, GetAllEmprestimosHandler>();
            services.AddScoped<IRequestHandler<PostEmprestimoRequest, ResultOperation<PostEmprestimoResponse>>, PostEmprestimoHandler>();
            services.AddScoped<IRequestHandler<PutEmprestimoRequest, ResultOperation<PutEmprestimoResponse>>, PutEmprestimoHandler>();
            #endregion

            #endregion

            #region Dependencies
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<Context>();
            #endregion
        }
    }
}
