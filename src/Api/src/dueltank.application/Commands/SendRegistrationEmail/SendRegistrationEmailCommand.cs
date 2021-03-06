﻿using MediatR;

namespace dueltank.application.Commands.SendRegistrationEmail
{
    public class SendRegistrationEmailCommand : IRequest<CommandResult>
    {
        public string Email { get; set; }
        public string CallBackUrl { get; set; }
        public string Username { get; set; }
    }
}