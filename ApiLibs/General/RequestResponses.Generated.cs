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
        public SwitchingProtocolsResponse() : base(HttpStatusCode.Continue) { }
        public SwitchingProtocolsResponse(RequestResponse resp) : base(resp) { }
    }

    public class OKResponse : RequestResponse
    {
        public OKResponse() : base(HttpStatusCode.Continue) { }
        public OKResponse(RequestResponse resp) : base(resp) { }
    }

    public class CreatedResponse : RequestResponse
    {
        public CreatedResponse() : base(HttpStatusCode.Continue) { }
        public CreatedResponse(RequestResponse resp) : base(resp) { }
    }

    public class AcceptedResponse : RequestResponse
    {
        public AcceptedResponse() : base(HttpStatusCode.Continue) { }
        public AcceptedResponse(RequestResponse resp) : base(resp) { }
    }

    public class NonAuthoritativeInformationResponse : RequestResponse
    {
        public NonAuthoritativeInformationResponse() : base(HttpStatusCode.Continue) { }
        public NonAuthoritativeInformationResponse(RequestResponse resp) : base(resp) { }
    }

    public class NoContentResponse : RequestResponse
    {
        public NoContentResponse() : base(HttpStatusCode.Continue) { }
        public NoContentResponse(RequestResponse resp) : base(resp) { }
    }

    public class ResetContentResponse : RequestResponse
    {
        public ResetContentResponse() : base(HttpStatusCode.Continue) { }
        public ResetContentResponse(RequestResponse resp) : base(resp) { }
    }

    public class PartialContentResponse : RequestResponse
    {
        public PartialContentResponse() : base(HttpStatusCode.Continue) { }
        public PartialContentResponse(RequestResponse resp) : base(resp) { }
    }

    public class AmbiguousResponse : RequestResponse
    {
        public AmbiguousResponse() : base(HttpStatusCode.Continue) { }
        public AmbiguousResponse(RequestResponse resp) : base(resp) { }
    }

    public class MultipleChoicesResponse : RequestResponse
    {
        public MultipleChoicesResponse() : base(HttpStatusCode.Continue) { }
        public MultipleChoicesResponse(RequestResponse resp) : base(resp) { }
    }

    public class MovedResponse : RequestResponse
    {
        public MovedResponse() : base(HttpStatusCode.Continue) { }
        public MovedResponse(RequestResponse resp) : base(resp) { }
    }

    public class MovedPermanentlyResponse : RequestResponse
    {
        public MovedPermanentlyResponse() : base(HttpStatusCode.Continue) { }
        public MovedPermanentlyResponse(RequestResponse resp) : base(resp) { }
    }

    public class FoundResponse : RequestResponse
    {
        public FoundResponse() : base(HttpStatusCode.Continue) { }
        public FoundResponse(RequestResponse resp) : base(resp) { }
    }

    public class RedirectResponse : RequestResponse
    {
        public RedirectResponse() : base(HttpStatusCode.Continue) { }
        public RedirectResponse(RequestResponse resp) : base(resp) { }
    }

    public class RedirectMethodResponse : RequestResponse
    {
        public RedirectMethodResponse() : base(HttpStatusCode.Continue) { }
        public RedirectMethodResponse(RequestResponse resp) : base(resp) { }
    }

    public class SeeOtherResponse : RequestResponse
    {
        public SeeOtherResponse() : base(HttpStatusCode.Continue) { }
        public SeeOtherResponse(RequestResponse resp) : base(resp) { }
    }

    public class NotModifiedResponse : RequestResponse
    {
        public NotModifiedResponse() : base(HttpStatusCode.Continue) { }
        public NotModifiedResponse(RequestResponse resp) : base(resp) { }
    }

    public class UseProxyResponse : RequestResponse
    {
        public UseProxyResponse() : base(HttpStatusCode.Continue) { }
        public UseProxyResponse(RequestResponse resp) : base(resp) { }
    }

    public class UnusedResponse : RequestResponse
    {
        public UnusedResponse() : base(HttpStatusCode.Continue) { }
        public UnusedResponse(RequestResponse resp) : base(resp) { }
    }

    public class RedirectKeepVerbResponse : RequestResponse
    {
        public RedirectKeepVerbResponse() : base(HttpStatusCode.Continue) { }
        public RedirectKeepVerbResponse(RequestResponse resp) : base(resp) { }
    }

    public class TemporaryRedirectResponse : RequestResponse
    {
        public TemporaryRedirectResponse() : base(HttpStatusCode.Continue) { }
        public TemporaryRedirectResponse(RequestResponse resp) : base(resp) { }
    }

    public class BadRequestResponse : RequestResponse
    {
        public BadRequestResponse() : base(HttpStatusCode.Continue) { }
        public BadRequestResponse(RequestResponse resp) : base(resp) { }
    }

    public class UnauthorizedResponse : RequestResponse
    {
        public UnauthorizedResponse() : base(HttpStatusCode.Continue) { }
        public UnauthorizedResponse(RequestResponse resp) : base(resp) { }
    }

    public class PaymentRequiredResponse : RequestResponse
    {
        public PaymentRequiredResponse() : base(HttpStatusCode.Continue) { }
        public PaymentRequiredResponse(RequestResponse resp) : base(resp) { }
    }

    public class ForbiddenResponse : RequestResponse
    {
        public ForbiddenResponse() : base(HttpStatusCode.Continue) { }
        public ForbiddenResponse(RequestResponse resp) : base(resp) { }
    }

    public class NotFoundResponse : RequestResponse
    {
        public NotFoundResponse() : base(HttpStatusCode.Continue) { }
        public NotFoundResponse(RequestResponse resp) : base(resp) { }
    }

    public class MethodNotAllowedResponse : RequestResponse
    {
        public MethodNotAllowedResponse() : base(HttpStatusCode.Continue) { }
        public MethodNotAllowedResponse(RequestResponse resp) : base(resp) { }
    }

    public class NotAcceptableResponse : RequestResponse
    {
        public NotAcceptableResponse() : base(HttpStatusCode.Continue) { }
        public NotAcceptableResponse(RequestResponse resp) : base(resp) { }
    }

    public class ProxyAuthenticationRequiredResponse : RequestResponse
    {
        public ProxyAuthenticationRequiredResponse() : base(HttpStatusCode.Continue) { }
        public ProxyAuthenticationRequiredResponse(RequestResponse resp) : base(resp) { }
    }

    public class RequestTimeoutResponse : RequestResponse
    {
        public RequestTimeoutResponse() : base(HttpStatusCode.Continue) { }
        public RequestTimeoutResponse(RequestResponse resp) : base(resp) { }
    }

    public class ConflictResponse : RequestResponse
    {
        public ConflictResponse() : base(HttpStatusCode.Continue) { }
        public ConflictResponse(RequestResponse resp) : base(resp) { }
    }

    public class GoneResponse : RequestResponse
    {
        public GoneResponse() : base(HttpStatusCode.Continue) { }
        public GoneResponse(RequestResponse resp) : base(resp) { }
    }

    public class LengthRequiredResponse : RequestResponse
    {
        public LengthRequiredResponse() : base(HttpStatusCode.Continue) { }
        public LengthRequiredResponse(RequestResponse resp) : base(resp) { }
    }

    public class PreconditionFailedResponse : RequestResponse
    {
        public PreconditionFailedResponse() : base(HttpStatusCode.Continue) { }
        public PreconditionFailedResponse(RequestResponse resp) : base(resp) { }
    }

    public class RequestEntityTooLargeResponse : RequestResponse
    {
        public RequestEntityTooLargeResponse() : base(HttpStatusCode.Continue) { }
        public RequestEntityTooLargeResponse(RequestResponse resp) : base(resp) { }
    }

    public class RequestUriTooLongResponse : RequestResponse
    {
        public RequestUriTooLongResponse() : base(HttpStatusCode.Continue) { }
        public RequestUriTooLongResponse(RequestResponse resp) : base(resp) { }
    }

    public class UnsupportedMediaTypeResponse : RequestResponse
    {
        public UnsupportedMediaTypeResponse() : base(HttpStatusCode.Continue) { }
        public UnsupportedMediaTypeResponse(RequestResponse resp) : base(resp) { }
    }

    public class RequestedRangeNotSatisfiableResponse : RequestResponse
    {
        public RequestedRangeNotSatisfiableResponse() : base(HttpStatusCode.Continue) { }
        public RequestedRangeNotSatisfiableResponse(RequestResponse resp) : base(resp) { }
    }

    public class ExpectationFailedResponse : RequestResponse
    {
        public ExpectationFailedResponse() : base(HttpStatusCode.Continue) { }
        public ExpectationFailedResponse(RequestResponse resp) : base(resp) { }
    }

    public class UpgradeRequiredResponse : RequestResponse
    {
        public UpgradeRequiredResponse() : base(HttpStatusCode.Continue) { }
        public UpgradeRequiredResponse(RequestResponse resp) : base(resp) { }
    }

    public class InternalServerErrorResponse : RequestResponse
    {
        public InternalServerErrorResponse() : base(HttpStatusCode.Continue) { }
        public InternalServerErrorResponse(RequestResponse resp) : base(resp) { }
    }

    public class NotImplementedResponse : RequestResponse
    {
        public NotImplementedResponse() : base(HttpStatusCode.Continue) { }
        public NotImplementedResponse(RequestResponse resp) : base(resp) { }
    }

    public class BadGatewayResponse : RequestResponse
    {
        public BadGatewayResponse() : base(HttpStatusCode.Continue) { }
        public BadGatewayResponse(RequestResponse resp) : base(resp) { }
    }

    public class ServiceUnavailableResponse : RequestResponse
    {
        public ServiceUnavailableResponse() : base(HttpStatusCode.Continue) { }
        public ServiceUnavailableResponse(RequestResponse resp) : base(resp) { }
    }

    public class GatewayTimeoutResponse : RequestResponse
    {
        public GatewayTimeoutResponse() : base(HttpStatusCode.Continue) { }
        public GatewayTimeoutResponse(RequestResponse resp) : base(resp) { }
    }

    public class HttpVersionNotSupportedResponse : RequestResponse
    {
        public HttpVersionNotSupportedResponse() : base(HttpStatusCode.Continue) { }
        public HttpVersionNotSupportedResponse(RequestResponse resp) : base(resp) { }
    }

}