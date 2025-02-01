using Microsoft.EntityFrameworkCore;
using WAPI_GS.Dto.Disciplina;
using WAPI_GS.Interfaces;
using WAPI_GS.Modelos;

namespace WAPI_GS.Service
{
    public class DisciplinaService(AppDbContext appDbContext) : ICS_Disciplina
    {
        private readonly AppDbContext _appDbContext = appDbContext;
        public async Task<string> Create(DtoCreateDisciplina dto, string requestKey)
        {
            try
            {
                bool requestValid = await ValidateRequestToken.Validate(_appDbContext, requestKey);
                if (!requestValid)
                {
                    throw new Exception("000-Token Inválido");
                }

                TblDisciplina? tblDisciplina = await _appDbContext.TblDisciplina.Where(x => x.Codigo == dto.Codigo).FirstOrDefaultAsync();
                if (tblDisciplina != null)
                {
                    throw new Exception("Disciplina já cadastrada");
                }

                TblDisciplina tblDisciplina1 = new TblDisciplina
                {
                    Codigo = dto.Codigo,
                    Nome = dto.Nome,
                    Sigla = dto.Sigla,
                    CargaHoraria = dto.CargaHoraria,
                    TotalAulas = dto.CargaHoraria / 4
                };

                _appDbContext.Add(tblDisciplina1);
                await _appDbContext.SaveChangesAsync();
                return "Disciplina cadastrada com sucesso!";

            }
            catch (Exception ex)
            {

                throw new Exception(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }

        public async Task<string> Update(DtoCreateDisciplina dto, int id, string requestKey)
        {
            try
            {
                bool requestValid = await ValidateRequestToken.Validate(_appDbContext, requestKey);
                if (!requestValid)
                {
                    throw new Exception("000-Token Inválido");
                }

                TblDisciplina? tblDisciplina = await _appDbContext.TblDisciplina.Where(x => x.Codigo == dto.Codigo).FirstOrDefaultAsync();
                if (tblDisciplina != null)
                {
                    throw new Exception("Disciplina já cadastrada");
                }

                TblDisciplina? tblDisciplinaUpdate = await _appDbContext.TblDisciplina.Where(x => x.Id == id).FirstOrDefaultAsync();

                if (tblDisciplinaUpdate == null)
                {
                    throw new Exception("Disciplina não encontrada");
                }


                tblDisciplinaUpdate.Codigo = dto.Codigo;
                tblDisciplinaUpdate.Nome = dto.Nome;
                tblDisciplinaUpdate.Sigla = dto.Sigla;
                tblDisciplinaUpdate.Codigo = dto.Codigo;
                tblDisciplinaUpdate.CargaHoraria = dto.CargaHoraria;
                tblDisciplinaUpdate.TotalAulas = dto.CargaHoraria / 4;
                tblDisciplinaUpdate.Id = id;


                _appDbContext.Update(tblDisciplinaUpdate);
                await _appDbContext.SaveChangesAsync();
                return "Disciplina atualizada com sucesso!";

            }
            catch (Exception ex)
            {

                throw new Exception(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }

        public async Task<List<TblDisciplina>> GetList(string requestKey)
        {
            try
            {
                bool requestValid = await ValidateRequestToken.Validate(_appDbContext, requestKey);
                if (!requestValid)
                {
                    throw new Exception("000-Token Inválido");
                }
                return await _appDbContext.TblDisciplina.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
            }
        }

    }
}
