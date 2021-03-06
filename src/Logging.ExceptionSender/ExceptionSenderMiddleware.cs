﻿namespace Logging.ExceptionSender
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;

    public class ExceptionSenderMiddleware
    {
        private readonly RequestDelegate nextMiddleware;
        private readonly IExceptionAccumulator exceptionAccumulator;

        public ExceptionSenderMiddleware(RequestDelegate next, IExceptionAccumulator exceptionAccumulator)
        {
            nextMiddleware = next;
            this.exceptionAccumulator = exceptionAccumulator;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await nextMiddleware(context).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                await exceptionAccumulator.SaveExceptionAsync(ex).ConfigureAwait(false);
                throw;
            }
        }
    }
}
