namespace WAPI_GS.Interfaces
{
    public interface IUnitOfWork
    {
        ISalaService SalaService { get; }
        IProfessorService ProfessorService { get; }
        ILoginService AuthService { get; }
        IAtribuicaoService AtribuicaoService { get; }
        IDisciplinaService DisciplinaService { get; }
        ITurmaService TurmaService { get; }
    }
}
