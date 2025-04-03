using WAPI_GS.Modelos;
using WAPI_GS.Utilidades;

namespace WAPI_GS.Repositorios.ProfessorSala
{
    public class ProfessorSalaRepositoryImpl(AppDbContext appDbContext)
    {
        private readonly AppDbContext _appDbContext = appDbContext;


        public string AtribuirProfessorASala(TblPtd entity)
        {
            _appDbContext.Add(entity);
            return HelperMessages.ATRIBUICAO_FEITA;
        }

        public string AtualizarAtribuicaoProfessorSala(TblPtd entity)
        {
            _appDbContext.Update(entity);
            return HelperMessages.ATRIBUICAO_FEITA;
        }

        public void RemoverAtribuicaoProfessorSala(TblPtd entity)
        {
            _appDbContext.Remove(entity);
        }
    }
}


