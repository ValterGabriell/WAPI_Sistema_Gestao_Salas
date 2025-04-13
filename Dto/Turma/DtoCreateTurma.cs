using WAPI_GS.Modelos;
using WAPI_GS.Utilidades;

namespace WAPI_GS.Dto.Turma
{
    public class DtoCreateTurma
    {
        public EnumTurnoTurma Turno { get; set; } = EnumTurnoTurma.MATUTINO;
        public int Bloco { get; set; }
        public string Nome { get; set; } = string.Empty;

        public TblTurma ToEntity(string id)
        {
            return new TblTurma
            {
                Id = id,
                Turno = Turno.ToString(),
                Bloco = Bloco,
                Nome = Nome
            };
        }
    }
}
