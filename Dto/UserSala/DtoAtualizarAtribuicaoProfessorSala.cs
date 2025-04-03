using WAPI_GS.Modelos;

namespace WAPI_GS.Dto.UserSala
{
    public class DtoAtualizarAtribuicaoProfessorSala
    {
        public int UserId { get; set; }
        public int HoraInicial { get; set; }
        public int HoraFinal { get; set; }
        public required string DiaCorrente { get; set; }

        public TblPtd ToEntity(int SalaId, int DisciplinaID)
        {
            return new TblPtd
            {
                UserId = this.UserId,
                SalaId = SalaId,
                DisciplinaId = DisciplinaID,
                Dia = this.Dia,
                HoraInicial = this.HoraInicial,
                HoraFinal = this.HoraFinal,
            };
        }
    }
}
