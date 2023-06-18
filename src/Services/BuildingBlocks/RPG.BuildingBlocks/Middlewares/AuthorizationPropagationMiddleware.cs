using Microsoft.AspNetCore.Http;

namespace RPG.BuildingBlocks.Middlewares
{
    public class AuthorizationPropagationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthorizationPropagationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string authorizationHeader = context.Request.Headers["Authorization"];

            // Verifica si existe un token de autorización en el encabezado
            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                // Verifica si la clave "Authorization" ya existe en el diccionario de encabezados
                if (!context.Request.Headers.ContainsKey("Authorization"))
                {
                    // Agrega el token de autorización al encabezado de la llamada saliente
                    context.Request.Headers.Add("Authorization", authorizationHeader);
                }
            }

            await _next(context);
        }
        
    }
}
