using WAPI_GS.Modelos;

namespace WAPI_GS.Dto.UserSala
{
    public class DtoGetUserSala
    {
        public DateOnly Dia { get; set; }
        public int HoraInit { get; set; }
        public int HoraFinal { get; set; }
        public List<SalaComProfessores> Salas { get; set; }  // Lista de salas e seus professores

        public class SalaComProfessores
        {
            public int SalaId { get; set; }  // Id da sala
            public TblSala TblSala { get; set; }  // Detalhes da sala
            public List<TblUser> Professores { get; set; }  // Lista de professores alocados na sala
        }
    }
}
