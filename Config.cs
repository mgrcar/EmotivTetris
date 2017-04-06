using System.Configuration;

namespace EmotivTetris
{
    public static class Config
    {
        public static readonly string ProfileFileName
            = ConfigurationManager.AppSettings["ProfileFileName"];
        public static readonly string GameUrl
            = ConfigurationManager.AppSettings["GameUrl"];
    }
}
