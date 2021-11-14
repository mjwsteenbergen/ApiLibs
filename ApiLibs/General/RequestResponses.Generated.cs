using System.Net;

namespace ApiLibs
{
    public class ContinueResponse : RequestResponse
    {
        public ContinueResponse() : base(HttpStatusCode.Continue) { }
        public ContinueResponse(RequestResponse resp) : base(resp) { }
    }

    public class SwitchingProtocolsResponse : RequestResponse
    {
        public SwitchingProtocolsResponse() : base(HttpStatusCode.SwitchingProtocols) { }
        public SwitchingProtocolsResponse(RequestResponse resp) : base(resp) { }
    }

    public class OKResponse : RequestResponse
    {
        public OKResponse() : base(HttpStatusCode.OK) { }
        public OKResponse(RequestResponse resp) : base(resp) { }
    }

    public class CreatedResponse : RequestResponse
    {
        public CreatedResponse() : base(HttpStatusCode.Created) { }
        public CreatedResponse(RequestResponse resp) : base(resp) { }
    }

    public class AcceptedResponse : RequestResponse
    {
        public AcceptedResponse() : base(HttpStatusCode.Accepted) { }
        public AcceptedResponse(RequestResponse resp) : base(resp) { }
    }

    public class NonAuthoritativeInformationResponse : RequestResponse
    {
        public NonAuthoritativeInformationResponse() : base(HttpStatusCode.NonAuthoritativeInformation) { }
        public NonAuthoritativeInformationResponse(RequestResponse resp) : base(resp) { }
    }

    public class NoContentResponse : RequestResponse
    {
        public NoContentResponse() : base(HttpStatusCode.NoContent) { }
        public NoContentResponse(RequestResponse resp) : base(resp) { }
    }

    public class ResetContentResponse : RequestResponse
    {
        public ResetContentResponse() : base(HttpStatusCode.ResetContent) { }
        public ResetContentResponse(RequestResponse resp) : base(resp) { }
    }

    public class PartialContentResponse : RequestResponse
    {
        public PartialContentResponse() : base(HttpStatusCode.PartialContent) { }
        public PartialContentResponse(RequestResponse resp) : base(resp) { }
    }

    public class AmbiguousResponse : RequestResponse
    {
        public AmbiguousResponse() : base(HttpStatusCode.Ambiguous) { }
        public AmbiguousResponse(RequestResponse resp) : base(resp) { }
    }

    public class MultipleChoicesResponse : RequestResponse
    {
        public MultipleChoicesResponse() : base(HttpStatusCode.MultipleChoices) { }
        public MultipleChoicesResponse(RequestResponse resp) : base(resp) { }
    }

    public class MovedResponse : RequestResponse
    {
        public MovedResponse() : base(HttpStatusCode.Moved) { }
        public MovedResponse(RequestResponse resp) : base(resp) { }
    }

    public class MovedPermanentlyResponse : RequestResponse
    {
        public MovedPermanentlyResponse() : base(HttpStatusCode.MovedPermanently) { }
        public MovedPermanentlyResponse(RequestResponse resp) : base(resp) { }
    }

    public class FoundResponse : RequestResponse
    {
        public FoundResponse() : base(HttpStatusCode.Found) { }
        public FoundResponse(RequestResponse resp) : base(resp) { }
    }

    public class RedirectResponse : RequestResponse
    {
        public RedirectResponse() : base(HttpStatusCode.Redirect) { }
        public RedirectResponse(RequestResponse resp) : base(resp) { }
    }

    public class RedirectMethodResponse : RequestResponse
    {
        public RedirectMethodResponse() : base(HttpStatusCode.RedirectMethod) { }
        public RedirectMethodResponse(RequestResponse resp) : base(resp) { }
    }

    public class SeeOtherResponse : RequestResponse
    {
        public SeeOtherResponse() : base(HttpStatusCode.SeeOther) { }
        public SeeOtherResponse(RequestResponse resp) : base(resp) { }
    }

    public class NotModifiedResponse : RequestResponse
    {
        public NotModifiedResponse() : base(HttpStatusCode.NotModified) { }
        public NotModifiedResponse(RequestResponse resp) : base(resp) { }
    }

    public class UseProxyResponse : RequestResponse
    {
        public UseProxyResponse() : base(HttpStatusCode.UseProxy) { }
        public UseProxyResponse(RequestResponse resp) : base(resp) { }
    }

    public class UnusedResponse : RequestResponse
    {
        public UnusedResponse() : base(HttpStatusCode.Unused) { }
        public UnusedResponse(RequestResponse resp) : base(resp) { }
    }

    public class RedirectKeepVerbResponse : RequestResponse
    {
        public RedirectKeepVerbResponse() : base(HttpStatusCode.RedirectKeepVerb) { }
        public RedirectKeepVerbResponse(RequestResponse resp) : base(resp) { }
    }

    public class TemporaryRedirectResponse : RequestResponse
    {
        public TemporaryRedirectResponse() : base(HttpStatusCode.TemporaryRedirect) { }
        public TemporaryRedirectResponse(RequestResponse resp) : base(resp) { }
    }

    public class BadRequestResponse : RequestResponse
    {
        public BadRequestResponse() : base(HttpStatusCode.BadRequest) { }
        public BadRequestResponse(RequestResponse resp) : base(resp) { }
    }

    public class UnauthorizedResponse : RequestResponse
    {
        public UnauthorizedResponse() : base(HttpStatusCode.Unauthorized) { }
        public UnauthorizedResponse(RequestResponse resp) : base(resp) { }
    }

    public class PaymentRequiredResponse : RequestResponse
    {
        public PaymentRequiredResponse() : base(HttpStatusCode.PaymentRequired) { }
        public PaymentRequiredResponse(RequestResponse resp) : base(resp) { }
    }

    public class ForbiddenResponse : RequestResponse
    {
        public ForbiddenResponse() : base(HttpStatusCode.Forbidden) { }
        public ForbiddenResponse(RequestResponse resp) : base(resp) { }
    }

    public class NotFoundResponse : RequestResponse
    {
        public NotFoundResponse() : base(HttpStatusCode.NotFound) { }
        public NotFoundResponse(RequestResponse resp) : base(resp) { }
    }

    public class MethodNotAllowedResponse : RequestResponse
    {
        public MethodNotAllowedResponse() : base(HttpStatusCode.MethodNotAllowed) { }
        public MethodNotAllowedResponse(RequestResponse resp) : base(resp) { }
    }

    public class NotAcceptableResponse : RequestResponse
    {
        public NotAcceptableResponse() : base(HttpStatusCode.NotAcceptable) { }
        public NotAcceptableResponse(RequestResponse resp) : base(resp) { }
    }

    public class ProxyAuthenticationRequiredResponse : RequestResponse
    {
        public ProxyAuthenticationRequiredResponse() : base(HttpStatusCode.ProxyAuthenticationRequired) { }
        public ProxyAuthenticationRequiredResponse(RequestResponse resp) : base(resp) { }
    }

    public class RequestTimeoutResponse : RequestResponse
    {
        public RequestTimeoutResponse() : base(HttpStatusCode.RequestTimeout) { }
        public RequestTimeoutResponse(RequestResponse resp) : base(resp) { }
    }

    public class ConflictResponse : RequestResponse
    {
        public ConflictResponse() : base(HttpStatusCode.Conflict) { }
        public ConflictResponse(RequestResponse resp) : base(resp) { }
    }

    public class GoneResponse : RequestResponse
    {
        public GoneResponse() : base(HttpStatusCode.Gone) { }
        public GoneResponse(RequestResponse resp) : base(resp) { }
    }

    public class LengthRequiredResponse : RequestResponse
    {
        public LengthRequiredResponse() : base(HttpStatusCode.LengthRequired) { }
        public LengthRequiredResponse(RequestResponse resp) : base(resp) { }
    }

    public class PreconditionFailedResponse : RequestResponse
    {
        public PreconditionFailedResponse() : base(HttpStatusCode.PreconditionFailed) { }
        public PreconditionFailedResponse(RequestResponse resp) : base(resp) { }
    }

    public class RequestEntityTooLargeResponse : RequestResponse
    {
        public RequestEntityTooLargeResponse() : base(HttpStatusCode.RequestEntityTooLarge) { }
        public RequestEntityTooLargeResponse(RequestResponse resp) : base(resp) { }
    }

    public class RequestUriTooLongResponse : RequestResponse
    {
        public RequestUriTooLongResponse() : base(HttpStatusCode.RequestUriTooLong) { }
        public RequestUriTooLongResponse(RequestResponse resp) : base(resp) { }
    }

    public class UnsupportedMediaTypeResponse : RequestResponse
    {
        public UnsupportedMediaTypeResponse() : base(HttpStatusCode.UnsupportedMediaType) { }
        public UnsupportedMediaTypeResponse(RequestResponse resp) : base(resp) { }
    }

    public class RequestedRangeNotSatisfiableResponse : RequestResponse
    {
        public RequestedRangeNotSatisfiableResponse() : base(HttpStatusCode.RequestedRangeNotSatisfiable) { }
        public RequestedRangeNotSatisfiableResponse(RequestResponse resp) : base(resp) { }
    }

    public class ExpectationFailedResponse : RequestResponse
    {
        public ExpectationFailedResponse() : base(HttpStatusCode.ExpectationFailed) { }
        public ExpectationFailedResponse(RequestResponse resp) : base(resp) { }
    }

    public class UpgradeRequiredResponse : RequestResponse
    {
        public UpgradeRequiredResponse() : base(HttpStatusCode.UpgradeRequired) { }
        public UpgradeRequiredResponse(RequestResponse resp) : base(resp) { }
    }

    public class InternalServerErrorResponse : RequestResponse
    {
        public InternalServerErrorResponse() : base(HttpStatusCode.InternalServerError) { }
        public InternalServerErrorResponse(RequestResponse resp) : base(resp) { }
    }

    public class NotImplementedResponse : RequestResponse
    {
        public NotImplementedResponse() : base(HttpStatusCode.NotImplemented) { }
        public NotImplementedResponse(RequestResponse resp) : base(resp) { }
    }

    public class BadGatewayResponse : RequestResponse
    {
        public BadGatewayResponse() : base(HttpStatusCode.BadGateway) { }
        public BadGatewayResponse(RequestResponse resp) : base(resp) { }
    }

    public class ServiceUnavailableResponse : RequestResponse
    {
        public ServiceUnavailableResponse() : base(HttpStatusCode.ServiceUnavailable) { }
        public ServiceUnavailableResponse(RequestResponse resp) : base(resp) { }
    }

    public class GatewayTimeoutResponse : RequestResponse
    {
        public GatewayTimeoutResponse() : base(HttpStatusCode.GatewayTimeout) { }
        public GatewayTimeoutResponse(RequestResponse resp) : base(resp) { }
    }

    public class HttpVersionNotSupportedResponse : RequestResponse
    {
        public HttpVersionNotSupportedResponse() : base(HttpStatusCode.HttpVersionNotSupported) { }
        public HttpVersionNotSupportedResponse(RequestResponse resp) : base(resp) { }
    }

}