namespace WAPI_GS.Interfaces
{
    public interface IUnitOfWork
    {
        ISalaService SalaService { get; }
        IProfessorService ProfessorService { get; }
        IAtribuicaoService AtribuicaoService { get; }
        IDisciplinaService DisciplinaService { get; }
        ITurmaService TurmaService { get; }
    }
}
