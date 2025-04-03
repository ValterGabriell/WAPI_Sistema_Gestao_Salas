using WAPI_GS.Modelos;

namespace WAPI_GS.Dto.Disciplina
{
    public class DtoCreateDisciplina
    {
        public string Nome { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public string Sigla { get; set; } = string.Empty;
        public int CargaHoraria { get; set; } = 0;
        public string TurmaId { get; set; } = string.Empty;

        public TblDisciplina ToEntity()
        {
            TblDisciplina newDisciplina = new()
            {
                Codigo = this.Codigo,
                Nome = this.Nome,
                Sigla = this.Sigla,
                CargaHoraria = this.CargaHoraria,
                TotalAulas = this.CargaHoraria / 4,
                TurmaId = this.TurmaId
            };
            return newDisciplina;
        }

        public TblDisciplina ToEntityForUpdate(int id)
        {
            TblDisciplina newDisciplina = new()
            {
                Id = id,
                Codigo = this.Codigo,
                Nome = this.Nome,
                Sigla = this.Sigla,
                CargaHoraria = this.CargaHoraria,
                TotalAulas = this.CargaHoraria / 4,
                TurmaId = this.TurmaId,
            };
            return newDisciplina;
        }
    }
}
