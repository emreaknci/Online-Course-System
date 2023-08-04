using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;

namespace OnlineCourse.IdentityServer.Services
{
    public class TokenExchangeExtensionGrantValidator : IExtensionGrantValidator
    {
        private readonly ITokenValidator _tokenValidator;

        public TokenExchangeExtensionGrantValidator(ITokenValidator tokenValidator)
        {
            _tokenValidator = tokenValidator;
        }
        public string GrantType => "urn:ietf:params:oauth:grant-type:token-exchange";

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var token = context.Request.Raw.Get("subject_token");

            if (string.IsNullOrEmpty(token))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, "Token missing");
                return;
            }

            var tokenValidateResult = await _tokenValidator.ValidateAccessTokenAsync(token);
            if (tokenValidateResult.IsError)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Token invalid");
                return;
            }

            var subjectClaim = tokenValidateResult.Claims.FirstOrDefault(c => c.Type == "sub");

            if (subjectClaim == null)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Token must contain sub value");
                return;
            }

            context.Result = new GrantValidationResult(subjectClaim.Value, "access_token");
        }

    }
}
