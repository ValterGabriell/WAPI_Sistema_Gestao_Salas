using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using WAPI_GS.Modelos;

namespace WAPI_GS.Repositorios.Email
{
    public class EmailRepositoryImpl(AppDbContext appDbContext, IConfiguration configuration)
    {
        private readonly AppDbContext _appDbContext = appDbContext;
        private readonly IConfiguration _configuration = configuration;

        public async Task<bool> SendEmailSolicitacao(string destEmail,
            string body,
            string title,
            string fullUrl,
            int salaId,
            DateOnly dia,
            int currentUserId,
            string currentUsername,
        int horaInit,
        int horaFinal
        )
        {

            try
            {
                int year = dia.Year;
                int month = dia.Month;
                int day = dia.Day;
                string formattedDate = $"{day:D2}/{month:D2}/{year}";
                var smtpSettings = _configuration.GetSection("SmtpSettings");

                using (SmtpClient client = new SmtpClient(smtpSettings["Host"], int.Parse(smtpSettings["Port"])))
                {
                    client.Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]);
                    client.EnableSsl = bool.Parse(smtpSettings["EnableSsl"]);

                    string emailBody = $@"
                    <html>
                    <head>
                        <style>
                            body {{
                                font-family: Arial, sans-serif;
                                background-color: #f4f4f4;
                                margin: 0;
                                padding: 20px;
                            }}
                            .container {{
                                max-width: 600px;
                                background: #ffffff;
                                padding: 20px;
                                border-radius: 10px;
                                box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                            }}
                            h2 {{
                                color: #333;
                            }}
                            p {{
                                font-size: 16px;
                                color: #555;
                                line-height: 1.5;
                            }}
                            .button {{
                                display: inline-block;
                                padding: 10px 20px;
                                margin: 10px 5px;
                                text-decoration: none;
                                color: white;
                                border-radius: 5px;
                                font-weight: bold;
                            }}
                            .accept {{
                                background-color: #28a745;
                            }}
                            .reject {{
                                background-color: #dc3545;
                            }}
                            .footer {{
                                margin-top: 20px;
                                font-size: 12px;
                                color: #777;
                                text-align: center;
                            }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <h2>Confirmação de Reserva de Sala</h2>
                            <p>{body}</p>
                            <p>Caso aceite, clique em um dos botões abaixo:</p>
                            <a href='{fullUrl}/accept?salaId={salaId}&dia={formattedDate}&userId={currentUserId}&currentUsername={currentUsername}&horaInit={horaInit}&horaFinal={horaFinal}' class='button accept'>✔ Aceito</a>
                            <a href='{fullUrl}/notAccept?salaId={salaId}' class='button reject'>❌ Não Aceito</a>
                        </div>
                    </body>
                    </html>";

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(smtpSettings["Username"], "Gestão de Salas"),
                        Subject = title,
                        Body = emailBody,
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(destEmail);

                    await client.SendMailAsync(mailMessage);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public async Task<bool> Accept(int salaId, DateOnly dia, int userId, string currentUsername, int horaInit, int horaFinal)
        {
            int year = dia.Year;
            int month = dia.Month;
            int day = dia.Day;

            // Formatar como "YYYY-DD-MM"
            string formattedDate = $"{year}-{month:D2}-{day:D2}";
            var tblUsersSala = await _appDbContext.TblUsersSala
                .Where(e => e.SalaId == salaId
                    && e.UserId == userId
                    && e.Dia == DateOnly.Parse(formattedDate)
                    && e.HoraInicial == horaInit
                    && e.HoraFinal == horaFinal)
                .FirstOrDefaultAsync() ?? throw new Exception("Nenhuma reserva encontrada com os parâmetros fornecidos.");


            TblProfessor user = await _appDbContext.TblUsers.Where(e => e.Username == currentUsername).FirstAsync();
            tblUsersSala.UserId = user.Id;
            _appDbContext.Update(tblUsersSala);
            await _appDbContext.SaveChangesAsync();

            TblProfessor tblUser = await _appDbContext.TblUsers.Where(e => e.Id == user.Id).FirstAsync();
            TblSala tblSala = await _appDbContext.TblSalas.Where(e => e.Id == salaId).FirstAsync();
            await SendEmail(tblUser.Email!,
                " Solicitação para troca de sala aceita! " + tblSala.Name +
                " agora está alocada para você em " +
                tblUsersSala.Dia + " das " + tblUsersSala.HoraInicial + ":00h até as " + tblUsersSala.HoraFinal + ":00h",
                tblSala.Name + "Solicitação de troca de sala aceita!"
                );


            return true;
        }

        public async Task<bool> SendEmail(string destEmail, string body, string title)
        {
            try
            {
                var smtpSettings = _configuration.GetSection("SmtpSettings");

                using (SmtpClient client = new(smtpSettings["Host"], int.Parse(smtpSettings["Port"])))
                {
                    client.Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]);
                    client.EnableSsl = bool.Parse(smtpSettings["EnableSsl"]);

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(smtpSettings["Username"], "Gestão de Salas"),
                        Subject = title,
                        Body = body,
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(destEmail);

                    await client.SendMailAsync(mailMessage);
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> NotAccept(int salaId)
        {
            TblPtd tblUsersSala = await _appDbContext.TblUsersSala.Where(e => e.SalaId == salaId).FirstAsync();
            TblProfessor tblUser = await _appDbContext.TblUsers.Where(e => e.Id == tblUsersSala.UserId).FirstAsync();
            TblSala tblSala = await _appDbContext.TblSalas.Where(e => e.Id == salaId).FirstAsync();

            await SendEmail(tblUser.Email!,
                $"Solicitação para troca de sala recusada! {tblSala.Name} - O professor {tblUser.Name} não aceitou sua solicitação",
                ""
            );

            return false;
        }

    }
}
