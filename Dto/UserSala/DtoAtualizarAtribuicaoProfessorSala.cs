using WAPI_GS.Modelos;

namespace WAPI_GS.Dto.UserSala
{
    public class DtoAtualizarAtribuicaoProfessorSala
    {
        public int UserId { get; set; }
        public int HoraInicial { get; set; }
        public int HoraFinal { get; set; }
        public required DateOnly DiaCorrente { get; set; }

        public TblPtd ToEntity(int SalaId, int DisciplinaID, string turmaId)
        {
            return new TblPtd
            {
                UserId = this.UserId,
                SalaId = SalaId,
                DisciplinaId = DisciplinaID,
                Dia = this.DiaCorrente,
                HoraInicial = this.HoraInicial,
                HoraFinal = this.HoraFinal,
                TurmaId = turmaId
            };
        }
    }
}
