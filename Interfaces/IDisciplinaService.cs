﻿using WAPI_GS.Dto;
using WAPI_GS.Dto.Disciplina;
using WAPI_GS.Modelos;

namespace WAPI_GS.Interfaces
{
    public interface IDisciplinaService
    {
        Task<string> Create(DtoCreateDisciplina dto);
        Task<string> UpdateAsync(DtoCreateDisciplina dto, int id);
        Task<List<TblDisciplina>> GetList();
        Task<List<DtoGetCombo>> GetListCombo();

    }
}
