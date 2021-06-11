namespace WebApi.Helpers
{   
    ///<summary>
    ///Class containing the Secret key variable.
    ///</summary>
    public class AppSettings
    {   
        ///<summary>
        ///The secret key used to generate the Bearer Tokens for Authorization.
        ///</summary>
        public string Secret { get; set; }
    }
}