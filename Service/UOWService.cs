using WAPI_GS.Infra.Professor;
using WAPI_GS.Interfaces;
using WAPI_GS.Repositorios.Disciplina;
using WAPI_GS.Repositorios.ProfessorSala;
using WAPI_GS.Repositorios.Salas;
using WAPI_GS.Repositorios.Turma;

namespace WAPI_GS.Service
{
    public class UOWService(
        ISalaRepository salaRepository,
        IProfessorRepository professorRepository,
        IDisciplinaRepository disciplinaRepository,
        IProfessorSalaRepository professorSalaRepository,
        ITurmaRepository turmaRepository
        ) : IUnitOfWork
    {
        public ISalaService SalaService => new SalaService(salaRepository);

        public IProfessorService ProfessorService => new ProfessorService(professorRepository);

        public IDisciplinaService DisciplinaService => new DisciplinaService(disciplinaRepository);
        public ITurmaService TurmaService => new TurmaServiceImpl(turmaRepository);

        public IAtribuicaoService AtribuicaoService => new AtribuicaoService(
            professorSalaRepository, disciplinaRepository, salaRepository, professorRepository, turmaRepository);
    }
}
