using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using WAPI_GS.Modelos;

namespace WAPI_GS.Repositorios.Email
{
    public class EmailRepositoryImpl(AppDbContext appDbContext, IConfiguration configuration) : IEmailRepository
    {
        private readonly AppDbContext _appDbContext = appDbContext;
        private readonly IConfiguration _configuration = configuration;

        public async Task<bool> SendEmailSolicitacao(string destEmail,
            string body,
            string title,
            string fullUrl,
            int salaId,
            DateOnly dia,
            int antigoUsuarioID,
            int novoUsuarioID,
        int horaInit,
        int horaFinal
        )
        {

            try
            {
                int year = dia.Year;
                int month = dia.Month;
                int day = dia.Day;
                string formattedDate = dia.ToString("yyyy-MM-dd"); // Formato seguro para URL
                var smtpSettings = _configuration.GetSection("SmtpSettings");

                using (SmtpClient client = new SmtpClient(smtpSettings["Host"], int.Parse(smtpSettings["Port"])))
                {
                    client.Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]);
                    client.EnableSsl = bool.Parse(smtpSettings["EnableSsl"]);

                    string emailBody = $@"
<html>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Confirmação de Reserva de Sala</title>
    <style>
        body {{
            font-family: 'Segoe UI', Arial, sans-serif;
            background-color: #f4f6f8;
            margin: 0;
            padding: 0;
        }}
        .container {{
            max-width: 600px;
            margin: 40px auto;
            background: #fff;
            border-radius: 12px;
            box-shadow: 0 4px 24px rgba(0,0,0,0.08);
            padding: 32px 24px;
            text-align: center;
        }}
        .header {{
            display: flex;
            align-items: center;
            justify-content: center;
            margin-bottom: 24px;
        }}
        .logo {{
            height: 40px;
            margin-right: 12px;
        }}
        .title {{
            font-size: 24px;
            color: #2c3e50;
            font-weight: 600;
        }}
        .icon {{
            display: inline-block;
            vertical-align: middle;
            margin-right: 8px;
        }}
        p {{
            color: #555;
            font-size: 17px;
            margin-bottom: 18px;
            line-height: 1.6;
        }}
        .button {{
            display: inline-block;
            padding: 12px 28px;
            margin: 12px 8px;
            text-decoration: none;
            color: #fff;
            border-radius: 6px;
            font-weight: 600;
            font-size: 16px;
            transition: background 0.2s;
            box-shadow: 0 2px 8px rgba(44,62,80,0.08);
        }}
        .accept {{
            background-color: #28a745;
        }}
        .accept:hover {{
            background-color: #218838;
        }}
        .reject {{
            background-color: #dc3545;
        }}
        .reject:hover {{
            background-color: #c82333;
        }}
        .footer {{
            margin-top: 32px;
            font-size: 13px;
            color: #888;
            text-align: center;
        }}
        @media (max-width: 600px) {{
            .container {{
                padding: 16px 8px;
            }}
            .title {{
                font-size: 20px;
            }}
            .button {{
                width: 90%;
                margin: 10px auto;
                display: block;
            }}
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <img src='https://cdn-icons-png.flaticon.com/512/3209/3209265.png' alt='Gestão de Salas' class='logo' />
            <span class='title'>Confirmação de Reserva de Sala</span>
        </div>
        <p>{body}</p>
        <p>Para prosseguir, escolha uma das opções abaixo:</p>
        <a href='{fullUrl}/accept?salaId={salaId}&dia={formattedDate}&antigoUsuarioID={antigoUsuarioID}&novoUsuarioID={novoUsuarioID}&horaInit={horaInit}&horaFinal={horaFinal}' class='button accept'>
            <span class='icon'>✔️</span> Aceitar Reserva
        </a>
        <a href='{fullUrl}/notAccept?salaId={salaId}&usuarioQueSolicitou={novoUsuarioID}' class='button reject'>
            <span class='icon'>❌</span> Recusar Reserva
        </a>
        <div class='footer'>
            Este e-mail foi gerado automaticamente pelo sistema Gestão de Salas.<br>
            &copy; 2025 Gestão de Salas
        </div>
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



        public async Task<bool> Accept(int salaId, DateOnly dia, int antigoUsuarioID,
            int novoUsuarioID, int horaInit, int horaFinal)
        {
            int year = dia.Year;
            int month = dia.Month;
            int day = dia.Day;

            // Formatar como "YYYY-DD-MM"
            string formattedDate = $"{year}-{month:D2}-{day:D2}";
            var tblUsersSala = await _appDbContext.TblUsersSala
                .Where(e => e.SalaId == salaId
                    && e.UserId == antigoUsuarioID
                    && e.Dia == DateOnly.Parse(formattedDate)
                    && e.HoraInicial == horaInit
                    && e.HoraFinal == horaFinal)
                .FirstOrDefaultAsync() ?? throw new Exception("Nenhuma reserva encontrada com os parâmetros fornecidos.");


            TblProfessor usuarioNovo = await _appDbContext.TblUsers.Where(e => e.Id == novoUsuarioID).FirstAsync();
            tblUsersSala.UserId = usuarioNovo.Id;
            _appDbContext.Update(tblUsersSala);
            await _appDbContext.SaveChangesAsync();

            TblProfessor tblUser = await _appDbContext.TblUsers.Where(e => e.Id == usuarioNovo.Id).FirstAsync();
            TblSala tblSala = await _appDbContext.TblSalas.Where(e => e.Id == salaId).FirstAsync();

            string emailBody = $@"
<html>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Solicitação de Troca de Sala Aceita</title>
    <style>
        body {{
            font-family: 'Segoe UI', Arial, sans-serif;
            background-color: #f4f6f8;
            margin: 0;
            padding: 0;
        }}
        .container {{
            max-width: 600px;
            margin: 40px auto;
            background: #fff;
            border-radius: 12px;
            box-shadow: 0 4px 24px rgba(0,0,0,0.08);
            padding: 32px 24px;
            text-align: center;
        }}
        .header {{
            display: flex;
            align-items: center;
            justify-content: center;
            margin-bottom: 24px;
        }}
        .logo {{
            height: 40px;
            margin-right: 12px;
        }}
        .title {{
            font-size: 24px;
            color: #28a745;
            font-weight: 600;
        }}
        .icon {{
            font-size: 40px;
            margin-bottom: 12px;
        }}
        .info {{
            background: #e9f7ef;
            border-radius: 8px;
            padding: 16px;
            margin-bottom: 18px;
            color: #155724;
            font-size: 16px;
        }}
        p {{
            color: #555;
            font-size: 17px;
            margin-bottom: 18px;
            line-height: 1.6;
        }}
        .footer {{
            margin-top: 32px;
            font-size: 13px;
            color: #888;
            text-align: center;
        }}
        @media (max-width: 600px) {{
            .container {{
                padding: 16px 8px;
            }}
            .title {{
                font-size: 20px;
            }}
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <img src='https://cdn-icons-png.flaticon.com/512/3209/3209265.png' alt='Gestão de Salas' class='logo' />
            <span class='title'>Solicitação de Troca de Sala Aceita</span>
        </div>
        <div class='icon'>✔️</div>
        <p>Olá,</p>
        <div class='info'>
            Sua solicitação para troca de sala foi <b>aceita</b>!<br>
            <b>Sala:</b> {tblSala.Name}<br>
            <b>Data:</b> {tblUsersSala.Dia:dd/MM/yyyy}<br>
            <b>Horário:</b> das {tblUsersSala.HoraInicial}:00h até as {tblUsersSala.HoraFinal}:00h
        </div>
        <p>A sala agora está alocada para você. Em caso de dúvidas, entre em contato com a administração.</p>
        <div class='footer'>
            Este e-mail foi gerado automaticamente pelo sistema Gestão de Salas.<br>
            &copy; 2025 Gestão de Salas
        </div>
    </div>
</body>
</html>";



            await SendEmail(tblUser.Email!,
                emailBody,
                tblSala.Name + " Solicitação de troca de sala aceita!"
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

        public async Task<bool> NotAccept(int salaId, int usuarioQueSolicitou)
        {

            TblProfessor tblUser = await _appDbContext.TblUsers.Where(e => e.Id == usuarioQueSolicitou).FirstAsync();
            TblSala tblSala = await _appDbContext.TblSalas.Where(e => e.Id == salaId).FirstAsync();

            string emailBody = $@"
<html>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Solicitação de Troca de Sala Recusada</title>
    <style>
        body {{
            font-family: 'Segoe UI', Arial, sans-serif;
            background-color: #f4f6f8;
            margin: 0;
            padding: 0;
        }}
        .container {{
            max-width: 600px;
            margin: 40px auto;
            background: #fff;
            border-radius: 12px;
            box-shadow: 0 4px 24px rgba(0,0,0,0.08);
            padding: 32px 24px;
            text-align: center;
        }}
        .header {{
            display: flex;
            align-items: center;
            justify-content: center;
            margin-bottom: 24px;
        }}
        .logo {{
            height: 40px;
            margin-right: 12px;
        }}
        .title {{
            font-size: 24px;
            color: #dc3545;
            font-weight: 600;
        }}
        .icon {{
            font-size: 40px;
            margin-bottom: 12px;
        }}
        .info {{
            background: #f8d7da;
            border-radius: 8px;
            padding: 16px;
            margin-bottom: 18px;
            color: #721c24;
            font-size: 16px;
        }}
        p {{
            color: #555;
            font-size: 17px;
            margin-bottom: 18px;
            line-height: 1.6;
        }}
        .footer {{
            margin-top: 32px;
            font-size: 13px;
            color: #888;
            text-align: center;
        }}
        @media (max-width: 600px) {{
            .container {{
                padding: 16px 8px;
            }}
            .title {{
                font-size: 20px;
            }}
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <img src='https://cdn-icons-png.flaticon.com/512/3209/3209265.png' alt='Gestão de Salas' class='logo' />
            <span class='title'>Solicitação de Troca de Sala Recusada</span>
        </div>
        <div class='icon'>❌</div>
        <p>Olá,</p>
        <div class='info'>
            Sua solicitação para troca da sala <b>{tblSala.Name}</b> foi <b>recusada</b>.<br>
            O professor <b>{tblUser.Name}</b> não aceitou sua solicitação.
        </div>
        <p>Se precisar de mais informações, entre em contato com a administração.</p>
        <div class='footer'>
            Este e-mail foi gerado automaticamente pelo sistema Gestão de Salas.<br>
            &copy; 2025 Gestão de Salas
        </div>
    </div>
</body>
</html>";

            await SendEmail(tblUser.Email!,
                emailBody,
                "Troca de Sala Recusada"
            );

            return false;
        }

    }
}
