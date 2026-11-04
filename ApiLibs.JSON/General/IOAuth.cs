using System;

namespace ApiLibs.JSON
{
    public abstract class IOAuth
    {
        public abstract string RedirectUrl { get; }

        public void ActivateOAuth(string url)
        {
            ActivateOAuth(new Uri(url));
        }

        public abstract void ActivateOAuth(Uri url);
        public abstract void ActivateOAuth(Uri url, string redirectUrl);

    }
}
