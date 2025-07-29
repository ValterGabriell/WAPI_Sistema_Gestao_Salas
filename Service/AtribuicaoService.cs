using Microsoft.EntityFrameworkCore;
using WAPI_GS.Dto.UserSala;
using WAPI_GS.Infra.Professor;
using WAPI_GS.Interfaces;
using WAPI_GS.Modelos;
using WAPI_GS.Repositorios.Disciplina;
using WAPI_GS.Repositorios.Email;
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
        ITurmaRepository turmaRepository,
        IEmailRepository emailRepository,
        AppDbContext appDbContext) : IAtribuicaoService
    {
        private readonly IProfessorSalaRepository _professorSalaRepository = professorSalaRepository;
        private readonly IDisciplinaRepository _disciplinaRepository = disciplinaRepository;
        private readonly ISalaRepository _salaRepository = salaRepository;
        private readonly IProfessorRepository _professorRepository = professorRepository;
        private readonly IEmailRepository _emailRepository = emailRepository;
        private readonly ITurmaRepository _turmaRepository = turmaRepository;
        private readonly AppDbContext context = appDbContext;


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
                var query = from ptd in context.TblUsersSala.AsNoTracking()
                            join sala in context.TblSalas.AsNoTracking() on ptd.SalaId equals sala.Id
                            join disciplina in context.TblDisciplina.AsNoTracking() on ptd.DisciplinaId equals disciplina.Id
                            join professor in context.TblUsers.AsNoTracking() on ptd.UserId equals professor.Id
                            join turma in context.TblTurma.AsNoTracking() on ptd.TurmaId equals turma.Id
                            select new
                            {
                                ptd.Dia,
                                SalaComProfessores = new DtoGetUserSala.SalaComProfessores
                                {
                                    SalaId = ptd.SalaId,
                                    TblSala = sala,
                                    HoraInit = ptd.HoraInicial,
                                    HoraFinal = ptd.HoraFinal,
                                    Professor = professor,
                                    Disciplina = disciplina,
                                    Turma = turma
                                }
                            };

                var agrupado = await query
                    .GroupBy(x => x.Dia)
                    .Select(g => new DtoGetUserSala
                    {
                        Dia = g.Key,
                        Salas = g.Select(x => x.SalaComProfessores).ToList()
                    })
                    .ToListAsync();

                return agrupado;
            }
            catch (Exception ex)
            {
                throw new Exception(HelperExceptions.CreateExceptionMessage(ex));
            }
        }

        public async Task<bool> SendEmailSolicitacao(
          string destEmail,
            string body,
            string title,
            string fullUrl,
            int salaId,
            DateOnly dia,
            int antigoUsuarioID,
            int novoUsuarioID,
        int horaInit,
        int horaFinal)
        {
            try
            {
                return await _emailRepository
                    .SendEmailSolicitacao(destEmail, body, title, fullUrl, salaId, dia,
                    antigoUsuarioID, novoUsuarioID, horaInit, horaFinal);
            }
            catch (Exception ex)
            {
                throw new Exception(HelperExceptions.CreateExceptionMessage(ex));
            }
        }

        public async Task<bool> Accept(
           int salaId, DateOnly dia, int antigoUsuarioID,
             int novoUsuarioID, int horaInit, int horaFinal)
        {
            try
            {
                return await _emailRepository
                    .Accept(salaId, dia, antigoUsuarioID, novoUsuarioID, horaInit, horaFinal);
            }
            catch (Exception ex)
            {
                throw new Exception(HelperExceptions.CreateExceptionMessage(ex));
            }
        }

        public async Task<bool> NotAccept(
          int salaId, int usuarioQueSolicitou)
        {
            try
            {
                return await _emailRepository
                    .NotAccept(salaId, usuarioQueSolicitou);
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


