﻿using WAPI_GS.Modelos;

namespace WAPI_GS.Dto.UserSala
{
    public class DtoAtribuirProfessorASala
    {
        public int UserId { get; set; }
        public int SalaId { get; set; }

        public int DisciplinaId { get; set; }
        public string TurmaID { get; set; }
        public DateOnly DiaDeAulaDaSemana { get; set; }
        public int HoraInicial { get; set; }
        public int HoraFinal { get; set; }

        public TblPtd ToEntity()
        {
            return new TblPtd
            {
                UserId = this.UserId,
                SalaId = this.SalaId,
                DisciplinaId = this.DisciplinaId,
                Dia = this.DiaDeAulaDaSemana,
                HoraInicial = this.HoraInicial,
                HoraFinal = this.HoraFinal,
                TurmaId = this.TurmaID
            };
        }
    }
}

