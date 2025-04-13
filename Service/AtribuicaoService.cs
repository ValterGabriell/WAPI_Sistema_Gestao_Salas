using WAPI_GS.Dto.UserSala;
using WAPI_GS.Infra.Professor;
using WAPI_GS.Interfaces;
using WAPI_GS.Modelos;
using WAPI_GS.Repositorios.Disciplina;
using WAPI_GS.Repositorios.ProfessorSala;
using WAPI_GS.Repositorios.Salas;
using WAPI_GS.Repositorios.Turma;
using WAPI_GS.Utilidades;

namespace WAPI_GS.Service
{
    public class AtribuicaoService(
        IProfessorSalaRepository professorSalaRepository,
        IDisciplinaRepository disciplinaRepository,
        ISalaRepository salaRepository,
        IProfessorRepository professorRepository,
        ITurmaRepository turmaRepository) : IAtribuicaoService
    {
        private readonly IProfessorSalaRepository _professorSalaRepository = professorSalaRepository;
        private readonly IDisciplinaRepository _disciplinaRepository = disciplinaRepository;
        private readonly ISalaRepository _salaRepository = salaRepository;
        private readonly IProfessorRepository _professorRepository = professorRepository;
        private readonly ITurmaRepository _turmaRepository = turmaRepository;


        public async Task<DtoResponseCreate> AtribuirProfessorASala(DtoAtribuirProfessorASala dto)
        {
            TblDisciplina tblDisciplina = await _disciplinaRepository.RecuperaDisciplinaPorIDELancaExcecaoSeNaoAchar(dto.DisciplinaId);
            int quantidadeTotalAulas = tblDisciplina.TotalAulas;

            TblPtd tblProfessorSala = InicializaEntidade(dto);

            List<string> listaEntidadesQueNaoForamSalvas = await ProcessarAtribuicaoProfessor(dto, quantidadeTotalAulas, tblProfessorSala);

            return new DtoResponseCreate
            {
                message = "Entidade gerada!",
                errors = listaEntidadesQueNaoForamSalvas
            };
        }


        public async Task<string> AtualizarAtribuicaoProfessorASala(DtoAtualizarAtribuicaoProfessorSala dto,
            int previousUserId, int SalaId)
        {
            try
            {
                TblPtd tblUsersSala = await _professorSalaRepository.RecuperarProfessorParaAtualizacaoSalaELancaExcecaoSeNaoEncontrar(dto, previousUserId, SalaId);

                tblUsersSala.UserId = dto.UserId;
                tblUsersSala.HoraInicial = dto.HoraInicial;
                tblUsersSala.HoraFinal = dto.HoraFinal;

                await _professorSalaRepository.AtualizarAtribuicaoProfessorSala(tblUsersSala);
                return tblUsersSala.SalaId.ToString() + "-> Atualizado para professor: " + tblUsersSala.UserId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task RemoverAtribuicaoProfessorSala(int userId, int salaId, string turmaID, DateOnly dia)
        {
            try
            {
                TblPtd tblProfessorSalaEntity = await _professorSalaRepository
                    .RecuperarProfessorSalaParaDiaParaDeletar(userId, salaId, turmaID, dia);
                await _professorSalaRepository.RemoverAtribuicaoProfessorSala(tblProfessorSalaEntity);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task RemoverTodasAtribuicaoProfessorSala(int userId, int salaId, string turmaID)
        {
            try
            {
                List<TblPtd> tblProfessorSalaEntity = await _professorSalaRepository
                    .RecuperarTodosProfessorSalaParaDiaParaDeletar(userId, salaId, turmaID);

                foreach (var item in tblProfessorSalaEntity)
                {
                    await _professorSalaRepository.RemoverAtribuicaoProfessorSala(item);
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<List<DtoGetUserSala>> GetList()
        {
            try
            {
                List<TblPtd> tblProfessorSalaList = await _professorSalaRepository.RecuperaTodasAsAtribuicoes();

                List<DtoGetUserSala> dtoResponse = [];
                // Itera sobre as TblUsersSalas
                foreach (var atribuicaoAtual in tblProfessorSalaList)
                {
                    DtoGetUserSala? diaExistenteNoDtoResposta = dtoResponse
                        .FirstOrDefault(e => e.Dia == atribuicaoAtual.Dia);

                    // Caso o dia não exista, cria um novo DTO
                    if (diaExistenteNoDtoResposta == null)
                    {
                        diaExistenteNoDtoResposta = new DtoGetUserSala
                        {
                            Dia = atribuicaoAtual.Dia,
                            Salas = []
                        };
                        dtoResponse.Add(diaExistenteNoDtoResposta);
                    }

                    TblSala tblSalaDetails = await _salaRepository.GetByIdAsync(atribuicaoAtual.SalaId);
                    TblDisciplina tblDisciplinaDetails = await _disciplinaRepository.GetByIdAsync(atribuicaoAtual.DisciplinaId);
                    TblProfessor tblProfessorDetails = await _professorRepository.GetByIdAsync(atribuicaoAtual.UserId);
                    TblTurma tblTurma = await _turmaRepository.GetByIdAsync(atribuicaoAtual.TurmaId!);

                    // Cria um objeto SalaComProfessores
                    DtoGetUserSala.SalaComProfessores salaComProfessores = new()
                    {
                        SalaId = atribuicaoAtual.SalaId,
                        TblSala = tblSalaDetails,
                        HoraInit = atribuicaoAtual.HoraInicial,
                        HoraFinal = atribuicaoAtual.HoraFinal,
                        Professor = tblProfessorDetails,
                        Disciplina = tblDisciplinaDetails,
                        Turma = tblTurma
                    };

                    // Adiciona a sala com os professores no DTO Corrente do dia
                    diaExistenteNoDtoResposta.Salas.Add(salaComProfessores);
                }
                return dtoResponse;
            }
            catch (Exception ex)
            {
                throw new Exception(HelperExceptions.CreateExceptionMessage(ex));
            }
        }


        private async Task<List<string>> ProcessarAtribuicaoProfessor(
            DtoAtribuirProfessorASala dto, int totalAulas, TblPtd tblProfessorSala)
        {
            try
            {
                List<string> listaEntidadesQueNaoForamSalvas = [];
                for (var i = 0; i < totalAulas; i++)
                {
                    bool jaExisteAulaPraEsseDia = _professorSalaRepository.VerificaSeEntidadeJaEstaAgendadaParaODia(dto.DiaDeAulaDaSemana,
                                                                                                                      dto.HoraInicial,
                                                                                                                      dto.HoraFinal);
                    if (jaExisteAulaPraEsseDia)
                    {
                        listaEntidadesQueNaoForamSalvas
                               .Add("Dia: " +
                                dto.DiaDeAulaDaSemana + " com horário inicial " +
                                dto.HoraInicial + " e hora final " +
                                dto.HoraFinal + " já cadastrado!");
                        AtualizaDiaParaProximaSemana(dto);
                        continue;
                    }
                    var ID = Guid.NewGuid();
                    tblProfessorSala.Id = ID.ToString();
                    tblProfessorSala.Dia = dto.DiaDeAulaDaSemana;
                    await _professorSalaRepository.AtribuirProfessorASala(tblProfessorSala);
                    AtualizaDiaParaProximaSemana(dto);
                }

                return listaEntidadesQueNaoForamSalvas;
            }
            catch (Exception ex)
            {
                throw new Exception(HelperExceptions.CreateExceptionMessage(ex));
            }
        }

        private static void AtualizaDiaParaProximaSemana(DtoAtribuirProfessorASala dto)
        {
            /**
            * adiciona 7 dias para a proxima iteração, representando a data de aula da proxima semana
            */
            dto.DiaDeAulaDaSemana = dto.DiaDeAulaDaSemana.AddDays(7);
        }

        private static TblPtd InicializaEntidade(DtoAtribuirProfessorASala dto)
        {
            TblPtd tblPtd = dto.ToEntity();
            return tblPtd;
        }
    }
}


