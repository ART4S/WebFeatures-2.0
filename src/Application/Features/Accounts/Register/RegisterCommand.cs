﻿using Application.Infrastructure.Requests;

namespace Application.Features.Accounts.Register
{
    /// <summary>
    /// Зарегистрировать новго пользователя
    /// </summary>
    public class RegisterCommand : CommandBase<UserCreateDto>
    {
        /// <summary>
        /// Логин
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// E-mail
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }
    }
}
